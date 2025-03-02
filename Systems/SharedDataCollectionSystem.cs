using Carto.Domain;
using Carto.IO;
using Colossal.Logging;
using Game;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

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
        /// The system managing the terrain.（管理地形的系統。）<br/>
        /// See <see cref="Instance.Terrain"/> for more information.
        /// </summary>
        static readonly TerrainSystem _terrain = Instance.Terrain;

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
        /// The query to collect all company entities.
        /// （收集所有公司實體的查詢。）
        /// </summary>
        static EntityQuery _companyQuery;

        /// <summary>
        /// The query to collect basic economic settings.
        /// （收集基本經濟設定的查詢。）
        /// </summary>
        static EntityQuery _economyParameterQuery;

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
        public ref List<Brand> Brands => ref _brands;

        /// <summary>
        /// See <see cref="Brands"/>.
        /// </summary>
        private List<Brand> _brands;

        /// <summary>
        /// The map between brand entities and their index in <see cref="Brands"/>.<br/>
        /// （品牌／企業實體與其在 <see cref="Brands"/> 索引值的映射表。）
        /// </summary>
        public ref NativeParallelHashMap<Entity, int> BrandsEntityMap => ref _brandsEntityMap;

        /// <summary>
        /// See <see cref="BrandsEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _brandsEntityMap;

        /// <summary>
        /// The list of all building's statistics in the savegame.
        /// （遊戲存檔內所有建築的統計數據。）
        /// </summary>
        public ref NativeList<BuildingStat> BuildingStats => ref _buildingStats;

        /// <summary>
        /// See <see cref="BuildingStats"/>.
        /// </summary>
        private NativeList<BuildingStat> _buildingStats;
         
        /// <summary>
        /// The list of in-game themes' / asset packs' information.
        /// （遊戲內建築風格／資產包資訊的列表。）
        /// </summary>
        public ref List<Theme> Themes => ref _themes;

        /// <summary>
        /// See <see cref="Themes"/>.
        /// </summary>
        private List<Theme> _themes;

        /// <summary>
        /// The map between theme / asset pack prefab objects and their index in <see cref="Themes"/>.<br/>
        /// （建築風格／資產包預製模板物件與其在 <see cref="Themes"/> 索引值的映射表。）
        /// </summary>
        public ref Dictionary<PrefabBase, int> ThemesPrefabMap => ref _themesPrefabMap;

        /// <summary>
        /// See <see cref="ThemesPrefabMap"/>.
        /// </summary>
        private Dictionary<PrefabBase, int> _themesPrefabMap;

        /// <summary>
        /// The elevation grid of the world heightmap.<br/>
        /// （世界高度圖的網格。）
        /// </summary>
        public ref NativeArray<ushort> WorldElevation => ref _worldElevation;

        /// <summary>
        /// See <see cref="WorldElevation"/>.
        /// </summary>
        private NativeArray<ushort> _worldElevation;

        /// <summary>
        /// The list of in-game zoning types' information.
        /// （遊戲內分區類型資訊的列表。）
        /// </summary>
        public ref NativeList<ZoningType> ZoningTypes => ref _zoningTypes;

        /// <summary>
        /// See <see cref="ZoningTypes"/>.
        /// </summary>
        private NativeList<ZoningType> _zoningTypes;

        /// <summary>
        /// The map between zoning prefab references and their index in <see cref="ZoningTypes"/> and <see cref="ZoningTypesNames"/>.<br/>
        /// （分區預製模板參考與其在 <see cref="ZoningTypes"/> 與 <see cref="ZoningTypesNames"/> 索引值的映射表。）
        /// </summary>
        public ref NativeParallelHashMap<Entity, int> ZoningTypesEntityMap => ref _zoningTypesEntityMap;

        /// <summary>
        /// See <see cref="ZoningTypesEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _zoningTypesEntityMap;

        /// <summary>
        /// The map between zoning ids and their index in <see cref="ZoningTypes"/> and <see cref="ZoningTypesNames"/>.<br/>
        /// （分區識別碼與其在 <see cref="ZoningTypes"/> 與 <see cref="ZoningTypesNames"/> 索引值的映射表。）
        /// </summary>
        public ref NativeParallelHashMap<ushort, int> ZoningTypesIdMap => ref _zoningTypesIdMap;

        /// <summary>
        /// See <see cref="ZoningTypesIdMap"/>.
        /// </summary>
        private NativeParallelHashMap<ushort, int> _zoningTypesIdMap;

        /// <summary>
        /// The list of in-game zoning types' prefab name.
        /// （遊戲內分區類型名稱的列表。）
        /// </summary>
        public ref NativeList<NativeText> ZoningTypesNames => ref _zoningTypesNames;

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

            _companyQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<CompanyData>(),
                    ComponentType.ReadOnly<Employee>(),
                    ComponentType.ReadOnly<Game.Economy.Resources>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _economyParameterQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<EconomyParameterData>()
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
            Utils.CommonUtils.Dispose(ref _worldElevation);
            Utils.CommonUtils.Dispose(ref _zoningTypes);
            Utils.CommonUtils.Dispose(ref _zoningTypesEntityMap);
            Utils.CommonUtils.Dispose(ref _zoningTypesIdMap);
            Utils.CommonUtils.Dispose(ref _zoningTypesNames);
        }

        /// <summary>
        /// Try disposing of the specific properties set in unmanaged memory.
        /// （嘗試丟棄儲存於未控管記憶體的特定屬性。）
        /// </summary>
        /// <param name="lifeCycle">The phase to dispose of specific properties.（丟棄特定屬性的階段。）</param>
        public void Dispose(DisposePhase lifeCycle)
        {
            switch (lifeCycle)
            {
                case DisposePhase.AfterAreaSystem:
                    // The following properties are disposed of after area system finishes its work, since they are required for calculating statistics.
                    //（以下屬性在區域系統完成工作後丟棄，因為它們被用於計算區域統計資訊。）
                    Utils.CommonUtils.Dispose(ref _buildingStats);

                    // Manually call the dispose for AfterBuildingSystem, in case of the situation that the system is not used.
                    // （手動呼叫 AfterBuildingSystem 的拋棄指令，以避免該系統並未被使用。）
                    Dispose(DisposePhase.AfterBuildingSystem);
                    break;

                case DisposePhase.AfterBuildingStats:
                    Utils.CommonUtils.Dispose(ref _brandsEntityMap);
                    break;

                case DisposePhase.AfterBuildingSystem:
                    // The following properties are disposed of after building system finishes its work, since they are required for the zoning field.
                    // （以下屬性在建築系統完成工作後丟棄，因為 zoning 欄位會用到它們。）
                    Utils.CommonUtils.Dispose(ref _zoningTypes);
                    Utils.CommonUtils.Dispose(ref _zoningTypesEntityMap);

                    // Manually call the dispose for AfterZoningSystem, in case of the situation that the system is not used.
                    // （手動呼叫 AfterZoningSystem 的拋棄指令，以避免該系統並未被使用。）
                    Dispose(DisposePhase.AfterZoningSystem);
                    break;

                case DisposePhase.AfterTerrainRelated:
                    Utils.CommonUtils.Dispose(ref _worldElevation);
                    break;

                case DisposePhase.AfterZoningSystem:
                    Utils.CommonUtils.Dispose(ref _zoningTypesIdMap);
                    Utils.CommonUtils.Dispose(ref _zoningTypesNames);
                    break;

                default:
                    break;
            }
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
        /// <param name="options">The export options.（檔案輸出選項。）</param>
        public void GetBuildingStats(Options options)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref List<Brand> brands = ref _brands;
            ref List<Theme> themes = ref _themes;
            ref NativeList<BuildingStat> stats = ref _buildingStats;
            ref NativeList<ZoningType> zonings = ref _zoningTypes;
            ref NativeList<NativeText> zoningsNames = ref _zoningTypesNames;
            ref NativeParallelHashMap<Entity, int> brandsEntityMap = ref _brandsEntityMap;
            ref NativeParallelHashMap<Entity, int> zoningsEntityMap = ref _zoningTypesEntityMap;

            // Export options.（輸出設定。）
            bool hasAge = options.Contains(Property.Age);
            bool hasBrand = options.Contains(Property.Brand);
            bool hasPopulation = options.ContainsAny(Property.Age, Property.Labor, Property.Resident, Property.SexRatio, Property.Wage);
            bool hasWage = options.Contains(Property.Wage);
            bool hasZoning = options.ContainsAny(Property.Theme, Property.Zoning) ||
                             options.ContainsAny(IO.System.Zoning, Property.Category, Property.Color, Property.Density, Property.Name);

            // Collect brands.（收集品牌。）
            if (hasBrand)
            {
                GetBrands();
            }
            else
            {
                brands = new() { new() { entity = Entity.Null, name = string.Empty } };
                Utils.CommonUtils.Reset(ref brandsEntityMap, 1);
            }

            // Collect zoning types.（收集分區類型。）
            if (hasZoning)
            {
                GetZoningTypes(options);
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
            NativeParallelHashMap<Entity, int> dividendEntityMap = new(_companyQuery.CalculateEntityCount(), Allocator.Persistent);
            NativeParallelHashMap<Entity, bool> sexEntityMap = new(_citizenPrefabQuery.CalculateEntityCount(), Allocator.Persistent);

            try
            {
                // Collect the dividend of each company.（收集各公司的員工分紅。）
                if (hasWage)
                {
                    CollectCompanyDividendsJob collectDividendJob = new()
                    {
                        hashmap = dividendEntityMap.AsParallelWriter()
                    };
                    JobHandle collectDividendHandle = collectDividendJob.ScheduleParallel(_companyQuery, default);
                    collectDividendHandle.Complete();
                }
                
                // Collect the sex of each citizen prefab.（收集各種市民預製模板的生理性別。）
                if (hasPopulation)
                {
                    CollectCitizenSexJob collectSexJob = new()
                    {
                        hashmap = sexEntityMap.AsParallelWriter()
                    };
                    JobHandle collectSexHandle = collectSexJob.ScheduleParallel(_citizenPrefabQuery, default);
                    collectSexHandle.Complete();
                }

                // Retrieve the basic economy parameters.（獲得基本經濟參數。）
                EconomyParameterData economyParameterData = default;
                if (hasWage)
                {
                    if (_economyParameterQuery.TryGetSingleton(out EconomyParameterData economyParameter))
                    {
                        economyParameterData = economyParameter;
                    }
                }

                // Retrieve the current time frame.（獲得目前的時間幀。）
                TimeData timeData = default;
                uint currentFrame = default;
                if (hasAge)
                {
                    if (_timeDataQuery.TryGetSingleton(out TimeData singleton))
                    {
                        timeData = singleton;
                    }
                    currentFrame = Instance.Simulation.frameIndex;
                }

                // Collect the statistics of each building.（收集各個建築的統計資料。）
                CollectBuildingStatsJob collectStatsJob = new()
                {
                    countHomeless = options.Homeless,
                    taxableIncomeOnly = options.Taxable,
                    citizenBufferLookup = GetBufferLookup<HouseholdCitizen>(true),
                    employeeBufferLookup = GetBufferLookup<Employee>(true),
                    renterBufferLookup = GetBufferLookup<Renter>(true),
                    citizenLookup = GetComponentLookup<Citizen>(true),
                    companyDataLookup = GetComponentLookup<CompanyData>(true),
                    healthProblemLookup = GetComponentLookup<HealthProblem>(true),
                    homelessHouseholdLookup = GetComponentLookup<HomelessHousehold>(true),
                    householdLookup = GetComponentLookup<Household>(true),
                    prefabRefLookup = GetComponentLookup<PrefabRef>(true),
                    processLookup = GetComponentLookup<IndustrialProcessData>(true),
                    spawnableDataLookup = GetComponentLookup<SpawnableBuildingData>(true),
                    taxPayerLookup = GetComponentLookup<TaxPayer>(true),
                    travelPurposeLookup = GetComponentLookup<TravelPurpose>(true),
                    workerLookup = GetComponentLookup<Worker>(true),
                    economyParameter = economyParameterData,
                    emptyZoningTypeIndex = zonings.Length - 1,
                    currentFrameIndex = currentFrame,
                    initialTime = timeData,
                    brandEntityMap = brandsEntityMap,
                    dividendEntityMap = dividendEntityMap,
                    sexEntityMap = sexEntityMap,
                    zoningEntityMap = zoningsEntityMap,
                    list = stats.AsParallelWriter(),
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
                Utils.CommonUtils.Dispose(ref dividendEntityMap);
                Utils.CommonUtils.Dispose(ref sexEntityMap);
                Dispose(DisposePhase.AfterBuildingStats); // brandsEntityMap
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
            public bool countHomeless;
            
            [ReadOnly]
            public bool taxableIncomeOnly;
            
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
            public ComponentLookup<HomelessHousehold> homelessHouseholdLookup;

            [ReadOnly]
            public ComponentLookup<Household> householdLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<IndustrialProcessData> processLookup;

            [ReadOnly]
            public ComponentLookup<SpawnableBuildingData> spawnableDataLookup;

            [ReadOnly]
            public ComponentLookup<TaxPayer> taxPayerLookup;

            [ReadOnly]
            public ComponentLookup<TravelPurpose> travelPurposeLookup;

            [ReadOnly]
            public ComponentLookup<Worker> workerLookup;

            [ReadOnly]
            public EconomyParameterData economyParameter;

            [ReadOnly]
            public int emptyZoningTypeIndex;

            [ReadOnly]
            public uint currentFrameIndex;

            [ReadOnly]
            public TimeData initialTime;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> brandEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> dividendEntityMap;

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
                    profit = 0,
                    residentFemale = 0,
                    residentMale = 0,
                    wage = 0,
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
                        bool recordable = countHomeless || !homelessHouseholdLookup.HasComponent(renter);

                        if (companyDataLookup.TryGetComponent(renter, out CompanyData companyData))
                        {
                            stat.company++;

                            if (taxPayerLookup.TryGetComponent(renter, out TaxPayer taxData))
                            {
                                // Commercial / industiral taxes are collected 32 times each day, so the value is estimated.（商業／工業稅每天稽徵 32 次，因此金額為估計值。）
                                // As of the version 1.2.3f1, warehousing companies seem to have full tax exemption.（截至 1.2.3f1 版本，倉儲業似乎完全免稅。）
                                stat.profit = taxData.m_UntaxedIncome * TaxSystem.kUpdatesPerDay;
                            }

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

                        if (householdLookup.HasComponent(renter) && recordable)
                        {
                            stat.household++;
                        }

                        if (citizenBufferLookup.TryGetBuffer(renter, out DynamicBuffer<HouseholdCitizen> citizenBuffer) && recordable)
                        {
                            for (int j = 0; j < citizenBuffer.Length; j++)
                            {
                                Entity citizen = citizenBuffer[j].m_Citizen;
                                if (!IsCitizenAlive(citizen))
                                {
                                    continue;
                                }

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

                                if (workerLookup.TryGetComponent(citizen, out Worker workerData))
                                {
                                    Entity workplace = workerData.m_Workplace;
                                    if ((workplace != Entity.Null) & (employeeBufferLookup.TryGetBuffer(workplace, out DynamicBuffer<Employee> employeeBufferPerWorkplace)))
                                    {
                                        for (int k = 0; k < employeeBufferPerWorkplace.Length; k++)
                                        {
                                            if (employeeBufferPerWorkplace[k].m_Worker == citizen)
                                            {
                                                // As of version 1.2.3f1, citizens are paid according to their education level. （截至 1.2.3f1 版本，市民的薪資是根據其教育程度給付。）
                                                // The salary brackets in a vanilla game are:（遊戲的原始薪資級距如下：）
                                                // * Uneducated（未受教育）－ ₡1500
                                                // * Poorly educated（教育不良）－ ₡1800
                                                // * Educated（受過教育）－ ₡2100
                                                // * Well educated（教育良好）－ ₡2400
                                                // * Highly educated（高等教育水準）－ ₡2700
                                                // See `Game.Simulation.PayWageSystem` for more information.（更多資訊請參見 `Game.Simulation.PayWageSystem`。）
                                                int salary = economyParameter.GetWage(workerData.m_Level);

                                                // According to `Game.Simulation.CompanyDividendSystem`, the company sets aside 12.5% (or 1/8) of its cash for employee dividends,
                                                // which are then distributed equally among all employees.
                                                // （根據 `Game.Simulation.CompanyDividendSystem`，公司會將 12.5%（1 / 8）的現金保留為員工分紅，並平分給所有員工。）
                                                if (dividendEntityMap.TryGetValue(workplace, out int dividend))
                                                {
                                                    salary += dividend;
                                                }

                                                // Taxable income = Gross income - Exemptions（應納稅所得 = 總收入 - 免稅額）
                                                // The exemption worths ₡1400.（免稅額為 ₡1400。）
                                                if (taxableIncomeOnly)
                                                {
                                                    salary -= economyParameter.m_ResidentialMinimumEarnings;
                                                    if (salary < 0) salary = 0;
                                                }

                                                stat.labor++;
                                                stat.wage += salary;
                                                break;
                                            }
                                        }
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
        /// The job to collect each company's dividend.
        /// （收集每間公司員工分紅的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectCompanyDividendsJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, int>.ParallelWriter hashmap;

            public void Execute(in DynamicBuffer<Employee> employee, in DynamicBuffer<Game.Economy.Resources> resources, Entity company)
            {
                // According to `Game.Simulation.CompanyDividendSystem`, the company sets aside 12.5% (or 1/8) of its cash for employee dividends,
                // which are then distributed equally among all employees.
                // （根據 `Game.Simulation.CompanyDividendSystem`，公司會將 12.5%（1 / 8）的現金保留為員工分紅，並平分給所有員工。）
                int cash = EconomyUtils.GetResources(Resource.Money, resources);
                if (cash <= 0 || employee.Length <= 0) return;
                hashmap.TryAdd(company, cash / (8 * employee.Length));
            }
        }

        /// <summary>
        /// Retrieve the world heightmap.
        /// （獲取世界高度圖。）
        /// </summary>
        /// <param name="options">The export options.（檔案輸出設定。）</param>
        public void GetWorldElevation(Options options)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref NativeArray<ushort> worldElevation = ref _worldElevation;
            Texture map = _terrain.worldHeightmap;

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset(ref worldElevation, map.width * map.height);

            // Convert the texture into array.（將材質貼圖轉為陣列。）
            AsyncGPUReadback.RequestIntoNativeArray(ref worldElevation, map).WaitForCompletion();
        }

        /// <summary>
        /// Retrieve theme / asset pack's information.
        /// （獲取建築風格／資產包的資訊。）
        /// </summary>
        /// <param name="options">The export options.（檔案輸出設定。）</param>
        private void GetThemes(Options options)
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
            if (options.AssetPack)
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
        /// <param name="options">The export options.（檔案輸出設定。）</param>
        public void GetZoningTypes(Options options)
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
                GetThemes(options);

                // Add the data that can only be retrieved in the main thread.（添加只能在主執行緒取得的資料。）
                for (int index = 0; index < zoningTypeCount; index++)
                {
                    ref ZoningType zoningType = ref types.ElementAt(index);

                    // Ensure safety when the zonings are not correctly loaded (e.g. a region pack is missing).
                    // （確保分區未正確載入時的安全性（例如缺少地區包）。）
                    if (!Instance.Prefab.TryGetPrefab(zoningType.prefabData, out ZonePrefab zonePrefabData))
                    {
                        names.Add(new("Placeholder", Allocator.Persistent));
                        continue;
                    }

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
                        Game.Zones.AreaType.Residential => ZoningCategory.Residential,
                        Game.Zones.AreaType.Commercial => ZoningCategory.Commercial,
                        Game.Zones.AreaType.Industrial => ZoningCategory.Industrial,
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