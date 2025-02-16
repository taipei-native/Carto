using Carto.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Carto.IO
{
    /// <summary>
    /// The class that provides utility functions to write GeoTIFF.
    /// （提供寫出 GeoTIFF 功能的類別。）<br/>
    /// </summary>
    public static class GeoTiff
    {
        /// <summary>
        /// Check whether the endianess is little endian.
        /// （確認端序是否為由小至大。）
        /// </summary>
        private static readonly bool _littleEndian = BitConverter.IsLittleEndian;

        /// <summary>
        /// The type indicating a 8-bit unsigned integer. It is equivalent to <see cref="byte"/> in C#.<br/>
        /// （代表 8 位元不帶正負號的整數。相當於 C# 的 <see cref="byte"/>。）
        /// </summary>
        public const short fieldTypeByte = 1;

        /// <summary>
        /// The type indicating a 7-bit ASCII code ending with NUL.<br/>
        /// （代表 7 位元的 ASCII 碼，其中最後一個字元為 NUL。）
        /// </summary>
        public const short fieldTypeAscii = 2;

        /// <summary>
        /// The type indicating a 16-bit unsigned integer. It is equivalent to <see cref="ushort"/> in C#.<br/>
        /// （代表 16 位元不帶正負號的整數。相當於 C# 的 <see cref="ushort"/>。）
        /// </summary>
        public const short fieldTypeShort = 3;

        /// <summary>
        /// The type indicating a 32-bit unsigned integer. It is equivalent to <see cref="uint"/> in C#.<br/>
        /// （代表 32 位元不帶正負號的整數。相當於 C# 的 <see cref="uint"/>。）
        /// </summary>
        public const short fieldTypeLong = 4;

        /// <summary>
        /// The type indicating a fraction. Composed by two 32-bit unsigned integers (<see cref="uint"/>),<br/>
        /// the first one indicates the numerator, while the last one indicates the denominator.<br/>
        /// （代表分數。由兩個 32 位元不帶正負號的整數（<see cref="uint"/>）組成，前者為分子，後者為分母。）
        /// </summary>
        public const short fieldTypeRational = 5;

        /// <summary>
        /// The type indicating a 8-bit signed integer. It is equivalent to <see cref="sbyte"/> in C#.<br/>
        /// （代表 8 位元帶正負號的整數。相當於 C# 的 <see cref="sbyte"/>。）
        /// </summary>
        public const short fieldTypeSbyte = 6;

        /// <summary>
        /// The type that might contain anything within a 8-bit byte.<br/>
        /// （代表長度為 8 位元組的任意資料。）
        /// </summary>
        public const short fieldTypeUndefined = 7;

        /// <summary>
        /// The type indicating a 16-bit signed integer. It is equivalent to <see cref="short"/> in C#.<br/>
        /// （代表 16 位元帶正負號的整數。相當於 C# 的 <see cref="short"/>。）
        /// </summary>
        public const short fieldTypeSShort = 8;

        /// <summary>
        /// The type indicating a 32-bit signed integer. It is equivalent to <see cref="int"/> in C#.<br/>
        /// （代表 32 位元帶正負號的整數。相當於 C# 的 <see cref="int"/>。）
        /// </summary>
        public const short fieldTypeSLong = 9;

        /// <summary>
        /// The type indicating a fraction. Composed by two 32-bit signed integers (<see cref="int"/>),<br/>
        /// the first one indicates the numerator, while the last one indicates the denominator.<br/>
        /// （代表分數。由兩個 32 位元帶正負號的整數（<see cref="int"/>）組成，前者為分子，後者為分母。）
        /// </summary>
        public const short fieldTypeSRational = 10;

        /// <summary>
        /// The type indicating a single-precision floating-point number. It is equivalent to <see cref="float"/> in C#.<br/>
        /// （代表單精度浮點數。相當於 C# 的 <see cref="float"/>。）
        /// </summary>
        public const short fieldTypeFloat = 11;

        /// <summary>
        /// The type indicating a double-precision floating-point number. It is equivalent to <see cref="double"/> in C#.<br/>
        /// （代表雙精度浮點數。相當於 C# 的 <see cref="double"/>。）
        /// </summary>
        public const short fieldTypeDouble = 12;

        /// <summary>
        /// The look-up table for each tag's type.
        /// （每個標籤代表型別的對照表。）
        /// </summary>
        public static Dictionary<int, short> TagTypeTable = new()
        {
            { 256, fieldTypeShort },
            { 257, fieldTypeShort },
            { 258, fieldTypeShort },
            { 259, fieldTypeShort },
            { 262, fieldTypeShort },
            { 273, fieldTypeLong },
            { 277, fieldTypeShort },
            { 278, fieldTypeShort },
            { 279, fieldTypeShort },
            { 284, fieldTypeShort },
            { 305, fieldTypeAscii },
            { 306, fieldTypeAscii },
            { 339, fieldTypeShort },
            { 33550, fieldTypeDouble },
            { 33922, fieldTypeDouble },
            { 34735, fieldTypeShort },
            { 34737, fieldTypeAscii },
            { 42113, fieldTypeAscii }
        };
        
        /// <summary>
        /// Write the GeoTIFF file.
        /// （寫出 GeoTIFF 檔案。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public static void Write(Options options, Action<string, int> onReportMethod)
        {
            /*
                # References: （資料來源：）

                * Aldus Developers Desk. (1992). TIFF™ Revision 6.0
                    https://www.itu.int/itudoc/itu-t/com16/tiff-fx/docs/tiff6.pdf

                * Open Geospatial Consortium. (2019). OGC GeoTIFF Standard
                    https://docs.ogc.org/is/19-008r4/19-008r4.html
            */

            Stopwatch stopwatch = Stopwatch.StartNew();
            if (options == null) throw new ArgumentNullException("The parameters cannot be null. 參數不可為空值。");
            string filePath = options.FilePath;

            using FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 81920);
            using BinaryWriter writer = new(fs);

            WriteTag(writer, 256, 1, 4096);

            stopwatch.Stop();
            Instance.Log.Debug($"Write '{Path.GetFileName(filePath)}' in {CommonUtils.FormatTimeSpan(stopwatch.Elapsed)}.");
        }

        /// <summary>
        /// Write a TIFF / EXIF tag to the file.
        /// （寫入一個 TIFF／EXIF 標籤至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="number">The unique code of the tag.（標籤的獨特代碼。）</param>
        /// <param name="count">The number of values.（數值的數量。）</param>
        /// <param name="value">The value waiting to be written. In most of the time, it represents the value offset in bytes.<br/>
        /// （等待被寫入的數值，通常代表數值位移量，以位元組計。）</param>
        public static void WriteTag(BinaryWriter writer, int number, int count, int value)
        {
            if (!TagTypeTable.TryGetValue(number, out short type))
            {
                throw new KeyNotFoundException($"The tag number `{number}` is not in TagTypeTable. 標籤代號 {number} 未紀錄於 TagTypeTable。");
            }

            if (_littleEndian)
            {
                writer.Write(BitConverter.GetBytes((short)number));
                writer.Write(BitConverter.GetBytes(type));
                writer.Write(BitConverter.GetBytes(count));
                writer.Write(BitConverter.GetBytes(value));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes((short)number));
                writer.Write(IOUtils.GetFlippedBytes(type));
                writer.Write(IOUtils.GetFlippedBytes(count));
                writer.Write(IOUtils.GetFlippedBytes(value));
            }
        }
    }
}