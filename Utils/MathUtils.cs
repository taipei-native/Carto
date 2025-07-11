using Carto.Geodata;
using Colossal.Mathematics;
using System;
using System.Globalization;
using Unity.Collections;
using Unity.Mathematics;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides utility functions to numbers.
    /// （提供數學相關功能的類別。）
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// The class that provides format options for <see cref="Colossal.Mathematics"/> and <see cref="Unity.Mathematics"/> structs.
        /// （格式化 <see cref="Colossal.Mathematics"/> 及 <see cref="Unity.Mathematics"/> 結構的類別。）
        /// </summary>
        public static class Format
        {
            public static string ToString(Bezier4x1 curve) => $"Bezier4x1({curve.a}, {curve.b}, {curve.c}, {curve.d})";
            public static string ToString(Bezier4x2 curve) => $"Bezier4x2({ToStringInternal(curve.a)}, {ToStringInternal(curve.b)}, {ToStringInternal(curve.c)}, {ToStringInternal(curve.d)})";
            public static string ToString(Bezier4x3 curve) => $"Bezier4x3({ToStringInternal(curve.a)}, {ToStringInternal(curve.b)}, {ToStringInternal(curve.c)}, {ToStringInternal(curve.d)})";
            public static string ToString(Bounds1 bounds) => $"Bounds1({bounds.min}, {bounds.max})";
            public static string ToString(Bounds2 bounds) => $"Bounds2({ToStringInternal(bounds.min)}, {ToStringInternal(bounds.max)})";
            public static string ToString(Bounds3 bounds) => $"Bounds3({ToStringInternal(bounds.min)}, {ToStringInternal(bounds.max)})";
            public static string ToString(Box3 box) => $"Box3({{B = {ToStringInternal(box.bounds)}}}, {{R = {ToStringInternal(box.rotation)}}})";
            public static string ToString(Circle2 circle) => $"Circle2({{O = {ToStringInternal(circle.position)}}}, {{r = {circle.radius}}})";
            public static string ToString(Circle3 circle) => $"Circle3({{O = {ToStringInternal(circle.position)}}}, {{r = {circle.radius}}}, {{R = {ToStringInternal(circle.rotation)}}})";
            public static string ToString(Line1 line) => $"Line1({line.a}, {line.b})";
            public static string ToString(Line1.Segment segment) => $"Segment1({segment.a}, {segment.b})";
            public static string ToString(Line2 line) => $"Line2({ToStringInternal(line.a)}, {ToStringInternal(line.b)})";
            public static string ToString(Line2.Segment segment) => $"Segment2({ToStringInternal(segment.a)}, {ToStringInternal(segment.b)})";
            public static string ToString(Line3 line) => $"Line3({ToStringInternal(line.a)}, {ToStringInternal(line.b)})";
            public static string ToString(Line3.Segment segment) => $"Segment3({ToStringInternal(segment.a)}, {ToStringInternal(segment.b)})";
            public static string ToString(Quad2 quad) => $"Quad2({ToStringInternal(quad.a)}, {ToStringInternal(quad.b)}, {ToStringInternal(quad.c)}, {ToStringInternal(quad.d)})";
            public static string ToString(Quad3 quad) => $"Quad3({ToStringInternal(quad.a)}, {ToStringInternal(quad.b)}, {ToStringInternal(quad.c)}, {ToStringInternal(quad.d)})";

            private static string ToStringInternal(Bounds1 bounds) => $"<{bounds.min}, {bounds.max}>";
            private static string ToStringInternal(Bounds2 bounds) => $"<{ToStringInternal(bounds.min)}, {ToStringInternal(bounds.max)}>";
            private static string ToStringInternal(Bounds3 bounds) => $"<{ToStringInternal(bounds.min)}, {ToStringInternal(bounds.max)}>";
            private static string ToStringInternal(float2 point) => $"({point.x} {point.y})";
            private static string ToStringInternal(float3 point) => $"({point.x} {point.y} {point.z})";
            private static string ToStringInternal(float4 point) => $"({point.x} {point.y} {point.z} {point.w})";
            private static string ToStringInternal(quaternion quaternion) => $"[{quaternion.value.x} {quaternion.value.y} {quaternion.value.z} {quaternion.value.w}]";
        }
        
        /// <summary>
        /// The definition of the real roots of a cubic equation.
        /// （一元三次方程式實根的定義。）
        /// </summary>
        public struct Roots
        {
            /// <summary>
            /// The maximum acceptable difference to be considered 0.
            /// （被視為 0 的最大可接受差距值。）
            /// </summary>
            public static readonly double Epsilon = 1E-10;
            
            /// <summary>
            /// The first real root.（第一實根。）
            /// </summary>
            private double a;

            /// <summary>
            /// The second real root.（第二實根。）
            /// </summary>
            private double b;

            /// <summary>
            /// The third real root.（第三實根。）
            /// </summary>
            private double c;

            /// <summary>
            /// The number of real roots.（實根的數量。）
            /// </summary>
            public int count;

            public readonly double this[int index]
            {
                get
                {
                    return index switch
                    {
                        0 => a,
                        1 => b,
                        2 => c,
                        _ => throw new IndexOutOfRangeException("There are at most 3 real roots. 僅至多 3 個實根。")
                    };
                }
            }

            /// <summary>
            /// Add a real root value.（添加一個實根值。）
            /// </summary>
            /// <param name="root">The real root value.（實根值。）</param>
            private void Add(double root)
            {
                if (count >= 3) return;
                
                switch (count)
                {
                    case 0:
                        a = root;
                        break;

                    case 1:
                        b = root;
                        break;

                    case 2:
                        c = root;
                        break;
                }

                count++;
            }

            /// <summary>
            /// Solve a cubic equation.（解一元三次方程式。）
            /// </summary>
            /// <param name="a">The coefficient of x³.（x³ 的係數。）</param>
            /// <param name="b">The coefficient of x².（x² 的係數。）</param>
            /// <param name="c">The coefficient of x.（x 的係數。）</param>
            /// <param name="d">The constant.（常數。）</param>
            /// <returns>The real roots of the equation.（方程式的實數根。）</returns>
            public static Roots Solve(double a, double b, double c, double d)
            {
                Roots roots = new();
                if (math.abs(a) < Epsilon)
                {
                    if (math.abs(b) < Epsilon)
                    {
                        if (c == 0)
                        {
                            // Infinity / no solution.（無限多組／無解。）
                            // d = 0
                        }
                        else
                        {
                            // Linear equation.（一元一次方程式。）
                            // cx + d = 0, c ≠ 0

                            roots.Add(-d / c);
                        }
                    }
                    else
                    {
                        // Quadratic equation.（一元二次方程式。）
                        // bx² + cx + d = 0, b ≠ 0

                        double disc = c * c - 4 * b * d;
                        if (disc > 0)
                        {
                            roots.Add((-c - math.sqrt(disc)) / (2 * b));
                            roots.Add((-c + math.sqrt(disc)) / (2 * b));
                        }
                        else if (math.abs(disc) < Epsilon)
                        {
                            roots.Add(-c / (2 * b));
                        }
                    }
                }
                else
                {
                    // Cubic equation.（一元三次方程式。）
                    // ax³ + bx² + cx + d = 0, a ≠ 0

                    double cA = b / a;
                    double cB = c / a;
                    double cC = d / a;
                    double p = cB - cA * cA / 3d;
                    double q = 2d * cA * cA * cA / 27d - cA * cB / 3d + cC;

                    // Now solve for:（現在改解：）
                    // t³ + pt + q = 0

                    double disc = (q * q / 4d) + (p * p * p / 27d);

                    if (disc > 0)
                    {
                        // One real root.（一個實根。）
                        double sqrtD = math.sqrt(disc);
                        double u = Math.Cbrt(-q / 2d + sqrtD);
                        double v = Math.Cbrt(-q / 2d - sqrtD);
                        roots.Add(u + v - cA / 3d);
                    }
                    else if (math.abs(disc) < Epsilon)
                    {
                        if (math.abs(q) < Epsilon)
                        {
                            // One (actually triple) real root.（一個（三重）實根。）
                            roots.Add(-cA / 3d);
                        }
                        else
                        {
                            // Two real roots.（兩個實根。）
                            double u = Math.Cbrt(-q / 2d);
                            roots.Add(2 * u - cA / 3d);
                            roots.Add(-u - cA / 3d);
                        }
                    }
                    else
                    {
                        // Three real roots.（三個實根。）
                        double phi = math.acos(-q / (2 * math.sqrt(-p * p * p / 27d)));
                        double t1 = 2 * math.sqrt(-p / 3d) * math.cos(phi / 3d);
                        double t2 = 2 * math.sqrt(-p / 3d) * math.cos((phi + 2 * math.PI) / 3d);
                        double t3 = 2 * math.sqrt(-p / 3d) * math.cos((phi + 4 * math.PI) / 3d);
                        roots.Add(t1 - cA / 3d);
                        roots.Add(t2 - cA / 3d);
                        roots.Add(t3 - cA / 3d);
                    }
                }

                return roots;
            }

            public override readonly string ToString()
            {
                return count switch
                {
                    0 => "Roots(No roots)",
                    1 => $"Roots({a})",
                    2 => $"Roots({a}, {b})",
                    3 => $"Roots({a}, {b}, {c})",
                    _ => "Roots(Invalid)",
                };
            }
        }

        /// <summary>
        /// The inverse hyperbolic cosine.（逆雙曲餘弦函數。）
        /// </summary>
        /// <param name="x">The input number.（輸入的數字。）</param>
        /// <returns>The original angle.（原始角度。）</returns>
        public static double Acosh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x - 1));
        }

        /// <summary>
        /// Align a bezier curve to the X axis on the XZ plane.
        /// （將一個貝茲曲線對齊至 XZ 平面上的 X 軸。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <param name="transform">The transform value.（位移量。）</param>
        /// <param name="rotation">The rotation angle in radians.（以弳度計算的角度。）</param>
        /// <returns>The aligned curve.（對齊的曲線。）</returns>
        public static Bezier4x3 AlignXAxis(Bezier4x3 curve, out float2 transform, out double rotation)
        {
            transform = -curve.a.xz;
            Bezier4x3 shiftedCurve = Shift(curve, transform);
            rotation = -RotationAngle(new(1, 0), new(shiftedCurve.d.xz));
            return Rotate(shiftedCurve, default, rotation);
        }

        /// <summary>
        /// The inverse hyperbolic sine.（逆雙曲正弦函數。）
        /// </summary>
        /// <param name="x">The input number.（輸入的數字。）</param>
        /// <returns>The original angle.（原始角度。）</returns>
        public static double Asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x + 1));
        }

        /// <summary>
        /// The inverse hyperbolic tangent.（逆雙曲正切函數。）
        /// </summary>
        /// <param name="x">The input number.（輸入的數字。）</param>
        /// <returns>The original angle.（原始角度。）</returns>
        public static double Atanh(double x)
        {
            return Math.Log((1 + x) / (1 - x)) / 2;
        }

        /// <summary>
        /// Construct a tight bounding box for the bezier curve.
        /// （為貝茲曲線建構一個緊密的定界框。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <returns>The bounding box.（定界框。）</returns>
        public static Bounds2 Bounds(Bezier4x3 curve)
        {
            float2 a = curve.a.xz;
            float2 d = curve.d.xz;
            Bounds2 bounds = new(math.min(a, d), math.max(a, d));

            /*
                #  References: （資料來源：）

                * Pomax. (2025). A Primer on Bézier Curves. Finding extremities: root finding
                    https://pomax.github.io/bezierinfo/#extremities
             */

            Roots xRoots = Extrema(curve.x);
            for (int i = 0; i < xRoots.count; i++)
            {
                float xT = (float)xRoots[i];
                if ((xT < 0) || (xT > 1)) continue;
                bounds = Merge(bounds, Colossal.Mathematics.MathUtils.Position(curve, xT));
            }

            Roots yRoots = Extrema(curve.z);
            for (int j = 0; j < yRoots.count; j++)
            {
                float yT = (float)yRoots[j];
                if ((yT < 0) || (yT > 1)) continue;
                bounds = Merge(bounds, Colossal.Mathematics.MathUtils.Position(curve, yT));
            }

            return bounds;
        }

        /// <summary>
        /// Calculate the number of points required to interpolate a bezier curve.
        /// （計算內插一個貝茲曲線所需的點數。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <param name="threshold">The maximum distance for a curve to be considered a straight line.（一條曲線可被視為直線的最大距離。）</param>
        /// <param name="gap">The interpolation gap.（內插間隔。）</param>
        /// <returns>The number of points required.（所需的點數。）</returns>
        public static int CountInterpolationPoints(Bezier4x3 curve, float threshold = 1f, float gap = 2f)
        {
            if (IsStraightLine(curve, threshold))
            {
                return 2;
            }
            else
            {
                float length = Colossal.Mathematics.MathUtils.Length(curve);
                int sections = (int) Math.Ceiling(length / gap);
                return sections < 1 ? 2 : sections + 1;
            }
        }

        /// <summary>
        /// Count the number of points required to interpolate a circle with the maximum difference distance under the threshold.
        /// （計算內插一個圓所需的點數，其中內插結果與圓弧的最大距離不超過設定的閾值。）
        /// </summary>
        /// <param name="radius">The radius of the circle.（圓的半徑。）</param>
        /// <param name="threshold">The maximum distance between interpolation result and the arc.（圓弧與內插結果間的最大距離。）</param>
        /// <returns>The number of points required.（所需的點數。）</returns>
        public static int CountInterpolationPoints(float radius, float threshold = 0.2f)
        {
            /*
                Given a circular arc with radius `r` and the maximum distance between the chord with same endpoints
                as the arc `d`, the central angle `θ` is defined as 4 * arcsin(sqrt(d / 2r)). This could be found by
                writing `d` as r * versin(θ/2), or 2r * sin^2(θ/4).
                （給定一個半徑為`r`的圓弧 和 弧與共享端點的弦之間的最大距離`d`，其圓心角`θ`被定義為 4 * arcsin(sqrt(d / 2r))。
                　這可以由`d`推導出來：r * versin(θ/2)，或是 2r * sin^2(θ/4)。）
            */

            float angle = 4 * math.asin(math.sqrt(threshold / 2 / radius));
            int pointsCount = (int)math.ceil(2 * math.PI_DBL / angle);
            return pointsCount > 6 ? pointsCount : 6;
        }

        /// <summary>
        /// The helper function to solve the first derivative of bezier curve component functions.
        /// （尋找貝茲曲線分量函數一階導數解的輔助函數。）
        /// </summary>
        /// <param name="curve">The one dimension bezier curve.（一維貝茲曲線。）</param>
        /// <returns>The real roots.（實數根。）</returns>
        private static Roots Extrema(Bezier4x1 curve)
        {
            double cA = 3 * (-curve.a + 3 * curve.b - 3 * curve.c + curve.d);
            double cB = 6 * (curve.a - 2 * curve.b + curve.c);
            double cC = 3 * (curve.b - curve.a);
            return Roots.Solve(0, cA, cB, cC);
        }

        /// <summary>
        /// Retrieve the digit count of the number.
        /// （獲得數字的位數。）
        /// </summary>
        /// <param name="number">The input number.（輸入的數字。）</param>
        /// <param name="decimalDigits">The number of place after the period.（小數點後的位數。）</param>
        /// <param name="countSign">Whether to count the sign as a digit.（是否要將負號視為一位。）</param>
        /// <param name="countSign">Whether to count the period as a digit.（是否要將小數點視為一位。）</param>
        /// <returns>The number of digits.（數字的位數。）</returns>
        public static int GetDigits(float number, out int decimalDigits, bool countSign = false,  bool countPoint = false)
        {
            string[] parts = number.ToString("G", CultureInfo.InvariantCulture).Split('.');
            int total = parts[0].Length;
            decimalDigits = 0;

            if (!countSign && parts[0].StartsWith("-"))
            {
                total--;
            }

            if (parts.Length > 1)
            {
                decimalDigits = parts[1].Length;
                total += decimalDigits;
                if (countPoint) total++;
            }

            return total;
        }
        
        /// <summary>
        /// Retrieve the digit count of the number.
        /// （獲得數字的位數。）
        /// </summary>
        /// <param name="number">The input number.（輸入的數字。）</param>
        /// <param name="countSign">Whether to count the sign as a digit.（是否要將負號視為一位。）</param>
        /// <returns>The number of digits.（數字的位數。）</returns>
        public static int GetDigits(int number, bool countSign = false)
        {
            if (number >= 0)
            {
                return number switch
                {
                    > 999999999 => 10,
                    > 99999999 => 9,
                    > 9999999 => 8,
                    > 999999 => 7,
                    > 99999 => 6,
                    > 9999 => 5,
                    > 999 => 4,
                    > 99 => 3,
                    > 9 => 2,
                    _ => 1,
                };
            }
            else
            {
                return number switch
                {
                    < -999999999 => countSign ? 11 : 10,
                    < -99999999 => countSign ? 10 : 9,
                    < -9999999 => countSign ? 9 : 8,
                    < -999999 => countSign ? 8 : 7,
                    < -99999 => countSign ? 7 : 6,
                    < -9999 => countSign ? 6 : 5,
                    < -999 => countSign ? 5 : 4,
                    < -99 => countSign ? 4 : 3,
                    < -9 => countSign ? 3 : 2,
                    _ => countSign ? 2 : 1,
                };
            }
        }

        /// <summary>
        /// Retrieve the digit count of the number.
        /// （獲得數字的位數。）
        /// </summary>
        /// <param name="number">The input number.（輸入的數字。）</param>
        /// <param name="decimalDigits">The number of place after the period.（小數點後的位數。）</param>
        /// <param name="countSign">Whether to count the sign as a digit.（是否要將負號視為一位。）</param>
        /// <param name="countSign">Whether to count the period as a digit.（是否要將小數點視為一位。）</param>
        /// <returns>The number of digits.（數字的位數。）</returns>
        public static int GetDigitsBurstCompatible(float number, out int decimalDigits, bool countSign = false, bool countPoint = false)
        {
            float decimalParts = Math.Abs(number);
            int total = GetDigits((int)decimalParts) + (countSign && (number < 0) ? 1 : 0);
            decimalParts -= (int)decimalParts;
            decimalDigits = 0;
            int trailingZeros = 0;
            while (decimalDigits < 7 && Math.Round(decimalParts, 4) > 0)
            {
                decimalParts *= 10;
                if ((int)decimalParts == 0)
                {
                    trailingZeros++;
                }
                else
                {
                    decimalDigits += 1 + trailingZeros;
                    trailingZeros = 0;
                }

                total++;
                decimalParts -= (int)decimalParts;
            }
            return total + (countPoint && (decimalDigits > 0) ? 1 : 0);
        }

        /// <summary>
        /// Computes the hypotenuse given two values representing the lengths of the shorter sides in a right-angled triangle.
        /// （計算給定兩個較短邊長時，直角三角形中斜邊的長度。）
        /// </summary>
        /// <param name="x">The value to square and add to <paramref name="y"/>.（要平方並且與  <paramref name="y"/> 相加的數。）</param>
        /// <param name="y">The value to square and add to <paramref name="x"/>.（要平方並且與  <paramref name="x"/> 相加的數。）</param>
        /// <returns>The square root of <paramref name="x"/>-squared plus <paramref name="y"/>-squared.（<paramref name="x"/> 平方與 <paramref name="y"/> 平方的總和平方根。）</returns>
        public static double Hypot(double x, double y)
        {
            /*
                 #  References: （資料來源：）

                    * Microsoft. (2025). Single.cs. Hypot()
                        https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Single.cs#L1507
             */

            double result;
            if (!double.IsInfinity(x) && !double.IsInfinity(y))
            {
                double ax = Math.Abs(x);
                double ay = Math.Abs(y);

                if (ax == 0.0f)
                {
                    result = ay;
                }
                else if (ay == 0.0)
                {
                    result = ax;
                }
                else
                {
                    double xx = ax;
                    xx *= xx;

                    double yy = ay;
                    yy *= yy;

                    result = Math.Sqrt(xx + yy);
                }
            }
            else if (double.IsInfinity(x) || double.IsInfinity(y))
            {
                result = double.PositiveInfinity;
            }
            else
            {
                result = double.NaN;
            }

            return result;
        }

        /// <summary>
        /// Interpolates a bezier curve.
        /// （內插一個貝茲曲線。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <param name="pointsList">The output point list.（輸出的點列表。）</param>
        /// <param name="threshold">The threshold to determine whether the curve is straight or not.（用於決定曲線是否為直線的閾值。）</param>
        /// <param name="gap">The interpolation gap.（內插間隔。）</param>
        /// <param name="last">Whether to include the last point of the interpolation.（是否要納入內插的最後一點。）</param>
        /// <param name="lastPoint">The last point of the interpolation.（內插的最後一點。）</param>
        /// <param name="center">The map center coordinate.（地圖中心坐標。）</param>
        /// <param name="sourceCRS">The CRS of the original coordinate.（轉換前坐標的坐標參考系統。）</param>
        /// <param name="targetCRS">>The CRS of the converted coordinate.（轉換後坐標的坐標參考系統。）</param>
        /// <param name="sourceProjection">The source custom Transverse Mercator projection.（使用者自訂的來源橫麥卡托投影。）</param>
        /// <param name="targetProjection">The target custom Transverse Mercator projection.（使用者自訂的目標橫麥卡托投影。）</param>
        public static void Interpolate(Bezier4x3 curve, ref NativeList<double3> pointsList, float threshold, float gap, bool last, out double3 lastPoint,
                                       Coord center, CRS sourceCRS, CRS targetCRS, ProjectionDefinition sourceProjection, ProjectionDefinition targetProjection)
        {
            lastPoint = default;
            if (!pointsList.IsCreated) return;
            if (IsStraightLine(curve, threshold))
            {
                pointsList.Add(Transform.Apply(center.Shift(curve.a.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3());
                lastPoint = Transform.Apply(center.Shift(curve.d.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();
                if (last) pointsList.AddNoResize(lastPoint);
            }
            else
            {
                float length = Colossal.Mathematics.MathUtils.Length(curve);
                int sections = (int)Math.Ceiling(length / gap);
                float3 previous = curve.a;
                for (int i = 0; i <= sections + 1; i++)
                {
                    float3 point = Colossal.Mathematics.MathUtils.Position(curve, (float)i / (sections + 1));
                    double3 transformedPoint = Transform.Apply(center.Shift(point.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();

                    if (i == 0)
                    {
                        pointsList.AddNoResize(transformedPoint);
                        continue;
                    }

                    if (i == sections + 1)
                    {
                        lastPoint = transformedPoint;
                        if (last) pointsList.AddNoResize(transformedPoint);
                    }
                    else if (!previous.Equals(point))
                    {
                        pointsList.AddNoResize(transformedPoint);
                        previous = point;
                    }
                }
            }
        }

        /// <summary>
        /// Check whether the corner of <paramref name="quad"/> is in counterclockwise order.
        /// （確認 <paramref name="quad"/> 的角落是否以逆時針順序排列。）
        /// </summary>
        /// <param name="quad">The input polygon.（輸入多邊形。）</param>
        /// <returns>True if the order is counterclockwise.（若為真，則其順序為逆時針。）</returns>
        public static bool IsCounterclockwise(Quad3 quad)
        {
            float3 a = quad.a;
            float3 b = quad.b;
            float3 c = quad.c;
            float3 d = quad.d;

            /*
                Utilize the Shoelace formula to calculate the signed area of a simple polygon (a polygon that has no holes and doesn't intersect itself.)
                If the area is positive, the link a -> b -> c -> d -> a is a clockwise ring; otherwise, the link is in counterclockwise order.
                （使用測量員公式計算簡單多邊形（沒有洞，且不和自身相交的多邊形）帶正負號的面積。
                  如果面積是正數，表示 a -> b -> c -> d -> a 的連結為順時針方向的環，反之則為逆時鐘順序。）
             */
            float area = (b.x - a.x) * (b.z + a.z) + (c.x - b.x) * (c.z + b.z) + (d.x - c.x) * (d.z + c.z) + (a.x - d.x) * (a.z + d.z);
            return area < 0;
        }

        /// <summary>
        /// Check whether the bezier curve can be considered a straight line.
        /// （確認 <paramref name="curve"/> 是否可被視為直線。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <param name="threshold">The threshold to determine whether the curve is straight or not.（用於決定曲線是否為直線的閾值。）</param>
        /// <returns>True if the maximum distance between the curve and the line is smaller than <paramref name="threshold"/>.<br/>（若為真，則曲線和直線的最大距離小於 <paramref name="threshold"/>。）</returns>
        public static bool IsStraightLine(Bezier4x3 curve, float threshold)
        {
            // Old algorithm from version 0.1（來自版本 0.1 的舊演算法。）
            Line3.Segment straight = Colossal.Mathematics.MathUtils.Line(curve);
            if ((Colossal.Mathematics.MathUtils.Distance(straight.xz, curve.b.xz, out _) <= threshold) & (Colossal.Mathematics.MathUtils.Distance(straight.xz, curve.c.xz, out _) <= threshold))
            {
                return true;
            }

            // New algorithm, too sensitive（新演算法，太敏感了。）
            Bounds1 yValues = Bounds(AlignXAxis(curve, out _, out _)).y;
            return math.max(math.abs(yValues.min), math.abs(yValues.max)) <= threshold;
        }

        /// <summary>
        /// Merge a point into the bounding box.
        /// （將一個點合併至定界框。）
        /// </summary>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        /// <param name="point">The input point.（輸入點。）</param>
        /// <returns>The updated bounding box.（更新的定界框。）</returns>
        public static Bounds2 Merge(Bounds2 bounds, float3 point) => new(math.min(bounds.min, point.xz), math.max(bounds.max, point.xz));

        /// <summary>
        /// Rotate a bezier curve around a defined center by given angle on the XZ plane.
        /// （在 XZ 平面上根據既定的中心，將一個貝茲曲線旋轉指定的角度。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <param name="center">>The rotation center.（旋轉中心。）</param>
        /// <param name="angle">The rotation angle in radians.（以弳度計算的角度。）</param>
        /// <returns>The rotated curve.（已旋轉的貝茲曲線。）</returns>
        public static Bezier4x3 Rotate(Bezier4x3 curve, float3 center, double angle)
        {
            return new((float3)Rotate(curve.a, center, angle),
                       (float3)Rotate(curve.b, center, angle),
                       (float3)Rotate(curve.c, center, angle),
                       (float3)Rotate(curve.d, center, angle));
        }

        /// <summary>
        /// Rotate a 2D point around a defined center by given angle.
        /// （根據既定的中心，將一個 2D 點旋轉指定的角度。）
        /// </summary>
        /// <param name="point">The input point.（輸入的點。）</param>
        /// <param name="center">The rotation center.（旋轉中心。）</param>
        /// <param name="angle">The rotation angle in radians.（以弳度計算的角度。）</param>
        /// <returns>The rotated point.（已旋轉的點。）</returns>
        public static double2 Rotate(double2 point, double2 center, double angle)
        {
            double2 difference = point - center;
            double cosAngle = math.cos(angle);
            double sinAngle = math.sin(angle);
            return new double2(difference.x * cosAngle - difference.y * sinAngle,
                               difference.x * sinAngle + difference.y * cosAngle) + center;
        }

        /// <summary>
        /// Rotate a 3D point around a defined center by given angle on the XZ plane.
        /// （在 XZ 平面上根據既定的中心，將一個 3D 點旋轉指定的角度。）
        /// </summary>
        /// <param name="point">The input point.（輸入的點。）</param>
        /// <param name="center">The rotation center.（旋轉中心。）</param>
        /// <param name="angle">The rotation angle in radians.（以弳度計算的角度。）</param>
        /// <returns>The rotated point.（已旋轉的點。）</returns>
        public static double3 Rotate(double3 point, double3 center, double angle)
        {
            double2 point2D = Rotate(point.xz, center.xz, angle);
            return new(point2D.x, point.y, point2D.y);
        }

        /// <summary>
        /// Find the left-handed rotation angle from <paramref name="fromVector"/> to <paramref name="toVector"/>.
        /// （找出 <paramref name="fromVector"/> 向量至 <paramref name="toVector"/> 向量的逆時鐘方向夾角。）
        /// </summary>
        /// <param name="fromVector">The reference vector.（參考向量。）</param>
        /// <param name="toVector">The vector pending evaluation.（待評估的向量。）</param>
        public static double RotationAngle(double2 fromVector, double2 toVector)
        {
            double3 fromVector3 = new(fromVector, 0);
            double3 toVector3 = new(toVector, 0);
            double3 normal = math.cross(fromVector3, toVector3);
            double3 normalStdVector = math.select(-1, 1, normal.y >= 0) * normal / math.length(normal);
            return math.atan2(math.dot(normal, normalStdVector), math.dot(fromVector, toVector));
        }

        /// <summary>
        /// Shift a bezier curve.
        /// （將貝茲曲線平移。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <param name="transform">The transform value.（位移量。）</param>
        /// <returns>The transformed bezier curve.（平移後的貝茲曲線。）</returns>
        public static Bezier4x3 Shift(Bezier4x3 curve, float2 transform)
        {
            float2 a = curve.a.xz + transform;
            float2 b = curve.b.xz + transform;
            float2 c = curve.c.xz + transform;
            float2 d = curve.d.xz + transform;
            return new(new(a.x, curve.a.y, a.y), new(b.x, curve.b.y, b.y), new(c.x, curve.c.y, c.y), new(d.x, curve.d.y, d.y));
        }

        /// <summary>
        /// Trim the Bezier curve to a new curve between <paramref name="t0"/> &amp; <paramref name="t1"/>.
        /// （將貝茲曲線裁剪為起終點位於<paramref name="t0"/>和<paramref name="t1"/>的新曲線。）
        /// </summary>
        /// <param name="curve">The input curve.（輸入的曲線。）</param>
        /// <param name="t0">The start location.（起點位置。）</param>
        /// <param name="t1">The end location.（終點位置。）</param>
        public static Bezier4x3 Trim(Bezier4x3 curve, float t0, float t1)
        {
            /*
                # References: （資料來源：）

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