using System;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of the auxiliary latitude coefficients.
    /// （輔助緯度係數的定義。）
    /// </summary>
    public struct Coefficients
    {
        /// <summary>
        /// The first number of the coefficients.
        /// （第一個係數。）
        /// </summary>
        public double a;

        /// <summary>
        /// The second number of the coefficients.
        /// （第二個係數。）
        /// </summary>
        public double b;

        /// <summary>
        /// The third number of the coefficients.
        /// （第三個係數。）
        /// </summary>
        public double c;

        /// <summary>
        /// The fourth number of the coefficients.
        /// （第四個係數。）
        /// </summary>
        public double d;

        /// <summary>
        /// The fifth number of the coefficients.
        /// （第五個係數。）
        /// </summary>
        public double e;

        /// <summary>
        /// The sixth number of the coefficients.
        /// （第六個係數。）
        /// </summary>
        public double f;

        public double this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => a,
                    1 => b,
                    2 => c,
                    3 => d,
                    4 => e,
                    5 => f,
                    _ => throw new IndexOutOfRangeException("The auxilary latitude coefficient has only 6 values. 輔助緯度係數僅有 6 個參數。")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: a = value; break;
                    case 1: b = value; break;
                    case 2: c = value; break;
                    case 3: d = value; break;
                    case 4: e = value; break;
                    case 5: f = value; break;
                    default:
                        throw new IndexOutOfRangeException("The auxilary latitude coefficient has only 6 values. 輔助緯度係數僅有 6 個參數。");
                }
            }
        }

        public override readonly string ToString()
        {
            return $"Coefficients({a}, {b}, {c}, {d}, {e}, {f})";
        }
    }
}