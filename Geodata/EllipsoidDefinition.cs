using System;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of an ellipsoid.
    /// （橢球體的定義。）
    /// </summary>
    public struct EllipsoidDefinition : IEquatable<EllipsoidDefinition>
    {
        /// <summary>
        /// The semi-major axis length of the ellipsoid in meters.（橢球體的半長軸長度，單位為公尺。）
        /// </summary>
        public double a;

        /// <summary>
        /// The semi-minor axis length of the ellipsoid in meters.（橢球體的半短軸長度，單位為公尺。）
        /// </summary>
        public double b;

        /// <summary>
        /// The eccentricity of the ellipsoid.（橢球體的偏心率。）
        /// </summary>
        public double eSquare;

        /// <summary>
        /// The flattening of the ellipsoid.（橢球體扁平率。）
        /// </summary>
        public double f;

        /// <summary>
        /// The reciprocal of the flattening of the ellipsoid; inverse flattening.（橢球體扁平率的倒數。）
        /// </summary>
        public double rf;
        
        public EllipsoidDefinition(double semiMajor, double semiMinor, double inverseFlattening)
        {
            a = semiMajor;
            b = semiMinor;
            rf = inverseFlattening;
            f = 1 / rf;
            eSquare = f * (2 - f);
        }

        public EllipsoidDefinition(Ellipsoid ellipsoid)
        {
            switch (ellipsoid)
            {
                case Ellipsoid.GRS80:
                    a = 6378137;
                    rf = 298.257222101;
                    break;

                case Ellipsoid.WGS84:
                default:
                    a = 6378137;
                    rf = 298.257223563;
                    break;
            }

            f = 1 / rf;
            b = a * (1 - f);
            eSquare = f * (2 - f);
        }

        public readonly bool Equals(EllipsoidDefinition other)
        {
            return (a == other.a) & (b == other.b) & (f == other.f);
        }
    }
}