using Carto.Utils;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace Carto.Geodata
{    
    /// <summary>
    /// The definition of the auxiliary latitude coefficients.
    /// （輔助緯度係數的定義。）
    /// </summary>
    public struct Coefficients
    {
        /// <summary>
        /// The first number of the coefficients.
        /// （第一個係數。）
        /// </summary>
        public double a;

        /// <summary>
        /// The second number of the coefficients.
        /// （第二個係數。）
        /// </summary>
        public double b;

        /// <summary>
        /// The third number of the coefficients.
        /// （第三個係數。）
        /// </summary>
        public double c;

        /// <summary>
        /// The fourth number of the coefficients.
        /// （第四個係數。）
        /// </summary>
        public double d;

        /// <summary>
        /// The fifth number of the coefficients.
        /// （第五個係數。）
        /// </summary>
        public double e;

        /// <summary>
        /// The sixth number of the coefficients.
        /// （第六個係數。）
        /// </summary>
        public double f;

        public double this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => a,
                    1 => b,
                    2 => c,
                    3 => d,
                    4 => e,
                    5 => f,
                    _ => throw new IndexOutOfRangeException("The auxilary latitude coefficient has only 6 values. 輔助緯度係數僅有 6 個參數。")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: a = value; break;
                    case 1: b = value; break;
                    case 2: c = value; break;
                    case 3: d = value; break;
                    case 4: e = value; break;
                    case 5: f = value; break;
                    default:
                        throw new IndexOutOfRangeException("The auxilary latitude coefficient has only 6 values. 輔助緯度係數僅有 6 個參數。");
                }
            }
        }

        public override readonly string ToString()
        {
            return $"Coefficients({a}, {b}, {c}, {d}, {e}, {f})";
        }
    }

    /// <summary>
    /// The definition of a coordinate.
    /// （坐標的定義。）
    /// </summary>
    public struct BurstCompatibleCoord
    {
        /// <summary>
        /// The type of CRS of the coordinate.（坐標的坐標參考系統類別。）
        /// </summary>
        public CRS crs;

        /// <summary>
        /// The x value.
        /// （x 值。）
        /// </summary>
        public double x;

        /// <summary>
        /// The y value.
        /// （y 值。）
        /// </summary>
        public double y;

        /// <summary>
        /// The z value (height).
        /// （z（高度）值。）
        /// </summary>
        public double z;

        /// <summary>
        /// The hemisphere where the coordinate located in.
        /// （坐標所在的半球。）
        /// </summary>
        private readonly Hemisphere _hemisphere;

        /// <summary>
        /// The zone number of UTM projection.
        /// （UTM 投影的區域編號。）
        /// </summary>
        private readonly int _zone;

        public BurstCompatibleCoord(double x, double y, CRS crs = CRS.Game)
        {
            this.crs = crs;
            this.x = x;
            this.y = y;
            z = 0;
            _hemisphere = (crs == CRS.WGS84) && (y < 0) ? Hemisphere.South : Hemisphere.North;
            _zone = crs == CRS.WGS84 ? (int)Math.Round(math.floor((x + 180) / 6) + 1) : 0;
        }

        public BurstCompatibleCoord(double x, double y, double z, CRS crs = CRS.Game)
        {
            this.crs = crs;
            this.x = x;
            this.y = y;
            this.z = z;
            _hemisphere = Hemisphere.North;
            _zone = 0;
        }

        public BurstCompatibleCoord(double x, double y, Hemisphere hemisphere, int zone)
        {
            crs = CRS.UTM;
            this.x = x;
            this.y = y;
            z = 0;
            _hemisphere = hemisphere;
            _zone = zone;
        }

        public BurstCompatibleCoord(double x, double y, double z, Hemisphere hemisphere, int zone)
        {
            crs = CRS.UTM;
            this.x = x;
            this.y = y;
            this.z = z;
            _hemisphere = hemisphere;
            _zone = zone;
        }

        /// <summary>
        /// The hemisphere where the coordinate located in.
        /// （坐標所在的半球。）
        /// </summary>
        public readonly Hemisphere Hemisphere
        {
            get
            {
                return crs switch
                {
                    CRS.UTM => _hemisphere,
                    CRS.WGS84 => y >= 0 ? Hemisphere.North : Hemisphere.South,
                    _ => throw new NotSupportedException("The CRS other than UTM and WGS84 is not supported. 不支援 UTM 與 WGS84 以外的坐標參考系統。")
                };
            }
        }

        /// <summary>
        /// The zone number of UTM projection.
        /// （UTM 投影的區域編號。）
        /// </summary>
        public readonly int UTMZone
        {
            get
            {
                return crs switch
                {
                    CRS.UTM => _zone,
                    CRS.WGS84 => (int)Math.Round(math.floor((x + 180) / 6) + 1),
                    _ => throw new NotSupportedException("The CRS other than UTM and WGS84 is not supported. 不支援 UTM 與 WGS84 以外的坐標參考系統。")
                };
            }
        }

        /// <summary>
        /// Shift the coordinate.（平移坐標。）
        /// </summary>
        /// <param name="shift">The shift in x and y direction.（X 與 Y 方向的平移量。）</param>
        /// <returns>The shifted coordinate.（平移後的坐標。）</returns>
        public readonly BurstCompatibleCoord Shift(float2 shift)
        {
            return new(x + shift.x, y + shift.y, z, _hemisphere, _zone);
        }

        /// <summary>
        /// Convert the coordinate to <see cref="double3"/>.
        /// （將坐標轉換為 <see cref="double3"/>。）
        /// </summary>
        /// <returns>A double3 instance.（一個 double3 實例。）</returns>
        public readonly double3 ToDouble3()
        {
            return new(x, y, z);
        }

        /// <summary>
        /// Convert the coordinate to <see cref="float3"/>.
        /// （將坐標轉換為 <see cref="float3"/>。）
        /// </summary>
        /// <returns>A float3 instance.（一個 float3 實例。）</returns>
        public readonly float3 ToFloat3()
        {
            return new((float)x, (float)y, (float)z);
        }

        public override readonly string ToString()
        {
            return $"Coord({x}, {y}, {z}) - CRS [{crs}], Hemisphere [{_hemisphere}], Zone [{_zone}]";
        }
    }

    /// <summary>
    /// The definition of an ellipsoid.
    /// （橢球體的定義。）
    /// </summary>
    public struct BurstCompatibleEllipsoidDefinition
    {
        /// <summary>
        /// The semi-major axis length of the ellipsoid in meters.（橢球體的半長軸長度，單位為公尺。）
        /// </summary>
        public double a;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the conformal latitude (χ) to the geographic latitude (φ).<br/>
        /// （用以將等角緯度（χ）轉換為地理緯度（φ）的係數。）
        /// </summary>
        public Coefficients coefficientsCG;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the conformal latitude (χ) to the rectifying latitude (μ).<br/>
        /// （用以將等角緯度（χ）轉換為矯正緯度（μ）的係數。）
        /// </summary>
        public Coefficients coefficientsCR;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the geographic latitude (φ) to the conformal latitude (χ).<br/>
        /// （用以將地理緯度（φ）轉換為等角緯度（χ）的係數。）
        /// </summary>
        public Coefficients coefficientsGC;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the rectifying latitude (μ) to the conformal latitude (χ).<br/>
        /// （用以將矯正緯度（μ）轉換為等角緯度（χ）的係數。）
        /// </summary>
        public Coefficients coefficientsRC;

        /// <summary>
        /// The semi-minor axis length of the ellipsoid in meters.（橢球體的半短軸長度，單位為公尺。）
        /// </summary>
        public double b;

        /// <summary>
        /// The flattening of the ellipsoid.（橢球體扁平率。）
        /// </summary>
        public double f;

        /// <summary>
        /// The reciprocal of the flattening of the ellipsoid; the inverse flattening.（橢球體扁平率的倒數。）
        /// </summary>
        public double rf;

        public BurstCompatibleEllipsoidDefinition(double semiMajor, double inverseFlattening, bool burst = false)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. PoderEngsager
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L35

                * PROJ contributors. (2025). tmerc.cpp. setup_exact()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L445
             */

            a = semiMajor;
            rf = inverseFlattening;
            f = 1 / rf;
            b = a * (1 - f);

            if (!burst)
            {
                double n = (a - b) / (a + b);   // The third flattening（第三扁平率。）
                coefficientsCG = BurstCompatibleDatumUtils.GetCoefficients(n, 4, 0);
                coefficientsCR = BurstCompatibleDatumUtils.GetCoefficients(n, 4, 3);
                coefficientsGC = BurstCompatibleDatumUtils.GetCoefficients(n, 0, 4);
                coefficientsRC = BurstCompatibleDatumUtils.GetCoefficients(n, 3, 4);
            }
            else
            {
                coefficientsCG = default;
                coefficientsCR = default;
                coefficientsGC = default;
                coefficientsRC = default;
            }
        }

        /// <summary>
        /// The first eccentricity squared of the ellipsoid.（橢球體的第一偏心率平方。）
        /// </summary>
        public readonly double E1Square => f * (2 - f);

        /// <summary>
        /// The second eccentricity squared of the ellipsoid.（橢球體的第二偏心率平方。）
        /// </summary>
        public readonly double E2Square => E1Square / (1 - E1Square);

        /// <summary>
        /// The third eccentricity squared of the ellipsoid.（橢球體的第三偏心率平方。）
        /// </summary>
        public readonly double E3Square => E1Square / (2 - E1Square);

        /// <summary>
        /// The second flattening of the ellipsoid.（橢球體的第二扁平率。）
        /// </summary>
        public readonly double F2 => (a - b) / b;

        /// <summary>
        /// The third flattening of the ellipsoid.（橢球體的第三扁平率。）
        /// </summary>
        public readonly double F3 => (a - b) / (a + b);

        public override readonly string ToString()
        {
            return $"Ellipsoid({a}, {rf})";
        }
    }

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

        /// <summary>
        /// Revert the Helmert Transform to convert the WGS84-geocentric coordinate to any geocentric coordinate.
        /// （以逆赫爾默特轉換參數將 WGS84 的地心坐標轉換為任意地心坐標。）
        /// </summary>
        /// <param name="coord">The input coordinate.（輸入的坐標。）</param>
        /// <returns>The transformed coordinate.（轉換後的坐標。）</returns>
        public readonly BurstCompatibleCoord ConvertFromWGS84(BurstCompatibleCoord coord)
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
        public readonly BurstCompatibleCoord ConvertToWGS84(BurstCompatibleCoord coord)
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

        public override readonly string ToString()
        {
            return $"Helmert({x}, {y}, {z}, {rx}, {ry}, {rz}, {s})";
        }
    }

    /// <summary>
    /// The definition of a projection.
    /// （投影法的定義。）
    /// </summary>
    public struct BurstCompatibleProjectionDefinition
    {
        /// <summary>
        /// The reference ellipsoid of the projection.
        /// （投影法的參考橢球體。）
        /// </summary>
        public BurstCompatibleEllipsoidDefinition ellipsoid;

        /// <summary>
        /// The meridian quadrant.
        /// （經線象限。）
        /// </summary>
        public double meridianQuadrant;

        /// <summary>
        /// The origin of the projection coordinate system in WGS84.
        /// （投影法坐標系的 WGS84 原點。）
        /// </summary>
        public double2 origin;

        /// <summary>
        /// The shift of the coordinates, and it is also known as false easting (x axis) and false northing (y axis).
        /// （坐標的位移，通常被稱為東移量（X 方向）或北移量（Y 方向）。）
        /// </summary>
        public double2 shift;

        /// <summary>
        /// The radius vector in polar coordinate systems.
        /// （極坐標系統中的半徑向量。）
        /// </summary>
        public double radiusVector;

        /// <summary>
        /// The ratio of the unit length at the origin of the projection to the true ground distance.
        /// （投影法原點的單位長度與真實地面距離的比值。）
        /// </summary>
        public double scaleFactor;

        /// <summary>
        /// The Helmert Transform parameters, and its length should be either 0, 3, or 7.
        /// （赫爾默特轉換參數，其長度應為 0、3 或 7。）
        /// </summary>
        public HelmertTransform transform;

        public BurstCompatibleProjectionDefinition(BurstCompatibleEllipsoidDefinition ellipsoid, double originX, double originY, double shiftX, double shiftY, double scaleFactor, HelmertTransform transform)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. PoderEngsager
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L35

                * PROJ contributors. (2025). tmerc.cpp. setup_exact()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L445
             */

            this.ellipsoid = ellipsoid;
            origin = new(originX, originY);
            shift = new(shiftX, shiftY);
            this.scaleFactor = scaleFactor;
            this.transform = transform;
            meridianQuadrant = scaleFactor * BurstCompatibleDatumUtils.GetRectifyingRadius(ellipsoid.F3);
            double gaussianLatitude = BurstCompatibleDatumUtils.Burst.ConvertAuxiliaryLatitude(math.radians(originY), ellipsoid.coefficientsGC);
            radiusVector = -meridianQuadrant * BurstCompatibleDatumUtils.Burst.ConvertAuxiliaryLatitude(gaussianLatitude, ellipsoid.coefficientsCR);
        }

        /// <summary>
        /// Check whether the projection has Helmert Transform parameters.
        /// （確認投影法是否有赫爾默特轉換參數。）
        /// </summary>
        public readonly bool HasTransform => transform.paramCount != 0;
    }
    
    /// <summary>
    /// The class that provides transformation between popular CRSs.
    /// （提供數種熱門坐標參考系統間轉換的類別。）
    /// </summary>
    public static class BurstCompatibleTransform
    {
        /// <summary>
        /// Convert a Transverse Mercator coordinate to a WGS84 coordinate.
        /// （將橫麥卡托投影坐標轉換為 WGS84 坐標。）
        /// </summary>
        /// <param name="tm">The Transverse Mercator coordinate.（橫麥卡托坐標。）</param>
        /// <param name="projection">The definition of the Transverse Mercator projection.（橫麥卡托投影的定義。）</param>
        /// <returns>The converted WGS84 coordinate.（轉換後的 WGS84 坐標。）</returns>
        public static BurstCompatibleCoord TransverseMercatorToWGS84(BurstCompatibleCoord tm, BurstCompatibleProjectionDefinition projection)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. exact_e_inv()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L379
             */

            // Initial values（初始值）
            double lon0 = math.radians(projection.origin.x);

            // Intermediate values（中繼值）
            double Ce = (tm.x - projection.shift.x) / projection.ellipsoid.a / projection.meridianQuadrant;
            double Cn = ((tm.y - projection.shift.y) / projection.ellipsoid.a - projection.radiusVector) / projection.meridianQuadrant;

            if (math.abs(Ce) <= 2.623395162778)
            {
                double sinArgR = math.sin(2 * Cn);
                double cosArgR = math.cos(2 * Cn);
                double exp2Ce = math.exp(2 * Ce);
                double halfInvExp2Ce = 0.5 / exp2Ce;
                double sinhArgI = 0.5 * exp2Ce - halfInvExp2Ce;
                double coshArgI = 0.5 * exp2Ce + halfInvExp2Ce;
                double r = BurstCompatibleDatumUtils.Burst.ClenshawSum(sinArgR, cosArgR, sinhArgI, coshArgI, projection.ellipsoid.coefficientsRC, out double i);

                Ce += i;
                Cn += r;

                double sinCn = math.sin(Cn);
                double cosCn = math.cos(Cn);
                double sinhCe = math.sinh(Ce);
                Ce = math.atan2(sinhCe, cosCn);
                double modulusCe = MathUtils.Hypot(sinhCe, cosCn);
                double rr = MathUtils.Hypot(sinCn, modulusCe);
                Cn = math.atan2(sinCn, modulusCe);
                double lat = math.degrees(BurstCompatibleDatumUtils.Burst.ConvertAuxiliaryLatitude(Cn, sinCn / rr, modulusCe / rr, projection.ellipsoid.coefficientsCG));
                double lon = math.degrees(Ce + lon0);
                BurstCompatibleCoord wgs84 = new(lon, lat, CRS.WGS84);

                // Datum Transformation（大地基準轉換）
                if (projection.HasTransform)
                {
                    BurstCompatibleEllipsoidDefinition wgs84Ellipsoid = new(6378137, 298.257223563, true);
                    wgs84 = BurstCompatibleDatumUtils.Burst.ConvertToGeocentric(wgs84, projection.ellipsoid);
                    wgs84 = projection.transform.ConvertToWGS84(wgs84);
                    wgs84 = BurstCompatibleDatumUtils.Burst.ConvertFromGeocentric(wgs84, wgs84Ellipsoid);
                }

                return wgs84;
            }

            return new(double.MaxValue, double.MaxValue, CRS.WGS84);
        }

        /// <summary>
        /// Convert a WGS84 coordinate to a Transverse Mercator coordinate.
        /// （將 WGS84 坐標轉換為橫麥卡托投影坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <param name="projection">The definition of the Transverse Mercator projection.（橫麥卡托投影的定義。）</param>
        /// <returns>The converted Transverse Mercator coordinate.（轉換後的橫麥卡托投影坐標。）</returns>
        public static BurstCompatibleCoord WGS84ToTransverseMercator(BurstCompatibleCoord wgs84, BurstCompatibleProjectionDefinition projection)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. exact_e_fwd()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L293
             */

            // Datum Transformation（大地基準轉換）
            if (projection.HasTransform)
            {
                BurstCompatibleEllipsoidDefinition wgs84Ellipsoid = new(6378137, 298.257223563, true);
                wgs84 = BurstCompatibleDatumUtils.Burst.ConvertToGeocentric(wgs84, wgs84Ellipsoid);
                wgs84 = projection.transform.ConvertFromWGS84(wgs84);
                wgs84 = BurstCompatibleDatumUtils.Burst.ConvertFromGeocentric(wgs84, projection.ellipsoid);
            }

            // Initial values（初始值）
            double lat = math.radians(wgs84.y);
            double lon = math.radians(wgs84.x);
            double lon0 = math.radians(projection.origin.x);

            // Intermediate values（中繼值）
            double Ce = lon - lon0;
            double Cn = BurstCompatibleDatumUtils.Burst.ConvertAuxiliaryLatitude(lat, projection.ellipsoid.coefficientsGC);
            double sinCe = math.sin(Ce);
            double cosCe = math.cos(Ce);
            double sinCn = math.sin(Cn);
            double cosCn = math.cos(Cn);
            double cosCecosCn = cosCe * cosCn;
            Cn = math.atan2(sinCn, cosCecosCn);

            double invDenomTanCe = 1.0 / MathUtils.Hypot(sinCn, cosCecosCn);
            double twoInvDenomTanCe2 = 2 * invDenomTanCe * invDenomTanCe;
            double tanCe = sinCe * cosCn * invDenomTanCe;
            Ce = MathUtils.Asinh(tanCe);

            double tmp = cosCecosCn * twoInvDenomTanCe2;
            double sinArgR = sinCn * tmp;
            double cosArgR = cosCecosCn * tmp - 1;
            double sinhArgI = 2 * tanCe * invDenomTanCe;
            double coshArgI = twoInvDenomTanCe2 - 1;
            double r = BurstCompatibleDatumUtils.Burst.ClenshawSum(sinArgR, cosArgR, sinhArgI, coshArgI, projection.ellipsoid.coefficientsCR, out double i);
            Ce += i;
            Cn += r;

            if (math.abs(Ce) <= 2.623395162778)
            {
                double x = projection.meridianQuadrant * Ce * projection.ellipsoid.a + projection.shift.x;
                double y = (projection.meridianQuadrant * Cn + projection.radiusVector) * projection.ellipsoid.a + projection.shift.y;
                return new(x, y, CRS.TransverseMercator);
            }
            else
            {
                return new(double.MaxValue, double.MaxValue, CRS.TransverseMercator);
            }
        }
    }

    /// <summary>
    /// The class that provides utility functions to calculate datum transformations.
    /// （提供計算大地基準轉換功能的類別。）<br/>
    /// Most of the codes in this class was rewritten from <seealso href="https://github.com/OSGeo/PROJ">PROJ</seealso> library.
    /// （這個類別的大部分程式碼是從 <seealso href="https://github.com/OSGeo/PROJ">PROJ</seealso> 套件改寫的。）
    /// </summary>
    public static class BurstCompatibleDatumUtils
    {
        /// <summary>
        /// The Burst-compatible methods.
        /// （適用於 Burst 編譯的方法。）
        /// </summary>
        public static class Burst
        {
            /// <summary>
            /// Calcuate the Clenshaw summation result.
            /// （計算 Clenshaw 和的結果。）
            /// </summary>
            /// <param name="sinArgR">The sine parameter of the real part.（實數部分的正弦參數。）</param>
            /// <param name="cosArgR">The cosine parameter of the real part.（實數部分的餘弦參數。）</param>
            /// <param name="sinhArgI">The sine hyperbolic parameter of the imaginary part.（虛數部分的雙曲正弦參數。）</param>
            /// <param name="coshArgI">The cosine hyperbolic parameter of the imaginary part.（虛數部分的雙曲餘弦參數。）</param>
            /// <param name="coefficients">The auxiliary latitude coefficients.（輔助緯度係數。）</param>
            /// <param name="i">The imaginary part of the result.（計算結果的虛數部分。）</param>
            /// <returns>The real part of the result.（計算結果的實數部分。）</returns>
            public static double ClenshawSum(double sinArgR, double cosArgR, double sinhArgI, double coshArgI, Coefficients coefficients, out double i)
            {
                /*
                    #  References: （資料來源：）

                    * PROJ contributors. (2025). tmerc.cpp. clenS()
                        https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L263
                 */

                double i0 = -2 * sinArgR * sinhArgI;
                double r0 = 2 * cosArgR * coshArgI;
                int p = 5;

                double hi = 0;
                double hi1 = 0;
                double hr = coefficients[p];
                double hr1 = 0;
                p--;

                while (p >= 0)
                {
                    double hi2 = hi1;
                    double hr2 = hr1;
                    hi1 = hi;
                    hr1 = hr;
                    hi = -hi2 + i0 * hr1 + r0 * hi1;
                    hr = -hr2 + r0 * hr1 - i0 * hi1 + coefficients[p];
                    p--;
                }

                double iFinal = cosArgR * sinhArgI;
                double rFinal = sinArgR * coshArgI;
                i = rFinal * hi + iFinal * hr;
                return rFinal * hr - iFinal * hi;
            }

            /// <summary>
            /// Calculate the auxiliary latitude using <see href="https://en.wikipedia.org/wiki/Clenshaw_algorithm">Clenshaw summation</see>.<br/>
            /// （運用 <see href="https://en.wikipedia.org/wiki/Clenshaw_algorithm">Clenshaw 遞推公式</see>求輔助緯度的值。）
            /// </summary>
            /// <param name="zeta">The input latitude.（輸入的緯度。）</param>
            /// <param name="coefficients">The auxiliary latitude coefficients.（輔助緯度係數。）</param>
            /// <returns>The converted latitude.（轉換後的緯度。）</returns>
            public static double ConvertAuxiliaryLatitude(double zeta, Coefficients coefficients)
            {
                /*
                    #  References: （資料來源：）

                    * PROJ contributors. (2025). latitudes.cpp. pj_auxlat_convert()
                        https://github.com/OSGeo/PROJ/blob/master/src/latitudes.cpp#L407

                    * PROJ contributors. (2025). latitudes.cpp. pj_clenshaw()
                        https://github.com/OSGeo/PROJ/blob/master/src/latitudes.cpp#L384
                 */

                double cZeta = math.cos(zeta);
                double sZeta = math.sin(zeta);
                return ConvertAuxiliaryLatitude(zeta, sZeta, cZeta, coefficients);
            }

            /// <summary>
            /// Calculate the auxiliary latitude using <see href="https://en.wikipedia.org/wiki/Clenshaw_algorithm">Clenshaw summation</see>.<br/>
            /// （運用 <see href="https://en.wikipedia.org/wiki/Clenshaw_algorithm">Clenshaw 遞推公式</see>求輔助緯度的值。）
            /// </summary>
            /// <param name="zeta">The input latitude.（輸入的緯度。）</param>
            /// <param name="sZeta">The sine value of the latitude.（緯度的正弦值。）</param>
            /// <param name="cZeta">The cosine value of the latitude.（緯度的餘弦值。）</param>
            /// <param name="coefficients">The auxiliary latitude coefficients.（輔助緯度係數。）</param>
            /// <returns>The converted latitude.（轉換後的緯度。）</returns>
            public static double ConvertAuxiliaryLatitude(double zeta, double sZeta, double cZeta, Coefficients coefficients)
            {
                /*
                    #  References: （資料來源：）

                    * PROJ contributors. (2025). latitudes.cpp. pj_auxlat_convert()
                        https://github.com/OSGeo/PROJ/blob/master/src/latitudes.cpp#L407

                    * PROJ contributors. (2025). latitudes.cpp. pj_clenshaw()
                        https://github.com/OSGeo/PROJ/blob/master/src/latitudes.cpp#L384
                 */

                double u0 = 0;
                double u1 = 0;
                double x = 2 * (cZeta - sZeta) * (cZeta + sZeta);

                for (int i = 5; i > -1; i--)
                {
                    double t = x * u0 - u1 + coefficients[i];
                    u1 = u0;
                    u0 = t;
                }

                return 2 * cZeta * sZeta * u0 + zeta;
            }

            /// <summary>
            /// Transform geocentric coordinates (x, y, z) into geodetic coordinates (λ, φ).
            /// （將地心坐標 (x, y, z) 轉換為大地坐標 (λ, φ)。）
            /// </summary>
            /// <param name="coord">The geocentric coordinates.（地心坐標。）</param>
            /// <param name="ellipsoid">The target ellipsoid.（目標橢球體。）</param>
            /// <returns>The geodetic coordinates on the target ellipsoid.（目標橢球體上的大地坐標。）</returns>
            public static BurstCompatibleCoord ConvertFromGeocentric(BurstCompatibleCoord coord, BurstCompatibleEllipsoidDefinition ellipsoid)
            {
                /*
                    #  References: （資料來源：）

                    * PROJ contributors. (2025). cart.cpp. geodetic()
                        https://github.com/OSGeo/PROJ/blob/master/src/conversions/cart.cpp#L156

                    * PROJ4JS contributors. (2025). datumUtils.js. geocentricToGeodetic()
                        https://github.com/proj4js/proj4js/blob/master/lib/datumUtils.js#L74
                 */

                double a = ellipsoid.a;
                double es = ellipsoid.E1Square;
                double gn = 1E-12;
                double gn2 = gn * gn;
                double height;
                double lat;
                double lon = default;
                double x = coord.x;
                double y = coord.y;
                double z = coord.z;
                double p = Math.Sqrt(x * x + y * y);
                double rr = Math.Sqrt(x * x + y * y + z * z);

                if (p / a < gn)
                {
                    if (rr / a < gn)
                    {
                        return new(x, y, CRS.WGS84);
                    }
                }
                else
                {
                    lon = Math.Atan2(y, x);
                }

                double ct = z / rr;
                double st = p / rr;
                double rn;
                double rk;
                double rx = 1 / Math.Sqrt(1 - es * (2 - es) * st * st);
                double cphi = default;
                double cphi0 = st * (1 - es) * rx;
                double sdphi;
                double sphi = default;
                double sphi0 = ct * rx;

                // Original comments from PROJ4JS:（來自 PROJ4JS 的原始註解：）
                /* --------------------------------------------------------------
                 * Following iterative algorithm was developped by
                 * "Institut for Erdmessung", University of Hannover, July 1988.
                 * Internet: www.ife.uni-hannover.de
                 * Iterative computation of CPHI,SPHI and Height.
                 * Iteration of CPHI and SPHI to 10**-12 radian resp.
                 * 2*10**-7 arcsec.
                 * --------------------------------------------------------------
                 */

                for (int i = 0; i < 30; i++)
                {
                    rn = a / Math.Sqrt(1 - es * sphi0 * sphi0);
                    height = p * cphi0 + z * sphi0 - rn * (1 - es * sphi0 * sphi0);
                    rk = es * rn / (rn + height);
                    rx = 1 / Math.Sqrt(1 - rk * (2 - rk) * st * st);
                    cphi = st * (1 - rk) * rx;
                    sphi = ct * rx;
                    sdphi = sphi * cphi0 - cphi * sphi0;
                    cphi0 = cphi;
                    sphi0 = sphi;
                    if (sdphi * sdphi <= gn2) break;
                }

                lat = Math.Atan(sphi / Math.Abs(cphi));
                return new(math.degrees(lon), math.degrees(lat), CRS.WGS84);
            }

            /// <summary>
            /// Transform geodetic coordinates (λ, φ) into geocentric coordinates (x, y, z).
            /// （將大地坐標 (λ, φ) 轉換為地心坐標 (x, y, z)。）
            /// </summary>
            /// <param name="coord">The geodetic coordinates.（大地坐標。）</param>
            /// <param name="ellipsoid">The source ellipsoid.（來源橢球體。）</param>
            /// <returns>The geocentric coordinates.（地心坐標。）</returns>
            public static BurstCompatibleCoord ConvertToGeocentric(BurstCompatibleCoord coord, BurstCompatibleEllipsoidDefinition ellipsoid)
            {
                /*
                    #  References: （資料來源：）

                    * PROJ contributors. (2025). cart.cpp. cartesian()
                        https://github.com/OSGeo/PROJ/blob/master/src/conversions/cart.cpp#L139

                    * PROJ4JS contributors. (2025). datumUtils.js. geodeticToGeocentric()
                        https://github.com/proj4js/proj4js/blob/master/lib/datumUtils.js#L32
                 */

                double a = ellipsoid.a;
                double es = ellipsoid.E1Square;
                double lat = math.radians(coord.y);
                double lon = math.radians(coord.x);

                if ((lat < -Math.PI / 2) && (lat > -1.001 * Math.PI / 2))
                {
                    lat = -Math.PI / 2;
                }
                else if ((lat > Math.PI / 2) && (lat < 1.001 * Math.PI / 2))
                {
                    lat = Math.PI / 2;
                }
                else if (lat < -Math.PI / 2)
                {
                    return new(double.MinValue, double.MinValue, CRS.Unknown);
                }
                else if (lat > Math.PI / 2)
                {
                    return new(double.MaxValue, double.MaxValue, CRS.Unknown);
                }

                double clat = Math.Cos(lat);
                double slat = Math.Sin(lat);
                double s2lat = slat * slat;
                double rn = a / Math.Sqrt(1 - es * s2lat);
                return new(rn * clat * Math.Cos(lon), rn * clat * Math.Sin(lon), rn * (1 - es) * slat, CRS.Unknown);
            }
        }

        /// <summary>
        /// The flatten taylor series coefficients to calculate auxiliary latitude coefficients.<br/>
        /// （用於計算輔助緯度係數的扁平泰勒展開式係數。）
        /// </summary>
        public static List<double> FlattenTaylorSeries => new()
        {
            // C[phi,mu]; even coeffs only
            3.0/2.0, -27.0/32.0, 269.0/512.0,
            21.0/16.0, -55.0/32.0, 6759.0/4096.0,
            151.0/96.0, -417.0/128.0,
            1097.0/512.0, -15543.0/2560.0,
            8011.0/2560.0,
            293393.0/61440.0,
            // C[phi,chi]
            2.0, -2.0/3.0, -2.0, 116.0/45.0, 26.0/45.0, -2854.0/675.0,
            7.0/3.0, -8.0/5.0, -227.0/45.0, 2704.0/315.0, 2323.0/945.0,
            56.0/15.0, -136.0/35.0, -1262.0/105.0, 73814.0/2835.0,
            4279.0/630.0, -332.0/35.0, -399572.0/14175.0,
            4174.0/315.0, -144838.0/6237.0,
            601676.0/22275.0,
            // C[phi,xi]
            4.0/3.0, 4.0/45.0, -16.0/35.0, -2582.0/14175.0, 60136.0/467775.0,
            28112932.0/212837625.0,
            46.0/45.0, 152.0/945.0, -11966.0/14175.0, -21016.0/51975.0,
            251310128.0/638512875.0,
            3044.0/2835.0, 3802.0/14175.0, -94388.0/66825.0, -8797648.0/10945935.0,
            6059.0/4725.0, 41072.0/93555.0, -1472637812.0/638512875.0,
            768272.0/467775.0, 455935736.0/638512875.0,
            4210684958.0/1915538625.0,
            // C[mu,phi]; even coeffs only
            -3.0/2.0, 9.0/16.0, -3.0/32.0,
            15.0/16.0, -15.0/32.0, 135.0/2048.0,
            -35.0/48.0, 105.0/256.0,
            315.0/512.0, -189.0/512.0,
            -693.0/1280.0,
            1001.0/2048.0,
            // C[mu,chi]
            1.0/2.0, -2.0/3.0, 5.0/16.0, 41.0/180.0, -127.0/288.0, 7891.0/37800.0,
            13.0/48.0, -3.0/5.0, 557.0/1440.0, 281.0/630.0, -1983433.0/1935360.0,
            61.0/240.0, -103.0/140.0, 15061.0/26880.0, 167603.0/181440.0,
            49561.0/161280.0, -179.0/168.0, 6601661.0/7257600.0,
            34729.0/80640.0, -3418889.0/1995840.0,
            212378941.0/319334400.0,
            // C[chi,phi]
            -2.0, 2.0/3.0, 4.0/3.0, -82.0/45.0, 32.0/45.0, 4642.0/4725.0,
            5.0/3.0, -16.0/15.0, -13.0/9.0, 904.0/315.0, -1522.0/945.0,
            -26.0/15.0, 34.0/21.0, 8.0/5.0, -12686.0/2835.0,
            1237.0/630.0, -12.0/5.0, -24832.0/14175.0,
            -734.0/315.0, 109598.0/31185.0,
            444337.0/155925.0,
            // C[chi,mu]
            -1.0/2.0, 2.0/3.0, -37.0/96.0, 1.0/360.0, 81.0/512.0,
            -96199.0/604800.0,
            -1.0/48.0, -1.0/15.0, 437.0/1440.0, -46.0/105.0, 1118711.0/3870720.0,
            -17.0/480.0, 37.0/840.0, 209.0/4480.0, -5569.0/90720.0,
            -4397.0/161280.0, 11.0/504.0, 830251.0/7257600.0,
            -4583.0/161280.0, 108847.0/3991680.0,
            -20648693.0/638668800.0,
            // C[xi,phi]
            -4.0/3.0, -4.0/45.0, 88.0/315.0, 538.0/4725.0, 20824.0/467775.0,
            -44732.0/2837835.0,
            34.0/45.0, 8.0/105.0, -2482.0/14175.0, -37192.0/467775.0,
            -12467764.0/212837625.0,
            -1532.0/2835.0, -898.0/14175.0, 54968.0/467775.0,
            100320856.0/1915538625.0,
            6007.0/14175.0, 24496.0/467775.0, -5884124.0/70945875.0,
            -23356.0/66825.0, -839792.0/19348875.0,
            570284222.0/1915538625.0
        };

        /// <summary>
        /// The map between the initial index in <see cref="FlattenTaylorSeries"/> and the combination of the auxiliary latitude coefficient conversions.<br/>
        /// （<see cref="FlattenTaylorSeries"/> 與輔助緯度係數轉換組合間的映射表。）
        /// </summary>
        public static int[] InitialIndexMap => new int[]
        {
            0, 0, 0, 0, 12, 33, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54,
            66, 66, 66, 66, 87, 87, 108, 108, 108, 129, 129, 129, 150, 150, 150,
            150, 150, 150
        };

        /// <summary>
        /// Retrieve the auxiliary latitude coefficients from the ellipsoid's third flattening.
        /// （由橢球體的第三扁平率獲得輔助緯度係數。）
        /// </summary>
        /// <param name="thirdFlattening">The third flattening of the ellipsoid.（橢球體的第三扁平率。）</param>
        /// <param name="auxIn">
        /// The type of the input auxiliary latitude. Use `0` for geographic latitude, `3` for rectifying latitude, and `4` for conformal latitude.<br/>
        /// （輸入輔助緯度的類別。若為地理緯度填 `0`，矯正緯度填 `3`，等角緯度填 `4`。）
        /// </param>
        /// <param name="auxOut">
        /// The type of the output auxiliary latitude. Use `0` for geographic latitude, `3` for rectifying latitude, and `4` for conformal latitude.<br/>
        /// （輸出輔助緯度的類別。若為地理緯度填 `0`，矯正緯度填 `3`，等角緯度填 `4`。）
        /// </param>
        /// <returns>The coefficients with 6 values.（擁有 6 個數值的係數。）</returns>
        public static Coefficients GetCoefficients(double thirdFlattening, int auxIn, int auxOut)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). latitudes.cpp. pj_auxlat_coeffs()
                    https://github.com/OSGeo/PROJ/blob/master/src/latitudes.cpp#L239

                # The original notes:（原始註解：）

                    pj_auxlat_convert (3 signatures) provide a uniform interface for converting
                    between any pair of auxiliary latitudes using series expansions in the third
                    flattening, n.  There are 6 (= AuxLat::NUMBER) auxiliary latitudes
                    supported labeled by
                        AuxLat::GEOGRAPHIC for geographic latitude, phi
                        AuxLat::PARAMETRIC for parametric latitude, beta
                        AuxLat::GEOCENTRIC for geocentric latitude, theta
                        AuxLat::RECTIFYING for rectifying latitude, mu
                        AuxLat::CONFORMAL for conformal latitude, chi
                        AuxLat::AUTHALIC for authlatic latitude, xi

                    This is adapted from
                        C. F. F. Karney, On auxiliary latitudes,
                        Survey Review 56, 165-180 (2024)
                        https://doi.org/10.1080/00396265.2023.2217604
                        Preprint: https://arxiv.org/abs/2212.05818

                    The conversions are Fourier series in the auxiliary latitude where each
                    coefficient is given as a Taylor series in n truncated at order 6 (=
                    AuxLat::ORDER).  This suffices to give full double precision accuracy for
                    |f| <= 1/150 and probably provide satisfactory results for |f| <= 1/50.  The
                    coefficients for these Taylor series are given by matrics listed in
                    Eqs. (A1-A28) of this paper.

                    These coefficients are bundled up into a single array coeffs in
                    pj_auxlat_coeffs.  Only the upper triangular portion of the matrices are
                    included.  Furthermore, half the coefficients for the conversions between
                    any of phi, bete, theta, and mu are zero (the Taylor series are expansions
                    in n^2), these zero elements are excluded.

                    Only a subset of the conversion matrices are written out.  To add others,
                    include them in the list "required" in writecppproj().  The conversions
                    currently supported are
                        phi <-> mu for meridian distance
                        phi <-> chi for tmerc
                        phi <-> xi for authalic latitude conversions
                        chi <-> mu for tmerc

                    Because all the matrices are concatenated together into a single array,
                    coeff, an auxiliary array, ptrs, or length 37 = AUXNUMBER^2 + 1, is written
                    out to give the starting point of any particular matrix.
             */

            if ((auxIn < 0) || (auxIn > 5)) throw new ArgumentOutOfRangeException("The input latitude type is unknown. 輸入的緯度類別未知。");
            if ((auxOut < 0) || (auxOut > 5)) throw new ArgumentOutOfRangeException("The output latitude type is unknown. 輸出的緯度類別未知。");

            int k = 6 * auxOut + auxIn;
            int o = InitialIndexMap[k];
            if (o == InitialIndexMap[k + 1]) throw new NotSupportedException("The provided latitude type combination is not supported. 不支援的緯度類別組合。");

            Coefficients coeffs = default;
            double d = thirdFlattening;
            double n2 = thirdFlattening * thirdFlattening;

            if ((auxIn <= 3) && (auxOut <= 3))
            {
                for (int i = 0; i < 6; i++)
                {
                    int m = (5 - i) / 2;
                    coeffs[i] = d * GetPolynomialSum(n2, FlattenTaylorSeries.GetRange(o, m + 1), m);
                    o += m + 1;
                    d *= thirdFlattening;
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    int m = 5 - i;
                    coeffs[i] = d * GetPolynomialSum(thirdFlattening, FlattenTaylorSeries.GetRange(o, m + 1), m);
                    o += m + 1;
                    d *= thirdFlattening;
                }
            }

            return coeffs;
        }

        /// <summary>
        /// Calculate the value of sum(coeffs[i] * x^i, i, 0, order) using <see href="https://en.wikipedia.org/wiki/Horner%27s_method">Horner's method</see>.<br/>
        /// （運用<see href="https://en.wikipedia.org/wiki/Horner%27s_method">霍納算法</see>求 sum(coeffs[i] * x^i, i, 0, order) 的值。）
        /// </summary>
        /// <param name="x">The input number.（輸入的數字。）</param>
        /// <param name="coeffs">The coefficients.（係數。）</param>
        /// <param name="order">The order of the iteration.（遞迴的次序。）</param>
        /// <returns>The evaluation result.（校驗結果。）</returns>
        private static double GetPolynomialSum(double x, List<double> coeffs, int order)
        {
            /*
                #  References: （資料來源：）

                * PROJ contributors. (2025). latitudes.cpp. pj_polyval()
                    https://github.com/OSGeo/PROJ/blob/master/src/latitudes.cpp#L375
             */

            double y = order < 0 ? 0 : coeffs[order];
            while (order > 0)
            {
                order--;
                y = y * x + coeffs[order];
            }
            return y;
        }

        /// <summary>
        /// Retrieve the rectifying radius from the ellipsoid's third flattening.
        /// （由橢球體的第三扁平率獲得矯正半徑。）
        /// </summary>
        /// <param name="thirdFlattening">The third flattening of the ellipsoid.（橢球體的第三扁平率。）</param>
        /// <returns>The radius.（半徑。）</returns>
        public static double GetRectifyingRadius(double thirdFlattening)
        {
            /*
                #  References: （資料來源：）

                * PROJ contributors. (2025). latitudes.cpp. pj_rectifying_radius()
                    https://github.com/OSGeo/PROJ/blob/master/src/latitudes.cpp#L427
             */

            return GetPolynomialSum(thirdFlattening * thirdFlattening, new List<double> { 1, 1.0 / 4, 1.0 / 64, 1.0 / 256 }, 3) / (thirdFlattening + 1);
        }
    }
}