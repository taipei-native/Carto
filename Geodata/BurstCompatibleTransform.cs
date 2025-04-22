using Unity.Mathematics;

namespace Carto.Geodata
{
    public struct BurstCompatibleProjectionDefinition
    {
        public EllipsoidDefinition ellipsoid;

        public double2 origin;

        public double2 shift;

        public double scaleFactor;

        public HelmertTransform transform;
    }

    public struct HelmertTransform
    {
        public double x;

        public double y;

        public double z;

        public double rx;

        public double ry;

        public double rz;

        public double s;
    }
    
    /// <summary>
    /// The class that provides transformation between popular CRSs.
    /// （提供數種熱門坐標參考系統間轉換的類別。）
    /// </summary>
    public static class BurstCompatibleTransform
    {
        public static Coord TransverseMercatorToWGS84(Coord tm, BurstCompatibleProjectionDefinition projection)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. exact_e_inv()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L379
             */

            // Constants（常數）
            double lat = 0;
            double lon = 0;
            double x = 

            return new((lon, lat));
        }
    }

    public static class BurstCompatibleDatumUtils
    {
        public static void SetUp(BurstCompatibleProjectionDefinition projection)
        {
            
        }
    }
}