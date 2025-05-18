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
        /// The form of the network.
        /// （網路的形式。）
        /// </summary>
        public Form form;

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
        /// The traffic volume of the network.
        /// （網路的交通流量。）
        /// </summary>
        public float volume;

        /// <summary>
        /// The width of the network.
        /// （網路的寬度。）
        /// </summary>
        public float width;

        public override readonly string ToString()
        {
            return $"Network({entity.Index}:{entity.Version}) - Category [{category}], Capacity [{capacity}], Direction [{direction}], Discharge [{discharge}], Elevation [{elevation}], Form [{form}], Length [{length}], Limit [{limit}], Load [{load}], Volume [{volume}], Width [{width}]";
        }
    }
}