using Carto.Utils;
using Colossal.Logging;
using System;

namespace Carto.Geodata
{
    /// <summary>
    /// The class that provides transformation between popular CRSs.
    /// （提供數種熱門坐標參考系統間轉換的類別。）
    /// </summary>
    public static class Transform
    {
        /*
            # References: （資料來源：）
            
            * Evenden, G. I. (2005). libproj4: A Comprehensive Library of Cartographic Projection Functions (Preliminary Draft).
                https://github.com/OSGeo/PROJ/blob/master/docs/old/libproj.pdf

            * Manchuk, J. G., & Deutsch, C. V. (2009). Conversion of Latitude and Longitude to UTM Coordinates. Centre for Computational Geostatistics Report 11, 410.
              University of Alberta, Canada.
                https://www.ccgalberta.com/ccgresources/report11/2009-410_converting_latlon_to_utm.pdf
            
            * Snyder J.P. (1987). Map projections – a working manual. U.S. Geological Survey Professional Paper 1395, 385 pages
                https://pubs.usgs.gov/pp/1395/report.pdf
         */

        public static ILog log = LogManager.GetLogger(nameof(Carto)).SetShowsErrorsInUI(false);

        /// <summary>
        /// The WGS84 ellipsoid.
        /// （WGS84 橢球體。）
        /// </summary>
        static readonly EllipsoidDefinition EllipWGS84 = new(Ellipsoid.WGS84);

        /// <summary>
        /// EPSG: 326xx. The WGS84 / UTM projection in the northern hemisphere.
        /// （北半球的 WGS84 / UTM 投影。）
        /// </summary>
        static readonly ProjectionDefinition ProjUTMNorth = new(EllipWGS84, (0, 0), (5E6, 0), 0.9996, new double[0]);

        /// <summary>
        /// EPSG: 327xx. The WGS84 / UTM projection in the southern hemisphere.
        /// （南半球的 WGS84 / UTM 投影。）
        /// </summary>
        static readonly ProjectionDefinition ProjUTMSouth = new(EllipWGS84, (0, 0), (5E6, 1E7), 0.9996, new double[0]);

        /// <summary>
        /// Transform any Transverse Mercator coordinates to WGS84 coordinates.
        /// （將任意橫麥卡托投影坐標轉換為 WGS84 坐標。）
        /// </summary>
        /// <param name="tm">The Transverse Mercator coodinates.（橫麥卡托坐標。）</param>
        /// <param name="projection">The Transverse Mercator projection metadata.（橫麥卡托投影的元資料。）</param>
        /// <returns>The WGS84 coordinate.（WGS84 坐標。）</returns>
        public static (double longitude, double latitude) TransverseMercatorToWGS84((double easting, double northing) tm, ProjectionDefinition projection)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). tmerc.js. inverse()
                    https://github.com/proj4js/proj4js/blob/master/lib/projections/tmerc.js
            */

            // Constants（常數）
            double es = projection.ellipsoid.eSquare;
            double lat;
            double lat0 = projection.origin.latitude / 180 * Math.PI;
            double lon = default;
            double lon0 = projection.origin.longitude / 180 * Math.PI;
            double sf = projection.scaleFactor;
            double x = (tm.easting - projection.shift.easting) / projection.ellipsoid.a;
            double y = (tm.northing - projection.shift.northing) / projection.ellipsoid.a;

            // Intermediate Values（中繼值）
            // Directly multiply the numbers is faster than using Math.Pow() to perform nth power calculations.（直接將數字相乘比起使用 Math.Pow() 進行次方運算更為快速。）
            double ml0 = DatumUtils.MeridionalDistance(lat0, es);
            double con = ml0 + y / sf;
            double phi = DatumUtils.MeridionalDistanceInverse(con, es);

