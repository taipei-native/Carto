using Unity.Entities;
using Unity.Mathematics;

namespace Carto.Domain
{
    /// <summary>
    /// The container of zoning cell's information.
    /// （分區單元資訊的容器。）
    /// </summary>
    public struct ZoningCell
    {
        /// <summary>
        /// One of the point in the zoning cell.
        /// （分區單元的其中一點。）
        /// </summary>
        public double3 a;

        /// <summary>
        /// One of the point in the zoning cell.
        /// （分區單元的其中一點。）
        /// </summary>
        public double3 b;

        /// <summary>
        /// One of the point in the zoning cell.
        /// （分區單元的其中一點。）
        /// </summary>
        public double3 c;
        
        /// <summary>
        /// The cell's index in the zoning block.
        /// （分區單元在分區中的索引值。）
        /// </summary>
        public int cellIndex;

        /// <summary>
        /// One of the point in the zoning cell.
        /// （分區單元的其中一點。）
        /// </summary>
        public double3 d;

        /// <summary>
        /// The zoning block entity.
        /// （分區實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The cell's zoning type index in <see cref="Systems.SharedDataCollectionSystem.ZoningTypes"/> and <see cref="Systems.SharedDataCollectionSystem.ZoningTypesNames"/>.<br/>
        /// （分區單元所屬的分區類型在 <see cref="Systems.SharedDataCollectionSystem.ZoningTypes"/> 與 <see cref="Systems.SharedDataCollectionSystem.ZoningTypesNames"/> 的索引值。）
        /// </summary>
        public int zoningTypeIndex;

        public override readonly string ToString()
        {
            return $"Zoning Cell ({entity.Index}:{entity.Version}) - Cell Index [{cellIndex}], Zoning Type Index [{zoningTypeIndex}]";
        }
    }
}