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

        /// <summary>
        /// The direction of the network lane.
        /// （車道的方向。）
        /// </summary>
        public Direction direction;

        /// <summary>
        /// Whether the lane is an utility lane.
        /// （車道是否為公共車道。）
        /// </summary>
        public readonly bool IsUtilityLane => ((NetworkCategory.LowCable | NetworkCategory.HighCable | NetworkCategory.WaterPipe | NetworkCategory.SewagePipe | NetworkCategory.StormPipe | NetworkCategory.Fence) & category) != 0;

        public override readonly string ToString()
        {
            return $"Lane({entity.Index}:{entity.Version}) - Category [{category}], Direction [{direction}]";
        }
    }
}