using Carto.Geodata;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides utility functions to calculate datum transformations.
    /// （提供計算大地基準轉換功能的類別。）<br/>
    /// Most of the codes in this class was rewritten from <seealso href="https://github.com/OSGeo/PROJ">PROJ</seealso> library.
    /// （這個類別的大部分程式碼是從 <seealso href="https://github.com/OSGeo/PROJ">PROJ</seealso> 套件改寫的。）
    /// </summary>
    public static class DatumUtils
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
            public static Coord ConvertFromGeocentric(Coord coord, EllipsoidDefinition ellipsoid)
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
            public static Coord ConvertToGeocentric(Coord coord, EllipsoidDefinition ellipsoid)
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