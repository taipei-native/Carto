namespace Carto.Systems
{
    using Carto.Domain;
    using Carto.Utils;
    using Colossal.Logging;
    using Colossal.Mathematics;
    using Game;
    using Game.Areas;
    using Game.Buildings;
    using Game.City;
    using Game.Common;
    using Game.Companies;
    using Game.Economy;
    using Game.Net;
    using Game.Objects;
    using Game.Prefabs;
    using Game.Routes;
    using Game.Tools;
    using Game.UI;
    using GeometryType = Utils.ExportUtils.GeometryType;
    using Purpose = Colossal.Serialization.Entities.Purpose;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using ZoningCategory = ZoningSystem.ZoningCategory;

    /// <summary>
    /// The system instance that manages the POI properties, inheriting from GameSystemBase.
    /// （管理興趣點屬性的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class POISystem : GameSystemBase
    {
        private static readonly ILog m_Log = Instance.Log;
        private static readonly NameSystem m_Name = Instance.Name;
        private static readonly PrefabSystem m_Prefab = Instance.Prefab;

        // The entity query instance that searches for several instances, using Unity.Entities.EntityQuery.
        // （用於搜尋數種實例的Unity實體查詢實例。）
        private static EntityQuery _buildingQuery;
        private static EntityQuery _districtQuery;
        private static EntityQuery _helipadQuery;
        private static EntityQuery _pylonQuery;
        private static EntityQuery _trafficLightQuery;
        private static EntityQuery _transportStopQuery;

        /// <summary>
        /// The list storing the categories' rank.
        /// （儲存分類排序的列表。）
        /// </summary>
        private static List<POICategory> CategoryRank = new List<POICategory>
        {
            POICategory.BuildingPassengerAirplane, POICategory.BuildingCargoAirplane, POICategory.BuildingPassengerShip, POICategory.BuildingCargoShip, // Rank 1A: Heavy Transport Buildings
            POICategory.BuildingPassengerTrain, POICategory.BuildingCargoTrain, POICategory.BuildingSubway, POICategory.BuildingTram,   // Rank 1B: Rail Transport Buildings
            POICategory.BuildingBus, POICategory.BuildingTaxi, POICategory.BuildingHelicopter, POICategory.SpaceCenter, // Rank 1C: Road & Uncommon Transport Buildings
            POICategory.DepotTrain, POICategory.DepotSubway, POICategory.DepotTram, POICategory.DepotBus, POICategory.DepotTaxi,    // Rank 1D: Depots
            POICategory.StopPassengerAirplane, POICategory.StopCargoAirplane, POICategory.StopPassengerShip, POICategory.StopCargoShip, // Rank 1E: Heavy Transport Stops
            POICategory.StopPassengerTrain, POICategory.StopCargoTrain, POICategory.StopSubway, POICategory.StopTram,   // Rank 1F: Rail Transport Stops
            POICategory.StopBus, POICategory.StopTaxi, POICategory.StopHelicopter, // Rank 1G: Road & Uncommon Transport Stops
            POICategory.TransportationGeneric,  // Rank 1H: Fallback

            POICategory.PowerBattery, POICategory.PowerDam, POICategory.PowerTurbine, POICategory.PowerSubstation, POICategory.PowerPlant, POICategory.PowerGeneric, // Rank 2A: Power
            POICategory.Water, POICategory.Sewage,  // Rank 2B: Water & Sewage
            POICategory.Fire, POICategory.FireWatchTower, POICategory.Disaster, POICategory.Health, POICategory.MortuaryCemetery, POICategory.MortuaryCrematorium, POICategory.MortuaryGeneric, // Rank 2C: Emergency Services
            POICategory.Police, POICategory.Prison, POICategory.Post, POICategory.Maintenance, POICategory.Admin,   // Rank 2D: Administrative Services
            POICategory.Research, POICategory.EducationUniversity, POICategory.EducationCollege, POICategory.EducationHigh, POICategory.EducationElementary, POICategory.EducationGeneric,  // Rank 2E: Education & Research
            POICategory.Communication, POICategory.Waste,   // Rank 2F: Other Utility Services
            POICategory.Attraction, POICategory.Park, POICategory.Parking,  // Rank 2G: Recreations
            
            POICategory.StoreBank, POICategory.StoreSoftware, POICategory.StoreTelecom, POICategory.StoreMedia, POICategory.StoreOffice,    // Rank 3A: Offices
            POICategory.StoreHotel, POICategory.StoreGasStation,    // Rank 3B: Road Services
            POICategory.StoreRestaurant, POICategory.StoreBar, POICategory.StoreBeverage, POICategory.StoreFood, POICategory.StoreConvenienceStore,    // Rank 3C: Catering
            POICategory.StoreBookStore, POICategory.StoreCarStore, POICategory.StoreChemicals, POICategory.StoreDrugStore, POICategory.StoreElectronics,    // Rank 3D: Other Commercial
            POICategory.StoreFashionStore, POICategory.StoreFurniture, POICategory.StorePlastics, POICategory.StoreRecreation, POICategory.StoreGeneric,    // Rank 3E: Other Commercial
            POICategory.IndustiralCoal, POICategory.IndustrialCotton, POICategory.IndustrialGrain, POICategory.IndustrialLivestock, POICategory.IndustrialOil,  // Rank 3F: Specialized Industries
            POICategory.IndustrialOre, POICategory.IndustrialStone, POICategory.IndustrialVegetables, POICategory.IndustrialWood,   // Rank 3G: Specialized Industries
            POICategory.IndustrialWarehouse, POICategory.IndustrialFactory, POICategory.IndustrialGeneric,  // Rank 3H: Normal Industries

            POICategory.TrafficLight, POICategory.PostBox, POICategory.UtilityPylon, POICategory.UtilityPole, POICategory.Helipad, // Rank 4A: Objects
            POICategory.None,   // Rank 4B: Fallback
        };

        /// <summary>
        /// The dictionary storing all triangles forming the districts.
        /// 儲存所有組成行政區的三角形的字典。
        /// </summary>
        private static Dictionary<Entity, List<Triangle3>> Districts = new Dictionary<Entity, List<Triangle3>>();

        /// <summary>
        /// The enum storing POI categories.
        /// （儲存興趣點分類的列舉。）
        /// </summary>
        public enum POICategory
        {
            /// <summary>
            /// The POI without special classification.（沒有特殊分類的興趣點。）
            /// </summary>
            None = 0,

            // Public Service Facilities（公眾服務設施）
            Admin,
            Communication,
            Disaster,
            EducationCollege,
            EducationElementary,
            EducationGeneric,
            EducationHigh,
            EducationUniversity,
            Fire,
            FireWatchTower,
            Health,
            Maintenance,
            MortuaryCemetery,
            MortuaryCrematorium,
            MortuaryGeneric,
            Police,
            Post,
            PostBox,
            PowerBattery,
            PowerDam,
            PowerGeneric,
            PowerPlant,
            PowerSubstation,
            PowerTurbine,
            Prison,
            Research,
            Sewage,
            UtilityPole,
            UtilityPylon,
            Waste,
            Water,

            // Public Transportation Facilities（大眾運輸設施）
            Helipad,
            Parking,
            SpaceCenter,
            TrafficLight,
            BuildingBus,
            BuildingCargoAirplane,
            BuildingCargoShip,
            BuildingCargoTrain,
            BuildingHelicopter,
            BuildingPassengerAirplane,
            BuildingPassengerShip,
            BuildingPassengerTrain,
            BuildingSubway,
            BuildingTaxi,
            BuildingTram,
            DepotBus,
            DepotSubway,
            DepotTaxi,
            DepotTrain,
            DepotTram,
            StopBus,
            StopCargoAirplane,
            StopCargoShip,
            StopCargoTrain,
            StopHelicopter,
            StopPassengerAirplane,
            StopPassengerShip,
            StopPassengerTrain,
            StopSubway,
            StopTaxi,
            StopTram,
            TransportationGeneric,            

            // Commercial Facilities & Offices（商辦設施）
            /// <summary>
            /// The office that sells the products not listed below.（販售未在下表列出的產品的辦公場所。）
            /// </summary>
            StoreOffice,

            /// <summary>
            /// The commercial store that sells the products not listed below.（販售未在下表列出的產品的商店。）
            /// </summary>
            StoreGeneric,

            /// <summary>
            /// The office that sells Financial.（販售金融的辦公場所。）
            /// </summary>
            StoreBank,

            /// <summary>
            /// The store that sells Entertainment.（販售娛樂的商店。）
            /// </summary>
            StoreBar,

            /// <summary>
            /// The store that sells Paper.（販售紙張的商店。）
            /// </summary>
            StoreBookStore,

            /// <summary>
            /// The store that sells Beverage.（販售飲料的商店。）
            /// </summary>
            StoreBeverage,

            /// <summary>
            /// The store that sells Vehicles.（販售車輛的商店。）
            /// </summary>
            StoreCarStore,

            /// <summary>
            /// The store that sells Chemicals.（販售化學品的商店。）
            /// </summary>
            StoreChemicals,

            /// <summary>
            /// The store that sells Convenience Food.（販售即時食品的商店。）
            /// </summary>
            StoreConvenienceStore,

            /// <summary>
            /// The store that sells Pharmaceuticals.（販售藥品的商店。）
            /// </summary>
            StoreDrugStore,

            /// <summary>
            /// The store that sells Electronics.（販售電子產品的商店。）
            /// </summary>
            StoreElectronics,

            /// <summary>
            /// The store that sells Textiles.（販售紡織品的商店。）
            /// </summary>
            StoreFashionStore,

            /// <summary>
            /// The store that sells Food.（販售食物的商店。）
            /// </summary>
            StoreFood,

            /// <summary>
            /// The store that sells Furnitures.（販售家具的商店。）
            /// </summary>
            StoreFurniture,

            /// <summary>
            /// The store that sells Petrochemicals.（販售石化產品的商店。）
            /// </summary>
            StoreGasStation,

            /// <summary>
            /// The store that sells Lodging.（販售旅宿的商店。）
            /// </summary>
            StoreHotel,

            /// <summary>
            /// The office that sells Media.（販售媒體的辦公場所。）
            /// </summary>
            StoreMedia,

            /// <summary>
            /// The store that sells Plastics.（販售塑膠的商店。）
            /// </summary>
            StorePlastics,

            /// <summary>
            /// The store that sells Recreation.（販售休閒的商店。）
            /// </summary>
            StoreRecreation,

            /// <summary>
            /// The store that sells Meals.（販售膳食的商店。）
            /// </summary>
            StoreRestaurant,

            /// <summary>
            /// The office that sells Software.（販售軟體的辦公場所。）
            /// </summary>
            StoreSoftware,

            /// <summary>
            /// The office that sells Telecom.（販售電信的辦公場所。）
            /// </summary>
            StoreTelecom,

            // Industrial Facilities（工業設施）
            IndustiralCoal,
            IndustrialCotton,
            IndustrialFactory,
            IndustrialGeneric,
            IndustrialGrain,
            IndustrialLivestock,
            IndustrialOil,
            IndustrialOre,
            IndustrialStone,
            IndustrialVegetables,
            IndustrialWarehouse,
            IndustrialWood,

            // Other Facilities（其他設施）
            /// <summary>
            /// The tourist attractions & landmarks.（旅遊景點與地標。）
            /// </summary>
            Attraction,
            Park
        }

        /// <summary>
        /// This event triggers when the system is created.
        /// （這是當系統實例被創造時，會被觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _buildingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Building>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Placeholder>(),

                    ComponentType.ReadOnly<Destroyed>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _districtQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<District>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _helipadQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
                    ComponentType.ReadOnly<Game.Routes.TakeoffLocation>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _pylonQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Pillar>(),
                    ComponentType.ReadOnly<Game.Objects.UtilityObject>(),
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _trafficLightQuery = GetEntityQuery(new EntityQueryDesc
            { 
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Net.Node>(),
                    ComponentType.ReadOnly<TrafficLights>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _transportStopQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Routes.TransportStop>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),

                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            base.OnCreate();
            m_Log.Debug("POISystem instance created. 興趣點系統實例創造完成。");
        }

        /// <summary>
        /// This event triggers when the game is loaded.
        /// （這是當遊戲載入完成時，會被觸發的事件。）
        /// </summary>
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode) { }

        /// <summary>
        /// This event triggers when the system is updated.
        /// （這是當系統實例被更新時，會被觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// This event triggers when the system is destroyed.
        /// （這是當系統實例被銷毀時，會被觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// Retrieve the district the object situated in.
        /// （獲取物體所在的行政區。）
        /// </summary>
        /// <param name="position">The object's position.（物體的位置。）</param>
        public string GetObjectCurrentDistrict(float3 position)
        {
            if (Districts.Count == 0)
            {
                return LocaleUtils.Translate("Carto.address.UNINCORPORATED[District]");
            }
            else
            {
                foreach(KeyValuePair<Entity, List<Triangle3>> kvp in Districts)
                {
                    foreach (Triangle3 triangle in kvp.Value)
                    {
                        if (MathUtils.Intersect(triangle.xz, position.xz))
                        {
                            return m_Name.GetRenderedLabelName(kvp.Key);
                        }
                    }
                }

                return LocaleUtils.Translate("Carto.address.UNINCORPORATED[District]");
            }
        }

        /// <summary>
        /// Retrieve the category of the POI.
        /// （獲取興趣點的分類。）
        /// </summary>
        public static List<POICategory> GetPOICategoryFromBuilding(Entity entity, EntityManager entityManager, out string brand)
        {
            Entity prefab = entityManager.GetComponentData<PrefabRef>(entity).m_Prefab;
            List<POICategory> categories = new List<POICategory>();
            Dictionary<Entity, ZoningCategory?> zoningCategory = Instance.Zoning.GetZoningTypes(false, false, false, true).ToDictionary(kvp => kvp.Value.Entity, kvp => kvp.Value.Category);

            bool useUpgrade = Instance.Settings.UseUpgrade;
            bool hasPowerClassification = false;
            bool hasTransportClassification = false;
            brand = string.Empty;

            // Public Service Facilities（公眾服務設施）
            
            if (entityManager.HasComponent<AdminBuilding>(entity) || entityManager.HasComponent<Game.Buildings.WelfareOffice>(entity))
            {
                categories.Add(POICategory.Admin);
            }
            if (entityManager.HasComponent<Game.Buildings.TelecomFacility>(entity))
            {
                categories.Add(POICategory.Communication);
            }
            if (entityManager.HasComponent<Game.Buildings.DisasterFacility>(entity) || entityManager.HasComponent<Game.Buildings.EmergencyShelter>(entity))
            {
                categories.Add(POICategory.Disaster);
            }
            if (entityManager.HasComponent<Game.Buildings.School>(entity))
            {
                if (entityManager.HasComponent<SchoolData>(prefab))
                {
                    switch (entityManager.GetComponentData<SchoolData>(prefab).m_EducationLevel)
                    {
                        case 1:
                            categories.Add(POICategory.EducationElementary);
                            break;

                        case 2:
                            categories.Add(POICategory.EducationHigh);
                            break;

                        case 3:
                            categories.Add(POICategory.EducationCollege);
                            break;

                        case 4:
                            categories.Add(POICategory.EducationUniversity);
                            break;

                        default:
                            categories.Add(POICategory.EducationGeneric);
                            break;
                    }
                }
                else
                {
                    categories.Add(POICategory.EducationGeneric);
                }
            }
            if (entityManager.HasComponent<Game.Buildings.FireStation>(entity))
            {
                categories.Add(POICategory.Fire);
            }
            if (entityManager.HasComponent<Game.Buildings.FirewatchTower>(entity))
            {
                categories.Add(POICategory.FireWatchTower);
            }
            if (entityManager.HasComponent<Game.Buildings.Hospital>(entity))
            {
                categories.Add(POICategory.Health);
            }
            if (entityManager.HasComponent<Game.Buildings.MaintenanceDepot>(entity))
            {
                categories.Add(POICategory.Maintenance);
            }
            if (entityManager.HasComponent<Game.Buildings.DeathcareFacility>(entity))
            {
                if (entityManager.HasComponent<DeathcareFacilityData>(prefab))
                {
                    if (entityManager.GetComponentData<DeathcareFacilityData>(prefab).m_LongTermStorage)
                    {
                        categories.Add(POICategory.MortuaryCemetery);
                    }
                    else
                    {
                        categories.Add(POICategory.MortuaryCrematorium);
                    }
                }
                else
                {
                    categories.Add(POICategory.MortuaryGeneric);
                }
            }
            if (entityManager.HasComponent<Game.Buildings.PoliceStation>(entity))
            {
                categories.Add(POICategory.Police);
            }
            if (entityManager.HasComponent<Game.Buildings.PostFacility>(entity))
            {
                categories.Add(POICategory.Post);
            }
            if (entityManager.HasComponent<Game.Buildings.Battery>(entity))
            {
                hasPowerClassification = true;
                categories.Add(POICategory.PowerBattery);
            }
            if (entityManager.HasComponent<ElectricityProducer>(entity))
            {
                if (entityManager.HasComponent<WaterPoweredData>(prefab))
                {
                    hasPowerClassification = true;
                    categories.Add(POICategory.PowerDam);
                }
                if (entityManager.HasComponent<PowerPlantData>(prefab) && !entityManager.HasComponent<WindPoweredData>(prefab) && !entityManager.HasComponent<WaterPoweredData>(prefab))
                {
                    hasPowerClassification = true;
                    categories.Add(POICategory.PowerPlant);
                }
                if (entityManager.HasComponent<WindPoweredData>(prefab))
                {
                    hasPowerClassification = true;
                    categories.Add(POICategory.PowerTurbine); 
                }
            }
            if (entityManager.HasComponent<Game.Buildings.Transformer>(entity))
            {
                hasPowerClassification = true;
                categories.Add(POICategory.PowerSubstation);
            }
            if (entityManager.HasComponent<Game.Buildings.Prison>(entity))
            {
                categories.Add(POICategory.Prison);
            }
            if (entityManager.HasComponent<Game.Buildings.ResearchFacility>(entity))
            {
                categories.Add(POICategory.Research);
            }
            if (entityManager.HasComponent<Game.Buildings.SewageOutlet>(entity))
            {
                categories.Add(POICategory.Sewage);
            }
            if (entityManager.HasComponent<Game.Buildings.GarbageFacility>(entity))
            {
                categories.Add(POICategory.Waste);
            }
            if (entityManager.HasComponent<Game.Buildings.WaterPumpingStation>(entity))
            {
                categories.Add(POICategory.Water);
            }

            // Public Transportation Facilities（大眾運輸設施）

            List<POICategory> GetTransportDepotCategory(Entity depot)
            {
                List<POICategory> transportDepotCategories = new List<POICategory>();
                Entity depotPrefab = entityManager.GetComponentData<PrefabRef>(depot);

                if (entityManager.HasComponent<TransportDepotData>(depotPrefab))
                {
                    switch (entityManager.GetComponentData<TransportDepotData>(depotPrefab).m_TransportType)
                    {
                        case TransportType.Bus:
                            transportDepotCategories.Add(POICategory.DepotBus);
                            break;

                        case TransportType.Rocket:
                            transportDepotCategories.Add(POICategory.SpaceCenter);
                            break;

                        case TransportType.Subway:
                            transportDepotCategories.Add(POICategory.DepotSubway);
                            break;

                        case TransportType.Taxi:
                            transportDepotCategories.Add(POICategory.DepotTaxi);
                            break;

                        case TransportType.Train:
                            transportDepotCategories.Add(POICategory.DepotTrain);
                            break;

                        case TransportType.Tram:
                            transportDepotCategories.Add(POICategory.DepotTram);
                            break;
                    }
                }

                return transportDepotCategories;
            }

            List<POICategory> GetTransportStationCategory(Entity station)
            {
                List<POICategory> transportCategories = new List<POICategory>();

                if (entityManager.HasBuffer<Game.Objects.SubObject>(station))
                {
                    foreach (Game.Objects.SubObject subObject in entityManager.GetBuffer<Game.Objects.SubObject>(station))
                    {
                        Entity subObjectPrefab = entityManager.GetComponentData<PrefabRef>(subObject.m_SubObject);

                        if (!entityManager.HasComponent<TransportStopData>(subObjectPrefab))
                        {
                            continue;
                        }
                        else
                        {
                            TransportStopData transportStopData = entityManager.GetComponentData<TransportStopData>(subObjectPrefab);
                            TransportType transportType = transportStopData.m_TransportType;
                            bool cargo = transportStopData.m_CargoTransport;
                            bool passenger = transportStopData.m_PassengerTransport;

                            switch (transportType)
                            {
                                case TransportType.Airplane:
                                    if (cargo) transportCategories.Add(POICategory.BuildingCargoAirplane);
                                    if (passenger) transportCategories.Add(POICategory.BuildingPassengerAirplane);
                                    break;

                                case TransportType.Bus:
                                    transportCategories.Add(POICategory.BuildingBus);
                                    break;

                                case TransportType.Helicopter:
                                    transportCategories.Add(POICategory.BuildingHelicopter);
                                    break;

                                case TransportType.Rocket:
                                    transportCategories.Add(POICategory.SpaceCenter);
                                    break;

                                case TransportType.Ship:
                                    if (cargo) transportCategories.Add(POICategory.BuildingCargoShip);
                                    if (passenger) transportCategories.Add(POICategory.BuildingPassengerShip);
                                    break;

                                case TransportType.Subway:
                                    transportCategories.Add(POICategory.BuildingSubway);
                                    break;

                                case TransportType.Taxi:
                                    transportCategories.Add(POICategory.BuildingTaxi);
                                    break;

                                case TransportType.Train:
                                    if (cargo) transportCategories.Add(POICategory.BuildingCargoTrain);
                                    if (passenger) transportCategories.Add(POICategory.BuildingPassengerTrain);
                                    break;

                                case TransportType.Tram:
                                    transportCategories.Add(POICategory.BuildingTram);
                                    break;
                            }
                        }
                    }
                }

                return transportCategories;
            }

            if (entityManager.HasComponent<Game.Buildings.TransportStation>(entity))
            {
                List<POICategory> stationCategories = new List<POICategory>();

                if (!useUpgrade && entityManager.HasBuffer<InstalledUpgrade>(entity))
                {
                    foreach (InstalledUpgrade upgrade in entityManager.GetBuffer<InstalledUpgrade>(entity))
                    {
                        stationCategories.AddRange(GetTransportStationCategory(upgrade));
                    }

                    stationCategories.AddRange(GetTransportStationCategory(entity));
                }
                else
                {
                    stationCategories.AddRange(GetTransportStationCategory(entity).Distinct().ToList());
                }

                if (stationCategories.Count > 0) hasTransportClassification = true;
                categories.AddRange(stationCategories.Distinct().ToList());
            }

            if (entityManager.HasComponent<Game.Buildings.TransportDepot>(entity))
            {
                List<POICategory> depotCategories = new List<POICategory>();

                if (!useUpgrade && entityManager.HasBuffer<InstalledUpgrade>(entity))
                {
                    foreach (InstalledUpgrade upgrade in entityManager.GetBuffer<InstalledUpgrade>(entity))
                    {
                        depotCategories.AddRange(GetTransportDepotCategory(upgrade));
                    }

                    depotCategories.AddRange(GetTransportDepotCategory(entity));
                }
                else
                {
                    depotCategories.AddRange(GetTransportDepotCategory(entity).Distinct().ToList());
                }

                if (depotCategories.Count > 0) hasTransportClassification = true;
                categories.AddRange(depotCategories.Distinct().ToList());
            }

            if (entityManager.HasComponent<Game.Buildings.ParkingFacility>(entity))
            {
                categories.Add(POICategory.Parking);
            }

            if (entityManager.HasComponent<SpawnableBuildingData>(prefab) && (entityManager.HasComponent<CommercialProperty>(entity) || entityManager.HasComponent<IndustrialProperty>(entity)) && !entityManager.HasComponent<ExtractorProperty>(entity) && !entityManager.HasComponent<StorageProperty>(entity))
            {
                ZoningCategory zone = zoningCategory.TryGetValue(entityManager.GetComponentData<SpawnableBuildingData>(prefab).m_ZonePrefab, out ZoningCategory? zoneType) ? (ZoningCategory) zoneType : ZoningCategory.None;
                bool hasProduct = false;
                bool isCommercial = zone.HasFlag(ZoningCategory.Commercial);
                bool isIndustrial = zone.HasFlag(ZoningCategory.Industrial);
                bool isOffice     = zone.HasFlag(ZoningCategory.Office);
                List<POICategory> storeCategories = new List<POICategory>();

                if (entityManager.HasBuffer<Renter>(entity))
                {
                    Resource products = Resource.NoResource;

                    foreach (Renter renter in entityManager.GetBuffer<Renter>(entity))
                    {
                        if (entityManager.HasComponent<CompanyData>(renter.m_Renter))
                        {
                            Entity facilityPrefab = entityManager.GetComponentData<PrefabRef>(renter.m_Renter).m_Prefab;
                            Resource product = entityManager.GetComponentData<IndustrialProcessData>(facilityPrefab).m_Output.m_Resource;
                            brand = m_Name.GetRenderedLabelName(entityManager.GetComponentData<CompanyData>(renter.m_Renter).m_Brand);
                            products |= product;
                            hasProduct = true;
                            break;  // The game only regards the first company that appears in the renter list to be the "real business renter".（遊戲只將第一個出現在租客列表的公司視為租客。）
                        }
                    }

                    if (!isIndustrial)
                    {
                        foreach (Resource resource in MiscUtils.GetFlagComponents(products))
                        {
                            switch (resource)
                            {
                                case Resource.Beverages:
                                    storeCategories.Add(POICategory.StoreBeverage);
                                    break;

                                case Resource.Chemicals:
                                    storeCategories.Add(POICategory.StoreChemicals);
                                    break;

                                case Resource.ConvenienceFood:
                                    storeCategories.Add(POICategory.StoreConvenienceStore);
                                    break;

                                case Resource.Electronics:
                                    storeCategories.Add(POICategory.StoreElectronics);
                                    break;

                                case Resource.Entertainment:
                                    storeCategories.Add(POICategory.StoreBar);
                                    break;

                                case Resource.Financial:
                                    storeCategories.Add(POICategory.StoreBank);
                                    break;

                                case Resource.Food:
                                    storeCategories.Add(POICategory.StoreFood);
                                    break;

                                case Resource.Furniture:
                                    storeCategories.Add(POICategory.StoreFurniture);
                                    break;

                                case Resource.Lodging:
                                    storeCategories.Add(POICategory.StoreHotel);
                                    break;

                                case Resource.Meals:
                                    storeCategories.Add(POICategory.StoreRestaurant);
                                    break;

                                case Resource.Media:
                                    storeCategories.Add(POICategory.StoreMedia);
                                    break;

                                case Resource.Paper:
                                    storeCategories.Add(POICategory.StoreBookStore);
                                    break;

                                case Resource.Petrochemicals:
                                    if (isCommercial) storeCategories.Add(POICategory.StoreGasStation);
                                    break;

                                case Resource.Pharmaceuticals:
                                    storeCategories.Add(POICategory.StoreDrugStore);
                                    break;

                                case Resource.Plastics:
                                    storeCategories.Add(POICategory.StorePlastics);
                                    break;

                                case Resource.Recreation:
                                    storeCategories.Add(POICategory.StoreRecreation);
                                    break;

                                case Resource.Software:
                                    storeCategories.Add(POICategory.StoreSoftware);
                                    break;

                                case Resource.Telecom:
                                    storeCategories.Add(POICategory.StoreTelecom);
                                    break;

                                case Resource.Textiles:
                                    storeCategories.Add(POICategory.StoreFashionStore);
                                    break;

                                case Resource.Vehicles:
                                    storeCategories.Add(POICategory.StoreCarStore);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (hasProduct) storeCategories.Add(POICategory.IndustrialFactory);
                    }
                }

                if (storeCategories.Count == 0)
                {
                    if (isCommercial)
                    {
                        storeCategories.Add(POICategory.StoreGeneric);
                    }
                    else if (isIndustrial)
                    {
                        storeCategories.Add(POICategory.IndustrialGeneric);
                    }
                    else if (isOffice)
                    {
                        storeCategories.Add(POICategory.StoreOffice);
                    }
                }

                categories.AddRange(storeCategories);
            }

            if (entityManager.HasComponent<ExtractorProperty>(entity))
            {
                bool hasProduct = false;

                if (entityManager.HasBuffer<Renter>(entity))
                {
                    Resource products = Resource.NoResource;

                    foreach (Renter renter in entityManager.GetBuffer<Renter>(entity))
                    {
                        if (entityManager.HasComponent<CompanyData>(renter.m_Renter))
                        {
                            Entity facilityPrefab = entityManager.GetComponentData<PrefabRef>(renter.m_Renter).m_Prefab;
                            Resource product = entityManager.GetComponentData<IndustrialProcessData>(facilityPrefab).m_Output.m_Resource;
                            brand = m_Name.GetRenderedLabelName(entityManager.GetComponentData<CompanyData>(renter.m_Renter).m_Brand);
                            products |= product;
                            hasProduct = true;
                        }
                    }

                    foreach (Resource resource in MiscUtils.GetFlagComponents(products))
                    {
                        switch (resource)
                        {
                            case Resource.Coal:
                                categories.Add(POICategory.IndustiralCoal);
                                break;

                            case Resource.Cotton:
                                categories.Add(POICategory.IndustrialCotton);
                                break;

                            case Resource.Grain:
                                categories.Add(POICategory.IndustrialGrain);
                                break;

                            case Resource.Livestock:
                                categories.Add(POICategory.IndustrialLivestock);
                                break;

                            case Resource.Oil:
                                categories.Add(POICategory.IndustrialOil);
                                break;

                            case Resource.Ore:
                                categories.Add(POICategory.IndustrialOre);
                                break;

                            case Resource.Stone:
                                categories.Add(POICategory.IndustrialStone);
                                break;

                            case Resource.Vegetables:
                                categories.Add(POICategory.IndustrialVegetables);
                                break;

                            case Resource.Wood:
                                categories.Add(POICategory.IndustrialWood);
                                break;
                        }
                    }
                }

                if (!hasProduct) categories.Add(POICategory.IndustrialGeneric);
            }

            if (entityManager.HasComponent<StorageProperty>(entity))
            {
                if (entityManager.HasBuffer<Renter>(entity))
                {
                    foreach (Renter renter in entityManager.GetBuffer<Renter>(entity))
                    {
                        if (entityManager.HasComponent<CompanyData>(renter.m_Renter))
                        {
                            brand = m_Name.GetRenderedLabelName(entityManager.GetComponentData<CompanyData>(renter.m_Renter).m_Brand);
                        }
                    }
                }
                
                categories.Add(POICategory.IndustrialWarehouse);
            }

            if (entityManager.HasComponent<ServiceObjectData>(prefab) && (!hasPowerClassification || !hasTransportClassification))
            {
                Entity servicePrefab = entityManager.GetComponentData<ServiceObjectData>(prefab).m_Service;
                
                if (entityManager.HasComponent<ServiceData>(servicePrefab))
                {
                    switch (entityManager.GetComponentData<ServiceData>(servicePrefab).m_Service)
                    {
                        case CityService.Electricity:
                            if (!hasPowerClassification) categories.Add(POICategory.PowerGeneric);
                            break;

                        case CityService.Transportation:
                            if (!hasTransportClassification) categories.Add(POICategory.TransportationGeneric);
                            break;

                        default:
                            break;
                    }
                }

                // Other Facilities（其他設施）

                if (entityManager.HasComponent<Game.Buildings.Park>(entity))
                {
                    if (entityManager.HasComponent<UIObjectData>(prefab))
                    {
                        string UIGroup = m_Prefab.GetPrefabName(entityManager.GetComponentData<UIObjectData>(prefab).m_Group);

                        switch (UIGroup)
                        {
                            case "SignaturesLandmarks":                     // Pre-order Pack & Treasure Hunt buildings.（預購包及尋寶活動建築。）
                                categories.Add(POICategory.Attraction);
                                break;

                            case "TouristAttractions":                      // Base game buildings.（主遊戲建築。）
                                categories.Add(POICategory.Attraction);
                                break;

                            default:
                                categories.Add(POICategory.Park);
                                break;
                        }
                    }
                    else
                    {
                        categories.Add(POICategory.Park);
                    }
                }
            }

            return categories.Count > 0 ? categories : new List<POICategory> { POICategory.None };
        }

        /// <summary>
        /// Searching for properties of all POIs.
        /// （搜尋所有興趣點的屬性。）
        /// </summary>
        public List<CartoObject> GetPOIsProperties(out Dictionary<string, int> fieldLength)
        {
            Dictionary<Entity, POICategory> pylonPrefabs = new Dictionary<Entity, POICategory>();
            Dictionary<Entity, List<POICategory>> transportStopPrefabs = new Dictionary<Entity, List<POICategory>>();
            List<CartoObject> poiList = new List<CartoObject>();
            fieldLength = new Dictionary<string, int>();
            bool considerServiceUpgrades = !Instance.Settings.UseUpgrade;
            bool singleCategory = Instance.Settings.POIBehavior == ExportUtils.POIFormat.Single;
            bool useAddress = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.POI, "Address");
            bool useCategory = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.POI, "Category");
            bool useLocation = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.POI, "Location");
            bool useObject = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.POI, "Object");

            Districts.Clear();

            if (useAddress)
            {
                foreach(Entity _district in _districtQuery.ToEntityArray(Allocator.Temp))
                {
                    DynamicBuffer<Game.Areas.Node> nodes     = EntityManager.GetBuffer<Game.Areas.Node>(_district);
                    DynamicBuffer<Triangle>        triangles = EntityManager.GetBuffer<Triangle>(_district);
                    List<Triangle3> triangleList = new List<Triangle3>();

                    foreach (Triangle triangle in triangles) triangleList.Add(AreaUtils.GetTriangle3(nodes, triangle));
                    Districts[_district] = triangleList;
                }
            }

            foreach (Entity _build in _buildingQuery.ToEntityArray(Allocator.Temp))
            {
                bool isServiceUpgrades = EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(_build);

                if ((isServiceUpgrades && !considerServiceUpgrades) || !isServiceUpgrades)
                {
                    try
                    {
                        List<POICategory> _buildPOI = GetPOICategoryFromBuilding(_build, EntityManager, out string brandName);

                        if (_buildPOI[0] != POICategory.None)
                        {
                            var edges = new Dictionary<string, List<float3>>();
                            var props = new Dictionary<string, object>();
                            var type = new Dictionary<string, GeometryType>();

                            string buildName = string.IsNullOrEmpty(brandName) ? m_Name.GetRenderedLabelName(_build) : brandName;
                            props["Name"] = buildName;
                            fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", buildName);

                            // Retrieve the address of the POI. Expected output: Address(210 Elizabeth Street, Sunnyside Dale)
                            //（獲取興趣點的地址。預期輸出：Address(210 伊莉莎白街, 朝陽溪谷)）
                            if (useAddress)
                            {
                                Address buildPOIAddress = new Address(_build, EntityManager);
                                props["Address"] = buildPOIAddress;
                                int limit = MiscUtils.GetFieldLength(fieldLength, "Address", buildPOIAddress.GetLongestProperty());
                                fieldLength["Address"]          = limit;
                                fieldLength["Address_district"] = limit;
                                fieldLength["Address_street"]   = limit;
                                fieldLength["Address_number"]   = limit;
                            }
                            
                            // Retrieve the category of the POI. Expected output: StationSubway
                            // （獲取興趣點的分類。預期輸出：StationSubway）
                            if (useCategory)
                            {
                                if (singleCategory)
                                {
                                    foreach (POICategory category in CategoryRank)
                                    {
                                        if (_buildPOI.Contains(category))
                                        {
                                            _buildPOI = new List<POICategory> { category };
                                            break;
                                        }
                                    }
                                }
                                
                                string buildPOICategory = string.Join(", ", _buildPOI);
                                props["Category"] = buildPOICategory;
                                fieldLength["Category"] = MiscUtils.GetFieldLength(fieldLength, "Category", buildPOICategory);
                            }

                            // Retrieve the location of the POI. Expected output (per node): float3(-131.263f, 547.2352f, 819.4241f)
                            // （獲取興趣點的位置。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                            if (useLocation)
                            {
                                type["Location"] = GeometryType.Point;
                                edges["Location"] = new List<float3> { EntityManager.GetComponentData<Transform>(_build).m_Position };
                            }

                            if (useObject)
                            {
                                props["Object"] = "POI";
                                fieldLength["Object"] = 8;
                            }

                            poiList.Add(new CartoObject(edges, props, type));
                        }
                        else
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        m_Log.Error($"An error occured at GetPOIsProperties(); 於 GetPOIsProperties() 發生一個錯誤； {ex}");
                    }
                }
            }

            foreach (Entity _spawnLocation in _helipadQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    Entity prefab = EntityManager.GetComponentData<PrefabRef>(_spawnLocation).m_Prefab;
                    SpawnLocationData prefabData = EntityManager.GetComponentData<SpawnLocationData>(prefab);

                    if (prefabData.m_RoadTypes.HasFlag(RoadTypes.Helicopter))
                    {
                        var edges = new Dictionary<string, List<float3>>();
                        var props = new Dictionary<string, object>();
                        var type = new Dictionary<string, GeometryType>();

                        string helipadName = $"Helipad {_spawnLocation.Index}";
                        props["Name"] = helipadName;
                        fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", helipadName);

                        // Retrieve the address of the helipads. Expected output: Address(210 Elizabeth Street, Sunnyside Dale)
                        //（獲取停機坪的地址。預期輸出：Address(210 伊莉莎白街, 朝陽溪谷)）
                        if (useAddress)
                        {
                            Entity helipadOwner = _spawnLocation;

                            while (EntityManager.HasComponent<Owner>(helipadOwner))
                            {
                                helipadOwner = EntityManager.GetComponentData<Owner>(helipadOwner).m_Owner;
                            }
                            
                            Address helipadAddress = new Address(helipadOwner, EntityManager);
                            if (string.IsNullOrEmpty(helipadAddress.District)) helipadAddress.District = GetObjectCurrentDistrict(EntityManager.GetComponentData<Transform>(_spawnLocation).m_Position);
                            props["Address"] = helipadAddress;
                            int limit = MiscUtils.GetFieldLength(fieldLength, "Address", helipadAddress.GetLongestProperty());
                            fieldLength["Address"] = limit;
                            fieldLength["Address_district"] = limit;
                            fieldLength["Address_street"] = limit;
                            fieldLength["Address_number"] = limit;
                        }

                        // Retrieve the category of the helipads. Expected output: Helipad
                        // （獲取直升機停機坪的分類。預期輸出：Helipad）
                        if (useCategory)
                        {
                            props["Category"] = POICategory.Helipad.ToString();
                            fieldLength["Category"] = MiscUtils.GetFieldLength(fieldLength, "Category", POICategory.Helipad.ToString());
                        }

                        // Retrieve the location of the POI. Expected output (per node): float3(-131.263f, 547.2352f, 819.4241f)
                        // （獲取興趣點的位置。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                        if (useLocation)
                        {
                            type["Location"] = GeometryType.Point;
                            edges["Location"] = new List<float3> { EntityManager.GetComponentData<Transform>(_spawnLocation).m_Position };
                        }

                        if (useObject)
                        {
                            props["Object"] = "POI";
                            fieldLength["Object"] = 8;
                        }

                        poiList.Add(new CartoObject(edges, props, type));
                    }
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetPOIsProperties(); 於 GetPOIsProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _pillar in _pylonQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    Entity prefab = EntityManager.GetComponentData<PrefabRef>(_pillar).m_Prefab;
                    bool isPowerUtilityPillars = false;
                    POICategory category = POICategory.None;

                    if (pylonPrefabs.TryGetValue(prefab, out POICategory prefabCategory))
                    {
                        isPowerUtilityPillars = true;
                        category = prefabCategory;
                    }
                    else
                    {
                        if (EntityManager.HasComponent<UtilityObjectData>(prefab))
                        {
                            UtilityTypes utilityType = EntityManager.GetComponentData<UtilityObjectData>(prefab).m_UtilityTypes;

                            if (utilityType.HasFlag(UtilityTypes.HighVoltageLine))
                            {
                                isPowerUtilityPillars = true;
                                category = POICategory.UtilityPylon;
                                pylonPrefabs[prefab] = category;
                            }
                            else if (utilityType.HasFlag(UtilityTypes.LowVoltageLine))
                            {
                                isPowerUtilityPillars = true;
                                category = POICategory.UtilityPole;
                                pylonPrefabs[prefab] = category;
                            }
                        }
                    }

                    if (isPowerUtilityPillars)
                    {
                        var edges = new Dictionary<string, List<float3>>();
                        var props = new Dictionary<string, object>();
                        var type = new Dictionary<string, GeometryType>();
                        float3 position = default;

                        string objectType = category == POICategory.UtilityPole ? "Utility Pole" : "Utility Pylon";
                        string pillarName = $"{objectType} {_pillar.Index}";
                        props["Name"] = pillarName;
                        fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", pillarName);

                        if (useAddress || useLocation) position = EntityManager.GetComponentData<Transform>(_pillar).m_Position;

                        // Retrieve the address of the pillars. Expected output: Address(210 Elizabeth Street, Sunnyside Dale)
                        //（獲取柱子的地址。預期輸出：Address(210 伊莉莎白街, 朝陽溪谷)）
                        if (useAddress)
                        {
                            Address pillarAddress = new Address(0, string.Empty);
                            pillarAddress.District = GetObjectCurrentDistrict(position);
                            props["Address"] = pillarAddress;
                            int limit = MiscUtils.GetFieldLength(fieldLength, "Address", pillarAddress.GetLongestProperty());
                            fieldLength["Address"] = limit;
                            fieldLength["Address_district"] = limit;
                            fieldLength["Address_street"] = limit;
                            fieldLength["Address_number"] = limit;
                        }

                        // Retrieve the category of the pillars. Expected output: UtilityPylon
                        // （獲取柱子的分類。預期輸出：UtilityPylon）
                        if (useCategory)
                        {
                            props["Category"] = category.ToString();
                            fieldLength["Category"] = MiscUtils.GetFieldLength(fieldLength, "Category", category.ToString());
                        }

                        // Retrieve the location of the POI. Expected output (per node): float3(-131.263f, 547.2352f, 819.4241f)
                        // （獲取興趣點的位置。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                        if (useLocation)
                        {
                            type["Location"] = GeometryType.Point;
                            edges["Location"] = new List<float3> { position };
                        }

                        if (useObject)
                        {
                            props["Object"] = "POI";
                            fieldLength["Object"] = 8;
                        }

                        poiList.Add(new CartoObject(edges, props, type));
                    }
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetPOIsProperties(); 於 GetPOIsProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _trafficLight in _trafficLightQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    TrafficLightFlags trafficLightFlag = EntityManager.GetComponentData<TrafficLights>(_trafficLight).m_Flags;

                    if (!trafficLightFlag.HasFlag(TrafficLightFlags.LevelCrossing))
                    {
                        var edges = new Dictionary<string, List<float3>>();
                        var props = new Dictionary<string, object>();
                        var type = new Dictionary<string, GeometryType>();

                        string trafficLightName = $"Traffic Light {_trafficLight.Index}";
                        props["Name"] = trafficLightName;
                        fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", trafficLightName);

                        // Retrieve the address of the traffic lights. Expected output: Address(210 Elizabeth Street, Sunnyside Dale)
                        //（獲取交通號誌的地址。預期輸出：Address(210 伊莉莎白街, 朝陽溪谷)）
                        if (useAddress)
                        {
                            Address trafficLightAddress;
                            Entity intersectionEdgeEntity = EntityManager.GetBuffer<ConnectedEdge>(_trafficLight)[0].m_Edge;
                            Edge   intersectionEdge = EntityManager.GetComponentData<Edge>(intersectionEdgeEntity);
                            float  intersectionPosition = intersectionEdge.m_Start.Equals(_trafficLight) ? 0f : 1f;
                            
                            if (BuildingUtils.GetAddress(EntityManager, _trafficLight, intersectionEdgeEntity, intersectionPosition, out Entity road, out int number))
                            {
                                trafficLightAddress = new Address(number, m_Name.GetRenderedLabelName(road));
                            }
                            else
                            {
                                trafficLightAddress = new Address(0, string.Empty);
                            }

                            trafficLightAddress.District = GetObjectCurrentDistrict(EntityManager.GetComponentData<Game.Net.Node>(_trafficLight).m_Position);
                            props["Address"] = trafficLightAddress;
                            int limit = MiscUtils.GetFieldLength(fieldLength, "Address", trafficLightAddress.GetLongestProperty());
                            fieldLength["Address"] = limit;
                            fieldLength["Address_district"] = limit;
                            fieldLength["Address_street"] = limit;
                            fieldLength["Address_number"] = limit;
                        }

                        // Retrieve the category of the traffic lights. Expected output: TrafficLight
                        // （獲取交通號誌的分類。預期輸出：TrafficLight）
                        if (useCategory)
                        {
                            props["Category"] = POICategory.TrafficLight.ToString();
                            fieldLength["Category"] = MiscUtils.GetFieldLength(fieldLength, "Category", POICategory.TrafficLight.ToString());
                        }

                        // Retrieve the location of the POI. Expected output (per node): float3(-131.263f, 547.2352f, 819.4241f)
                        // （獲取興趣點的位置。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                        if (useLocation)
                        {
                            type["Location"] = GeometryType.Point;
                            edges["Location"] = new List<float3> { EntityManager.GetComponentData<Game.Net.Node>(_trafficLight).m_Position };
                        }

                        if (useObject)
                        {
                            props["Object"] = "POI";
                            fieldLength["Object"] = 8;
                        }

                        poiList.Add(new CartoObject(edges, props, type));
                    }
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetPOIsProperties(); 於 GetPOIsProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _transportStop in _transportStopQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    StopFlags stopFlag = EntityManager.GetComponentData<Game.Routes.TransportStop>(_transportStop).m_Flags;

                    if (stopFlag.HasFlag(StopFlags.Active))
                    {
                        var edges = new Dictionary<string, List<float3>>();
                        var props = new Dictionary<string, object>();
                        var type = new Dictionary<string, GeometryType>();

                        string transportStopName = m_Name.GetRenderedLabelName(_transportStop);
                        props["Name"] = transportStopName;
                        fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", transportStopName);

                        // Retrieve the address of the transportation stops. Expected output: Address(210 Elizabeth Street, Sunnyside Dale)
                        //（獲取大眾運輸車站的地址。預期輸出：Address(210 伊莉莎白街, 朝陽溪谷)）
                        if (useAddress)
                        {
                            Entity transportStopOwner = _transportStop;

                            while (EntityManager.HasComponent<Owner>(transportStopOwner))
                            {
                                transportStopOwner = EntityManager.GetComponentData<Owner>(transportStopOwner).m_Owner;
                            }

                            Address transportStopAddress = new Address(transportStopOwner, EntityManager);
                            props["Address"] = transportStopAddress;
                            if (string.IsNullOrEmpty(transportStopAddress.District)) transportStopAddress.District = GetObjectCurrentDistrict(EntityManager.GetComponentData<Transform>(_transportStop).m_Position);
                            int limit = MiscUtils.GetFieldLength(fieldLength, "Address", transportStopAddress.GetLongestProperty());
                            fieldLength["Address"] = limit;
                            fieldLength["Address_district"] = limit;
                            fieldLength["Address_street"] = limit;
                            fieldLength["Address_number"] = limit;
                        }

                        // Retrieve the category of the transport stops. Expected output: StopBus
                        // （獲取大眾運輸車站的分類。預期輸出：StopBus）
                        if (useCategory)
                        {
                            List<POICategory> categories = new List<POICategory>();
                            Entity prefab = EntityManager.GetComponentData<PrefabRef>(_transportStop).m_Prefab;

                            if (transportStopPrefabs.TryGetValue(prefab, out List<POICategory> category))
                            {
                                categories = category;
                            }
                            else
                            {
                                TransportStopData stopData = EntityManager.GetComponentData<TransportStopData>(prefab);
                                bool cargo = stopData.m_CargoTransport;
                                bool passenger = stopData.m_PassengerTransport;
                                TransportType transport = stopData.m_TransportType;

                                switch (transport)
                                {
                                    case TransportType.Airplane:
                                        if (passenger) categories.Add(POICategory.StopPassengerAirplane);
                                        if (singleCategory && passenger) break;
                                        if (cargo) categories.Add(POICategory.StopCargoAirplane);
                                        break;

                                    case TransportType.Bus:
                                        categories.Add(POICategory.StopBus);
                                        break;

                                    case TransportType.Helicopter:
                                        categories.Add(POICategory.StopHelicopter);
                                        break;

                                    case TransportType.Post:
                                        categories.Add(POICategory.PostBox);
                                        break;

                                    case TransportType.Rocket:
                                        categories.Add(POICategory.SpaceCenter);
                                        break;

                                    case TransportType.Ship:
                                        if (passenger) categories.Add(POICategory.StopPassengerShip);
                                        if (singleCategory && passenger) break;
                                        if (cargo) categories.Add(POICategory.StopCargoShip);
                                        break;

                                    case TransportType.Subway:
                                        categories.Add(POICategory.StopSubway);
                                        break;

                                    case TransportType.Taxi:
                                        categories.Add(POICategory.StopTaxi);
                                        break;

                                    case TransportType.Train:
                                        if (passenger) categories.Add(POICategory.StopPassengerTrain);
                                        if (singleCategory && passenger) break;
                                        if (cargo) categories.Add(POICategory.StopCargoTrain);
                                        break;

                                    case TransportType.Tram:
                                        categories.Add(POICategory.StopTram);
                                        break;

                                    default:
                                        categories.Add(POICategory.TransportationGeneric);
                                        break;
                                }
                            }
                            
                            string stopPOICategory = string.Join(", ", categories);
                            props["Category"] = stopPOICategory;
                            fieldLength["Category"] = MiscUtils.GetFieldLength(fieldLength, "Category", stopPOICategory);
                        }

                        // Retrieve the location of the POI. Expected output (per node): float3(-131.263f, 547.2352f, 819.4241f)
                        // （獲取興趣點的位置。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                        if (useLocation)
                        {
                            type["Location"] = GeometryType.Point;
                            edges["Location"] = new List<float3> { EntityManager.GetComponentData<Transform>(_transportStop).m_Position };
                        }

                        if (useObject)
                        {
                            props["Object"] = "POI";
                            fieldLength["Object"] = 8;
                        }

                        poiList.Add(new CartoObject(edges, props, type));
                    }
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetPOIsProperties(); 於 GetPOIsProperties() 發生一個錯誤； {ex}");
                }
            }

            return poiList;
        }
    }
}