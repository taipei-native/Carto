using Unity.Mathematics;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of a geometry.
    /// （幾何圖形的定義。）
    /// </summary>
    public class Geometry
    {
        /// <summary>
        /// Define the region thats removes area from a polygon.
        /// （定義從多邊形移除面積的部分。）
        /// </summary>
        public float3[][] Exclusions { get; set; }

        /// <summary>
        /// Define the region that forms area to the polygon.
        /// （定義添加面積至多邊形的部分。）
        /// </summary>
        public float3[][] Inclusions { get; set; }

        public Geometry(float3[][] exclusions, float3[][] inclusions)
        {
            Exclusions = exclusions;
            Inclusions = inclusions;
        }

        /// <summary>
        /// Whether the polygon has exclusion parts?
        /// （這個多邊形是否有排除的部分？）
        /// </summary>
        public bool HasHoles => Exclusions[0].Length > 0;

        /// <summary>
        /// Whether the geometry represents a multipolygon?
        /// （這個幾何圖形是否代表一個複數多邊形？）
        /// </summary>
        public bool IsMultiPolygon => Inclusions.Length > 1;
    }
}