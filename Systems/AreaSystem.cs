namespace Carto.Systems
{
    using Carto.Domain;
    using Carto.Utils;
    using Colossal.Logging;
    using Game;
    using Game.Areas;
    using Game.Buildings;
    using Game.Common;
    using Game.Objects;
    using Game.Tools;
    using Game.UI;
    using GeometryType = Utils.ExportUtils.GeometryType;
    using Purpose = Colossal.Serialization.Entities.Purpose;
    using System;
    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;

    /// <summary>
    /// The system instance that manages the area properties, inheriting from GameSystemBase.
    /// （管理區域屬性的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class AreaSystem : GameSystemBase
    {
        private static readonly ILog m_Log = Instance.Log;
        private static readonly NameSystem m_Name = Instance.Name;

        // The entity query instance that searches for several instances, using Unity.Entities.EntityQuery.
        // （用於搜尋數種實例的Unity實體查詢實例。）
        private static EntityQuery _buildingQuery;
        private static EntityQuery _districtQuery;
        private static EntityQuery _mapTileQuery;

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
                    ComponentType.ReadOnly<Building>(),
                    ComponentType.ReadOnly<CurrentDistrict>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Placeholder>(),
                    
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

            _mapTileQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<MapTile>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            base.OnCreate();
            m_Log.Debug("AreaSystem instance created. 區域系統實例創造完成。");
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
        protected override void OnUpdate() {}

        /// <summary>
        /// This event triggers when the system is destroyed.
        /// （這是當系統實例被銷毀時，會被觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// Searching for properties of all existing districts.
        /// （搜尋現有所有行政區的屬性。）
        /// </summary>
        public List<CartoObject> GetDistrictsProperties(out Dictionary<string, int> fieldLength)
        {   
            Dictionary<Entity, SimpleStatsObject> distStats = new Dictionary<Entity, SimpleStatsObject>();
            List<CartoObject> distList = new List<CartoObject>();
            fieldLength = new Dictionary<string, int>();
            bool useArea = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Area");
            bool useCenter = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Center");
            bool useCompany = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Company");
            bool useEdge = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Edge");
            bool useEmployee = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Employee");
            bool useHousehold = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Household");
            bool useObject = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Object");
            bool useResident = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Resident");

            if (useCompany || useEmployee || useHousehold || useResident) distStats = GetDistrictSimpleStats(useCompany, useEmployee, useHousehold, useResident, EntityManager);

            foreach (Entity _district in _districtQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, GeometryType>();

                    // Retrieve the custom name for the district. Expected output: "Apple Hill"
                    // （獲取行政區的自訂名稱。預期輸出："蘋果高地"）
                    string distName = m_Name.GetRenderedLabelName(_district);
                    props["Name"] = distName;
                    fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", distName);

                    // Retrieve the area of the district. Expected output: 447895
                    // （獲取行政區的面積。預期輸出：447895）
                    if (useArea) props["Area"] = EntityManager.GetComponentData<Geometry>(_district).m_SurfaceArea;

                    // Retrieve the number of companies in the district. Expected output: 35
                    // （獲得行政區內的公司數量。預期輸出：35）
                    if (useCompany) props["Company"] = distStats.TryGetValue(_district, out SimpleStatsObject sso) ? sso.Company : 0;

                    // Retrieve the center position of the district. Expected output: float3(-131.263f, 547.2352f, 819.4241f)
                    // （獲取行政區的中心點。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                    if (useCenter) props["Center"] = EntityManager.GetComponentData<Geometry>(_district).m_CenterPosition;

                    // Retrieve the nodes forming the boundary of the district. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成行政區邊界的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (useEdge)
                    {
                        List<float3> distNodes = new List<float3>();
                        foreach (Node _node in EntityManager.GetBuffer<Node>(_district)) distNodes.Add(_node.m_Position);
                        edges["Edge"] = distNodes;
                        type["Edge"] = GeometryType.Polygon;
                    }

                    // Retrieve the number of employees in the district. Expected output: 1042
                    // （獲得行政區內的員工數量。預期輸出：1042）
                    if (useEmployee) props["Employee"] = distStats.TryGetValue(_district, out SimpleStatsObject sso) ? sso.Employee : 0;

                    // Retrieve the number of households in the district. Expected output: 254
                    // （獲得行政區內的家庭數量。預期輸出：254）
                    if (useHousehold) props["Household"] = distStats.TryGetValue(_district, out SimpleStatsObject sso) ? sso.Household : 0;

                    // Retrieve the number of residents in the district. Expected output: 470
                    // （獲得行政區內的居民數量。預期輸出：470）
                    if (useResident) props["Resident"] = distStats.TryGetValue(_district, out SimpleStatsObject sso) ? sso.Resident : 0;

                    if (useObject)
                    {
                        props["Object"] = "District";
                        fieldLength["Object"] = 13;
                    }

                    distList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetDistrictsProperties(); 於 GetDistrictsProperties() 發生一個錯誤； {ex}");
                }
            }

            return distList;
        }

        /// <summary>
        /// Searching for properties of all existing map tiles.
        /// （搜尋現有所有地圖區塊的屬性。） 
        /// </summary>
        public List<CartoObject> GetMapTilesProperties(out Dictionary<string, int> fieldLength)
        {
            List<CartoObject> tileList = new List<CartoObject>();
            fieldLength = new Dictionary<string, int>();

            foreach (Entity _tile in _mapTileQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, GeometryType>();
                    bool useArea = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Area");
                    bool useCenter = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Center");
                    bool useEdge = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Edge");
                    bool useObject = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Object");
                    bool useUnlocked = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Unlocked");

                    // Retrieve the debug name for the map tile. Expected output: "Map Tile 45543"
                    // （獲取地圖區塊的自訂名稱。預期輸出："Map Tile 45543"）
                    string tileName = m_Name.GetDebugName(_tile);
                    props["Name"] = tileName;
                    fieldLength["Name"] = MiscUtils.GetFieldLength(fieldLength, "Name", tileName);

                    // Retrieve the area of the map tile. Expected output: 447895
                    // （獲取地圖區塊的面積。預期輸出：447895）
                    if (useArea) props["Area"] = EntityManager.GetComponentData<Geometry>(_tile).m_SurfaceArea;

                    // Retrieve the center position of the map tile. Expected output: float3(-131.263f, 547.2352f, 819.4241f)
                    // （獲取地圖區塊的中心點。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                    if (useCenter) props["Center"] = EntityManager.GetComponentData<Geometry>(_tile).m_CenterPosition;

                    // Retrieve the nodes forming the boundary of the map tile. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成地圖區塊邊界的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (useEdge)
                    {
                        List<float3> tileNodes = new List<float3>();
                        foreach (Node _node in EntityManager.GetBuffer<Node>(_tile)) tileNodes.Add(_node.m_Position);
                        edges["Edge"] = tileNodes;
                        type["Edge"] = GeometryType.Polygon;
                    }

                    // Retrieve the unlocked status of the map tile. Expected output: true
                    // （獲取地圖區塊的解鎖狀態。預期輸出：True）
                    if (useUnlocked) props["Unlocked"] = !EntityManager.HasComponent<Native>(_tile);

                    if (useObject)
                    {
                        props["Object"] = "Map Tile";
                        fieldLength["Object"] = 13;
                    }

                    tileList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetMapTilesProperties(); 於 GetMapTilesProperties() 發生一個錯誤； {ex}");
                }
            }

            return tileList;
        }

        /// <summary>
        /// Retrieve the simple statistics of the district.
        /// （獲取行政區的簡單統計資料。）
        /// </summary>
        /// <param name="useCompany">Whether to export the number of companies.（是否輸出公司數量。）</param>
        /// <param name="useEmployee">Whether to export the number of employees.（是否輸出員工數量。）</param>
        /// <param name="useHousehold">Whether to export the number of households.（是否輸出家庭數量。）</param>
        /// <param name="useResident">Whether to export the number of residents.（是否輸出居民數量。）</param>
        /// <param name="entityManager">The systems' entity manager.（系統的實體管理器。）</param>
        public static Dictionary<Entity, SimpleStatsObject> GetDistrictSimpleStats(bool useCompany, bool useEmployee, bool useHousehold, bool useResident, EntityManager entityManager)
        {
            Dictionary<Entity, SimpleStatsObject> result = new Dictionary<Entity, SimpleStatsObject>();

            foreach (Entity building in _buildingQuery.ToEntityArray(Allocator.Temp))
            {
                Entity district = entityManager.GetComponentData<CurrentDistrict>(building).m_District;
                SimpleStatsObject sso = BuildingSystem.GetBuildingSimpleStats(building, false, useCompany, useEmployee, useHousehold, false, useResident, entityManager, out _);
                result[district] = result.TryGetValue(district, out SimpleStatsObject oldSso) ? oldSso + sso : sso;
            }

            return result;
        }
    }
}