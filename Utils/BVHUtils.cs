using Colossal.Mathematics;
using Game.Areas;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Carto.Utils
{
    public static partial class BVHUtils
    {
        public struct Bounds : IEquatable<Bounds>
        {
            public float2 max;

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

            public readonly bool Intersect(Bounds other)
            {
                return MathUtils.Intersect(ToBounds2(), other.ToBounds2());
            }

            public readonly bool Intersect(float2 point)
            {
                return MathUtils.Intersect(ToBounds2(), point);
            }

            public readonly bool Intersect(float3 point)
            {
                return Intersect(point.xz);
            }

            public void Merge(Bounds other)
            {
                max = math.max(max, other.max);
                min = math.min(min, other.min);
            }

            public void Merge(float2 point)
            {
                max = math.max(max, point);
                min = math.min(min, point);
            }

            public void Merge(float3 point)
            {
                Merge(point.xz);
            }

            public readonly Bounds2 ToBounds2()
            {
                return new(min, max);
            }

            public override readonly string ToString()
            {
                return $"BVHTree.Bounds - Max [{max.x} {max.y}], Min [{min.x} {min.y}]";
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

        public struct Node : IEquatable<Node>
        {
            public Bounds bounds;

            public int left;

            public int right;

            public int triangleCount;

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

            public readonly bool IsLeaf()
            {
                return triangleCount > 0;
            }

            public override readonly string ToString()
            {
                return $"BVHTree.Node - Bounds [{bounds}], Left [{left}], Right [{right}], TriangleCount [{triangleCount}], TriangleIndex [{triangleIndex}]";
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

        public struct Tree
        {
            private int _nextIndex;

            public NativeArray<Node> nodes;

            public NativeArray<Triangle> triangles;

            public Tree(NativeList<Triangle> triangles, Allocator allocator)
            {
                _nextIndex = 0;
                this.triangles = triangles.AsArray();
                nodes = new(triangles.Length * 4, allocator);
            }

            public void Build()
            {
                _ = Build(0, triangles.Length);
            }

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

            public void Dispose()
            {
                CommonUtils.Dispose(ref nodes);
                CommonUtils.Dispose(ref triangles);
            }

            private int GetAxis(int index, int count)
            {
                Bounds bounds = GetBounds(index, count);
                float2 difference = bounds.max - bounds.min;
                return difference.x > difference.y ? 0 : 1;
            }

            private Bounds GetBounds(int index, int count)
            {
                Bounds bounds = triangles[index].Bounds();
                for (int i = index + 1; i < index + count; i++)
                {
                    bounds.Merge(triangles[i].Bounds());
                }
                return bounds;
            }

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
        }

        public struct Triangle : IEquatable<Triangle>
        {
            public float2 a;

            public float2 b;

            public float2 c;

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

            public readonly Bounds Bounds()
            {
                return new(math.max(math.max(a, b), c), math.min(math.min(a, b), c));
            }

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

            public readonly bool Intersect(float2 point)
            {
                return MathUtils.Intersect(ToTriangle2(), point);
            }

            public readonly bool Intersect(float3 point)
            {
                return Intersect(point.xz);
            }

            public override readonly string ToString()
            {
                return $"BVHTree.Triangle - A [{a.x} {a.y}], B [{b.x} {b.y}], C [{c.x} {c.y}], Owner [{owner.Index}:{owner.Version}]";
            }

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

        public struct TriangleComparer : IComparer<Triangle>
        {
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