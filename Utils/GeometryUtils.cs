namespace Carto.Utils
{
    using Colossal.Mathematics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.Mathematics;

    /// <summary>
    /// A class to extend functionality on the manipulation of geometries.
    /// （一個擴展操作幾何圖形功能的類別。）
    /// </summary>
    public class GeometryUtils
    {
        /// <summary>
        /// Add specific value to the target azimuth angle.（添加特定值至目標的方位角。）
        /// </summary>
        /// <param name="target">The target azimuth angle.（目標方位角。）</param>
        /// <param name="addition">The value adds to the target azimuth angle.（添加至目標方位角的值。）</param>
        public static float AddAngle(float target, float addition)
        {
            return Standardize(Standardize(target) + addition);
        }
        
        /// <summary>
        /// Interpolate a circular arc segment.
        /// （內插一段圓弧。）
        /// </summary>
        /// <param name="circle">The reference circle.（參考圓。）</param>
        /// <param name="fromAngle">The azimuth of the start.（起點處的方位角。）</param>
        /// <param name="toAngle">The azimuth of the end.（終點處的方位角。）</param>
        /// <param name="threshold">The maximum distance between original arc and the intepolatted line segments.（原始圓弧與內插線段之間的最大距離。）</param>
        public static List<float3> Arc(Circle3 circle, float fromAngle, float toAngle, float threshold = 0.2f)
        {
            float maxAngle = 60;
            float angle = 4 * math.asin(math.sqrt(threshold / 2 / circle.radius));
            if (toAngle < fromAngle) toAngle += (float)(2 * math.PI_DBL);
            int minPointsCount = (int)math.ceil((toAngle - fromAngle) / maxAngle) + 1;
            int pointsCount = (int)math.ceil((toAngle - fromAngle) / angle) + 1;

            List<float3> points = new List<float3>();

            if (pointsCount >= minPointsCount)
            {
                float newAngle = (float)((toAngle - fromAngle) / (pointsCount - 1));

                for (int i = 0; i < pointsCount; i++)
                {
                    float3 delta = new float3(circle.radius * math.sin(fromAngle + newAngle * i), 0, circle.radius * math.cos(fromAngle + newAngle * i));
                    float3 node = circle.position + delta;
                    points.Add(node);
                }
            }
            else
            {
                for (int i = 0; i < minPointsCount; i++)
                {
                    float3 delta = new float3(circle.radius * math.sin(fromAngle + maxAngle * i), 0, circle.radius * math.cos(fromAngle + maxAngle * i));
                    float3 node = circle.position + delta;
                    points.Add(node);
                }
            }

            return points;
        }

        /// <summary>
        /// Find out the bearing angle of the vector.
        /// （找出向量的方位角。）
        /// </summary>
        /// <param name="vector">The reference vector.（參考向量。）</param>
        public static float Azimuth(float2 vector)
        {
            float len = math.sqrt(vector.x * vector.x + vector.y * vector.y);
            float angle = math.asin(vector.x / len);

            if (vector.y < 0)
            {
                return math.PI - angle;
            }
            else if (vector.x >= 0)
            {
                return angle;
            }
            else
            {
                return 2 * math.PI + angle;
            }
        }

        /// <summary>
        /// Find out the bearing angle of the vector.
        /// （找出向量的方位角。）
        /// </summary>
        /// <param name="vector">The reference vector.（參考向量。）</param>
        public static float Azimuth(float3 vector)
        {
            return Azimuth(vector.xz);
        }

        /// <summary>
        /// Find out the bearing angle of the destination.
        /// （找出目的地的方位角。）
        /// </summary>
        /// <param name="fromPosition">The start position.（開始位置。）</param>
        /// <param name="toPosition">The end position.（結束位置。）</param>
        public static float Azimuth(float2 fromPosition, float2 toPosition)
        {
            float2 vector = toPosition - fromPosition;
            return Azimuth(vector);
        }

        /// <summary>
        /// Find out the bearing angle of the destination.
        /// （找出目的地的方位角。）
        /// </summary>
        /// <param name="fromPosition">The start position.（開始位置。）</param>
        /// <param name="toPosition">The end position.（結束位置。）</param>
        public static float Azimuth(float3 fromPosition, float3 toPosition)
        {
            return Azimuth(fromPosition.xz, toPosition.xz);
        }

        /// <summary>
        /// Reflect the line at the end of the <paramref name="line"/> segment.
        /// （將線段從終點處反轉。）
        /// </summary>
        /// <param name="line">The original line segment.（原始線段。）</param>
        public static Line3.Segment EndReflect(Line3.Segment line)
        {
            float3 vector = line.a - line.b;
            return new Line3.Segment(line.b - vector, line.b);
        }

        /// <summary>
        /// Trim the <paramref name="line"/> segment so that the new line segment has same end with the original one and the length of <paramref name="length"/>.
        /// （裁剪線段使其與原始線段共享相同的終點及擁有<paramref name="length"/>的長度。）
        /// </summary>
        /// <param name="line">The original line segment.（原始線段。）</param>
        /// <param name="length">The new line segment length.（新線段長度。）</param>
        public static Line3.Segment EndTrim(Line3.Segment line, float length)
        {
            float3 vector = (line.a - line.b) / MathUtils.Length(line) * length;
            return new Line3.Segment(line.b + vector, line.b);
        }

        /// <summary>
        /// Interpolate a Bezier curve.
        /// （內插貝茲曲線。）
        /// </summary>
        public static List<float3> Interpolate(Bezier4x3 curve, float gap = 4f, float threshold = 2f)
        {
            List<float3> points = new List<float3>();
            Line3.Segment straight = MathUtils.Line(curve);

            // If the distance from bezier point (b & c) to the straight line between fix points (a & d) is less than the threshold value (2 meters), the curve can be treat as a straight line.
            // （若由貝茲點（B、C）至固定點（A、D）間連線的距離小於閾值（2 公尺），該曲線可被視為直線。）
            if ((MathUtils.Distance(straight.xz, curve.b.xz, out _) <= threshold) & (MathUtils.Distance(straight.xz, curve.c.xz, out _) <= threshold))
            {
                points.Add(curve.a);
                points.Add(curve.d);
            }
            else
            {
                // If the distance exceeds the threshold, the curve will be sampled multiple times, based on the total length of the curve.
                // （若距離超過了閾值，那麼曲線會根據總長度採樣數次。）
                int samplePoints = (int)Math.Ceiling(MathUtils.Length(curve) / gap);
                float3 lastNode = new float3();
                for (float i = 0; i <= samplePoints + 1; i++)
                {
                    float t = i / (samplePoints + 1);
                    float3 node = MathUtils.Position(curve, t);

                    if ((points.Count == 0) || !node.Equals(lastNode))
                    {
                        points.Add(node);
                        lastNode = node;
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// Interpolate a Bezier curve.
        /// （內插貝茲曲線。）
        /// </summary>
        public static List<float3> Interpolate(Bezier4x3 curve, float threshold = 0.2f)
        {
            float bypass = 2f;
            float pntr = 0;
            List<float3> points = new List<float3>();

            /// <summary>
            /// Dynamically searching t value so that the interpolatted curve meets the threshold requirement.
            /// （動態搜尋t值，使內插的曲線符合閾值要求。）
            /// </summary>
            float3 FindT(float t0, out float t1)
            {
                float lastT = t0;
                float nextT = 1;
                float t = nextT;

                for (int i = 0; i < 15; i++)
                {
                    Bezier4x3 subCurve = Trim(curve, t0, t);
                    float distance = MaxDistance(subCurve, out bool twoSided);
                    if (twoSided || (distance > threshold))
                    {
                        nextT = t;
                        t = (lastT + t) / 2;
                    }
                    else
                    {
                        if (t == 1)
                        {
                            break;
                        }
                        else
                        {
                            lastT = t;
                            t = (nextT + t) / 2;
                        }
                    }
                }

                t1 = t;
                return MathUtils.Position(curve, t);
            }

            int counter = 0;
            Line3.Segment straight = MathUtils.Line(curve);

            if ((MathUtils.Distance(straight.xz, curve.b.xz, out _) <= bypass) & (MathUtils.Distance(straight.xz, curve.c.xz, out _) <= bypass))
            {
                points.Add(curve.a);
                points.Add(curve.d);
                return points;
            }

            while (pntr != 1)
            {
                points.Add(FindT(pntr, out float newPntr));
                pntr = newPntr;
                counter++;

                if (counter > 200)
                {
                    break;
                }
            }

            return points;
        }

        /// <summary>
        /// Interpolate a circle.
        /// （內插圓形。）
        /// </summary>
        /// <param name="circle">Original circle.（原始圓形。）</param>
        /// <param name="threshold">Maximum distance between outputted line segments and the circle.（輸出的線段與圓形間的最大距離。）</param>
        public static List<float3> Interpolate(Circle3 circle, float threshold = 0.2f)
        {
            /*
                Given a circular arc with radius `r` and the maximum distance between the chord with same endpoints
                as the arc `d`, the central angle `θ` is defined as 4 * arcsin(sqrt(d / 2r)). This could be found by
                writing `d` as r * versin(θ/2), or 2r * sin^2(θ/4).
                （給定一個半徑為`r`的圓弧 和 弧與共享端點的弦之間的最大距離`d`，其圓心角`θ`被定義為 4 * arcsin(sqrt(d / 2r))。
                　這可以由`d`推導出來：r * versin(θ/2)，或是 2r * sin^2(θ/4)。）
            */

            int minPointsCount = 6;
            float maxAngle = (float)(2 * math.PI_DBL / minPointsCount);
            float angle = 4 * math.asin(math.sqrt(threshold / 2 / circle.radius));
            int pointsCount = (int)math.ceil(2 * math.PI_DBL / angle);

            List<float3> points = new List<float3>();

            if (pointsCount >= minPointsCount)
            {
                float newAngle = (float)(2 * math.PI_DBL / pointsCount);

                for (int i = 0; i < pointsCount; i++)
                {
                    float3 delta = new float3(circle.radius * math.sin(newAngle * i), 0, circle.radius * math.cos(newAngle * i));
                    float3 node = circle.position + delta;
                    points.Add(node);
                }
            }
            else
            {
                for (int i = 0; i < minPointsCount; i++)
                {
                    float3 delta = new float3(circle.radius * math.sin(maxAngle * i), 0, circle.radius * math.cos(maxAngle * i));
                    float3 node = circle.position + delta;
                    points.Add(node);
                }
            }

            return points;
        }

        /// <summary>
        /// Find out whether the circle intersects a line, and their intersections.
        /// （找出圓形與直線是否相交，以及它們的交點。）
        /// </summary>
        /// <param name="circle">The reference circle.（參考圓。）</param>
        /// <param name="line">The reference line.（參考直線。）</param>
        /// <param name="intersection">The points where the circle intersects the line, if any.（直線與圓相交的點（若有）。）</param>
        public static bool Intersect(Circle2 circle, Line2 line, out float2[] intersection)
        {
            intersection = new float2[2];
            float a = circle.position.x;
            float b = circle.position.y;
            float m = line.a.x;
            float n = line.a.y;
            float r = circle.radius;
            float u = line.b.x - m;
            float v = line.b.y - n;
            float al = u * u + v * v;
            float be = 2 * (u * (m - a) + v * (n - b));
            float ga = a * a + b * b + m * m + n * n - r * r - 2 * (a * m + b * n);
            float de = be * be - 4 * al * ga;
            if (de < 0) return false;
            float f1 = (-be + math.sqrt(de)) / 2 / al;
            float f2 = (-be - math.sqrt(de)) / 2 / al;
            intersection[0] = new float2(m + f1 * u, n + f1 * v);
            intersection[1] = new float2(m + f2 * u, n + f2 * v);
            return true;
        }

        /// <summary>
        /// Find out whether the circle intersects a line, and their intersections.
        /// （找出圓形與直線是否相交，以及它們的交點。）
        /// </summary>
        /// <param name="circle">The reference circle.（參考圓。）</param>
        /// <param name="line">The reference line.（參考直線。）</param>
        /// <param name="intersection">The points where the circle intersects the line, if any.（直線與圓相交的點（若有）。）</param>
        public static bool Intersect(Circle3 circle, Line2 line, out float3[] intersection)
        {
            intersection = new float3[2];
            Circle2 flatCircle = new Circle2(circle.radius, circle.position.xz);
            bool isIntersect = Intersect(flatCircle, line, out float2[] intersections);
            intersection[0] = new float3(intersections[0].x, circle.position.y, intersections[0].y);
            intersection[1] = new float3(intersections[1].x, circle.position.y, intersections[1].y);
            return isIntersect;
        }

        /// <summary>
        /// Find out whether the circle intersects a line, and their intersections.
        /// （找出圓形與直線是否相交，以及它們的交點。）
        /// </summary>
        /// <param name="circle">The reference circle.（參考圓。）</param>
        /// <param name="line">The reference line.（參考直線。）</param>
        /// <param name="intersection">The points where the circle intersects the line, if any.（直線與圓相交的點（若有）。）</param>
        public static bool Intersect(Circle3 circle, Line3 line, out float3[] intersection)
        {
            Line2 flatLine = line.xz;
            bool isIntersect = Intersect(circle, flatLine, out float3[] intersections);
            intersection = intersections;
            return isIntersect;
        }

        /// <summary>
        /// Find the intersections between the interpolatted circle and continuous line segments.
        /// （搜尋內插後的圓形與連續線段的交點。）
        /// </summary>
        /// <param name="circle">Interpolatted circle.（內插後的圓形。）</param>
        /// <param name="line">Continuous line segments.（連續線段。）</param>
        /// <param name="circleNodeIndex">Intersection's position on the circle.（交點在圓形上的位置。）</param>
        /// <param name="lineNodeIndex">Intersection's position on the line segments.（交點在線段上的位置。）</param>
        public static List<float3> Intersection(List<float3> circle, List<float3> line, out List<int> circleNodeIndex, out List<int> lineNodeIndex)
        {
            circleNodeIndex  = new List<int>();
            lineNodeIndex    = new List<int>();
            List<Bounds2> circleBounds = new List<Bounds2>();
            List<Bounds2> lineBounds   = new List<Bounds2>();
            List<float3>  points       = new List<float3>();
            List<Line3.Segment> circleSegments = new List<Line3.Segment>();
            List<Line3.Segment> lineSegments   = new List<Line3.Segment>();

            for(int i = 0; i < circle.Count; i++)
            {
                Line3.Segment segment = new Line3.Segment(circle[i], (i == circle.Count - 1) ? circle[0] : circle[i + 1]);
                Bounds3 bounds = MathUtils.Bounds(segment);
                circleSegments.Add(segment);
                circleBounds.Add(bounds.xz);
            }

            for (int i = 0; i < line.Count - 1; i++)
            {
                Line3.Segment segment = new Line3.Segment(line[i], line[i + 1]);
                Bounds3 bounds = MathUtils.Bounds(segment);
                lineSegments.Add(segment);
                lineBounds.Add(bounds.xz);
            }

            for (int i = 0; i < circleSegments.Count; i++)
            {
                for (int j = 0; j < lineSegments.Count; j++)
                {
                    // Only try to find the segment-segment intersection point if there is an intersection between bounds.
                    // （只在邊界框有相交時嘗試尋找線段－線段交點。）
                    if (MathUtils.Intersect(circleBounds[i], lineBounds[j]))
                    {
                        if (MathUtils.Intersect(circleSegments[i].xz, lineSegments[j].xz, out float2 t))
                        {
                            points.Add(MathUtils.Position(circleSegments[i], t.x));
                            circleNodeIndex.Add(i + 1);
                            lineNodeIndex.Add(j + 1);
                        }
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// Check whether the target azimuth angle is lefter than the reference azimuth angle.
        /// （確認目標方位角是否位於參考方位角左側。)
        /// </summary>
        /// <param name="target">The target azimuth angle.（目標方位角。）</param>
        /// <param name="reference">The reference azimuth angle.（參考方位角。）</param>
        public static bool Lefter(float target, float reference)
        {
            return Lefter(target, reference, out _);
        }

        /// <summary>
        /// Check whether the target azimuth angle is lefter than the reference azimuth angle.
        /// （確認目標方位角是否位於參考方位角左側。)
        /// </summary>
        /// <param name="target">The target azimuth angle.（目標方位角。）</param>
        /// <param name="reference">The reference azimuth angle.（參考方位角。）</param>
        /// <param name="lefter">The lefter azimuth angle.（位於左側的方位角。）</param>
        public static bool Lefter(float target, float reference, out float lefter)
        {
            target = Standardize(target);
            float[] azimuths = new float[] { target, reference };
            lefter = Leftest(azimuths, reference);
            return lefter == target;
        }

        /// <summary>
        /// Find out the leftest azimuth angle according to the reference center azimuth angle.
        /// （根據參考中心方位角，找出最左側的方位角。）
        /// </summary>
        /// <param name="azimuths">The collection of azimuth angles.（方位角的集合。）</param>
        /// <param name="center">The reference center azimuth angle.（參考中央方位角。）</param>
        /// <param name="standardized">Whether to use standardized (0 ~ 2π) azimuth angles.（是否使用標準化的方位角（0 ~ 2π）。）</param>
        public static float Leftest(float[] azimuths, float center, bool standardized = true)
        {
            float leftest; 
            float[] modified;
            center = Standardize(center);
            if (center <= math.PI) modified = azimuths.Select(n => ((Standardize(n) >= center + math.PI) && (Standardize(n) <= 2 * math.PI)) ? Standardize(n) - 2 * math.PI : Standardize(n)).ToArray();
            else                   modified = azimuths.Select(n => ((Standardize(n) >= 0) && (Standardize(n) <= center -  math.PI)) ? Standardize(n) + 2 * math.PI : Standardize(n)).ToArray();
            leftest = modified.Min();
            return standardized ? Standardize(leftest) : leftest;
        }

        /// <summary>
        /// Calculate the total length of continuous line segments.
        /// （計算連續線段的總長度。）
        /// </summary>
        /// <param name="line">The continuos line segments.（連續線段。）</param>
        public static float Length(List<float3> line)
        {
            float total = 0;

            for(int i = 0; i < line.Count - 2; i++)
            {
                total += math.distance(line[i], line[i + 1]);
            }

            return total;
        }

        /// <summary>
        /// Calculate the location where the circle and the ray, which initiates at circle's center with specific angle, intersects.
        /// （計算由圓心出發的特定角度射線與圓相交的位置。）
        /// </summary>
        /// <param name="circle">The reference circle.（參考圓。）</param>
        /// <param name="angle">The angle ray pointing to.（射線指向的角度。）</param>
        /// <returns></returns>
        public static float3 Location(Circle3 circle, float angle)
        {
            return new float3(circle.radius * math.sin(angle) + circle.position.x, circle.position.y, circle.radius * math.cos(angle) + circle.position.z);
        }

        /// <summary>
        /// Calculate the mean angle between two azimuth angles.
        /// （計算兩方位角之間的平均值。）
        /// </summary>
        /// <param name="angleA">One of the two azimuth angles.（兩個方位角的其中一個。）</param>
        /// <param name="angleB">Another one of the two azimuth angles.（兩個方位角的另外一個。）</param>
        public static float MeanAngle(float angleA, float angleB)
        {
            angleA = Standardize(angleA);
            angleB = Standardize(angleB);
            float meanAngleA = (angleA + angleB) / 2;
            float meanAngleB = (angleA + angleB + 2 * math.PI) / 2;
            return (RotationAngle(angleA, meanAngleA) >= RotationAngle(angleA, meanAngleB)) ? meanAngleB : meanAngleA;
        }

        /// <summary>
        /// Calculate the maximum distance between the curve and the straight line sharing same endpoints.
        /// （計算曲線和共享起訖點的直線間的最大距離。）
        /// </summary>
        /// <param name="curve">The original curve.（原始曲線。）</param>
        /// <param name="twoSided">Whether the straight line intersects the curve in the middle.（曲線是否與直線相交於中段？）</param>
        /// <returns></returns>
        public static float MaxDistance(Bezier4x3 curve, out bool twoSided)
        {
            // Align the curve with the x-axis, so that the x value of the tight bounds of the transformed curve represents
            // the maximum distance between the curve and the straight line through endpoints.
            // 將曲線與x軸對齊，因此變換後的曲線的緊密邊界x值表示曲線與其端點相連直線間的最大距離。
            float2 newAxisVector = curve.d.xz - curve.a.xz;
            float angle = RotationAngle(newAxisVector, new float2(1f, 0f));
            float2 Q1 = new float2(0f, 0f);
            float2 Q2 = MathUtils.RotateLeft(curve.b.xz - curve.a.xz, angle);
            float2 Q3 = MathUtils.RotateLeft(curve.c.xz - curve.a.xz, angle);
            float2 Q4 = MathUtils.RotateLeft(curve.d.xz - curve.a.xz, angle);
            Bounds2 bounds = MathUtils.TightBounds(new Bezier4x2(Q1, Q2, Q3, Q4));

            if (MiscUtils.SameSign(bounds.x.max, bounds.x.min))
            {
                twoSided = false;
                return bounds.x.max > 0 ? bounds.x.max : -bounds.x.min;
            }
            else
            {
                twoSided = true;
                return math.max(math.abs(bounds.x.max), math.abs(bounds.x.min));
            }
        }

        /// <summary>
        /// Check whether the target azimuth angle is righter than the reference azimuth angle.
        /// （確認目標方位角是否位於參考方位角右側。)
        /// </summary>
        /// <param name="target">The target azimuth angle.（目標方位角。）</param>
        /// <param name="reference">The reference azimuth angle.（參考方位角。）</param>
        public static bool Righter(float target, float reference)
        {
            return Righter(target, reference, out _);
        }

        /// <summary>
        /// Check whether the target azimuth angle is righter than the reference azimuth angle.
        /// （確認目標方位角是否位於參考方位角右側。)
        /// </summary>
        /// <param name="target">The target azimuth angle.（目標方位角。）</param>
        /// <param name="reference">The reference azimuth angle.（參考方位角。）</param>
        /// <param name="righter">The righter azimuth angle.（位於右側的方位角。）</param>
        public static bool Righter(float target, float reference, out float righter)
        {
            target = Standardize(target);
            float[] azimuths = new float[] { target, reference };
            righter = Rightest(azimuths, reference);
            return righter == target;
        }

        /// <summary>
        /// Find out the rightest azimuth angle according to the reference center azimuth angle.
        /// （根據參考中心方位角，找出最右側的方位角。）
        /// </summary>
        /// <param name="azimuths">The collection of azimuth angles.（方位角的集合。）</param>
        /// <param name="center">The reference center azimuth angle.（參考中央方位角。）</param>
        /// <param name="standardized">Whether to use standardized (0 ~ 2π) azimuth angles.（是否使用標準化的方位角（0 ~ 2π）。）</param>
        public static float Rightest(float[] azimuths, float center, bool standardized = true)
        {
            float rightest;
            float[] modified;
            center = Standardize(center);
            if (center <= math.PI) modified = azimuths.Select(n => ((Standardize(n) >= center + math.PI) && (Standardize(n) <= 2 * math.PI)) ? Standardize(n) - 2 * math.PI : Standardize(n)).ToArray();
            else                   modified = azimuths.Select(n => ((Standardize(n) >= 0) && (Standardize(n) <= center - math.PI)) ? Standardize(n) + 2 * math.PI : Standardize(n)).ToArray();
            rightest = modified.Max();
            return standardized ? Standardize(rightest) : rightest;
        }

        /// <summary>
        /// Find out the rotation angle from <paramref name="reference"/> azimuth angle to <paramref name="target"/> azimuth angle.
        /// （找出由<paramref name="reference"/>方位角至<paramref name="target"/>方位角的旋轉角度。）
        /// </summary>
        /// <param name="target">The target azimuth angle.（目標方位角。）</param>
        /// <param name="reference">The reference azimuth angle.（參考方位角。）</param>
        public static float RotationAngle(float target, float reference)
        {
            float angle;
            target = Standardize(target);
            reference = Standardize(reference);
            if (reference <= math.PI) angle = ((target >= reference + math.PI) && (target <= 2 * math.PI)) ? reference - target + 2 * math.PI : reference - target;
            else                      angle = ((target >= 0) && (target <= reference - math.PI)) ? reference - target - 2 * math.PI : reference - target;
            return math.abs(angle);
        }

        /// <summary>
        /// Find the left-handed rotation angle from <paramref name="fromVector"/> to <paramref name="toVector"/>.
        /// （找出<paramref name="fromVector"/>向量至<paramref name="toVector"/>向量的逆時鐘方向夾角。）
        /// </summary>
        /// <param name="fromVector">The reference vector.（參考向量。）</param>
        /// <param name="toVector">The vector pending evaluation.（待評估的向量。）</param>
        public static float RotationAngle(float2 fromVector, float2 toVector)
        {
            float3 fromVector3 = new float3(fromVector, 0);
            float3 toVector3 = new float3(toVector, 0);
            float3 normal = math.cross(fromVector3, toVector3);
            float3 normalStdVector = math.select(-1, 1, normal.y >= 0) * normal / math.length(normal);
            return math.atan2(math.dot(normal, normalStdVector), math.dot(fromVector, toVector));
        }

        /// <summary>
        /// Evaluate whether <paramref name="otherCurve"/> follows same direction with <paramref name="mainCurve"/>'s.
        /// （評估其他貝茲曲線是否與主貝茲曲線方向相同。）
        /// </summary>
        /// <param name="mainCurve">The reference Bezier curve.（參考貝茲曲線。）</param>
        /// <param name="otherCurve">The Bezier curve pending evaluation.（待評估的貝茲曲線。）</param>
        /// <param name="t"><paramref name="otherCurve"/>'s intersection position with the <paramref name="mainCurve"/>.（其他貝茲曲線與主貝茲曲線的交點位置。）</param>
        /// <param name="mainCurveStart">Whether <paramref name="otherCurve"/> join at the start of the <paramref name="mainCurve"/>.（其他貝茲曲線是否交於主貝茲曲線的起點位置。） </param>
        public static bool SameDirection(Bezier4x3 mainCurve, Bezier4x3 otherCurve, out float t, bool mainCurveStart = true)
        {
            float distA = MathUtils.Distance(mainCurve, otherCurve.a, out float tA);
            float distD = MathUtils.Distance(mainCurve, otherCurve.d, out float tD);
            if (mainCurveStart)
            {
                if (distA > distD)
                {
                    t = tD;
                    return true;
                }
                else
                {
                    t = tA;
                    return false;
                }
            }
            else
            {
                if (distA > distD)
                {
                    t = tD;
                    return false;
                }
                else
                {
                    t = tA;
                    return true;
                }
            }
        }

        /// <summary>
        /// Standardize the azimuth angle (0 ~ 2π).
        /// （標準化方位角（0 ~ 2π）。）
        /// </summary>
        /// <param name="azimuth">The inputted azimuth angle.（輸入的方位角。）</param>
        public static float Standardize(float azimuth)
        {
            return (azimuth >= 0) ? (azimuth % (2 * math.PI)) : (2 * math.PI + (azimuth % (-2 * math.PI)));
        }

        /// <summary>
        /// Reflect the line at the start of the <paramref name="line"/> segment.
        /// （將線段從起點處反轉。）
        /// </summary>
        /// <param name="line">The original line segment.（原始線段。）</param>
        public static Line3.Segment StartReflect(Line3.Segment line)
        {
            float3 vector = line.b - line.a;
            return new Line3.Segment(line.a, line.a - vector);
        }

        /// <summary>
        /// Trim the <paramref name="line"/> segment so that the new line segment has same start with the original one and the length of <paramref name="length"/>.
        /// （裁剪線段使其與原始線段共享相同的起點及擁有<paramref name="length"/>的長度。）
        /// </summary>
        /// <param name="line">The original line segment.（原始線段。）</param>
        /// <param name="length">The new line segment length.（新線段長度。）</param>
        public static Line3.Segment StartTrim(Line3.Segment line, float length)
        {
            float3 vector = (line.b - line.a) / MathUtils.Length(line) * length;
            return new Line3.Segment(line.a, line.a + vector);
        }

        /// <summary>
        /// Retrieve the tangent points of a circle when another point is given.
        /// （獲得已知點與圓的切點。）
        /// </summary>
        /// <param name="circle">The reference circle.（參考圓。）</param>
        /// <param name="point">The reference point.（參考點。）</param>
        public static float3[] TangentPoint(Circle3 circle, float3 point)
        {
            float3 translatedPoint = point - circle.position;
            float3[] tangentPoints = new float3[2];
            if (math.distance(point, circle.position) < circle.radius) return tangentPoints;
            float a = translatedPoint.x;
            float b = translatedPoint.z;
            float c = circle.radius * circle.radius;
            float m = a * a + b * b;
            float n = a * c;
            float o = b * c;
            float p = math.sqrt(a * a * c * c - c * m * (c - b * b));
            float q = math.sqrt(b * b * c * c - c * m * (c - a * a));
            bool  d = a * b > 0;
            tangentPoints[0] = d ? new float3((n - p) / m, 0, (o + q) / m) + circle.position : new float3((n + p) / m, 0, (o + q) / m) + circle.position;
            tangentPoints[1] = d ? new float3((n + p) / m, 0, (o - q) / m) + circle.position : new float3((n - p) / m, 0, (o - q) / m) + circle.position;
            return tangentPoints;
        }

        /// <summary>
        /// Trim the Bezier curve to a new curve between <paramref name="t0"/> &amp; <paramref name="t1"/>.
        /// （將貝茲曲線裁剪為起終點位於<paramref name="t0"/>和<paramref name="t1"/>的新曲線。）
        /// </summary>
        /// <param name="curve">The original Bezier curve.（原始貝茲曲線。）</param>
        /// <param name="t0">The start location.（起點位置。）</param>
        /// <param name="t1">The end location.（終點位置。）</param>
        public static Bezier4x3 Trim(Bezier4x3 curve, float t0, float t1)
        {
            /*
                # Source: （資料來源：）

                * MvG. (2012). Drawing part of a Bézier curve by reusing a basic Bézier-curve-function?
                    https://stackoverflow.com/questions/878862/drawing-part-of-a-b%C3%A9zier-curve-by-reusing-a-basic-b%C3%A9zier-curve-function/11705483

                * Wikipedia Contibutors. (March 21, 2024). De Casteljau's algorithm
                    https://en.wikipedia.org/wiki/De_Casteljau%27s_algorithm
            */

            float u0 = 1 - t0;
            float u1 = 1 - t1;
            float3 Q1 = u0 * u0 * u0 * curve.a + 3 * t0 * u0 * u0 * curve.b + 3 * t0 * t0 * u0 * curve.c + t0 * t0 * t0 * curve.d;
            float3 Q2 = u0 * u0 * u1 * curve.a + (2 * t0 * u0 * u1 + t1 * u0 * u0) * curve.b + (2 * t0 * t1 * u0 + t0 * t0 * u1) * curve.c + t0 * t0 * t1 * curve.d;
            float3 Q3 = u0 * u1 * u1 * curve.a + (2 * t1 * u0 * u1 + t0 * u1 * u1) * curve.b + (2 * t0 * t1 * u1 + t1 * t1 * u0) * curve.c + t0 * t1 * t1 * curve.d;
            float3 Q4 = u1 * u1 * u1 * curve.a + 3 * t1 * u1 * u1 * curve.b + 3 * t1 * t1 * u1 * curve.c + t1 * t1 * t1 * curve.d;
            return new Bezier4x3(Q1, Q2, Q3, Q4);
        }
    }
}