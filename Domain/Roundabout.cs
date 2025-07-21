using Unity.Entities;
using Unity.Mathematics;

namespace Carto.Domain
{
    /// <summary>
    /// The container of roundabout's information.
    /// （圓環資訊的容器。）
    /// </summary>
    public struct Roundabout
    {
        /// <summary>
        /// If the roundabout is created by an attached asset (e.g. placeable Cul-de-sac or roundabouts,) the entity would be here.<br/>
        /// （若圓環由附加的資產（例如可放置的囊底路或圓環）創造，該實體會出現在這裡。）
        /// </summary>
        public Entity attached;
        
        /// <summary>
        /// The radius of the inner ring of the roundabout.
        /// （圓環內環的半徑。） 
        /// </summary>
        public float innerRingRadius;

        /// <summary>
        /// The node entity where the roundabout attached on.
        /// （圓環附著的節點實體。）
        /// </summary>
        public Entity node;

        /// <summary>
        /// The radius of the outer ring of the roundabout.
        /// （圓環外環的半徑。） 
        /// </summary>
        public float outerRingRadius;

        /// <summary>
        /// The position of the roundabout.
        /// （圓環的位置。）
        /// </summary>
        public float3 position;

        /// <summary>
        /// The road width of the roundabout.
        /// （圓環的道路寬度。）
        /// </summary>
        public float width;

        public override readonly string ToString()
        {
            return $"Roundabout({node.Index}: {node.Version}) - Attached [{attached.Index}:{attached.Version}], Inner Ring Radius [{innerRingRadius}], Outer Ring Radius [{outerRingRadius}], Width [{width}]";
        }
    }
}