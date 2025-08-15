using System;
using Unity.Mathematics;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of the Helmert Transform matrix.
    /// （赫爾默特轉換矩陣的定義。）
    /// </summary>
    public struct HelmertTransform
    {
        /// <summary>
        /// The shift on the X axis.（X 軸上的平移量。）
        /// </summary>
        public double x;

        /// <summary>
        /// The shift on the Y axis.（Y 軸上的平移量。）
        /// </summary>
        public double y;

        /// <summary>
        /// The shift on the Z axis.（Z 軸上的平移量。）
        /// </summary>
        public double z;

        /// <summary>
        /// The rotation on the X axis.（X 軸上的旋轉量。）
        /// </summary>
        public double rx;

        /// <summary>
        /// The rotation on the Y axis.（Y 軸上的旋轉量。）
        /// </summary>
        public double ry;

        /// <summary>
        /// The rotation on the Z axis.（Z 軸上的旋轉量。）
        /// </summary>
        public double rz;

        /// <summary>
        /// The scale factor.（縮放係數。）
        /// </summary>
        public double s;

        /// <summary>
        /// The number of parameters in the matrix.（矩陣內的參數數量。）
        /// </summary>
        public int paramCount;

        public HelmertTransform(double[] param)
        {
            x = 0;
            y = 0;
            z = 0;
            rx = 0;
            ry = 0;
            rz = 0;
            s = 0;
            paramCount = param.Length;

            if ((paramCount != 0) && (paramCount != 3) && (paramCount != 7))
            {
                throw new ArgumentException("The number of Helmert Transform parameters should be either 0, 3, or 7. 赫爾默特轉換參數的數量應為 0、3 或 7 個。");
            }

            if (paramCount > 0)
            {
                x = param[0];
                y = param[1];
                z = param[2];
            }

            if (paramCount > 3)
            {
                rx = param[3];
                ry = param[4];
                rz = param[5];
                s = param[6];
            }
        }

        public double this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    3 => rx,
                    4 => ry,
                    5 => rz,
                    6 => s,
                    _ => throw new IndexOutOfRangeException("The Helmert Transform has 7 values at most. 赫爾默特轉換參數僅至多 7 個參數。")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: rx = value; break;
                    case 4: ry = value; break;
                    case 5: rz = value; break;
                    case 6: s = value; break;
                    default:
                        throw new IndexOutOfRangeException("The Helmert Transform has 7 values at most. 赫爾默特轉換參數僅至多 7 個參數。");
                }
            }
        }

        /// <summary>
        /// Revert the Helmert Transform to convert the WGS84-geocentric coordinate to any geocentric coordinate.
        /// （以逆赫爾默特轉換參數將 WGS84 的地心坐標轉換為任意地心坐標。）
        /// </summary>
        /// <param name="coord">The input coordinate.（輸入的坐標。）</param>
        /// <returns>The transformed coordinate.（轉換後的坐標。）</returns>
        public readonly Coord ConvertFromWGS84(Coord coord)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). helmert.cpp. helmert_reverse_3d()
                    https://github.com/OSGeo/PROJ/blob/master/src/transformations/helmert.cpp#L402
             */

            switch (paramCount)
            {
                case 0:
                    return coord;

                case 3:
                    return new(coord.x - x, coord.y - y, coord.z - z, CRS.Unknown);

                default:
                    double m = s / 1E6 + 1;
                    double x0 = (coord.x - x) / m;
                    double y0 = (coord.y - y) / m;
                    double z0 = (coord.z - z) / m;
                    double rx0 = math.radians(rx) / 3600;
                    double ry0 = math.radians(ry) / 3600;
                    double rz0 = math.radians(rz) / 3600;
                    return new(x0 + rz0 * y0 - ry0 * z0, -rz0 * x0 + y0 + rx0 * z0, ry0 * x0 - rx0 * y0 + z0, CRS.Unknown);
            }
        }

        /// <summary>
        /// Apply the Helmert Transform to convert any geocentric coordinate to the WGS84-geocentric coordinate.
        /// （以赫爾默特轉換參數將任意地心坐標轉換為 WGS84 的地心坐標。）
        /// </summary>
        /// <param name="coord">The input coordinate.（輸入的坐標。）</param>
        /// <returns>The transformed coordinate.（轉換後的坐標。）</returns>
        public readonly Coord ConvertToWGS84(Coord coord)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). helmert.cpp. helmert_forward_3d()
                    https://github.com/OSGeo/PROJ/blob/master/src/transformations/helmert.cpp#L362
             */

            switch (paramCount)
            {
                case 0:
                    return coord;

                case 3:
                    return new(coord.x + x, coord.y + y, coord.z + z, CRS.Unknown);

                default:
                    double m = s / 1E6 + 1;
                    double rx0 = math.radians(rx) / 3600;
                    double ry0 = math.radians(ry) / 3600;
                    double rz0 = math.radians(rz) / 3600;
                    return new(m * (coord.x - rz0 * coord.y + ry0 * coord.z) + x, m * (rz0 * coord.x + coord.y - rx0 * coord.z) + y, m * (-ry0 * coord.x + rx0 * coord.y + coord.z) + z, CRS.Unknown);
            }
        }

        /// <summary>
        /// Retrieve the array representation of the parameters.
        /// （取得參數的陣列表示形式。）
        /// </summary>
        /// <returns>The array with the length <see cref="paramCount"/>.（長度為 <see cref="paramCount"/> 的陣列。）</returns>
        public readonly double[] ToArray()
        {
            return paramCount switch
            {
                0 => new double[0],
                3 => new double[] { x, y, z },
                _ => new double[] { x, y, z, rx, ry, rz, s }
            };
        }

        public override readonly string ToString()
        {
            return $"Helmert({x}, {y}, {z}, {rx}, {ry}, {rz}, {s})";
        }
    }
}