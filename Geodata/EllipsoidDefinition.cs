using Carto.Utils;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of an ellipsoid.
    /// （橢球體的定義。）
    /// </summary>
    public struct EllipsoidDefinition
    {
        /// <summary>
        /// The semi-major axis length of the ellipsoid in meters.（橢球體的半長軸長度，單位為公尺。）
        /// </summary>
        public double a;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the conformal latitude (χ) to the geographic latitude (φ).<br/>
        /// （用以將等角緯度（χ）轉換為地理緯度（φ）的係數。）
        /// </summary>
        public Coefficients coefficientsCG;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the conformal latitude (χ) to the rectifying latitude (μ).<br/>
        /// （用以將等角緯度（χ）轉換為矯正緯度（μ）的係數。）
        /// </summary>
        public Coefficients coefficientsCR;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the geographic latitude (φ) to the conformal latitude (χ).<br/>
        /// （用以將地理緯度（φ）轉換為等角緯度（χ）的係數。）
        /// </summary>
        public Coefficients coefficientsGC;

        /// <summary>
        /// The auxiliary latitude coefficients to convert the rectifying latitude (μ) to the conformal latitude (χ).<br/>
        /// （用以將矯正緯度（μ）轉換為等角緯度（χ）的係數。）
        /// </summary>
        public Coefficients coefficientsRC;

        /// <summary>
        /// The semi-minor axis length of the ellipsoid in meters.（橢球體的半短軸長度，單位為公尺。）
        /// </summary>
        public double b;

        /// <summary>
        /// The flattening of the ellipsoid.（橢球體扁平率。）
        /// </summary>
        public double f;

        /// <summary>
        /// The reciprocal of the flattening of the ellipsoid; the inverse flattening.（橢球體扁平率的倒數。）
        /// </summary>
        public double rf;

        public EllipsoidDefinition(double semiMajor, double inverseFlattening)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. PoderEngsager
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L35

                * PROJ contributors. (2025). tmerc.cpp. setup_exact()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L445
             */

            a = semiMajor;
            rf = inverseFlattening;
            f = 1 / rf;
            b = a * (1 - f);

            double n = (a - b) / (a + b);   // The third flattening（第三扁平率。）
            coefficientsCG = DatumUtils.GetCoefficients(n, 4, 0);
            coefficientsCR = DatumUtils.GetCoefficients(n, 4, 3);
            coefficientsGC = DatumUtils.GetCoefficients(n, 0, 4);
            coefficientsRC = DatumUtils.GetCoefficients(n, 3, 4);
        }

        public EllipsoidDefinition(double semiMajor, double inverseFlattening, bool burst)
        {
            /*
                # References: （資料來源：）

                * PROJ contributors. (2025). tmerc.cpp. PoderEngsager
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L35

                * PROJ contributors. (2025). tmerc.cpp. setup_exact()
                    https://github.com/OSGeo/PROJ/blob/master/src/projections/tmerc.cpp#L445
             */

            a = semiMajor;
            rf = inverseFlattening;
            f = 1 / rf;
            b = a * (1 - f);
            coefficientsCG = default;
            coefficientsCR = default;
            coefficientsGC = default;
            coefficientsRC = default;
        }

        /// <summary>
        /// The first eccentricity squared of the ellipsoid.（橢球體的第一偏心率平方。）
        /// </summary>
        public readonly double E1Square => f * (2 - f);

        /// <summary>
        /// The second eccentricity squared of the ellipsoid.（橢球體的第二偏心率平方。）
        /// </summary>
        public readonly double E2Square => E1Square / (1 - E1Square);

        /// <summary>
        /// The third eccentricity squared of the ellipsoid.（橢球體的第三偏心率平方。）
        /// </summary>
        public readonly double E3Square => E1Square / (2 - E1Square);

        /// <summary>
        /// The second flattening of the ellipsoid.（橢球體的第二扁平率。）
        /// </summary>
        public readonly double F2 => (a - b) / b;

        /// <summary>
        /// The third flattening of the ellipsoid.（橢球體的第三扁平率。）
        /// </summary>
        public readonly double F3 => (a - b) / (a + b);

        public override readonly string ToString()
        {
            return $"Ellipsoid({a}, {rf})";
        }
    }
}