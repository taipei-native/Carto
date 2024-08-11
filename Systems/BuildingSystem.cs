namespace Carto.Systems
{
    using Carto;
    using Carto.Domain;
    using Carto.Utils;
    using Colossal.Logging;
    using Colossal.Mathematics;
    using Game;
    using Game.Buildings;
    using Game.Citizens;
    using Game.Common;
    using Game.Companies;
    using Game.Economy;
    using Game.Objects;
    using Game.Prefabs;
    using Game.Tools;
    using Game.UI;
    using GeometryType = Utils.ExportUtils.GeometryType;
    using Purpose = Colossal.Serialization.Entities.Purpose;
    using System;
    using System.Linq;
    using Unity.Collections;
    using System.Collections.Generic;
    using Unity.Entities;
    using Unity.Mathematics;
    using ZoningCategory = ZoningSystem.ZoningCategory;

    /// <summary>
    /// The system instance that manages the building properties, inheriting from GameSystemBase.
    /// （管理建築屬性的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class BuildingSystem : GameSystemBase
    {
        private static readonly ILog m_Log = Instance.Log;
        private static readonly NameSystem m_Name = Instance.Name;
        private static readonly PrefabSystem m_Prefab = Instance.Prefab;

        // The entity query instance that searches for several instances, using Unity.Entities.EntityQuery.
        // （用於搜尋數種實例的Unity實體查詢實例。）
        private static EntityQuery _buildingQuery;

        /// <summary>
        /// The enum storing building categories.
        /// （儲存建築分類的列舉。）
        /// </summary>
        [Flags]
        public enum BuildingCategory
        {
            // Building Status（建築狀態）
            /// <summary>
            /// The building without special classification.（沒有特殊分類的建築。）
            /// </summary>
            None = 0,

            /// <summary>
            /// The building upgrades that require main buildings.（需要主建築的建築升級。）
            /// </summary>
            Extension      = 1 <<  0,

            /// <summary>
            /// The building under construction.（正在建造中的建築。）
            /// </summary>
            Construction   = 1 <<  1,

            /// <summary>
            /// Public service buildings.（公共服務建築。）
            /// </summary>
            Public         = 1 <<  2,

            // Private / Decorative Buildings（私有／裝飾性建築）
            /// <summary>
            /// The decorative buildings.（裝飾性的建築。）
            /// </summary>
            Decoration     = 1 <<  3,

            /// <summary>
            /// The buildings that automatically spawn on special industries.（自動生成在特化工業上的建築。）
            /// </summary>
            Extractor      = 1 <<  4,

            /// <summary>
            /// The private buildings that spawn on the dedicated zonings.（生成在特定分區的私人建築。）
            /// </summary>
            Property       = 1 <<  5,

            // Public / Service Buildings（公共／服務建築）

            Admin          = 1 <<  6,
            Communication  = 1 <<  7,
            Disaster       = 1 <<  8,
            Education      = 1 <<  9,
            Fire           = 1 << 10,
            Health         = 1 << 11,
            Maintenance    = 1 << 12,
            Mortuary       = 1 << 13,
            Park           = 1 << 14,
            Parking        = 1 << 15,
            Police         = 1 << 16,
            Post           = 1 << 17,
            Power          = 1 << 18,
            Research       = 1 << 19,
            Sewage         = 1 << 20,
            Transportation = 1 << 21,
            Waste          = 1 << 22,
            Water          = 1 << 23
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
                    
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            
            base.OnCreate();
            m_Log.Debug("BuildingSystem instance created. 建築系統實例創造完成。");
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
        /// Retrieve the category of the building.
        /// （獲取建築的分類。）
        /// </summary>
        /// <param name="building">The building entity.（建築實體。）</param>
        public static BuildingCategory GetBuildingCategory(Entity building, EntityManager entityManager)
        {
            BuildingCategory category = BuildingCategory.None;

            if (entityManager.HasComponent<UnderConstruction>(building))
            {
                category |= BuildingCategory.Construction;
            }
            if (entityManager.HasComponent<Game.Buildings.ServiceUpgrade>(building))
            {
                category |= BuildingCategory.Extension;
            }
            if (entityManager.HasComponent<Game.Buildings.ExtractorFacility>(building))
            {
                category |= BuildingCategory.Extractor;
            }
            if (entityManager.HasComponent<CommercialProperty>(building) || entityManager.HasComponent<IndustrialProperty>(building) || entityManager.HasComponent<ResidentialProperty>(building))
            {
                category |= BuildingCategory.Property;
            }
            if (entityManager.HasComponent<AdminBuilding>(building) || entityManager.HasComponent<Game.Buildings.WelfareOffice>(building))
            {
                category |= BuildingCategory.Admin;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.TelecomFacility>(building))
            {
                category |= BuildingCategory.Communication;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.DisasterFacility>(building) || entityManager.HasComponent<Game.Buildings.EmergencyShelter>(building))
            {
                category |= BuildingCategory.Disaster;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.School>(building))
            {
                category |= BuildingCategory.Education;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.FireStation>(building) || entityManager.HasComponent<Game.Buildings.FirewatchTower>(building))
            {
                category |= BuildingCategory.Fire;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.Hospital>(building))
            {
                category |= BuildingCategory.Health;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.MaintenanceDepot>(building))
            {
                category |= BuildingCategory.Maintenance;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.DeathcareFacility>(building))
            {
                category |= BuildingCategory.Mortuary;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.Park>(building))
            {
                category |= BuildingCategory.Park;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.ParkingFacility>(building))
            {
                category |= BuildingCategory.Parking;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.PoliceStation>(building) || entityManager.HasComponent<Game.Buildings.Prison>(building))
            {
                category |= BuildingCategory.Police;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.PostFacility>(building))
            {
                category |= BuildingCategory.Post;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.Battery>(building) || entityManager.HasComponent<ElectricityProducer>(building) || entityManager.HasComponent<Game.Buildings.Transformer>(building))
            {
                category |= BuildingCategory.Power;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.ResearchFacility>(building))
            {
                category |= BuildingCategory.Research;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.SewageOutlet>(building))
            {
                category |= BuildingCategory.Sewage;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.TransportDepot>(building) || entityManager.HasComponent<Game.Buildings.TransportStation>(building))
            {
                category |= BuildingCategory.Transportation;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.GarbageFacility>(building))
            {
                category |= BuildingCategory.Waste;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Game.Buildings.WaterPumpingStation>(building))
            {
                category |= BuildingCategory.Water;
                category |= BuildingCategory.Public;
            }
            if (entityManager.HasComponent<Native>(building) && category == BuildingCategory.None)
            {
                category |= BuildingCategory.Decoration;
            }

            return category;
        }

        /// <summary>
        /// Retrieve the edge of the building.
        /// （獲取建築的邊緣。）
        /// </summary>
        /// <param name="buildingEntity">The building entity.（建築實體。）</param>
        /// <param name="entityManager">The system's entity manager.（系統的實體管理器。）</param>
        public static List<float3> GetBuildingEdge(Entity buildingEntity, EntityManager entityManager)
        {
            Entity buildingPrefab = entityManager.GetComponentData<PrefabRef>(buildingEntity);
            bool circular = m_Prefab.GetPrefab<BuildingPrefab>(buildingPrefab).m_Circular;
            int2 lotSize = entityManager.GetComponentData<BuildingData>(buildingPrefab).m_LotSize;
            Transform transform = entityManager.GetComponentData<Transform>(buildingEntity);

            if (circular)
            {
                return GeometryUtils.Interpolate(new Circle3(math.min(lotSize.x, lotSize.y) * 4, transform.m_Position, new quaternion(0, 0, 0, 0)));
            }
            else
            {
                Quad3 corners = BuildingUtils.CalculateCorners(transform, lotSize);
                return new List<float3> { corners.a, corners.b, corners.c, corners.d };
            }
        }

        /// <summary>
        /// Searching for properties of all existing buildings.
        /// （搜尋現有所有建築的屬性。）
        /// </summary>
        public List<CartoObject> GetBuildingsProperties(out Dictionary<string, int> fieldLength)
        {
            List<CartoObject> buildList = new List<CartoObject>();
            Dictionary<ushort, ZoningType> zoningTypes;
            Dictionary<Entity, ZoningCategory?> zoningCategories;
            Dictionary<Entity, string> zoningThemes;
            fieldLength = new Dictionary<string, int>();
            bool useAddress = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Address");
            bool useAsset = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Asset");
            bool useBrand = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Brand");
            bool useCategory = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Category");
            bool useEdge = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Edge");
            bool useEmployee = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Employee");
            bool useHousehold = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Household");
            bool useLevel = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Level");
            bool useObject = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Object");
            bool useProduct = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Product");
            bool useResident = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Resident");
            bool useTheme = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Theme");
            bool useZoning = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Building, "Zoning");

            if (true)
            {
                zoningTypes = Instance.Zoning.GetZoningTypes(false, false, useTheme, useZoning);
                zoningCategories = zoningTypes.ToDictionary(kvp => kvp.Value.Entity, kvp => kvp.Value.Category);
                zoningThemes = zoningTypes.ToDictionary(kvp => kvp.Value.Entity, kvp => kvp.Value.Theme);
            }

            foreach (Entity _build in _buildingQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, GeometryType>();

                    Entity buildPrefab = EntityManager.GetComponentData<PrefabRef>(_build).m_Prefab;
                    SimpleStatsObject buildStats = GetBuildingSimpleStats(_build, useBrand, false, useEmployee, useHousehold, useProduct, useResident, EntityManager, out bool hasShop);

                    // Retrieve the name of the building.
                    // （獲取建築的名稱。）
                    string buildName = m_Name.GetRenderedLabelName(_build);
                    props["Name"] = buildName;
                    fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", buildName);

                    // Retrieve the address of the building. Expected output: Address(210 Elizabeth Street, Sunnyside Dale)
                    //（獲取建築的地址。預期輸出：Address(210 伊莉莎白街, 朝陽溪谷)）
                    if (useAddress)
                    {
                        Address buildAddress = new Address(_build, EntityManager);
                        props["Address"] = buildAddress;
                        int limit = MiscUtils.GetFieldLength(fieldLength, "Address", buildAddress.GetLongestProperty());
                        fieldLength["Address"] = limit;
                        fieldLength["Address.district"] = limit;
                        fieldLength["Address.street"]   = limit;
                        fieldLength["Address.number"]   = limit;
                    }

                    // Retrieve the asset name of the building. Expected output: EU Mixed 01 - L1 2x2
                    // （獲取建築的資產名稱。預期輸出：歐式混合 01 - L1 2x2）
                    if (useAsset)
                    {
                        string buildAsset = LocaleUtils.TryTranslate($"Assets.NAME[{m_Prefab.GetPrefabName(buildPrefab)}]", out string translated) ? translated : m_Prefab.GetPrefabName(buildPrefab);
                        props["Asset"] = buildAsset;
                        fieldLength["Asset"] = MiscUtils.GetFieldLength(fieldLength, "Asset", buildAsset);
                    }

                    // Retrieve the name of the renter's brand. Expected output: Chachi Threads
                    // （獲取租客的品牌名稱。預期輸出：Chachi Threads）
                    if (useBrand)
                    {
                        string buildBrand = hasShop ? buildStats.Brands[0] : string.Empty;
                        props["Brand"] = buildBrand;
                        fieldLength["Brand"] = MiscUtils.GetFieldLength(fieldLength, "Brand", buildBrand);
                    }

                    // Retrieve the category of the building. Expected output: Property
                    // （獲取建築的分類。預期輸出：Property）
                    if (useCategory)
                    {
                        string buildCategory = GetBuildingCategory(_build, EntityManager).ToString();
                        props["Category"] = buildCategory;
                        fieldLength["Category"] = MiscUtils.GetFieldLength(fieldLength, "Category", buildCategory);
                    }

                    // Retrieve the (incollidable) edge of the building. Expected output (per node): float3(-131.263f, 547.2352f, 819.4241f)
                    // （獲取建築物的（不可碰撞）邊緣。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                    if (useEdge)
                    {
                        type["Edge"]  = GeometryType.Polygon;
                        edges["Edge"] = GetBuildingEdge(_build, EntityManager);
                    }

                    // Retrieve the amount of the employees in the building. Expected output: 12
                    // （獲取建築內的員工人數。預期輸出：12）
                    if (useEmployee) props["Employee"] = buildStats.Employee;

                    // Retrieve the amount of the households in the building. Expected output: 30
                    // （獲取建築內的家庭數量。預期輸出：30）
                    if (useHousehold) props["Household"] = buildStats.Household;

                    // Retrieve the level of the building. Expected output: 3
                    // （獲取建築的等級。預期輸出：3）
                    if (useLevel)
                    {
                        props["Level"] = (short)(EntityManager.HasComponent<SpawnableBuildingData>(buildPrefab) ? EntityManager.GetComponentData<SpawnableBuildingData>(buildPrefab).m_Level : 0);
                        fieldLength["Level"] = 1;
                    }

                    // Retrieve the product manufactured in the building. Expected output: Textiles
                    // （獲取建築內生產的商品。預期輸出：紡織品）
                    if (useProduct)
                    {
                        string buildProduct = hasShop ? buildStats.Products[0] : string.Empty;
                        props["Product"] = buildProduct;
                        fieldLength["Product"] = MiscUtils.GetFieldLength(fieldLength, "Product", buildProduct);
                    }

                    // Retrieve the amount of residents in the building. Expected output: 48
                    // （獲取建築內的居民人數。預期輸出：48）
                    if (useResident) props["Resident"] = buildStats.Resident;

                    if (useTheme || useZoning)
                    {
                        if (EntityManager.HasComponent<SpawnableBuildingData>(buildPrefab))
                        {
                            Entity zone = EntityManager.GetComponentData<SpawnableBuildingData>(buildPrefab).m_ZonePrefab;

                            // Retrieve the theme of the building. Expected output: European
                            // （獲取建築的主題風格。預期輸出：歐式）
                            if (useTheme)
                            {
                                props["Theme"] = zoningThemes[zone];
                                fieldLength["Theme"] = MiscUtils.GetFieldLength(fieldLength, "Theme", zoningThemes[zone]);
                            }

                            // Retrieve the zoning purposes of the building. Expected output: Residential, Commercial
                            // （獲取建築的分區用途。預期輸出：Residential, Commercial）
                            if (useZoning)
                            {
                                props["Zoning"] = zoningCategories[zone].ToString();
                                fieldLength["Zoning"] = MiscUtils.GetFieldLength(fieldLength, "Zoning", zoningCategories[zone].ToString());
                            }
                        }
                        else
                        {
                            if (useTheme)
                            {
                                props["Theme"] = LocaleUtils.Translate("Assets.THEME[Carto Generic]");
                                fieldLength["Theme"] = MiscUtils.GetFieldLength(fieldLength, "Theme", LocaleUtils.Translate("Assets.THEME[Carto Generic]"));
                            }

                            if (useZoning)
                            {
                                props["Zoning"] = ZoningCategory.None.ToString();
                                fieldLength["Zoning"] = MiscUtils.GetFieldLength(fieldLength, "Zoning", ZoningCategory.None.ToString());
                            }
                        }
                    }

                    if (useObject)
                    {
                        props["Object"] = "Building";
                        fieldLength["Object"] = 13;
                    }

                    buildList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetBuildingsProperties(); 於 GetBuildingsProperties() 發生一個錯誤； {ex}");
                }
            }

            return buildList;
        }

        /// <summary>
        /// Retrieve the simple statistics of the building.
        /// （獲取建築的簡單統計資料。）
        /// </summary>
        /// <param name="buildingEntity">The building entity.（建築實體。）</param>
        /// <param name="useBrand">Whether to export the brands.（是否輸出品牌。）</param>
        /// <param name="useEmployee">Whether to export the number of employees.（是否輸出員工數量。）</param>
        /// <param name="useHousehold">Whether to export the number of households.（是否輸出家庭數量。）</param>
        /// <param name="useProduct">Whether to export the products.（是否輸出產品。）</param>
        /// <param name="useResident">Whether to export the number of residents.（是否輸出居民數量。）</param>
        /// <param name="entityManager">The systems' entity manager.（系統的實體管理器。）</param>
        /// <param name="hasShop">Whether there are shops in the building.（建築內是否有商店。）</param>
        public static SimpleStatsObject GetBuildingSimpleStats(Entity buildingEntity, bool useBrand, bool useCompany, bool useEmployee, bool useHousehold, bool useProduct, bool useResident, EntityManager entityManager, out bool hasShop)
        {
            if (!entityManager.HasComponent<Building>(buildingEntity)) throw new ArgumentException($"Only accepts entities with the component `Game.Buildings.Building`.");

            hasShop = false;
            bool isHomeless = !entityManager.HasComponent<ResidentialProperty>(buildingEntity);
            SimpleStatsObject stats = new SimpleStatsObject();
            if (!(useBrand || useCompany || useEmployee || useHousehold || useProduct || useResident)) return stats;

            if (entityManager.HasBuffer<Employee>(buildingEntity) && useEmployee) stats.Employee += entityManager.GetBuffer<Employee>(buildingEntity).Length;
            if (entityManager.HasBuffer<Renter>(buildingEntity))
            {
                foreach(Renter renter in entityManager.GetBuffer<Renter>(buildingEntity))
                {
                    Entity renterEntity = renter.m_Renter;

                    if (entityManager.HasComponent<CompanyData>(renterEntity) && (useBrand || useCompany || useProduct))
                    {
                        hasShop = true;
                        
                        if (useBrand)
                        {
                            Entity brandEntity = entityManager.GetComponentData<CompanyData>(renterEntity).m_Brand;
                            stats.Brands.Add(m_Name.GetRenderedLabelName(brandEntity));
                        }

                        if (useCompany) stats.Company ++;

                        if (useProduct)
                        {
                            Entity facilityPrefab = entityManager.GetComponentData<PrefabRef>(renterEntity).m_Prefab;
                            Resource product = entityManager.GetComponentData<IndustrialProcessData>(facilityPrefab).m_Output.m_Resource;
                            string[] productArray = MiscUtils.GetFlagComponents(product).Select(productType => LocaleUtils.Translate($"Resources.TITLE[{productType}]")).ToArray();
                            stats.Products.Add(string.Join(", ", productArray));
                        }
                    }

                    if (entityManager.HasBuffer<Employee>(renterEntity) && useEmployee) stats.Employee += entityManager.GetBuffer<Employee>(renterEntity).Length;

                    if (entityManager.HasComponent<Household>(renterEntity) && useHousehold)
                    {
                        if (!isHomeless || isHomeless && Instance.Settings.UseHomeless) stats.Household++;
                    }

                    if (entityManager.HasBuffer<HouseholdCitizen>(renterEntity) && useResident)
                    {
                        if (!isHomeless || isHomeless && Instance.Settings.UseHomeless)
                        {
                            foreach (HouseholdCitizen citizen in entityManager.GetBuffer<HouseholdCitizen>(renterEntity))
                            {
                                if (!CitizenUtils.IsCorpsePickedByHearse(entityManager, citizen.m_Citizen)) stats.Resident++;
                            }
                        }
                    }
                }
            }

            return stats;
        }
    }
}