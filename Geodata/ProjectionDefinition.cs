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

        public ProjectionDefinition(ProjectionDefinitionSafe projection)
        {
            ellipsoid = default;
            origin = projection.origin;
            shift = projection.shift;
            scaleFactor = projection.scaleFactor;
            if (projection.transformA == double.MinValue)
            {
                transform = new double[0];
            }
            else if (projection.transformD == double.MinValue)
            {
                transform = new double[3];
                transform[0] = projection.transformA;
                transform[1] = projection.transformB;
                transform[2] = projection.transformC;
            }
            else if (projection.transformG != double.MinValue)
            {
                transform = new double[7];
                transform[0] = projection.transformA;
                transform[1] = projection.transformB;
                transform[2] = projection.transformC;
                transform[3] = projection.transformD;
                transform[4] = projection.transformE;
                transform[5] = projection.transformF;
                transform[6] = projection.transformG;
            }
            else
            {
                transform = new double[0];
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

        public readonly ProjectionDefinitionSafe ToSafe() => new(this);

        public static bool operator ==(ProjectionDefinition left, ProjectionDefinition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ProjectionDefinition left, ProjectionDefinition right)
        {
            return !left.Equals(right);
        }
    }

    public struct ProjectionDefinitionSafe
    {
        public (double longitude, double latitude) origin;
        public (double easting, double northing) shift;
        public double scaleFactor;
        public double transformA;
        public double transformB;
        public double transformC;
        public double transformD;
        public double transformE;
        public double transformF;
        public double transformG;

        public ProjectionDefinitionSafe(ProjectionDefinition projection)
        {
            origin = projection.origin;
            shift = projection.shift;
            scaleFactor = projection.scaleFactor;
            transformA = double.MinValue;
            transformB = double.MinValue;
            transformC = double.MinValue;
            transformD = double.MinValue;
            transformE = double.MinValue;
            transformF = double.MinValue;
            transformG = double.MinValue;

            if (projection.transform.Length == 3)
            {
                transformA = projection.transform[0];
                transformB = projection.transform[1];
                transformC = projection.transform[2];
            }
            if (projection.transform.Length == 7)
            {
                transformD = projection.transform[3];
                transformE = projection.transform[4];
                transformF = projection.transform[5];
                transformG = projection.transform[6];
            }
        }

        public ProjectionDefinitionSafe((double longitude, double latitude) origin, (double easting, double northing) shift, double scaleFactor, double[] transform)
        {
            this.origin = origin;
            this.shift = shift;
            this.scaleFactor = scaleFactor;
            transformA = double.MinValue;
            transformB = double.MinValue;
            transformC = double.MinValue;
            transformD = double.MinValue;
            transformE = double.MinValue;
            transformF = double.MinValue;
            transformG = double.MinValue;

            if (transform.Length == 3)
            {
                transformA = transform[0];
                transformB = transform[1];
                transformC = transform[2];
            }
            if (transform.Length == 7)
            {
                transformD = transform[3];
                transformE = transform[4];
                transformF = transform[5];
                transformG = transform[6];
            }
        }

        public readonly ProjectionDefinition ToUnsafe() => new(this);
    }
}