using System;
using System.Globalization;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides utility functions to numbers.
    /// （提供數學相關功能的類別。）
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// The inverse hyperbolic cosine.（逆雙曲餘弦函數。）
        /// </summary>
        /// <param name="x">The input number.（輸入的數字。）</param>
        /// <returns>The original angle.（原始角度。）</returns>
        public static double Acosh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x - 1));
        }

        /// <summary>
        /// The inverse hyperbolic sine.（逆雙曲正弦函數。）
        /// </summary>
        /// <param name="x">The input number.（輸入的數字。）</param>
        /// <returns>The original angle.（原始角度。）</returns>
        public static double Asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x + 1));
        }

        /// <summary>
        /// The inverse hyperbolic tangent.（逆雙曲正切函數。）
        /// </summary>
        /// <param name="x">The input number.（輸入的數字。）</param>
        /// <returns>The original angle.（原始角度。）</returns>
        public static double Atanh(double x)
        {
            return Math.Log((1 + x) / (1 - x)) / 2;
        }
        
        /// <summary>
        /// Retrieve the digit count of the number.
        /// （獲得數字的位數。）
        /// </summary>
        /// <param name="number">The input number.（輸入的數字。）</param>
        /// <param name="decimalDigits">The number of place after the period.（小數點後的位數。）</param>
        /// <param name="countSign">Whether to count the sign as a digit.（是否要將負號視為一位。）</param>
        /// <param name="countSign">Whether to count the period as a digit.（是否要將小數點視為一位。）</param>
        /// <returns>The number of digits.（數字的位數。）</returns>
        public static int GetDigits(float number, out int decimalDigits, bool countSign = false,  bool countPoint = false)
        {
            string[] parts = number.ToString("G", CultureInfo.InvariantCulture).Split('.');
            int total = parts[0].Length;
            decimalDigits = 0;

            if (!countSign && parts[0].StartsWith("-"))
            {
                total--;
            }

            if (parts.Length > 1)
            {
                decimalDigits = parts[1].Length;
                total += decimalDigits;
                if (countPoint) total++;
            }

            return total;
        }
        
        /// <summary>
        /// Retrieve the digit count of the number.
        /// （獲得數字的位數。）
        /// </summary>
        /// <param name="number">The input number.（輸入的數字。）</param>
        /// <param name="countSign">Whether to count the sign as a digit.（是否要將負號視為一位。）</param>
        /// <returns>The number of digits.（數字的位數。）</returns>
        public static int GetDigits(int number, bool countSign = false)
        {
            if (number >= 0)
            {
                return number switch
                {
                    > 999999999 => 10,
                    > 99999999 => 9,
                    > 9999999 => 8,
                    > 999999 => 7,
                    > 99999 => 6,
                    > 9999 => 5,
                    > 999 => 4,
                    > 99 => 3,
                    > 9 => 2,
                    _ => 1,
                };
            }
            else
            {
                return number switch
                {
                    < -999999999 => countSign ? 11 : 10,
                    < -99999999 => countSign ? 10 : 9,
                    < -9999999 => countSign ? 9 : 8,
                    < -999999 => countSign ? 8 : 7,
                    < -99999 => countSign ? 7 : 6,
                    < -9999 => countSign ? 6 : 5,
                    < -999 => countSign ? 5 : 4,
                    < -99 => countSign ? 4 : 3,
                    < -9 => countSign ? 3 : 2,
                    _ => countSign ? 2 : 1,
                };
            }
        }

        /// <summary>
        /// Retrieve the digit count of the number.
        /// （獲得數字的位數。）
        /// </summary>
        /// <param name="number">The input number.（輸入的數字。）</param>
        /// <param name="decimalDigits">The number of place after the period.（小數點後的位數。）</param>
        /// <param name="countSign">Whether to count the sign as a digit.（是否要將負號視為一位。）</param>
        /// <param name="countSign">Whether to count the period as a digit.（是否要將小數點視為一位。）</param>
        /// <returns>The number of digits.（數字的位數。）</returns>
        public static int GetDigitsBurstCompatible(float number, out int decimalDigits, bool countSign = false, bool countPoint = false)
        {
            float decimalParts = Math.Abs(number);
            int total = GetDigits((int)decimalParts) + (countSign && (number < 0) ? 1 : 0);
            decimalParts -= (int)decimalParts;
            decimalDigits = 0;
            int trailingZeros = 0;
            while (decimalDigits < 7 && Math.Round(decimalParts, 4) > 0)
            {
                decimalParts *= 10;
                if ((int)decimalParts == 0)
                {
                    trailingZeros++;
                }
                else
                {
                    decimalDigits += 1 + trailingZeros;
                    trailingZeros = 0;
                }

                total++;
                decimalParts -= (int)decimalParts;
            }
            return total + (countPoint && (decimalDigits > 0) ? 1 : 0);
        }

        /// <summary>
        /// Computes the hypotenuse given two values representing the lengths of the shorter sides in a right-angled triangle.
        /// （計算給定兩個較短邊長時，直角三角形中斜邊的長度。）
        /// </summary>
        /// <param name="x">The value to square and add to <paramref name="y"/>.（要平方並且與  <paramref name="y"/> 相加的數。）</param>
        /// <param name="y">The value to square and add to <paramref name="x"/>.（要平方並且與  <paramref name="x"/> 相加的數。）</param>
        /// <returns>The square root of <paramref name="x"/>-squared plus <paramref name="y"/>-squared.（<paramref name="x"/> 平方與 <paramref name="y"/> 平方的總和平方根。）</returns>
        public static double Hypot(double x, double y)
        {
            /*
                 #  References: （資料來源：）

                    * Microsoft. (2025). Single.cs. Hypot()
                        https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Single.cs#L1507
             */

            double result;
            if (!double.IsInfinity(x) && !double.IsInfinity(y))
            {
                double ax = Math.Abs(x);
                double ay = Math.Abs(y);

                if (ax == 0.0f)
                {
                    result = ay;
                }
                else if (ay == 0.0)
                {
                    result = ax;
                }
                else
                {
                    double xx = ax;
                    xx *= xx;

                    double yy = ay;
                    yy *= yy;

                    result = Math.Sqrt(xx + yy);
                }
            }
            else if (double.IsInfinity(x) || double.IsInfinity(y))
            {
                result = double.PositiveInfinity;
            }
            else
            {
                result = double.NaN;
            }

            return result;
        }
    }
}