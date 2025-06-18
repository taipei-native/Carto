using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Carto.Systems
{
    /// <summary>
    /// The system that searches buildings.
    /// （搜尋建築的系統。）
    /// </summary>
    public partial class BuildingSystem : GameSystemBase
    {
        /// <summary>
        /// The exclusion filters in the area entity query.
        /// （區域實體查詢中排除的篩選條件。）
        /// </summary>
        static readonly List<ComponentType> _areaFilters = new()
        {
            ComponentType.ReadOnly<Deleted>(),
            ComponentType.ReadOnly<District>(),
            ComponentType.ReadOnly<MapTile>(),
            ComponentType.ReadOnly<Space>(),
            ComponentType.ReadOnly<Game.Areas.Surface>(),
            ComponentType.ReadOnly<Temp>()
        };

        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The system managing names.（管理名稱的系統。）<br/>
        /// See <see cref="Instance.Name"/> for more information.
        /// </summary>
        static readonly NameSystem _name = Instance.Name;

        /// <summary>
        /// The system managing prefabricated data.（管理預製模板資料的系統。）<br/>
        /// See <see cref="Instance.Prefab"/> for more information.
        /// </summary>
        static readonly PrefabSystem _prefab = Instance.Prefab;

        /// <summary>
        /// The system collecting shared data.（收集共享資料的系統。）<br/>
        /// See <see cref="Instance.Shared"/> for more information.
        /// </summary>
        static readonly SharedDataCollectionSystem _shared = Instance.Shared;

        /// <summary>
        /// The query to collect all building prefabs.
        /// （收集所有建築預製模板的查詢。）
        /// </summary>
        static EntityQuery _buildingPrefabQuery;

        /// <summary>
        /// The query for existing areas.（現有區域的查詢。）
        /// </summary>
        static EntityQueryDesc _areaQueryDesc;

        /// <summary>
        /// The local variant of <see cref="BuildingStat"/> list from the <see cref="SharedDataCollectionSystem"/>.
        /// （本地源自 <see cref="SharedDataCollectionSystem"/> 的 <see cref="BuildingStat"/> 列表變種。）
        /// </summary>
        private NativeList<BuildingStat> _localBuildingStats;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _areaQueryDesc = new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>(),
                    ComponentType.ReadOnly<Owner>()
                }
            };
            
            _buildingPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<BuildingData>(),
                    ComponentType.ReadOnly<PrefabData>()
                }
            });
            
            base.OnCreate();
            _log.Debug("BuildingSystem instance created. 建築系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy()
        {
            Utils.CommonUtils.Dispose(ref _localBuildingStats);
            base.OnDestroy();
        }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Retrieve the building prefabs with circular boundaries.
        /// （獲得環形邊界的建築預製件。）
        /// </summary>
        private void GetCircularBoundaryBuildingPrefabs(ref NativeParallelHashSet<Entity> circularBuildingPrefabs)
        {
            // Validate native containers integrity.（驗證原生容器的完整性。）
            if (circularBuildingPrefabs.Equals(null) || !circularBuildingPrefabs.IsCreated) return;

            // Collect building prefabs.（收集建築預製件。）
            NativeArray<Entity> buildingPrefabEntities = _buildingPrefabQuery.ToEntityArray(Allocator.Temp);
            NativeArray<PrefabData> buildingPrefabData = _buildingPrefabQuery.ToComponentDataArray<PrefabData>(Allocator.Temp);
            for (int i = 0; i < buildingPrefabEntities.Length; i++)
            {
                if (Instance.Prefab.TryGetPrefab(buildingPrefabData[i], out BuildingPrefab buildingPrefab) && buildingPrefab != null)
                {
                    if (buildingPrefab.m_Circular) circularBuildingPrefabs.Add(buildingPrefabEntities[i]);
                }
            }
        }

        /// <summary>
        /// Write boundary attributes to the designated file.
        /// （寫出邊界屬性至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        /// <param name="fieldMap">The map between the property and the fields.（屬性與欄位的映射表。）</param>
        public void WriteBoundaryDBF(BinaryWriter writer, Options options, HashSet<Property> validatedFields, List<Entity> entitySyncList, out Dictionary<Property, FieldInfo> fieldMap)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Building) && validatedFields.Contains(Property.Name);
            bool hasAddress = options.Contains(Property.Address, IO.System.Building) && validatedFields.Contains(Property.Address);
            bool hasAge = options.Contains(Property.Age, IO.System.Building) && validatedFields.Contains(Property.Age);
            bool hasAsset = options.Contains(Property.Asset, IO.System.Building) && validatedFields.Contains(Property.Asset);
            bool hasBrand = options.Contains(Property.Brand, IO.System.Building) && validatedFields.Contains(Property.Brand);
            bool hasCategory = options.Contains(Property.Category, IO.System.Building) && validatedFields.Contains(Property.Category);
            bool hasElevation = options.Contains(Property.Elevation, IO.System.Building) && validatedFields.Contains(Property.Elevation);
            bool hasEmployee = options.Contains(Property.Employee, IO.System.Building) && validatedFields.Contains(Property.Employee);
            bool hasHeight = options.Contains(Property.Height, IO.System.Building) && validatedFields.Contains(Property.Height);
            bool hasHousehold = options.Contains(Property.Household, IO.System.Building) && validatedFields.Contains(Property.Household);
            bool hasLabor = options.Contains(Property.Labor, IO.System.Building) && validatedFields.Contains(Property.Labor);
            bool hasLevel = options.Contains(Property.Level, IO.System.Building) && validatedFields.Contains(Property.Level);
            bool hasObject = options.Contains(Property.Object, IO.System.Building) && validatedFields.Contains(Property.Object);
            bool hasProduct = options.Contains(Property.Product, IO.System.Building) && validatedFields.Contains(Property.Product);
            bool hasProfit = options.Contains(Property.Profit, IO.System.Building) && validatedFields.Contains(Property.Profit);
            bool hasResident = options.Contains(Property.Resident, IO.System.Building) && validatedFields.Contains(Property.Resident);
            bool hasSexRatio = options.Contains(Property.SexRatio, IO.System.Building) && validatedFields.Contains(Property.SexRatio);
            bool hasStory = options.Contains(Property.Story, IO.System.Building) && validatedFields.Contains(Property.Story);
            bool hasTheme = options.Contains(Property.Theme, IO.System.Building) && validatedFields.Contains(Property.Theme);
            bool hasValue = options.Contains(Property.Value, IO.System.Building) && validatedFields.Contains(Property.Value);
            bool hasWage = options.Contains(Property.Wage, IO.System.Building) && validatedFields.Contains(Property.Wage);
            bool hasZone = options.Contains(Property.Zone, IO.System.Building) && validatedFields.Contains(Property.Zone);

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<ZoningType> zoningTypes = ref _shared.ZoningTypes;
            ref NativeList<NativeText> zoningTypesNames = ref _shared.ZoningTypesNames;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref _localBuildingStats, true);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypes, true);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypesNames, true);

            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, int> syncMap = new(_localBuildingStats.Length, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            Dictionary<Resource, string> resourcesMap = _shared.ResourceNameMap;
            List<Brand> brands = _shared.Brands;
            List<LiteralAddress> buildingAddresses = new();
            List<string> buildingAssets = new();
            List<string> buildingNames = new();
            List<Theme> themes = _shared.Themes;
            List<ZoningType> zoningTypesManaged = Utils.CommonUtils.Copy(ref zoningTypes);
            string[] zoningTypesNamesManaged = Utils.CommonUtils.Copy(ref zoningTypesNames);
            Dictionary<Property, FieldInfo> _fieldMap = new();

            try
            {
                FieldInfo addressField = new(0, 0, false, FieldType.String);
                FieldInfo ageField = new(0f);
                FieldInfo assetField = new(0, 0, false, FieldType.String);
                FieldInfo brandField = new(0, 0, false, FieldType.String);
                FieldInfo categoryField = new(0, 0, false, FieldType.String);
                FieldInfo elevationField = new(0f);
                FieldInfo employeeField = new(0);
                //FieldInfo heightField = new(0f); // TODO: Not available in 1.0.0 release.
                FieldInfo householdField = new(0);
                FieldInfo laborField = new(0);
                FieldInfo levelField = new(0, 1, false, FieldType.Int);
                FieldInfo nameField = new(0, 0, false, FieldType.String);
                FieldInfo objectField = new(0, 0, false, FieldType.String);
                FieldInfo productField = new(0, 0, false, FieldType.String);
                FieldInfo profitField = new(0f);
                FieldInfo residentField = new(0);
                FieldInfo sexRatioField = new(0f);
                //FieldInfo storyField = new(0); // TODO: Not available in 1.0.0 release.
                FieldInfo themeField = new(0, 0, false, FieldType.String);
                //FieldInfo valueField = new(0f); // TODO: Not available in 1.0.0 release.
                FieldInfo wageField = new(0f);
                FieldInfo zoneField = new(0, 0, false, FieldType.String);

                for (int i = 0; i < _localBuildingStats.Length; i++)
                {
                    BuildingStat stat = _localBuildingStats[i];
                    bool hasValidZoningType = (stat.zoning >= 0) && (stat.zoning < zoningTypesManaged.Count);
                    ZoningType zoningType = hasValidZoningType ? zoningTypesManaged[stat.zoning] : zoningTypesManaged[^1];

                    if (hasName)
                    {
                        buildingNames.Add(_name.GetRenderedLabelName(stat.entity));
                        nameField += new FieldInfo(buildingNames[i]);
                    }
                    if (hasAddress)
                    {
                        LiteralAddress address = stat.address.ToLiteral(_name);
                        buildingAddresses.Add(address);
                        addressField += new FieldInfo(address.district);
                        addressField += new FieldInfo(address.street);
                    }
                    if (hasAge)
                    {
                        ageField += new FieldInfo(stat.GetAverageAge());
                    }
                    if (hasAsset)
                    {
                        string prefabName = _prefab.GetPrefabName(stat.prefab);
                        buildingAssets.Add(LocaleUtils.TryTranslate($"Assets.NAME[{prefabName}]", out string assetName) ? assetName : prefabName);
                        assetField += new FieldInfo(buildingAssets[i]);
                    }
                    if (hasBrand)
                    {
                        string brandName = (stat.brand >= 0) && (stat.brand < brands.Count) ? brands[stat.brand].name : string.Empty;
                        brandField += new FieldInfo(brandName);
                    }
                    if (hasCategory)
                    {
                        BuildingCategory category = options.Display[(Property.Category, IO.System.Building)] ? stat.category : Utils.CommonUtils.GetFirstMatch(stat.category, IO.IO.BuildingCategoryDisplayOrder);
                        categoryField += new FieldInfo(category.ToString("G"));
                    }
                    if (hasElevation)
                    {
                        elevationField += new FieldInfo(stat.elevation);
                    }
                    if (hasEmployee)
                    {
                        employeeField += new FieldInfo(stat.employee);
                    }
                    //if (hasHeight) // TODO: Not available in 1.0.0 release.
                    if (hasHousehold)
                    {
                        householdField += new FieldInfo(stat.household);
                    }
                    if (hasLabor)
                    {
                        laborField += new FieldInfo(stat.labor);
                    }
                    // Level: The value lies between 1 and 5, so no need to aggregate the FieldInfo.（等級總是介於 1 至 5，因此無須相加欄位資訊。）
                    if (hasObject)
                    {
                        Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? stat.objectType : Utils.CommonUtils.GetFirstMatch(stat.objectType, IO.IO.FeatureDisplayOrder);
                        objectField += new FieldInfo(displayType.ToString("G"));
                    }
                    if (hasProduct)
                    {
                        productField += new FieldInfo(stat.GetLocalizedProducts(resourcesMap));
                    }
                    if (hasProfit)
                    {
                        profitField += new FieldInfo(stat.GetAverageProfit());
                    }
                    if (hasResident)
                    {
                        residentField += new FieldInfo(stat.residentFemale + stat.residentMale);
                    }
                    if (hasSexRatio)
                    {
                        sexRatioField += new FieldInfo(stat.GetSexRatio());
                    }
                    //if (hasStory) // TODO: Not available in 1.0.0 release.
                    if (hasTheme)
                    {
                        string themeName = (zoningType.theme >= 0) && (zoningType.theme < themes.Count) ? themes[zoningType.theme].name : string.Empty;
                        themeField += new FieldInfo(themeName);
                    }
                    //if (hasValue) // TODO: Not available in 1.0.0 release.
                    if (hasWage)
                    {
                        wageField += new FieldInfo(stat.GetAverageWage());
                    }
                    if (hasZone)
                    {
                        string zoningTypeName = hasValidZoningType ? zoningTypesNamesManaged[stat.zoning] : zoningTypesNamesManaged[^1];
                        zoneField += new FieldInfo(zoningTypeName);
                    }
                }

                if (hasName)
                {
                    _fieldMap.Add(Property.Name, nameField);
                }
                if (hasAddress)
                {
                    _fieldMap.Add(Property.Address, addressField);
                }
                if (hasAge)
                {
                    ageField = Shapefile.ApplyFieldInfoConstraint(Property.Age, ageField);
                    _fieldMap.Add(Property.Age, ageField);
                }
                if (hasAsset)
                {
                    _fieldMap.Add(Property.Asset, assetField);
                }
                if (hasBrand)
                {
                    _fieldMap.Add(Property.Brand, brandField);
                }
                if (hasCategory)
                {
                    _fieldMap.Add(Property.Category, categoryField);
                }
                if (hasElevation)
                {
                    elevationField = Shapefile.ApplyFieldInfoConstraint(Property.Elevation, elevationField);
                    _fieldMap.Add(Property.Elevation, elevationField);
                }
                if (hasEmployee)
                {
                    employeeField = Shapefile.ApplyFieldInfoConstraint(Property.Employee, employeeField);
                    _fieldMap.Add(Property.Employee, employeeField);
                }
                if (hasHousehold)
                {
                    householdField = Shapefile.ApplyFieldInfoConstraint(Property.Household, householdField);
                    _fieldMap.Add(Property.Household, householdField);
                }
                if (hasLabor)
                {
                    laborField = Shapefile.ApplyFieldInfoConstraint(Property.Labor, laborField);
                    _fieldMap.Add(Property.Labor, laborField);
                }
                if (hasLevel)
                {
                    _fieldMap.Add(Property.Level, levelField);
                }
                if (hasObject)
                {
                    _fieldMap.Add(Property.Object, objectField);
                }
                if (hasProduct)
                {
                    _fieldMap.Add(Property.Product, productField);
                }
                if (hasProfit)
                {
                    profitField = Shapefile.ApplyFieldInfoConstraint(Property.Profit, profitField);
                    _fieldMap.Add(Property.Profit, profitField);
                }
                if (hasResident)
                {
                    residentField = Shapefile.ApplyFieldInfoConstraint(Property.Resident, residentField);
                    _fieldMap.Add(Property.Resident, residentField);
                }
                if (hasSexRatio)
                {
                    sexRatioField = Shapefile.ApplyFieldInfoConstraint(Property.SexRatio, sexRatioField);
                    _fieldMap.Add(Property.SexRatio, sexRatioField);
                }
                if (hasTheme)
                {
                    _fieldMap.Add(Property.Theme, themeField);
                }
                if (hasWage)
                {
                    wageField = Shapefile.ApplyFieldInfoConstraint(Property.Wage, wageField);
                    _fieldMap.Add(Property.Wage, wageField);
                }
                if (hasZone)
                {
                    _fieldMap.Add(Property.Zone, zoneField);
                }

                // Sync the entity order with that of the .shp file.（與 .shp 檔案的實體順序同步。）
                Shapefile.SyncStatsToIndex(entitySyncList, ref _localBuildingStats, ref syncMap);

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    for (int index = 0; index < entitySyncList.Count; index++)
                    {
                        if (!syncMap.TryGetValue(entitySyncList[index], out int i))
                        {
                            _log.Error($"Couldn't find the statistical object of {entitySyncList[index]} at index {index}. 無法找到位於索引值 {index} 的實體 {entitySyncList[index]} 之統計物件。");
                        }
                        
                        BuildingStat stat = _localBuildingStats[i];
                        bool hasValidZoningType = (stat.zoning >= 0) && (stat.zoning < zoningTypesManaged.Count);
                        ZoningType zoningType = hasValidZoningType ? zoningTypesManaged[stat.zoning] : zoningTypesManaged[^1];
                        writer.Write((byte)32);

                        if (hasName)
                        {
                            Shapefile.WriteRecord(writer, nameField, buildingNames[i]);
                        }
                        if (hasAddress)
                        {
                            LiteralAddress address = buildingAddresses[i];
                            Shapefile.WriteRecord(writer, addressField, address.district);
                            Shapefile.WriteRecord(writer, addressField, address.street);
                            Shapefile.WriteRecord(writer, new(100000), address.number);
                        }
                        if (hasAge)
                        {
                            Shapefile.WriteRecord(writer, ageField, stat.GetAverageAge());
                        }
                        if (hasAsset)
                        {
                            Shapefile.WriteRecord(writer, assetField, buildingAssets[i]);
                        }
                        if (hasBrand)
                        {
                            string brandName = (stat.brand >= 0) && (stat.brand < brands.Count) ? brands[stat.brand].name : string.Empty;
                            Shapefile.WriteRecord(writer, brandField, brandName);
                        }
                        if (hasCategory)
                        {
                            BuildingCategory category = options.Display[(Property.Category, IO.System.Building)] ? stat.category : Utils.CommonUtils.GetFirstMatch(stat.category, IO.IO.BuildingCategoryDisplayOrder);
                            Shapefile.WriteRecord(writer, categoryField, category.ToString("G"));
                        }
                        if (hasElevation)
                        {
                            Shapefile.WriteRecord(writer, elevationField, stat.elevation);
                        }
                        if (hasEmployee)
                        {
                            Shapefile.WriteRecord(writer, employeeField, stat.employee);
                        }
                        //if (hasHeight)
                        //{
                            // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasHousehold)
                        {
                            Shapefile.WriteRecord(writer, householdField, stat.household);
                        }
                        if (hasLabor)
                        {
                            Shapefile.WriteRecord(writer, laborField, stat.labor);
                        }
                        if (hasLevel)
                        {
                            Shapefile.WriteRecord(writer, levelField, stat.level);
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? stat.objectType : Utils.CommonUtils.GetFirstMatch(stat.objectType, IO.IO.FeatureDisplayOrder);
                            Shapefile.WriteRecord(writer, objectField, displayType.ToString("G"));
                        }
                        if (hasProduct)
                        {
                            Shapefile.WriteRecord(writer, productField, stat.GetLocalizedProducts(resourcesMap));
                        }
                        if (hasProfit)
                        {
                            Shapefile.WriteRecord(writer, profitField, stat.GetAverageProfit());
                        }
                        if (hasResident)
                        {
                            if (options.SeparateResident)
                            {
                                Shapefile.WriteRecord(writer, residentField, stat.residentFemale);
                                Shapefile.WriteRecord(writer, residentField, stat.residentMale);
                            }
                            else
                            {
                                Shapefile.WriteRecord(writer, residentField, stat.residentFemale + stat.residentMale);
                            }
                        }   
                        if (hasSexRatio)
                        {
                            Shapefile.WriteRecord(writer, sexRatioField, stat.GetSexRatio());
                        }
                        //if (hasStory)
                        //{
                            // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasTheme)
                        {
                            string themeName = (zoningType.theme >= 0) && (zoningType.theme < themes.Count) ? themes[zoningType.theme].name : string.Empty;
                            Shapefile.WriteRecord(writer, themeField, themeName);
                        }
                        //if (hasValue)
                        //{
                            // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasWage)
                        {
                            Shapefile.WriteRecord(writer, wageField, stat.GetAverageWage());
                        }
                        if (hasZone)
                        {
                            // The fallback value is "Unzoned".（後備值是「無分區」。）
                            string zoningTypeName = hasValidZoningType ? zoningTypesNamesManaged[stat.zoning] : zoningTypesNamesManaged[^1];
                            Shapefile.WriteRecord(writer, zoneField, zoningTypeName);
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                fieldMap = _fieldMap;
                Utils.CommonUtils.Dispose(ref _localBuildingStats);
                Utils.CommonUtils.Dispose(ref syncMap);
                _shared.Dispose(DisposePhase.AfterBuildingSystem);
            }
        }

        /// <summary>
        /// Write boundary features (geometries and properties) to the designated file.
        /// （寫出邊界圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public void WriteBoundaryFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            Feature featureFlag = options.Features;
            bool useBuilding = featureFlag.HasFlag(Feature.Building);
            bool useExtractor = featureFlag.HasFlag(Feature.Extractor);
            bool useLandfill = featureFlag.HasFlag(Feature.Landfill);

            // Build area feature queries.（建立區域圖徵查詢。）
            List<ComponentType> areaFilters = new(_areaFilters);
            EntityQueryDesc areaQueryDesc = new()
            {
                All = _areaQueryDesc.All
            };
            if (!useExtractor) areaFilters.Add(ComponentType.ReadOnly<Extractor>());
            if (!useLandfill) areaFilters.Add(ComponentType.ReadOnly<Storage>());
            areaQueryDesc.None = areaFilters.ToArray();
            EntityQuery areaQuery = GetEntityQuery(areaQueryDesc);

            bool hasName = options.Contains(Property.Name, IO.System.Building);
            bool hasAddress = options.Contains(Property.Address, IO.System.Building);
            bool hasAge = options.Contains(Property.Age, IO.System.Building);
            bool hasAsset = options.Contains(Property.Asset, IO.System.Building);
            bool hasBrand = options.Contains(Property.Brand, IO.System.Building);
            bool hasCategory = options.Contains(Property.Category, IO.System.Building);
            bool hasElevation = options.Contains(Property.Elevation, IO.System.Building);
            bool hasEmployee = options.Contains(Property.Employee, IO.System.Building);
            bool hasHeight = options.Contains(Property.Height, IO.System.Building);
            bool hasHousehold = options.Contains(Property.Household, IO.System.Building);
            bool hasLabor = options.Contains(Property.Labor, IO.System.Building);
            bool hasLevel = options.Contains(Property.Level, IO.System.Building);
            bool hasObject = options.Contains(Property.Object, IO.System.Building);
            bool hasProduct = options.Contains(Property.Product, IO.System.Building);
            bool hasProfit = options.Contains(Property.Profit, IO.System.Building);
            bool hasResident = options.Contains(Property.Resident, IO.System.Building);
            bool hasSexRatio = options.Contains(Property.SexRatio, IO.System.Building);
            bool hasStory = options.Contains(Property.Story, IO.System.Building);
            bool hasTheme = options.Contains(Property.Theme, IO.System.Building);
            bool hasValue = options.Contains(Property.Value, IO.System.Building);
            bool hasWage = options.Contains(Property.Wage, IO.System.Building);
            bool hasZone = options.Contains(Property.Zone, IO.System.Building);

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<BuildingStat> buildingStats = ref _shared.BuildingStats;
            ref NativeList<ZoningType> zoningTypes = ref _shared.ZoningTypes;
            ref NativeList<NativeText> zoningTypesNames = ref _shared.ZoningTypesNames;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref buildingStats, true);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypes, true);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypesNames, true);

            // Initialize native containers.（初始化原生容器。）
            int areaCount = areaQuery.CalculateEntityCount();
            int buildingCount = buildingStats.Length;
            int buildingPrefabCount = _buildingPrefabQuery.CalculateEntityCount();
            int totalEntityCount = useBuilding ? buildingCount + areaCount : areaCount;
            NativeList<BuildingStat> affliatedAreaStats = new(areaCount, Allocator.Persistent);
            NativeParallelHashSet<Entity> circularBuildingPrefabs = new(buildingPrefabCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, NativeArray<double3>> nodeEntityMap = new(totalEntityCount, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            Dictionary<Resource, string> resourcesMap = _shared.ResourceNameMap;
            List<Brand> brands = _shared.Brands;
            List<LiteralAddress> buildingAddresses = new();
            List<string> buildingAssets = new();
            List<string> buildingNames = new();
            List<BuildingStat> buildingStatsManaged = new();
            List<Theme> themes = _shared.Themes;
            List<ZoningType> zoningTypesManaged = Utils.CommonUtils.Copy(ref zoningTypes);
            string[] zoningTypesNamesManaged = Utils.CommonUtils.Copy(ref zoningTypesNames);

            try
            {
                if (useBuilding)
                {
                    // Collect the statistics of each building.（收集各個建築的統計資料。）
                    Utils.CommonUtils.AddTo(buildingStatsManaged, ref buildingStats);
                    
                    // Collect building prefabs with circular boundaries.（收集包含環形邊界的建築預製件。）
                    GetCircularBoundaryBuildingPrefabs(ref circularBuildingPrefabs);

                    // Collect the boundary of each building.（收集各個建築的邊界。）
                    CollectBoundariesJob collectBuildingBoundariesJob = new()
                    {
                        center = options.GetTMCoord(),
                        buildingDataLookup = GetComponentLookup<BuildingData>(),
                        prefabRefLookup = GetComponentLookup<PrefabRef>(),
                        transformLookup = GetComponentLookup<Game.Objects.Transform>(),
                        sourceCRS = options.GetTMProjection(),
                        targetCRS = Geodata.CRS.WGS84,
                        sourceProjection = options.GetTMProjectionDefinition(),
                        targetProjection = default,
                        buildingStats = buildingStats,
                        circularBuildingPrefabs = circularBuildingPrefabs,
                        nodeEntityMap = nodeEntityMap.AsParallelWriter()
                    };
                    JobHandle collectBuildingBoundariesHandle = collectBuildingBoundariesJob.Schedule(buildingCount, 4);
                    collectBuildingBoundariesHandle.Complete();
                }

                if (useExtractor || useLandfill)
                {
                    // Collect the statistics of each facility.（收集各個設施的統計資料。）
                    CollectAffliatedAreaStatsJob collectAreaStatsJob = new()
                    {
                        aggregateElementLookup = GetBufferLookup<AggregateElement>(),
                        aggregatedLookup = GetComponentLookup<Aggregated>(),
                        attachmentLookup = GetComponentLookup<Attachment>(),
                        buildingLookup = GetComponentLookup<Building>(),
                        buildingDataLookup = GetComponentLookup<BuildingData>(),
                        buildingPropertyDataLookup = GetComponentLookup<BuildingPropertyData>(),
                        compositionLookup = GetComponentLookup<Composition>(),
                        currentDistrictLookup = GetComponentLookup<CurrentDistrict>(),
                        curveLookup = GetComponentLookup<Curve>(),
                        edgeLookup = GetComponentLookup<Edge>(),
                        netCompositionDataLookup = GetComponentLookup<NetCompositionData>(),
                        ownerLookup = GetComponentLookup<Owner>(),
                        prefabRefLookup = GetComponentLookup<PrefabRef>(),
                        roundaboutLookup = GetComponentLookup<Game.Net.Roundabout>(),
                        storageLookup = GetComponentLookup<Storage>(),
                        storageAreaDataLookup = GetComponentLookup<StorageAreaData>(),
                        transformLookup = GetComponentLookup<Game.Objects.Transform>(),
                        emptyZoningTypeIndex = _shared.ZoningTypes.Length - 1,
                        list = affliatedAreaStats.AsParallelWriter()
                    };
                    JobHandle collectAreaStatsHandle = collectAreaStatsJob.ScheduleParallel(areaQuery, default);
                    collectAreaStatsHandle.Complete();
                    Utils.CommonUtils.AddTo(buildingStatsManaged, ref affliatedAreaStats);

                    // Collect the boundary of each facility.（收集各個設施的邊界。）
                    AreaSystem.CollectBoundariesJob collectAreaBoundariesJob = new()
                    {
                        affliatedArea = true,
                        ownerLookup = GetComponentLookup<Owner>(),
                        storageLookup = GetComponentLookup<Storage>(),
                        center = options.GetTMCoord(),
                        sourceCRS = options.GetTMProjection(),
                        targetCRS = Geodata.CRS.WGS84,
                        sourceProjection = options.GetTMProjectionDefinition(),
                        targetProjection = default,
                        nodeEntityMap = nodeEntityMap.AsParallelWriter()
                    };
                    JobHandle collectAreaBoundariesHandle = collectAreaBoundariesJob.ScheduleParallel(areaQuery, default);
                    collectAreaBoundariesHandle.Complete();
                }

                // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
                if (hasName || hasAddress || hasAsset)
                {
                    for (int i = 0; i < buildingStatsManaged.Count; i++)
                    {
                        BuildingStat stat = buildingStatsManaged[i];

                        if (hasName)
                        {
                            buildingNames.Add(_name.GetRenderedLabelName(stat.entity));
                        }

                        if (hasAddress)
                        {
                            buildingAddresses.Add(stat.address.ToLiteral(_name));
                        }

                        if (hasAsset)
                        {
                            string prefabName = _prefab.GetPrefabName(stat.prefab);
                            buildingAssets.Add(LocaleUtils.TryTranslate($"Assets.NAME[{prefabName}]", out string assetName) ? assetName : prefabName);
                        }
                    }
                }

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    for (int i = 0; i < buildingStatsManaged.Count; i++)
                    {
                        BuildingStat buildingStat = buildingStatsManaged[i];
                        Entity building = buildingStat.entity;
                        bool hasValidZoningType = (buildingStat.zoning >= 0) && (buildingStat.zoning < zoningTypesManaged.Count);
                        ZoningType zoningType = hasValidZoningType ? zoningTypesManaged[buildingStat.zoning] : zoningTypesManaged[^1];
                        if (!nodeEntityMap.TryGetValue(building, out NativeArray<double3> buildingNodes)) continue;

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");

                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        GeoJson.WriteGeometry(writer, new Geodata.Geometry(ref buildingNodes), Shape.Polygon, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();

                        if (hasName)
                        {
                            GeoJson.WriteProperty(writer, Property.Name, buildingNames[i]);
                        }
                        if (hasAddress)
                        {
                            GeoJson.WriteProperty(writer, Property.Address, buildingAddresses[i].ToArray());
                        }
                        if (hasAge)
                        {
                            GeoJson.WriteProperty(writer, Property.Age, buildingStat.GetAverageAge());
                        }
                        if (hasAsset)
                        {
                            GeoJson.WriteProperty(writer, Property.Asset, buildingAssets[i]);
                        }
                        if (hasBrand)
                        {
                            string brandName = (buildingStat.brand >= 0) && (buildingStat.brand < brands.Count) ? brands[buildingStat.brand].name : string.Empty;
                            GeoJson.WriteProperty(writer, Property.Brand, brandName);
                        }
                        if (hasCategory)
                        {
                            BuildingCategory category = options.Display[(Property.Category, IO.System.Building)] ? buildingStat.category : Utils.CommonUtils.GetFirstMatch(buildingStat.category, IO.IO.BuildingCategoryDisplayOrder);
                            GeoJson.WriteProperty(writer, Property.Category, category.ToString("G"));
                        }
                        if (hasElevation)
                        {
                            GeoJson.WriteProperty(writer, Property.Elevation, (float)Math.Round(buildingStat.elevation, 4));
                        }
                        if (hasEmployee)
                        {
                            GeoJson.WriteProperty(writer, Property.Employee, buildingStat.employee);
                        }
                        //if (hasHeight)
                        //{
                            // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasHousehold)
                        {
                            GeoJson.WriteProperty(writer, Property.Household, buildingStat.household);
                        }
                        if (hasLabor)
                        {
                            GeoJson.WriteProperty(writer, Property.Labor, buildingStat.labor);
                        }
                        if (hasLevel)
                        {
                            GeoJson.WriteProperty(writer, Property.Level, buildingStat.level);
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? buildingStat.objectType : Utils.CommonUtils.GetFirstMatch(buildingStat.objectType, IO.IO.FeatureDisplayOrder);
                            GeoJson.WriteProperty(writer, Property.Object, displayType.ToString("G"));
                        }
                        if (hasProduct)
                        {
                            GeoJson.WriteProperty(writer, Property.Product, buildingStat.GetLocalizedProducts(resourcesMap));
                        }
                        if (hasProfit)
                        {
                            GeoJson.WriteProperty(writer, Property.Profit, buildingStat.GetAverageProfit());
                        }
                        if (hasResident)
                        {
                            if (options.SeparateResident)
                            {
                                GeoJson.WriteProperty(writer, Property.Resident, new int[2] { buildingStat.residentFemale, buildingStat.residentMale }, options);
                            }
                            else
                            {
                                GeoJson.WriteProperty(writer, Property.Resident, buildingStat.residentFemale + buildingStat.residentMale);
                            }
                        }
                        if (hasSexRatio)
                        {
                            GeoJson.WriteProperty(writer, Property.SexRatio, buildingStat.GetSexRatio());
                        }
                        //if (hasStory)
                        //{
                            // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasTheme)
                        {
                            string themeName = (zoningType.theme >= 0) && (zoningType.theme < themes.Count) ? themes[zoningType.theme].name : string.Empty;
                            GeoJson.WriteProperty (writer, Property.Theme, themeName);
                        }
                        //if (hasValue)
                        //{
                            // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasWage)
                        {
                            GeoJson.WriteProperty(writer, Property.Wage, buildingStat.GetAverageWage());
                        }
                        if (hasZone)
                        {
                            // The fallback value is "Unzoned".（後備值是「無分區」。）
                            string zoningTypeName = hasValidZoningType ? zoningTypesNamesManaged[buildingStat.zoning] : zoningTypesNamesManaged[^1];
                            GeoJson.WriteProperty(writer, Property.Zone, zoningTypeName);
                        }

                        writer.WriteEndObject();
                        writer.WriteEndObject();

                        // Report method, not filled temporary.
                        onReportMethod?.Invoke(string.Empty, 0);
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref affliatedAreaStats);
                Utils.CommonUtils.Dispose(ref circularBuildingPrefabs);
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                _shared.Dispose(DisposePhase.AfterBuildingSystem);
            }
        }

        /// <summary>
        /// Write boundary geometries to the designated file.
        /// （寫出邊界幾何至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        public void WriteBoundarySHP(BinaryWriter writer, Options options, out List<Shapefile.IndexPair> indexPairs, out Bounds3 bounds, out List<Entity> entitySyncList)
        {
            Feature featureFlag = options.Features;
            bool useBuilding = featureFlag.HasFlag(Feature.Building);
            bool useExtractor = featureFlag.HasFlag(Feature.Extractor);
            bool useLandfill = featureFlag.HasFlag(Feature.Landfill);

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<BuildingStat> buildingStats = ref _shared.BuildingStats;
            ref NativeList<BuildingStat> localBuildingStats = ref _localBuildingStats;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref buildingStats);

            // Build area feature queries.（建立區域圖徵查詢。）
            List<ComponentType> areaFilters = new(_areaFilters);
            EntityQueryDesc areaQueryDesc = new()
            {
                All = _areaQueryDesc.All
            };
            if (!useExtractor) areaFilters.Add(ComponentType.ReadOnly<Extractor>());
            if (!useLandfill) areaFilters.Add(ComponentType.ReadOnly<Storage>());
            areaQueryDesc.None = areaFilters.ToArray();
            EntityQuery areaQuery = GetEntityQuery(areaQueryDesc);

            // Initialize native containers.（初始化原生容器。）
            int areaCount = areaQuery.CalculateEntityCount();
            int buildingCount = buildingStats.Length;
            int buildingPrefabCount = _buildingPrefabQuery.CalculateEntityCount();
            int totalEntityCount = useBuilding ? buildingCount + areaCount : areaCount;
            NativeList<BuildingStat> affliatedAreaStats = new(areaCount, Allocator.Persistent);
            NativeParallelHashSet<Entity> circularBuildingPrefabs = new(buildingPrefabCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, NativeArray<double3>> nodeEntityMap = new(totalEntityCount, Allocator.Persistent);
            Utils.CommonUtils.Reset(ref localBuildingStats, totalEntityCount, Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<Entity> _entitySyncList = new();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                if (useBuilding)
                {
                    // Collect building prefabs with circular boundaries.（收集包含環形邊界的建築預製件。）
                    GetCircularBoundaryBuildingPrefabs(ref circularBuildingPrefabs);

                    // Collect the boundary of each building.（收集各個建築的邊界。）
                    CollectBoundariesJob collectBuildingBoundariesJob = new()
                    {
                        center = options.GetTMCoord(),
                        buildingDataLookup = GetComponentLookup<BuildingData>(),
                        prefabRefLookup = GetComponentLookup<PrefabRef>(),
                        transformLookup = GetComponentLookup<Game.Objects.Transform>(),
                        sourceCRS = options.GetTMProjection(),
                        targetCRS = options.TargetProjection,
                        sourceProjection = options.GetTMProjectionDefinition(),
                        targetProjection = options.TargetProjectionDefinition,
                        buildingStats = buildingStats,
                        circularBuildingPrefabs = circularBuildingPrefabs,
                        nodeEntityMap = nodeEntityMap.AsParallelWriter()
                    };
                    JobHandle collectBuildingBoundariesHandle = collectBuildingBoundariesJob.Schedule(buildingCount, 8);
                    collectBuildingBoundariesHandle.Complete();
                    Utils.CommonUtils.AddTo(ref localBuildingStats, ref buildingStats);
                }

                if (useExtractor || useLandfill)
                {
                    // Collect the statistics of each facility.（收集各個設施的統計資料。）
                    CollectAffliatedAreaStatsJob collectAreaStatsJob = new()
                    {
                        aggregateElementLookup = GetBufferLookup<AggregateElement>(),
                        aggregatedLookup = GetComponentLookup<Aggregated>(),
                        attachmentLookup = GetComponentLookup<Attachment>(),
                        buildingLookup = GetComponentLookup<Building>(),
                        buildingDataLookup = GetComponentLookup<BuildingData>(),
                        buildingPropertyDataLookup = GetComponentLookup<BuildingPropertyData>(),
                        compositionLookup = GetComponentLookup<Composition>(),
                        currentDistrictLookup = GetComponentLookup<CurrentDistrict>(),
                        curveLookup = GetComponentLookup<Curve>(),
                        edgeLookup = GetComponentLookup<Edge>(),
                        netCompositionDataLookup = GetComponentLookup<NetCompositionData>(),
                        ownerLookup = GetComponentLookup<Owner>(),
                        prefabRefLookup = GetComponentLookup<PrefabRef>(),
                        roundaboutLookup = GetComponentLookup<Game.Net.Roundabout>(),
                        storageLookup = GetComponentLookup<Storage>(),
                        storageAreaDataLookup = GetComponentLookup<StorageAreaData>(),
                        transformLookup = GetComponentLookup<Game.Objects.Transform>(),
                        emptyZoningTypeIndex = _shared.ZoningTypes.Length - 1,
                        list = affliatedAreaStats.AsParallelWriter()
                    };
                    JobHandle collectAreaStatsHandle = collectAreaStatsJob.ScheduleParallel(areaQuery, default);
                    collectAreaStatsHandle.Complete();
                    Utils.CommonUtils.AddTo(ref localBuildingStats, ref affliatedAreaStats);

                    // Collect the boundary of each facility.（收集各個設施的邊界。）
                    AreaSystem.CollectBoundariesJob collectAreaBoundariesJob = new()
                    {
                        affliatedArea = true,
                        ownerLookup = GetComponentLookup<Owner>(),
                        storageLookup = GetComponentLookup<Storage>(),
                        center = options.GetTMCoord(),
                        sourceCRS = options.GetTMProjection(),
                        targetCRS = options.TargetProjection,
                        sourceProjection = options.GetTMProjectionDefinition(),
                        targetProjection = options.TargetProjectionDefinition,
                        nodeEntityMap = nodeEntityMap.AsParallelWriter()
                    };
                    JobHandle collectAreaBoundariesHandle = collectAreaBoundariesJob.ScheduleParallel(areaQuery, default);
                    collectAreaBoundariesHandle.Complete();
                }

                Task writerThread = Task.Run(() =>
                {
                    int enumeratorIndex = 0;
                    int shapeId = Shapefile.GetShapeType(VectorKind.Boundary, options.Elevation);
                    NativeParallelHashMap<Entity, NativeArray<double3>>.Enumerator enumerator = nodeEntityMap.GetEnumerator();

                    if (BitConverter.IsLittleEndian)
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeArray<double3>> feature = enumerator.Current;
                            Shapefile.WriteGeometryLE(writer, enumeratorIndex, shapeId, new(ref feature.Value), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(feature.Key);
                            _indexPairs.Add(indexPair);
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeArray<double3>> feature = enumerator.Current;
                            Shapefile.WriteGeometryBE(writer, enumeratorIndex, shapeId, new(ref feature.Value), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(feature.Key);
                            _indexPairs.Add(indexPair);
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref affliatedAreaStats);
                Utils.CommonUtils.Dispose(ref circularBuildingPrefabs);
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                bounds = _bounds;
                entitySyncList = _entitySyncList;
                indexPairs = _indexPairs;
            }
        }

        /// <summary>
        /// The job to collect and aggregate affliated area statistics.
        /// （收集並聚合附屬區域統計的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectAffliatedAreaStatsJob : IJobEntity
        {
            [ReadOnly]
            public BufferLookup<AggregateElement> aggregateElementLookup;

            [ReadOnly]
            public ComponentLookup<Aggregated> aggregatedLookup;

            [ReadOnly]
            public ComponentLookup<Attachment> attachmentLookup;

            [ReadOnly]
            public ComponentLookup<Building> buildingLookup;

            [ReadOnly]
            public ComponentLookup<BuildingData> buildingDataLookup;

            [ReadOnly]
            public ComponentLookup<BuildingPropertyData> buildingPropertyDataLookup;

            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<CurrentDistrict> currentDistrictLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<Edge> edgeLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<Owner> ownerLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.Roundabout> roundaboutLookup;

            [ReadOnly]
            public ComponentLookup<Storage> storageLookup;

            [ReadOnly]
            public ComponentLookup<StorageAreaData> storageAreaDataLookup;

            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup;

            [ReadOnly]
            public int emptyZoningTypeIndex;

            [WriteOnly]
            public NativeList<BuildingStat>.ParallelWriter list;

            public void Execute(in Game.Areas.Geometry geometry, in Owner owner, in PrefabRef prefabRef, Entity area)
            {
                BuildingStat stat = new()
                {
                    entity = area,
                    address = Address.Null,
                    age = 0f,
                    brand = 0,
                    category = BuildingCategory.None,
                    company = 0,
                    elevation = geometry.m_CenterPosition.y,
                    employee = 0,
                    household = 0,
                    labor = 0,
                    level = 0,
                    mainBuilding = Entity.Null,
                    objectType = Feature.Extractor,
                    prefab = prefabRef,
                    product = Resource.NoResource,
                    profit = 0,
                    residentFemale = 0,
                    residentMale = 0,
                    wage = 0,
                    zoning = emptyZoningTypeIndex
                };

                // Find the main building.（找到主建築。）
                if (attachmentLookup.TryGetComponent(owner.m_Owner, out Attachment attachment))
                {
                    stat.mainBuilding = attachment.m_Attached;
                }
                else
                {
                    stat.mainBuilding = owner.m_Owner;
                }

                // Find the product.（找到產品。）
                if (storageLookup.TryGetComponent(area, out _))
                {
                    if (storageAreaDataLookup.TryGetComponent(prefabRef.m_Prefab, out StorageAreaData prefabStorageData))
                    {
                        stat.objectType = Feature.Landfill;
                        stat.product = prefabStorageData.m_Resources;
                    }
                }
                else
                {
                    // There are two layers of area for extractor area, and we want to include harvest area (top layer), rather than navigation area (bottom layer).
                    // （開採區域有兩個圖層，而我們只想要收穫區域（上層）而非導航區域（下層）。）
                    if (ownerLookup.TryGetComponent(owner.m_Owner, out _)) return;

                    if (prefabRefLookup.TryGetComponent(owner.m_Owner, out PrefabRef ownerPrefabRef) && buildingPropertyDataLookup.TryGetComponent(ownerPrefabRef.m_Prefab, out BuildingPropertyData ownerPrefabProperty))
                    {
                        stat.product = ownerPrefabProperty.m_AllowedManufactured;
                    }
                }

                if (buildingLookup.TryGetComponent(owner.m_Owner, out Building ownerBuilding))
                {
                    if (SharedDataCollectionSystem.GetAddress(owner.m_Owner, ownerBuilding.m_RoadEdge, ownerBuilding.m_CurvePosition, out Entity road, out int number,
                                                          ref aggregateElementLookup, ref aggregatedLookup, ref buildingDataLookup, ref curveLookup,
                                                          ref compositionLookup, ref edgeLookup, ref netCompositionDataLookup, ref prefabRefLookup,
                                                          ref roundaboutLookup, ref transformLookup))
                    {
                        stat.address.street = road;
                        stat.address.number = number;
                    }
                    
                    if (currentDistrictLookup.TryGetComponent(owner.m_Owner, out CurrentDistrict currentDistrict))
                    {
                        stat.address.district = currentDistrict.m_District;
                    }
                }

                list.AddNoResize(stat);
            }
        }
        
        /// <summary>
        /// The job to collect buildings' boundaries.
        /// （收集建築邊界的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectBoundariesJob : IJobParallelFor
        {
            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public ComponentLookup<BuildingData> buildingDataLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [ReadOnly]
            public NativeParallelHashSet<Entity> circularBuildingPrefabs;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeArray<double3>>.ParallelWriter nodeEntityMap;

            public void Execute(int index)
            {
                Entity building = buildingStats[index].entity;
                if (prefabRefLookup.TryGetComponent(building, out PrefabRef prefabRefComponent))
                {
                    Entity buildingPrefabEntity = prefabRefComponent.m_Prefab;

                    if (!buildingDataLookup.TryGetComponent(buildingPrefabEntity, out BuildingData buildingDataComponent)) return;
                    if (!transformLookup.TryGetComponent(building, out Game.Objects.Transform transformComponent)) return;
                    int2 lotSize = buildingDataComponent.m_LotSize;

                    if (circularBuildingPrefabs.Contains(buildingPrefabEntity))
                    {
                        float radius = math.min(lotSize.x, lotSize.y) * 4;
                        int pointsCount = Utils.MathUtils.CountInterpolationPoints(radius);
                        float3 position = transformComponent.m_Position;
                        double angle = 2 * math.PI_DBL / pointsCount;

                        NativeArray<double3> nodes = new(pointsCount, Allocator.Persistent);
                        for (int i = 0; i < pointsCount; i++)
                        {
                            float3 delta = new((float)(radius * -math.sin(angle * i)), (float)(radius * math.cos(angle * i)), 0);
                            nodes[i] = Geodata.Transform.Apply(center.Shift(position.xzy + delta), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();
                        }
                        nodeEntityMap.TryAdd(building, nodes);
                    }
                    else
                    {
                        Quad3 corners = BuildingUtils.CalculateCorners(transformComponent, lotSize);
                        bool isCounterClockwise = Utils.MathUtils.IsCounterclockwise(corners);
                        double3 a = Geodata.Transform.Apply(center.Shift(corners.a.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();
                        double3 b = Geodata.Transform.Apply(center.Shift(corners.b.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();
                        double3 c = Geodata.Transform.Apply(center.Shift(corners.c.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();
                        double3 d = Geodata.Transform.Apply(center.Shift(corners.d.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();

                        NativeArray<double3> nodes = new(4, Allocator.Persistent);
                        nodes[0] = a;
                        nodes[1] = isCounterClockwise ? b : d;
                        nodes[2] = c;
                        nodes[3] = isCounterClockwise ? d : b;
                        nodeEntityMap.TryAdd(building, nodes);
                    }
                }
            }
        }
    }
}