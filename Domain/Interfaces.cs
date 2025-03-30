using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The interface for all struct with `Stat` prefix.
    /// （所有具 `Stat` 後綴結構的介面。）
    /// </summary>
    public interface IStat
    {
        /// <summary>
        /// The entity that represents the statistic object.
        /// （代表統計物件的實體。）
        /// </summary>
        public Entity Entity { get; set; }
    }
}