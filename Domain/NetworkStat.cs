using Unity.Entities;

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
        /// The curve position where the sub lane ends.
        /// （子車道結束的曲線位置。）
        /// </summary>
        public float end;

        /// <summary>
        /// The index of the roundabout at the end of the network.
        /// （網路結束處圓環的索引值。）
        /// </summary>
        public int endRoundaboutIndex;

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
        /// The curve position where the sub lane starts.
        /// （子車道開始的曲線位置。）
        /// </summary>
        public float start;

        /// <summary>
        /// The index of the roundabout at the start of the network.
        /// （網路開始處圓環的索引值。）
        /// </summary>
        public int startRoundaboutIndex;

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
        /// Whether the network ends at a roundabout.
        /// （網路是否停止在一個圓環？）
        /// </summary>
        public readonly bool HasEndRoundabout => endRoundaboutIndex >= 0;

        /// <summary>
        /// Whether the network starts at a roundabout.
        /// （網路是否開始在一個圓環？）
        /// </summary>
        public readonly bool HasStartRoundabout => startRoundaboutIndex >= 0;

        public override readonly string ToString()
        {
            return $"Network({entity.Index}:{entity.Version}) - Category [{category}], Capacity [{capacity}], Direction [{direction}], Discharge [{discharge}], Elevation [{elevation}], Form [{form}], Length [{length}], Limit [{limit}], Load [{load}], Volume [{volume}], Width [{width}]";
        }
    }
}