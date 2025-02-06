using Carto.Domain;
using Carto.IO;
using Colossal.Logging;
using Game;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Zones;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Carto.Systems
{
    /// <summary>
    /// The system that collects shared data across various systems.
    /// （收集多種系統所需之共享資料的系統。）
    /// </summary>
    public partial class SharedDataCollectionSystem : GameSystemBase
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The query to collect all asset pack prefabs.
        /// （收集所有資產包預製模板的查詢。）
        /// </summary>
        static EntityQuery _assetPackPrefabQuery;

        /// <summary>
        /// The query to collect all brand entities.
        /// （收集所有品牌實體的查詢。）
        /// </summary>
        static EntityQuery _brandQuery;

        /// <summary>
        /// The query to collect all building entities.
        /// （收集所有建築實體的查詢。）
        /// </summary>
        static EntityQuery _buildingQuery;

        /// <summary>
        /// The query to collect all citizen prefabs.
        /// （收集所有市民預製模板的查詢。）
        /// </summary>
        static EntityQuery _citizenPrefabQuery;

        /// <summary>
        /// The query to collect all spawnable building prefabs.
        /// （收集所有自長建築預製模板的查詢。）
        /// </summary>
        static EntityQuery _spawnableBuildingPrefabQuery;

        /// <summary>
        /// The query to collect all theme prefabs.
        /// （收集所有建築風格預製模板的查詢。）
        /// </summary>
        static EntityQuery _themePrefabQuery;

        /// <summary>
        /// The query to find time settings.
        /// （尋找時間設定的查詢。）
        /// </summary>
        static EntityQuery _timeDataQuery;

        /// <summary>
        /// The query to collect all zoning type prefabs. 
        /// （收集所有分區類別預製模板的查詢。）
        /// </summary>
        static EntityQuery _zoningPrefabQuery;

        /// <summary>
        /// The list of in-game brands' / enterprises' prefab name.
        /// （遊戲內品牌／企業預製模板名稱的列表。）
        /// </summary>
        public List<Brand> Brands => _brands;

        /// <summary>
        /// See <see cref="Brands"/>.
        /// </summary>
        private List<Brand> _brands;

        /// <summary>
        /// The map between brand entities and their index in <see cref="Brands"/>.<br/>
        /// （品牌／企業實體與其在 <see cref="Brands"/> 索引值的映射表。）
        /// </summary>
        public NativeParallelHashMap<Entity, int> BrandsEntityMap => _brandsEntityMap;

        /// <summary>
        /// See <see cref="BrandsEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _brandsEntityMap;

        /// <summary>
        /// The list of all building's statistics in the savegame.
        /// （遊戲存檔內所有建築的統計數據。）
        /// </summary>
        public NativeList<BuildingStat> BuildingStats => _buildingStats;

        /// <summary>
        /// See <see cref="BuildingStats"/>.
        /// </summary>
        private NativeList<BuildingStat> _buildingStats;

        /// <summary>
        /// The list of in-game themes' / asset packs' information.
        /// （遊戲內建築風格／資產包資訊的列表。）
        /// </summary>
        public List<Theme> Themes => _themes;

        /// <summary>
        /// See <see cref="Themes"/>.
        /// </summary>
        private List<Theme> _themes;

        /// <summary>
        /// The map between theme / asset pack prefab objects and their index in <see cref="Themes"/>.<br/>
        /// （建築風格／資產包預製模板物件與其在 <see cref="Themes"/> 索引值的映射表。）
        /// </summary>
        public Dictionary<PrefabBase, int> ThemesPrefabMap => _themesPrefabMap;

        /// <summary>
        /// See <see cref="ThemesPrefabMap"/>.
        /// </summary>
        private Dictionary<PrefabBase, int> _themesPrefabMap;

        /// <summary>
        /// The list of in-game zoning types' information.
        /// （遊戲內分區類型資訊的列表。）
        /// </summary>
        public NativeList<ZoningType> ZoningTypes => _zoningTypes;

        /// <summary>
        /// See <see cref="ZoningTypes"/>.
        /// </summary>
        private NativeList<ZoningType> _zoningTypes;

        /// <summary>
        /// The map between zoning prefab references and their index in <see cref="ZoningTypes"/> and <see cref="ZoningTypesNames"/>.<br/>
        /// （分區預製模板參考與其在 <see cref="ZoningTypes"/> 與 <see cref="ZoningTypesNames"/> 索引值的映射表。）
        /// </summary>
        public NativeParallelHashMap<Entity, int> ZoningTypesEntityMap => _zoningTypesEntityMap;

        /// <summary>
        /// See <see cref="ZoningTypesEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _zoningTypesEntityMap;

        /// <summary>
        /// The map between zoning ids and their index in <see cref="ZoningTypes"/> and <see cref="ZoningTypesNames"/>.<br/>
        /// （分區識別碼與其在 <see cref="ZoningTypes"/> 與 <see cref="ZoningTypesNames"/> 索引值的映射表。）
        /// </summary>
        public NativeParallelHashMap<ushort, int> ZoningTypesIdMap => _zoningTypesIdMap;

        /// <summary>
        /// See <see cref="ZoningTypesIdMap"/>.
        /// </summary>
        private NativeParallelHashMap<ushort, int> _zoningTypesIdMap;

        /// <summary>
        /// The list of in-game zoning types' prefab name.
        /// （遊戲內分區類型名稱的列表。）
        /// </summary>
        public NativeList<NativeText> ZoningTypesNames => _zoningTypesNames;

        /// <summary>
        /// See <see cref="ZoningTypesNames"/>.
        /// </summary>
        private NativeList<NativeText> _zoningTypesNames;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _assetPackPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<AssetPackData>(),
                    ComponentType.ReadOnly<PrefabData>()
                }
            });

            _brandQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<BrandData>()
                }
            });

            _buildingQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Building>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Placeholder>(),

                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _citizenPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<CitizenData>()
                }
            });

            _spawnableBuildingPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ObjectData>(),
                    ComponentType.ReadOnly<SpawnableBuildingData>(),
                }
            });

            _themePrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<PrefabData>(),
                    ComponentType.ReadOnly<ThemeData>()
                }
            });

            _timeDataQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<TimeData>()
                }
            });

            _zoningPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ZoneData>()
                }
            });

            base.OnCreate();
            _log.Debug("SharedDataCollectionSystem instance created. 共享資料收集系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy()
        {
            Dispose();
            base.OnDestroy();
        }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Try disposing of all properties stored in unmanaged memory.
        /// （嘗試丟棄儲存於未控管記憶體的屬性。）
        /// </summary>
        public void Dispose()
        {
            Utils.CommonUtils.Dispose(ref _brandsEntityMap);
            Utils.CommonUtils.Dispose(ref _buildingStats);
            Utils.CommonUtils.Dispose(ref _zoningTypes);
            Utils.CommonUtils.Dispose(ref _zoningTypesEntityMap);
            Utils.CommonUtils.Dispose(ref _zoningTypesIdMap);
            Utils.CommonUtils.Dispose(ref _zoningTypesNames);
        }

        /// <summary>
        /// Retrieve brand's information.
        /// （獲取品牌的資訊。）
        /// </summary>
        private void GetBrands()
        {
            // Create local copy of properties.（創造屬性的區域副本。）
            ref List<Brand> brands = ref _brands;
            ref NativeParallelHashMap<Entity, int> entityMap = ref _brandsEntityMap;

            // Initialize native containers.（初始化原生容器。）
            NativeArray<Entity> brandEntities = _brandQuery.ToEntityArray(Allocator.Temp);

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset<List<Brand>, Brand>(ref brands);
            Utils.CommonUtils.Reset(ref entityMap, brandEntities.Length);

            // Add the fallback brand.（添加後備品牌。）
            brands.Add(new() { entity = Entity.Null, name = string.Empty });

            // Collect brands.（收集品牌。）
            for (int i = 0; i < brandEntities.Length; i++)
            {
                Entity brand = brandEntities[i];
                Brand data = new()
                {
                    entity = brand,
                    name = Instance.Prefab.GetPrefabName(brand)
                };
                brands.Add(data);
                entityMap.Add(brand, brands.Count - 1);
            }
        }

        /// <summary>
        /// Retrieve building entities' statistical data.
        /// （獲取建築實體的統計資料。）
        /// </summary>
        /// <param name="option">The export options.（檔案輸出選項。）</param>
        public void GetBuildingStats(Options option)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref List<Brand> brands = ref _brands;
            ref List<Theme> themes = ref _themes;
            ref NativeList<BuildingStat> stats = ref _buildingStats;
            ref NativeList<ZoningType> zonings = ref _zoningTypes;
            ref NativeList<NativeText> zoningsNames = ref _zoningTypesNames;
            ref NativeParallelHashMap<Entity, int> brandsEntityMap = ref _brandsEntityMap;
            ref NativeParallelHashMap<Entity, int> zoningsEntityMap = ref _zoningTypesEntityMap;

            // Collect brands.（收集品牌。）
            if (option.Contains(Property.Brand))
            {
                GetBrands();
            }
            else
            {
                brands = new() { new() { entity = Entity.Null, name = string.Empty } };
                Utils.CommonUtils.Reset(ref brandsEntityMap, 1);
            }

            // Collect zoning types.（收集分區類型。）
            if
            (
                option.ContainsAny(Property.Theme, Property.Zoning) ||
                option.ContainsAny(IO.System.Zoning, Property.Category, Property.Color, Property.Density, Property.Name)
            )
            {
                GetZoningTypes(option);
            }
            else
            {
                themes = new() { new() { entity = Entity.Null, name = "Carto Generic" } };
                Utils.CommonUtils.Reset(ref zonings, 1);
                Utils.CommonUtils.Reset(ref zoningsEntityMap, 1);
                Utils.CommonUtils.Reset(ref zoningsNames, 1);
            }
            zonings.Add(new()
            {
                entity = Entity.Null,
                category = ZoningCategory.None,
                color = new(),
                density = ZoningDensity.Generic,
                id = 0,
                prefabData = new(),
                theme = 0
            });
            zoningsNames.Add(new("Empty", Allocator.Persistent));

            // Reset output containers.（重置輸出容器。）
            int buildingEntityCount = _buildingQuery.CalculateEntityCount();
            Utils.CommonUtils.Reset(ref stats, buildingEntityCount);

            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, bool> sexEntityMap = new(_citizenPrefabQuery.CalculateEntityCount(), Allocator.Persistent);

            try
            {
                // Collect the sex of each citizen prefab.（收集各種市民預製模板的生理性別。）
                CollectCitizenSexJob collectSexJob = new()
                {
                    hashmap = sexEntityMap.AsParallelWriter()
                };
                JobHandle collectSexHandle = collectSexJob.ScheduleParallel(_citizenPrefabQuery, default);
                collectSexHandle.Complete();

                // Retrieve the current time frame.（獲得目前的時間幀。）
                TimeData timeData = default;
                if (_timeDataQuery.TryGetSingleton(out TimeData singleton))
                {
                    timeData = singleton;
                }

                // Collect the statistics of each building.（收集各個建築的統計資料。）
                CollectBuildingStatsJob collectStatsJob = new()
                {
                    citizenBufferLookup = GetBufferLookup<HouseholdCitizen>(true),
                    employeeBufferLookup = GetBufferLookup<Employee>(true),
                    renterBufferLookup = GetBufferLookup<Renter>(true),
                    citizenLookup = GetComponentLookup<Citizen>(true),
                    companyDataLookup = GetComponentLookup<CompanyData>(true),
                    healthProblemLookup = GetComponentLookup<HealthProblem>(true),
                    householdLookup = GetComponentLookup<Household>(true),
                    prefabRefLookup = GetComponentLookup<PrefabRef>(true),
                    processLookup = GetComponentLookup<IndustrialProcessData>(true),
                    spawnableDataLookup = GetComponentLookup<SpawnableBuildingData>(true),
                    travelPurposeLookup = GetComponentLookup<TravelPurpose>(true),
                    emptyZoningTypeIndex = zonings.Length - 1,
                    currentFrameIndex = Instance.Simulation.frameIndex,
                    initialTime = timeData,
                    brandEntityMap = brandsEntityMap,
                    sexEntityMap = sexEntityMap,
                    zoningEntityMap = zoningsEntityMap,
                    list = stats.AsParallelWriter()
                };
                JobHandle collectStatsHandle = collectStatsJob.ScheduleParallel(_buildingQuery, default);
                collectStatsHandle.Complete();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref brandsEntityMap);
                Utils.CommonUtils.Dispose(ref sexEntityMap);
                Utils.CommonUtils.Dispose(ref zoningsEntityMap);
            }
        }

        /// <summary>
        /// The job to collect building statistics.
        /// （收集建築統計資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectBuildingStatsJob : IJobEntity
        {
            [ReadOnly]
            public BufferLookup<HouseholdCitizen> citizenBufferLookup;

            [ReadOnly]
            public BufferLookup<Employee> employeeBufferLookup;

            [ReadOnly]
            public BufferLookup<Renter> renterBufferLookup;

            [ReadOnly]
            public ComponentLookup<Citizen> citizenLookup;

            [ReadOnly]
            public ComponentLookup<CompanyData> companyDataLookup;

            [ReadOnly]
            public ComponentLookup<HealthProblem> healthProblemLookup;

            [ReadOnly]
            public ComponentLookup<Household> householdLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<IndustrialProcessData> processLookup;

            [ReadOnly]
            public ComponentLookup<SpawnableBuildingData> spawnableDataLookup;

            [ReadOnly]
            public ComponentLookup<TravelPurpose> travelPurposeLookup;

            [ReadOnly]
            public int emptyZoningTypeIndex;

            [ReadOnly]
            public uint currentFrameIndex;

            [ReadOnly]
            public TimeData initialTime;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> brandEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, bool> sexEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> zoningEntityMap;

            [WriteOnly]
            public NativeList<BuildingStat>.ParallelWriter list;

            public void Execute(in PrefabRef prefabRef, Entity building)
            {
                // Whether the first shop is recorded or not.（第一間商店是否被記錄了？）
                bool isFirstShop = true;

                BuildingStat stat = new()
                {
                    entity = building,
                    age = 0f,
                    brand = 0,
                    company = 0,
                    employee = 0,
                    household = 0,
                    level = 0,
                    product = Resource.NoResource,
                    residentFemale = 0,
                    residentMale = 0,
                    zoning = emptyZoningTypeIndex
                };

                if (employeeBufferLookup.TryGetBuffer(building, out DynamicBuffer<Employee> employeeBuffer))
                {
                    stat.employee += employeeBuffer.Length;
                }

                if (renterBufferLookup.TryGetBuffer(building, out DynamicBuffer<Renter> renterBuffer))
                {
                    for (int i = 0; i < renterBuffer.Length; i++)
                    {
                        Entity renter = renterBuffer[i].m_Renter;

                        if (companyDataLookup.TryGetComponent(renter, out CompanyData companyData))
                        {
                            stat.company++;

                            if (isFirstShop)
                            {
                                if (brandEntityMap.TryGetValue(companyData.m_Brand, out int brandIndex))
                                {
                                    stat.brand = brandIndex;
                                }
                                stat.product = processLookup[prefabRefLookup[renter].m_Prefab].m_Output.m_Resource;
                                isFirstShop = false;
                            }
                        }

                        if (employeeBufferLookup.TryGetBuffer(renter, out DynamicBuffer<Employee> employeeBufferPerRenter))
                        {
                            stat.employee += employeeBufferPerRenter.Length;
                        }

                        if (householdLookup.HasComponent(renter))
                        {
                            stat.household++;
                        }

                        if (citizenBufferLookup.TryGetBuffer(renter, out DynamicBuffer<HouseholdCitizen> citizenBuffer))
                        {
                            for (int j = 0; j < citizenBuffer.Length; j++)
                            {
                                Entity citizen = citizenBuffer[j].m_Citizen;
                                if (IsCitizenAlive(citizen))
                                {
                                    if (!sexEntityMap.TryGetValue(prefabRefLookup[citizen].m_Prefab, out bool isMale))
                                    {
                                        continue;
                                    }

                                    if (!citizenLookup.TryGetComponent(citizen, out Citizen citizenComponent))
                                    {
                                        continue;
                                    }

                                    stat.age += citizenComponent.GetAgeInDays(currentFrameIndex, initialTime);

                                    if (isMale)
                                    {
                                        stat.residentMale++;
                                    }
                                    else
                                    {
                                        stat.residentFemale++;
                                    }
                                }
                            }
                        }
                    }
                }

                if (spawnableDataLookup.TryGetComponent(prefabRef.m_Prefab, out SpawnableBuildingData spawnableData))
                {
                    if (zoningEntityMap.TryGetValue(spawnableData.m_ZonePrefab, out int zoningIndex))
                    {
                        stat.zoning = zoningIndex;
                    }
                }

                list.AddNoResize(stat);
            }

            private bool IsCitizenAlive(Entity citizen)
            {
                if (healthProblemLookup.HasComponent(citizen))
                {
                    if ((healthProblemLookup[citizen].m_Flags & HealthProblemFlags.Dead) != 0)
                    {
                        return false;
                    }
                }

                if (travelPurposeLookup.HasComponent(citizen))
                {
                    Purpose purpose = travelPurposeLookup[citizen].m_Purpose;
                    return (purpose != Purpose.Deathcare) & (purpose != Purpose.InDeathcare);
                }

                return true;
            }
        }

        /// <summary>
        /// The job to collect citizen prefab sexes.
        /// （收集市民生理性別資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectCitizenSexJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, bool>.ParallelWriter hashmap;

            public void Execute(in CitizenData citizenData, Entity citizen)
            {
                hashmap.TryAdd(citizen, citizenData.m_Male);
            }
        }

        /// <summary>
        /// Retrieve theme / asset pack's information.
        /// （獲取建築風格／資產包的資訊。）
        /// </summary>
        /// <param name="option">The export options.（檔案輸出選項。）</param>
        private void GetThemes(Options option)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref List<Theme> themes = ref _themes;
            ref Dictionary<PrefabBase, int> prefabMap = ref _themesPrefabMap;
            
            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset<List<Theme>, Theme>(ref themes);
            Utils.CommonUtils.Reset<Dictionary<PrefabBase, int>, PrefabBase, int>(ref prefabMap);

            // Add the fallback theme.（添加後備建築風格。）
            themes.Add(new() { entity = Entity.Null, name = "Carto Generic" });

            // Collect building themes.（收集建築風格。）
            NativeArray<Entity> themeEntities = _themePrefabQuery.ToEntityArray(Allocator.Temp);
            NativeArray<PrefabData> themePrefabs = _themePrefabQuery.ToComponentDataArray<PrefabData>(Allocator.Temp);
            for (int i = 0; i < themeEntities.Length; i++)
            {
                Entity theme = themeEntities[i];
                PrefabBase themePrefab = Instance.Prefab.GetPrefab<PrefabBase>(themePrefabs[i]);
                Theme data = new()
                {
                    entity = theme,
                    name = Instance.Prefab.GetPrefabName(theme),
                };
                themes.Add(data);
                prefabMap.Add(themePrefab, themes.Count - 1);
            }

            // Collect asset packs.（收集資產包。）
            if (option.AssetPack)
            {
                NativeArray<Entity> assetPacks = _assetPackPrefabQuery.ToEntityArray(Allocator.Temp);
                NativeArray<PrefabData> assetPackPrefabs = _assetPackPrefabQuery.ToComponentDataArray<PrefabData>(Allocator.Temp);
                for (int i = 0; i < assetPacks.Length; i++)
                {
                    Entity assetPack = assetPacks[i];
                    PrefabBase assetPackPrefab = Instance.Prefab.GetPrefab<PrefabBase>(assetPackPrefabs[i]);
                    Theme data = new()
                    {
                        entity = assetPack,
                        name = Instance.Prefab.GetPrefabName(assetPack)
                    };
                    themes.Add(data);
                    prefabMap.Add(assetPackPrefab, themes.Count - 1);
                }
            }
        }

        /// <summary>
        /// Retrieve zoning types' information.
        /// （獲取分區類別的資訊。）
        /// </summary>
        /// <param name="option">The export options.（檔案輸出選項。）</param>
        public void GetZoningTypes(Options option)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref NativeParallelHashMap<Entity, int> entityMap = ref _zoningTypesEntityMap;
            ref NativeParallelHashMap<ushort, int> idMap = ref _zoningTypesIdMap;
            ref NativeList<NativeText> names = ref _zoningTypesNames;
            ref NativeList<ZoningType> types = ref _zoningTypes;

            // Initialize native containers.（初始化原生容器。）
            int zoningTypeCount = _zoningPrefabQuery.CalculateEntityCount();
            NativeParallelHashSet<Entity> zoningTypePool = new(zoningTypeCount, Allocator.TempJob);

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset(ref entityMap, zoningTypeCount);
            Utils.CommonUtils.Reset(ref idMap, zoningTypeCount);
            Utils.CommonUtils.Reset(ref names, zoningTypeCount);
            Utils.CommonUtils.Reset(ref types, zoningTypeCount);

            try
            {
                // Collect zoning types by looking at all spawanable building prefabs.（透過檢查所有自長建築預製模板收集分區類型。）
                CollectZoningTypesJob collectJob = new()
                {
                    prefabDataLookup = GetComponentLookup<PrefabData>(),
                    zoneDataLookup = GetComponentLookup<ZoneData>(),
                    list = types.AsParallelWriter(),
                    zoningTypePool = zoningTypePool.AsParallelWriter(),
                    commercialIndex = TypeManager.GetTypeIndex<CommercialProperty>(),
                    industrialIndex = TypeManager.GetTypeIndex<IndustrialProperty>(),
                    officeIndex = TypeManager.GetTypeIndex<OfficeProperty>(),
                    residentialIndex = TypeManager.GetTypeIndex<ResidentialProperty>()
                };
                JobHandle collectHandle = collectJob.ScheduleParallel(_spawnableBuildingPrefabQuery, default);
                collectHandle.Complete();

                // Ensure to collect zoning types without buildings (ex. Unzoned).（確保收集到沒有建築的分區類型，例如無分區類型。）
                VerifyZoningTypesJob verifyJob = new()
                {
                    list = types.AsParallelWriter(),
                    zoningTypePool = zoningTypePool
                };
                JobHandle verifyHandle = verifyJob.ScheduleParallel(_zoningPrefabQuery, default);
                verifyHandle.Complete();

                // Prepare themes / asset packs information.（準備建築風格／資產包資訊。）
                GetThemes(option);

                // Add the data that can only be retrieved in the main thread.（添加只能在主執行緒取得的資料。）
                for (int index = 0; index < zoningTypeCount; index++)
                {
                    ref ZoningType zoningType = ref types.ElementAt(index);
                    ZonePrefab zonePrefabData = Instance.Prefab.GetPrefab<ZonePrefab>(zoningType.prefabData);
                    zoningType.color = zonePrefabData.m_Color;

                    if (zonePrefabData.Has<AssetPackItem>())
                    {
                        if (_themesPrefabMap.TryGetValue(zonePrefabData.GetComponent<AssetPackItem>().m_Packs[0], out int themeIndex))
                        {
                            zoningType.theme = themeIndex;
                        }
                    }

                    if (zonePrefabData.Has<ThemeObject>())
                    {
                        if (_themesPrefabMap.TryGetValue(zonePrefabData.GetComponent<ThemeObject>().m_Theme, out int themeIndex))
                        {
                            zoningType.theme = themeIndex;
                        }
                    }

                    entityMap.TryAdd(zoningType.entity, index);
                    idMap.TryAdd(zoningType.id, index);
                    names.Add(new NativeText(Instance.Prefab.GetPrefabName(zoningType.entity), Allocator.Persistent));
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                if (zoningTypePool.IsCreated)
                {
                    zoningTypePool.Dispose();
                }

                _themesPrefabMap.Clear();
            }
        }

        /// <summary>
        /// The job to collect zoning types.
        /// （收集分區類別的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectZoningTypesJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<PrefabData> prefabDataLookup;

            [ReadOnly]
            public ComponentLookup<ZoneData> zoneDataLookup;

            [WriteOnly]
            public NativeList<ZoningType>.ParallelWriter list;

            [WriteOnly]
            public NativeParallelHashSet<Entity>.ParallelWriter zoningTypePool;

            [ReadOnly]
            public TypeIndex commercialIndex;

            [ReadOnly]
            public TypeIndex industrialIndex;

            [ReadOnly]
            public TypeIndex officeIndex;

            [ReadOnly]
            public TypeIndex residentialIndex;

            public void Execute(in ObjectData objectData, in SpawnableBuildingData spawnableData)
            {
                NativeArray<ComponentType> archetypeComponents = default;
                try
                {
                    // Check whether the prefab is already recorded.（確認預製模板是否已被記錄過。）
                    Entity zoningPrefab = spawnableData.m_ZonePrefab;
                    if (!zoningTypePool.Add(zoningPrefab))
                    {
                        return;
                    }

                    // Find out categories.（找出分類。）
                    ZoningCategory categories = ZoningCategory.None;
                    archetypeComponents = objectData.m_Archetype.GetComponentTypes(Allocator.Temp);
                    for (int j = 0; j < archetypeComponents.Length; j++)
                    {
                        TypeIndex componentIndex = archetypeComponents[j].TypeIndex;
                        if (componentIndex == commercialIndex) categories |= ZoningCategory.Commercial;
                        if (componentIndex == industrialIndex) categories |= ZoningCategory.Industrial;
                        if (componentIndex == officeIndex) categories |= ZoningCategory.Office;
                        if (componentIndex == residentialIndex) categories |= ZoningCategory.Residential;
                    }

                    // Remove the industrial zoning from the office zoning.（從辦公分區中移除工業分區。）
                    if ((categories & ZoningCategory.Office) != 0)
                    {
                        categories &= ~ZoningCategory.Industrial;
                    }

                    // Find out density.（找出發展強度。）
                    ZoningDensity density = GetZoningDensity(categories, zoneDataLookup[zoningPrefab]);

                    // Find out id.（找出識別碼。）
                    ushort id = zoneDataLookup[zoningPrefab].m_ZoneType.m_Index;

                    // Insert data.（插入資料。）
                    ZoningType data = new()
                    {
                        entity = zoningPrefab,
                        category = categories,
                        color = new(),
                        density = density,
                        id = id,
                        prefabData = prefabDataLookup[zoningPrefab],
                        theme = 0
                    };
                    list.AddNoResize(data);
                }
                finally
                {
                    if (archetypeComponents.IsCreated)
                    {
                        archetypeComponents.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// The job to verify zoning types integrity.
        /// （驗證分區類別完整性的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct VerifyZoningTypesJob : IJobEntity
        {
            [WriteOnly]
            public NativeList<ZoningType>.ParallelWriter list;

            [ReadOnly]
            public NativeParallelHashSet<Entity> zoningTypePool;

            public void Execute(in PrefabData prefabData, in ZoneData zoneData, Entity zoningPrefab)
            {
                if (!zoningTypePool.Contains(zoningPrefab))
                {
                    // Find out categories.（找出分類。）
                    ZoningCategory categories = zoneData.m_AreaType switch
                    {
                        AreaType.Residential => ZoningCategory.Residential,
                        AreaType.Commercial => ZoningCategory.Commercial,
                        AreaType.Industrial => ZoningCategory.Industrial,
                        _ => ZoningCategory.None,
                    };

                    // Remove the industrial zoning from the office zoning.（從辦公分區中移除工業分區。）
                    if ((zoneData.m_ZoneFlags & ZoneFlags.Office) != 0)
                    {
                        categories |= ZoningCategory.Office;
                        categories &= ~ZoningCategory.Industrial;
                    }

                    // Find out density.（找出發展強度。）
                    ZoningDensity density = GetZoningDensity(categories, zoneData);

                    // Find out id.（找出識別碼。）
                    ushort id = zoneData.m_ZoneType.m_Index;

                    // Insert data.（插入資料。）
                    ZoningType data = new()
                    {
                        entity = zoningPrefab,
                        category = categories,
                        color = new(),
                        density = density,
                        id = id,
                        prefabData = prefabData,
                        theme = 0
                    };
                    list.AddNoResize(data);
                }
            }
        }

        /// <summary>
        /// Get zoning density from its categories and ZoneData component.
        /// （由分區分類與 ZoneData 部件獲得其發展強度。）
        /// </summary>
        /// <param name="categories">The flag indicating zoning categories.（顯示分區分類的旗標。）</param>
        /// <param name="zoneData">The ZoneData component.（ZoneData 部件。）</param>
        /// <returns>A flag indicating zoning density.（顯示分區發展強度的旗標。）</returns>
        private static ZoningDensity GetZoningDensity(ZoningCategory categories, ZoneData zoneData)
        {
            ZoningDensity density = ZoningDensity.Generic;
            ushort heightLimit = zoneData.m_MaxHeight;

            if ((categories & ZoningCategory.Residential) != 0)
            {
                density = heightLimit switch
                {
                    < 12 => ZoningDensity.Low,
                    < 60 => ZoningDensity.Medium,
                    _ => ZoningDensity.High
                };
            }
            else if ((categories & ZoningCategory.Commercial) != 0 | (categories & ZoningCategory.Office) != 0)
            {
                density = heightLimit < 20 ? ZoningDensity.Low : ZoningDensity.High;
            }

            return density;
        }
    }
}