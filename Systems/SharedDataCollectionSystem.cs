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
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
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
        /// The assembly of Zone Color Changer mod.（Zone Color Changer 模組組件。）<br/>
        /// See <see cref="Instance.Zcc"/> for more information.
        /// </summary>
        static readonly ZoneColorChanger _zcc = Instance.Zcc;

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
        /// The query to collect all lane prefabs.
        /// （收集所有車道網路預製模板的查詢。）
        /// </summary>
        static EntityQuery _lanePrefabQuery;

        /// <summary>
        /// The query to collect all network entities.
        /// （收集所有網路實體的查詢。）
        /// </summary>
        static EntityQuery _networkQuery;

        /// <summary>
        /// The query to collect the attachment of roundabouts.
        /// （收集圓環附件的查詢。）
        /// </summary>
        static EntityQuery _roundaboutAttachmentQuery;

        /// <summary>
        /// The query to collect all roundabouts.
        /// （收集所有圓環的查詢。）
        /// </summary>
        static EntityQuery _roundaboutQuery;

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
        /// The query to collect all utility service network entities.
        /// （收集所有公共服務管線網路實體的查詢。）
        /// </summary>
        static EntityQuery _utilityServiceNetworkQuery;

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
        /// The list of all network's statistics in the savegame.
        /// （遊戲存檔內所有網路的統計數據。）
        /// </summary>
        public ref NativeList<NetworkStat> NetworkStats => ref _networkStats;

        /// <summary>
        /// See <see cref="NetworkStats"/>.
        /// </summary>
        private NativeList<NetworkStat> _networkStats;

        /// <summary>
        /// The map between network entities and their index in <see cref="NetworkStats"/>.
        /// （網路實體與其在 <see cref="NetworkStats"/> 索引值的映射表。）
        /// </summary>
        public ref NativeParallelHashMap<Entity, int> NetworkStatsEntityMap => ref _networkStatsEntityMap;

        /// <summary>
        /// See <see cref="NetworkStatsEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _networkStatsEntityMap;

        /// <summary>
        /// The list of in-game roundabouts.
        /// （遊戲內圓環的列表。）
        /// </summary>
        public ref NativeList<Domain.Roundabout> Roundabouts => ref _roundabouts;

        /// <summary>
        /// See <see cref="Roundabouts"/>.
        /// </summary>
        public NativeList<Domain.Roundabout> _roundabouts;

        /// <summary>
        /// The map between roundabout nodes and their index in <see cref="Roundabouts"/>.
        /// （圓環節點實體與其在 <see cref="Roundabouts"/> 索引值的映射表。）
        /// </summary>
        public ref NativeParallelHashMap<Entity, int> RoundaboutsEntityMap => ref _roundaboutsEntityMap;

        /// <summary>
        /// See <see cref="RoundaboutsEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _roundaboutsEntityMap;

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

            _lanePrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<NetLaneData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<SecondaryLaneData>()
                }
            });

            _networkQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Composition>(),
                    ComponentType.ReadOnly<Curve>(),
                    ComponentType.ReadOnly<Edge>(),
                    ComponentType.ReadOnly<EdgeGeometry>(),
                    ComponentType.ReadOnly<Game.Net.SubLane>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _roundaboutAttachmentQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Attached>(),
                    ComponentType.ReadOnly<Game.Objects.NetObject>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Building>(),
                    ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
                    ComponentType.ReadOnly<Pillar>(),
                    ComponentType.ReadOnly<Game.Routes.TransportStop>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _roundaboutQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ConnectedEdge>(),
                    ComponentType.ReadOnly<Node>(),
                    ComponentType.ReadOnly<Game.Net.Roundabout>(),
                    ComponentType.ReadOnly<Game.Objects.SubObject>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
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

            _utilityServiceNetworkQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Edge>()
                },
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<ElectricityNodeConnection>(),
                    ComponentType.ReadOnly<WaterPipeNodeConnection>()
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
            _zcc.Dispose();
            Utils.CommonUtils.Dispose(ref _brandsEntityMap);
            Utils.CommonUtils.Dispose(ref _buildingStats);
            Utils.CommonUtils.Dispose(ref _networkStats);
            Utils.CommonUtils.Dispose(ref _networkStatsEntityMap);
            Utils.CommonUtils.Dispose(ref _roundabouts);
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

                case DisposePhase.AfterNetworkRelated:
                    Utils.CommonUtils.Dispose(ref _networkStats);
                    Utils.CommonUtils.Dispose(ref _networkStatsEntityMap);
                    Utils.CommonUtils.Dispose(ref _roundabouts);
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
            ref NativeList<NetworkStat> networkStats = ref _networkStats;
            ref NativeList<ZoningType> zonings = ref _zoningTypes;
            ref NativeList<NativeText> zoningsNames = ref _zoningTypesNames;
            ref NativeParallelHashMap<Entity, int> brandsEntityMap = ref _brandsEntityMap;
            ref NativeParallelHashMap<Entity, int> networkStatsEntityMap = ref _networkStatsEntityMap;
            ref NativeParallelHashMap<Entity, int> zoningsEntityMap = ref _zoningTypesEntityMap;

            // Export options.（輸出設定。）
            bool hasAddress = options.Contains(Property.Address) || ((options.Systems & IO.System.Network) != 0);
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

            // Collect networks.（收集網路。）
            if (hasAddress)
            {
                GetNetworkStats(options);
            }
            else
            {
                Utils.CommonUtils.Reset(ref networkStats, 1);
                Utils.CommonUtils.Reset(ref networkStatsEntityMap, 1);
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
            zoningsNames.Add(new("Unzoned", Allocator.Persistent)); 

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
        /// Get the form of the road from the NetCompositionData component.
        /// （由 NetCompositionData 部件獲得道路的形式。）
        /// </summary>
        /// <param name="netCompositionData">The NetCompositionData component.（NetCompositionData 部件。）</param>
        /// <returns>The form of the road.（道路的形式。）</returns>
        private static Form GetForm(NetCompositionData netCompositionData)
        {
            Form form = Form.Normal;
            CompositionFlags.General generalCompositions = netCompositionData.m_Flags.m_General;
            if ((generalCompositions & CompositionFlags.General.Elevated) != 0) form = Form.Elevated;
            if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) form = Form.Tunnel;
            return form;
        }

        /// <summary>
        /// Retrieve network entities' statistical data.
        /// （獲取網路實體的統計資料。）
        /// </summary>
        /// <param name="options">The export options.（檔案輸出選項。）</param>
        public void GetNetworkStats(Options options)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref NativeParallelHashMap<Entity, int> entityMap = ref _networkStatsEntityMap;
            ref NativeList<Domain.Roundabout> roundabouts = ref _roundabouts;
            ref NativeParallelHashMap<Entity, int> roundaboutsEntityMap = ref _roundaboutsEntityMap;
            ref NativeList<NetworkStat> stats = ref _networkStats;

            // Export options.（輸出設定。）
            Feature featureFlag = options.Features;
            bool hasCenterline = (
                (options.VectorKinds.TryGetValue(IO.System.Network, out VectorKind networkKinds) && (networkKinds & VectorKind.Centerline) != 0) ||
                (options.VectorKinds.TryGetValue(IO.System.Route,   out VectorKind routeKinds)   && (routeKinds   & VectorKind.Centerline) != 0)
            );
            bool hasUtilityServices = ((featureFlag & Feature.Cable) != 0) || ((featureFlag & Feature.Pipe) != 0);

            // Initialize native containers.（初始化原生容器。）
            int lanePrefabCount = _lanePrefabQuery.CalculateEntityCount();
            int roundaboutAttachmentCount = _roundaboutAttachmentQuery.CalculateEntityCount();
            int roundaboutCount = _roundaboutQuery.CalculateEntityCount();
            int utilityServiceNetworkCount = GetUtilityServiceNetworkSegmentsCount();
            int networkCount = _networkQuery.CalculateEntityCount();
            if (hasCenterline) networkCount += roundaboutCount;
            if (hasUtilityServices) networkCount += utilityServiceNetworkCount;
            NativeParallelHashMap<Entity, Domain.Lane> lanesEntityMap = new(lanePrefabCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, Entity> attachmentEntityMap = new(roundaboutAttachmentCount, Allocator.Persistent);
            NativeParallelHashSet<Entity> utilityLanes = new(lanePrefabCount, Allocator.Persistent);

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset(ref entityMap, networkCount);
            Utils.CommonUtils.Reset(ref roundabouts, roundaboutCount);
            Utils.CommonUtils.Reset(ref roundaboutsEntityMap, roundaboutCount);
            Utils.CommonUtils.Reset(ref stats, networkCount);

            try
            {
                CollectLanesJob collectLanesJob = new()
                {
                    carLaneDataLookup = GetComponentLookup<CarLaneData>(),
                    parkingLaneDataLookup = GetComponentLookup<ParkingLaneData>(),
                    trackLaneDataLookup = GetComponentLookup<TrackLaneData>(),
                    utilityLaneDataLookup = GetComponentLookup<UtilityLaneData>(),
                    entityMap = lanesEntityMap.AsParallelWriter(),
                    utilityLanes = utilityLanes.AsParallelWriter()
                };
                JobHandle collectLanesHandle = collectLanesJob.ScheduleParallel(_lanePrefabQuery, default);
                collectLanesHandle.Complete();

                MapRoundaboutAttachmentsJob mapAttachmentJob = new()
                {
                    entityMap = attachmentEntityMap.AsParallelWriter(),
                };
                JobHandle mapAttachmentHandle = mapAttachmentJob.ScheduleParallel(_roundaboutAttachmentQuery, default);
                mapAttachmentHandle.Complete();

                CollectRoundaboutsJob collectRoundaboutsJob = new()
                {
                    hasCenterline = hasCenterline,
                    leftHandTraffic = Instance.City.leftHandTraffic,
                    compositionLookup = GetComponentLookup<Composition>(),
                    netCompositionDataLookup = GetComponentLookup<NetCompositionData>(),
                    netGeometryDataLookup = GetComponentLookup<NetGeometryData>(),
                    objectGeometryDataLookup = GetComponentLookup<ObjectGeometryData>(),
                    placeableObjectDataLookup = GetComponentLookup<PlaceableObjectData>(),
                    roadLookup = GetComponentLookup<Road>(),
                    roadCompositionLookup = GetComponentLookup<RoadComposition>(),
                    subwayTrackLookup = GetComponentLookup<SubwayTrack>(),
                    trackCompositionLookup = GetComponentLookup<TrackComposition>(),
                    trainTrackLookup = GetComponentLookup<TrainTrack>(),
                    tramTrackLookup = GetComponentLookup<TramTrack>(),
                    attachmentEntityMap = attachmentEntityMap,
                    list = roundabouts.AsParallelWriter(),
                    stats = stats.AsParallelWriter()
                };
                JobHandle collectRoundaboutsHandle = collectRoundaboutsJob.ScheduleParallel(_roundaboutQuery, default);
                collectRoundaboutsHandle.Complete();

                MapRoundaboutsIndexJob mapIndexJob = new()
                {
                    list = roundabouts,
                    map = roundaboutsEntityMap
                };
                JobHandle mapIndexHandle = mapIndexJob.Schedule(roundaboutCount, 16);
                mapIndexHandle.Complete();

                CollectNetworkStatsJob collectNetworkStatsJob = new()
                {
                    hasUtilityServiceNetworks = hasUtilityServices,
                    connectedBuildingBufferLookup = GetBufferLookup<ConnectedBuilding>(),
                    connectedFlowEdgeBufferLookup = GetBufferLookup<ConnectedFlowEdge>(),
                    netCompositionPieceBufferLookup = GetBufferLookup<NetCompositionPiece>(),
                    netPieceLaneBufferLookup = GetBufferLookup<NetPieceLane>(),
                    subLaneBufferLookup = GetBufferLookup<Game.Net.SubLane>(),
                    buildingLookup = GetComponentLookup<Building>(),
                    electricityConsumerLookup = GetComponentLookup<ElectricityConsumer>(),
                    electricityFlowEdgeLookup = GetComponentLookup<ElectricityFlowEdge>(),
                    electricityNodeConnectionLookup = GetComponentLookup<ElectricityNodeConnection>(),
                    markerLookup = GetComponentLookup<Game.Net.Marker>(),
                    netCompositionDataLookup = GetComponentLookup<NetCompositionData>(),
                    pathwayCompositionLookup = GetComponentLookup<PathwayComposition>(),
                    pipelineDataLookup = GetComponentLookup<PipelineData>(),
                    powerLineDataLookup = GetComponentLookup<PowerLineData>(),
                    prefabRefLookup = GetComponentLookup<PrefabRef>(),
                    roadLookup = GetComponentLookup<Road>(),
                    roadCompositionLookup = GetComponentLookup<RoadComposition>(),
                    taxiwayCompositionLookup = GetComponentLookup<TaxiwayComposition>(),
                    trackCompositionLookup = GetComponentLookup<TrackComposition>(),
                    waterPipeEdgeLookup = GetComponentLookup<WaterPipeEdge>(),
                    waterPipeNodeConnectionLookup = GetComponentLookup<WaterPipeNodeConnection>(),
                    waterwayCompositionLookup = GetComponentLookup<WaterwayComposition>(),
                    lanesEntityMap = lanesEntityMap,
                    roundaboutsEntityMap = roundaboutsEntityMap,
                    list = stats.AsParallelWriter()
                };
                JobHandle collectNetworkStatsHandle = collectNetworkStatsJob.ScheduleParallel(_networkQuery, default);
                collectNetworkStatsHandle.Complete();

                _log.Info(hasUtilityServices);
                _log.Info(networkCount);
                _log.Info(stats.Length);

                for (int i = 0; i < roundabouts.Length; i++)
                {
                    _log.Info(roundabouts[i]);
                }

                _log.Info("Roundabouts:");

                for (int i = 0; i < stats.Length; i++)
                {
                    NetworkStat stat = stats[i];
                    if (stat.isRoundabout)
                    {
                        _log.Info(stat);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref attachmentEntityMap);
                Utils.CommonUtils.Dispose(ref lanesEntityMap);
                Utils.CommonUtils.Dispose(ref utilityLanes);
            }

            // TODO: Temporary disposal
            Dispose(DisposePhase.AfterNetworkRelated);
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
            themes.Add(new() { entity = Entity.Null, name = Utils.LocaleUtils.TryTranslate("Assets.THEME[Carto Generic]", out string genericTheme) ? genericTheme : "Generic" });

            // Collect building themes.（收集建築風格。）
            NativeArray<Entity> themeEntities = _themePrefabQuery.ToEntityArray(Allocator.Temp);
            NativeArray<PrefabData> themePrefabs = _themePrefabQuery.ToComponentDataArray<PrefabData>(Allocator.Temp);
            for (int i = 0; i < themeEntities.Length; i++)
            {
                if (Instance.Prefab.TryGetPrefab(themePrefabs[i], out PrefabBase themePrefab))
                {
                    Entity theme = themeEntities[i];
                    string themePrefabName = Instance.Prefab.GetPrefabName(theme);
                    Theme data = new()
                    {
                        entity = theme,
                        name = Utils.LocaleUtils.TryTranslate($"Assets.THEME[{themePrefabName}]", out string themeUiName) ? themeUiName : themePrefabName
                    };
                    themes.Add(data);
                    prefabMap.Add(themePrefab, themes.Count - 1);
                }
            }

            // Collect asset packs.（收集資產包。）
            if (options.AssetPack)
            {
                NativeArray<Entity> assetPacks = _assetPackPrefabQuery.ToEntityArray(Allocator.Temp);
                NativeArray<PrefabData> assetPackPrefabs = _assetPackPrefabQuery.ToComponentDataArray<PrefabData>(Allocator.Temp);
                for (int i = 0; i < assetPacks.Length; i++)
                {
                    if (Instance.Prefab.TryGetPrefab(assetPackPrefabs[i], out PrefabBase assetPackPrefab))
                    {
                        Entity assetPack = assetPacks[i];
                        string assetPackPrefabName = Instance.Prefab.GetPrefabName(assetPack);
                        Theme data = new()
                        {
                            entity = assetPack,
                            name = Utils.LocaleUtils.TryTranslate($"Assets.NAME[{assetPackPrefabName}]", out string assetPackUiName) ? assetPackUiName : assetPackPrefabName
                        };
                        themes.Add(data);
                        prefabMap.Add(assetPackPrefab, themes.Count - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the number of utility service network segments.
        /// （獲得公共服務管線路段的數量。）
        /// </summary>
        /// <returns>The number of segments.（路段的數量。）</returns>
        private int GetUtilityServiceNetworkSegmentsCount()
        {
            int count = 0;

            // Initialize native containers.（初始化原生容器。）
            NativeQueue<int> individualSegmentCounts = new(Allocator.Persistent);
            NativeQueue<int> integratedSegmentCounts = new(Allocator.Persistent);

            try
            {
                CountIndividualUtilityServiceNetworkSegmentsJob countIndividualJob = new()
                {
                    electricityConnectionLookup = GetComponentLookup<Game.Net.ElectricityConnection>(),
                    pipeLineDataLookup = GetComponentLookup<PipelineData>(),
                    powerLineDataLookup = GetComponentLookup<PowerLineData>(),
                    waterPipeConnectionLookup = GetComponentLookup<Game.Net.WaterPipeConnection>(),
                    queue = individualSegmentCounts.AsParallelWriter()
                };
                JobHandle countIndividualHandle = countIndividualJob.ScheduleParallel(_networkQuery, default);
                countIndividualHandle.Complete();
                
                CountIntegratedUtilityServiceNetworkSegmentsJob countIntegratedJob = new()
                {
                    connectedBuildingBufferLookup = GetBufferLookup<ConnectedBuilding>(),
                    connectedFlowEdgeBufferLookup = GetBufferLookup<ConnectedFlowEdge>(),
                    connectedNodeBufferLookup = GetBufferLookup<ConnectedNode>(),
                    buildingLookup = GetComponentLookup<Building>(),
                    electricityBuildingConnectionLookup = GetComponentLookup<ElectricityBuildingConnection>(),
                    electricityConsumerLookup = GetComponentLookup<ElectricityConsumer>(),
                    electricityFlowEdgeLookup = GetComponentLookup<ElectricityFlowEdge>(),
                    electricityNodeConnectionLookup = GetComponentLookup<ElectricityNodeConnection>(),
                    placeholderLookup = GetComponentLookup<Placeholder>(),
                    waterConsumerLookup = GetComponentLookup<WaterConsumer>(),
                    waterPipeConnectionLookup = GetComponentLookup<Game.Net.WaterPipeConnection>(),
                    waterPipeEdgeLookup = GetComponentLookup<WaterPipeEdge>(),
                    waterPipeNodeConnectionLookup = GetComponentLookup<WaterPipeNodeConnection>(),
                    queue = integratedSegmentCounts.AsParallelWriter()
                };
                JobHandle countIntegratedHandle = countIntegratedJob.ScheduleParallel(_utilityServiceNetworkQuery, default);
                countIntegratedHandle.Complete();

                count += Utils.CommonUtils.Sum(ref individualSegmentCounts);
                count += Utils.CommonUtils.Sum(ref integratedSegmentCounts);
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref individualSegmentCounts);
                Utils.CommonUtils.Dispose(ref integratedSegmentCounts);
            }

            return count;
        }

        /// <summary>
        /// Get the volume of the road from the Road component.
        /// （由 Road 部件獲得道路的流量。）
        /// </summary>
        /// <param name="road">The Road component.（Road 部件。）</param>
        /// <returns>The number of vehicles using the road.（使用道路的車輛數量。）</returns>
        private static float GetVolume(Road road)
        {
            // According to `Game.UI.InGame.RoadSection`, the volume data is calculated every 6 hours.
            // The x, y, z, and w property represent the distance / duration at 00:00, 06:00, 12:00 and 18:00.
            // Carto use the traffic volume at noon to show the traffic in peak hour.
            // （根據 `Game.UI.InGame.RoadSection`，流量每 6 小時計算一次，其中 x、y、z、w 屬性分別代表 00:00、06:00、12:00、18:00 的距離／持續時間。）
            // （為展示尖峰時間的車流量，Carto 使用中午的數據。）
            return (float) Math.Round((road.m_TrafficFlowDistance0.z + road.m_TrafficFlowDistance1.z) * 8f / 3f, 2);
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

            // Initialize managed containers.（初始化控管容器。）
            Dictionary<string, UnityEngine.Color> vanillaColorMap = new();

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

                // Prepare the vanilla zone color map if the Zone Color Changer mod presents.（若 Zone Color Changer 模組出現，則準備原版分區顏色映射表。）
                bool vanillaColorAccessible = false;
                bool groupThemes = false;
                if (!options.ZccColor && _zcc.TryGet())
                {
                    if (_zcc.TryGetColorMap(out vanillaColorMap, out groupThemes))
                    {
                        vanillaColorAccessible = true;
                    }
                    else
                    {
                        _log.Warn($"Couldn't access the vanilla zone colors from {_zcc}. The latest verified version is {_zcc.VerifiedVersion}. 無法由 {_zcc} 存取原版分區色彩。最新的已驗證版本為 {_zcc.VerifiedVersion}。");
                    }
                }

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

                    string zoningTypeName = Instance.Prefab.GetPrefabName(zoningType.entity);
                    string zccName = groupThemes ? Regex.Replace(zoningTypeName, "^[A-Z]{2,3} ", string.Empty) : zoningTypeName;
                    if (vanillaColorAccessible && vanillaColorMap.TryGetValue(zccName, out UnityEngine.Color zoningVanillaColor))
                    {
                        zoningType.color = zoningVanillaColor;
                    }
                    else
                    {
                        zoningType.color = zonePrefabData.m_Color;
                    }

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
                    names.Add(new NativeText(Utils.LocaleUtils.TryTranslate($"Assets.NAME[{zoningTypeName}]", out string zoningTypeUiName) ? zoningTypeUiName : zoningTypeName, Allocator.Persistent));
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
        /// Round the speed limit to more "beautiful" numbers.
        /// （將速度限制數字取整為「漂亮的」數字。）
        /// </summary>
        /// <param name="input">The input value.（輸入的數值。）</param>
        /// <returns>The speed limit value that is divisable by 5.（可被 5 整除的速度限制數值。）</returns>
        private static float RoundSpeedLimit(float input) => (float)(Math.Round(input * 2 / 5.0) * 5.0);

        /// <summary>
        /// Try to retrieve the electricity link. This is a Burst-compatible version of <see cref="ElectricityGraphUtils.TryGetFlowEdge"/>.<br/>
        /// （嘗試取得電流的連結。這是 <see cref="ElectricityGraphUtils.TryGetFlowEdge"/> 的可 Burst 編譯版本。）
        /// </summary>
        /// <param name="startNode">The entity that holds the start node of the link.（擁有連結起始節點的實體。）</param>
        /// <param name="endNode">The entity that holds the end node of the link.（擁有連結結尾節點的實體。）</param>
        /// <param name="flowEdges">The look-up of <see cref="ConnectedFlowEdge"/>.（<see cref="ConnectedFlowEdge"/> 的查詢。）</param>
        /// <param name="flowEdgeLookup">The look-up of <see cref="ElectricityFlowEdge"/>.（<see cref="ElectricityFlowEdge"/> 的查詢。）</param>
        /// <param name="flowEdge">The electricity link.（電流連結。）</param>
        /// <returns>Whether the link exists or not.（連結是否存在？）</returns>
        private static bool TryGetElectricityFlowEdge(Entity startNode, Entity endNode, ref BufferLookup<ConnectedFlowEdge> flowEdges, ref ComponentLookup<ElectricityFlowEdge> flowEdgeLookup, out ElectricityFlowEdge flowEdge)
        {
            flowEdge = default;
            if ((startNode.Index <= 0) || (endNode.Index <= 0)) return false;
            if (!flowEdges.TryGetBuffer(startNode, out DynamicBuffer<ConnectedFlowEdge> connectedFlowEdges)) return false;

            for (int i = 0; i < connectedFlowEdges.Length; i++)
            {
                ConnectedFlowEdge connectedFlowEdge = connectedFlowEdges[i];
                if (flowEdgeLookup.TryGetComponent(connectedFlowEdge.m_Edge, out ElectricityFlowEdge edgeCandidate) && (edgeCandidate.m_Start == startNode) && (edgeCandidate.m_End == endNode))
                {
                    flowEdge = edgeCandidate;
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Try to retrieve the water link. This is a Burst-compatible version of <see cref="WaterPipeGraphUtils.TryGetFlowEdge"/>.<br/>
        /// （嘗試取得水源的連結。這是 <see cref="WaterPipeGraphUtils.TryGetFlowEdge"/> 的可 Burst 編譯版本。）
        /// </summary>
        /// <param name="startNode">The entity that holds the start node of the link.（擁有連結起始節點的實體。）</param>
        /// <param name="endNode">The entity that holds the end node of the link.（擁有連結結尾節點的實體。）</param>
        /// <param name="flowEdges">The look-up of <see cref="ConnectedFlowEdge"/>.（<see cref="ConnectedFlowEdge"/> 的查詢。）</param>
        /// <param name="flowEdgeLookup">The look-up of <see cref="WaterPipeEdge"/>.（<see cref="WaterPipeEdge"/> 的查詢。）</param>
        /// <param name="flowEdge">The water pipe link.（水流連結。）</param>
        /// <returns>Whether the link exists or not.（連結是否存在？）</returns>
        private static bool TryGetWaterPipeEdge(Entity startNode, Entity endNode, ref BufferLookup<ConnectedFlowEdge> flowEdges, ref ComponentLookup<WaterPipeEdge> flowEdgeLookup, out WaterPipeEdge flowEdge)
        {
            flowEdge = default;
            if ((startNode.Index <= 0) || (endNode.Index <= 0)) return false;
            if (!flowEdges.TryGetBuffer(startNode, out DynamicBuffer<ConnectedFlowEdge> connectedFlowEdges)) return false;

            for (int i = 0; i < connectedFlowEdges.Length; i++)
            {
                ConnectedFlowEdge connectedFlowEdge = connectedFlowEdges[i];
                if (flowEdgeLookup.TryGetComponent(connectedFlowEdge.m_Edge, out WaterPipeEdge edgeCandidate) && (edgeCandidate.m_Start == startNode) && (edgeCandidate.m_End == endNode))
                {
                    flowEdge = edgeCandidate;
                    return true;
                }
            }

            return false;
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
            public bool useAddress;

            [ReadOnly]
            public BufferLookup<AggregateElement> aggregateElementBufferLookup;

            [ReadOnly]
            public BufferLookup<HouseholdCitizen> citizenBufferLookup;

            [ReadOnly]
            public BufferLookup<Employee> employeeBufferLookup;

            [ReadOnly]
            public BufferLookup<Renter> renterBufferLookup;

            [ReadOnly]
            public ComponentLookup<Aggregated> aggregatedLookup;

            [ReadOnly]
            public ComponentLookup<Citizen> citizenLookup;

            [ReadOnly]
            public ComponentLookup<CompanyData> companyDataLookup;

            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<Edge> edgeLookup;

            [ReadOnly]
            public ComponentLookup<HealthProblem> healthProblemLookup;

            [ReadOnly]
            public ComponentLookup<HomelessHousehold> homelessHouseholdLookup;

            [ReadOnly]
            public ComponentLookup<Household> householdLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

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
            public ComponentLookup<Game.Objects.Transform> transformLookup;

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

            public void Execute(in Building buildingComponent, in PrefabRef prefabRef, Entity building)
            {
                // Whether the first shop is recorded or not.（第一間商店是否被記錄了？）
                bool isFirstShop = true;

                BuildingStat stat = new()
                {
                    entity = building,
                    address = Address.Null,
                    age = 0f,
                    brand = 0,
                    company = 0,
                    employee = 0,
                    household = 0,
                    labor = 0,
                    level = 0,
                    mainBuilding = Entity.Null,
                    objectType = Feature.Building,
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
                    stat.level = spawnableData.m_Level;
                    
                    if (zoningEntityMap.TryGetValue(spawnableData.m_ZonePrefab, out int zoningIndex))
                    {
                        stat.zoning = zoningIndex;
                    }
                }

                if (useAddress)
                {
                    // This is the Unity job version of the vanilla `BuildingUtils.GetAddress` method.（原始遊戲 `BuildingUtils.GetAddress` 的 Unity 工作版本。）
                    Entity curve = buildingComponent.m_RoadEdge;
                    float curvePosition = buildingComponent.m_CurvePosition;
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
        /// The job to collect network lane's information.
        /// （收集車道資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectLanesJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<CarLaneData> carLaneDataLookup;

            [ReadOnly]
            public ComponentLookup<ParkingLaneData> parkingLaneDataLookup;

            [ReadOnly]
            public ComponentLookup<TrackLaneData> trackLaneDataLookup;

            [ReadOnly]
            public ComponentLookup<UtilityLaneData> utilityLaneDataLookup;
            
            [WriteOnly]
            public NativeParallelHashMap<Entity, Domain.Lane>.ParallelWriter entityMap;

            [WriteOnly]
            public NativeParallelHashSet<Entity>.ParallelWriter utilityLanes;

            public void Execute(in NetLaneData netLaneData, Entity lane)
            {
                Domain.Lane laneStruct = new()
                {
                    entity = lane,
                    category = NetworkCategory.None
                };

                if (!parkingLaneDataLookup.TryGetComponent(lane, out ParkingLaneData _))
                {
                    LaneFlags laneGeneralFlag = netLaneData.m_Flags;
                    
                    if ((laneGeneralFlag & LaneFlags.OnWater) != 0)
                    {
                        laneStruct.category |= NetworkCategory.Waterway;
                    }

                    if ((laneGeneralFlag & LaneFlags.PublicOnly) != 0)
                    {
                        laneStruct.category |= NetworkCategory.Bus;
                    }

                    if (carLaneDataLookup.TryGetComponent(lane, out CarLaneData carLaneData))
                    {
                        RoadTypes roadTypes = carLaneData.m_RoadTypes;

                        if ((roadTypes & RoadTypes.Car) != 0)        laneStruct.category |= NetworkCategory.Car;
                        if ((roadTypes & RoadTypes.Watercraft) != 0) laneStruct.category |= NetworkCategory.Waterway;
                    }

                    if (trackLaneDataLookup.TryGetComponent(lane, out TrackLaneData trackLaneData))
                    {
                        TrackTypes trackTypes = trackLaneData.m_TrackTypes;

                        if ((trackTypes & TrackTypes.Train) != 0)  laneStruct.category |= NetworkCategory.Train;
                        if ((trackTypes & TrackTypes.Tram) != 0)   laneStruct.category |= NetworkCategory.Tram;
                        if ((trackTypes & TrackTypes.Subway) != 0) laneStruct.category |= NetworkCategory.Subway;
                    }

                    if (utilityLaneDataLookup.TryGetComponent(lane, out UtilityLaneData utilityLaneData))
                    {
                        UtilityTypes utilityTypes = utilityLaneData.m_UtilityTypes;

                        if ((utilityTypes != UtilityTypes.None) && ((utilityTypes & UtilityTypes.Catenary) == 0))
                        {
                            if ((utilityTypes & UtilityTypes.Fence) != 0)           laneStruct.category |= NetworkCategory.Fence;
                            if ((utilityTypes & UtilityTypes.HighVoltageLine) != 0) laneStruct.category |= NetworkCategory.HighCable;
                            if ((utilityTypes & UtilityTypes.LowVoltageLine) != 0)  laneStruct.category |= NetworkCategory.LowCable;
                            if ((utilityTypes & UtilityTypes.SewagePipe) != 0)      laneStruct.category |= NetworkCategory.SewagePipe;
                            if ((utilityTypes & UtilityTypes.StormwaterPipe) != 0)  laneStruct.category |= NetworkCategory.StormPipe;
                            if ((utilityTypes & UtilityTypes.WaterPipe) != 0)       laneStruct.category |= NetworkCategory.WaterPipe;
                            utilityLanes.Add(lane);
                        }
                    }
                }

                entityMap.TryAdd(lane, laneStruct);
            }
        }

        /// <summary>
        /// The job to collect network statistics.
        /// （收集網路統計資料的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectNetworkStatsJob : IJobEntity
        {
            [ReadOnly]
            public bool hasUtilityServiceNetworks;

            [ReadOnly]
            public BufferLookup<ConnectedBuilding> connectedBuildingBufferLookup;

            [ReadOnly]
            public BufferLookup<ConnectedFlowEdge> connectedFlowEdgeBufferLookup;

            [ReadOnly]
            public BufferLookup<NetCompositionPiece> netCompositionPieceBufferLookup;

            [ReadOnly]
            public BufferLookup<NetPieceLane> netPieceLaneBufferLookup;

            [ReadOnly]
            public BufferLookup<Game.Net.SubLane> subLaneBufferLookup;

            [ReadOnly]
            public ComponentLookup<Building> buildingLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityConsumer> electricityConsumerLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityFlowEdge> electricityFlowEdgeLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityNodeConnection> electricityNodeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.Marker> markerLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<PathwayComposition> pathwayCompositionLookup;

            [ReadOnly]
            public ComponentLookup<PipelineData> pipelineDataLookup;

            [ReadOnly]
            public ComponentLookup<Placeholder> placeholderLookup;

            [ReadOnly]
            public ComponentLookup<PowerLineData> powerLineDataLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Road> roadLookup;

            [ReadOnly]
            public ComponentLookup<RoadComposition> roadCompositionLookup;

            [ReadOnly]
            public ComponentLookup<TaxiwayComposition> taxiwayCompositionLookup;

            [ReadOnly]
            public ComponentLookup<TrackComposition> trackCompositionLookup;

            [ReadOnly]
            public ComponentLookup<WaterConsumer> waterConsumerLookup;

            [ReadOnly]
            public ComponentLookup<WaterPipeEdge> waterPipeEdgeLookup;

            [ReadOnly]
            public ComponentLookup<WaterPipeNodeConnection> waterPipeNodeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<WaterwayComposition> waterwayCompositionLookup;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Domain.Lane> lanesEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> roundaboutsEntityMap;

            [WriteOnly]
            public NativeList<NetworkStat>.ParallelWriter list;

            public void Execute(in Composition composition, in Curve curve, in Edge edge, in EdgeGeometry edgeGeometry, in PrefabRef prefabRef, in DynamicBuffer<Game.Net.SubLane> subLanes, Entity network)
            {
                NetworkStat stat = new()
                {
                    entity = network,
                    capacity = 0f,
                    category = NetworkCategory.None,
                    direction = Direction.None,
                    discharge = 0f,
                    elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                    end = 1f,
                    endRoundaboutIndex = roundaboutsEntityMap.TryGetValue(edge.m_End, out int endRoundaboutIndex) ? endRoundaboutIndex : -1,
                    form = Form.Normal,
                    isRoundabout = false,
                    length = curve.m_Length,
                    limit = 0f,
                    load = 0f,
                    start = 0f,
                    startRoundaboutIndex = roundaboutsEntityMap.TryGetValue(edge.m_Start, out int startRoundaboutIndex) ? startRoundaboutIndex : -1,
                    volume = 0f,
                    width = 0f
                };

                DynamicBuffer<ConnectedFlowEdge> cableEdges = default;
                DynamicBuffer<ConnectedFlowEdge> pipeEdges = default;

                bool isPipeline = pipelineDataLookup.TryGetComponent(prefabRef, out _) && waterPipeNodeConnectionLookup.TryGetComponent(network, out WaterPipeNodeConnection pipeNodeConnection) && connectedFlowEdgeBufferLookup.TryGetBuffer(pipeNodeConnection.m_WaterPipeNode, out pipeEdges) && (pipeEdges.Length >= 2);
                bool isPowerLine = powerLineDataLookup.TryGetComponent(prefabRef, out _) && electricityNodeConnectionLookup.TryGetComponent(network, out ElectricityNodeConnection cableNodeConnection) && connectedFlowEdgeBufferLookup.TryGetBuffer(cableNodeConnection.m_ElectricityNode, out cableEdges) && (cableEdges.Length >= 2);
                bool isNotUtilityServiceNetwork = !(isPipeline || isPowerLine);
                bool overrideForm = false;
                Entity networkComposition = composition.m_Edge;
                bool hasComposition = networkComposition != Entity.Null;
                bool isTaxiway = taxiwayCompositionLookup.TryGetComponent(networkComposition, out TaxiwayComposition taxiwayComposition);

                if (markerLookup.TryGetComponent(network, out _) && !isTaxiway) return;

                if (isNotUtilityServiceNetwork)
                {
                    for (int i = 0; i < subLanes.Length; i++)
                    {
                        Entity subLane = subLanes[i].m_SubLane;
                        if (prefabRefLookup.TryGetComponent(subLane, out PrefabRef subLanePrefab) && lanesEntityMap.TryGetValue(subLanePrefab.m_Prefab, out Domain.Lane laneType) && !laneType.IsUtilityLane)
                        {
                            stat.category |= laneType.category;
                        } 
                    }
                    
                    if (hasComposition)
                    {
                        // If there are multiple compositions present, the priority of speed limit value would be pathway < waterway < taxiway < track < road.
                        //（若出現多種配置，優先順序為路徑 < 航路 < 滑行道 < 軌道 < 道路。）

                        if (pathwayCompositionLookup.TryGetComponent(networkComposition, out PathwayComposition pathwayComposition))
                        {
                            stat.category |= NetworkCategory.Pathway;
                            stat.direction = Direction.Both;
                            stat.limit = RoundSpeedLimit(pathwayComposition.m_SpeedLimit);
                        }

                        if (waterwayCompositionLookup.TryGetComponent(networkComposition, out WaterwayComposition waterwayComposition))
                        {
                            stat.elevation = edgeGeometry.m_Bounds.min.y;
                            stat.form = Form.Normal;    // Force the form to be normal; the vanilla setting is `Elevated`.（強制將形式變為一般；原版設定是 `Elevated`。）
                            stat.limit = RoundSpeedLimit(waterwayComposition.m_SpeedLimit);
                            overrideForm = true;
                        }

                        if (isTaxiway)
                        {
                            TaxiwayFlags taxiwayFlags = taxiwayComposition.m_Flags;
                            if (taxiwayFlags == TaxiwayFlags.Airspace) return;  // Exporting air space is not supported.（不支援輸出空域。）
                            bool hasRunwayFlag = (taxiwayFlags & TaxiwayFlags.Runway) != 0;
                            stat.category = hasRunwayFlag ? NetworkCategory.Runway : NetworkCategory.Taxiway;
                            stat.elevation = edgeGeometry.m_Bounds.min.y;
                            stat.limit = RoundSpeedLimit(taxiwayComposition.m_SpeedLimit);
                        }

                        if (trackCompositionLookup.TryGetComponent(networkComposition, out TrackComposition trackComposition))
                        {
                            stat.limit = RoundSpeedLimit(trackComposition.m_SpeedLimit);
                        }

                        if (roadCompositionLookup.TryGetComponent(networkComposition, out RoadComposition roadComposition))
                        {
                            bool hasHighwayRule = (roadComposition.m_Flags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0;
                            if (hasHighwayRule)
                            {
                                stat.category &= ~NetworkCategory.Car;
                                stat.category |= NetworkCategory.Highway;
                            }
                            stat.limit = RoundSpeedLimit(roadComposition.m_SpeedLimit);
                        }

                        if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositionData))
                        {
                            // The network direction.（網路方向。）
                            CompositionState state = netCompositionData.m_State;
                            if ((state & CompositionState.HasForwardRoadLanes) != 0) stat.direction |= Direction.Forward;
                            if ((state & CompositionState.HasForwardTrackLanes) != 0) stat.direction |= Direction.Forward;
                            if ((state & CompositionState.HasBackwardRoadLanes) != 0) stat.direction |= Direction.Backward;
                            if ((state & CompositionState.HasBackwardTrackLanes) != 0) stat.direction |= Direction.Backward;

                            // The network form.（網路形式。）
                            if (!overrideForm) stat.form = GetForm(netCompositionData);

                            stat.width = netCompositionData.m_Width;
                        }
                    }

                    // Calculate the traffic volume.（計算交通流量。）
                    if (roadLookup.TryGetComponent(network, out Road roadComponent))
                    {
                        stat.volume = GetVolume(roadComponent);
                    }

                    list.AddNoResize(stat);
                }

                // Handle the individual utility service networks.（處理獨立公共管線網路。）
                if (hasUtilityServiceNetworks && !isNotUtilityServiceNetwork)
                {
                    // Generate the `NetworkStat` for the pipe.（產生水管的 `NetworkStat`。）
                    if (isPipeline && waterPipeEdgeLookup.TryGetComponent(pipeEdges[0].m_Edge, out WaterPipeEdge pipeFlow))
                    {
                        if (pipeFlow.m_FreshCapacity > 0)
                        {
                            NetworkStat water = new()
                            {
                                entity = network,
                                capacity = pipeFlow.m_FreshCapacity,
                                category = NetworkCategory.WaterPipe,
                                direction = Direction.None,
                                discharge = Math.Abs(pipeFlow.m_FreshFlow),
                                elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                                end = 1f,
                                endRoundaboutIndex = -1,
                                form = Form.Normal,
                                isRoundabout = false,
                                length = curve.m_Length,
                                limit = 0f,
                                load = 0f,
                                start = 0f,
                                startRoundaboutIndex = -1,
                                volume = 0f,
                                width = 0f
                            };

                            switch (pipeFlow.m_FreshFlow)
                            {
                                case > 0:
                                    water.direction = Direction.Forward;
                                    break;

                                case < 0:
                                    water.direction = Direction.Backward;
                                    break;

                                default:
                                    break;
                            }

                            if(hasComposition)
                            {
                                if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositiondata))
                                {
                                    CompositionFlags.General generalCompositions = netCompositiondata.m_Flags.m_General;
                                    if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) water.form = Form.Tunnel;
                                }

                                bool pieceFound = false;
                                float pieceWidth = 1.5f; // The vanilla water pipe's width.（原版遊戲自來水管的寬度。）

                                if (netCompositionPieceBufferLookup.TryGetBuffer(networkComposition, out DynamicBuffer<NetCompositionPiece> netCompositionPieces))
                                {
                                    for (int i = 0; i < netCompositionPieces.Length; i++)
                                    {
                                        if (netPieceLaneBufferLookup.TryGetBuffer(netCompositionPieces[i].m_Piece, out DynamicBuffer<NetPieceLane> netPieceLanes))
                                        {
                                            for (int j = 0; j < netPieceLanes.Length; j++)
                                            {
                                                if (lanesEntityMap.TryGetValue(netPieceLanes[j].m_Lane, out Domain.Lane lane) && lane.category == NetworkCategory.WaterPipe)
                                                {
                                                    pieceFound = true;
                                                    pieceWidth = Math.Abs(netCompositionPieces[i].m_Size.x);
                                                    break;
                                                }
                                            }
                                        }

                                        if (pieceFound) break;
                                    }

                                    water.width = pieceWidth;
                                }
                            }

                            list.AddNoResize(water);
                        }

                        if (pipeFlow.m_SewageCapacity > 0)
                        {
                            NetworkStat sewage = new()
                            {
                                entity = network,
                                capacity = pipeFlow.m_SewageCapacity,
                                category = NetworkCategory.SewagePipe,
                                direction = Direction.None,
                                discharge = Math.Abs(pipeFlow.m_SewageFlow),
                                elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                                end = 1f,
                                endRoundaboutIndex = -1,
                                form = Form.Normal,
                                isRoundabout = false,
                                length = curve.m_Length,
                                limit = 0f,
                                load = 0f,
                                start = 0f,
                                startRoundaboutIndex = -1,
                                volume = 0f,
                                width = 0f
                            };

                            switch (pipeFlow.m_SewageFlow)
                            {
                                // The default direction of the sewage water is opposite to that of fresh water.（汙水的方向與自來水的方向預設相反。）
                                case > 0:
                                    sewage.direction = Direction.Backward;
                                    break;

                                case < 0:
                                    sewage.direction = Direction.Forward;
                                    break;

                                default:
                                    break;
                            }

                            if (hasComposition)
                            {
                                if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositiondata))
                                {
                                    CompositionFlags.General generalCompositions = netCompositiondata.m_Flags.m_General;
                                    if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) sewage.form = Form.Tunnel;
                                }

                                bool pieceFound = false;
                                float pieceWidth = 2f; // The vanilla sewage pipe's width.（原版遊戲污水管的寬度。）

                                if (netCompositionPieceBufferLookup.TryGetBuffer(networkComposition, out DynamicBuffer<NetCompositionPiece> netCompositionPieces))
                                {
                                    for (int i = 0; i < netCompositionPieces.Length; i++)
                                    {
                                        if (netPieceLaneBufferLookup.TryGetBuffer(netCompositionPieces[i].m_Piece, out DynamicBuffer<NetPieceLane> netPieceLanes))
                                        {
                                            for (int j = 0; j < netPieceLanes.Length; j++)
                                            {
                                                if (lanesEntityMap.TryGetValue(netPieceLanes[j].m_Lane, out Domain.Lane lane) && lane.category == NetworkCategory.WaterPipe)
                                                {
                                                    pieceFound = true;
                                                    pieceWidth = Math.Abs(netCompositionPieces[i].m_Size.x);
                                                    break;
                                                }
                                            }
                                        }

                                        if (pieceFound) break;
                                    }

                                    sewage.width = pieceWidth;
                                }
                            }

                            list.AddNoResize(sewage);
                        }

                        // TODO: Uncomment the line when the storm pipes are added... if (pipeFlow.m_StormCapacity > 0)
                    }

                    // Generate the `NetworkStat` for the cable.（產生電纜的 `NetworkStat`。）
                    if (isPowerLine && electricityFlowEdgeLookup.TryGetComponent(cableEdges[0].m_Edge, out ElectricityFlowEdge cableFlow))
                    {
                        NetworkStat cable = new()
                        {
                            entity = network,
                            capacity = cableFlow.m_Capacity / 10f,
                            category = NetworkCategory.None,
                            direction = Direction.None,
                            discharge = 0f,
                            elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                            end = 1f,
                            endRoundaboutIndex = -1,
                            form = Form.Elevated, // The above ground cables are always overhead-ed.（地表之上的電纜是架空的。）
                            isRoundabout = false,
                            length = curve.m_Length,
                            limit = 0f,
                            load = Math.Abs(cableFlow.m_Flow) / 10f,
                            start = 0f,
                            startRoundaboutIndex = -1,
                            volume = 0f,
                            width = 0f
                        };

                        for (int i = 0; i < subLanes.Length; i++)
                        {
                            Entity subLane = subLanes[i].m_SubLane;
                            if (prefabRefLookup.TryGetComponent(subLane, out PrefabRef subLanePrefab) && lanesEntityMap.TryGetValue(subLanePrefab.m_Prefab, out Domain.Lane laneType) && laneType.IsUtilityLane)
                            {
                                cable.category |= laneType.category;
                            }
                        }

                        switch (cableFlow.m_Flow)
                        {
                            case > 0:
                                cable.direction = Direction.Forward;
                                break;

                            case < 0:
                                cable.direction = Direction.Backward;
                                break;

                            default:
                                break;
                        }

                        if (hasComposition)
                        {
                            if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositiondata))
                            {
                                CompositionFlags.General generalCompositions = netCompositiondata.m_Flags.m_General;
                                if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) cable.form = Form.Tunnel;
                                cable.width = netCompositiondata.m_Width;
                            }
                        }

                        list.AddNoResize(cable);
                    }
                }

                // TODO: Handle the integrated utility service networks.（處理整合公共管線網路。）
                /*
                //if (hasUtilityServiceNetworks && isNotUtilityServiceNetwork)
                //{
                //    Entity startNode = edge.m_Start;
                //    Entity endNode = edge.m_End;
                    
                //    // Generate the `NetworkStat` for the cable.（產生電纜的 `NetworkStat`。）
                //    if (electricityNodeConnectionLookup.TryGetComponent(network, out ElectricityNodeConnection electricityNodeCenter) &&
                //        electricityNodeConnectionLookup.TryGetComponent(startNode, out ElectricityNodeConnection electricityNodeStart) &&
                //        electricityNodeConnectionLookup.TryGetComponent(endNode, out ElectricityNodeConnection electricityNodeEnd) &&
                //        connectedFlowEdgeBufferLookup.TryGetBuffer(electricityNodeCenter.m_ElectricityNode, out DynamicBuffer<ConnectedFlowEdge> electricityFlows) &&
                //        electricityFlows.Length >= 2)
                //    {
                //        ElectricityFlowEdge electricityFlowFromStart = default;
                //        ElectricityFlowEdge electricityFlowToEnd = default;

                //        for (int i = 0; i < electricityFlows.Length; i++)
                //        {
                //            if (TryGetElectricityFlowEdge(electricityNodeStart.m_ElectricityNode,
                //                                          electricityNodeCenter.m_ElectricityNode,
                //                                          ref connectedFlowEdgeBufferLookup,
                //                                          ref electricityFlowEdgeLookup,
                //                                          out ElectricityFlowEdge electricityFlowA))
                //            {
                //                electricityFlowFromStart = electricityFlowA;
                //            }
                //            if (TryGetElectricityFlowEdge(electricityNodeCenter.m_ElectricityNode,
                //                                          electricityNodeEnd.m_ElectricityNode,
                //                                          ref connectedFlowEdgeBufferLookup,
                //                                          ref electricityFlowEdgeLookup,
                //                                          out ElectricityFlowEdge electricityFlowB))
                //            {
                //                electricityFlowToEnd = electricityFlowB;
                //            }
                //        }

                //        FixedList4096Bytes<float> curvePositions = default;

                //        if ((electricityFlowFromStart.m_Start == electricityNodeStart.m_ElectricityNode) &&
                //            (electricityFlowToEnd.m_End == electricityNodeEnd.m_ElectricityNode) &&
                //            connectedBuildingBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedBuilding> connectedBuildings))
                //        {
                //            for (int i = 0; i < connectedBuildings.Length; i++)
                //            {
                //                Entity buildingEntity = connectedBuildings[i].m_Building;
                //                if ((!buildingLookup.TryGetComponent(buildingEntity, out Building buildingComponent) &&
                //                     !electricityConsumerLookup.TryGetComponent(buildingEntity, out ElectricityConsumer electricityConsumer)) ||
                //                    placeholderLookup.TryGetComponent(buildingEntity, out _))
                //                {
                //                    continue;
                //                }

                //                curvePositions.Add(buildingComponent.m_CurvePosition);
                //            }
                //        }
                //    }
                //}
                */
            }
        }

        /// <summary>
        /// The job to collect roundabouts.
        /// （收集圓環的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectRoundaboutsJob : IJobEntity
        {
            [ReadOnly]
            public bool hasCenterline;

            [ReadOnly]
            public bool leftHandTraffic;
            
            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<NetGeometryData> netGeometryDataLookup;

            [ReadOnly]
            public ComponentLookup<ObjectGeometryData> objectGeometryDataLookup;

            [ReadOnly]
            public ComponentLookup<PlaceableObjectData> placeableObjectDataLookup;

            [ReadOnly]
            public ComponentLookup<Road> roadLookup;

            [ReadOnly]
            public ComponentLookup<RoadComposition> roadCompositionLookup;

            [ReadOnly]
            public ComponentLookup<SubwayTrack> subwayTrackLookup;

            [ReadOnly]
            public ComponentLookup<TrackComposition> trackCompositionLookup;

            [ReadOnly]
            public ComponentLookup<TrainTrack> trainTrackLookup;

            [ReadOnly]
            public ComponentLookup<TramTrack> tramTrackLookup;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Entity> attachmentEntityMap;

            [WriteOnly]
            public NativeList<Domain.Roundabout>.ParallelWriter list;

            [WriteOnly]
            public NativeList<NetworkStat>.ParallelWriter stats;

            public void Execute(in Node node, in PrefabRef prefabRef, in Game.Net.Roundabout roundaboutComponent, in DynamicBuffer<ConnectedEdge> connectedEdges, in DynamicBuffer<Game.Objects.SubObject> subObjects, Entity entity)
            {
                bool allTrackConnection = true;
                bool allTunnelConnection = true;
                bool highwayConnection = false;
                float innerRadius = 0f;
                float roadLimit = 0f;
                float trackLimit = 0f;
                float width = 0f;
                Form form = Form.Normal;

                Domain.Roundabout roundabout = new()
                {
                    attached = attachmentEntityMap.TryGetValue(entity, out Entity attachment) ? attachment : Entity.Null,
                    innerRingRadius = 0f,
                    node = entity,
                    outerRingRadius = roundaboutComponent.m_Radius,
                    width = 0f
                };

                // Reference（參考資料）: `Game.Net.GeometrySystem.CalculateEdgeGeometryJob.CalculateMiddleRadius`
                for (int i = 0; i < subObjects.Length; i++)
                {
                    Entity subObject = subObjects[i].m_SubObject;
                    if (placeableObjectDataLookup.TryGetComponent(subObject, out PlaceableObjectData placeableObjectData) &&
                        ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.RoadNode) != 0) &&
                        objectGeometryDataLookup.TryGetComponent(subObject, out ObjectGeometryData objectGeometryData))
                    {
                        float subObjectRadius = math.cmax(objectGeometryData.m_Size.xz) / 2f;

                        if (((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != 0) &&
                            netGeometryDataLookup.TryGetComponent(prefabRef.m_Prefab, out NetGeometryData netGeometryData))
                        {
                            float lyingRadius = math.cmax(objectGeometryData.m_LegSize.xz) / 2f;

                            if (netGeometryData.m_DefaultHeightRange.max > objectGeometryData.m_LegSize.y)
                            {
                                subObjectRadius = math.max(subObjectRadius, lyingRadius);
                            }
                            else
                            {
                                subObjectRadius = lyingRadius;
                            }
                        }

                        innerRadius = math.max(innerRadius, subObjectRadius);
                    }
                }

                for (int i = 0; i < connectedEdges.Length; i++)
                {
                    Entity connectedNetwork = connectedEdges[i].m_Edge;
                    if (!roadLookup.TryGetComponent(connectedNetwork, out _) &&
                        !tramTrackLookup.TryGetComponent(connectedNetwork, out _) &&
                        !subwayTrackLookup.TryGetComponent(connectedNetwork, out _) &&
                        !trainTrackLookup.TryGetComponent(connectedNetwork, out _))
                    {
                        continue;
                    }

                    if (compositionLookup.TryGetComponent(connectedNetwork, out Composition connectedComposition) &&
                        netCompositionDataLookup.TryGetComponent(connectedComposition.m_Edge, out NetCompositionData connectedCompositionData))
                    {
                        float compositionWidth = connectedCompositionData.m_Width / 2f;
                        width = math.max(width, compositionWidth);

                        Form connectedForm = GetForm(connectedCompositionData);
                        if (allTunnelConnection && connectedForm != Form.Tunnel)
                        {
                            allTunnelConnection = false;
                            form = connectedForm;
                        }
                        if ((form != Form.Elevated) && connectedForm == Form.Elevated)
                        {
                            form = connectedForm;
                        }

                        if (roadCompositionLookup.TryGetComponent(connectedComposition.m_Edge, out RoadComposition roadComposition))
                        {
                            allTrackConnection = false;
                            highwayConnection |= (roadComposition.m_Flags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0;
                            roadLimit = math.max(roadLimit, RoundSpeedLimit(roadComposition.m_SpeedLimit));
                        }

                        if (trackCompositionLookup.TryGetComponent(connectedComposition.m_Edge, out TrackComposition trackComposition))
                        {
                            trackLimit = math.max(trackLimit, RoundSpeedLimit(trackComposition.m_SpeedLimit));
                        }
                    }
                }

                roundabout.innerRingRadius = innerRadius > 0 ? innerRadius : roundabout.outerRingRadius - width;
                roundabout.width = width;

                list.AddNoResize(roundabout);

                if (!hasCenterline) return;

                // Append roundabout's statistics.（添加圓環的統計資訊。）
                NetworkStat stat = new()
                {
                    entity = entity,
                    capacity = 0f,
                    category = highwayConnection ? NetworkCategory.Highway : NetworkCategory.Car,
                    direction = leftHandTraffic ? Direction.Forward : Direction.Backward,
                    discharge = 0f,
                    elevation = node.m_Position.y,
                    end = 1f,
                    endRoundaboutIndex = -1,
                    form = allTunnelConnection ? Form.Tunnel : form,
                    isRoundabout = true,
                    length = 2 * math.PI * (roundabout.outerRingRadius - width / 2),
                    limit = allTrackConnection ? trackLimit : roadLimit,
                    load = 0f,
                    start = 0f,
                    startRoundaboutIndex = -1,
                    volume = roadLookup.TryGetComponent(entity, out Road road) ? math.max(0f, GetVolume(road)) : 0f,
                    width = width
                };

                stats.AddNoResize(stat);
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
        /// The job to count the number of segments required to represent the individual (NOT included in the roads) utility service networks.
        /// （計算表達獨立公共服務管線（未包含在道路中的）所需路段數量的工作。）
        /// </summary>
        public partial struct CountIndividualUtilityServiceNetworkSegmentsJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<Game.Net.ElectricityConnection> electricityConnectionLookup;
            
            [ReadOnly]
            public ComponentLookup<PipelineData> pipeLineDataLookup;

            [ReadOnly]
            public ComponentLookup<PowerLineData> powerLineDataLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.WaterPipeConnection> waterPipeConnectionLookup;

            [WriteOnly]
            public NativeQueue<int>.ParallelWriter queue;

            public void Execute(in PrefabRef prefabRef, Entity network)
            {
                int count = 0;
                Entity prefab = prefabRef.m_Prefab;

                if (pipeLineDataLookup.TryGetComponent(prefab, out _) && waterPipeConnectionLookup.TryGetComponent(network, out Game.Net.WaterPipeConnection waterPipeData))
                {
                    if (waterPipeData.m_FreshCapacity > 0) count++;
                    if (waterPipeData.m_SewageCapacity > 0) count++;
                    // TODO: Uncomment the line when the storm pipes are added... if (waterPipeData.m_StormCapacity > 0) count++;
                }

                if (powerLineDataLookup.TryGetComponent(prefab, out _) && electricityConnectionLookup.TryGetComponent(network, out _))
                {
                    count++;
                }

                queue.Enqueue(count);
            }
        }

        /// <summary>
        /// The job to count the number of segments required to represent the integrated (included in the roads) utility service networks.
        /// （計算表達整合公共服務管線（包含在道路中的）所需路段數量的工作。）
        /// </summary>
        public partial struct CountIntegratedUtilityServiceNetworkSegmentsJob : IJobEntity
        {
            [ReadOnly]
            public BufferLookup<ConnectedBuilding> connectedBuildingBufferLookup;
            
            [ReadOnly]
            public BufferLookup<ConnectedFlowEdge> connectedFlowEdgeBufferLookup;

            [ReadOnly]
            public BufferLookup<ConnectedNode> connectedNodeBufferLookup;

            [ReadOnly]
            public ComponentLookup<Building> buildingLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityBuildingConnection> electricityBuildingConnectionLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityConsumer> electricityConsumerLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityFlowEdge> electricityFlowEdgeLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityNodeConnection> electricityNodeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<Placeholder> placeholderLookup;

            [ReadOnly]
            public ComponentLookup<WaterConsumer> waterConsumerLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.WaterPipeConnection> waterPipeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<WaterPipeNodeConnection> waterPipeNodeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<WaterPipeEdge> waterPipeEdgeLookup;
            
            [WriteOnly]
            public NativeQueue<int>.ParallelWriter queue;

            public void Execute(Entity network)
            {
                bool hasCable = electricityNodeConnectionLookup.TryGetComponent(network, out ElectricityNodeConnection electricityConnCenter);
                bool hasPipe = waterPipeNodeConnectionLookup.TryGetComponent(network, out WaterPipeNodeConnection waterPipeConnCenter);
                if (!hasCable && !hasPipe) return;

                int count = 0;

                if (hasCable)
                {
                    count++;

                    // Handle the connection between user-drawn cables.（處理與使用者繪製的電纜相接處。）
                    if (connectedNodeBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedNode> connectedNodes))
                    {
                        for (int i = 0; i < connectedNodes.Length; i++)
                        {
                            ConnectedNode connectedNode = connectedNodes[i];
                            if (electricityNodeConnectionLookup.TryGetComponent(connectedNode.m_Node, out ElectricityNodeConnection electricityConnStart) &&
                                TryGetElectricityFlowEdge(electricityConnStart.m_ElectricityNode,
                                                          electricityConnCenter.m_ElectricityNode,
                                                          ref connectedFlowEdgeBufferLookup,
                                                          ref electricityFlowEdgeLookup,
                                                          out _))
                            {
                                count++;
                            }
                        }
                    }

                    // Handle building connections.（處理建築連結。）
                    if (connectedBuildingBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedBuilding> connectedBuildings))
                    {
                        /* 
                         * The list can hold at least 1000 floats (4,000 bytes).
                         * This design ensures even when the user stretches the network really long [with Mod] and creates lots of connected buildings, the list capacity is still enough in most extreme cases.
                         * （列表可以裝下至少 1000 個單精度浮點數（4,000 位元組）。）
                         * （這個設計確保即使使用者［透過模組］將路段拉得非常長，並創造許多相連建築時，列表容量依然充裕。）
                         */
                        FixedList4096Bytes<float> curvePositions = default;
                        int uniqueBuildingsCount = 0;

                        for (int i = 0; i < connectedBuildings.Length; i++)
                        {
                            Entity connectedBuilding = connectedBuildings[i].m_Building;
                            if (buildingLookup.TryGetComponent(connectedBuilding, out Building buildingComponent) &&
                                electricityConsumerLookup.TryGetComponent(connectedBuilding, out _) &&
                                !placeholderLookup.TryGetComponent(connectedBuilding, out _))
                            {
                                bool recorded = false;
                                float curvePosition = buildingComponent.m_CurvePosition;
                                for (int j = 0; j < curvePositions.Length; j++)
                                {
                                    if (curvePositions[j] == curvePosition)
                                    {
                                        recorded = true;
                                        break;
                                    }
                                }

                                if (!recorded)
                                {
                                    curvePositions.Add(curvePosition);
                                }

                                uniqueBuildingsCount++;
                            }
                        }

                        // For each connected building, it splits an existing cable and create a cable to the building.
                        //（每棟相接的建築皆會切分既有的電纜，並且創造連接建築的電纜。）
                        count += curvePositions.Length + uniqueBuildingsCount;
                    }
                }

                if (hasPipe)
                {
                    int pipeTypes = 0;
                    if (waterPipeConnectionLookup.TryGetComponent(network, out Game.Net.WaterPipeConnection waterPipeData))
                    {
                        if (waterPipeData.m_FreshCapacity > 0) pipeTypes++;
                        if (waterPipeData.m_SewageCapacity > 0) pipeTypes++;
                        count += pipeTypes;
                    }

                    // Handle the connection between user-drawn pipes.（處理與使用者繪製的水管相接處。）
                    if (connectedNodeBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedNode> connectedNodes))
                    {
                        for (int i = 0; i < connectedNodes.Length; i++)
                        {
                            ConnectedNode connectedNode = connectedNodes[i];
                            if (waterPipeNodeConnectionLookup.TryGetComponent(connectedNode.m_Node, out WaterPipeNodeConnection waterPipeConnStart) &&
                                TryGetWaterPipeEdge(waterPipeConnStart.m_WaterPipeNode,
                                                    waterPipeConnCenter.m_WaterPipeNode,
                                                    ref connectedFlowEdgeBufferLookup,
                                                    ref waterPipeEdgeLookup,
                                                    out _))
                            {
                                count++;
                            }
                        }
                    }

                    // Handle building connections.（處理建築連結。）
                    if (connectedBuildingBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedBuilding> connectedBuildings))
                    {
                        FixedList4096Bytes<float> curvePositions = default;
                        int uniqueBuildingsCount = 0;

                        for (int i = 0; i < connectedBuildings.Length; i++)
                        {
                            Entity connectedBuilding = connectedBuildings[i].m_Building;
                            if (buildingLookup.TryGetComponent(connectedBuilding, out Building buildingComponent) &&
                                waterConsumerLookup.TryGetComponent(connectedBuilding, out _) &&
                                !placeholderLookup.TryGetComponent(connectedBuilding, out _))
                            {
                                bool recorded = false;
                                float curvePosition = buildingComponent.m_CurvePosition;
                                for (int j = 0; j < curvePositions.Length; j++)
                                {
                                    if (curvePositions[j] == curvePosition)
                                    {
                                        recorded = true;
                                        break;
                                    }
                                }

                                if (!recorded)
                                {
                                    curvePositions.Add(curvePosition);
                                }

                                uniqueBuildingsCount++;
                            }
                        }

                        // For each connected building, it splits existing pipes and create pipes to the building.
                        //（每棟相接的建築皆會切分既有的水管，並且創造連接建築的水管。）
                        count += (curvePositions.Length + uniqueBuildingsCount) * pipeTypes;
                    }
                }

                queue.Enqueue(count);
            }
        }

        /// <summary>
        /// The job to map roundabout attachements to a native hashmap.
        /// （將圓環附件映射至原生映射表的工作。）
        /// </summary>
        public partial struct MapRoundaboutAttachmentsJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, Entity>.ParallelWriter entityMap;

            public void Execute(in Attached attached, Entity attachment)
            {
                entityMap.TryAdd(attached.m_Parent, attachment);
            }
        }

        /// <summary>
        /// The job to map roundabout node instance to theor index in <see cref="_roundabouts"/>.
        /// （將圓環節點個體映射至在 <see cref="_roundabouts"/> 的索引的工作。）
        /// </summary>
        public partial struct MapRoundaboutsIndexJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeList<Domain.Roundabout> list;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int> map;

            public void Execute(int index)
            {
                map.TryAdd(list[index].node, index);
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
    }
}