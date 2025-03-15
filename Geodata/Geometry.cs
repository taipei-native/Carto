using Colossal.Mathematics;
using System;
using System.Collections.Generic;
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
        /// The look-up table of inclusion rings with exlusion rings.
        /// （擁有排除環的包含環的對照表。）
        /// </summary>
        public Dictionary<int, int> ExclusionIndexTable { get; set; }

        /// <summary>
        /// Define the region thats removes area from a polygon.
        /// （定義從多邊形移除面積的部分。）
        /// </summary>
        public float3[][][] Exclusions { get; set; }

        /// <summary>
        /// Define the region that forms area to the polygon.
        /// （定義添加面積至多邊形的部分。）
        /// </summary>
        public float3[][] Inclusions { get; set; }

        public Geometry(float3[][] inclusions)
        {
            Inclusions = inclusions;
            Exclusions = new float3[0][][];
            ExclusionIndexTable = new Dictionary<int, int> { };
        }

        public Geometry(float3[][] inclusions, float3[][][] exclusions, Dictionary<int, int> exclusionIndexTable)
        {
            Inclusions = inclusions;
            Exclusions = exclusions;
            ExclusionIndexTable = exclusionIndexTable;
            ValidateTable();
        }

        /// <summary>
        /// Whether the polygon has exclusion parts?
        /// （這個多邊形是否有排除的部分？）
        /// </summary>
        public bool HasHoles => Exclusions.Length > 0;

        /// <summary>
        /// Whether the geometry represents a multipolygon?
        /// （這個幾何圖形是否代表一個複數多邊形？）
        /// </summary>
        public bool IsMultiPolygon => Inclusions.Length > 1;

        /// <summary>
        /// The number of parts in the geometry.
        /// （幾何圖形的部件數。）
        /// </summary>
        /// <param name="pointCount">The number of points in the geometry.（幾何圖形的點數。）</param>
        /// <param name="pointCounts">The number of points in each part.（每個部件包含的點數。）</param>
        /// <param name="bounds">The bounding box of the geometry.（幾何圖形的定界框。）</param>
        public int GetParts(out int pointCount, out List<int> pointCounts, out Bounds3 bounds)
        {
            bounds = new();
            bounds.Reset();
            int partCount = Inclusions.Length;
            pointCount = 0;
            pointCounts = new();
            for (int i = 0; i < Inclusions.Length; i++)
            {
                pointCount += Inclusions[i].Length;
                pointCounts.Add(Inclusions[i].Length);
                for (int j = 0;  j < Inclusions[i].Length; j++)
                {                    
                    bounds |= Inclusions[i][j];
                }
            }
            for (int i = 0; i < Exclusions.Length; i++)
            {
                partCount += Exclusions[i].Length;
                for (int j = 0; j < Exclusions[i].Length; j++)
                {
                    pointCount += Exclusions[i][j].Length;
                    pointCounts.Add(Exclusions[i][j].Length);
                    for (int k = 0;  k < Exclusions[i][j].Length; k++)
                    {
                        bounds |= Exclusions[i][j][k];
                    }
                }
            }
            return partCount;
        }

        /// <summary>
        /// Validate the <see cref="ExclusionIndexTable"/>'s correctness.<br/>
        /// （檢驗 <see cref="ExclusionIndexTable"/> 是否正確。）
        /// </summary>
        private void ValidateTable()
        {
            foreach (int inclusionIndex in ExclusionIndexTable.Keys)
            {
                if ((inclusionIndex >= Inclusions.Length) || (inclusionIndex < 0))
                {
                    throw new ArgumentException($"Invalid inclusion ring index `{inclusionIndex}` in ExclusionIndexTable. ExclusionIndexTable 中有無效的包含環索引 `{inclusionIndex}` 。");
                }

                int exclusionIndex = ExclusionIndexTable[inclusionIndex];

                if ((exclusionIndex >= Exclusions.Length) || (exclusionIndex < 0))
                {
                    throw new ArgumentException($"Invalid exclusion ring index `{exclusionIndex}` in ExclusionIndexTable. ExclusionIndexTable 中有無效的排除環索引 `{exclusionIndex}` 。");
                }
            }
        }
    }
}