            if (Math.Abs(phi) < Math.PI / 2)
            {
                double cphi = Math.Cos(phi);
                double sphi = Math.Sin(phi);
                double tphi = Math.Abs(cphi) > 1E-10 ? Math.Tan(phi) : 0;
                double c = es * cphi * cphi;
                double cs = c * c;
                con = 1 - es * sphi * sphi;
                double d = x * Math.Sqrt(con) / sf;
                double ds = d * d;
                double t = tphi * tphi;
                double ts = t * t;
                con *= tphi;

                lat = phi - con * ds / (1 - es) * 0.5 * (1 -
                      ds / 12 * (5 + 3 * t - 9 * c * t + c - 4 * cs -
                      ds / 30 * (61 + 90 * t - 252 * c * t + 45 * ts + 46 * c -
                      ds / 56 * (1385 + 3633 * t + 4095 * ts + 1574 * ts * t))));

                lon = lon0 + d * (1 -
                      ds / 6 * (1 + 2 * t + c -
                      ds / 20 * (5 + 28 * t + 24 * ts + 8 * c * t + 6 * c -
                      ds / 42 * (61 + 662 * t + 1320 * ts + 720 * ts * t)))) / cphi;
                
                lon = DatumUtils.ClampLongitude(lon);
            }
            else
            {
                lat = y / Math.Abs(y) * Math.PI / 2;
            }

            if (projection.HasTransform())
            {
                (double, double, double) gcc = DatumUtils.GeodeticToGeocentric((lon, lat), projection.ellipsoid);
                gcc = DatumUtils.GeocentricToWGS84(gcc, projection.transform);
                (lon, lat) = DatumUtils.GeocentricToGeodetic(gcc, EllipWGS84);
            }

