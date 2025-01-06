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
            
            * Snyder J.P. (1987) Map projections – a working manual. U.S. Geological Survey Professional Paper 1395, 385 pages
                https://pubs.usgs.gov/pp/1395/report.pdf
         */

        /// <summary>
        /// The semi-major axis of WGS84 ellipsoid (unit: m).
        /// （WGS84 橢球體的半長軸，單位：公尺）
        /// </summary>
        const double a = 6378137;

        /// <summary>
        /// The eccentricity of WGS84 ellipsoid (unitless).
        /// （WGS84 橢球體的偏心率，無因次量）
        /// </summary>
        const double e = 0.00669438;

        /// <summary>
        /// The default false easting of UTM system (unit: m).
        /// （UTM 系統的預設東距，單位：公尺）
        /// </summary>
        const double fE = 500000;

        /// <summary>
        /// Scale factor of WGS84. (unitless).
        /// （WGS84 的尺度因子，單位：公尺）
        /// </summary>
        const double sf = 0.9996;

        /// <summary>
        /// Transform any Transverse Mercator coordinates to WGS84 coordinates.
        /// （將任意橫麥卡托投影坐標轉換為 WGS84 坐標。）
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="ellipsoid"></param>
        /// <param name="origin"></param>
        /// <param name="shift"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public static (double longitude, double latitude) TransverseMercatorToWGS84((double easting, double northing) tm,
                                                                                    EllipsoidDefinition ellipsoid,
                                                                                    (double longitude, double latitude) origin,
                                                                                    (double easting, double northing) shift,
                                                                                    double scaleFactor)
        {
            
        }

        /// <summary>
        /// Transform UTM coordinates to WGS84 coordinates.
        /// （將 UTM 坐標轉換為 WGS84 坐標。）
        /// </summary>
        /// <param name="utm">The UTM coordinate.（UTM 坐標。）</param>
        /// <returns>The WGS84 coordinate.（WGS84 坐標。）</returns>
        public static (double longitude, double latitude) UTMToWGS84((double easting, double northing, int zone, Hemisphere hemisphere) utm)
        {
            // Intermediate Values （中繼值）
            // Directly multiply the numbers is faster than using Math.Pow() to perform nth power calculations. （直接將數字相乘比起使用 Math.Pow() 進行次方運算更為快速。）
            double EST = utm.easting - fE;
            double NOR = utm.northing - ((utm.hemisphere == Hemisphere.North) ? 0 : 10000000);
            double CLON = ((utm.zone - 1) * 6 + 3 - 180);
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

            // WGS84 Coordinates （WGS84 坐標）
            double LAT = (latI - (NI * Math.Tan(latI) / RI) * (D * D / 2 - (5 + 3 * TI + 10 * CI - 4 * CI * CI - 9 * ePrime) * D * D * D * D / 24
                                + (61 + 90 * TI + 298 * CI - 45 * TI * TI - 252 * ePrime - 3 * CI * CI) * D * D * D * D * D * D / 720)) / Math.PI * 180;
            double LON = CLON + ((D - (1 + 2 * TI + CI) * D * D * D / 6
                                + (5 - 2 * CI + 28 * TI - 3 * CI * CI + 8 * ePrime + 24 * TI * TI) * D * D * D * D * D / 120) / Math.Cos(latI)) / Math.PI * 180;

            // The latitude distance of 0.000001 degrees near the poles and the longitude distance of 0.000001 degrees at the equator are ≈ 0.11 meters, which is accurate enough.
            // （兩極附近的0.000001度緯距 ≈ 0.11公尺，而赤道的0.000001度經距 ≈ 0.11公尺，已足夠精準。）
            return (Math.Round(LON, 6), Math.Round(LAT, 6));
        }

        /// <summary>
        /// Transform WGS84 coordinates to Pseudo Mercator coordinates.
        /// （將 WGS84 坐標轉換為偽麥卡托坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <returns>The Pseudo Mercator coordinate.（偽麥卡托坐標。）</returns>
        public static (double x, double y) WGS84ToPseudoMercator((double longitude, double latitude) wgs84)
        {
            // Intermediate Values （中繼值）
            double LATr = wgs84.latitude / 180 * Math.PI;
            double LONr = wgs84.longitude / 180 * Math.PI;

            // Pseudo Mercator Coordinates （偽麥卡托坐標）
            double X = a * LONr;
            double Y = a * Math.Log(Math.Tan(Math.PI / 4 + LATr / 2));

            // The accuracy of 0.00001 meters (0.01 milimeters) is good enough.
            //（0.00001公尺（0.01毫米）的準確度已經足夠好了。）
            return (Math.Round(X, 6), Math.Round(Y, 6));
        }

        /// <summary>
        /// Transform WGS84 coordinates to UTM coordinates.
        /// （將 WGS84 坐標轉換為 UTM 坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <returns>The UTM coordinate.（UTM 坐標。）</returns>
        public static (double easting, double northing, int zone, Hemisphere hemisphere) WGS84ToUTM((double longitude, double latitude) wgs84)
        {
            // Constants （常數）
            Hemisphere hemisphere = (wgs84.latitude >= 0) ? Hemisphere.North : Hemisphere.South; // The hemisphere point located at. （點位所在的半球。）
            double fN = (hemisphere == Hemisphere.North) ? 0 : 10000000;  // The default false northing (unit: m) （預設北距，單位：公尺）

            // Intermediate Values （中繼值）
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