using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides utility functions to build and query the Bounding Volume Hierarchy (BVH) tree.
    /// （提供建構與查詢 BVH （定界體積階層）樹功能的類別。）
    /// </summary>
    public static partial class BVHUtils
    {
        /// <summary>
        /// The definition of an Axis-Aligned Bounding Box (AABB).
        /// （軸對齊定界框（AABB）的定義。）
        /// </summary>
        public struct Bounds : IEquatable<Bounds>
        {
            /// <summary>
            /// The maximum indices of the bounding box.
            /// （定界框座標的極大值。）
            /// </summary>
            public float2 max;

            /// <summary>
            /// The minimum indices of the bounding box.
            /// （定界框座標的極小值。）
            /// </summary>
            public float2 min;

            public Bounds(Bounds2 bounds)
            {
                max = bounds.max;
                min = bounds.min;
            }

            public Bounds(Bounds3 bounds)
            {
                max = bounds.xz.max;
                min = bounds.xz.min;
            }

            public Bounds(float2 max, float2 min)
            {
                this.max = max;
                this.min = min;
            }

            public Bounds(float3 max, float3 min)
            {
                this.max = max.xz;
                this.min = min.xz;
            }

            public readonly bool Equals(Bounds other)
            {
                return math.all((max == other.max) & (min == other.min));
            }

            public override readonly bool Equals(object other)
            {
                return other is Bounds bounds && Equals(bounds);
            }

            public override readonly int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + max.GetHashCode();
                    hash = hash * 31 + min.GetHashCode();
                    return hash;
                }
            }

            /// <summary>
            /// Check whether two bounding box intersect with each other.
            /// （確認兩個定界框是否相交。）
            /// </summary>
            /// <param name="other">Another bounding box.（另一個定界框。）</param>
            /// <returns>Return true if two bounding box intersect with each other.（若兩個定界框相交，回傳 true。）</returns>
            public readonly bool Intersect(Bounds other)
            {
                return MathUtils.Intersect(ToBounds2(), other.ToBounds2());
            }

            /// <summary>
            /// Check whether the bounding box intersects a point.
            /// （確認定界框是否與一點相交。）
            /// </summary>
            /// <param name="point">A point in 2D.（平面空間的點。）</param>
            /// <returns>Return true if intersects.（若相交，回傳 true。）</returns>
            public readonly bool Intersect(float2 point)
            {
                return MathUtils.Intersect(ToBounds2(), point);
            }

            /// <summary>
            /// Check whether the bounding box intersects a point.
            /// （確認定界框是否與一點相交。）
            /// </summary>
            /// <param name="point">A point in 3D.（立體空間的點。）</param>
            /// <returns>Return true if intersects.（若相交，回傳 true。）</returns>
            public readonly bool Intersect(float3 point)
            {
                return Intersect(point.xz);
            }

            /// <summary>
            /// Update the bounding box so it fits both boxes' boundary.
            /// （更新定界框，使其涵蓋兩定界框的邊界。）
            /// </summary>
            /// <param name="other">Another bounding box.（另一個定界框。）</param>
            public void Merge(Bounds other)
            {
                max = math.max(max, other.max);
                min = math.min(min, other.min);
            }

            /// <summary>
            /// Update the bounding box so it fits the original boundary and the newly added point.
            /// （更新定界框，使其涵蓋原始的邊界與新加入的點。）
            /// </summary>
            /// <param name="point">A point in 2D.（平面空間的點。）</param>
            public void Merge(float2 point)
            {
                max = math.max(max, point);
                min = math.min(min, point);
            }

            /// <summary>
            /// Update the bounding box so it fits the original boundary and the newly added point.
            /// （更新定界框，使其涵蓋原始的邊界與新加入的點。）
            /// </summary>
            /// <param name="point">A point in 3D.（立體空間的點。）</param>
            public void Merge(float3 point)
            {
                Merge(point.xz);
            }

            /// <summary>
            /// Convert to CO's <see cref="Bounds2"/> struct.
            /// （轉換為 CO 的 <see cref="Bounds2"/> 結構。）
            /// </summary>
            /// <returns>A <see cref="Bounds2"/> struct.（一個 <see cref="Bounds2"/> 結構。）</returns>
            public readonly Bounds2 ToBounds2()
            {
                return new(min, max);
            }

            public override readonly string ToString()
            {
                return $"BVHUtils.Bounds - Max [{max.x} {max.y}], Min [{min.x} {min.y}]";
            }

            public static bool operator ==(Bounds left, Bounds right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Bounds left, Bounds right)
            {
                return !left.Equals(right);
            }
        }

        /// <summary>
        /// The definition of a node on a BVH tree.
        /// （BVH 樹的節點定義。）
        /// </summary>
        public struct Node : IEquatable<Node>
        {
            /// <summary>
            /// The bounding box of all triangles of the node and its children.
            /// （節點與其子代所有的三角形組成的定界框。）
            /// </summary>
            public Bounds bounds;

            /// <summary>
            /// The index of the left node.（左節點的索引值。）
            /// </summary>
            public int left;

            /// <summary>
            /// The index of the right node.（右節點的索引值。）
            /// </summary>
            public int right;

            /// <summary>
            /// The number of triangles stored by the node.（節點儲存的三角形數量。）
            /// </summary>
            public int triangleCount;

            /// <summary>
            /// The index of the first triangle.（第一個三角形的索引值。）
            /// </summary>
            public int triangleIndex;

            public readonly bool Equals(Node other)
            {
                return (bounds == other.bounds) &
                       (left == other.left) &
                       (right == other.right) &
                       (triangleCount == other.triangleCount) &
                       (triangleIndex == other.triangleIndex);
            }

            public override readonly bool Equals(object other)
            {
                return other is Node node && Equals(node);
            }

            public override readonly int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + bounds.GetHashCode();
                    hash = hash * 31 + left.GetHashCode();
                    hash = hash * 31 + right.GetHashCode();
                    hash = hash * 31 + triangleCount.GetHashCode();
                    hash = hash * 31 + triangleIndex.GetHashCode();
                    return hash;
                }
            }

            /// <summary>
            /// Check whether the node is a leaf node.
            /// （確認節點是否為葉節點。）
            /// </summary>
            /// <returns>Return true if the node is a leaf node.（若節點為葉節點，回傳 true。）</returns>
            public readonly bool IsLeaf()
            {
                return triangleCount > 0;
            }

            public override readonly string ToString()
            {
                return $"BVHUtils.Node - Bounds [{bounds}], Left [{left}], Right [{right}], TriangleCount [{triangleCount}], TriangleIndex [{triangleIndex}]";
            }

            public static bool operator ==(Node left, Node right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Node left, Node right)
            {
                return !left.Equals(right);
            }
        }

        /// <summary>
        /// The definition of a BVH tree.
        /// （BVH 樹的定義。）
        /// </summary>
        public struct Tree
        {
            /// <summary>
            /// The index of the next inserted node.
            /// （下一個將被插入的節點索引值。）
            /// </summary>
            private int _nextIndex;

            /// <summary>
            /// The array representation of all nodes from the tree.
            /// （樹上所有節點的陣列表示形式。）
            /// </summary>
            public NativeArray<Node> nodes;

            /// <summary>
            /// The array representation of all triangles from the nodes.
            /// （節點涵蓋三角形的陣列表示形式。）
            /// </summary>
            public NativeArray<Triangle> triangles;

            public Tree(NativeList<Triangle> triangles, Allocator allocator)
            {
                _nextIndex = 0;
                this.triangles = triangles.AsArray();
                nodes = new(triangles.Length * 4, allocator);
            }

            /// <summary>
            /// Build the BVH tree.
            /// （建構 BVH 樹。）
            /// </summary>
            public void Build()
            {
                _ = Build(0, triangles.Length);
            }

            /// <summary>
            /// Build the BVH tree from the given node.
            /// （由給定的節點開始建構 BVH 樹。）
            /// </summary>
            /// <param name="index">The triangle index.（三角形索引值。）</param>
            /// <param name="count">The number of triangles in the node and its children.（節點與其子代所有三角形的數量。）</param>
            /// <returns>The buiult node's index.（建構完成的節點索引值。）</returns>
            private int Build(int index, int count)
            {
                int nodeIndex = _nextIndex++;
                Node node = new() { bounds = GetBounds(index, count) };

                // Handle leaf node.（處理葉節點。）
                if (count <= 3)
                {
                    node.left = -1;
                    node.right = -1;
                    node.triangleCount = count;
                    node.triangleIndex = index;
                    nodes[nodeIndex] = node;
                    return nodeIndex;
                }

                // Handle internal nodes.（處理中途節點。）
                int medium = Split(index, count);
                node.left = Build(index, medium - index);
                node.right = Build(medium, index + count - medium);
                node.triangleCount = 0;
                node.triangleIndex = -1;
                nodes[nodeIndex] = node;
                return nodeIndex;
            }

            /// <summary>
            /// Try to dispose native containers used in the tree creation.
            /// （嘗試拋棄在建構過程中使用的原生容器。）
            /// </summary>
            public void Dispose()
            {
                CommonUtils.Dispose(ref nodes);
                CommonUtils.Dispose(ref triangles);
            }

            /// <summary>
            /// Retrieve the best axis to split the tree.
            /// （獲得最適合分割樹的軸線。）
            /// </summary>
            /// <param name="index">The triangle index.（三角形索引值。）</param>
            /// <param name="count">The number of triangles in the node and its children.（節點與其子代所有三角形的數量。）</param>
            /// <returns>Return 0 if the axis is x-axis, or 1 if the axis is y-axis.（若最適合的軸線是 X 軸，回傳 0；若為 Y 軸，回傳 1。）</returns>
            private int GetAxis(int index, int count)
            {
                Bounds bounds = GetBounds(index, count);
                float2 difference = bounds.max - bounds.min;
                return difference.x > difference.y ? 0 : 1;
            }

            /// <summary>
            /// Retrieve the bounding box of all triangles in the given node and its children.
            /// （獲得節點與其子代所有三角形的定界框。）
            /// </summary>
            /// <param name="index">The triangle index.（三角形索引值。）</param>
            /// <param name="count">The number of triangles in the node and its children.（節點與其子代所有三角形的數量。）</param>
            /// <returns>The bounding box of these triangles.（這些三角形的定界框。）</returns>
            private Bounds GetBounds(int index, int count)
            {
                Bounds bounds = triangles[index].Bounds();
                for (int i = index + 1; i < index + count; i++)
                {
                    bounds.Merge(triangles[i].Bounds());
                }
                return bounds;
            }

            /// <summary>
            /// Split the tree into two sub-trees.
            /// （將樹分割為兩個子樹。）
            /// </summary>
            /// <param name="index">The triangle index.（三角形索引值。）</param>
            /// <param name="count">The number of triangles in the node and its children.（節點與其子代所有三角形的數量。）</param>
            /// <returns>The index of the median node.（中位節點的索引值。）</returns>
            private int Split(int index, int count)
            {
                int medium = index + count / 2;
                NativeArray<Triangle> temp = new(count, Allocator.Temp);

                for (int i = 0; i < count; i++)
                {
                    temp[i] = triangles[index + i];
                }

                temp.Sort(new TriangleComparer(GetAxis(index, count)));

                for (int i = 0; i < count; i++)
                {
                    triangles[index + i] = temp[i];
                }

                temp.Dispose();
                return medium;
            }

            public override readonly string ToString()
            {
                return $"BVHUtils.Tree - Nodes [{nodes.Length}], Triangles [{triangles.Length}]";
            }
        }

        /// <summary>
        /// The definition of a triangle owned by an entity.
        /// （由一個實體擁有的三角形的定義。）
        /// </summary>
        public struct Triangle : IEquatable<Triangle>
        {
            /// <summary>
            /// One of the point in a triangle.
            /// （三角形中的其中一點。）
            /// </summary>
            public float2 a;

            /// <summary>
            /// One of the point in a triangle.
            /// （三角形中的其中一點。）
            /// </summary>
            public float2 b;

            /// <summary>
            /// One of the point in a triangle.
            /// （三角形中的其中一點。）
            /// </summary>
            public float2 c;

            /// <summary>
            /// The owner of the triangle.
            /// （三角形的擁有者。）
            /// </summary>
            public Entity owner;

            public Triangle(float2 a, float2 b, float2 c, Entity owner)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.owner = owner;
            }

            public Triangle(float3 a, float3 b, float3 c, Entity owner)
            {
                this.a = a.xz;
                this.b = b.xz;
                this.c = c.xz;
                this.owner = owner;
            }

            public Triangle(Triangle2 triangle, Entity owner)
            {
                a = triangle.a;
                b = triangle.b;
                c = triangle.c;
                this.owner = owner;
            }

            public Triangle(Triangle3 triangle, Entity owner)
            {
                a = triangle.a.xz;
                b = triangle.b.xz;
                c = triangle.c.xz;
                this.owner = owner;
            }

            /// <summary>
            /// Retrieve the bounding box of the triangle.
            /// （獲得三角形的定界框。）
            /// </summary>
            /// <returns>The bounding box.（定界框。）</returns>
            public readonly Bounds Bounds()
            {
                return new(math.max(math.max(a, b), c), math.min(math.min(a, b), c));
            }

            /// <summary>
            /// Retrieve the centroid of the triangle.
            /// （獲得三角形的中心點（重心）。）
            /// </summary>
            /// <returns>The position of the centroid.（中心點的位置。）</returns>
            public readonly float2 Center()
            {
                return (a + b + c) / 3f;
            }

            public override readonly bool Equals(object other)
            {
                return other is Triangle triangle && Equals(triangle);
            }

            public readonly bool Equals(Triangle other)
            {
                return math.all((a == other.a) &
                                (b == other.b) &
                                (c == other.c)) &
                       (owner == other.owner);
            }

            public override readonly int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + a.GetHashCode();
                    hash = hash * 31 + b.GetHashCode();
                    hash = hash * 31 + c.GetHashCode();
                    hash = hash * 31 + owner.GetHashCode();
                    return hash;
                }
            }

            /// <summary>
            /// Check whether the triangle intersects a point.
            /// （確認三角形與點是否相交。）
            /// </summary>
            /// <param name="point">A point in 2D.（平面空間的點。）</param>
            /// <returns>Return true if intersects.（若相交，回傳 true。）</returns>
            public readonly bool Intersect(float2 point)
            {
                return MathUtils.Intersect(ToTriangle2(), point);
            }

            /// <summary>
            /// Check whether the triangle intersects a point.
            /// （確認三角形與點是否相交。）
            /// </summary>
            /// <param name="point">A point in 3D.（立面空間的點。）</param>
            /// <returns>Return true if intersects.（若相交，回傳 true。）</returns>
            public readonly bool Intersect(float3 point)
            {
                return Intersect(point.xz);
            }

            public override readonly string ToString()
            {
                return $"BVHUtils.Triangle - A [{a.x} {a.y}], B [{b.x} {b.y}], C [{c.x} {c.y}], Owner [{owner.Index}:{owner.Version}]";
            }

            /// <summary>
            /// Convert to CO's <see cref="Triangle2"/> struct.
            /// （轉換為 CO 的 <see cref="Triangle2"/> 結構。）
            /// </summary>
            /// <returns>A <see cref="Triangle2"/> struct.（一個 <see cref="Triangle2"/> 結構。）</returns>
            public readonly Triangle2 ToTriangle2()
            {
                return new(a, b, c);
            }

            public static bool operator ==(Triangle left, Triangle right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Triangle left, Triangle right)
            {
                return !left.Equals(right);
            }
        }

        /// <summary>
        /// The conditional comparer of the <see cref="Triangle"/>.
        /// （<see cref="Triangle"/> 的條件式比較函數。）
        /// </summary>
        public readonly struct TriangleComparer : IComparer<Triangle>
        {
            /// <summary>
            /// The axis index.
            /// （軸線的索引值。）
            /// </summary>
            private readonly int _axis;

            public TriangleComparer(int axis)
            {
                _axis = axis;
            }

            public readonly int Compare(Triangle x, Triangle y)
            {
                return x.Center()[_axis].CompareTo(y.Center()[_axis]);
            }
        }

        /// <summary>
        /// The job to search intersect triangle in the BVH tree.
        /// （用於在 BVH 樹中尋找相交三角形的工作。）
        /// </summary>
        [BurstCompile]
        public struct SearchJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<float3> points;

            [ReadOnly]
            public Tree tree;

            [WriteOnly]
            public NativeParallelMultiHashMap<Entity, int>.ParallelWriter hashmap;

            public void Execute(int index)
            {
                float3 point = points[index];
                NativeList<int> temp = new(Allocator.Temp) { 0 };

                while (temp.Length > 0)
                {
                    Node node = tree.nodes[temp[temp.Length - 1]];
                    temp.RemoveAt(temp.Length - 1);
                    if (!node.bounds.Intersect(point)) continue;

                    if (node.IsLeaf())
                    {
                        for (int i = 0; i < node.triangleCount; i++)
                        {
                            Triangle triangle = tree.triangles[node.triangleIndex + i];
                            if (triangle.Intersect(point))
                            {
                                temp.Dispose();
                                hashmap.Add(triangle.owner, index);
                                return;
                            }
                        }
                    }
                    else
                    {
                        temp.Add(node.left);
                        temp.Add(node.right);
                    }
                }

                // No match triangles.（沒有符合的三角形。）
                temp.Dispose();
            }
        }
        
        /// <summary>
        /// Map the input points to triangles' owner.
        /// （將輸入點映射至三角形的擁有者。）
        /// </summary>
        /// <param name="triangles">The list of triangles.（三角形的列表。）</param>
        /// <param name="points">The array of point locations.（點的位置陣列。）</param>
        /// <param name="hashmap">The multi-hashmap between owner and point indices.（擁有者與點位索引值之間的多重映射表。）</param>
        public static void GetIntersectMap(ref NativeList<Triangle> triangles, ref NativeArray<float3> points, ref NativeParallelMultiHashMap<Entity, int> hashmap)
        {
            // Build a BVH tree.（建立一個 BVH 樹。）
            Tree tree = new(triangles, Allocator.Persistent);
            tree.Build();

            // Search in the BVH Tree.（在 BVH 樹中搜尋。）
            SearchJob searchJob = new()
            {
                points = points,
                tree = tree,
                hashmap = hashmap.AsParallelWriter()
            };
            JobHandle searchHandle = searchJob.Schedule(points.Length, 8);
            searchHandle.Complete();
            tree.Dispose();
        }
    }
}