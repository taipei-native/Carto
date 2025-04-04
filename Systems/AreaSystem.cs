using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Areas;
using Game.Common;
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
    /// The system that searches area.
    /// （搜尋區域的系統。）
    /// </summary>
    public partial class AreaSystem : GameSystemBase
    {
        /// <summary>
        /// The exclusion filters in the entity query.
        /// （實體查詢中排除的篩選條件。）
        /// </summary>
        static readonly List<ComponentType> _filters = new()
        {
            ComponentType.ReadOnly<Deleted>(),
            ComponentType.ReadOnly<Extractor>(),
            ComponentType.ReadOnly<Navigation>(),
            ComponentType.ReadOnly<Space>(),
            ComponentType.ReadOnly<Storage>(),
            ComponentType.ReadOnly<Surface>(),
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
        /// The query for existing map tiles.（現有地圖區塊的查詢。）
        /// </summary>
        static EntityQuery _mapTileQuery;

        /// <summary>
        /// The query for existing areas.（現有區域的查詢。）
        /// </summary>
        static EntityQueryDesc _queryDesc;

        /// <summary>
        /// The system collecting shared data.（收集共享資料的系統。）<br/>
        /// See <see cref="Instance.Shared"/> for more information.
        /// </summary>
        static readonly SharedDataCollectionSystem _shared = Instance.Shared;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _mapTileQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>(),
                    ComponentType.ReadOnly<MapTile>()
                },
                None = _filters.ToArray()
            });
            
            _queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>()
                }
            };

            base.OnCreate();
            _log.Debug("AreaSystem instance created. 區域系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Fill the map between area entity and building's index in <see cref="SharedDataCollectionSystem.BuildingStats"/>.<br/>
        /// （填入區域實體與建築在 <see cref="SharedDataCollectionSystem.BuildingStats"/> 的索引值的映射表。）
        /// </summary>
        /// <param name="buildingStats">The list of all building's statistics in the savegame.（遊戲存檔內所有建築的統計數據。）</param>
        /// <param name="areaEntityMap">The map between area entity and the building index.（區域實體與建築索引值間的映射表。）</param>
        /// <param name="mapTileStatistics">Whether to export statistics (e.g. age, company, employee, household, ...) for map tiles.（是否要輸出地圖區塊的統計資料？（例如年齡、公司、員工、家庭……）？）</param>
        private void FillBuildingStatMap(ref NativeList<BuildingStat> buildingStats, ref NativeParallelMultiHashMap<Entity, int> areaEntityMap, bool mapTileStatistics)
        {
            // Initialize native containers.（初始化原生容器。）
            NativeArray<float3> locations = new(buildingStats.Length, Allocator.Persistent);
            NativeList<BVHUtils.Triangle> triangles = new(_mapTileQuery.CalculateEntityCount() * 4, Allocator.Persistent);

            // Map each building to districts.（將各棟建築映射至行政區。）
            MapBuildingsToDistrictsJob mapDistrictsJob = new()
            {
                currentDistrictLookup = GetComponentLookup<CurrentDistrict>(true),
                buildingStats = buildingStats,
                hashmap = areaEntityMap.AsParallelWriter()
            };
            JobHandle mapDistrictsHandle = mapDistrictsJob.Schedule(buildingStats.Length, 8);
            mapDistrictsHandle.Complete();

            // Only execute this part when map tile requires statistical fields.
            //（僅在地圖區塊需要統計欄位時執行。）
            if (mapTileStatistics)
            {
                // Extract building centroids.（萃取建築中點。）
                GetBuildingLocationsJob getLocationsJob = new()
                {
                    transformLookup = GetComponentLookup<Game.Objects.Transform>(true),
                    buildingStats = buildingStats,
                    locations = locations
                };
                JobHandle getLocationHandle = getLocationsJob.Schedule(buildingStats.Length, 8);
                getLocationHandle.Complete();

                // Extract map tile triangles.（萃取地圖區塊三角形。）
                // Can't apply parallel operation on this job since the length of `triangles` is unknown.（無法在這個工作上執行平行處理，因為 `triangle` 的長度未知。）
                RetrieveTrianglesJob retrieveJob = new()
                {
                    triangleList = triangles
                };
                JobHandle retrieveHandle = retrieveJob.Schedule(_mapTileQuery, default);
                retrieveHandle.Complete();

                // Map each building to map tiles.（將各建築映射至地圖區塊。）
                BVHUtils.GetIntersectMap(ref triangles, ref locations, ref areaEntityMap);
            }

            // Dispose the native containers.（拋棄原生容器。）
            Utils.CommonUtils.Dispose(ref locations);
            Utils.CommonUtils.Dispose(ref triangles);
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
            Feature featureFlag = options.Features;
            if (!featureFlag.HasFlag(Feature.District)) _filters.Add(ComponentType.ReadOnly<District>());
            if (!featureFlag.HasFlag(Feature.MapTile)) _filters.Add(ComponentType.ReadOnly<MapTile>());
            _queryDesc.None = _filters.ToArray();
            EntityQuery query = GetEntityQuery(_queryDesc);

            bool hasName = validatedFields.Contains(Property.Name);
            bool hasAge = validatedFields.Contains(Property.Age);
            bool hasArea = validatedFields.Contains(Property.Area);
            bool hasCompany = validatedFields.Contains(Property.Company);
            bool hasEmployee = validatedFields.Contains(Property.Employee);
            bool hasHousehold = validatedFields.Contains(Property.Household);
            bool hasLabor = validatedFields.Contains(Property.Labor);
            bool hasObject = validatedFields.Contains(Property.Object);
            bool hasProfit = validatedFields.Contains(Property.Profit);
            bool hasResident = validatedFields.Contains(Property.Resident);
            bool hasSexRatio = validatedFields.Contains(Property.SexRatio);
            bool hasUnlocked = validatedFields.Contains(Property.Unlocked);
            bool hasWage = validatedFields.Contains(Property.Wage);
            bool hasStatistics = hasAge | hasCompany | hasEmployee | hasHousehold | hasLabor | hasProfit | hasResident | hasSexRatio | hasWage;

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<BuildingStat> buildingStats = ref _shared.BuildingStats;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref buildingStats, true);

            // Initialize native containers.（初始化原生容器。）
            int areaCount = query.CalculateEntityCount();
            int burstCompatibleFieldCount = validatedFields.Count - (hasName ? 1 : 0) - (hasObject ? 1 : 0);
            NativeList<AreaStat> areaStats = new(areaCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, int> syncMap = new(areaCount, Allocator.Persistent);
            NativeParallelHashSet<EnumWrapper<Property>> propertySet = new(burstCompatibleFieldCount, Allocator.Persistent);
            NativeParallelMultiHashMap<Entity, int> areaEntityMap = new(buildingStats.Length * 2, Allocator.Persistent);
            NativeParallelMultiHashMap<EnumWrapper<Property>, FieldInfo> propertyFieldMap = new(burstCompatibleFieldCount * areaCount, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<Feature> areaFeatures = new();
            List<string> areaNames = new();
            Dictionary<Property, FieldInfo> _fieldMap = new();

            // Copy managed container contents to their native counterparts.（將控管容器的內容複製至原生容器。）
            Shapefile.FilterBurstCompatibleProperties(validatedFields, ref propertySet);

            try
            {
                // Only execute this part when any of these fields are required: age, company, employee, household, labor, profit, resident, sex ratio, and wage.
                //（僅在需要下列任何欄位時執行：年齡、公司、員工、家庭、勞工、利潤、居民、性別比、薪資）
                if (hasStatistics) FillBuildingStatMap(ref buildingStats, ref areaEntityMap, options.StatisticsMapTile);

                // Collect the statistics of each area.（收集各個區域的統計資料。）
                CollectAreaStatsSHPJob collectStatsJob = new()
                {
                    districtLookup = GetComponentLookup<District>(true),
                    mapTileLookup = GetComponentLookup<MapTile>(true),
                    nativeLookup = GetComponentLookup<Native>(true),
                    buildingStats = buildingStats,
                    propertySet = propertySet,
                    areaEntityMap = areaEntityMap,
                    statslist = areaStats.AsParallelWriter(),
                    propertyFieldMap = propertyFieldMap.AsParallelWriter()
                };
                JobHandle collectStatsHandle = collectStatsJob.ScheduleParallel(query, default);
                collectStatsHandle.Complete();

                // Sync the entity order with that of the .shp file.（與 .shp 檔案的實體順序同步。）
                Shapefile.SyncStatsToIndex(entitySyncList, ref areaStats, ref syncMap);

                // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
                if (hasName)
                {
                    FieldInfo nameField = new(0, 0, false, FieldType.String);

                    for (int i = 0; i < areaStats.Length; i++)
                    {
                        AreaStat stat = areaStats[i];
                        areaNames.Add(stat.objectType == Feature.District ? _name.GetRenderedLabelName(stat.entity) : _name.GetDebugName(stat.entity));
                        nameField += new FieldInfo(areaNames[i]);
                    }

                    _fieldMap.Add(Property.Name, nameField);
                }

                if (hasObject)
                {
                    FieldInfo objectField = new(0, 0, false, FieldType.String);

                    for (int i = 0; i < areaStats.Length; i++)
                    {
                        AreaStat stat = areaStats[i];
                        areaFeatures.Add(options.Display[(Property.Object, IO.System.Unknown)] ? stat.objectType : Utils.CommonUtils.GetFirstMatch(stat.objectType, IO.IO.FeatureDisplayOrder));
                        objectField += new FieldInfo(areaFeatures[i].ToString());
                    }

                    _fieldMap.Add(Property.Object, objectField);
                }

                Shapefile.CombineFieldInfos(ref propertySet, ref propertyFieldMap, _fieldMap);

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    for (int index = 0; index < entitySyncList.Count; index++)
                    {
                        if (!syncMap.TryGetValue(entitySyncList[index], out int i))
                        {
                            _log.Error($"Couldn't find the statistical object of {entitySyncList[index]} at index {index}. 無法找到位於索引值 {index} 的實體 {entitySyncList[index]} 之統計物件。");
                        }

                        AreaStat stat = areaStats[i];
                        writer.Write((byte)32);

                        if (hasName && _fieldMap.TryGetValue(Property.Name, out FieldInfo nameField))
                        {
                            Shapefile.WriteRecord(writer, nameField, areaNames[i]);
                        }
                        if (hasAge && _fieldMap.TryGetValue(Property.Age, out FieldInfo ageField))
                        {
                            Shapefile.WriteRecord(writer, ageField, stat.GetAverageAge());
                        }
                        if (hasArea && _fieldMap.TryGetValue(Property.Area, out FieldInfo areaField))
                        {
                            Shapefile.WriteRecord(writer, areaField, stat.area);
                        }
                        if (hasCompany && _fieldMap.TryGetValue(Property.Company, out FieldInfo companyField))
                        {
                            Shapefile.WriteRecord(writer, companyField, stat.company);
                        }
                        if (hasEmployee && _fieldMap.TryGetValue(Property.Employee, out FieldInfo employeeField))
                        {
                            Shapefile.WriteRecord(writer, employeeField, stat.employee);
                        }
                        if (hasHousehold && _fieldMap.TryGetValue(Property.Household, out FieldInfo householdField))
                        {
                            Shapefile.WriteRecord(writer, householdField, stat.household);
                        }
                        if (hasLabor && _fieldMap.TryGetValue(Property.Labor, out FieldInfo laborField))
                        {
                            Shapefile.WriteRecord(writer, laborField, stat.labor);
                        }
                        if (hasObject && _fieldMap.TryGetValue(Property.Object, out FieldInfo objectField))
                        {
                            Shapefile.WriteRecord(writer, objectField, areaFeatures[i].ToString());
                        }
                        if (hasProfit && _fieldMap.TryGetValue(Property.Profit, out FieldInfo profitField))
                        {
                            Shapefile.WriteRecord(writer, profitField, stat.GetAverageProfit());
                        }
                        if (hasResident && _fieldMap.TryGetValue(Property.Resident, out FieldInfo residentField))
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
                        if (hasSexRatio && _fieldMap.TryGetValue(Property.SexRatio, out FieldInfo sexRatioField))
                        {
                            Shapefile.WriteRecord(writer, sexRatioField, stat.GetSexRatio());
                        }
                        if (hasUnlocked && _fieldMap.TryGetValue(Property.Unlocked, out FieldInfo unlockedField))
                        {
                            Shapefile.WriteRecord(writer, unlockedField, stat.unlocked);
                        }
                        if (hasWage && _fieldMap.TryGetValue(Property.Wage, out FieldInfo wageField))
                        {
                            Shapefile.WriteRecord(writer, wageField, stat.GetAverageWage());
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
                Utils.CommonUtils.Dispose(ref areaEntityMap);
                Utils.CommonUtils.Dispose(ref areaStats);
                Utils.CommonUtils.Dispose(ref propertyFieldMap);
                Utils.CommonUtils.Dispose(ref propertySet);
                Utils.CommonUtils.Dispose(ref syncMap);
                Instance.Shared.Dispose(DisposePhase.AfterAreaSystem);
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
            if (!featureFlag.HasFlag(Feature.District)) _filters.Add(ComponentType.ReadOnly<District>());
            if (!featureFlag.HasFlag(Feature.MapTile)) _filters.Add(ComponentType.ReadOnly<MapTile>());
            _queryDesc.None = _filters.ToArray();
            EntityQuery query = GetEntityQuery(_queryDesc);

            bool hasName = options.Contains(Property.Name, IO.System.Area);
            bool hasAge = options.Contains(Property.Age, IO.System.Area);
            bool hasArea = options.Contains(Property.Area, IO.System.Area);
            bool hasCompany = options.Contains(Property.Company, IO.System.Area);
            bool hasEmployee = options.Contains(Property.Employee, IO.System.Area);
            bool hasHousehold = options.Contains(Property.Household, IO.System.Area);
            bool hasLabor = options.Contains(Property.Labor, IO.System.Area);
            bool hasObject = options.Contains(Property.Object, IO.System.Area);
            bool hasProfit = options.Contains(Property.Profit, IO.System.Area);
            bool hasResident = options.Contains(Property.Resident, IO.System.Area);
            bool hasSexRatio = options.Contains(Property.SexRatio, IO.System.Area);
            bool hasUnlocked = options.Contains(Property.Unlocked, IO.System.Area);
            bool hasWage = options.Contains(Property.Wage, IO.System.Area);
            bool hasStatistics = hasAge | hasCompany | hasEmployee | hasHousehold | hasLabor | hasProfit | hasResident | hasSexRatio | hasWage;

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<BuildingStat> buildingStats = ref _shared.BuildingStats;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref buildingStats, true);

            // Initialize native containers.（初始化原生容器。）
            int areaCount = query.CalculateEntityCount();
            NativeList<AreaStat> areaStats = new(areaCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, NativeArray<float3>> nodeEntityMap = new(areaCount, Allocator.Persistent);
            NativeParallelMultiHashMap<Entity, int> areaEntityMap = new(buildingStats.Length * 2, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<string> areaNames = new();

            try
            {
                // Only execute this part when any of these fields are required: age, company, employee, household, labor, profit, resident, sex ratio, and wage.
                //（僅在需要下列任何欄位時執行：年齡、公司、員工、家庭、勞工、利潤、居民、性別比、薪資）
                if (hasStatistics) FillBuildingStatMap(ref buildingStats, ref areaEntityMap, options.StatisticsMapTile);

                // Collect the statistics of each area.（收集各個區域的統計資料。）
                CollectAreaStatsJob collectStatsJob = new()
                {
                    districtLookup = GetComponentLookup<District>(true),
                    mapTileLookup = GetComponentLookup<MapTile>(true),
                    nativeLookup = GetComponentLookup<Native>(true),
                    areaEntityMap = areaEntityMap,
                    buildingStats = buildingStats,
                    statslist = areaStats.AsParallelWriter()
                };
                JobHandle collectStatsHandle = collectStatsJob.ScheduleParallel(query, default);
                collectStatsHandle.Complete();

                // Collect the boundary of each area.（收集各個區域的邊界。）
                CollectBoundariesJob collectBoundariesJob = new()
                {
                    nodeEntityMap = nodeEntityMap.AsParallelWriter()
                };
                JobHandle collectBoundariesHandle = collectBoundariesJob.ScheduleParallel(query, default);
                collectBoundariesHandle.Complete();

                // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
                if (hasName)
                {
                    for (int i = 0; i < areaStats.Length; i++)
                    {
                        AreaStat stat = areaStats[i];
                        areaNames.Add(stat.objectType == Feature.District ? _name.GetRenderedLabelName(stat.entity) : _name.GetDebugName(stat.entity));
                    }
                }

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    Coord referenceCoord = options.GetTMCoord();
                    Geodata.CRS referenceProjection = options.GetTMProjection();
                    ProjectionDefinition referenceProjectionDefinition = options.GetTMProjectionDefinition();
                    
                    for (int i = 0; i < areaStats.Length; i++)
                    {
                        AreaStat stat = areaStats[i];
                        if (!nodeEntityMap.TryGetValue(stat.entity, out NativeArray<float3> areaNodes)) continue;
                        float3[] transformedAreaNodes = new float3[areaNodes.Length];

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");
                        
                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        for (int j = 0; j < areaNodes.Length; j++)
                        {
                            Coord coord = new(referenceCoord.Double3 + areaNodes[j], referenceCoord);
                            transformedAreaNodes[j] = Transform.Apply(coord, referenceProjection, Geodata.CRS.WGS84, referenceProjectionDefinition, new ProjectionDefinition()).Float3;
                        }

                        GeoJson.WriteGeometry(writer, new Geodata.Geometry(new float3[1][] { transformedAreaNodes }), Shape.Polygon, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();
                        if (hasName)
                        {
                            GeoJson.WriteProperty(writer, Property.Name, areaNames[i]);
                        }
                        if (hasAge)
                        {
                            GeoJson.WriteProperty(writer, Property.Age, stat.GetAverageAge());
                        }
                        if (hasArea)
                        {
                            GeoJson.WriteProperty(writer, Property.Area, stat.area);
                        }
                        if (hasCompany)
                        {
                            GeoJson.WriteProperty(writer, Property.Company, stat.company);
                        }
                        if (hasEmployee)
                        {
                            GeoJson.WriteProperty(writer, Property.Employee, stat.employee);
                        }
                        if (hasHousehold)
                        {
                            GeoJson.WriteProperty(writer, Property.Household, stat.household);
                        }
                        if (hasLabor)
                        {
                            GeoJson.WriteProperty(writer, Property.Labor, stat.labor);
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? stat.objectType : Utils.CommonUtils.GetFirstMatch(stat.objectType, IO.IO.FeatureDisplayOrder);
                            GeoJson.WriteProperty(writer, Property.Object, displayType.ToString("G"));
                        }
                        if (hasProfit)
                        {
                            GeoJson.WriteProperty(writer, Property.Profit, stat.GetAverageProfit());
                        }
                        if (hasResident)
                        {
                            if (options.SeparateResident)
                            {
                                GeoJson.WriteProperty(writer, Property.Resident, new int[2] { stat.residentFemale, stat.residentMale }, options);
                            }
                            else
                            {
                                GeoJson.WriteProperty(writer, Property.Resident, stat.residentFemale + stat.residentMale);
                            }
                        }
                        if (hasSexRatio)
                        {
                            GeoJson.WriteProperty(writer, Property.SexRatio, stat.GetSexRatio());
                        }
                        if (hasUnlocked)
                        {
                            GeoJson.WriteProperty(writer, Property.Unlocked, stat.unlocked);
                        }
                        if (hasWage)
                        {
                            GeoJson.WriteProperty(writer, Property.Wage, stat.GetAverageWage());
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
                Utils.CommonUtils.Dispose(ref areaEntityMap);
                Utils.CommonUtils.Dispose(ref areaStats);
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                Instance.Shared.Dispose(DisposePhase.AfterAreaSystem);
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
            if (!featureFlag.HasFlag(Feature.District)) _filters.Add(ComponentType.ReadOnly<District>());
            if (!featureFlag.HasFlag(Feature.MapTile)) _filters.Add(ComponentType.ReadOnly<MapTile>());
            _queryDesc.None = _filters.ToArray();
            EntityQuery query = GetEntityQuery(_queryDesc);

            // Initialize native containers.（初始化原生容器。）
            int areaCount = query.CalculateEntityCount();
            NativeParallelHashMap<Entity, NativeArray<float3>> nodeEntityMap = new(areaCount, Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<Entity> _entitySyncList = new();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                CollectBoundariesJob collectBoundariesJob = new()
                {
                    nodeEntityMap = nodeEntityMap.AsParallelWriter()
                };
                JobHandle collectBoundariesHandle = collectBoundariesJob.ScheduleParallel(query, default);
                collectBoundariesHandle.Complete();
                
                Task writerThread = Task.Run(() =>
                {
                    Coord referenceCoord = options.GetTMCoord();
                    Geodata.CRS referenceProjection = options.GetTMProjection();
                    int enumeratorIndex = 0;
                    int shapeId = Shapefile.GetShapeType(VectorKind.Boundary, options.Elevation);
                    NativeParallelHashMap<Entity, NativeArray<float3>>.Enumerator enumerator = nodeEntityMap.GetEnumerator();
                    ProjectionDefinition referenceProjectionDefinition = options.GetTMProjectionDefinition();

                    if (BitConverter.IsLittleEndian)
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeArray<float3>> feature = enumerator.Current;
                            float3[] transformedAreaNodes = new float3[feature.Value.Length];

                            for (int i = 0; i < feature.Value.Length; i++)
                            {
                                Coord coord = new(referenceCoord.Double3 + feature.Value[i], referenceCoord);
                                transformedAreaNodes[i] = Transform.Apply(coord, referenceProjection, options.TargetProjection, referenceProjectionDefinition, options.TargetProjectionDefinition).Float3;
                            }

                            Shapefile.WriteGeometryLE(writer, enumeratorIndex, shapeId, new(new float3[1][] { transformedAreaNodes }), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
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
                            KeyValue<Entity, NativeArray<float3>> feature = enumerator.Current;
                            float3[] transformedAreaNodes = new float3[feature.Value.Length];

                            for (int i = 0; i < feature.Value.Length; i++)
                            {
                                Coord coord = new(referenceCoord.Double3 + feature.Value[i], referenceCoord);
                                transformedAreaNodes[i] = Transform.Apply(coord, referenceProjection, options.TargetProjection, referenceProjectionDefinition, options.TargetProjectionDefinition).Float3;
                            }

                            Shapefile.WriteGeometryBE(writer, enumeratorIndex, shapeId, new(new float3[1][] { transformedAreaNodes }), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
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
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                bounds = _bounds;
                entitySyncList = _entitySyncList;
                indexPairs = _indexPairs;
            }
        }

        /// <summary>
        /// The job to collect and aggregate area statistics.
        /// （收集並聚合區域統計的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectAreaStatsJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<District> districtLookup;

            [ReadOnly]
            public ComponentLookup<MapTile> mapTileLookup;

            [ReadOnly]
            public ComponentLookup<Native> nativeLookup;

            [ReadOnly]
            public NativeParallelMultiHashMap<Entity, int> areaEntityMap;

            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [WriteOnly]
            public NativeList<AreaStat>.ParallelWriter statslist;

            public void Execute(in Game.Areas.Geometry geometry, Entity area)
            {
                AreaStat stat = new()
                {
                    entity = area,
                    age = 0f,
                    area = geometry.m_SurfaceArea,
                    company = 0,
                    employee = 0,
                    household = 0,
                    labor = 0,
                    objectType = Feature.None,
                    profit = 0,
                    residentFemale = 0,
                    residentMale = 0,
                    wage = 0,
                    unlocked = false
                };

                if (areaEntityMap.TryGetFirstValue(area, out int buildingIndex, out NativeParallelMultiHashMapIterator<Entity> iterator))
                {
                    do
                    {
                        if ((buildingIndex >= 0) & (buildingIndex < buildingStats.Length))
                        {
                            ref BuildingStat building = ref buildingStats.ElementAt(buildingIndex);
                            stat.age += building.age;
                            stat.company += building.company;
                            stat.employee += building.employee;
                            stat.household += building.household;
                            stat.labor += building.labor;
                            stat.profit += building.profit;
                            stat.residentFemale += building.residentFemale;
                            stat.residentMale += building.residentMale;
                            stat.wage += building.wage;
                        }
                    }
                    while (areaEntityMap.TryGetNextValue(out buildingIndex, ref iterator));
                }

                if (districtLookup.TryGetComponent(area, out _)) stat.objectType |= Feature.District;
                if (mapTileLookup.TryGetComponent(area, out _)) stat.objectType |= Feature.MapTile;

                if (mapTileLookup.TryGetComponent(area, out _) && !nativeLookup.TryGetComponent(area, out _))
                {
                    stat.unlocked = true;
                }

                statslist.AddNoResize(stat);
            }
        }

        /// <summary>
        /// The job to collect and aggregate area statistics (Shapefile variant).
        /// （收集並聚合區域統計的工作（Shapefile 變種）。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectAreaStatsSHPJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<District> districtLookup;

            [ReadOnly]
            public ComponentLookup<MapTile> mapTileLookup;

            [ReadOnly]
            public ComponentLookup<Native> nativeLookup;
            
            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [ReadOnly]
            public NativeParallelHashSet<EnumWrapper<Property>> propertySet;

            [ReadOnly]
            public NativeParallelMultiHashMap<Entity, int> areaEntityMap;

            [WriteOnly]
            public NativeParallelMultiHashMap<EnumWrapper<Property>, FieldInfo>.ParallelWriter propertyFieldMap;

            [WriteOnly]
            public NativeList<AreaStat>.ParallelWriter statslist;

            public void Execute(in Game.Areas.Geometry geometry, Entity area)
            {
                AreaStat stat = new()
                {
                    entity = area,
                    age = 0f,
                    area = geometry.m_SurfaceArea,
                    company = 0,
                    employee = 0,
                    household = 0,
                    labor = 0,
                    objectType = Feature.None,
                    profit = 0,
                    residentFemale = 0,
                    residentMale = 0,
                    wage = 0,
                    unlocked = false
                };

                if (areaEntityMap.TryGetFirstValue(area, out int buildingIndex, out NativeParallelMultiHashMapIterator<Entity> iterator))
                {
                    do
                    {
                        if ((buildingIndex >= 0) & (buildingIndex < buildingStats.Length))
                        {
                            ref BuildingStat building = ref buildingStats.ElementAt(buildingIndex);
                            stat.age += building.age;
                            stat.company += building.company;
                            stat.employee += building.employee;
                            stat.household += building.household;
                            stat.labor += building.labor;
                            stat.profit += building.profit;
                            stat.residentFemale += building.residentFemale;
                            stat.residentMale += building.residentMale;
                            stat.wage += building.wage;
                        }
                    }
                    while (areaEntityMap.TryGetNextValue(out buildingIndex, ref iterator));
                }

                if (districtLookup.TryGetComponent(area, out _)) stat.objectType |= Feature.District;
                if (mapTileLookup.TryGetComponent(area, out _)) stat.objectType |= Feature.MapTile;

                if (mapTileLookup.TryGetComponent(area, out _) && !nativeLookup.TryGetComponent(area, out _))
                {
                    stat.unlocked = true;
                }

                statslist.AddNoResize(stat);

                if (propertySet.Contains(Property.Age))
                {
                    propertyFieldMap.Add(Property.Age, new(stat.GetAverageAge()));
                }
                
                if (propertySet.Contains(Property.Area))
                {
                    propertyFieldMap.Add(Property.Area, new(stat.area));
                }

                if (propertySet.Contains(Property.Company))
                {
                    propertyFieldMap.Add(Property.Company, new(stat.company));
                }

                if (propertySet.Contains(Property.Employee))
                {
                    propertyFieldMap.Add(Property.Employee, new(stat.employee));
                }

                if (propertySet.Contains(Property.Household))
                {
                    propertyFieldMap.Add(Property.Household, new(stat.household));
                }

                if (propertySet.Contains(Property.Labor))
                {
                    propertyFieldMap.Add(Property.Labor, new(stat.labor));
                }

                if (propertySet.Contains(Property.Profit))
                {
                    propertyFieldMap.Add(Property.Profit, new(stat.GetAverageProfit()));
                }

                if (propertySet.Contains(Property.Resident))
                {
                    propertyFieldMap.Add(Property.Resident, new(stat.residentFemale + stat.residentMale));
                }

                if (propertySet.Contains(Property.SexRatio))
                {
                    propertyFieldMap.Add(Property.SexRatio, new(stat.GetSexRatio()));
                }

                if (propertySet.Contains(Property.Unlocked))
                {
                    propertyFieldMap.Add(Property.Unlocked, new(stat.unlocked));
                }

                if (propertySet.Contains(Property.Wage))
                {
                    propertyFieldMap.Add(Property.Wage, new(stat.GetAverageWage()));
                }
            }
        }

        /// <summary>
        /// The job to collect area's boundaries.
        /// （收集區域邊界的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectBoundariesJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeArray<float3>>.ParallelWriter nodeEntityMap;

            public void Execute(in Area areaComponent, in DynamicBuffer<Node> nodes, Entity area)
            {
                NativeArray<float3> nodesArray = new(nodes.Length, Allocator.Persistent);
                if ((areaComponent.m_Flags & AreaFlags.CounterClockwise) != 0)
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodesArray[i] = nodes[i].m_Position.xzy;
                    }
                }
                else
                {
                    for (int i = nodes.Length - 1; i > -1; i--)
                    {
                        nodesArray[i] = nodes[i].m_Position.xzy;
                    }
                }

                nodeEntityMap.TryAdd(area, nodesArray);
            }
        }

        /// <summary>
        /// The job to extract building centroid positions.
        /// （萃取建築中點位置的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct GetBuildingLocationsJob : IJobParallelFor
        {
            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup; 
            
            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [WriteOnly]
            public NativeArray<float3> locations;

            public void Execute(int index)
            {
                Entity building = buildingStats[index].entity;
                if (building != Entity.Null)
                {
                    if (transformLookup.TryGetComponent(building, out Game.Objects.Transform transform))
                    {
                        locations[index] = transform.m_Position;
                        return;
                    }
                }

                // No match. Assign the max value so it won't be mapped.（沒有符合的項目。填入極大值以避免被錯誤的輸出。）
                locations[index] = new(float.MaxValue);
            }
        }

        /// <summary>
        /// The job to map each building to districts.
        /// （映射每棟建築至行政區的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct MapBuildingsToDistrictsJob : IJobParallelFor
        {
            [ReadOnly]
            public ComponentLookup<CurrentDistrict> currentDistrictLookup;
            
            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [WriteOnly]
            public NativeParallelMultiHashMap<Entity, int>.ParallelWriter hashmap;

            public void Execute(int index)
            {
                Entity building = buildingStats[index].entity;
                if (currentDistrictLookup.TryGetComponent(building, out CurrentDistrict currentDistrict))
                {
                    if (currentDistrict.m_District != Entity.Null)
                    {
                        hashmap.Add(currentDistrict.m_District, index);
                    }
                }
            }
        }

        /// <summary>
        /// The job to retrieve map tiles' triangles.
        /// （獲得組成地圖區塊三角形的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct RetrieveTrianglesJob : IJobEntity
        {
            [WriteOnly]
            public NativeList<BVHUtils.Triangle> triangleList;

            public void Execute(in DynamicBuffer<Node> nodes, in DynamicBuffer<Triangle> triangles, Entity area)
            {
                for (int i = 0; i < triangles.Length; i++)
                {
                    BVHUtils.Triangle triangle = new(AreaUtils.GetTriangle2(nodes, triangles[i]), area);
                    triangleList.Add(triangle);
                }
            }
        }
    }
}