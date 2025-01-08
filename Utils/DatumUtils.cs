using Carto.Geodata;
using System;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides utility functions to calculate datum transformations.
    /// （提供計算大地基準轉換功能的類別。）<br/>
    /// Most of the codes in this class was rewritten from <seealso href="https://github.com/proj4js/proj4js/">PROJ4JS</seealso> library.
    /// （這個類別的大部分程式碼是從 <seealso href="https://github.com/proj4js/proj4js/">PROJ4JS</seealso> 套件改寫的。）
    /// </summary>
    public static class DatumUtils
    {
        /// <summary>
        /// Clamp the longitude into the correct radians.
        /// （將經度修正（夾緊）至正確的弧度。）
        /// </summary>
        /// <param name="longitude">The input longitude.（輸入的經度。）</param>
        /// <returns>The clamped longitude.（修正後的經度。）</returns>
        public static double ClampLongitude(double longitude)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). adjust_lon.js.
                    https://github.com/proj4js/proj4js/blob/master/lib/common/adjust_lon.js
            */

            return Math.Abs(longitude) <= 3.14159265359 ? longitude : longitude - (longitude / Math.Abs(longitude) * Math.PI * 2);
        }

        /// <summary>
        /// Transform geocentric coordinates (x, y, z) into geodetic coordinates (λ, φ).
        /// （將地心坐標 (x, y, z) 轉換為大地坐標 (λ, φ)。）
        /// </summary>
        /// <param name="geocentric">The geocentric coordinates.（地心坐標。）</param>
        /// <param name="ellipsoid">The target ellipsoid.（目標橢球體。）</param>
        /// <returns>The geodetic coordinates on the target ellipsoid.（目標橢球體上的大地坐標。）</returns>
        public static (double longitude, double latitude) GeocentricToGeodetic((double x, double y, double z) geocentric, EllipsoidDefinition ellipsoid)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). datumUtils.js. geocentricToGeodetic()
                    https://github.com/proj4js/proj4js/blob/master/lib/datumUtils.js
            */

            double a = ellipsoid.a;
            double es = ellipsoid.eSquare;
            double gn = 1E-12;
            double gn2 = gn * gn;
            double height;
            double lat;
            double lon = default;
            double x = geocentric.x;
            double y = geocentric.y;
            double z = geocentric.z;
            double p = Math.Sqrt(x * x + y * y);
            double rr = Math.Sqrt(x * x + y * y + z * z);

            if (p / a < gn)
            {
                if (rr / a < gn)
                {
                    return (x, y);
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
            return (lon, lat);
        }

        /// <summary>
        /// Transform the geocentric coordinates from WGS84 to any datum.
        /// （將 WGS84 的地心坐標轉換為任意大地基準。）
        /// </summary>
        /// <param name="wgs84">The geocentric coordinates from WGS84 ellipsoid.（來自 WGS84 橢球體的地心坐標。）</param>
        /// <param name="transform">The Helmert Transform parameters to transform any datum to WGS84.（由任意大地基準變為 WGS84 的赫爾默特轉換參數。）</param>
        /// <returns>The transformed geocentric coordinates.（已轉換的地心坐標。）</returns>
        public static (double x, double y, double z) GeocentricFromWGS84((double x, double y, double z) wgs84, double[] transform)
        {
            if (transform.Length == 3)
            {
                return (wgs84.x - transform[0],  wgs84.y - transform[1], wgs84.z - transform[2]);
            }
            else if (transform.Length == 7)
            {
                double dx = transform[0];
                double dy = transform[1];
                double dz = transform[2];
                double rx = transform[3] * Math.PI / 180 / 3600;
                double ry = transform[4] * Math.PI / 180 / 3600;
                double rz = transform[5] * Math.PI / 180 / 3600;
                double m  = transform[6] / 1E6 + 1;
                double x = (wgs84.x - dx) / m;
                double y = (wgs84.y - dy) / m;
                double z = (wgs84.z - dz) / m;
                return (x + rz * y - ry * z, -rz * x + y + rx * z, ry * x - rx * y + z);
            }

            return wgs84;
        }

        /// <summary>
        /// Transform the geocentric coordinates from any datum to WGS84.
        /// （將任意大地基準的地心坐標轉換為 WGS84。）
        /// </summary>
        /// <param name="datum">The geocentric coordinates from any datum.（來自任意大地基準的地心坐標。）</param>
        /// <param name="transform">The Helmert Transform parameters to transform any datum to WGS84.（由任意大地基準變為 WGS84 的赫爾默特轉換參數。）</param>
        /// <returns>The transformed geocentric coordinates.（已轉換的地心坐標。）</returns>
        public static (double x, double y, double z) GeocentricToWGS84((double x, double y, double z) datum, double[] transform)
        {
            if (transform.Length == 3)
            {
                return (datum.x + transform[0], datum.y + transform[1], datum.z + transform[2]);
            }
            else if (transform.Length == 7)
            {
                double dx = transform[0];
                double dy = transform[1];
                double dz = transform[2];
                double rx = transform[3] * Math.PI / 180 / 3600;
                double ry = transform[4] * Math.PI / 180 / 3600;
                double rz = transform[5] * Math.PI / 180 / 3600;
                double m = transform[6] / 1E6 + 1;
                double x = datum.x;
                double y = datum.y;
                double z = datum.z;
                return (m * (x - rz * y + ry * z) + dx, m * (rz * x + y - rx * z) + dy, m * (-ry * x + rx * y + z) + dz);
            }

            return datum;
        }

        /// <summary>
        /// Transform geodetic coordinates (λ, φ) into geocentric coordinates (x, y, z).
        /// （將大地坐標 (λ, φ) 轉換為地心坐標 (x, y, z)。）
        /// </summary>
        /// <param name="geodetic">The geodetic coordinates.（大地坐標。）</param>
        /// <param name="ellipsoid">The source ellipsoid.（來源橢球體。）</param>
        /// <returns>The geocentric coordinates.（地心坐標。）</returns>
        public static (double x, double y, double z) GeodeticToGeocentric((double longitude, double latitude) geodetic, EllipsoidDefinition ellipsoid)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). datumUtils.js. geodeticToGeocentric()
                    https://github.com/proj4js/proj4js/blob/master/lib/datumUtils.js
            */

            double a = ellipsoid.a;
            double es = ellipsoid.eSquare;
            double lat = geodetic.latitude;
            double lon = geodetic.longitude;

            if ((lat < - Math.PI / 2) && (lat > -1.001 * Math.PI / 2))
            {
                lat = - Math.PI / 2;
            }
            else if ((lat > Math.PI / 2) && (lat < 1.001 * Math.PI / 2))
            {
                lat = Math.PI / 2;
            }
            else if (lat < -Math.PI / 2)
            {
                return (double.MinValue, double.MinValue, 0);
            }
            else if (lat > Math.PI / 2)
            {
                return (double.MaxValue, double.MaxValue, 0);
            }

            double clat = Math.Cos(lat);
            double slat = Math.Sin(lat);
            double s2lat = slat * slat;
            double rn = a / Math.Sqrt(1 - es * s2lat);
            return (rn * clat * Math.Cos(lon), rn * clat * Math.Sin(lon), rn * (1 - es) * slat);
        }

        /// <summary>
        /// Calculate the meridional distance (M) between equator and the geodetic latitude (φ) from the eccentricity squared (e²) value on an unit ellipsoid (a = 1).<br/>
        /// 根據偏心率平方（e²）值計算單位橢球體（a = 1）上赤道至大地緯度（φ）的距離。
        /// </summary>
        /// <param name="phi">The geodetic latitude, the angle between equatorial plane and the surface normal.（大地緯度，地表法線與赤道平面相交的角度。）</param>
        /// <param name="es">The eccentricity squared.（偏心率平方值。）</param>
        /// <returns>The meridional distance.（緯距。）</returns>
        public static double MeridionalDistance(double phi, double es)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). pj_mlfn.js.
                    https://github.com/proj4js/proj4js/blob/master/lib/common/pj_mlfn.js
            */
            
            double[] en = MeridionalDistanceCoefficients(es);
            return MeridionalDistance(phi, en);
        }

        /// <summary>
        /// Calculate the meridional distance (M) between equator and the geodetic latitude (φ) from the meridional distance coefficients on an unit ellipsoid (a = 1).<br/>
        /// 根據緯距係數計算單位橢球體（a = 1）上赤道至大地緯度（φ）的距離。
        /// </summary>
        /// <param name="phi">The geodetic latitude, the angle between equatorial plane and the surface normal.（大地緯度，地表法線與赤道平面相交的角度。）</param>
        /// <param name="en">The meridional distance coefficients.（緯距係數。）</param>
        /// <returns>The meridional distance.（緯距。）</returns>
        private static double MeridionalDistance(double phi, double[] en)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). pj_mlfn.js.
                    https://github.com/proj4js/proj4js/blob/master/lib/common/pj_mlfn.js
            */
            double cphi = Math.Cos(phi) * Math.Sin(phi);
            double sphi = Math.Sin(phi) * Math.Sin(phi);
            return en[0] * phi - cphi * (en[1] + sphi * (en[2] + sphi * (en[3] + sphi * en[4])));
        }

        /// <summary>
        /// Calculate the coefficient used in the meridional distance.
        /// （計算用於緯距的係數。）
        /// </summary>
        /// <param name="es">The eccentricity squared.（偏心率平方值。）</param>
        /// <returns>A coefficient array with length of 5.（長度為 5 的係數陣列。）</returns>
        private static double[] MeridionalDistanceCoefficients(double es)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). pj_enfn.js.
                    https://github.com/proj4js/proj4js/blob/master/lib/common/pj_enfn.js
            */

            double c00 =   1;
            double c02 =   1 /     4.0; // 0.25
            double c04 =   3 /    64.0; // 0.046875
            double c06 =   5 /   256.0; // 0.01953125
            double c08 =   7 / 65536.0; // 0.01068115234375
            double c22 =   3 /     4.0; // 0.75
            double c44 =  15 /    32.0; // 0.46875
            double c46 =   5 /   384.0; // 0.01302083...
            double c48 = 175 / 24576.0; // 0.00712076822916...
            double c66 =  35 /    96.0; // 0.364583...
            double c68 =  35 /  6144.0; // 0.005696614583...
            double c88 = 315 /  1024.0; // 0.3076171875
            double[] en = new double[5];

            en[0] = c00 - es * (c02 + es * (c04 + es * (c06 + es * c08)));
            en[1] = es * (c22 - es * (c04 + es * (c06 + es * c08)));
            en[2] = es * es * (c44 - es * (c46 + es * c48));
            en[3] = es * es * es * (c66 - es * c68);
            en[4] = es * es * es * es * c88;
            return en;
        }

        /// <summary>
        /// Calculate the geodetic latitude (φ) from the eccentricity squared (e²) value and the meridional distance (M) on an unit ellipsoid (a = 1).<br/>
        /// 根據偏心率平方（e²）值與緯距（M）計算單位橢球體（a = 1）上對應的大地緯度（φ）。
        /// </summary>
        /// <param name="distance">The meridional distance.（緯距。）</param>
        /// <param name="es">The eccentricity squared.（偏心率平方值。）</param>
        /// <returns>The geodetic latitude.（大地緯度。）</returns>
        public static double MeridionalDistanceInverse(double distance, double es)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). pj_inv_mlfn.js.
                    https://github.com/proj4js/proj4js/blob/master/lib/common/pj_inv_mlfn.js
            */

            double[] en = MeridionalDistanceCoefficients(es);
            double k = 1 / (1 - es);
            double phi = distance;
            for (int i = 0; i < 20; i++)
            {
                double sphi = Math.Sin(phi);
                double t = 1 - es * sphi * sphi;
                t = (MeridionalDistance(phi, en) - distance) * (t * Math.Sqrt(t)) * k;
                phi -= t;
                if (Math.Abs(t) < 1E-10) return phi;
            }
            return phi;
        }
    }
}