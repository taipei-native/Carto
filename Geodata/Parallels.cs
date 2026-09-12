using System;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of standard parallels.
    /// （標準平行線的定義。）
    /// </summary>
    public struct Parallels
    {
        /// <summary>
        /// The latitude of the first standard parallel.
        ///（第一標準平行線的緯度。）
        /// </summary>
        public double first;

        /// <summary>
        /// The latitude of the second standard parallel.
        /// （第二標準平行線的緯度。）
        /// </summary>
        public double second;

        public Parallels(double[] param)
        {
            first = double.MinValue;
            second = double.MinValue;
            int paramCount = param.Length;

            if (paramCount > 2)
            {
                throw new ArgumentException("The number of standard parallels should be either 0, 1, or 2. 標準平行線數量應為 0、1 或 2。");
            }

            if (paramCount >= 1)
            {
                first = param[0];

                if (paramCount == 2)
                {
                    second = param[1];
                }
            }
        }

        public double this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => first,
                    1 => second,
                    _ => throw new IndexOutOfRangeException("Only the first and the second standard parallels are supported. 僅支援第一和第二標準平行線。")
                };
            }
            set
            {
                switch (index) {
                    case 0: first = value; break;
                    case 1: second = value; break;
                    default:
                        throw new IndexOutOfRangeException("Only the first and the second standard parallels are supported. 僅支援第一和第二標準平行線。");
                }
            }
        }

        /// <summary>
        /// The number of standard parallels included in the structure.
        /// （結構內包含的標準平行線數量。）
        /// </summary>
        public readonly int Count => (first != double.MinValue ? 1 : 0) + (second != double.MinValue ? 1 : 0);

        /// <summary>
        /// Obtain a Parallels struct with no parallels.
        /// （取得無標準平行線的 Parallels 結構。）
        /// </summary>
        public static Parallels None => new()
        {
            first = double.MinValue,
            second = double.MinValue
        };

        public override readonly string ToString()
        {
            string firstValue = first == double.MinValue ? string.Empty : first.ToString("F5");
            string secondValue = second == double.MinValue ? string.Empty : second.ToString("F5");
            return $"Parallels({firstValue}, {secondValue})";
        }
    }
}