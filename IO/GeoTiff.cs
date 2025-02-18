using Carto.Utils;
using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

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
            /// The bounds of the value.
            /// （數值的界限。）
            /// </summary>
            public Bounds1 bounds;

            /// <summary>
            /// The datetime of the file creation.
            /// （檔案創建時的日期與時間。）
            /// </summary>
            public string dateTime;

            /// <summary>
            /// The number of bits of each sample.
            /// （每個樣本的位元數。）
            /// </summary>
            public int depth;

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

            /// <summary>
            /// The format of a sample.
            /// （樣本的格式。）
            /// </summary>
            public int sampleFormat;

            /// <summary>
            /// The name of the software that produces the file.
            /// （產出檔案的軟體名稱。）
            /// </summary>
            public string software;

            /// <summary>
            /// The number of bytes of each strip.
            /// （每個片段的位元組數。）
            /// </summary>
            public readonly short BytesPerStrip() => (short)(imageWidth * depth / 8);
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
            int depth = format == GeoTiffFormat.Float32 ? 32 : 16;
            float nodata = format == GeoTiffFormat.Float32 ? 1.70141E+38f : (format == GeoTiffFormat.Int16 ? -32768f : 0f);
            int sample = format == GeoTiffFormat.Float32 ? 3 : (format == GeoTiffFormat.Int16 ? 2 : 1);
            Parameter param = new() { depth = depth, format = format, nodata = nodata, sampleFormat = sample };

            // Write the grid data.（寫入網格資料。）
            writeGridMethod.Invoke(writer, ref param);

            // Write the metadata.（寫入元資料。）
            Task writerThread = Task.Run(() => {
                // Write IFDs.（寫入影像檔案目錄。）
                IOUtils.WriteLE(writer, (short)18);
                WriteTag(writer, 256, 1, param.imageWidth);
                WriteTag(writer, 257, 1, param.imageHeight);
                WriteTag(writer, 258, 1, depth);
                WriteTag(writer, 259, 1, 1);
                WriteTag(writer, 262, 1, 1);
                WriteTag(writer, 273, param.imageHeight, param.offsetStrips);
                WriteTag(writer, 277, 1, 1);
                WriteTag(writer, 278, 1, 1);
                WriteTag(writer, 279, param.imageHeight, param.offsetBytesPerStrip);
                WriteTag(writer, 284, 1, 1);
                WriteTag(writer, 305, Encoding.UTF8.GetBytes(param.software).Length + 1, param.offsetSoftware);
                WriteTag(writer, 306, 20, param.offsetDateTime);
                WriteTag(writer, 339, 1, param.sampleFormat);
                WriteTag(writer, 33550, 3, param.offsetModelPixelScaleTag);
                WriteTag(writer, 33922, 6, param.offsetModelTiepointTag);
                WriteTag(writer, 34735, 0 + 48, param.offsetGeoKeyDirectoryTag); // Needs rewrite
                WriteTag(writer, 34737, 0 + 14, param.offsetGeoAsciiParamsTag); // Needs rewrite
                WriteTag(writer, 42113, 0 + 0, param.offsetGdalNodata); // Needs rewrite

                // Write additional data.（寫入額外資料。）
            });

            stopwatch.Stop();
            Instance.Log.Debug($"Write '{Path.GetFileName(filePath)}' in {CommonUtils.FormatTimeSpan(stopwatch.Elapsed)}.");
        }

        /// <summary>
        /// Write the grid data to the file.
        /// （寫入網格資料至檔案中。）
        /// </summary>
        /// <typeparam name="T1">The type of the array elements.（陣列元素的型別。）</typeparam>
        /// <typeparam name="T2">The type that actually writes into the file.（實際寫入檔案的型別。）</typeparam>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="grid">The data array.（資料陣列。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        /// <param name="conversion">The function to convert a <typeparamref name="T1"/> object to <typeparamref name="T2"/>.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteGridData<T1, T2>(BinaryWriter writer, ref IList<T1> grid, in Parameter param, Func<Parameter, T1, T2> conversion = null)
        {   
            // Validate the grid object.（檢驗網格物體。）
            if (grid == null)
            {
                throw new ArgumentNullException("The input grid must not be empty. 輸入的網格不可為空值。");
            }

            // Validate the length of the input grid.（檢驗輸入網格的長度。）
            int length = param.imageHeight * param.imageWidth;
            if (grid.Count != length)
            {
                throw new ArgumentException($"Length mismatch: expect {length}, but got {grid.Count}. 長度錯誤：預期為 {length}，實際為 {grid.Count}。");
            }

            // Fill grid data.（填入網格資料。）
            if (conversion == null)
            {
                for (int i = 0; i < grid.Count; i++)
                {
                    IOUtils.WriteLE(writer, grid[i]);
                }
            }
            else
            {
                for (int i = 0; i < grid.Count; i++)
                {
                    IOUtils.WriteLE(writer, conversion.Invoke(param, grid[i]));
                }
            }

            WriteGridDataCommon(writer, param.BytesPerStrip(), param.imageHeight);
        }

        /// <summary>
        /// Write the grid data to the file.
        /// （寫入網格資料至檔案中。）
        /// </summary>
        /// <typeparam name="T1">The type of the array elements.（陣列元素的型別。）</typeparam>
        /// <typeparam name="T2">The type that actually writes into the file.（實際寫入檔案的型別。）</typeparam>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="grid">The data array.（資料陣列。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        /// <param name="conversion">The function to convert a <typeparamref name="T1"/> object to <typeparamref name="T2"/>.</param>
        public static void WriteGridData<T1, T2>(BinaryWriter writer, ref NativeArray<T1> grid, in Parameter param, Func<Parameter, T1, T2> conversion = null) where T1 : struct
        {
            // Ensure native container safety.（確保原生容器的安全性。）
            if (!grid.IsCreated)
            {
                throw new ArgumentException("The input grid is not initiated. 輸入的網格尚未初始化。");
            }
            
            // Validate the length of the input grid.（檢驗輸入網格的長度。）
            int length = param.imageHeight * param.imageWidth;
            if (grid.Length != length)
            {
                throw new ArgumentException($"Length mismatch: expect {length}, but got {grid.Length}. 長度錯誤：預期為 {length}，實際為 {grid.Length}。");
            }

            // Fill grid data.（填入網格資料。）
            if (conversion == null)
            {
                for (int i = 0; i < grid.Length; i++)
                {
                    IOUtils.WriteLE(writer, grid[i]);
                }
            }
            else
            {
                for (int i = 0; i < grid.Length; i++)
                {
                    IOUtils.WriteLE(writer, conversion.Invoke(param, grid[i]));
                }
            }

            WriteGridDataCommon(writer, param.BytesPerStrip(), param.imageHeight);
        }

        /// <summary>
        /// Write the common grid-related data.
        /// （寫入與網格相關的共同資料。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bytesPerStrip">The number of bytes of each strip.（每個影像片段的位元組數。）</param>
        /// <param name="imageHeight">The height of the image in pixel.（影像以像素計的高度。）</param>
        private static void WriteGridDataCommon(BinaryWriter writer, int bytesPerStrip, int imageHeight)
        {
            byte[] bytesPerStripArray;
            if (_littleEndian)
            {
                bytesPerStripArray = BitConverter.GetBytes((short)bytesPerStrip);
                for (int i = 0; i < imageHeight; i++)
                {
                    writer.Write(BitConverter.GetBytes(12 + i * bytesPerStrip));
                }
                for (int i = 1; i <= imageHeight; i++)
                {
                    writer.Write(bytesPerStripArray);
                }
            }
            else
            {
                bytesPerStripArray = IOUtils.GetFlippedBytes((short)bytesPerStrip);
                for (int i = 0; i < imageHeight; i++)
                {
                    writer.Write(IOUtils.GetFlippedBytes(12 + i * bytesPerStrip));
                }
                for (int i = 1; i <= imageHeight; i++)
                {
                    writer.Write(bytesPerStripArray);
                }
            }
        }

        /// <summary>
        /// Write the header of a TIFF file.
        /// （寫入 TIFF 檔案的標頭。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        /// <exception cref="ArgumentException"></exception>
        public static void WriteHeader(BinaryWriter writer, ref Parameter param)
        {
            if ((param.depth == 0) || (param.imageHeight == 0) || (param.imageWidth == 0))
            {
                throw new ArgumentException("At least one of the parameter is unset: depth, imageHeight, or imageWidth. 至少一個參數未設定：depth，imageHeight，或是 imageWidth。");
            }

            param.dateTime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
            param.software = "CartoMod";

            // A row is a strip. There are H strips in an image with height of H pixels.（一排是一個片段。在高度為 H 像素的影像中，共有 H 個影像片段。）
            // Note: b = bytes per sample, f = first strip's offset = 12, H = image height.（註：b = 每個樣本位元組數，f = 第一個片段偏移量，H = 影像高度。）
            //
            // Offset（偏移量） Item（項目）
            // ----------------------------
            // f                Start of the first strip.（第一個片段開始。）
            // f + b * H        Start of StripOffsets tag's content.（StripOffsets 標籤內容開始。）
            // f + (b + 4) * H  Start of StripByteCounts tag's content.（StripByteCounts 標籤內容開始。）
            // f + (b + 6) * H  Start of the IFD.（影像檔案目錄開始。）
            int bps = param.BytesPerStrip();
            param.offsetBytesPerStrip = 12 + (bps + 4) * param.imageHeight;
            param.offsetIFD = 12 + (bps + 6) * param.imageHeight;
            param.offsetStrips = 12 + bps * param.imageHeight;

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