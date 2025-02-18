using Carto.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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
        /// The metadata of the GeoTIFF.
        /// （GeoTIFF 的元資料。）
        /// </summary>
        public struct Parameter
        {
            /// <summary>
            /// The format to export to.
            /// （輸出的格式。）
            /// </summary>
            public GeoTiffFormat format;
            
            /// <summary>
            /// The height of the image in pixel.
            /// （影像以像素計算的高度。）
            /// </summary>
            public int imageHeight;

            /// <summary>
            /// The width of the image in pixel.
            /// （影像以像素計算的寬度。）
            /// </summary>
            public int imageWidth;

            /// <summary>
            /// The nodata value used in the GeoTIFF.
            /// （GeoTIFF 中代表無資料的數值。）
            /// </summary>
            public float nodata;

            /// <summary>
            /// The offset of StripByteCounts (279 / 0x0117) tag.<br/>
            /// （每片段位元組數（279／0x0117）標籤的位移量。）
            /// </summary>
            public int offsetBytesPerStrip;

            /// <summary>
            /// The offset of DateTime (306 / 0x0132) tag.<br/>
            /// （日期與時間（306／0x0132）標籤的位移量。）
            /// </summary>
            public int offsetDateTime;

            /// <summary>
            /// The offset of GDAL_NODATA (42113 / 0xA481) tag.<br/>
            /// （GDAL 無資料值（42113／0xA481）標籤的位移量。）
            /// </summary>
            public int offsetGdalNodata;

            /// <summary>
            /// The offset of GeoAsciiParamsTag (34737 / 0x87B1) tag.<br/>
            /// （地理 ASCII 參數（34737／0x87B1）標籤的位移量。）
            /// </summary>
            public int offsetGeoAsciiParamsTag;

            /// <summary>
            /// The offset of GeoKeyDirectoryTag (34735 / 0x87AF) tag.<br/>
            /// （地理鍵目錄（34735 ／0x87AF）標籤的位移量。）
            /// </summary>
            public int offsetGeoKeyDirectoryTag;

            /// <summary>
            /// The offset of the image file directory (IFD).<br/>
            /// （影像檔案目錄（IFD）的位移量。）
            /// </summary>
            public int offsetIFD;

            /// <summary>
            /// The offset of ModelPixelScaleTag (33550 / 0x830E) tag.<br/>
            /// （空間－像素縮放比例（33550／0x830E）標籤的位移量。）
            /// </summary>
            public int offsetModelPixelScaleTag;

            /// <summary>
            /// The offset of ModelTiepointTag (33922 / 0x8482) tag.<br/>
            /// （模型連接點（33922／0x8482）標籤的位移量。）
            /// </summary>
            public int offsetModelTiepointTag;

            /// <summary>
            /// The offset of Software (305 / 0x0131) tag.<br/>
            /// （軟體（305／0x0131）標籤的位移量。）
            /// </summary>
            public int offsetSoftware;

            /// <summary>
            /// The offset of StripOffsets (273 / 0x0111) tag.<br/>
            /// （影像片段偏移（273／0x0111）標籤的位移量。）
            /// </summary>
            public int offsetStrips;
        }

        /// <summary>
        /// The delegate of the WriteGrid() methods implemented in each system.
        /// （在各個系統實作的 WriteGrid() 方法的委派。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public delegate void WriteGridMethod(BinaryWriter writer, ref Parameter param);

        /// <summary>
        /// Write the GeoTIFF file.
        /// （寫出 GeoTIFF 檔案。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="writeGridMethod">The WriteGrid() method implemented in each system.（各系統實作的 WriteGrid() 方法。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public static void Write(Options options, WriteGridMethod writeGridMethod, Action<string, int> onReportMethod)
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

            // Prepare the metadata.（準備元資料。）
            GeoTiffFormat format = options.GeoTiffFormat;
            float nodata = format == GeoTiffFormat.Float32 ? 1.70141E+38f : (format == GeoTiffFormat.Int16 ? -32768f : 0f);
            Parameter param = new() { format = format, nodata = nodata };

            // Write the grid data.（寫入網格資料。）
            writeGridMethod.Invoke(writer, ref param);

            // Write the metadata.（寫入元資料。）
            IOUtils.WriteLE(writer, (short)18);
            WriteTag(writer, 256, 1, param.imageWidth);
            WriteTag(writer, 257, 1, param.imageHeight);

            stopwatch.Stop();
            Instance.Log.Debug($"Write '{Path.GetFileName(filePath)}' in {CommonUtils.FormatTimeSpan(stopwatch.Elapsed)}.");
        }

        /// <summary>
        /// Write the grid data to the file.
        /// （寫入網格資料至檔案中。）
        /// </summary>
        /// <typeparam name="T">The type of the array elements.（陣列元素的型別。）</typeparam>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="grid">The data array.（資料陣列。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public static void WriteGridData<T>(BinaryWriter writer, ref IEnumerable<T> grid, ref Parameter param)
        {

        }

        /// <summary>
        /// Write the header of a TIFF file.
        /// （寫入 TIFF 檔案的標頭。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public static void WriteHeader(BinaryWriter writer, in Parameter param)
        {
            if (_littleEndian)
            {
                writer.Write(Encoding.UTF8.GetBytes("II"));
                writer.Write(BitConverter.GetBytes((short)42));
                writer.Write(BitConverter.GetBytes(param.offsetIFD));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes("II"));
                writer.Write(IOUtils.GetFlippedBytes((short)42));
                writer.Write(IOUtils.GetFlippedBytes(param.offsetIFD));
            }
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