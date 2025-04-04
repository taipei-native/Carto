using System;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of a projection.
    /// （投影法的定義。）
    /// </summary>
    public struct ProjectionDefinition: IEquatable<ProjectionDefinition>
    {
        /// <summary>
        /// The reference ellipsoid of the projection.
        /// （投影法的參考橢球體。）
        /// </summary>
        public EllipsoidDefinition ellipsoid;

        /// <summary>
        /// The origin of the projection coordinate system in WGS84.
        /// （投影法坐標系的 WGS84 原點。）
        /// </summary>
        public (double longitude, double latitude) origin;

        /// <summary>
        /// The shift of the coordinates, and it is also known as false easting (x axis) and false northing (y axis).
        /// （坐標的位移，通常被稱為東移量（X 方向）或北移量（Y 方向）。）
        /// </summary>
        public (double easting, double northing) shift;

        /// <summary>
        /// The ratio of the unit length at the origin of the projection to the true ground distance.
        /// （投影法原點的單位長度與真實地面距離的比值。）
        /// </summary>
        public double scaleFactor;

        /// <summary>
        /// The Helmert Transform parameters, and its length should be either 0, 3, or 7.
        /// （赫爾默特轉換參數，其長度應為 0、3 或 7。）
        /// </summary>
        public double[] transform;

        public ProjectionDefinition(EllipsoidDefinition ellipsoid, (double longitude, double latitude) origin, (double easting, double northing) shift, double scaleFactor, double[] transform)
        {
            this.ellipsoid = ellipsoid;
            this.origin = origin;
            this.shift = shift;
            this.scaleFactor = scaleFactor;
            int length = transform.Length;

            if (length == 0 || length == 3 || length == 7)
            {
                this.transform = transform;
            }
            else
            {
                throw new ArgumentException("The length of the transform parameters should be either 0, 3 or 7. 轉換參數的長度應為 0、3 或 7。");
            }
        }

        public override readonly bool Equals(object obj)
        {
            return obj is ProjectionDefinition other && Equals(other);
        }

        public readonly bool Equals(ProjectionDefinition other)
        {
            return ellipsoid.Equals(other.ellipsoid) &&
                   (origin == other.origin) &&
                   (shift == other.shift) &&
                   (scaleFactor == other.scaleFactor) &&
                   (transform == other.transform);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + ellipsoid.GetHashCode();
                hash = hash * 31 + origin.GetHashCode();
                hash = hash * 31 + shift.GetHashCode();
                hash = hash * 31 + scaleFactor.GetHashCode();
                hash = hash * 31 + transform.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Check whether the projection has Helmert Transform parameters.
        /// （確認投影法是否有赫爾默特轉換參數。）
        /// </summary>
        /// <returns>
        /// True if the length of <see cref="transform"/>'s length is longer than 0.<br/>
        /// （當 <see cref="transform"/> 長度大於 0 時傳回真值。） 
        /// </returns>
        public readonly bool HasTransform()
        {
            return transform.Length > 0;
        }

        public override readonly string ToString()
        {
            string transformString = transform.Length == 0 ? string.Empty : string.Join(" ", transform);
            return $"Projection - Ellipsoid [{ellipsoid}], Origin [{origin.longitude}, {origin.latitude}], Scale Factor [{scaleFactor}], Shift [{shift.easting}, {shift.northing}], Transform [{transformString}]";
        }

        public static bool operator ==(ProjectionDefinition left, ProjectionDefinition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ProjectionDefinition left, ProjectionDefinition right)
        {
            return !left.Equals(right);
        }
    }
}