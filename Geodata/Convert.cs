namespace Carto.Geodata
{
    using System;

    /// <summary>
    /// A class to convert coordinate systems.
    /// （一個用以轉換坐標系統的類別。）
    /// </summary>
    public class Convert
    {

        /*
            # Source: （資料來源：）

            * Manchuk, J. G., & Deutsch, C. V., (2009). Conversion of Latitude and Longitude to UTM Coordinates. Centre for Computational Geostatistics Report 11, 410.
              University of Alberta, Canada.
                https://www.ccgalberta.com/ccgresources/report11/2009-410_converting_latlon_to_utm.pdf

            * Snyder J.P. (1987) Map projections – a working manual. U.S. Geological Survey Professional Paper 1395, 385 pages
                https://pubs.usgs.gov/pp/1395/report.pdf
        */

        // Constants （常數）
        const double a = 6378137;                   // Semi-major axis (unit: m) （半長軸，單位：公尺）
        const double e = 0.00669438;                // Eccentricity = 2f - f^2 (unitless) （偏心率，無因次量）
        const double f = 1 / 298.257223563;         // Flattening (unitless) （扁率，無因次量）
        const double sf = 0.9996;                   // Scale factor (unitless) （尺度係數／因子，無因次量）
        const double fE = 500000;                   // Default false easting (unit: m) （預設東距，單位：公尺）

        /// <summary>
        /// Convert WGS84 coordinates into UTM (Universal Transverse Mercator).
        /// （將 WGS84 坐標轉換為 UTM （通用橫麥卡托投影）。）
        /// </summary>
        /// <param name="wgs">
        /// The latitude, longitude in degrees; for latitude, northern hemisphere is positive; for longitude, eastern hemisphere is positive.
        /// （以度為單位的經緯度；北半球的緯度為正值，而東半球的經度為正值。）</param>
        /// <returns>
        /// A tuple containing the easting, northing, zone number and the hemisphere. 
        /// （一個包含東距、北距、分區和所在半球的元組。）
        /// </returns>
        public static (double easting, double northing, int zone, string hemisphere) ToUTM((double lat, double lon) wgs)
        {
            // Constants （常數）
            double fN = (wgs.lat >= 0) ? 0 : 10000000;  // Default false northing (unit: m) （預設北距，單位：公尺）
            string HEMI = (wgs.lat >= 0) ? "N" : "S";   // The hemisphere point located at. （點位所在的半球。）

            // Intermediate Values （中繼值）
            // Directly multiply the numbers is faster than using Math.Pow() to perform nth power calculations. （直接將數字相乘比起使用 Math.Pow() 進行次方運算更為快速。）
            double LATr = wgs.lat / 180 * Math.PI;
            double LONr = wgs.lon / 180 * Math.PI;
            double ZONE = Math.Truncate((wgs.lon + 180) / 6) + 1;
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

            return (X, Y, (int)ZONE, HEMI);
        }

        /// <summary>
        /// Convert UTM coordinates into WGS84 (latitude and longitude).
        /// （將 UTM 坐標轉換為 WGS84（經緯度）。）
        /// </summary>
        /// <param name="utm">
        /// The easting &amp; northing in meters, the UTM Zone number, and the hemisphere located in.
        /// （以公尺為單位的 UTM 坐標、UTM 分區和點位所屬的半球。）
        /// </param>
        /// <returns>
        /// A tuple containing the latitude &amp; longitude.
        /// （一個包含緯度和經度的元組。）
        /// </returns>
        public static (double lat, double lon) ToWGS84((double easting, double northing, int zone, string hemisphere) utm)
        {
            // Intermediate Values （中繼值）
            // Directly multiply the numbers is faster than using Math.Pow() to perform nth power calculations. （直接將數字相乘比起使用 Math.Pow() 進行次方運算更為快速。）
            double EST = utm.easting - fE;
            double NOR = utm.northing - ((utm.hemisphere == "N") ? 0 : 10000000);
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
            return (Math.Round(LAT, 6), Math.Round(LON, 6));
        }
    }
}