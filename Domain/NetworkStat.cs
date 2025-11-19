using Carto.IO;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Carto.Domain
{
    /// <summary>
    /// The container of network statistics.
    /// （網路統計資料的容器。）
    /// </summary>
    public struct NetworkStat : IStat
    {
        /// <summary>
        /// See <see cref="entity"/>.（詳見 <see cref="entity"/>。）
        /// </summary>
        public Entity Entity { readonly get => entity; set => entity = value; }

        /// <summary>
        /// The asset prefab entity.
        /// （資產預製模板實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The aggregation entity.
        /// （聚合實體。）
        /// </summary>
        public Entity aggregation;

        /// <summary>
        /// The bike traffic volume of the network.
        /// （網路的自行車流量。）
        /// </summary>
        public float bike;

        /// <summary>
        /// The maximum amount of subtance that the utility pipes can hold.
        /// （容量，管線所能乘載的物質數量上限。）
        /// </summary>
        public float capacity;

        /// <summary>
        /// The category of the network.
        /// （網路的分類。）
        /// </summary>
        public NetworkCategory category;

        /// <summary>
        /// The direction of the network.
        /// （網路的方向。）
        /// </summary>
        public Direction direction;

        /// <summary>
        /// The amount of substance transported in the pipe.
        /// （管線送的物質數量。）
        /// </summary>
        public float discharge;

        /// <summary>
        /// The average elevation of the network.
        /// （網路的平均高程。）
        /// </summary>
        public float elevation;
        
        /// <summary>
        /// The end node entity of the network.
        /// （網路的終點節點實體。）
        /// </summary>
        public Entity end;

        /// <summary>
        /// The form of the network.
        /// （網路的形式。）
        /// </summary>
        public Form form;

        /// <summary>
        /// Whether this `NetworkStat` represents a roundabout or not.
        /// （這個 `NetworkStat` 是否為一個圓環？）
        /// </summary>
        public bool isRoundabout;

        /// <summary>
        /// The number of motorized vehicle lanes.
        /// （機動車輛車道的數量。）
        /// </summary>
        public int lane;

        /// <summary>
        /// The length of the network.
        /// （網路的長度。）
        /// </summary>
        public float length;

        /// <summary>
        /// The speed limit of the network.
        /// （網路的速度限制。）
        /// </summary>
        public float limit;

        /// <summary>
        /// The amount of power transported in the cable.
        /// （電纜輸送的能量總量。）
        /// </summary>
        public float load;

        /// <summary>
        /// The prefab of the asset.
        /// （資產的預製模板。）
        /// </summary>
        public Entity prefab;

        /// <summary>
        /// The value range of the sub lane curve position.
        /// （子車道曲線位置的範圍。）
        /// </summary>
        public float2 range;

        /// <summary>
        /// Whether the start and the end node is a roundabout.
        /// （起點與終點節點是否為圓環？）
        /// </summary>
        public bool2 roundabout;

        /// <summary>
        /// The start node entity of the network.
        /// （網路的起點節點實體。）
        /// </summary>
        public Entity start;

        /// <summary>
        /// The UI group entity.
        /// （UI 群組實體。）
        /// </summary>
        public Entity uiGroup;

        /// <summary>
        /// The traffic volume of the network.
        /// （網路的交通流量。）
        /// </summary>
        public float volume;

        /// <summary>
        /// The width of the network.
        /// （網路的寬度。）
        /// </summary>
        public float width;

        /// <summary>
        /// The feature type of the network.
        /// （網路的圖徵型別。）
        /// </summary>
        public readonly Feature Object
        {
            get
            {
                Feature networkType = Feature.None;
                if ((category & (NetworkCategory.LowCable | NetworkCategory.HighCable)) != 0) networkType |= Feature.Cable;
                if ((category & NetworkCategory.Fence) != 0) networkType |= Feature.Fence;
                if ((category & (NetworkCategory.Bicycle | NetworkCategory.Pathway)) != 0) networkType |= Feature.Pathway;
                if ((category & (NetworkCategory.SewagePipe | NetworkCategory.StormPipe | NetworkCategory.WaterPipe)) != 0) networkType |= Feature.Pipe;
                if ((category & NetworkCategory.Car) != 0) networkType |= Feature.Road;
                if ((category & NetworkCategory.Highway) != 0) networkType |= Feature.Road;
                if ((category & NetworkCategory.Runway) != 0) networkType |= Feature.Runway;
                if ((category & NetworkCategory.Taxiway) != 0) networkType |= Feature.Taxiway;
                if ((category & (NetworkCategory.Subway | NetworkCategory.Train | NetworkCategory.Tram)) != 0) networkType |= Feature.Track;
                if ((category & NetworkCategory.Waterway) != 0) networkType |= Feature.Waterway;
                return networkType;
            }
        }

        /// <summary>
        /// Check whether the network has the node entity.
        /// （確認網路是否擁有節點實體。）
        /// </summary>
        /// <param name="node">The node entity.（節點實體。）</param>
        /// <returns>If true, the entity is either the start or the end node of the network.（若為真，實體為網路的起點或終點節點。）</returns>
        public readonly bool HasNode(Entity node)
        {
            return start.Equals(node) || end.Equals(node);
        }

        /// <summary>
        /// Initiate the roundabout-related properties.
        /// （初始化與圓環相關的屬性。）
        /// </summary>
        /// <param name="roundaboutEntityMap">The map between roundabout node and its statistics.（圓環節點與統計資訊的映射表。）</param>
        public void InitiateRoundaboutProperties(ref NativeParallelHashMap<Entity, Roundabout> roundaboutEntityMap)
        {
            if (!roundaboutEntityMap.IsCreated) return;
            if (roundaboutEntityMap.TryGetValue(entity, out _))
            {
                // The network itself is a roundabout.（網路本身即是圓環。）
                isRoundabout = true;
                roundabout = new(false, false);
            }
            else
            {
                // Test whether the start node and/or the end node is roundabouts.（測試起點與終點節點是否為圓環？）
                isRoundabout = false;
                roundabout = new(roundaboutEntityMap.TryGetValue(start, out _), roundaboutEntityMap.TryGetValue(end, out _));
            }
        }

        public override readonly string ToString()
        {
            return $"Network({entity.Index}:{entity.Version}) - Aggregation [{aggregation}], Bike [{bike}], Category [{category}], Capacity [{capacity}], Direction [{direction}], Discharge [{discharge}], Elevation [{elevation}], End [{end.Index}:{end.Version}], Form [{form}], Lane [{lane}], Length [{length}], Limit [{limit}], Load [{load}], Prefab [{prefab}], Range [{range.x}, {range.y}], Roundabout [{roundabout.x}, {roundabout.y}], Start [{start.Index}:{start.Version}], UI Group [{uiGroup}], Volume [{volume}], Width [{width}]";
        }
    }
}