            // WGS84 Coordinates（WGS84 坐標）
            return (Math.Round(lon / Math.PI * 180, 7), Math.Round(lat / Math.PI * 180, 7));
        }

        /// <summary>
        /// Transform UTM coordinates to WGS84 coordinates.
        /// （將 UTM 坐標轉換為 WGS84 坐標。）
        /// </summary>
        /// <param name="utm">The UTM coordinate.（UTM 坐標。）</param>
        /// <returns>The WGS84 coordinate.（WGS84 坐標。）</returns>
        public static (double longitude, double latitude) UTMToWGS84((double easting, double northing, int zone, Hemisphere hemisphere) utm)
        {
            // Constants（常數）
            double a = EllipWGS84.a;
            double e = EllipWGS84.eSquare;
            double fE = ProjUTMNorth.shift.easting;
            double fN = (utm.hemisphere == Hemisphere.North) ? ProjUTMNorth.shift.northing : ProjUTMSouth.shift.northing;
            double sf = ProjUTMNorth.scaleFactor;

            // Intermediate Values（中繼值）
            double EST = utm.easting - fE;
            double NOR = utm.northing - fN;
            double CLON = (utm.zone - 1) * 6 + 3 - 180;
            double eI = (1 - Math.Sqrt(1 - e)) / (1 + Math.Sqrt(1 - e));
            double ePrime = e / (1 - e);
            double M = NOR / sf;
            double mu = M / (a * (1 - e / 4 - 3 * e * e / 64 - 5 * e * e * e / 256));
            double latI = mu + (3 * eI / 2 - 27 * eI * eI * eI / 32) * Math.Sin(2 * mu) + (21 * eI * eI / 16 - 55 * eI * eI * eI * eI / 32) * Math.Sin(4 * mu)
                               + (151 * eI * eI * eI / 96) * Math.Sin(6 * mu) + (1097 * eI * eI * eI * eI / 512) * Math.Sin(8 * mu);
            double CI = ePrime * Math.Cos(latI) * Math.Cos(latI);
            double TI = Math.Tan(latI) * Math.Tan(latI);
            double NI = a / Math.Sqrt(1 - e * Math.Sin(latI) * Math.Sin(latI));
            double RI = a * (1 - e) / Math.Pow(1 - e * Math.Sin(latI) * Math.Sin(latI), 1.5);
            double D = EST / NI / sf;

            // WGS84 Coordinates（WGS84 坐標）
            double LAT = (latI - (NI * Math.Tan(latI) / RI) * (D * D / 2 - (5 + 3 * TI + 10 * CI - 4 * CI * CI - 9 * ePrime) * D * D * D * D / 24
                                + (61 + 90 * TI + 298 * CI - 45 * TI * TI - 252 * ePrime - 3 * CI * CI) * D * D * D * D * D * D / 720)) / Math.PI * 180;
            double LON = CLON + ((D - (1 + 2 * TI + CI) * D * D * D / 6
                                + (5 - 2 * CI + 28 * TI - 3 * CI * CI + 8 * ePrime + 24 * TI * TI) * D * D * D * D * D / 120) / Math.Cos(latI)) / Math.PI * 180;

            // The latitude distance of 0.000001 degrees near the poles and the longitude distance of 0.000001 degrees at the equator are ≈ 0.11 meters, which is accurate enough.
            // （兩極附近的0.000001度緯距 ≈ 0.11公尺，而赤道的0.000001度經距 ≈ 0.11公尺，已足夠精準。）
            return (Math.Round(LON, 7), Math.Round(LAT, 7));
        }

        /// <summary>
        /// Transform WGS84 coordinates to Pseudo Mercator coordinates.
        /// （將 WGS84 坐標轉換為偽麥卡托坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <returns>The Pseudo Mercator coordinate.（偽麥卡托坐標。）</returns>
        public static (double x, double y) WGS84ToPseudoMercator((double longitude, double latitude) wgs84)
        {
            // Constants（常數）
            double a = EllipWGS84.a;

            // Intermediate Values（中繼值）
            double LATr = wgs84.latitude / 180 * Math.PI;
            double LONr = wgs84.longitude / 180 * Math.PI;

            // Pseudo Mercator Coordinates（偽麥卡托坐標）
            double X = a * LONr;
            double Y = a * Math.Log(Math.Tan(Math.PI / 4 + LATr / 2));

            // The accuracy of 0.00001 meters (0.01 milimeters) is good enough.
            //（0.00001公尺（0.01毫米）的準確度已經足夠好了。）
            return (Math.Round(X, 6), Math.Round(Y, 6));
        }

        /// <summary>
        /// Transform WGS84 coordinates to any Transverse Mercator coordinates.
        /// （將 WGS84 坐標轉換為任意橫麥卡托投影坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <param name="projection">The Transverse Mercator projection metadata.（橫麥卡托投影的元資料。）</param>
        /// <returns>The Transverse Mercator coodinates.（橫麥卡托坐標。）</returns>
        public static (double easting, double northing) WGS84ToTransverseMercator((double longitude, double latitude) wgs84, ProjectionDefinition projection)
        {
            /*
                # References: （資料來源：）
            
                * PROJ4JS contributors. (2025). tmerc.js. forward()
                    https://github.com/proj4js/proj4js/blob/master/lib/projections/tmerc.js
            */

            // Constants（常數）
            double a = projection.ellipsoid.a;
            double es = projection.ellipsoid.eSquare;
            double lat = wgs84.latitude / 180 * Math.PI;
            double lat0 = projection.origin.latitude / 180 * Math.PI;
            double lon = wgs84.longitude / 180 * Math.PI;
            double lon0 = projection.origin.longitude / 180 * Math.PI;
            double sf = projection.scaleFactor;

            if (projection.HasTransform())
            {
                (double, double, double) gcc = DatumUtils.GeodeticToGeocentric((lon, lat), EllipWGS84);
                gcc = DatumUtils.GeocentricFromWGS84(gcc, projection.transform);
                (lon, lat) = DatumUtils.GeocentricToGeodetic(gcc, projection.ellipsoid);
            }

            // Intermediate Values（中繼值）
            double dLon = DatumUtils.ClampLongitude(lon - lon0);
            double cphi = Math.Cos(lat);
            double sphi = Math.Sin(lat);
            double al = cphi * dLon;
            double als = al * al;
            double c = es * cphi * cphi;
            double cs = c * c;
            double tphi = Math.Abs(cphi) > 1E-10 ? Math.Tan(lat) : 0;
            double t = tphi * tphi;
            double ts = t * t;
            al /= Math.Sqrt(1 - es * sphi * sphi);
            double ml = DatumUtils.MeridionalDistance(lat, es);
            double ml0 = DatumUtils.MeridionalDistance(lat0, es);

            // TM Coordinates （TM 坐標）
            double x = a * (sf * al * (1 +
                       als / 6 * (1 - t + c +
                       als / 20 * (5 - 18 * t + ts + 14 * c - 58 * t * c +
                       als / 42 * (61 + 179 * ts - ts * t - 479 * t))))) + projection.shift.easting;

            double y = a * (sf * (ml - ml0 +
                       sphi * dLon * al / 2 * (1 +
                       als / 12 * (5 - t + 9 * c + 4 * cs +
                       als / 30 * (61 + ts - 58 * t + 270 * c - 330 * t * c +
                       als / 56 * (1385 + 543 * ts - ts * t - 3111 * t)))))) + projection.shift.northing;

            return (x, y);
        }

        /// <summary>
        /// Transform WGS84 coordinates to UTM coordinates.
        /// （將 WGS84 坐標轉換為 UTM 坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <returns>The UTM coordinate.（UTM 坐標。）</returns>
        public static (double easting, double northing, int zone, Hemisphere hemisphere) WGS84ToUTM((double longitude, double latitude) wgs84)
        {
            // Constants（常數）
            double a = EllipWGS84.a;
            double e = EllipWGS84.eSquare;
            double fE = ProjUTMNorth.shift.easting;
            double fN = (wgs84.latitude >= 0) ? ProjUTMNorth.shift.northing : ProjUTMSouth.shift.northing;
            Hemisphere hemisphere = (wgs84.latitude >= 0) ? Hemisphere.North : Hemisphere.South;
            double sf = ProjUTMNorth.scaleFactor;

            // Intermediate Values（中繼值）
            double LATr = wgs84.latitude / 180 * Math.PI;
            double LONr = wgs84.longitude / 180 * Math.PI;
            double ZONE = Math.Truncate((wgs84.longitude + 180) / 6) + 1;
            double CLON = (ZONE - 1) * 6 + 3 - 180;
            double CLONr = CLON / 180 * Math.PI;
            double ePrime = e / (1 - e);
            double N = a / Math.Sqrt(1 - e * Math.Sin(LATr) * Math.Sin(LATr));
            double T = Math.Tan(LATr) * Math.Tan(LATr);
            double C = ePrime * Math.Cos(LATr) * Math.Cos(LATr);
            double A = (LONr - CLONr) * Math.Cos(LATr);
            double M = a * ((1 - e / 4 - 3 * e * e / 64 - 5 * e * e * e / 256) * LATr - (3 * e / 8 + 3 * e * e / 32 + 45 * e * e * e / 1024) * Math.Sin(2 * LATr)
                         + (15 * e * e / 256 + 45 * e * e * e / 1024) * Math.Sin(4 * LATr) - 35 * e * e * e / 3072 * Math.Sin(6 * LATr));

            // UTM Coordinates （UTM 坐標）
            double X = sf * N * (A + (1 - T + C) * A * A * A / 6 + (5 - 18 * T + T * T + 72 * C - 58 * ePrime) * A * A * A * A * A / 120) + fE;
            double Y = sf * (M + N * Math.Tan(LATr) * (A * A / 2
                                                       + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24
                                                       + (61 - 58 * T + T * T + 600 * C - 330 * ePrime) * A * A * A * A * A * A / 720)) + fN;

            return (X, Y, (int)ZONE, hemisphere);
        }
    }
}