using Carto.Utils;
using System;
using Unity.Mathematics;

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

            * PROJ contributors (2025). PROJ coordinate transformation software library. Open Source Geospatial Foundation.
                https://proj.org/. doi: 10.5281/zenodo.5884394
            
            * Snyder J.P. (1987). Map projections – a working manual. U.S. Geological Survey Professional Paper 1395, 385 pages
                https://pubs.usgs.gov/pp/1395/report.pdf
         */

        /// <summary>
        /// A helper function to apply CRS transformation easily.
        /// （用於簡易轉換坐標參考系統的輔助函數。）
        /// </summary>
        /// <param name="coordinate">The representation of a location.（空間中的位置。）</param>
        /// <param name="sourceCRS">The CRS of the original coordinate.（轉換前坐標的坐標參考系統。）</param>
        /// <param name="targetCRS">The CRS of the converted coordinate.（轉換後坐標的坐標參考系統。）</param>
        /// <param name="sourceProjection">The source custom Transverse Mercator projection.（使用者自訂的來源橫麥卡托投影。）</param>
        /// <param name="targetProjection">The target custom Transverse Mercator projection.（使用者自訂的目標橫麥卡托投影。）</param>
        /// <returns>The transformed coordinates.（轉換後的坐標。）</returns>
        /// <exception cref="NotSupportedException"></exception>
        public static Coord Apply(Coord coordinate, CRS sourceCRS, CRS targetCRS, ProjectionDefinition sourceProjection, ProjectionDefinition targetProjection)
        {
            if ((sourceCRS == targetCRS) && (sourceCRS != CRS.TransverseMercator)) return coordinate;
            if ((sourceCRS == CRS.Unknown) || (sourceCRS == CRS.Game) || (sourceCRS == CRS.PseudoMercator)) throw new NotSupportedException("No available conversion from sourceCRS to WGS84. 沒有自 sourceCRS 至 WGS84 的轉換。");
            if ((targetCRS == CRS.Unknown) || (targetCRS == CRS.Game)) throw new NotSupportedException("No available conversion from WGS84 to targetCRS. 沒有自 WGS84 至 targetCRS 的轉換。");

            Coord intermediateCoordinate = coordinate;

            switch (sourceCRS)
            {
                case CRS.TransverseMercator:
                    intermediateCoordinate = TransverseMercatorToWGS84(coordinate, sourceProjection);
                    break;

                case CRS.UTM:
                    intermediateCoordinate = UTMToWGS84(coordinate);
                    break;

                case CRS.WGS84:
                    break;
            }

            if (targetCRS == CRS.WGS84) return intermediateCoordinate;

            return targetCRS switch
            {
                CRS.PseudoMercator => WGS84ToPseudoMercator(intermediateCoordinate),
                CRS.TransverseMercator => WGS84ToTransverseMercator(intermediateCoordinate, targetProjection),
                CRS.UTM => WGS84ToUTM(intermediateCoordinate),
                _ => coordinate,
            };
        }

        /// <summary>
        /// Convert a Transverse Mercator coordinate to a WGS84 coordinate.
        /// （將橫麥卡托投影坐標轉換為 WGS84 坐標。）
        /// </summary>
        /// <param name="tm">The Transverse Mercator coordinate.（橫麥卡托坐標。）</param>
        /// <param name="projection">The definition of the Transverse Mercator projection.（橫麥卡托投影的定義。）</param>
        /// <returns>The converted WGS84 coordinate.（轉換後的 WGS84 坐標。）</returns>
        public static Coord TransverseMercatorToWGS84(Coord tm, ProjectionDefinition projection)
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
                double r = DatumUtils.Burst.ClenshawSum(sinArgR, cosArgR, sinhArgI, coshArgI, projection.ellipsoid.coefficientsRC, out double i);

                Ce += i;
                Cn += r;

                double sinCn = math.sin(Cn);
                double cosCn = math.cos(Cn);
                double sinhCe = math.sinh(Ce);
                Ce = math.atan2(sinhCe, cosCn);
                double modulusCe = MathUtils.Hypot(sinhCe, cosCn);
                double rr = MathUtils.Hypot(sinCn, modulusCe);
                Cn = math.atan2(sinCn, modulusCe);
                double lat = math.degrees(DatumUtils.Burst.ConvertAuxiliaryLatitude(Cn, sinCn / rr, modulusCe / rr, projection.ellipsoid.coefficientsCG));
                double lon = math.degrees(Ce + lon0);
                Coord wgs84 = new(lon, lat, CRS.WGS84);

                // Datum Transformation（大地基準轉換）
                if (projection.HasTransform)
                {
                    EllipsoidDefinition wgs84Ellipsoid = new(6378137, 298.257223563, true);
                    wgs84 = DatumUtils.Burst.ConvertToGeocentric(wgs84, projection.ellipsoid);
                    wgs84 = projection.transform.ConvertToWGS84(wgs84);
                    wgs84 = DatumUtils.Burst.ConvertFromGeocentric(wgs84, wgs84Ellipsoid);
                }

                wgs84.z = tm.z;
                return wgs84;
            }

            return new(double.MaxValue, double.MaxValue, tm.z, CRS.WGS84);
        }

        /// <summary>
        /// Transform UTM coordinates to WGS84 coordinates.
        /// （將 UTM 坐標轉換為 WGS84 坐標。）
        /// </summary>
        /// <param name="utm">The UTM coordinate.（UTM 坐標。）</param>
        /// <returns>The WGS84 coordinate.（WGS84 坐標。）</returns>
        public static Coord UTMToWGS84(Coord utm)
        {
            // Constants（常數）
            EllipsoidDefinition wgs84Ellipsoid = new(6378137, 298.257223563, true);
            double a = wgs84Ellipsoid.a;
            double e = wgs84Ellipsoid.E1Square;
            double fE = 5E5;
            double fN = (utm.Hemisphere == Hemisphere.North) ? 0 : 1E7;
            double sf = 0.9996;

            // Intermediate Values（中繼值）
            double EST = utm.x - fE;
            double NOR = utm.y - fN;
            double CLON = (utm.UTMZone - 1) * 6 + 3 - 180;
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

            return new(LON, LAT, utm.z, CRS.WGS84);
        }

        /// <summary>
        /// Transform WGS84 coordinates to Pseudo Mercator coordinates.
        /// （將 WGS84 坐標轉換為偽麥卡托坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <returns>The Pseudo Mercator coordinate.（偽麥卡托坐標。）</returns>
        public static Coord WGS84ToPseudoMercator(Coord wgs84)
        {
            // Constants（常數）
            double a = 6378137;

            // Intermediate Values（中繼值）
            double LATr = wgs84.y / 180 * Math.PI;
            double LONr = wgs84.x / 180 * Math.PI;

            // Pseudo Mercator Coordinates（偽麥卡托坐標）
            double X = a * LONr;
            double Y = a * Math.Log(Math.Tan(Math.PI / 4 + LATr / 2));

            return new(X, Y, wgs84.z, CRS.PseudoMercator);
        }

        /// <summary>
        /// Convert a WGS84 coordinate to a Transverse Mercator coordinate.
        /// （將 WGS84 坐標轉換為橫麥卡托投影坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <param name="projection">The definition of the Transverse Mercator projection.（橫麥卡托投影的定義。）</param>
        /// <returns>The converted Transverse Mercator coordinate.（轉換後的橫麥卡托投影坐標。）</returns>
        public static Coord WGS84ToTransverseMercator(Coord wgs84, ProjectionDefinition projection)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. exact_e_fwd()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L293
             */

            // Version 1.0.4: Save a copy of the height, so the output z value is correct.
            // （1.0.4 版本：儲存一個高度的複本，並使用其作為輸出的 z 值。）

            double z = wgs84.z;

            // Datum Transformation（大地基準轉換）
            if (projection.HasTransform)
            {
                EllipsoidDefinition wgs84Ellipsoid = new(6378137, 298.257223563, true);
                wgs84 = DatumUtils.Burst.ConvertToGeocentric(wgs84, wgs84Ellipsoid);
                wgs84 = projection.transform.ConvertFromWGS84(wgs84);
                wgs84 = DatumUtils.Burst.ConvertFromGeocentric(wgs84, projection.ellipsoid);
            }

            // Initial values（初始值）
            double lat = math.radians(wgs84.y);
            double lon = math.radians(wgs84.x);
            double lon0 = math.radians(projection.origin.x);

            // Intermediate values（中繼值）
            double Ce = lon - lon0;
            double Cn = DatumUtils.Burst.ConvertAuxiliaryLatitude(lat, projection.ellipsoid.coefficientsGC);
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
            double r = DatumUtils.Burst.ClenshawSum(sinArgR, cosArgR, sinhArgI, coshArgI, projection.ellipsoid.coefficientsCR, out double i);
            Ce += i;
            Cn += r;

            if (math.abs(Ce) <= 2.623395162778)
            {
                double x = projection.meridianQuadrant * Ce * projection.ellipsoid.a + projection.shift.x;
                double y = (projection.meridianQuadrant * Cn + projection.radiusVector) * projection.ellipsoid.a + projection.shift.y;
                return new(x, y, z, CRS.TransverseMercator);
            }
            else
            {
                return new(double.MaxValue, double.MaxValue, z, CRS.TransverseMercator);
            }
        }

        /// <summary>
        /// Transform WGS84 coordinates to UTM coordinates.
        /// （將 WGS84 坐標轉換為 UTM 坐標。）
        /// </summary>
        /// <param name="wgs84">The WGS84 coordinate.（WGS84 坐標。）</param>
        /// <returns>The UTM coordinate.（UTM 坐標。）</returns>
        public static Coord WGS84ToUTM(Coord wgs84)
        {
            // Constants（常數）
            EllipsoidDefinition wgs84Ellipsoid = new(6378137, 298.257223563, true);
            double a = wgs84Ellipsoid.a;
            double e = wgs84Ellipsoid.E1Square;
            double fE = 5E5;
            double fN = (wgs84.y >= 0) ? 0 : 1E7;
            Hemisphere hemisphere = (wgs84.y >= 0) ? Hemisphere.North : Hemisphere.South;
            double sf = 0.9996;

            // Intermediate Values（中繼值）
            double LATr = wgs84.y / 180 * Math.PI;
            double LONr = wgs84.x / 180 * Math.PI;
            double ZONE = Math.Truncate((wgs84.x + 180) / 6) + 1;
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

            return new(X, Y, wgs84.z, hemisphere, (int)ZONE);
        }
    }
}