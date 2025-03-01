using Carto.Geodata;
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
        /*
            # References: （資料來源：）

            * Aldus Developers Desk. (1992). TIFF™ Revision 6.0
                https://www.itu.int/itudoc/itu-t/com16/tiff-fx/docs/tiff6.pdf

            * Open Geospatial Consortium. (2019). OGC GeoTIFF Standard
                https://docs.ogc.org/is/19-008r4/19-008r4.html
        */

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
        /// The look-up table for each GeoKey's reference tag.
        /// （每個地理鍵值參考標籤位置的對照表。）
        /// </summary>
        public static readonly Dictionary<int, ushort> GeoKeyReferenceTable = new()
        {
            { 1, 1 },
            { 1024, 0 },
            { 1025, 0 },
            { 1026, 34737 },
            { 2048, 0 },
            { 2049, 34737 },
            { 2054, 0 },
            { 3072, 0 },
            { 3076, 0 }
        };

        /// <summary>
        /// The look-up table for each tag's type.
        /// （每個標籤代表型別的對照表。）
        /// </summary>
        public static readonly Dictionary<int, short> TagTypeTable = new()
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
            { 34736, fieldTypeDouble },
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
            /// The number of bits of each sample.
            /// （每個樣本的位元數。）
            /// </summary>
            public int depth;

            /// <summary>
            /// The EPSG code for the target ellipsoid.
            /// （目標橢球體的 EPSG 代號。）
            /// </summary>
            public int ellipsoidCode;

            /// <summary>
            /// The format to export to.
            /// （輸出的格式。）
            /// </summary>
            public GeoTiffFormat format;

            /// <summary>
            /// Whether the target projection uses a custom ellipsoid or not.
            /// （目標投影法是否使用自訂橢球體？）
            /// </summary>
            public bool hasCustomEllipsoid;

            /// <summary>
            /// Whether the target projection has Helmert Transform parameters or not.
            /// （目標投影法是否具有赫爾默特轉換參數？）
            /// </summary>
            public bool hasTransform;

            /// <summary>
            /// Whether the target projection is UTM or not.
            /// （目標投影法是否為 UTM？）
            /// </summary>
            public bool isUTM;

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
            /// The offset of the image file directory (IFD).<br/>
            /// （影像檔案目錄（IFD）的位移量。）
            /// </summary>
            public int offsetIFD;

            /// <summary>
            /// The offset of StripOffsets (273 / 0x0111) tag.<br/>
            /// （影像片段偏移（273／0x0111）標籤的位移量。）
            /// </summary>
            public int offsetStrips;

            /// <summary>
            /// The EPSG code for the target projection.
            /// （目標投影法的 EPSG 代號。）
            /// </summary>
            public int projectionCode;
            
            /// <summary>
            /// The format of a sample.
            /// （樣本的格式。）
            /// </summary>
            public int sampleFormat;

            /// <summary>
            /// The scale factor between the pixel and the target projection's length unit along the x-axis.
            /// （X 軸方向上像素與目標投影法之間的尺度係數。）
            /// </summary>
            public double scaleX;

            /// <summary>
            /// The scale factor between the pixel and the target projection's length unit along the y-axis.
            /// （Y 軸方向上像素與目標投影法之間的尺度係數。）
            /// </summary>
            public double scaleY;

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
        /// Validate whether the grid length is correct or not.
        /// （檢驗網格的長度是否正確？）
        /// </summary>
        /// <typeparam name="T">The type of the array elements.（陣列元素的型別。）</typeparam>
        /// <param name="grid">The data array.（資料陣列。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        /// <exception cref="ArgumentException"></exception>
        public static void ValidateGrid<T>(ref NativeArray<T> grid, in Parameter param) where T : struct
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
        }

        /// <summary>
        /// Write the GeoTIFF file.
        /// （寫出 GeoTIFF 檔案。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="writeGridMethod">The WriteGrid() method implemented in each system.（各系統實作的 WriteGrid() 方法。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public static void Write(Options options, WriteGridMethod writeGridMethod, Action<string, int> onReportMethod)
        {
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
            Parameter param = new() {
                depth = depth,
                ellipsoidCode = Epsg.Ellipsoid.GetCode(options.TargetEllipsoid),
                format = format,
                hasCustomEllipsoid = options.TargetEllipsoid == Ellipsoid.Custom,
                hasTransform = options.TargetProjectionDefinition.HasTransform(),
                isUTM = options.TargetProjection == CRS.UTM,
                nodata = nodata,
                projectionCode = Epsg.UserDefined,
                sampleFormat = sample,
            };

            // Write the grid data.（寫入網格資料。）
            writeGridMethod.Invoke(writer, ref param);

            // Write the metadata.（寫入元資料。）
            Task writerThread = Task.Run(() => {
                TagManager manager = new(writer, options, param);
                manager.Write();
            });
            writerThread.Wait();

            stopwatch.Stop();
            Instance.Log.Debug($"Write '{Path.GetFileName(filePath)}' in {CommonUtils.FormatTimeSpan(stopwatch.Elapsed)}.");
        }

        /// <summary>
        /// Write the common grid-related data.
        /// （寫入與網格相關的共同資料。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bytesPerStrip">The number of bytes of each strip.（每個影像片段的位元組數。）</param>
        /// <param name="imageHeight">The height of the image in pixel.（影像以像素計的高度。）</param>
        public static void WriteGridDataCommon(BinaryWriter writer, short bytesPerStrip, int imageHeight)
        {
            byte[] bytesPerStripArray;
            bytesPerStripArray = BitConverter.GetBytes(bytesPerStrip);
            for (int i = 0; i < imageHeight; i++)
            {
                writer.Write(BitConverter.GetBytes(8 + i * bytesPerStrip));
            }
            for (int i = 1; i <= imageHeight; i++)
            {
                writer.Write(bytesPerStripArray);
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

            // A row is a strip. There are H strips in an image with height of H pixels.（一排是一個片段。在高度為 H 像素的影像中，共有 H 個影像片段。）
            // Note: b = bytes per sample, f = first strip's offset = 8, H = image height.（註：b = 每個樣本位元組數，f = 第一個片段偏移量，H = 影像高度。）
            //
            // Offset（偏移量） Item（項目）
            // ----------------------------
            // f                Start of the first strip.（第一個片段開始。）
            // f + b * H        Start of StripOffsets tag's content.（StripOffsets 標籤內容開始。）
            // f + (b + 4) * H  Start of StripByteCounts tag's content.（StripByteCounts 標籤內容開始。）
            // f + (b + 6) * H  Start of the IFD.（影像檔案目錄開始。）
            int bps = param.BytesPerStrip();
            param.offsetBytesPerStrip = 8 + (bps + 4) * param.imageHeight;
            param.offsetIFD = 8 + (bps + 6) * param.imageHeight;
            param.offsetStrips = 8 + bps * param.imageHeight;

            if (BitConverter.IsLittleEndian)
            {
                writer.Write(Encoding.UTF8.GetBytes("II"));
            }
            else
            {
                writer.Write(Encoding.UTF8.GetBytes("MM"));
            }

            writer.Write(BitConverter.GetBytes((short)42));
            writer.Write(BitConverter.GetBytes(param.offsetIFD));
        }
    }
}