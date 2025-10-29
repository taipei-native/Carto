using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Tools;
using Game.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Carto.Systems
{
    /// <summary>
    /// The system that searches POIs (point of interest).
    /// （搜尋興趣點（POI）的系統。）
    /// </summary>
    public partial class POISystem : GameSystemBase
    {
        /// <summary>
        /// The system that searches areas.（搜尋區域的系統。）<br/>
        /// See <see cref="Instance.Area"/> for more information.
        /// </summary>
        static readonly AreaSystem _area = Instance.Area;
        
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The wrapper managing names.（管理名稱的包裝器。）<br/>
        /// See <see cref="Instance.Name"/> for more information.
        /// </summary>
        static readonly NameManager _name = Instance.Name;

        /// <summary>
        /// The system collecting shared data.（收集共享資料的系統。）<br/>
        /// See <see cref="Instance.Shared"/> for more information.
        /// </summary>
        static readonly SharedDataCollectionSystem _shared = Instance.Shared;

        /// <summary>
        /// The query to collect all districts.
        /// （收集所有遊戲中行政區的查詢。）
        /// </summary>
        static EntityQuery _districtQuery;

        /// <summary>
        /// The query to collect helipads.
        /// （收集停機坪的查詢。）
        /// </summary>
        static EntityQuery _helipadQuery;

        /// <summary>
        /// The query to collect park prefabs.
        /// （收集公園預製模板的查詢。）
        /// </summary>
        static EntityQuery _parkPrefabQuery;

        /// <summary>
        /// The query to collect pylons and utility poles.
        /// （收集電塔與電線杆的查詢。）
        /// </summary>
        static EntityQuery _pylonQuery;

        /// <summary>
        /// The query to collect traffic lights.
        /// （收集交通號誌燈的查詢。）
        /// </summary>
        static EntityQuery _trafficLightQuery;

        /// <summary>
        /// The query to collect transport stops.
        /// （收集運輸站點的查詢。）
        /// </summary>
        static EntityQuery _transportStopMarkerQuery;

        /// <summary>
        /// The query to collect transport stops' prefabs.
        /// （收集運輸站點預製模板的查詢。）
        /// </summary>
        static EntityQuery _transportStopMarkerPrefabQuery;

        /// <summary>
        /// The query to collect utility object's prefabs.
        /// （收集公用事業物件預製模板的查詢。）
        /// </summary>
        static EntityQuery _utilityObjectPrefabQuery;

        /// <summary>
        /// The list of <see cref="POI"/>s stored in the local system.
        /// （儲存於本地系統的 <see cref="POI"/> 列表。）
        /// </summary>
        private NativeList<POI> _localPOIs;

        /// <summary>
        /// The map between POI entities and their categories.
        /// （興趣點實體與其分類的映射表。）
        /// </summary>
        private NativeParallelHashMap<Entity, NativeParallelHashSet<EnumWrapper<POICategory>>> _categoryEntityMap;

        /// <summary>
        /// The object types that can be considered as POIs.
        /// （可被視為興趣點的物件類別。）
        /// </summary>
        private const Feature POIObjects = Feature.POIPrivate | Feature.POIPublic | Feature.POITransport | Feature.POIUtility;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _districtQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>(),
                    ComponentType.ReadOnly<District>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _helipadQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<SpawnLocation>(),
                    ComponentType.ReadOnly<Game.Routes.TakeoffLocation>(),
                    ComponentType.ReadOnly<Game.Objects.Transform>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _parkPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Prefabs.ParkData>(),
                    ComponentType.ReadOnly<Game.Prefabs.UIObjectData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _pylonQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Pillar>(),
                    ComponentType.ReadOnly<Game.Prefabs.PrefabRef>(),
                    ComponentType.ReadOnly<Game.Objects.Transform>(),
                    ComponentType.ReadOnly<UtilityObject>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _trafficLightQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ConnectedEdge>(),
                    ComponentType.ReadOnly<Game.Net.Node>(),
                    ComponentType.ReadOnly<TrafficLights>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _transportStopMarkerQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Prefabs.PrefabRef>(),
                    ComponentType.ReadOnly<Game.Routes.TransportStop>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _transportStopMarkerPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Prefabs.TransportStopData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _utilityObjectPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Prefabs.UtilityObjectData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            
            base.OnCreate();
            _log.Debug("POISystem instance created. 興趣點系統實例創造完成。");
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
        /// The container for the prefab's UI group information.
        /// （預製模板的 UI 組別資訊容器。）
        /// </summary>
        public struct PrefabUIGroup
        {
            /// <summary>
            /// The prefab entity.
            /// （預製模板實體。）
            /// </summary>
            public Entity prefab;

            /// <summary>
            /// The UI Group entity.
            /// （UI 組別實體。）
            /// </summary>
            public Entity uiGroup;
        }

        /// <summary>
        /// Retrieve the general POIs.
        /// （獲得一般的興趣點。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="buildingStats">The list of building statistics.（建築統計的列表。）</param>
        /// <param name="zoningTypes">The list of zoning types.（分區類型的列表。）</param>
        private void CollectGeneralPOIs(Options options, ref NativeList<BuildingStat> buildingStats, ref NativeList<ZoningType> zoningTypes)
        {
            IOUtils.GetTargetProjections(options, out Geodata.CRS targetCRS, out ProjectionDefinition targetProjection);

            // Collect the prefabs of parks that can be regarded as attractions.（收集可被視為地標的公園預製模板。）
            NativeParallelHashSet<Entity> attractions = new(_parkPrefabQuery.CalculateEntityCount(), Allocator.Persistent);
            GetAttractionPrefabs(ref attractions);

            CollectPOIsFromBuildingStatsJob collectFromBuildingStatsJob = new()
            {
                separateServiceUpgrade = options.SeparateServiceUpgrade,
                installedUpgradeBufferLookup = GetBufferLookup<InstalledUpgrade>(true),
                subObjectBufferLookup = GetBufferLookup<SubObject>(true),
                abandonedLookup = GetComponentLookup<Abandoned>(true),
                adminBuildingLookup = GetComponentLookup<AdminBuilding>(true),
                batteryLookup = GetComponentLookup<Battery>(true),
                commercialPropertyLookup = GetComponentLookup<CommercialProperty>(true),
                condemnedLookup = GetComponentLookup<Condemned>(true),
                customNameLookup = GetComponentLookup<CustomName>(true),
                deathcareFacilityLookup = GetComponentLookup<DeathcareFacility>(true),
                deathcareFacilityDataLookup = GetComponentLookup<Game.Prefabs.DeathcareFacilityData>(true),
                destroyedLookup = GetComponentLookup<Game.Common.Destroyed>(true),
                disasterFacilityLookup = GetComponentLookup<DisasterFacility>(true),
                earlyDisasterWarningSystemLookup = GetComponentLookup<EarlyDisasterWarningSystem>(true),
                electricityProducerLookup = GetComponentLookup<ElectricityProducer>(true),
                emergencyShelterLookup = GetComponentLookup<EmergencyShelter>(true),
                extractorFacilityLookup = GetComponentLookup<ExtractorFacility>(true),
                fireStationLookup = GetComponentLookup<FireStation>(true),
                firewatchTowerLookup = GetComponentLookup<FirewatchTower>(true),
                garbageFacilityLookup = GetComponentLookup<GarbageFacility>(true),
                hospitalLookup = GetComponentLookup<Hospital>(true),
                industrialPropertyLookup = GetComponentLookup<IndustrialProperty>(true),
                maintenanceDepotLookup = GetComponentLookup<MaintenanceDepot>(true),
                nativeLookup = GetComponentLookup<Game.Common.Native>(true),
                ownerLookup = GetComponentLookup<Game.Common.Owner>(true),
                parkLookup = GetComponentLookup<Park>(true),
                parkingFacilityLookup = GetComponentLookup<ParkingFacility>(true),
                prefabRefLookup = GetComponentLookup<Game.Prefabs.PrefabRef>(true),
                policeStationLookup = GetComponentLookup<PoliceStation>(true),
                postFacilityLookup = GetComponentLookup<PostFacility>(true),
                prisonLookup = GetComponentLookup<Prison>(true),
                researchFacilityLookup = GetComponentLookup<ResearchFacility>(true),
                residentialPropertyLookup = GetComponentLookup<ResidentialProperty>(true),
                schoolLookup = GetComponentLookup<School>(true),
                schoolDataLookup = GetComponentLookup<Game.Prefabs.SchoolData>(true),
                serviceDataLookup = GetComponentLookup<Game.Prefabs.ServiceData>(true),
                serviceUpgradeLookup = GetComponentLookup<ServiceUpgrade>(true),
                sewageOutletLookup = GetComponentLookup<SewageOutlet>(true),
                storagePropertyLookup = GetComponentLookup<StorageProperty>(true),
                telecomFacilityLookup = GetComponentLookup<TelecomFacility>(true),
                transformLookup = GetComponentLookup<Game.Objects.Transform>(true),
                transformerLookup = GetComponentLookup<Transformer>(true),
                transportDepotLookup = GetComponentLookup<TransportDepot>(true),
                transportDepotDataLookup = GetComponentLookup<Game.Prefabs.TransportDepotData>(true),
                transportStationLookup = GetComponentLookup<TransportStation>(true),
                transportStopDataLookup = GetComponentLookup<Game.Prefabs.TransportStopData>(true),
                underConstructionLookup = GetComponentLookup<UnderConstruction>(true),
                waterPoweredDataLookup = GetComponentLookup<Game.Prefabs.WaterPoweredData>(true),
                waterPumpingStationLookup = GetComponentLookup<WaterPumpingStation>(true),
                welfareOfficeLookup = GetComponentLookup<WelfareOffice>(true),
                windPoweredDataLookup = GetComponentLookup<Game.Prefabs.WindPoweredData>(true),
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                buildingStats = buildingStats,
                zoningTypes = zoningTypes,
                attractions = attractions,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                POIs = _localPOIs.AsParallelWriter(),
                POICategories = _categoryEntityMap.AsParallelWriter()
            };
            JobHandle collectFromBuildingStatsHandle = collectFromBuildingStatsJob.Schedule(buildingStats.Length, 32);
            collectFromBuildingStatsHandle.Complete();

            CommonUtils.Dispose(ref attractions);
        }

        /// <summary>
        /// Retrieve the transport-related POIs.
        /// （獲得運輸相關的興趣點。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        private void CollectTransportPOIs(Options options)
        {
            bool hasAddress = options.Contains(Property.Address, IO.System.POI);
            IOUtils.GetTargetProjections(options, out Geodata.CRS targetCRS, out ProjectionDefinition targetProjection);
            NativeParallelHashMap<Entity, Game.Prefabs.TransportStopData> transportStopDataMap = new(_transportStopMarkerPrefabQuery.CalculateEntityCount(), Allocator.Persistent);

            CollectHelipadsJob collectHelipadsJob = new()
            {
                useAddress = hasAddress,
                aggregateElementBufferLookup = GetBufferLookup<AggregateElement>(true),
                aggregatedLookup = GetComponentLookup<Aggregated>(true),
                buildingLookup = GetComponentLookup<Building>(true),
                buildingDataLookup = GetComponentLookup<Game.Prefabs.BuildingData>(true),
                compositionLookup = GetComponentLookup<Composition>(true),
                curveLookup = GetComponentLookup<Curve>(true),
                edgeLookup = GetComponentLookup<Edge>(true),
                netCompositionDataLookup = GetComponentLookup<Game.Prefabs.NetCompositionData>(true),
                ownerLookup = GetComponentLookup<Game.Common.Owner>(true),
                prefabRefLookup = GetComponentLookup<Game.Prefabs.PrefabRef>(true),
                roundaboutLookup = GetComponentLookup<Game.Net.Roundabout>(true),
                transformLookup = GetComponentLookup<Game.Objects.Transform>(true),
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                POIs = _localPOIs.AsParallelWriter(),
                POICategories = _categoryEntityMap.AsParallelWriter()
            };
            JobHandle collectHelipadsHandle = collectHelipadsJob.ScheduleParallel(_helipadQuery, default);
            collectHelipadsHandle.Complete();

            CollectTrafficLightsJob collectTrafficLightsJob = new()
            {
                useAddress = hasAddress,
                aggregateElementBufferLookup = GetBufferLookup<AggregateElement>(true),
                aggregatedLookup = GetComponentLookup<Aggregated>(true),
                buildingDataLookup = GetComponentLookup<Game.Prefabs.BuildingData>(true),
                compositionLookup = GetComponentLookup<Composition>(true),
                curveLookup = GetComponentLookup<Curve>(true),
                edgeLookup = GetComponentLookup<Edge>(true),
                netCompositionDataLookup = GetComponentLookup<Game.Prefabs.NetCompositionData>(true),
                prefabRefLookup = GetComponentLookup<Game.Prefabs.PrefabRef>(true),
                roadLookup = GetComponentLookup<Road>(true),
                roundaboutLookup = GetComponentLookup<Game.Net.Roundabout>(true),
                transformLookup = GetComponentLookup<Game.Objects.Transform>(true),
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                POIs = _localPOIs.AsParallelWriter(),
                POICategories = _categoryEntityMap.AsParallelWriter()
            };
            JobHandle collectTrafficLightsHandle = collectTrafficLightsJob.ScheduleParallel(_trafficLightQuery, default);
            collectTrafficLightsHandle.Complete();

            CollectTransportStopDataJob collectTransportStopDataJob = new()
            {
                map = transportStopDataMap.AsParallelWriter()
            };
            JobHandle collectTransportStopDataHnadle = collectTransportStopDataJob.ScheduleParallel(_transportStopMarkerPrefabQuery, default);
            collectTransportStopDataHnadle.Complete();

            CollectTransportStopMarkersJob collectTransportStopMarkersJob = new()
            {
                useAddress = hasAddress,
                aggregateElementBufferLookup = GetBufferLookup<AggregateElement>(true),
                aggregatedLookup = GetComponentLookup<Aggregated>(true),
                attachedLookup = GetComponentLookup<Attached>(true),
                buildingLookup = GetComponentLookup<Building>(true),
                buildingDataLookup = GetComponentLookup<Game.Prefabs.BuildingData>(true),
                compositionLookup = GetComponentLookup<Composition>(true),
                curveLookup = GetComponentLookup<Curve>(true),
                customNameLookup = GetComponentLookup<CustomName>(true),
                edgeLookup = GetComponentLookup<Edge>(true),
                netCompositionDataLookup = GetComponentLookup<Game.Prefabs.NetCompositionData>(true),
                ownerLookup = GetComponentLookup<Game.Common.Owner>(true),
                prefabRefLookup = GetComponentLookup<Game.Prefabs.PrefabRef>(true),
                roundaboutLookup = GetComponentLookup<Game.Net.Roundabout>(true),
                transformLookup = GetComponentLookup<Game.Objects.Transform>(true),
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                transportStopDataMap = transportStopDataMap,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                POIs = _localPOIs.AsParallelWriter(),
                POICategories = _categoryEntityMap.AsParallelWriter()
            };
            JobHandle collectTransportStopMarkersHandle = collectTransportStopMarkersJob.ScheduleParallel(_transportStopMarkerQuery, default);
            collectTransportStopMarkersHandle.Complete();

            CommonUtils.Dispose(ref transportStopDataMap);
        }

        /// <summary>
        /// Retrieve the utility-related POIs.
        /// （獲得公用事業相關的興趣點。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        private void CollectUtilityPOIs(Options options)
        {
            IOUtils.GetTargetProjections(options, out Geodata.CRS targetCRS, out ProjectionDefinition targetProjection);
            int utilityObjectPrefabCount = _utilityObjectPrefabQuery.CalculateEntityCount();
            NativeParallelHashSet<Entity> poles = new(utilityObjectPrefabCount, Allocator.Persistent);
            NativeParallelHashSet<Entity> pylons = new(utilityObjectPrefabCount, Allocator.Persistent);

            CollectPylonPrefabsJob collectPylonPrefabsJob = new()
            {
                poles = poles.AsParallelWriter(),
                pylons = pylons.AsParallelWriter()
            };
            JobHandle collectPylonPrefabsHandle = collectPylonPrefabsJob.ScheduleParallel(_utilityObjectPrefabQuery, default);
            collectPylonPrefabsHandle.Complete();

            CollectPylonsJob collectPylonsJob = new()
            {
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                poles = poles,
                pylons = pylons,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                POIs = _localPOIs.AsParallelWriter(),
                POICategories = _categoryEntityMap.AsParallelWriter()
            };
            JobHandle collectPylonsHandle = collectPylonsJob.ScheduleParallel(_pylonQuery, default);
            collectPylonsHandle.Complete();

            CommonUtils.Dispose(ref poles);
            CommonUtils.Dispose(ref pylons);
        }

        /// <summary>
        /// Try disposing of all properties stored in unmanaged memory.
        /// （嘗試丟棄儲存於未控管記憶體的屬性。）
        /// </summary>
        public void Dispose()
        {
            CommonUtils.Dispose(ref _categoryEntityMap);
            CommonUtils.Dispose(ref _localPOIs);
        }

        /// <summary>
        /// Retrieve the prefabs that can be considered attractions.
        /// （獲得可被視為景點的預製模板。）
        /// </summary>
        /// <param name="attractions">The set of prefab entities.（預製模板實體的集合。）</param>
        private void GetAttractionPrefabs(ref NativeParallelHashSet<Entity> attractions)
        {
            NativeList<PrefabUIGroup> parkPrefabs = new(_parkPrefabQuery.CalculateEntityCount(), Allocator.Persistent);

            CollectUIObjectGroups collectUIGroupJob = new()
            {
                list = parkPrefabs.AsParallelWriter()
            };
            JobHandle collectUIGroupHandle = collectUIGroupJob.ScheduleParallel(_parkPrefabQuery, default);
            collectUIGroupHandle.Complete();

            for (int i = 0; i < parkPrefabs.Length; i++)
            {
                PrefabUIGroup prefabSet = parkPrefabs[i];
                string uiGroupName = _name.GetPrefabName(prefabSet.uiGroup);
                switch (uiGroupName)
                {
                    // Pre-order Pack & Treasure Hunt buildings.（預購包及尋寶活動建築。）
                    case "SignaturesLandmarks":
                        attractions.Add(prefabSet.prefab);
                        break;

                    // Base game buildings.（主遊戲建築。）
                    case "TouristAttractions":
                        attractions.Add(prefabSet.prefab);
                        break;

                    default:
                        break;
                }
            }

            CommonUtils.Dispose(ref parkPrefabs);
        }

        /// <summary>
        /// Retrieve the string of comma-separated list of <see cref="POICategory"/>.
        /// （獲得逗號分隔的 <see cref="POICategory"/> 列表字串。）
        /// </summary>
        /// <param name="categories">The input array.（輸入的陣列。）</param>
        /// <returns>The comma-separated string.（逗號分隔字串。）</returns>
        private static string GetCategoryString(POICategory[] categories)
        {
            StringBuilder POICategoryNames = new();

            for (int i = 0; i < categories.Length; i++)
            {
                if (categories[i] == POICategory.None)
                {
                    continue;
                }

                POICategoryNames.Append(categories[i].ToString("G"));

                if (i < categories.Length - 1)
                {
                    POICategoryNames.Append(", ");
                }
            }

            return POICategoryNames.ToString();
        }

        /// <summary>
        /// Retrieve the maximum possible number of <see cref="POICategory"/> from a <see cref="BuildingCategory"/>.<br/>
        /// （由 <see cref="BuildingCategory"/> 獲得 <see cref="POICategory"/> 的最大可能數量。）
        /// </summary>
        /// <param name="buildingCategory">The input building category.（輸入的建築分類。）</param>
        /// <returns>The maximum possible number of categories.（最大可能的分類數量。）</returns>
        private static int GetMaximumCategoryCount(BuildingCategory buildingCategory) 
        {
            int maxCount = 0;

            if ((buildingCategory & BuildingCategory.Extractor) != 0)
            {
                maxCount += 11;
            }
            if ((buildingCategory & BuildingCategory.Property) != 0)
            {
                maxCount += 25;
            }
            if ((buildingCategory & BuildingCategory.Admin) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Communication) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Decoration) != 0)
            {
                maxCount += 2;
            }
            if ((buildingCategory & BuildingCategory.Disaster) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Education) != 0)
            {
                maxCount += 5;
            }
            if ((buildingCategory & BuildingCategory.Fire) != 0)
            {
                maxCount += 2;
            }
            if ((buildingCategory & BuildingCategory.Health) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Maintenance) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Mortuary) != 0)
            {
                maxCount += 3;
            }
            if ((buildingCategory & BuildingCategory.Park) != 0)
            {
                maxCount += 2;
            }
            if ((buildingCategory & BuildingCategory.Parking) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Police) != 0)
            {
                maxCount += 2;
            }
            if ((buildingCategory & BuildingCategory.Post) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Power) != 0)
            {
                maxCount += 6;
            }
            if ((buildingCategory & BuildingCategory.Research) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Sewage) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Transportation) != 0)
            {
                maxCount += 21;
            }
            if ((buildingCategory & BuildingCategory.Waste) != 0)
            {
                maxCount += 1;
            }
            if ((buildingCategory & BuildingCategory.Water) != 0)
            {
                maxCount += 1;
            }

            return maxCount;
        }

        /// <summary>
        /// Retrieve the POI category of the entity.
        /// （獲得實體的興趣點分類。）
        /// </summary>
        /// <param name="entity">The target entity.（目標實體。）</param>
        /// <param name="prefab">The target entity's prefab.（目標實體的預製模板。）</param>
        /// <param name="buildingCategory">The building category of the target entity.（目標實體的建築分類。）</param>
        /// <param name="product">The resource sold by the target entity.（目標實體販售的資源。）</param>
        /// <param name="brand">The brand index.（品牌的索引值。）</param>
        /// <param name="zoningTypeIndex">The index of the zone type that the entity belongs to.（實體所屬的分區類別的索引值。）</param>
        /// <param name="hasPrefabRef">Whether the target entity has <see cref="Game.Prefabs.PrefabRef"/> component.（目標實體是否有 <see cref="Game.Prefabs.PrefabRef"/> 組件？）</param>
        /// <param name="isSubBuilding">Whether the target entity is a sub building/service upgrade or not.（目標實體是否為一個子建築／服務升級？）</param>
        public static void GetPOICategoryFromBuilding(Entity entity, Entity prefab, BuildingCategory buildingCategory, Resource product, int brand, int zoningTypeIndex, bool hasPrefabRef, bool isSubBuilding,
                                                      ref BufferLookup<SubObject> subObjectBufferLookup,
                                                      ref ComponentLookup<Battery> batteryLookup, ref ComponentLookup<Game.Prefabs.DeathcareFacilityData> deathcareFacilityDataLookup,
                                                      ref ComponentLookup<ElectricityProducer> electricityProducerLookup, ref ComponentLookup<FirewatchTower> firewatchTowerLookup,
                                                      ref ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup, ref ComponentLookup<Prison> prisonLookup, ref ComponentLookup<Game.Prefabs.ServiceData> serviceDataLookup,
                                                      ref ComponentLookup<Game.Prefabs.SchoolData> schoolDataLookup, ref ComponentLookup<StorageProperty> storagePropertyLookup,
                                                      ref ComponentLookup<Transformer> transformerLookup, ref ComponentLookup<TransportDepot> transportDepotLookup,
                                                      ref ComponentLookup<Game.Prefabs.TransportDepotData> transportDepotDataLookup, ref ComponentLookup<TransportStation> transportStationLookup,
                                                      ref ComponentLookup<Game.Prefabs.TransportStopData> transportStopDataLookup, ref ComponentLookup<Game.Prefabs.WaterPoweredData> waterPoweredDataLookup,
                                                      ref ComponentLookup<Game.Prefabs.WindPoweredData> windPoweredDataLookup, ref NativeList<ZoningType> zoningTypes,
                                                      ref NativeParallelHashSet<Entity> attractions, ref NativeParallelHashSet<EnumWrapper<POICategory>> categories, out Feature objectType)
        {
            objectType = Feature.None;
            bool poiPrivate = false;
            bool poiPublic = false;
            bool poiTransport = false;
            bool poiUtility = false;

            if (!categories.IsCreated) return;
            
            if ((buildingCategory & BuildingCategory.Property) != 0)
            {
                if ((zoningTypeIndex >= 0) && (zoningTypeIndex < zoningTypes.Length) && (brand >= 0))
                {
                    ZoningType zoningType = zoningTypes[zoningTypeIndex];
                    bool isCommercial = (zoningType.category & ZoningCategory.Commercial) != 0;
                    bool isIndustrial = (zoningType.category & ZoningCategory.Industrial) != 0;
                    bool isOffice = (zoningType.category & ZoningCategory.Office) != 0;

                    if ((buildingCategory & BuildingCategory.Extractor) != 0)
                    {
                        poiPrivate = true;
                        bool hasAnyKnownProduct = false;
                        if ((product & Resource.Coal) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialCoal));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Cotton) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialCotton));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Fish) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialFish));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Grain) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialGrain));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Livestock) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialLivestock));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Oil) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialOil));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Ore) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialOre));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Stone) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialStone));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Vegetables) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialVegetables));
                            hasAnyKnownProduct = true;
                        }
                        if ((product & Resource.Wood) != 0)
                        {
                            categories.Add(new(POICategory.IndustrialWood));
                            hasAnyKnownProduct = true;
                        }
                        if (!hasAnyKnownProduct)
                        {
                            categories.Add(new(POICategory.IndustrialGeneric));
                        }
                    }
                    else
                    {
                        if (isCommercial || isOffice)
                        {
                            poiPrivate = true;
                            bool hasAnyKnownProduct = false;
                            if ((product & Resource.Beverages) != 0)
                            {
                                categories.Add(new(POICategory.StoreBeverage));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Chemicals) != 0)
                            {
                                categories.Add(new(POICategory.StoreChemicals));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.ConvenienceFood) != 0)
                            {
                                categories.Add(new(POICategory.StoreConvenienceStore));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Electronics) != 0)
                            {
                                categories.Add(new(POICategory.StoreElectronics));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Entertainment) != 0)
                            {
                                categories.Add(new(POICategory.StoreBar));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Financial) != 0)
                            {
                                categories.Add(new(POICategory.StoreBank));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Food) != 0)
                            {
                                categories.Add(new(POICategory.StoreFood));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Furniture) != 0)
                            {
                                categories.Add(new(POICategory.StoreFurniture));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Lodging) != 0)
                            {
                                categories.Add(new(POICategory.StoreHotel));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Meals) != 0)
                            {
                                categories.Add(new(POICategory.StoreRestaurant));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Media) != 0)
                            {
                                categories.Add(new(POICategory.StoreMedia));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Paper) != 0)
                            {
                                categories.Add(new(POICategory.StoreBookStore));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Petrochemicals) != 0)
                            {
                                categories.Add(new(POICategory.StoreGasStation));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Pharmaceuticals) != 0)
                            {
                                categories.Add(new(POICategory.StoreDrugStore));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Plastics) != 0)
                            {
                                categories.Add(new(POICategory.StorePlastics));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Recreation) != 0)
                            {
                                categories.Add(new(POICategory.StoreRecreation));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Software) != 0)
                            {
                                categories.Add(new(POICategory.StoreSoftware));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Telecom) != 0)
                            {
                                categories.Add(new(POICategory.StoreTelecom));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Textiles) != 0)
                            {
                                categories.Add(new(POICategory.StoreFashionStore));
                                hasAnyKnownProduct = true;
                            }
                            if ((product & Resource.Vehicles) != 0)
                            {
                                categories.Add(new(POICategory.StoreCarStore));
                                hasAnyKnownProduct = true;
                            }
                            if (!hasAnyKnownProduct)
                            {
                                categories.Add(new(isCommercial ? POICategory.StoreGeneric : POICategory.StoreOffice));
                            }
                        }
                        if (isIndustrial)
                        {
                            poiPrivate = true;
                            categories.Add(storagePropertyLookup.HasComponent(entity) ? POICategory.IndustrialWarehouse : POICategory.IndustrialFactory);
                        }
                    }
                }
            }
            if ((buildingCategory & BuildingCategory.Admin) != 0)
            {
                poiPublic = true;
                categories.Add(new(POICategory.Admin));
            }
            if ((buildingCategory & BuildingCategory.Communication) != 0)
            {
                poiUtility = true;
                categories.Add(new(POICategory.Communication));
            }
            if ((buildingCategory & BuildingCategory.Disaster) != 0)
            {
                poiPublic = true;
                categories.Add(new(POICategory.Disaster));
            }
            if (((buildingCategory & BuildingCategory.Education) != 0) && hasPrefabRef)
            {
                poiPublic = true;
                if (schoolDataLookup.TryGetComponent(prefab, out Game.Prefabs.SchoolData school))
                {
                    switch (school.m_EducationLevel)
                    {
                        case 1:
                            categories.Add(new(POICategory.EducationElementary));
                            break;

                        case 2:
                            categories.Add(new(POICategory.EducationHigh));
                            break;

                        case 3:
                            categories.Add(new(POICategory.EducationCollege));
                            break;

                        case 4:
                            categories.Add(new(POICategory.EducationUniversity));
                            break;

                        default:
                            categories.Add(new(POICategory.EducationGeneric));
                            break;
                    }
                }
                else
                {
                    categories.Add(new(POICategory.EducationGeneric));
                }
            }
            if ((buildingCategory & BuildingCategory.Fire) != 0)
            {
                poiPublic = true;
                categories.Add(new(firewatchTowerLookup.HasComponent(entity) ? POICategory.FireWatchTower : POICategory.Fire));
            }
            if ((buildingCategory & BuildingCategory.Health) != 0)
            {
                poiPublic = true;
                categories.Add(new(POICategory.Health));
            }
            if ((buildingCategory & BuildingCategory.Maintenance) != 0)
            {
                poiPublic = true;
                categories.Add(new(POICategory.Maintenance));
            }
            if (((buildingCategory & BuildingCategory.Mortuary) != 0) && hasPrefabRef)
            {
                poiPublic = true;
                if (deathcareFacilityDataLookup.TryGetComponent(prefab, out Game.Prefabs.DeathcareFacilityData deathcareFacility))
                {
                    categories.Add(new(deathcareFacility.m_LongTermStorage ? POICategory.MortuaryCemetery : POICategory.MortuaryCrematorium));
                }
                else
                {
                    categories.Add(new(POICategory.MortuaryGeneric));
                }
            }
            if ((buildingCategory & BuildingCategory.Park) != 0)
            {
                poiPublic = true;
                if (hasPrefabRef)
                {
                    categories.Add(new(attractions.Contains(prefab) ? POICategory.Attraction : POICategory.Park));
                }
                else
                {
                    categories.Add(new(POICategory.Park));
                }
            }
            if ((buildingCategory & BuildingCategory.Parking) != 0)
            {
                poiPublic = true;
                categories.Add(new(POICategory.Parking));
            }
            if ((buildingCategory & BuildingCategory.Police) != 0)
            {
                poiPublic = true;
                categories.Add(new(prisonLookup.HasComponent(entity) ? POICategory.Prison : POICategory.Police));
            }
            if ((buildingCategory & BuildingCategory.Post) != 0)
            {
                poiPublic = true;
                categories.Add(new(POICategory.Post));
            }
            if ((buildingCategory & BuildingCategory.Power) != 0)
            {
                poiUtility = true;
                bool hasAnyPowerSubCategory = false;
                if (batteryLookup.HasComponent(entity))
                {
                    categories.Add(new(POICategory.PowerBattery));
                    hasAnyPowerSubCategory = true;
                }
                if (electricityProducerLookup.HasComponent(entity) && hasPrefabRef)
                {
                    bool waterPowered = waterPoweredDataLookup.HasComponent(prefab);
                    bool windPowered = windPoweredDataLookup.HasComponent(prefab);

                    if (waterPowered) categories.Add(new(POICategory.PowerDam));
                    if (windPowered) categories.Add(new(POICategory.PowerTurbine));
                    if (!waterPowered && !windPowered) categories.Add(new(POICategory.PowerPlant));

                    hasAnyPowerSubCategory = true;
                }
                if (transformerLookup.HasComponent(entity))
                {
                    categories.Add(new(POICategory.PowerSubstation));
                    hasAnyPowerSubCategory = true;
                }
                if (!hasAnyPowerSubCategory && hasPrefabRef)
                {
                    if (serviceDataLookup.TryGetComponent(prefab, out Game.Prefabs.ServiceData service))
                    {
                        if (service.m_Service == CityService.Electricity) categories.Add(new(POICategory.PowerGeneric));
                    }
                    else
                    {
                        categories.Add(new(POICategory.PowerGeneric));
                    }
                }
            }
            if ((buildingCategory & BuildingCategory.Research) != 0)
            {
                poiPublic = true;
                categories.Add(new(POICategory.Research));
            }
            if ((buildingCategory & BuildingCategory.Sewage) != 0)
            {
                poiUtility = true;
                categories.Add(new(POICategory.Sewage));
            }
            if (((buildingCategory & BuildingCategory.Transportation) != 0) || isSubBuilding)
            {
                bool hasAnyTransportationSubCategory = false;
                if (transportDepotLookup.HasComponent(entity) && hasPrefabRef)
                {
                    if (transportDepotDataLookup.TryGetComponent(prefab, out Game.Prefabs.TransportDepotData transportDepot))
                    {
                        switch (transportDepot.m_TransportType)
                        {
                            case Game.Prefabs.TransportType.Bus:
                                categories.Add(new(POICategory.DepotBus));
                                break;

                            case Game.Prefabs.TransportType.Ferry:
                                categories.Add(new(POICategory.DepotFerry));
                                break;

                            case Game.Prefabs.TransportType.Rocket:
                                categories.Add(new(POICategory.SpaceCenter));
                                break;

                            case Game.Prefabs.TransportType.Subway:
                                categories.Add(new(POICategory.DepotSubway));
                                break;

                            case Game.Prefabs.TransportType.Taxi:
                                categories.Add(new(POICategory.DepotTaxi));
                                break;

                            case Game.Prefabs.TransportType.Train:
                                categories.Add(new(POICategory.DepotTrain));
                                break;

                            case Game.Prefabs.TransportType.Tram:
                                categories.Add(new(POICategory.DepotTram));
                                break;

                            default:
                                categories.Add(new(POICategory.DepotGeneric));
                                break;
                        }
                    }
                    else
                    {
                        categories.Add(new(POICategory.DepotGeneric));
                    }

                    poiTransport = true;
                    hasAnyTransportationSubCategory = true;
                }
                if ((transportStationLookup.HasComponent(entity) || isSubBuilding) && subObjectBufferLookup.TryGetBuffer(entity, out DynamicBuffer<SubObject> subObjects))
                {
                    for (int i = 0; i < subObjects.Length; i++)
                    {
                        if (prefabRefLookup.TryGetComponent(subObjects[i].m_SubObject, out Game.Prefabs.PrefabRef subObjectPrefab) &&
                            transportStopDataLookup.TryGetComponent(subObjectPrefab, out Game.Prefabs.TransportStopData transportStop))
                        {
                            poiTransport = true;
                            Game.Prefabs.TransportType transportType = transportStop.m_TransportType;
                            bool isCargo = transportStop.m_CargoTransport;
                            bool isPassenger = transportStop.m_PassengerTransport;

                            switch (transportType)
                            {
                                case Game.Prefabs.TransportType.Airplane:
                                    if (isCargo) categories.Add(new(POICategory.BuildingCargoAirplane));
                                    if (isPassenger) categories.Add(new(POICategory.BuildingPassengerAirplane));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Bus:
                                    categories.Add(new(POICategory.BuildingBus));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Ferry:
                                    categories.Add(new(POICategory.BuildingFerry));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Helicopter:
                                    categories.Add(new(POICategory.BuildingHelicopter));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Rocket:
                                    categories.Add(new(POICategory.SpaceCenter));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Ship:
                                    if (isCargo) categories.Add(new(POICategory.BuildingCargoShip));
                                    if (isPassenger) categories.Add(new(POICategory.BuildingPassengerShip));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Subway:
                                    categories.Add(new(POICategory.BuildingSubway));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Taxi:
                                    categories.Add(new(POICategory.BuildingTaxi));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Train:
                                    if (isCargo) categories.Add(new(POICategory.BuildingCargoTrain));
                                    if (isPassenger) categories.Add(new(POICategory.BuildingPassengerTrain));
                                    hasAnyTransportationSubCategory = true;
                                    break;

                                case Game.Prefabs.TransportType.Tram:
                                    categories.Add(new(POICategory.BuildingTram));
                                    hasAnyTransportationSubCategory = true;
                                    break;
                            }
                        }
                    }
                }
                if (!hasAnyTransportationSubCategory && hasPrefabRef && !isSubBuilding)
                {
                    poiTransport = true;
                    if (serviceDataLookup.TryGetComponent(prefab, out Game.Prefabs.ServiceData service))
                    {
                        if (service.m_Service == CityService.Transportation) categories.Add(new(POICategory.TransportationGeneric));
                    }
                    else
                    {
                        categories.Add(new(POICategory.TransportationGeneric));
                    }
                }
            }
            if ((buildingCategory & BuildingCategory.Waste) != 0)
            {
                poiUtility = true;
                categories.Add(new(POICategory.Waste));
            }
            if ((buildingCategory & BuildingCategory.Water) != 0)
            {
                poiUtility = true;
                categories.Add(new(POICategory.Water));
            }

            if (poiPrivate) objectType |= Feature.POIPrivate;
            if (poiPublic) objectType |= Feature.POIPublic;
            if (poiTransport) objectType |= Feature.POITransport;
            if (poiUtility) objectType |= Feature.POIUtility;
        }

        /// <summary>
        /// Retrieve the name of the POI.
        /// （獲得興趣點的名稱。）
        /// </summary>
        /// <param name="poi">The input POI.（輸入的興趣點。）</param>
        /// <param name="brands">The list of brands.（品牌的列表。）</param>
        /// <param name="categoryEntityMap">The mpa between entities and their POI categories.（實體與其興趣點分類的映射表。）</param>
        /// <returns>The name of the POI.（興趣點的名稱。）</returns>
        private static string GetPOIName(POI poi, List<Brand> brands,
                                         ref NativeParallelHashMap<Entity, NativeParallelHashSet<EnumWrapper<POICategory>>> categoryEntityMap)
        {
            Entity entity = poi.entity;
            
            if (poi.isPrivate && (poi.brand >= 0) && (poi.brand < brands.Count))
            {
                return brands[poi.brand].name;
            }
            else if (categoryEntityMap.TryGetValue(entity, out NativeParallelHashSet<EnumWrapper<POICategory>> categories) && categories.IsCreated)
            {
                int entityIndex = entity.Index;

                if (categories.Contains(POICategory.Helipad))
                {
                    return $"Helipad {entityIndex}";
                }
                else if (categories.Contains(POICategory.LevelCrossing))
                {
                    return $"Level Crossing {entityIndex}";
                }
                else if (categories.Contains(POICategory.TrafficLight))
                {
                    return $"Traffic Light {entityIndex}";
                }
                else if (categories.Contains(POICategory.UtilityPylon))
                {
                    return $"Utility Pylon {entityIndex}";
                }
                else if (categories.Contains(POICategory.UtilityPole))
                {
                    return $"Utility Pole {entityIndex}";
                }
                else if (HasTransportStopPOI(ref categories) && !poi.hasCustomName)
                {
                    if (poi.hasOwner)
                    {
                        return _name.GetLabelName(poi.owner);
                    }
                    else
                    {
                        string streetName = _name.GetLabelName(poi.address.street);
                        string stopName = LocaleUtils.TryTranslate($"Assets.ADDRESS_NAME_FORMAT", out string translated) ? translated : _name.GetLabelName(entity);
                        return stopName.Replace("{ROAD}", streetName).Replace("{NUMBER}", poi.address.number.ToString("G"));
                    }
                }
                else
                {
                    return _name.GetLabelName(entity);
                }
            }
            else
            {
                return _name.GetLabelName(entity);
            }
        }

        /// <summary>
        /// Check whether the input POI includes transport stop POI categories.
        /// （確認輸入的興趣點是否包含運輸場站興趣點分類。）
        /// </summary>
        /// <param name="categories">The input hashset.（輸入的集合。）</param>
        /// <returns>If true, the input set has transport stop POI categories.（若為真，則該集合包含運輸場站興趣點分類。）</returns>
        private static bool HasTransportStopPOI(ref NativeParallelHashSet<EnumWrapper<POICategory>> categories)
        {
            return categories.Contains(POICategory.StopBus) ||
                   categories.Contains(POICategory.StopCargoAirplane) ||
                   categories.Contains(POICategory.StopCargoShip) ||
                   categories.Contains(POICategory.StopCargoTrain) ||
                   categories.Contains(POICategory.StopFerry) ||
                   categories.Contains(POICategory.StopHelicopter) ||
                   categories.Contains(POICategory.StopPassengerAirplane) ||
                   categories.Contains(POICategory.StopPassengerShip) ||
                   categories.Contains(POICategory.StopPassengerTrain) ||
                   categories.Contains(POICategory.StopSubway) ||
                   categories.Contains(POICategory.StopTaxi) ||
                   categories.Contains(POICategory.StopTram);
        }

        /// <summary>
        /// Check whether the input POI's type is allowed to be exported.
        /// （確認輸入興趣點的種類是否允許被輸出。）
        /// </summary>
        /// <param name="poiObjectType">The input POI's type.（輸入的興趣點類別。）</param>
        /// <param name="allowedType">The allowed POI type.（允許的 POI 種類。）</param>
        /// <returns>Is the POI allowed to be exported?（POI 允許被輸出嗎？）</returns>
        private bool IsAllowedPOIType(Feature poiObjectType, Feature allowedType) => (poiObjectType & (allowedType & POIObjects)) != 0;

        /// <summary>
        /// Map the districts to the input POI list.
        /// （將行政區映射至輸入的興趣點列表。）
        /// </summary>
        /// <param name="poiList">The list of POIs.（興趣點列表。）</param>
        /// <param name="districtQuery">The query to find the districts.（搜尋行政區的查詢。）</param>
        private void MapDistrictsToPOIs(ref NativeList<POI> poiList, ref EntityQuery districtQuery)
        {
            int poiCount = poiList.Length;
            int trianglesCount = _area.GetTrianglesCount(ref districtQuery);
            NativeArray<float3> positions = new(poiCount, Allocator.Persistent);
            NativeList<BVHUtils.Triangle> triangles = new(trianglesCount, Allocator.Persistent);
            NativeParallelMultiHashMap<Entity, int> areaEntityMap = new(poiCount, Allocator.Persistent);

            AreaSystem.RetrieveTrianglesJob retrieveTrianglesJob = new()
            {
                triangleList = triangles
            };
            JobHandle retrieveTrianglesHandle = retrieveTrianglesJob.Schedule(districtQuery, default);
            retrieveTrianglesHandle.Complete();

            ExtractPOIPositionsJob extractPositionsJob = new()
            {
                list = poiList,
                positions = positions
            };
            JobHandle extractPositionsHandle = extractPositionsJob.Schedule(poiCount, 32, default);
            extractPositionsHandle.Complete();

            BVHUtils.GetIntersectMap(ref triangles, ref positions, ref areaEntityMap);

            MapDistrictsToPOIsJob mapDistrictsJob = new()
            {
                areaEntityMap = areaEntityMap,
                POIs = poiList
            };
            JobHandle mapDistrictsHandle = mapDistrictsJob.Schedule(districtQuery, default);
            mapDistrictsHandle.Complete();

            CommonUtils.Dispose(ref areaEntityMap);
            CommonUtils.Dispose(ref positions);
            CommonUtils.Dispose(ref triangles);
        }

        /// <summary>
        /// Write location attributes to the designated file.
        /// （寫出位置屬性至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        /// <param name="fieldMap">The map between the property and the fields.（屬性與欄位的映射表。）</param>
        public void WriteLocationDBF(BinaryWriter writer, Options options, HashSet<Property> validatedFields, List<Entity> entitySyncList, out Dictionary<Property, FieldInfo> fieldMap)
        {
            bool hasName = options.Contains(Property.Name, IO.System.POI) && validatedFields.Contains(Property.Name);
            bool hasAddress = options.Contains(Property.Address, IO.System.POI) && validatedFields.Contains(Property.Address);
            bool hasCategory = options.Contains(Property.Category, IO.System.POI) && validatedFields.Contains(Property.Category);
            bool hasObject = options.Contains(Property.Object, IO.System.POI) && validatedFields.Contains(Property.Object);

            // Validate native containers integrity.（驗證原生容器的完整性。）
            CommonUtils.ValidateIntegrity(ref _categoryEntityMap, true);
            CommonUtils.ValidateIntegrity(ref _localPOIs, true);

            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, int> syncMap = new(_localPOIs.Length, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<Brand> brands = _shared.Brands;
            List<LiteralAddress> POIAddresses = new();
            List<string> POINames = new();
            Dictionary<Property, FieldInfo> _fieldMap = new();

            try
            {
                FieldInfo addressField = new(0, 0, false, FieldType.String);
                FieldInfo categoryField = new(0, 0, false, FieldType.String);
                FieldInfo nameField = new(0, 0, false, FieldType.String);
                FieldInfo objectField = new(0, 0, false, FieldType.String);

                for (int i = 0; i < _localPOIs.Length; i++)
                {
                    POI poi = _localPOIs[i];
                    Entity entity = poi.entity;
                    Feature originalObject = poi.objectType;
                    if ((poi.location.x == double.MaxValue) || !IsAllowedPOIType(originalObject, options.Features)) continue;

                    if (hasName)
                    {
                        string poiName = GetPOIName(poi, brands, ref _categoryEntityMap);
                        POINames.Add(poiName);
                        nameField += new FieldInfo(poiName);
                    }
                    if (hasAddress)
                    {
                        LiteralAddress address = poi.address.ToLiteral(_name);
                        POIAddresses.Add(address);
                        addressField += new FieldInfo(address.district);
                        addressField += new FieldInfo(address.street);
                    }
                    if (hasCategory)
                    {
                        string category = string.Empty;
                        if (_categoryEntityMap.TryGetValue(entity, out NativeParallelHashSet<EnumWrapper<POICategory>> categories))
                        {
                            POICategory[] categoriesArray = CommonUtils.Copy(ref categories);

                            if (options.Display[(Property.Category, IO.System.POI)])
                            {
                                category = GetCategoryString(categoriesArray);
                            }
                            else
                            {
                                category = CommonUtils.GetFirstMatch(categoriesArray, IO.IO.POICategoryDisplayOrder).ToString("G");
                            }
                        }
                        else
                        {
                            category = POICategory.None.ToString("G");
                        }

                        categoryField += new FieldInfo(category);
                    }
                    if (hasObject)
                    {
                        Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? originalObject : CommonUtils.GetFirstMatch(originalObject, IO.IO.FeatureDisplayOrder);
                        objectField += new FieldInfo(displayType.ToString("G"));
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
                if (hasCategory)
                {
                    _fieldMap.Add(Property.Category, categoryField);
                }
                if (hasObject)
                {
                    _fieldMap.Add(Property.Object, objectField);
                }

                // Sync the entity order with that of the .shp file.（與 .shp 檔案的實體順序同步。）
                Shapefile.SyncStatsToIndex(entitySyncList, ref _localPOIs, ref syncMap);

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    for (int index = 0; index < entitySyncList.Count; index++)
                    {
                        if (!syncMap.TryGetValue(entitySyncList[index], out int i))
                        {
                            _log.Error($"Couldn't find the statistical object of {entitySyncList[index]} at index {index}. 無法找到位於索引值 {index} 的實體 {entitySyncList[index]} 之統計物件。");
                        }

                        POI poi = _localPOIs[i];
                        Entity entity = poi.entity;
                        Feature originalObject = poi.objectType;
                        writer.Write((byte)32);

                        if (hasName)
                        {
                            // Use `index` as the index, since the length and the order of `POINames` is the same as `entitySyncList`.
                            //（使用 `index` 作為索引，因為 `POINames` 的長度與順序與 `entitySyncList` 順序相同。）
                            Shapefile.WriteRecord(writer, nameField, POINames[index]);
                        }
                        if (hasAddress)
                        {
                            // Use `index` as the index, since the length and the order of `POINames` is the same as `entitySyncList`.
                            //（使用 `index` 作為索引，因為 `POINames` 的長度與順序與 `entitySyncList` 順序相同。）
                            LiteralAddress address = POIAddresses[index];
                            Shapefile.WriteRecord(writer, addressField, address.district);
                            Shapefile.WriteRecord(writer, addressField, address.street);
                            Shapefile.WriteRecord(writer, new(100000), address.number);
                        }
                        if (hasCategory)
                        {
                            string category = string.Empty;
                            if (_categoryEntityMap.TryGetValue(entity, out NativeParallelHashSet<EnumWrapper<POICategory>> categories))
                            {
                                POICategory[] categoriesArray = CommonUtils.Copy(ref categories);

                                if (options.Display[(Property.Category, IO.System.POI)])
                                {
                                    category = GetCategoryString(categoriesArray);
                                }
                                else
                                {
                                    category = CommonUtils.GetFirstMatch(categoriesArray, IO.IO.POICategoryDisplayOrder).ToString("G");
                                }
                            }
                            else
                            {
                                category = POICategory.None.ToString("G");
                            }
                            Shapefile.WriteRecord(writer, categoryField, category);
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? originalObject : CommonUtils.GetFirstMatch(originalObject, IO.IO.FeatureDisplayOrder);
                            Shapefile.WriteRecord(writer, objectField, displayType.ToString("G"));
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                CommonUtils.Dispose(ref syncMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                fieldMap = _fieldMap;
                CommonUtils.Dispose(ref syncMap);
                Dispose();
            }
        }

        /// <summary>
        /// Write location features (geometries and properties) to the designated file.
        /// （寫出位置圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public void WriteLocationFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            Feature featureFlag = options.Features;
            bool usePrivate = featureFlag.HasFlag(Feature.POIPrivate);
            bool usePublic = featureFlag.HasFlag(Feature.POIPublic);
            bool useTransport = featureFlag.HasFlag(Feature.POITransport);
            bool useUtility = featureFlag.HasFlag(Feature.POIUtility);

            bool hasName = options.Contains(Property.Name, IO.System.POI);
            bool hasAddress = options.Contains(Property.Address, IO.System.POI);
            bool hasCategory = options.Contains(Property.Category, IO.System.POI);
            bool hasObject = options.Contains(Property.Object, IO.System.POI);

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<BuildingStat> buildingStats = ref _shared.BuildingStats;
            ref NativeList<ZoningType> zoningTypes = ref _shared.ZoningTypes;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            CommonUtils.ValidateIntegrity(ref buildingStats, true);
            CommonUtils.ValidateIntegrity(ref zoningTypes);

            // Initialize native containers.（初始化原生容器。）
            int buildingStatsCount = buildingStats.Length;
            int helipadsCount = _helipadQuery.CalculateEntityCount();
            int pylonsCount = _pylonQuery.CalculateEntityCount();
            int trafficLightsCount = _trafficLightQuery.CalculateEntityCount();
            int transportStopMarkersCount = _transportStopMarkerQuery.CalculateEntityCount();
            int maxPOICount = buildingStatsCount;
            if (useTransport) maxPOICount += helipadsCount + trafficLightsCount + transportStopMarkersCount;
            if (useUtility) maxPOICount += pylonsCount;
            CommonUtils.Reset(ref _categoryEntityMap, maxPOICount, Allocator.Persistent);
            CommonUtils.Reset(ref _localPOIs, maxPOICount, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<Brand> brands = _shared.Brands;
            List<LiteralAddress> POIAddresses = new();
            List<string> POINames = new();

            try
            {
                // Collect POIs from the building statistics.（由建築統計資料收集興趣點。）
                CollectGeneralPOIs(options, ref buildingStats, ref zoningTypes);

                if (useTransport)
                {
                    CollectTransportPOIs(options);
                }

                if (useUtility)
                {
                    CollectUtilityPOIs(options);
                }

                // Map the district to POIs.（將行政區映射至興趣點。）
                MapDistrictsToPOIs(ref _localPOIs, ref _districtQuery);

                // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
                if (hasName || hasAddress)
                {
                    for (int i = 0; i < _localPOIs.Length; i++)
                    {
                        POI poi = _localPOIs[i];
                        Entity entity = poi.entity;

                        if (hasName)
                        {
                            POINames.Add(GetPOIName(poi, brands, ref _categoryEntityMap));
                        }

                        if (hasAddress)
                        {
                            POIAddresses.Add(poi.address.ToLiteral(_name));
                        }
                    }
                }

                Task writerThread = Task.Run(() =>
                {
                    for (int i = 0; i < _localPOIs.Length; i++)
                    {
                        POI poi = _localPOIs[i];
                        Entity entity = poi.entity;
                        Feature originalObject = poi.objectType;
                        if ((poi.location.x == double.MaxValue) || !IsAllowedPOIType(originalObject, options.Features)) continue;

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");

                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        GeoJson.WriteGeometry(writer, new Geodata.Geometry(poi.location), Shape.Point, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();

                        if (hasName)
                        {
                            GeoJson.WriteProperty(writer, Property.Name, POINames[i]);
                        }
                        if (hasAddress)
                        {
                            GeoJson.WriteProperty(writer, Property.Address, POIAddresses[i].ToArray());
                        }
                        if (hasCategory)
                        {
                            if (_categoryEntityMap.TryGetValue(entity, out NativeParallelHashSet<EnumWrapper<POICategory>> categories))
                            {
                                POICategory[] categoriesArray = CommonUtils.Copy(ref categories);

                                if (options.Display[(Property.Category, IO.System.POI)])
                                {
                                    GeoJson.WriteProperty(writer, Property.Category, GetCategoryString(categoriesArray));
                                }
                                else
                                {
                                    GeoJson.WriteProperty(writer, Property.Category, CommonUtils.GetFirstMatch(categoriesArray, IO.IO.POICategoryDisplayOrder).ToString("G"));
                                }
                            }
                            else
                            {
                                GeoJson.WriteProperty(writer, Property.Category, POICategory.None.ToString("G"));
                            }
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? originalObject : CommonUtils.GetFirstMatch(originalObject, IO.IO.FeatureDisplayOrder);
                            GeoJson.WriteProperty(writer, Property.Object, displayType.ToString("G"));
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
                IO.IO.DisposeAll();
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Write location geometries to the designated file.
        /// （寫出位置幾何至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        public void WriteLocationSHP(BinaryWriter writer, Options options, out List<Shapefile.IndexPair> indexPairs, out Bounds3 bounds, out List<Entity> entitySyncList)
        {
            Feature featureFlag = options.Features;
            bool usePrivate = featureFlag.HasFlag(Feature.POIPrivate);
            bool usePublic = featureFlag.HasFlag(Feature.POIPublic);
            bool useTransport = featureFlag.HasFlag(Feature.POITransport);
            bool useUtility = featureFlag.HasFlag(Feature.POIUtility);

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<BuildingStat> buildingStats = ref _shared.BuildingStats;
            ref NativeList<ZoningType> zoningTypes = ref _shared.ZoningTypes;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            CommonUtils.ValidateIntegrity(ref buildingStats, true);
            CommonUtils.ValidateIntegrity(ref zoningTypes);

            // Initialize native containers.（初始化原生容器。）
            int buildingStatsCount = buildingStats.Length;
            int helipadsCount = _helipadQuery.CalculateEntityCount();
            int pylonsCount = _pylonQuery.CalculateEntityCount();
            int trafficLightsCount = _trafficLightQuery.CalculateEntityCount();
            int transportStopMarkersCount = _transportStopMarkerQuery.CalculateEntityCount();
            int maxPOICount = buildingStatsCount;
            if (useTransport) maxPOICount += helipadsCount + trafficLightsCount + transportStopMarkersCount;
            if (useUtility) maxPOICount += pylonsCount;
            CommonUtils.Reset(ref _categoryEntityMap, maxPOICount, Allocator.Persistent);
            CommonUtils.Reset(ref _localPOIs, maxPOICount, Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<Entity> _entitySyncList = new();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                // Collect POIs from the building statistics.（由建築統計資料收集興趣點。）
                CollectGeneralPOIs(options, ref buildingStats, ref zoningTypes);

                if (useTransport)
                {
                    CollectTransportPOIs(options);
                }

                if (useUtility)
                {
                    CollectUtilityPOIs(options);
                }

                // Map the district to POIs.（將行政區映射至興趣點。）
                MapDistrictsToPOIs(ref _localPOIs, ref _districtQuery);

                Task writerThread = Task.Run(() =>
                {
                    int enumeratorIndex = 0;
                    int shapeId = Shapefile.GetShapeType(VectorKind.Location, options.Elevation);

                    if (BitConverter.IsLittleEndian)
                    {
                        for (int i = 0; i < _localPOIs.Length; i++)
                        {
                            POI poi = _localPOIs[i];
                            Feature originalObject = poi.objectType;
                            if ((poi.location.x == double.MaxValue) || !IsAllowedPOIType(originalObject, options.Features)) continue;

                            enumeratorIndex++;
                            Shapefile.WriteGeometryLE(writer, enumeratorIndex, shapeId, new(poi.location), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(poi.entity);
                            _indexPairs.Add(indexPair);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _localPOIs.Length; i++)
                        {
                            POI poi = _localPOIs[i];
                            Feature originalObject = poi.objectType;
                            if ((poi.location.x == double.MaxValue) || !IsAllowedPOIType(originalObject, options.Features)) continue;

                            enumeratorIndex++;
                            Shapefile.WriteGeometryBE(writer, enumeratorIndex, shapeId, new(poi.location), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(poi.entity);
                            _indexPairs.Add(indexPair);
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                IO.IO.DisposeAll();
            }
            finally
            {
                // Don't dispose `_localPOIs` and `_categoryEntityMap`, both of them are required in `WriteLocationDBF()`.
                // （不要拋棄 `_localPOIs` 及 `_categoryEntityMap` ，它們仍會被 `WriteLocationDBF()` 呼叫。）
                bounds = _bounds;
                entitySyncList = _entitySyncList;
                indexPairs = _indexPairs;
            }
        }

        /// <summary>
        /// The job to collect helipad POIs.
        /// （收集直升機停機坪興趣點的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectHelipadsJob : IJobEntity
        {
            [ReadOnly]
            public bool useAddress;
            
            [ReadOnly]
            public BufferLookup<AggregateElement> aggregateElementBufferLookup;

            [ReadOnly]
            public ComponentLookup<Aggregated> aggregatedLookup;

            [ReadOnly]
            public ComponentLookup<Building> buildingLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.BuildingData> buildingDataLookup;

            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<Edge> edgeLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<Game.Common.Owner> ownerLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.Roundabout> roundaboutLookup;

            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeList<POI>.ParallelWriter POIs;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeParallelHashSet<EnumWrapper<POICategory>>>.ParallelWriter POICategories;

            public void Execute(in Game.Objects.Transform transform, Entity helipad)
            {
                POI poi = new()
                {
                    entity = helipad,
                    address = default,
                    brand = -1,
                    hasOwner = false,
                    inGamePosition = transform.m_Position,
                    isAddressVerified = false,
                    isPrivate = false,
                    location = Geodata.Transform.Apply(center.Shift(transform.m_Position.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                    objectType = Feature.POITransport
                };

                if (useAddress)
                {
                    Entity helipadOwner = helipad;

                    while (ownerLookup.HasComponent(helipadOwner))
                    {
                        ownerLookup.TryGetComponent(helipadOwner, out Game.Common.Owner owner);
                        helipadOwner = owner.m_Owner;
                    }

                    if (buildingLookup.TryGetComponent(helipadOwner, out Building ownerBuilding))
                    {
                        if (SharedDataCollectionSystem.GetAddress(helipadOwner, ownerBuilding.m_RoadEdge, ownerBuilding.m_CurvePosition, out Entity road, out int number,
                                                                  ref aggregateElementBufferLookup, ref aggregatedLookup, ref buildingDataLookup,
                                                                  ref curveLookup, ref compositionLookup, ref edgeLookup,
                                                                  ref netCompositionDataLookup, ref prefabRefLookup, ref roundaboutLookup,
                                                                  ref transformLookup))
                        {
                            poi.address = new(road, number);
                        }
                    }
                }

                NativeParallelHashSet<EnumWrapper<POICategory>> categories = new(1, Allocator.Persistent)
                {
                    POICategory.Helipad
                };

                POIs.AddNoResize(poi);
                POICategories.TryAdd(helipad, categories);
            }
        }

        /// <summary>
        /// The job to collect utility pylons and utility poles.
        /// （收集電塔與電線杆的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectPylonsJob : IJobEntity
        {
            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public NativeParallelHashSet<Entity> poles;

            [ReadOnly]
            public NativeParallelHashSet<Entity> pylons;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeList<POI>.ParallelWriter POIs;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeParallelHashSet<EnumWrapper<POICategory>>>.ParallelWriter POICategories;
            
            public void Execute(in Game.Prefabs.PrefabRef prefabRef, in Game.Objects.Transform transform, Entity pylon)
            {
                bool isPole = poles.Contains(prefabRef.m_Prefab);
                bool isPylon = pylons.Contains(prefabRef.m_Prefab);
                if (!isPole && !isPylon) return;

                POI poi = new()
                {
                    entity = pylon,
                    address = default,
                    brand = -1,
                    hasOwner = false,
                    inGamePosition = transform.m_Position,
                    isAddressVerified = false,
                    isPrivate = false,
                    location = Geodata.Transform.Apply(center.Shift(transform.m_Position.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                    objectType = Feature.POIUtility
                };

                NativeParallelHashSet<EnumWrapper<POICategory>> categories = new(2, Allocator.Persistent);
                if (isPole)  categories.Add(POICategory.UtilityPole);
                if (isPylon) categories.Add(POICategory.UtilityPylon);

                POIs.AddNoResize(poi);
                POICategories.TryAdd(pylon, categories);
            }
        }

        /// <summary>
        /// The job to collect the prefabs of utility pylons and utility poles.
        /// （收集電塔與電線杆預製模板的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectPylonPrefabsJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashSet<Entity>.ParallelWriter poles;

            [WriteOnly]
            public NativeParallelHashSet<Entity>.ParallelWriter pylons;

            public void Execute(in Game.Prefabs.UtilityObjectData utilityObjectData, Entity utilityObject)
            {
                UtilityTypes utilityType = utilityObjectData.m_UtilityTypes;
                if ((utilityType & UtilityTypes.LowVoltageLine) != 0)  poles.Add(utilityObject);
                if ((utilityType & UtilityTypes.HighVoltageLine) != 0) pylons.Add(utilityObject);
            }
        }

        /// <summary>
        /// The job to collect traffic light POIs.
        /// （收集交通號誌燈的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectTrafficLightsJob : IJobEntity
        {
            [ReadOnly]
            public bool useAddress;

            [ReadOnly]
            public BufferLookup<AggregateElement> aggregateElementBufferLookup;

            [ReadOnly]
            public ComponentLookup<Aggregated> aggregatedLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.BuildingData> buildingDataLookup;

            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<Edge> edgeLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Road> roadLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.Roundabout> roundaboutLookup;

            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeList<POI>.ParallelWriter POIs;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeParallelHashSet<EnumWrapper<POICategory>>>.ParallelWriter POICategories;

            public void Execute(in Game.Net.Node node, in TrafficLights trafficLightsComponent, Entity trafficLight, in DynamicBuffer<ConnectedEdge> connectedEdges)
            {
                TrafficLightFlags trafficLightFlags = trafficLightsComponent.m_Flags;
                if ((trafficLightFlags & TrafficLightFlags.MoveableBridge) != 0) return;

                POI poi = new()
                {
                    entity = trafficLight,
                    address = default,
                    brand = -1,
                    hasOwner = false,
                    inGamePosition = node.m_Position,
                    isAddressVerified = false,
                    isPrivate = false,
                    location = Geodata.Transform.Apply(center.Shift(node.m_Position.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                    objectType = Feature.POITransport
                };

                if (useAddress)
                {
                    if (connectedEdges.Length > 0)
                    {
                        int edgeIndex = 0;
                        while (!roadLookup.HasComponent(connectedEdges[edgeIndex].m_Edge) && (edgeIndex < connectedEdges.Length - 1))
                        {
                            edgeIndex++;
                        }

                        Entity connectedEdge = connectedEdges[edgeIndex].m_Edge;
                        if (!edgeLookup.TryGetComponent(connectedEdge, out Edge edgeComponent)) return;
                        
                        float curvePosition = edgeComponent.m_Start.Equals(trafficLight) ? 0f : 1f;
                        if (SharedDataCollectionSystem.GetAddress(trafficLight, connectedEdge, curvePosition, out Entity road, out int number,
                                                                  node.m_Position, node.m_Rotation,
                                                                  ref aggregateElementBufferLookup, ref aggregatedLookup, ref buildingDataLookup,
                                                                  ref curveLookup, ref compositionLookup, ref edgeLookup,
                                                                  ref netCompositionDataLookup, ref prefabRefLookup, ref roundaboutLookup))
                        {
                            poi.address = new(road, number);
                        }
                    }
                }

                POICategory trafficLightCategory = ((trafficLightFlags & TrafficLightFlags.LevelCrossing) != 0) ? POICategory.LevelCrossing : POICategory.TrafficLight;
                NativeParallelHashSet<EnumWrapper<POICategory>> categories = new(1, Allocator.Persistent)
                {
                    trafficLightCategory
                };

                POIs.AddNoResize(poi);
                POICategories.TryAdd(trafficLight, categories);
            }
        }

        /// <summary>
        /// The job to collect transportation stop POIs.
        /// （收集交通運輸站點興趣點的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectTransportStopMarkersJob : IJobEntity
        {
            [ReadOnly]
            public bool useAddress;

            [ReadOnly]
            public BufferLookup<AggregateElement> aggregateElementBufferLookup;

            [ReadOnly]
            public ComponentLookup<Aggregated> aggregatedLookup;

            [ReadOnly]
            public ComponentLookup<Attached> attachedLookup;

            [ReadOnly]
            public ComponentLookup<Building> buildingLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.BuildingData> buildingDataLookup;

            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<CustomName> customNameLookup;

            [ReadOnly]
            public ComponentLookup<Edge> edgeLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<Game.Common.Owner> ownerLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.Roundabout> roundaboutLookup;

            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Game.Prefabs.TransportStopData> transportStopDataMap;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeList<POI>.ParallelWriter POIs;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeParallelHashSet<EnumWrapper<POICategory>>>.ParallelWriter POICategories;

            public void Execute(in Game.Prefabs.PrefabRef prefabRef, in Game.Objects.Transform transform, Entity stop)
            {
                POI poi = new()
                {
                    entity = stop,
                    address = default,
                    brand = -1,
                    hasCustomName = customNameLookup.HasComponent(stop),
                    hasOwner = ownerLookup.HasComponent(stop),
                    inGamePosition = transform.m_Position,
                    isAddressVerified = false,
                    isPrivate = false,
                    location = Geodata.Transform.Apply(center.Shift(transform.m_Position.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                    objectType = Feature.POITransport,
                    owner = Entity.Null
                };

                Entity stopOwner = stop;

                while (ownerLookup.HasComponent(stopOwner))
                {
                    ownerLookup.TryGetComponent(stopOwner, out Game.Common.Owner owner);
                    stopOwner = owner.m_Owner;
                }

                if (poi.hasOwner) poi.owner = stopOwner;

                if (buildingLookup.TryGetComponent(stopOwner, out Building ownerBuilding))
                {
                    if (SharedDataCollectionSystem.GetAddress(stopOwner, ownerBuilding.m_RoadEdge, ownerBuilding.m_CurvePosition, out Entity road, out int number,
                                                              ref aggregateElementBufferLookup, ref aggregatedLookup, ref buildingDataLookup,
                                                              ref curveLookup, ref compositionLookup, ref edgeLookup,
                                                              ref netCompositionDataLookup, ref prefabRefLookup, ref roundaboutLookup,
                                                              ref transformLookup))
                    {
                        poi.address = new(road, number);
                    }
                }
                else if (attachedLookup.TryGetComponent(stopOwner, out Attached attachedComponent))
                {
                    if (SharedDataCollectionSystem.GetAddress(stopOwner, attachedComponent.m_Parent, attachedComponent.m_CurvePosition, out Entity road, out int number,
                                                              ref aggregateElementBufferLookup, ref aggregatedLookup, ref buildingDataLookup,
                                                              ref curveLookup, ref compositionLookup, ref edgeLookup,
                                                              ref netCompositionDataLookup, ref prefabRefLookup, ref roundaboutLookup,
                                                              ref transformLookup))
                    {
                        poi.address = new(road, number);
                    }
                }

                POICategory cargoStopCategory = POICategory.None;
                POICategory passengerStopCategory = POICategory.None;

                if (!transportStopDataMap.TryGetValue(prefabRef.m_Prefab, out Game.Prefabs.TransportStopData stopData))
                {
                    return;
                }
                else
                {
                    bool cargo = stopData.m_CargoTransport;
                    bool passenger = stopData.m_PassengerTransport;
                    Game.Prefabs.TransportType transportType = stopData.m_TransportType;

                    switch (transportType)
                    {
                        case Game.Prefabs.TransportType.Airplane:
                            if (cargo) cargoStopCategory = POICategory.StopCargoAirplane;
                            if (passenger) passengerStopCategory = POICategory.StopPassengerAirplane;
                            break;

                        case Game.Prefabs.TransportType.Bus:
                            passengerStopCategory = POICategory.StopBus;
                            break;

                        case Game.Prefabs.TransportType.Ferry:
                            passengerStopCategory= POICategory.StopFerry;
                            break;

                        case Game.Prefabs.TransportType.Helicopter:
                            passengerStopCategory = POICategory.StopHelicopter;
                            break;

                        case Game.Prefabs.TransportType.Post:
                            passengerStopCategory = POICategory.PostBox;
                            break;

                        case Game.Prefabs.TransportType.Rocket:
                            passengerStopCategory = POICategory.SpaceCenter;
                            break;

                        case Game.Prefabs.TransportType.Ship:
                            if (cargo) cargoStopCategory = POICategory.StopCargoShip;
                            if (passenger) passengerStopCategory = POICategory.StopPassengerShip;
                            break;

                        case Game.Prefabs.TransportType.Subway:
                            passengerStopCategory = POICategory.StopSubway;
                            break;

                        case Game.Prefabs.TransportType.Taxi:
                            passengerStopCategory = POICategory.StopTaxi;
                            break;

                        case Game.Prefabs.TransportType.Train:
                            if (cargo) cargoStopCategory = POICategory.StopCargoTrain;
                            if (passenger) passengerStopCategory = POICategory.StopPassengerTrain;
                            break;

                        case Game.Prefabs.TransportType.Tram:
                            passengerStopCategory = POICategory.StopTram;
                            break;

                        default:
                            passengerStopCategory = POICategory.TransportationGeneric;
                            break;
                    }
                }

                NativeParallelHashSet<EnumWrapper<POICategory>> categories = new(2, Allocator.Persistent);
                if (cargoStopCategory != POICategory.None) categories.Add(cargoStopCategory);
                if (passengerStopCategory != POICategory.None) categories.Add(passengerStopCategory);

                POIs.AddNoResize(poi);
                POICategories.TryAdd(stop, categories);
            }
        }

        /// <summary>
        /// The job to collect input prefab's transport stop data.
        /// （收集輸入預製模板的運輸站點資料的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectTransportStopDataJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, Game.Prefabs.TransportStopData>.ParallelWriter map;

            public void Execute(in Game.Prefabs.TransportStopData stopData, Entity stop)
            {
                map.TryAdd(stop, stopData);
            }
        }

        /// <summary>
        /// The job to retrieve the POI information from <see cref="BuildingStat"/>s.
        /// （由 <see cref="BuildingStat"/> 獲得興趣點資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectPOIsFromBuildingStatsJob : IJobParallelFor
        {
            [ReadOnly]
            public bool separateServiceUpgrade;

            [ReadOnly]
            public BufferLookup<InstalledUpgrade> installedUpgradeBufferLookup;

            [ReadOnly]
            public BufferLookup<SubObject> subObjectBufferLookup;

            [ReadOnly]
            public ComponentLookup<Abandoned> abandonedLookup;

            [ReadOnly]
            public ComponentLookup<AdminBuilding> adminBuildingLookup;

            [ReadOnly]
            public ComponentLookup<Battery> batteryLookup;

            [ReadOnly]
            public ComponentLookup<CommercialProperty> commercialPropertyLookup;

            [ReadOnly]
            public ComponentLookup<Condemned> condemnedLookup;

            [ReadOnly]
            public ComponentLookup<CustomName> customNameLookup;

            [ReadOnly]
            public ComponentLookup<DeathcareFacility> deathcareFacilityLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.DeathcareFacilityData> deathcareFacilityDataLookup;

            [ReadOnly]
            public ComponentLookup<Game.Common.Destroyed> destroyedLookup;

            [ReadOnly]
            public ComponentLookup<DisasterFacility> disasterFacilityLookup;

            [ReadOnly]
            public ComponentLookup<EarlyDisasterWarningSystem> earlyDisasterWarningSystemLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityProducer> electricityProducerLookup;

            [ReadOnly]
            public ComponentLookup<EmergencyShelter> emergencyShelterLookup;

            [ReadOnly]
            public ComponentLookup<ExtractorFacility> extractorFacilityLookup;

            [ReadOnly]
            public ComponentLookup<FireStation> fireStationLookup;

            [ReadOnly]
            public ComponentLookup<FirewatchTower> firewatchTowerLookup;

            [ReadOnly]
            public ComponentLookup<GarbageFacility> garbageFacilityLookup;

            [ReadOnly]
            public ComponentLookup<Hospital> hospitalLookup;

            [ReadOnly]
            public ComponentLookup<IndustrialProperty> industrialPropertyLookup;

            [ReadOnly]
            public ComponentLookup<MaintenanceDepot> maintenanceDepotLookup;

            [ReadOnly]
            public ComponentLookup<Game.Common.Native> nativeLookup;

            [ReadOnly]
            public ComponentLookup<Game.Common.Owner> ownerLookup;

            [ReadOnly]
            public ComponentLookup<Park> parkLookup;

            [ReadOnly]
            public ComponentLookup<ParkingFacility> parkingFacilityLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<PoliceStation> policeStationLookup;

            [ReadOnly]
            public ComponentLookup<PostFacility> postFacilityLookup;

            [ReadOnly]
            public ComponentLookup<Prison> prisonLookup;

            [ReadOnly]
            public ComponentLookup<ResearchFacility> researchFacilityLookup;

            [ReadOnly]
            public ComponentLookup<ResidentialProperty> residentialPropertyLookup;

            [ReadOnly]
            public ComponentLookup<School> schoolLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.SchoolData> schoolDataLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.ServiceData> serviceDataLookup;

            [ReadOnly]
            public ComponentLookup<ServiceUpgrade> serviceUpgradeLookup;

            [ReadOnly]
            public ComponentLookup<SewageOutlet> sewageOutletLookup;

            [ReadOnly]
            public ComponentLookup<StorageProperty> storagePropertyLookup;

            [ReadOnly]
            public ComponentLookup<TelecomFacility> telecomFacilityLookup;

            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup;

            [ReadOnly]
            public ComponentLookup<Transformer> transformerLookup;

            [ReadOnly]
            public ComponentLookup<TransportDepot> transportDepotLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.TransportDepotData> transportDepotDataLookup;

            [ReadOnly]
            public ComponentLookup<TransportStation> transportStationLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.TransportStopData> transportStopDataLookup;

            [ReadOnly]
            public ComponentLookup<UnderConstruction> underConstructionLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.WaterPoweredData> waterPoweredDataLookup;

            [ReadOnly]
            public ComponentLookup<WaterPumpingStation> waterPumpingStationLookup;

            [ReadOnly]
            public ComponentLookup<WelfareOffice> welfareOfficeLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.WindPoweredData> windPoweredDataLookup;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [ReadOnly]
            public NativeList<ZoningType> zoningTypes;

            [ReadOnly]
            public NativeParallelHashSet<Entity> attractions;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeList<POI>.ParallelWriter POIs;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeParallelHashSet<EnumWrapper<POICategory>>>.ParallelWriter POICategories;

            public void Execute(int index)
            {
                BuildingStat building = buildingStats[index];
                Entity entity = building.entity;
                bool hasPrefabRef = prefabRefLookup.TryGetComponent(entity, out Game.Prefabs.PrefabRef prefabRef);
                Entity prefab = prefabRef.m_Prefab;

                if (((ownerLookup.TryGetComponent(entity, out Game.Common.Owner owner) && (owner.m_Owner != Entity.Null)) ||
                      serviceUpgradeLookup.HasComponent(entity)) &&
                    !separateServiceUpgrade) return;

                if (!transformLookup.TryGetComponent(entity, out Game.Objects.Transform transformComponent)) return;
                float3 poiLocation = transformComponent.m_Position;

                BuildingCategory buildingCategory = building.category;
                NativeParallelHashSet<EnumWrapper<POICategory>> categories = new(GetMaximumCategoryCount(buildingCategory), Allocator.Persistent);

                GetPOICategoryFromBuilding(entity, prefab, buildingCategory, building.product, building.brand, building.zoning, hasPrefabRef, false,
                                           ref subObjectBufferLookup, ref batteryLookup, ref deathcareFacilityDataLookup, ref electricityProducerLookup,
                                           ref firewatchTowerLookup, ref prefabRefLookup, ref prisonLookup, ref serviceDataLookup,
                                           ref schoolDataLookup, ref storagePropertyLookup, ref transformerLookup, ref transportDepotLookup,
                                           ref transportDepotDataLookup, ref transportStationLookup, ref transportStopDataLookup, ref waterPoweredDataLookup,
                                           ref windPoweredDataLookup, ref zoningTypes, ref attractions, ref categories, out Feature objectType);

                if (!separateServiceUpgrade && installedUpgradeBufferLookup.TryGetBuffer(entity, out DynamicBuffer<InstalledUpgrade> installedUpgrades))
                {
                    for (int i = 0; i < installedUpgrades.Length; i++)
                    {
                        Entity serviceUpgrade = installedUpgrades[i].m_Upgrade;
                        BuildingCategory subBuildingCategory = SharedDataCollectionSystem.GetBuildingCategory(serviceUpgrade, ref abandonedLookup, ref adminBuildingLookup,
                                                                                                              ref batteryLookup, ref commercialPropertyLookup, ref condemnedLookup,
                                                                                                              ref deathcareFacilityLookup, ref destroyedLookup, ref disasterFacilityLookup,
                                                                                                              ref earlyDisasterWarningSystemLookup, ref electricityProducerLookup, ref emergencyShelterLookup,
                                                                                                              ref extractorFacilityLookup, ref fireStationLookup, ref firewatchTowerLookup,
                                                                                                              ref garbageFacilityLookup, ref hospitalLookup, ref industrialPropertyLookup,
                                                                                                              ref maintenanceDepotLookup, ref nativeLookup, ref parkLookup,
                                                                                                              ref parkingFacilityLookup, ref policeStationLookup, ref postFacilityLookup,
                                                                                                              ref prisonLookup, ref researchFacilityLookup, ref residentialPropertyLookup,
                                                                                                              ref schoolLookup, ref serviceUpgradeLookup, ref sewageOutletLookup,
                                                                                                              ref telecomFacilityLookup, ref transformerLookup, ref transportDepotLookup,
                                                                                                              ref transportStationLookup, ref underConstructionLookup, ref waterPumpingStationLookup,
                                                                                                              ref welfareOfficeLookup);
                        categories.Capacity += GetMaximumCategoryCount(subBuildingCategory);
                        bool hasSubPrefabRef = prefabRefLookup.TryGetComponent(serviceUpgrade, out Game.Prefabs.PrefabRef subPrefabRef);
                        GetPOICategoryFromBuilding(serviceUpgrade, subPrefabRef.m_Prefab, subBuildingCategory, Resource.NoResource, -1, -1, hasSubPrefabRef, true,
                                                   ref subObjectBufferLookup, ref batteryLookup, ref deathcareFacilityDataLookup, ref electricityProducerLookup,
                                                   ref firewatchTowerLookup, ref prefabRefLookup, ref prisonLookup, ref serviceDataLookup,
                                                   ref schoolDataLookup, ref storagePropertyLookup, ref transformerLookup, ref transportDepotLookup,
                                                   ref transportDepotDataLookup, ref transportStationLookup, ref transportStopDataLookup, ref waterPoweredDataLookup,
                                                   ref windPoweredDataLookup, ref zoningTypes, ref attractions, ref categories, out Feature extraObjectType);

                        objectType |= extraObjectType;
                    }
                }

                if (categories.IsEmpty) return;

                POI poi = new()
                {
                    entity = entity,
                    address = building.address,
                    brand = building.brand,
                    hasCustomName = customNameLookup.HasComponent(entity),
                    inGamePosition = poiLocation,
                    isAddressVerified = true,
                    isPrivate = building.brand >= 0,
                    location = Geodata.Transform.Apply(center.Shift(poiLocation.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                    objectType = objectType,
                };

                POIs.AddNoResize(poi);
                POICategories.TryAdd(entity, categories);
            }
        }

        /// <summary>
        /// The job to collect input prefab's UI group.
        /// （收集輸入預製模板的 UI 組別的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectUIObjectGroups : IJobEntity
        {
            [WriteOnly]
            public NativeList<PrefabUIGroup>.ParallelWriter list;

            public void Execute(in Game.Prefabs.UIObjectData uiObjectData, Entity entity)
            {
                list.AddNoResize(new()
                {
                    prefab = entity,
                    uiGroup = uiObjectData.m_Group
                });
            }
        }

        /// <summary>
        /// The job to retrieve POIs' in-game positions.
        /// （獲得興趣點遊戲內位置的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct ExtractPOIPositionsJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeList<POI> list;

            [WriteOnly]
            public NativeArray<float3> positions;

            public void Execute(int index)
            {
                POI poi = list[index];
                positions[index] = poi.inGamePosition;
            }
        }

        /// <summary>
        /// The job to map districts to POIs.
        /// （將行政區映射至興趣點的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct MapDistrictsToPOIsJob : IJobEntity
        {
            [ReadOnly]
            public NativeParallelMultiHashMap<Entity, int> areaEntityMap;

            public NativeList<POI> POIs;

            public void Execute(Entity district)
            {
                if (areaEntityMap.TryGetFirstValue(district, out int poiIndex, out NativeParallelMultiHashMapIterator<Entity> iterator))
                {
                    do
                    {
                        if ((poiIndex >= 0) && (poiIndex < POIs.Length))
                        {
                            ref POI poi = ref POIs.ElementAt(poiIndex);

                            if (!poi.isAddressVerified)
                            {
                                poi.address.district = district;
                            }
                        }
                    }
                    while (areaEntityMap.TryGetNextValue(out poiIndex, ref iterator));
                }
            }
        }
    }
}