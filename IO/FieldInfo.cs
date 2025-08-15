using System;
using System.Text;

namespace Carto.IO
{
    /// <summary>
    /// The length and type information of a field from a DBF file.
    /// （DBF 檔案欄位的長度與型別資訊。）
    /// </summary>
    public struct FieldInfo : IEquatable<FieldInfo>
    {
        /// <summary>
        /// The maximum decimal length of a float field for Shapefiles.
        /// （Shapefile 浮點數欄位最大小數點後長度。）
        /// </summary>
        public const int shpFloatDecimalLength = 7;

        /// <summary>
        /// The maximum total length of a float field for Shapefiles.
        /// （Shapefile 浮點數欄位最大總長度。）
        /// </summary>
        public const int shpFloatLength = 18;

        /// <summary>
        /// The maximum total length of a int field for Shapefiles.
        /// （Shapefile 整數欄位最大總長度。）
        /// </summary>
        public const int shpIntLength = 11;

        /// <summary>
        /// The maximum total length of a character field for Shapefiles.
        /// （Shapefile 字串欄位最大總長度。）
        /// </summary>
        public const int shpStringLength = 254;

        /// <summary>
        /// The number of places after the period.
        /// （小數點後的位數。）
        /// </summary>
        public readonly int decimalLength;

        /// <summary>
        /// The total length of the value in bytes.
        /// （以位元組計的數值總長度。）
        /// </summary>
        public readonly int length;

        /// <summary>
        /// Whether to represent the value in scientific notation or not.
        /// （是否以科學記號表示數值？）
        /// </summary>
        public readonly bool scientific;

        /// <summary>
        /// The type of the field.
        /// （欄位的型別。）
        /// </summary>
        public readonly FieldType type;

        public FieldInfo(bool value)
        {
            decimalLength = 0;
            length = 1;
            scientific = false;
            type = FieldType.Bool;
        }

        public FieldInfo(float value)
        {
            length = Utils.MathUtils.GetDigitsBurstCompatible(value, out int decimalLength, true, true);
            scientific = false;
            type = FieldType.Float;
            int excessiveLength = length - shpFloatLength;

            if (excessiveLength > 0)
            {
                if (excessiveLength <= decimalLength)
                {
                    decimalLength -= excessiveLength;
                    length = shpFloatLength;
                }
                else
                {
                    decimalLength = 0;
                    length -= excessiveLength - decimalLength;
                }
            }

            if (length > shpFloatLength)
            {
                scientific = true;
                length = shpFloatLength;
            }

            if (decimalLength > shpFloatDecimalLength) decimalLength = shpFloatDecimalLength;
            this.decimalLength = decimalLength;
        }

        public FieldInfo(int value)
        {
            decimalLength = 0;
            length = Utils.MathUtils.GetDigits(value, true);
            scientific = false;
            type = FieldType.Int;
        }

        public FieldInfo(string value)
        {
            decimalLength = 0;
            length = Encoding.UTF8.GetByteCount(value) + 6;
            if (length > shpStringLength)
            {
                length = shpStringLength;
            }
            scientific = false;
            type = FieldType.String;
        }

        public FieldInfo(int decimalLength, int length, bool scientific, FieldType type)
        {
            this.decimalLength = decimalLength;
            this.length = length;
            this.scientific = scientific;
            this.type = type;
        }

        public readonly bool Equals(FieldInfo other) => (type == other.type) && (length == other.length) && (decimalLength == other.decimalLength) && (scientific == other.scientific);

        public override readonly bool Equals(object obj) => obj is FieldInfo other && Equals(other);

        public override readonly int GetHashCode()
        {
            unchecked
            {
                int hash = 17 * 31 + decimalLength.GetHashCode();
                hash = hash * 31 + length.GetHashCode();
                hash = hash * 31 + scientific.GetHashCode();
                return hash * 31 + type.GetHashCode();
            }
        }

        /// <summary>
        /// Retrieve the column type symbol used in the .dbf files.
        /// （獲得用於 .dbf 檔案欄位型別的符號。）
        /// </summary>
        /// <returns>The char symbol.（字元符號。）</returns>
        public readonly char GetSymbol()
        {
            return type switch
            {
                FieldType.Bool => Shapefile.fieldTypeNumber,
                FieldType.Float => Shapefile.fieldTypeFloat,
                FieldType.Int => Shapefile.fieldTypeNumber,
                FieldType.String => Shapefile.fieldTypeCharacter,
                _ => Shapefile.fieldTypeCharacter
            };
        }

        public override readonly string ToString() => $"FieldInfo ({type}) - Length [{length}], DecimalLength [{decimalLength}]";

        public static FieldInfo operator +(FieldInfo left, FieldInfo right)
        {
            FieldType fieldType = left.type;
            if (fieldType != right.type) fieldType = FieldType.String;
            return new(fieldType == FieldType.String ? 0 : Math.Max(left.decimalLength, right.decimalLength),
                       Math.Max(left.length, right.length),
                       left.scientific || right.scientific,
                       fieldType);
        }
    }
}