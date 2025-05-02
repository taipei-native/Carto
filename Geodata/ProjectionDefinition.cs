using Carto.Utils;
using Unity.Mathematics;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of a projection.
    /// （投影法的定義。）
    /// </summary>
    public struct ProjectionDefinition
    {
        /// <summary>
        /// The reference ellipsoid of the projection.
        /// （投影法的參考橢球體。）
        /// </summary>
        public EllipsoidDefinition ellipsoid;

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

        public ProjectionDefinition(EllipsoidDefinition ellipsoid, double originX, double originY, double shiftX, double shiftY, double scaleFactor, HelmertTransform transform)
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

            meridianQuadrant = scaleFactor * DatumUtils.GetRectifyingRadius(ellipsoid.F3);
            double gaussianLatitude = DatumUtils.Burst.ConvertAuxiliaryLatitude(math.radians(originY), ellipsoid.coefficientsGC);
            radiusVector = -meridianQuadrant * DatumUtils.Burst.ConvertAuxiliaryLatitude(gaussianLatitude, ellipsoid.coefficientsCR);
        }

        public ProjectionDefinition(EllipsoidDefinition ellipsoid, double originX, double originY, double shiftX, double shiftY, double scaleFactor, HelmertTransform transform, bool burst)
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

            meridianQuadrant = 0.0;
            radiusVector = 0.0;
        }

        /// <summary>
        /// Check whether the projection has Helmert Transform parameters.
        /// （確認投影法是否有赫爾默特轉換參數。）
        /// </summary>
        public readonly bool HasTransform => transform.paramCount != 0;
    }
}