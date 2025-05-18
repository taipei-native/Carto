using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The container of network lane information.
    /// （車道資訊的容器。）
    /// </summary>
    public struct Lane
    {
        /// <summary>
        /// The network lane entity.
        /// （車道實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The category of the network lane.
        /// （車道的分類。）
        /// </summary>
        public NetworkCategory category;

        public override readonly string ToString()
        {
            return $"Lane({entity.Index}:{entity.Version}) - Category [{category}]";
        }
    }
}