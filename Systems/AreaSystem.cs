namespace Carto.Systems
{
    using Carto.Domain;
    using Carto.Utils;
    using Colossal.Logging;
    using Game;
    using Game.Areas;
    using Game.Common;
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
        private static EntityQuery _districtQuery;
        private static EntityQuery _mapTileQuery;

        /// <summary>
        /// This event triggers when the system is created.
        /// （這是當系統實例被創造時，會被觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
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
        public List<CartoObject> GetDistrictsProperties()
        {   
            List<CartoObject> distList = new List<CartoObject>();

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

                    // Retrieve the area of the district. Expected output: 447895
                    // （獲取行政區的面積。預期輸出：447895）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Area")) props["Area"] = EntityManager.GetComponentData<Geometry>(_district).m_SurfaceArea;

                    // Retrieve the center position of the district. Expected output: float3(-131.263f, 547.2352f, 819.4241f)
                    // （獲取行政區的中心點。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Center")) props["Center"] = EntityManager.GetComponentData<Geometry>(_district).m_CenterPosition;

                    // Retrieve the nodes forming the boundary of the district. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成行政區邊界的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Edge"))
                    {
                        List<float3> distNodes = new List<float3>();
                        foreach (Node _node in EntityManager.GetBuffer<Node>(_district)) distNodes.Add(_node.m_Position);
                        edges["Edge"] = distNodes;
                        type["Edge"] = GeometryType.Polygon;
                    }

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Object")) props["Object"] = "District";

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
        public List<CartoObject> GetMapTilesProperties()
        {
            List<CartoObject> tileList = new List<CartoObject>();

            foreach (Entity _tile in _mapTileQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, GeometryType>();

                    // Retrieve the debug name for the map tile. Expected output: "Map Tile 45543"
                    // （獲取地圖區塊的自訂名稱。預期輸出："Map Tile 45543"）
                    string tileName = m_Name.GetDebugName(_tile);
                    props["Name"] = tileName;

                    // Retrieve the area of the map tile. Expected output: 447895
                    // （獲取地圖區塊的面積。預期輸出：447895）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Area")) props["Area"] = EntityManager.GetComponentData<Geometry>(_tile).m_SurfaceArea;

                    // Retrieve the center position of the map tile. Expected output: float3(-131.263f, 547.2352f, 819.4241f)
                    // （獲取地圖區塊的中心點。預期輸出：float3(-131.263f, 547.2352f, 819.4241f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Center")) props["Center"] = EntityManager.GetComponentData<Geometry>(_tile).m_CenterPosition;

                    // Retrieve the nodes forming the boundary of the map tile. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成地圖區塊邊界的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Edge"))
                    {
                        List<float3> tileNodes = new List<float3>();
                        foreach (Node _node in EntityManager.GetBuffer<Node>(_tile)) tileNodes.Add(_node.m_Position);
                        edges["Edge"] = tileNodes;
                        type["Edge"] = GeometryType.Polygon;
                    }

                    // Retrieve the unlocked status of the map tile. Expected output: true
                    // （獲取地圖區塊的解鎖狀態。預期輸出：True）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Unlocked")) props["Unlocked"] = !EntityManager.HasComponent<Native>(_tile);

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Area, "Object")) props["Object"] = "Map Tile";

                    tileList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetMapTilesProperties(); 於 GetMapTilesProperties() 發生一個錯誤； {ex}");
                }
            }

            return tileList;
        }
    }
}