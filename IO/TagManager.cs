using Carto.Geodata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Carto.IO
{
    /// <summary>
    /// The helper class to write GeoTIFF tags.
    /// （用於協助寫入 GeoTIFF 標籤的類別。）
    /// </summary>
    public class TagManager
    {
        /// <summary>
        /// The lookup table between geokeys and their reference tags.
        /// （地理鍵與其參照標籤的對照表。）
        /// </summary>
        private static readonly Dictionary<int, int> _keyReferenceTable = new()
        {
            { 1, 1 },
            { 1024, 0 },
            { 1025, 0 },
            { 1026, 34737 },
            { 2048, 0 },
            { 2049, 34737 },
            { 2050, 0 },
            { 2051, 0 },
            { 2052, 0 },
            { 2054, 0 },
            { 2056, 0 },
            { 2057, 34736 },
            { 2059, 34736 },
            { 2062, 34736 },
            { 3072, 0 },
            { 3073, 34737 },
            { 3074, 0 },
            { 3075, 0 },
            { 3076, 0 },
            { 3080, 34736 },
            { 3081, 34736 },
            { 3082, 34736 },
            { 3083, 34736 },
            { 3092, 34736 }
        };

        /// <summary>
        /// The lookup table between tags and their field types.
        /// （標籤與其型別的對照表。）
        /// </summary>
        private static readonly Dictionary<int, short> _tagTypeTable = new()
        {
            { 256, GeoTiff.fieldTypeShort },
            { 257, GeoTiff.fieldTypeShort },
            { 258, GeoTiff.fieldTypeShort },
            { 259, GeoTiff.fieldTypeShort },
            { 262, GeoTiff.fieldTypeShort },
            { 273, GeoTiff.fieldTypeLong },
            { 277, GeoTiff.fieldTypeShort },
            { 278, GeoTiff.fieldTypeShort },
            { 279, GeoTiff.fieldTypeShort },
            { 284, GeoTiff.fieldTypeShort },
            { 305, GeoTiff.fieldTypeAscii },
            { 306, GeoTiff.fieldTypeAscii },
            { 339, GeoTiff.fieldTypeShort },
            { 33550, GeoTiff.fieldTypeDouble },
            { 33922, GeoTiff.fieldTypeDouble },
            { 34735, GeoTiff.fieldTypeShort },
            { 34736, GeoTiff.fieldTypeDouble },
            { 34737, GeoTiff.fieldTypeAscii },
            { 42113, GeoTiff.fieldTypeAscii }
        };

        /// <summary>
        /// The buffer for the tag 34737 (GeoAsciiParamsTag).<br/>
        /// （用於標籤編號 34737（ASCII 參數）的緩衝區。）
        /// </summary>
        private readonly BufferManager<string> _asciiBuffer;

        /// <summary>
        /// The current offset in bytes.
        /// （目前以位元組計的偏移量。）
        /// </summary>
        private int _currentBlockOffset;

        /// <summary>
        /// The buffer for the tag 34736 (GeoDoubleParamsTag).<br/>
        /// （用於標籤編號 34736（雙精度浮點數參數）的緩衝區。）
        /// </summary>
        private readonly BufferManager<double> _doubleBuffer;

        /// <summary>
        /// The lookup table between geokeys and their value counts.
        /// （地理鍵與其數值計數的對照表。）
        /// </summary>
        private readonly Dictionary<int, int> _geoKeyLengthTable;

        /// <summary>
        /// The lookup table between geokeys and their offset.
        /// （地理鍵與其偏移量的對照表。）
        /// </summary>
        private readonly Dictionary<int, int> _geoKeyOffsetTable;

        /// <summary>
        /// The lookup table between tags and their offset.
        /// （標籤與其偏移量的對照表。）
        /// </summary>
        private readonly Dictionary<int, int> _tagOffsetTable;

        /// <summary>
        /// The binary stream writer.（二進位資料流寫入者。）
        /// </summary>
        private readonly BinaryWriter _writer;

        /// <summary>
        /// The number of geokeys.
        /// （地理鍵的數量。）
        /// </summary>
        public int geoKeyCount;

        /// <summary>
        /// The GeoTIFF's export options.
        /// （GeoTIFF 的輸出設定。）
        /// </summary>
        public GeoTiff.Parameter param;

        /// <summary>
        /// The number of tags.
        /// （標籤的數量。）
        /// </summary>
        public int tagCount;

        /// <summary>
        /// The buffer for the tag 33550 (ModelPixelScaleTag) and 33922 (ModelTiepointTag).<br/>
        /// （用於標籤編號 33550（空間－像素縮放比例）和 33922（模型連接點）的緩衝區。）
        /// </summary>
        private readonly BufferManager<double> _transformBuffer;

        public TagManager(BinaryWriter writer, Options options, GeoTiff.Parameter param)
        {
            _asciiBuffer = new(suffix: new byte[1] { 0 });
            _currentBlockOffset = 29; // Base offset considering tag 305 & 306（考慮標籤 305 和 306 的基本位移量）
            _doubleBuffer = new();
            _geoKeyLengthTable = new();
            _geoKeyOffsetTable = new();
            _tagOffsetTable = new()
            {
                { 305, 0 }, { 306, 9 }
            };
            _transformBuffer = new();
            _writer = writer;
            geoKeyCount = 9; // Minimum GeoKeys: 1024, 1025, 1026, 2048, 2049, 2054, 3072, 3073 & 3076.
            this.param = param;
            tagCount = 18;
            Register(options);
        }

        /// <summary>
        /// Register tag and geokeys.
        /// （登記標籤與地理鍵。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        private void Register(Options options)
        {
            // Tag 33550 / 0x830E | ModelPixelScaleTag（空間－像素縮放比例）
            RegisterTag(33550);
            _transformBuffer.Add(new double[3] { param.scaleX, param.scaleY, 1 });
            _currentBlockOffset += 24; // 3 * 8 = 24

            // Tag 33922 / 0x8482 | ModelTiepointTag（模型連接點）
            RegisterTag(33922);
            Coord tiepoint = Transform.Apply(options.SourceCoordinates,
                                             options.SourceProjection,
                                             options.TargetProjection,
                                             options.SourceProjectionDefinition,
                                             options.TargetProjectionDefinition);
            _transformBuffer.Add(new double[6] { param.imageWidth / 2d,
                                                 param.imageHeight / 2d,
                                                 0d,
                                                 tiepoint.x,
                                                 tiepoint.y,
                                                 0d });
            _currentBlockOffset += 48; // 6 * 8 = 48

            // Tag 34736 / 0x87B0 | GeoDoubleParamsTag（雙精度浮點數參數）
            if (!param.isUTM)
            {
                RegisterTag(34736);

                // Require GeoKeys: 2050, 2051, 2056, 3074, 3075, 3080, 3081, 3082, 3083 & 3092.
                // Require tag: 34736.
                geoKeyCount += 10;
                tagCount++;
                ProjectionDefinition projection = options.TargetProjectionDefinition;

                if (param.hasCustomEllipsoid)
                {
                    // Require GeoKeys: 2052, 2057 & 2059.
                    geoKeyCount += 3;
                    EllipsoidDefinition ellipsoid = options.TargetProjectionDefinition.ellipsoid;

                    // GeoKey 2057 / 0x0809 | EllipsoidSemiMajorAxisGeoKey（橢球體半長軸）
                    RegisterGeoKey(2057, ellipsoid.a, 1, _doubleBuffer);

                    // GeoKey 2059 / 0x080B | EllipsoidInvFlatteningGeoKey（橢球體扁平率的倒數）
                    RegisterGeoKey(2059, ellipsoid.rf, 1, _doubleBuffer);
                }

                if (param.hasTransform)
                {
                    // Require GeoKey: 2062.
                    geoKeyCount++;
                    
                    // GeoKey 2062 / 0x080E | GeogToWGS84GeoKey（赫爾默特轉換參數）
                    // This is NOT a standard key. It is preserved here to provide datum transformations.
                    // （這不是標準的地理鍵。它被用於提供大地基準間的轉換。）
                    RegisterGeoKey(2062, projection.transform.ToArray(), _doubleBuffer);
                }

                // GeoKey 3080 / 0x0C08 | ProjNatOriginLongGeoKey（原點經度）
                RegisterGeoKey(3080, projection.origin.x, 1, _doubleBuffer);

                // GeoKey 3081 / 0x0C09 | ProjNatOriginLatGeoKey（原點緯度）
                RegisterGeoKey(3081, projection.origin.y, 1, _doubleBuffer);

                // GeoKey 3082 / 0x0C0A | ProjFalseEastingGeoKey（東距）
                RegisterGeoKey(3082, projection.shift.x, 1, _doubleBuffer);

                // GeoKey 3083 / 0x0C0B | ProjFalseNorthingGeoKey（北距）
                RegisterGeoKey(3083, projection.shift.y, 1, _doubleBuffer);

                // GeoKey 3092 / 0x0C13 | ProjScaleAtNatOriginGeoKey（原點尺度係數）
                RegisterGeoKey(3092, projection.scaleFactor, 1, _doubleBuffer);

                _currentBlockOffset += _doubleBuffer.GetFullLength();
            }

            // Tag 34737 / 0x87B1 | GeoAsciiParamsTag（ASCII 參數）
            RegisterTag(34737);

            // GeoKey 1026 / 0x0402 | GTCitationGeoKey（GeoTIFF 註解）
            string geoTiffCitation = "A Cities: Skylines II save|";
            RegisterGeoKey(1026, geoTiffCitation, _asciiBuffer);

            // GeoKey 2049 / 0x0801 | GeodeticCitationGeoKey (大地基準註解)
            string geodeticCitation;
            if (param.isUTM)
            {
                geodeticCitation = "WGS 84|";
            }
            else
            {
                string ellipsoidName = Epsg.Ellipsoid.GetName(options.TargetEllipsoid);
                geodeticCitation = $"User Defined Datum [Ellipsoid = {ellipsoidName} (EPSG:{param.ellipsoidCode}); Angular Unit = Degree (EPSG:{Epsg.Uom.Degree}); Linear Unit = Metre (EPSG:{Epsg.Uom.Metre})]|";
            }
            RegisterGeoKey(2049, geodeticCitation, _asciiBuffer);

            // GeoKey 3073 / 0x0C01 | ProjectedCitationGeoKey (投影註解)
            string projectedCitation;
            if (param.isUTM)
            {
                string hemisphere = tiepoint.Hemisphere == Hemisphere.North ? "N" : "S";
                int hemisphereCode = tiepoint.Hemisphere == Hemisphere.North ? 6 : 7;
                projectedCitation = $"WGS 84 / UTM zone {tiepoint.UTMZone}{hemisphere}|";
                param.projectionCode = Convert.ToInt32($"32{hemisphereCode}{tiepoint.UTMZone:00}");
            }
            else
            {
                projectedCitation = $"User Defined Transverse Mercator|";
            }
            RegisterGeoKey(3073, projectedCitation, _asciiBuffer, true);
            _currentBlockOffset += _asciiBuffer.GetFullLength();
        }

        /// <summary>
        /// Register a tag.
        /// （登記一個標籤。）
        /// </summary>
        /// <param name="id">The id for the tag.（標籤的代號。）</param>
        private void RegisterTag(int id)
        {
            _tagOffsetTable.Add(id, _currentBlockOffset);
        }

        /// <summary>
        /// Register a geokey.
        /// （登記一個地理鍵。）
        /// </summary>
        /// <typeparam name="T">The type of the geokey.（地理鍵的型別。）</typeparam>
        /// <param name="id">The id for the geokey.（地理鍵的代號。）</param>
        /// <param name="item">The content of the geokey.（地理鍵的內容。）</param>
        /// <param name="count">The number of values.（數值的數量。）</param>
        /// <param name="buffer">The buffer which the geokey belongs to.（地理鍵所屬的緩衝區。）</param>
        private void RegisterGeoKey<T>(int id, T item, int count, BufferManager<T> buffer)
        {
            buffer.Add(item);
            _geoKeyOffsetTable.Add(id, buffer.Count() - 1);
            _geoKeyLengthTable.Add(id, count);
        }

        /// <summary>
        /// Register a geokey.
        /// （登記一個地理鍵。）
        /// </summary>
        /// <typeparam name="T">The type of the geokey.（地理鍵的型別。）</typeparam>
        /// <param name="id">The id for the geokey.（地理鍵的代號。）</param>
        /// <param name="item">The content of the geokey.（地理鍵的內容。）</param>
        /// <param name="buffer">The buffer which the geokey belongs to.（地理鍵所屬的緩衝區。）</param>
        /// <param name="isLastInBuffer">Whether the item is the last to be appended in the buffer or not.（是否為緩衝區的最後一個物件？）</param>
        private void RegisterGeoKey<T>(int id, T item, BufferManager<T> buffer, bool isLastInBuffer = false)
        {
            buffer.Add(item, isLastInBuffer, out int offset, out int length);
            _geoKeyOffsetTable.Add(id, offset);
            _geoKeyLengthTable.Add(id, length);
        }

        /// <summary>
        /// Register a geokey.
        /// （登記一個地理鍵。）
        /// </summary>
        /// <typeparam name="T">The type of the geokey.（地理鍵的型別。）</typeparam>
        /// <param name="id">The id for the geokey.（地理鍵的代號。）</param>
        /// <param name="items">The content of the geokey.（地理鍵的內容。）</param>
        /// <param name="buffer">The buffer which the geokey belongs to.（地理鍵所屬的緩衝區。）</param>
        private void RegisterGeoKey<T>(int id, IList<T> items, BufferManager<T> buffer)
        {
            buffer.Add(items, out int offset, out int length);
            _geoKeyOffsetTable.Add(id, offset);
            _geoKeyLengthTable.Add(id, length);
        }

        /// <summary>
        /// Write the GeoTIFF tags.
        /// （寫入 GeoTIFF 標籤。）
        /// </summary>
        public void Write()
        {
            string nodata = $"{param.nodata: 0.00000e+000; -0.00000e+000}";
            int nonInlineOffset = param.offsetIFD + 2 + tagCount * 12;
            int geoKeyDirectoryOffset = _currentBlockOffset + nonInlineOffset;
            int gdalNodataOffset = geoKeyDirectoryOffset + (geoKeyCount + 1) * 8;

            // Write the IFDs.（寫入影像檔案目錄。）
            _writer.Write(BitConverter.GetBytes((ushort)tagCount));
            WriteTag(256, 1, param.imageWidth);                                                         // Tag   256 [0x0100] ImageWidth（影像寬度）
            WriteTag(257, 1, param.imageHeight);                                                        // Tag   257 [0x0101] ImageLength（影像高度）
            WriteTag(258, 1, param.depth);                                                              // Tag   258 [0x0102] BitsPerSample（每波段位元數）
            WriteTag(259, 1, 1);                                                                        // Tag   259 [0x0103] Compression（壓縮）
            WriteTag(262, 1, 1);                                                                        // Tag   262 [0x0106] PhotometricInterpretation（光度解讀）
            WriteTag(273, param.imageHeight, param.offsetStrips);                                       // Tag   273 [0x0111] StripOffsets（影像片段偏移）
            WriteTag(277, 1, 1);                                                                        // Tag   277 [0x0115] SamplesPerPixel（每像素波段數）
            WriteTag(278, 1, 1);                                                                        // Tag   278 [0x0116] RowsPerStrip（每片段垂直列數）
            WriteTag(279, param.imageHeight, param.offsetBytesPerStrip);                                // Tag   279 [0x0117] StripByteCounts（每片段位元組數）
            WriteTag(284, 1, 1);                                                                        // Tag   284 [0x011C] PlanarConfiguration（像素儲存方式）
            WriteTag(305, 9, _tagOffsetTable[305] + nonInlineOffset);                                   // Tag   305 [0x0131] Software（軟體）
            WriteTag(306, 20, _tagOffsetTable[306] + nonInlineOffset);                                  // Tag   306 [0x0132] DateTime（日期與時間）
            WriteTag(339, 1, param.sampleFormat);                                                       // Tag   339 [0x0153] SampleFormat（波段值類型）
            WriteTag(33550, 3, _tagOffsetTable[33550] + nonInlineOffset);                               // Tag 33550 [0x830E] ModelPixelScaleTag（空間－像素縮放比例）
            WriteTag(33922, 6, _tagOffsetTable[33922] + nonInlineOffset);                               // Tag 33922 [0x8482] ModelTiepointTag（空間對位）
            WriteTag(34735, (geoKeyCount + 1) * 4, geoKeyDirectoryOffset);                              // Tag 34735 [0x87AF] GeoKeyDirectoryTag（座標／投影系統）
            if (!param.isUTM)
            {
                WriteTag(34736, _doubleBuffer.Count(), _tagOffsetTable[34736] + nonInlineOffset);       // Tag 34736 [0x87B0] GeoDoubleParamsTag（雙精度浮點數參數）
            }
            WriteTag(34737, _asciiBuffer.GetFullLength(), _tagOffsetTable[34737] + nonInlineOffset);    // Tag 34737 [0x87B1] GeoAsciiParamsTag（ASCII 參數）
            WriteTag(42113, nodata.Length + 1, gdalNodataOffset);                                       // Tag 42113 [0xA481] GDAL_NODATA（GDAL 無資料值）

            // Write the additional data.（寫入額外的資料。）
            WriteAscii("CartoMod");                                                                     // Complete tag 305.（完成標籤 305。）
            WriteAscii(DateTime.UtcNow.ToString("yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture));  // Complete tag 306.（完成標籤 306。）
            _transformBuffer.Flush(_writer);                                                            // Complete tag 33550 & 33922.（完成標籤 33550 和 33922。）
            if (!param.isUTM)
            {
                _doubleBuffer.Flush(_writer);                                                           // Complete tag 34736.（完成標籤 34736。）
            }
            _asciiBuffer.Flush(_writer);                                                                // Complete tag 34737.（完成標籤 34737。）

            // Write the GeoKeyDirectory / complete tag 34735.（寫入地理鍵目錄／完成標籤 34735。）
            WriteGeoKey(1, 0, geoKeyCount);
            WriteGeoKey(1024, 1, 1);                                                                    // GeoKey 1024 [0x0400] GTModelTypeGeoKey（GeoTIFF 模型空間）
            WriteGeoKey(1025, 1, 1);                                                                    // GeoKey 1025 [0x0401] GTRasterTypeGeoKey（GeoTIFF 像素格式）
            WriteGeoKey(1026, _geoKeyLengthTable[1026], _geoKeyLengthTable[1026]);                      // GeoKey 1026 [0x0402] GTCitationGeoKey（GeoTIFF 註解）
            WriteGeoKey(2048, 1, param.isUTM ? Epsg.Crs.WGS84 : Epsg.UserDefined);                      // GeoKey 2048 [0x0800] GeodeticCRSGeoKey（大地坐標參考系統）
            WriteGeoKey(2049, _geoKeyLengthTable[2049], _geoKeyLengthTable[2049]);                      // GeoKey 2049 [0x0801] GeodeticCitationGeoKey（大地基準註解）
            if (!param.isUTM)
            {
                WriteGeoKey(2050, 1, Epsg.UserDefined);                                                 // GeoKey 2050 [0x0802] GeodeticDatumGeoKey（大地基準）
                WriteGeoKey(2051, 1, Epsg.Meridian.Greenwich);                                          // GeoKey 2051 [0x0803] PrimeMeridianGeoKey（本初經線）
                if (param.hasCustomEllipsoid)
                {
                    WriteGeoKey(2052, 1, Epsg.Uom.Metre);                                               // GeoKey 2052 [0x0804] GeogLinearUnitsGeoKey（大地長度單位）
                }
            }
            WriteGeoKey(2054, 1, Epsg.Uom.Degree);                                                      // GeoKey 2054 [0x0806] GeogAngularUnitsGeoKey（大地角度單位）
            if (!param.isUTM)
            {
                WriteGeoKey(2056, 1, param.ellipsoidCode);                                              // GeoKey 2056 [0x0808] EllipsoidGeoKey（橢球體）
                if (param.hasCustomEllipsoid)
                {
                    WriteGeoKey(2057, _geoKeyLengthTable[2057], _geoKeyOffsetTable[2057]);              // GeoKey 2057 [0x0809] EllipsoidSemiMajorAxisGeoKey（橢球體半長軸）
                    WriteGeoKey(2059, _geoKeyLengthTable[2059], _geoKeyOffsetTable[2059]);              // GeoKey 2059 [0x080B] EllipsoidInvFlatteningGeoKey（橢球體扁平率的倒數）
                }
                if (param.hasTransform)
                {
                    WriteGeoKey(2062, _geoKeyLengthTable[2062], _geoKeyOffsetTable[2062]);              // GeoKey 2062 [0x080E] GeogToWGS84GeoKey（赫爾默特轉換參數）
                }
            }
            WriteGeoKey(3072, 1, param.projectionCode);                                                 // GeoKey 3072 [0x0C00] ProjectedCRSGeoKey（投影坐標參考系統）
            WriteGeoKey(3073, _geoKeyLengthTable[3073], _geoKeyOffsetTable[3073]);                      // GeoKey 3073 [0x0C01] ProjectedCitationGeoKey（投影註解）
            if (!param.isUTM)
            {
                WriteGeoKey(3074, 1, Epsg.UserDefined);                                                 // GeoKey 3074 [0x0C02] ProjectionGeoKey（投影法）
                WriteGeoKey(3075, 1, 1);                                                                // GeoKey 3075 [0x0C03] ProjMethodGeoKey（投影方式）
            }
            WriteGeoKey(3076, 1, Epsg.Uom.Metre);                                                       // GeoKey 3076 [0x0C04] ProjLinearUnitsGeoKey（投影長度單位）
            if (!param.isUTM)
            {
                WriteGeoKey(3080, _geoKeyLengthTable[3080], _geoKeyOffsetTable[3080]);                  // GeoKey 3080 [0x0C08] ProjNatOriginLongGeoKey（原點經度）
                WriteGeoKey(3081, _geoKeyLengthTable[3081], _geoKeyOffsetTable[3081]);                  // GeoKey 3081 [0x0C09] ProjNatOriginLatGeoKey（原點緯度）
                WriteGeoKey(3082, _geoKeyLengthTable[3082], _geoKeyOffsetTable[3082]);                  // GeoKey 3082 [0x0C0A] ProjFalseEastingGeoKey（東距）
                WriteGeoKey(3083, _geoKeyLengthTable[3083], _geoKeyOffsetTable[3083]);                  // GeoKey 3083 [0x0C0B] ProjFalseNorthingGeoKey（北距）
                WriteGeoKey(3092, _geoKeyLengthTable[3092], _geoKeyOffsetTable[3092]);                  // GeoKey 3092 [0x0C13] ProjScaleAtNatOriginGeoKey（原點尺度係數）
            }

            WriteAscii(nodata);                                                                         // Complete tag 42113.（完成標籤 42113。）
        }

        /// <summary>
        /// Write an ASCII string in TIFF specification's style.
        /// （寫入以 TIFF 規格書寫的 ASCII 字串。）
        /// </summary>
        /// <param name="text">The input string.（輸入的字串。）</param>
        public void WriteAscii(string text)
        {
            _writer.Write(Encoding.UTF8.GetBytes(text));
            _writer.Write((byte) 0);
        }

        /// <summary>
        /// Write a OGC GeoKey to the file.
        /// （寫入一個 OGC 規範的 GeoKey 至檔案中。）
        /// </summary>
        /// <param name="id">The unique code of the tag.（標籤的獨特代碼。）</param>
        /// <param name="count">The length of the data in bytes.（以位元組計的資料長度。）</param>
        /// <param name="value">The value waiting to be written. In most of the time, it represents the value offset in bytes.<br/>
        /// （等待被寫入的數值，通常代表數值位移量，以位元組計。）</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void WriteGeoKey(int id, int count, int value)
        {
            if (!_keyReferenceTable.TryGetValue(id, out int reference))
            {
                throw new KeyNotFoundException($"The tag number `{id}` is not in _keyReferenceTable. 標籤代號 {id} 未紀錄於 _keyReferenceTable。");
            }

            _writer.Write(BitConverter.GetBytes((ushort)id));
            _writer.Write(BitConverter.GetBytes((ushort)reference));
            _writer.Write(BitConverter.GetBytes((short)count));
            _writer.Write(BitConverter.GetBytes((short)value));
        }

        /// <summary>
        /// Write a TIFF / EXIF tag to the file.
        /// （寫入一個 TIFF／EXIF 標籤至檔案中。）
        /// </summary>
        /// <param name="id">The unique code of the tag.（標籤的獨特代碼。）</param>
        /// <param name="count">The number of values.（數值的數量。）</param>
        /// <param name="value">The value waiting to be written. In most of the time, it represents the value offset in bytes.<br/>
        /// （等待被寫入的數值，通常代表數值位移量，以位元組計。）</param>
        public void WriteTag(int id, int count, int value)
        {
            if (!_tagTypeTable.TryGetValue(id, out short type))
            {
                throw new KeyNotFoundException($"The tag number `{id}` is not in _tagTypeTable. 標籤代號 {id} 未紀錄於 _tagTypeTable。");
            }

            _writer.Write(BitConverter.GetBytes((ushort)id));
            _writer.Write(BitConverter.GetBytes(type));
            _writer.Write(BitConverter.GetBytes(count));
            _writer.Write(BitConverter.GetBytes(value));
        }
    }
}