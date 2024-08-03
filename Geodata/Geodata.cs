namespace Carto.Geodata
{
    using BU = Utils.BinaryUtils;
    using Carto.Domain;
    using Carto.Utils;
    using Colossal.Mathematics;
    using Colossal.Logging;
    using FU = Utils.FileUtils;
    using GeometryType = Utils.ExportUtils.GeometryType;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using SystemConvert = System.Convert;
    using Unity.Mathematics;

    /// <summary> 
    /// A class to convert Carto objects into geodata.
    /// （一個用以將 Carto 物件轉換為地理資料的類別。）
    /// </summary>
    public partial class Geodata
    {
        /// <summary>
        /// The constructor of Carto.Geodata.Geodata.
        /// （Carto.Geodata.Geodata 的建構函式。）
        /// </summary>
        public Geodata(CartoObject input)
        {
            m_Objects = new List<CartoObject> { input };
        }

        /// <summary>
        /// The constructor of Carto.Geodata.Geodata.
        /// （Carto.Geodata.Geodata 的建構函式。）
        /// </summary>
        public Geodata(params CartoObject[] input)
        {
            m_Objects = input.ToList();
        }

        /// <summary>
        /// The constructor of Carto.Geodata.Geodata.
        /// （Carto.Geodata.Geodata 的建構函式。）
        /// </summary>
        public Geodata(List<CartoObject> input)
        {
            m_Objects = input;
        }

        private readonly (double e, double n, int z, string h) _center = Convert.ToUTM(Setting.MapCenter);
        private readonly string _contentFolder = Setting.ContentFolder;
        private readonly ILog m_Log = Instance.Log;
        private readonly List<CartoObject> m_Objects;

        /// <summary>
        /// The objects from the inputted list.
        /// （輸入的列表中的物件。）
        /// </summary>
        public List<CartoObject> Objects => m_Objects;

        /// <summary>
        /// Write the GeoJSON file. (*.json)
        /// （寫出 GeoJSON 檔案（*.json）。）
        /// </summary>
        public void ToGeoJSON(string fileName, string field, bool suppressFieldError = false)
        {
            /*
                # References: （資料來源：）

                * Gillies, S., Butler, H. J., Daly, M., Doyle, A., & Schaub, T. (2016). The GeoJSON format
                    https://doi.org/10.17487/rfc7946
            */

            int objID = 0;
            string path = MiscUtils.CombinePath(_contentFolder, "GeoJSON", fileName);
            Directory.CreateDirectory(MiscUtils.CombinePath(_contentFolder, "GeoJSON"));

            for (int i = 0; i < Objects.Count; i++)
            {
                if (!suppressFieldError)
                {
                    if ((!Objects[i].Edges.TryGetValue(field, out _)) || (!Objects[i].Type.TryGetValue(field, out GeometryType geomType)))
                    {
                        throw new ArgumentException($"Couldn't find the field `{field}` in Edges or Type from the item at index {i}. 無法於索引{i}的物件的 Edges 或 Type 屬性中找到 `{field}` 欄位。");
                    }

                    if (geomType == GeometryType.Raster)
                    {
                        throw new ArgumentException("Only Points, LineStrings and Polygons can be exported as GeoJSON. 只有點、線段和多邊形可以被輸出為 GeoJSON。");
                    }
                }
            }

            List<CartoObject> objs = suppressFieldError ? Objects.Where(o => o.Edges.ContainsKey(field)).ToList() : Objects;
            FU json = new FU(path);
            json.Clear();
            json.AppendLine("\n{\n\t\"type\": \"FeatureCollection\",\n\t\"id\": \"Collection\",\n\t\"features\": [");

            foreach (CartoObject obj in objs)
            {
                GeometryType geomType = obj.Type[field];
                var geometryContainer = new List<string>();
                var propertiesContainer = new List<string>();

                json.AppendLine($"\t\t{{\n\t\t\t\"type\": \"Feature\",\n\t\t\t\"id\": {objID},\n\t\t\t\"geometry\": {{");
                json.Append($"\t\t\t\t\"type\": \"{geomType}\",\n\t\t\t\t\"coordinates\": ");
                json.Append((geomType == GeometryType.Point) ? "" : "[");
                json.AppendLine((geomType == GeometryType.Polygon) ? "\n\t\t\t\t\t[" : "");
                geometryContainer.AddRange(from float3 coord in obj.Edges[field]
                                           let wgs = Convert.ToWGS84((coord.x + _center.e, coord.z + _center.n, _center.z, _center.h))
                                           select $"\t\t\t\t\t\t[{wgs.lon}, {wgs.lat}, {coord.y}]");

                // Polygon's last coordinate is its first coordinate.
                // （多邊形的最後一個坐標是它的第一個坐標。）
                if (geomType == GeometryType.Polygon)
                {
                    float3 coord = obj.Edges[field][0];
                    var (lat, lon) = Convert.ToWGS84((coord.x + _center.e, coord.z + _center.n, _center.z, _center.h));
                    geometryContainer.Add($"\t\t\t\t\t\t[{lon}, {lat}, {coord.y}]");
                }

                json.AppendLine(string.Join(",\n", geometryContainer));
                json.Append((geomType == GeometryType.Polygon) ? "\t\t\t\t\t]\n" : "");
                json.Append((geomType == GeometryType.Point) ? "" : "\t\t\t\t]\n");
                json.AppendLine("\t\t\t},\n\t\t\t\"properties\": {");

                foreach (KeyValuePair<string, object> prop in obj.Properties)
                {
                    var k = prop.Key;
                    var v = prop.Value;

                    if ((v is short) || (v is ushort) || (v is int) || (v is uint) || (v is long) || (v is ulong) || (v is float) || (v is double) || (v is decimal))
                    {
                        propertiesContainer.Add($"\t\t\t\t\"{k}\": {v}");
                    }
                    else if ((v is bool) || (v is null))
                    {
                        if (v is true)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}\": true");
                        }
                        else if (v is false)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}\": false");
                        }
                        else if (v is null)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}\": null");
                        }
                    }
                    else if ((v is char) || (v is string))
                    {
                        propertiesContainer.Add($"\t\t\t\t\"{k}\": \"{v}\"");
                    }

                    // Handling special value types.
                    // （處理特殊資料類型。）
                    else if ((v is int2) || (v is uint2) || (v is float2) || (v is double2))
                    {
                        if (v is int2 @int)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@int.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@int.y}");
                        }
                        else if (v is uint2 @uint)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@uint.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@uint.y}");
                        }
                        else if (v is float2 @float)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@float.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@float.y}");
                        }
                        else if (v is double2 @double)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@double.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@double.y}");
                        }
                    }
                    else if ((v is int3) || (v is uint3) || (v is float3) || (v is double3))
                    {
                        if (v is int3 @int)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@int.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@int.y}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.z\": {@int.z}");
                        }
                        else if (v is uint3 @uint)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@uint.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@uint.y}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.z\": {@uint.z}");
                        }
                        else if (v is float3 @float)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@float.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@float.y}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.z\": {@float.z}");
                        }
                        else if (v is double3 @double)
                        {
                            propertiesContainer.Add($"\t\t\t\t\"{k}.x\": {@double.x}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.y\": {@double.y}");
                            propertiesContainer.Add($"\t\t\t\t\"{k}.z\": {@double.z}");
                        }
                    }
                    else if (v is Address @Address)
                    {
                        propertiesContainer.Add($"\t\t\t\t\"{k}.district\": \"{@Address.District}\"");
                        propertiesContainer.Add($"\t\t\t\t\"{k}.street\": \"{@Address.Street}\"");
                        propertiesContainer.Add($"\t\t\t\t\"{k}.number\": {@Address.Number}");
                    }
                }

                json.AppendLine(string.Join(",\n", propertiesContainer));
                json.AppendLine("\t\t\t}");
                json.Append("\t\t}");
                json.AppendLine((objID == objs.Count - 1) ? "" : ",");
                objID++;
            }

            json.Append("\t]\n}");
            m_Log.Info($"`{fileName}` has been exported. `{fileName}`已經輸出完成。");
        }

        /// <summary>
        /// Write the OGC GeoPackage file. (*.gpkg)
        /// （寫出 OGC GeoPackage 檔案（*.gpkg）。）
        /// </summary>
        public void ToGeoPackage(string fileName)
        {
            /*
                # Source: （資料來源：）

                * Open Geospatial Consortium. (2023). OGC® GeoPackage Encoding Standard
                    https://www.geopackage.org/spec/
            */
            // TODO: Implement ToGeoPackage method. （待辦：實作 ToGeoPackage 方法。）
            throw new NotImplementedException();
        }

        /// <summary>
        /// TIFF's field types.
        /// （TIFF 的欄位型別。）
        /// </summary>
        internal enum TField : short
        {
            BY = 1,  // Byte
            AS = 2,  // ASCII
            SH = 3,  // Short
            LO = 4,  // Long
            RA = 5,  // Rational
            SB = 6,  // SByte
            UN = 7,  // Undefined
            SS = 8,  // SShort
            SL = 9,  // SLong
            SR = 10, // SRational
            FL = 11, // Float
            DO = 12  // Double
        }

        /// <summary>
        /// Write the GeoTIFF file. (*.tif)
        /// （寫出 GeoTIFF 檔案（*.tif）。）
        /// </summary>
        /// <param name="fileName">The output file name with its extension. （輸出檔案名稱，須包含副檔名。）</param>
        /// <param name="field">The field that is going to be exported. （即將被輸出的欄位名稱。）</param>>
        /// <param name="depth">The bitdepth of the file, default to be 32 (float); another option is 16 (short). （檔案的位元深度，預設為32（浮點數），可為16（短整數）。）</param>
        public void ToGeoTIFF(string fileName, string field, int depth = 32)
        {
            /*
                # Source: （資料來源：）

                * Aldus Developers Desk. (1992). TIFF™ Revision 6.0
                    https://www.itu.int/itudoc/itu-t/com16/tiff-fx/docs/tiff6.pdf

                * Open Geospatial Consortium. (2019). OGC GeoTIFF Standard
                    https://docs.ogc.org/is/19-008r4/19-008r4.html
            */

            var obj = Objects[0];
            var tiffBList = new List<byte>();
            double transRX = 2048;
            double transRY = 2048;
            double xPixelScale = 3.5;
            double yPixelScale = 3.5;
            double zPixelScale = 1;
            float nodata = 1.70141E+38f;
            float outputNodata;
            int firstStripIndex = 25066;
            int gaspIndex = 430;
            int geokIndex = 350;
            int gdalIndex = 470;
            int mdpsIndex = 270;
            int mdtpIndex = 300;
            int noDataLen;
            int pcsEPSG;
            int pcsNLen;
            int softIndex = 240;
            int spbyIndex = 16874;
            int spofIndex = 490;
            int timeIndex = 250;
            int SPI = 4096;
            int3 resolution = new int3(4096, 65536, 4096);
            short bytesPerStrip = (short)(4096 * depth / 8);
            string datetime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
            string gcsName = "WGS 84";
            string gdalNodata;
            string pcsName;
            string tiffPath = MiscUtils.CombinePath(_contentFolder, "GeoTIFF", fileName);

            if ((!obj.Values.TryGetValue(field, out _)) || (!obj.Type.TryGetValue(field, out GeometryType geomType)))
            {
                throw new ArgumentException($"Couldn't find the field `{field}` in Values or Type from the item at index 0. 無法於索引0的物件的 Values 或 Type 屬性中找到 `{field}` 欄位。");
            }

            if (geomType != GeometryType.Raster)
            {
                throw new ArgumentException("Only Rasters can be exported as GeoTIFF. 只有網格可以被輸出為 GeoTIFF。");
            }

            if ((depth != 16) && (depth != 32))
            {
                throw new ArgumentException($"Argument `depth` expected 16 or 32, but received {depth}. 參數 `depth` 僅可為16或32，但被設為{depth}。");
            }

            outputNodata = (depth == 16) ? -32768 : nodata;
            gdalNodata = $"{outputNodata: 0.00000e+000; -0.00000e+000}";
            noDataLen = gdalNodata.Length + 1;

            foreach (KeyValuePair<string, object> prop in Objects[0].Properties)
            {
                if ((prop.Key == "Matrix") && (prop.Value is Dictionary<string, object> geometry))
                {
                    foreach (KeyValuePair<string, object> geom in geometry)
                    {
                        if ((geom.Key == "resolution") && (geom.Value is int3 res))
                        {
                            resolution = res;
                            bytesPerStrip = (short)(res.x * depth / 8);
                            spbyIndex = spofIndex + res.z * 4;
                            transRX = res.x / 2;
                            transRY = res.z / 2;
                            firstStripIndex = spbyIndex + res.x * 2;
                            SPI = res.z;
                        }

                        if ((geom.Key == "scale") && (geom.Value is float3 scale))
                        {
                            xPixelScale = (double)Math.Abs(Math.Round(1 / scale.x, 3));
                            yPixelScale = (double)Math.Abs(Math.Round(1 / scale.z, 3));
                        }
                    }
                }
            }

            pcsEPSG = SystemConvert.ToInt32($"32{(_center.h == "N" ? 6 : 7)}{_center.z:00}");
            pcsName = $"WGS 84 / UTM zone {_center.z}{_center.h}";
            pcsNLen = pcsName.Length + 1;

            // Image File Header （影像檔案標頭）

            BU.AddBytes(tiffBList, "II");               // 0 Mark the file endianness to be little endian, so the issue of endianness can be lifted.
                                                        //   （位元組0：標示檔案使用小端序，如此一來後續便不用擔心交錯使用大、小端序。）
            BU.AddBytes(tiffBList, (short)42);          // 2 42. （位元組2：42。）
            BU.AddBytes(tiffBList, 8);                  // 4 IFD (Image file directory) offset, 8. （位元組4：IFD（影像檔案目錄）偏移，8。）

            // Image File Directory （影像檔案目錄）

            BU.AddBytes(tiffBList, (ushort)18);                                 //   8 Number of tags. （位元組8：標籤數量。）
            BU.AddTag(tiffBList, 256, TField.SH, 1, resolution.x);            //  10 Tag   256 (0x0100) ImageWidth （影像寬度）                      
            BU.AddTag(tiffBList, 257, TField.SH, 1, resolution.z);            //  22 Tag   257 (0x0101) ImageLength （影像高度）
            BU.AddTag(tiffBList, 258, TField.SH, 1, depth);                   //  34 Tag   258 (0x0102) BitsPerSample （每波段位元數）
            BU.AddTag(tiffBList, 259, TField.SH, 1, 1);                       //  46 Tag   259 (0x0103) Compression （壓縮）
            BU.AddTag(tiffBList, 262, TField.SH, 1, 1);                       //  58 Tag   262 (0x0106) PhotometricInterpretation （光度解讀）
            BU.AddTag(tiffBList, 273, TField.LO, SPI, spofIndex);             //  70 Tag   273 (0x0111) StripOffsets （影像片段偏移） 
            BU.AddTag(tiffBList, 277, TField.SH, 1, 1);                       //  82 Tag   277 (0x0115) SamplesPerPixel （每像素波段數）
            BU.AddTag(tiffBList, 278, TField.SH, 1, 1);                       //  94 Tag   278 (0x0116) RowsPerStrip （每片段垂直列數）
            BU.AddTag(tiffBList, 279, TField.SH, SPI, spbyIndex);             // 106 Tag   279 (0x0117) StripByteCounts （每片段位元組數）
            BU.AddTag(tiffBList, 284, TField.SH, 1, 1);                       // 118 Tag   284 (0x011C) PlanarConfiguration （像素儲存方式）
            BU.AddTag(tiffBList, 305, TField.AS, 9, softIndex);               // 130 Tag   305 (0x0131) Software （軟體）
            BU.AddTag(tiffBList, 306, TField.AS, 20, timeIndex);              // 142 Tag   306 (0x0132) DateTime （日期與時間）
            BU.AddTag(tiffBList, 339, TField.SH, 1, (depth == 16) ? 2 : 3);   // 154 Tag   339 (0x0153) SampleFormat （波段值類型）
            BU.AddTag(tiffBList, 33550, TField.DO, 3, mdpsIndex);               // 166 Tag 33550 (0x830E) ModelPixelScaleTag （空間－像素縮放比例）
            BU.AddTag(tiffBList, 33922, TField.DO, 6, mdtpIndex);               // 178 Tag 33922 (0x8482) ModelTiepointTag （空間對位）
            BU.AddTag(tiffBList, 34735, TField.SH, 48, geokIndex);              // 190 Tag 34735 (0x87AF) GeoKeyDirectoryTag （座標／投影系統）
            BU.AddTag(tiffBList, 34737, TField.AS, pcsNLen + 8, gaspIndex);     // 202 Tag 34737 (0x87B1) GeoAsciiParamsTag （ASCII參數）
            BU.AddTag(tiffBList, 42113, TField.AS, noDataLen, gdalIndex);       // 214 Tag 42113 (0xA481) GDAL_NODATA （GDAL無資料值）
            BU.Skip(tiffBList, 14, "byte");                                     // 226 Next IFD offset. （下一個IFD的偏移）

            // Fixed Length TIFF & GeoTIFF IFDs （固定長度的 TIFF 和 GeoTIFF 影像檔案目錄）

            BU.AddBytes(tiffBList, "CartoMod");                                 // 240 Complete Tag 305 (Software).（完成標籤305，軟體。）
            BU.Skip(tiffBList, 2, "byte");                                      // 248
            BU.AddBytes(tiffBList, datetime);                                   // 250 Complete Tag 306 (DateTime).（完成標籤306，日期與時間。）
            BU.Skip(tiffBList, 1, "byte");                                      // 269
            BU.AddBytes(tiffBList, xPixelScale, yPixelScale, zPixelScale);      // 270 Complete Tag 33550 (ModelPixelScaleTag).（完成標籤33550，空間－像素縮放比例。）
            BU.Skip(tiffBList, 6, "byte");                                      // 294
            BU.AddBytes(tiffBList, transRX, transRY, (double)0);                // 300 Complete Tag 33922 (ModelTiepointTag).（完成標籤33922，空間對位。）
            BU.AddBytes(tiffBList, _center.e, _center.n, (double)0);            // 324
            BU.Skip(tiffBList, 2, "byte");                                      // 348
            BU.AddGeoKey(tiffBList, 1, 1, 0, 8);                                // 350 Complete Tag 34735 (GeoKeyDirectoryTag) header.（完成標籤34735（座標／投影系統）的標頭。）
            BU.AddGeoKey(tiffBList, 1024, 0, 1, 1);                             // 358 : GeoKey  1024 (0x0400) GTModelType （座標／投影系統類別）
            BU.AddGeoKey(tiffBList, 1025, 0, 1, 1);                             // 366 : GeoKey  1025 (0x0401) GTRasterType （網格資料解讀方式）
            BU.AddGeoKey(tiffBList, 1026, 34737, pcsNLen, 14);                  // 374 : GeoKey  1026 (0x0402) GTCitation （註記）
            BU.AddGeoKey(tiffBList, 2048, 0, 1, 4326);                          // 382 : GeoKey  2048 (0x0800) GeodeticCRS （地理坐標系）
            BU.AddGeoKey(tiffBList, 2049, 34737, 7, 0);                         // 390 : GeoKey  2049 (0x0801) GeodeticCitation （地理坐標系註記）
            BU.AddGeoKey(tiffBList, 2054, 0, 1, 9102);                          // 398 : GeoKey  2054 (0x0806) GeogAngularUnits （地理坐標系角度單位）
            BU.AddGeoKey(tiffBList, 3072, 0, 1, pcsEPSG);                       // 406 : GeoKey  3072 (0x0C00) ProjectedCRS （投影坐標系）
            BU.AddGeoKey(tiffBList, 3076, 0, 1, 9001);                          // 414 : GeoKey  3076 (0x0C04) ProjLinearUnits （投影坐標系長度單位）
            BU.Skip(tiffBList, 8, "byte");                                      // 422
            BU.AddBytes(tiffBList, gcsName, "|", gcsName, "|", pcsName, "|");   // 430 Complete Tag 34737 (GeoAsciiParamsTag).（完成標籤34737，ASCII參數。）
            BU.Skip(tiffBList, 26 - pcsNLen, "byte");                           // 465/466
            BU.AddBytes(tiffBList, gdalNodata);                                 // 470 Complete Tag 42113 (GDAL_NODATA). （完成標籤42113，GDAL無資料值）
            BU.Skip(tiffBList, 21 - noDataLen, "byte");                         // 482/483

            // Non-fixed Length TIFF IFDs（非固定長度的 TIFF 影像檔案目錄）

            byte[] bpsBytes = BitConverter.GetBytes(bytesPerStrip);
            List<byte> bpsList = Enumerable.Range(1, SPI).SelectMany(i => bpsBytes).ToList();
            List<byte> sofList = Enumerable.Range(0, SPI).SelectMany(i => BitConverter.GetBytes(firstStripIndex + i * bytesPerStrip)).ToList();

            tiffBList.AddRange(sofList);                                        // 490 Complete Tag 273 (StripOffsets). （完成標籤273，影像片段偏移）
            tiffBList.AddRange(bpsList);                                        // 490 + X Complete Tag 279 (StripByteCounts). （完成標籤279，每片段位元組數）

            // Fill the data. （填滿資料。）

            if (depth == 16)
            {
                for (int i = 0; i < resolution.x; i++)
                {
                    for (int j = 0; j < resolution.z; j++)
                    {
                        float val = obj.Values[field][i, j];
                        val = (val == nodata) ? outputNodata : val;
                        tiffBList.AddRange(BitConverter.GetBytes(SystemConvert.ToInt16(val)));
                    }
                }
            }
            else
            {
                for (int i = 0; i < resolution.x; i++)
                {
                    for (int j = 0; j < resolution.z; j++)
                    {
                        tiffBList.AddRange(BitConverter.GetBytes(obj.Values[field][i, j]));
                    }
                }
            }

            // Write the GeoTIFF (and overwrite the original one.)
            // （寫出 GeoTIFF 檔案，並且覆寫原本的檔案。）
            Directory.CreateDirectory(MiscUtils.CombinePath(_contentFolder, "GeoTIFF"));
            File.WriteAllText(tiffPath, string.Empty);
            File.WriteAllBytes(tiffPath, tiffBList.ToArray());

            m_Log.Info($"`{fileName}` has been exported. `{fileName}`已經輸出完成。");
        }

        /// <summary>
        /// Write the ESRI Shapefile. (*.cpg, *.dbf, *.prj, *.shp &amp; *.shx)
        /// （寫出 ESRI Shapefile 檔案（*.cpg, *.dbf, *.prj, *.shp &amp; *.shx）。）
        /// </summary>
        /// <param name="fileName">The output file name with its extension. （輸出檔案名稱，須包含副檔名。）</param>
        /// <param name="field">The field that is going to be exported. （即將被輸出的欄位名稱。）</param>
        public void ToShapefile(string fileName, string field, bool suppressFieldError = false)
        {
            /*
                # Source: （資料來源：）

                * Environmental Systems Research Institute, Inc. (1998). ESRI Shapefile Technical Description
                    https://www.esri.com/content/dam/esrisites/sitecore-archive/Files/Pdfs/library/whitepapers/pdfs/shapefile.pdf

                * Bachmann, E. (2010). Xbase Data file (*.dbf)
                    https://www.clicketyclick.dk/databases/xbase/format/dbf.html

                * MapTiler. (2024). WGS / UTM zone xxX - EPSG:32xxx
                    https://epsg.io/
            */

            var bList = new List<byte>();
            var pTypNames = ExportUtils.ExportFieldTypes.Keys.ToList();
            var propNames = Objects.SelectMany(carto => carto.Properties.Select(prop => prop.Key)).Distinct().ToList();
            var propTypes = propNames.Select(propNameIndividual => ExportUtils.ExportFieldTypes.ContainsKey(propNameIndividual) ? ExportUtils.ExportFieldTypes[propNameIndividual] : null).ToList();
            string filePrefix = fileName.Split('.')[0];
            string cpgPath = MiscUtils.CombinePath(_contentFolder, "Shapefile", $"{filePrefix}.cpg");
            string dbfPath = MiscUtils.CombinePath(_contentFolder, "Shapefile", $"{filePrefix}.dbf");
            string prjPath = MiscUtils.CombinePath(_contentFolder, "Shapefile", $"{filePrefix}.prj");
            string shpPath = MiscUtils.CombinePath(_contentFolder, "Shapefile", $"{filePrefix}.shp");
            string shxPath = MiscUtils.CombinePath(_contentFolder, "Shapefile", $"{filePrefix}.shx");
            GeometryType shpType = Objects[0].Type.TryGetValue(field, out GeometryType _t) ? _t : GeometryType.None;

            /// <summary>
            /// Get corresponding dBASE field symbols by data type.
            /// （根據資料型別獲取對應的 dBASE 欄位符號。）
            /// </summary>
            string GetSymbol(Type ttype)
            {
                switch (ttype.Name)
                {
                    case "Int16":
                    case "UInt16":
                    case "Int32":
                    case "UInt32":
                    case "Int64":
                    case "UInt64":
                    case "int2":
                    case "uint2":
                    case "int3":
                    case "uint3":
                    case "Boolean":
                        return "N";

                    case "Single":
                    case "Double":
                    case "float2":
                    case "double2":
                    case "float3":
                    case "double3":
                        return "F";

                    default:
                        return "C";
                }
            }

            var fieldLength = new Dictionary<string, Dictionary<string, int>>
            {
                {"Int16",   new Dictionary<string, int>{ {"max",   6}, {"dec", 0}, {"dim", 1} }},
                {"UInt16",  new Dictionary<string, int>{ {"max",   6}, {"dec", 0}, {"dim", 1} }},
                {"Int32",   new Dictionary<string, int>{ {"max",  11}, {"dec", 0}, {"dim", 1} }},
                {"UInt32",  new Dictionary<string, int>{ {"max",  11}, {"dec", 0}, {"dim", 1} }},
                {"Int64",   new Dictionary<string, int>{ {"max",  20}, {"dec", 0}, {"dim", 1} }},
                {"UInt64",  new Dictionary<string, int>{ {"max",  20}, {"dec", 0}, {"dim", 1} }},
                {"Single",  new Dictionary<string, int>{ {"max",  15}, {"dec", 7}, {"dim", 1} }},
                {"Double",  new Dictionary<string, int>{ {"max",  20}, {"dec", 7}, {"dim", 1} }},
                {"Boolean", new Dictionary<string, int>{ {"max",   1}, {"dec", 0}, {"dim", 1} }},
                {"Null",    new Dictionary<string, int>{ {"max", 254}, {"dec", 0}, {"dim", 1} }},
                {"Char",    new Dictionary<string, int>{ {"max", 254}, {"dec", 0}, {"dim", 1} }},
                {"String",  new Dictionary<string, int>{ {"max", 254}, {"dec", 0}, {"dim", 1} }},
                {"int2",    new Dictionary<string, int>{ {"max",  11}, {"dec", 0}, {"dim", 2} }},
                {"uint2",   new Dictionary<string, int>{ {"max",   6}, {"dec", 0}, {"dim", 2} }},
                {"int3",    new Dictionary<string, int>{ {"max",  11}, {"dec", 0}, {"dim", 3} }},
                {"uint3",   new Dictionary<string, int>{ {"max",   6}, {"dec", 0}, {"dim", 3} }},
                {"float2",  new Dictionary<string, int>{ {"max",  15}, {"dec", 7}, {"dim", 2} }},
                {"double2", new Dictionary<string, int>{ {"max",  20}, {"dec", 7}, {"dim", 2} }},
                {"float3",  new Dictionary<string, int>{ {"max",  15}, {"dec", 7}, {"dim", 3} }},
                {"double3", new Dictionary<string, int>{ {"max",  20}, {"dec", 7}, {"dim", 3} }},
                {"Address", new Dictionary<string, int>{ {"max", 254}, {"dec", 0}, {"dim", 3} }}
            };

            for (int i = 0; i < Objects.Count; i++)
            {
                if (!suppressFieldError)
                {
                    if ((!Objects[i].Edges.TryGetValue(field, out _)) || (!Objects[i].Type.TryGetValue(field, out GeometryType geomType)))
                    {
                        throw new ArgumentException($"Couldn't find the field `{field}` in Edges or Type from the item at index {i}. 無法於索引{i}的物件的 Edges 或 Type 屬性中找到 `{field}` 欄位。");
                    }

                    if (geomType == GeometryType.Raster)
                    {
                        throw new ArgumentException("Only Points, LineStrings and Polygons can be exported as ESRI Shapefile. 只有點、線段和多邊形可以被輸出為 ESRI Shapefile。");
                    }

                    if (geomType != shpType)
                    {
                        throw new ArgumentException($"Geometry types should be consistent; expect `{shpType}` but get {Objects[i].Type[field]}. 幾何類型應保持一致；預期為 `{shpType}`，但出現了 `{Objects[i].Type[field]}`。");
                    }
                }
            }

            if (propNames.Except(pTypNames).Any())
            {
                throw new ArgumentException("The field records are not consistent across CartoObjects' `Properties` and `PropertyType` attributes. CartoObject 的 `Properties` 和 `PropertyType` 屬性欄位記錄不連續。");
            }

            FU dbf = new FU(dbfPath);
            FU shp = new FU(shpPath);
            FU shx = new FU(shxPath);
            dbf.Clear();
            shp.Clear();
            shx.Clear();
            // === MAIN FILE HEADER OF .SHP AND .SHX （.shp 和 .shx 的檔案主標頭。）===
            BU.AddBytes(bList, BU.FlipEndian(9994));                //   0 File Code, 9994 (BIG ENDIAN). （位元組0：檔案代碼，9994（大端序）。）
            BU.Skip(bList, 5);                                      //   4
            BU.Skip(bList, 1);                                      //  24 File length in 16-bit words (BIG ENDIAN). （位元組24：以16位元計的檔案長度（大端序）。） 
            BU.AddBytes(bList, 1000);                               //  28 Version, 1000. （位元組28：版本，1000。）

            if (shpType == GeometryType.Point)                      //  32 Shape type （位元組32：幾何類型。）
            {
                BU.AddBytes(bList, 11);
            }
            else if (shpType == GeometryType.LineString)
            {
                BU.AddBytes(bList, 13);
            }
            else if (shpType == GeometryType.Polygon)
            {
                BU.AddBytes(bList, 15);
            }

            BU.Skip(bList, 12);                                     //  36 Bounding box （位元組36：邊界框範圍。）
            BU.Skip(bList, 4);                                      //  84
            shp.Append(bList.ToArray());
            shx.Append(bList.ToArray());
            bList.Clear();
            // === HEADER OF .DBF （.dbf 的標頭。）===
            BU.AddBytes(bList, (byte)3);                            //   0 Signature, 3 (File wihout DBT). （位元組0：簽章，3（不含 DBT 的檔案）。）
            BU.AddBytes(bList, (byte)(DateTime.Now.Year - 1900));   //   1 Year counts since 1900. （位元組1：年份，自1900年以來的年計數。）
            BU.AddBytes(bList, (byte)DateTime.Now.Month);           //   2 Month. （位元組2：月份。）
            BU.AddBytes(bList, (byte)DateTime.Now.Day);             //   3 Day. （位元組3：日期。）
            BU.AddBytes(bList, Objects.Count);                      //   4 Number of records. （位元組4：記錄的數量。）
            BU.Skip(bList, 2, "byte");                              //   8 Number of bytes in the header. （位元組8：以位元組計的標頭長度。）
            BU.Skip(bList, 2, "byte");                              //  10 Number of bytes in the record. （位元組10：以位元組計的記錄長度。）
            BU.Skip(bList, 5);                                      //  12
            dbf.Append(bList.ToArray());
            bList.Clear();

            for (int i = 0; i < propNames.Count; i++)
            {
                string fieldName = propNames[i];
                Type fieldType = propTypes[i];
                Dictionary<string, int> fl = fieldLength[fieldType.Name];

                void fillFieldDescripter(string fName, string catagory, byte maxLength, byte maxDecimal)
                {                                                                                   // === FIELD ARRAY OF .DBF （.dbf 的欄位陣列。）===
                    BU.AddBytes(bList, fName);                                                      //   0 Field name. （位元組0：欄位名稱。）
                    BU.Skip(bList, 11 - Encoding.UTF8.GetBytes(fName).Length, "byte");              //   ?
                    BU.AddBytes(bList, catagory);                                                   //  11 Field type. （位元組11：欄位類型。）
                    BU.Skip(bList, 1);                                                              //  12
                    BU.AddBytes(bList, maxLength);                                                  //  16 Field length. （位元組16：欄位長度。）

                    if (maxDecimal > 0)
                    {
                        BU.AddBytes(bList, maxDecimal);                                             //  17 Decimal length. （位元組17：小數點後長度。）
                        BU.Skip(bList, 14, "byte");                                                 //  18
                    }
                    else
                    {
                        BU.Skip(bList, 15, "byte");                                                 //  17
                    }
                }

                void buildFieldDescripter(string catagory, byte maxLength, byte maxDecimal, int nd)
                {
                    int colLen = (nd > 1 ? 8 : 10);    // The column name is 10 bytes at most. （欄位名稱長度至多為 10 位元組。）
                    while (Encoding.UTF8.GetBytes(fieldName).Length > colLen) { fieldName = fieldName.Substring(0, fieldName.Length - 1); }

                    if (nd > 1)
                    {
                        List<string> newFieldNames = new List<string>();

                        if (fieldType == typeof(Address))
                        {
                            string newFieldTitle = (fieldName == "Address") ? "Addr" : fieldName.Substring(0, 4);
                            fillFieldDescripter($"{newFieldTitle}.dist", catagory, maxLength, maxDecimal);
                            fillFieldDescripter($"{newFieldTitle}.strt", catagory, maxLength, maxDecimal);
                            fillFieldDescripter($"{newFieldTitle}.nmbr", "N", 11, 0);
                        }
                        else
                        {
                            newFieldNames = new List<string> { $"{fieldName}.x", $"{fieldName}.y" };
                            if (nd > 2) newFieldNames.Add($"{fieldName}.z");
                            foreach (string fName in newFieldNames) fillFieldDescripter(fName, catagory, maxLength, maxDecimal);
                        }
                    }
                    else
                    {
                        fillFieldDescripter(fieldName, catagory, maxLength, maxDecimal);
                    }
                }

                buildFieldDescripter(GetSymbol(fieldType), (byte)fl["max"], (byte)fl["dec"], fl["dim"]);
            }

            BU.AddBytes(bList, (byte)13);                                                           //   n Field array terminator, 13 （位元組n：欄位陣列終止符，13。）
            dbf.Append(bList.ToArray());
            bList.Clear();
            dbf.ReplaceBytes(8, (short)dbf.Length);                                                 // Complete Byte 8 (header length) in dBASE header. （完成 dBASE 標頭的位元組8（標頭長度）。）
            int _dbfHL = dbf.Length;

            int _id = 1;
            int _ttLen = 0;
            List<CartoObject> objs = suppressFieldError ? Objects.Where(o => o.Edges.ContainsKey(field)).ToList() : Objects;
            List<double> _hList = new List<double>();
            List<int> _conLen = new List<int>();
            List<int> _offset = new List<int>();
            double3 _trans = new double3(_center.e, 0, _center.n);
            float3 _coord = new float3(objs[0].Edges[field][0].x, objs[0].Edges[field][0].y, objs[0].Edges[field][0].z);
            Bounds3 _bound = new Bounds3(_coord, _coord);

            foreach (CartoObject obj in objs)
            {                                                       // === RECORD HEADER OF .SHP （.shp 的記錄標頭。）===
                BU.AddBytes(bList, BU.FlipEndian(_id));             //   0 Record number (BIG ENDIAN). （位元組0：記錄編號（大端序）。）
                BU.Skip(bList, 1);                                  //   4 Content length in 16-bit words (BIG ENDIAN). （位元組4：以16位元計的內容長度（大端序）。）
                _offset.Add((_ttLen + 100) / 2);

                if (shpType == GeometryType.Point)
                {
                    float3 p = obj.Edges[field][0];
                    double3 _p = p + _trans;
                    if (p.x > _bound.max.x) _bound.max.x = p.x;
                    if (p.x < _bound.min.x) _bound.min.x = p.x;
                    if (p.y > _bound.max.y) _bound.max.y = p.y;
                    if (p.y < _bound.min.y) _bound.min.y = p.y;
                    if (p.z > _bound.max.z) _bound.max.z = p.z;
                    if (p.z < _bound.min.z) _bound.min.z = p.z;
                    // === RECORD CONTENT OF .SHP（.shp 的記錄內容。）===
                    BU.AddBytes(bList, 11);                                 //   8 Shape Type. （位元組0：幾何類型。）
                    BU.AddBytes(bList, _p.x, _p.z, _p.y);                   //  12 Coordinate. （位元組4：坐標。）
                    BU.Skip(bList, 2);                                      //  36
                }
                else
                {
                    if (shpType == GeometryType.LineString)
                    {                                                       // === RECORD CONTENT OF .SHP（.shp 的記錄內容。）===
                        BU.AddBytes(bList, 13);                             //   8 Shape Type. （位元組0：幾何類型。）
                        BU.Skip(bList, 8);                                  //  12 Bounding box for X, Y （位元組4：X、Y邊界框範圍。）
                        BU.AddBytes(bList, 1);                              //  44 Number of parts. （位元組36：部件的數量。）
                        BU.AddBytes(bList, obj.Edges[field].Count);         //  48 Number of points. （位元組40：點的數量。）
                    }
                    else
                    {                                                       // === RECORD CONTENT OF .SHP（.shp 的記錄內容。）===
                        BU.AddBytes(bList, 15);                             //   8 Shape Type. （位元組0：幾何類型。）
                        BU.Skip(bList, 8);                                  //  12 Bounding box for X, Y （位元組4：X、Y邊界框範圍。）
                        BU.AddBytes(bList, 1);                              //  44 Number of parts. （位元組36：部件的數量。）
                        BU.AddBytes(bList, obj.Edges[field].Count + 1);     //  48 Number of points. （位元組40：點的數量。）
                    }

                    float3 coordL = new float3(obj.Edges[field][0].x, obj.Edges[field][0].y, obj.Edges[field][0].z);
                    Bounds3 boundL = new Bounds3(coordL, coordL);
                    BU.Skip(bList, 1);                                      //  52 Part. （位元組44：部件。）

                    foreach (float3 p in obj.Edges[field])
                    {
                        double3 _p = p + _trans;
                        if (p.x > _bound.max.x) _bound.max.x = p.x;
                        if (p.x < _bound.min.x) _bound.min.x = p.x;
                        if (p.y > _bound.max.y) _bound.max.y = p.y;
                        if (p.y < _bound.min.y) _bound.min.y = p.y;
                        if (p.z > _bound.max.z) _bound.max.z = p.z;
                        if (p.z < _bound.min.z) _bound.min.z = p.z;
                        if (p.x > boundL.max.x) boundL.max.x = p.x;
                        if (p.x < boundL.min.x) boundL.min.x = p.x;
                        if (p.y > boundL.max.y) boundL.max.y = p.y;
                        if (p.y < boundL.min.y) boundL.min.y = p.y;
                        if (p.z > boundL.max.z) boundL.max.z = p.z;
                        if (p.z < boundL.min.z) boundL.min.z = p.z;

                        BU.AddBytes(bList, _p.x, _p.z);                     //   x Coordinate for X, Y. （位元組X：X、Y坐標。）
                        _hList.Add(_p.y);
                    }

                    // Polygon's last coordinate is its first coordinate.
                    // （多邊形的最後一個坐標是它的第一個坐標。）
                    if (shpType == GeometryType.Polygon)
                    {
                        BU.AddBytes(bList, obj.Edges[field][0].x + _trans.x, obj.Edges[field][0].z + _trans.z);
                        _hList.Add(obj.Edges[field][0].y + _trans.y);
                    }

                    boundL += (float3)_trans;
                    BU.ReplaceBytes(bList, 12, (double)boundL.min.x);                   // Complete Byte 12 (Bounding box for X, Y). （完成位元組12（X、Y邊界框範圍）。）
                    BU.ReplaceBytes(bList, 20, (double)boundL.min.z);
                    BU.ReplaceBytes(bList, 28, (double)boundL.max.x);
                    BU.ReplaceBytes(bList, 36, (double)boundL.max.z);
                    BU.AddBytes(bList, (double)boundL.max.y, (double)boundL.min.y);     //      y Bounding box for Z. （位元組Y：Z邊界框範圍。）
                    foreach (double h in _hList) BU.AddBytes(bList, h);                 // y + 16 Coordinate for Z. （位元組Y + 16：Z坐標。）
                    _hList.Clear();
                }

                BU.ReplaceBytes(bList, 4, BU.FlipEndian((bList.Count - 8) / 2));        // Complete Byte 4 (Content length). （完成位元組4（內容長度）。）
                _conLen.Add((bList.Count - 8) / 2);
                _ttLen += bList.Count;
                shp.Append(bList.ToArray());
                bList.Clear();

                /// <summary>
                /// A function to trim the value if the input exceeds the maximum length.
                /// （一個將超過最大長度的輸入值裁減的函式。）
                /// </summary>
                string trimValue(object fieldRecord, string fieldName)
                {
                    string fieldType = ExportUtils.ExportFieldTypes[fieldName].Name;
                    Dictionary<string, int> fl = fieldLength[fieldType];

                    try
                    {
                        // Force all string go to exception section to prevent numeric strings like "123" pass the next check.
                        // （強迫所有字串進入例外處理，以排除可被轉換為數字的字串通過下個檢查。）
                        if (fieldType == "String")
                        {
                            _ = (bool)fieldRecord;
                        }

                        // If the input can be converted into `double`, it means that the value is a numeric type. （如果輸入值可以被轉換為`double`，就表示輸入值是一個數值。）
                        _ = SystemConvert.ToDouble(fieldRecord);

                        // If the input converted into `long` is same as the one converted into `double`, it means that the value is an integer.
                        // （如果輸入值轉換成`long`的值和轉換成`double`的值相同，就表示輸入值是一個整數。）
                        long longR = SystemConvert.ToInt64(fieldRecord);
                        double doubleR = SystemConvert.ToDouble(fieldRecord);
                        if (longR == doubleR)
                        {
                            string lNum = $"{longR}";
                            string spaceHolder1 = string.Empty;

                            if (Encoding.UTF8.GetBytes(lNum).Length > fl["max"])
                            {
                                string dec = new string('#', fl["max"] - ((longR > 0) ? 6 : 7));
                                dec = "0." + dec + "e+00";
                                lNum = longR.ToString(dec);
                            }

                            if (fl["max"] - Encoding.UTF8.GetBytes(lNum).Length > 0)
                            {
                                spaceHolder1 = new string(' ', fl["max"] - Encoding.UTF8.GetBytes(lNum).Length);
                            }

                            return spaceHolder1 + lNum;
                        }
                        else
                        {
                            string dNum = $"{doubleR}";
                            string spaceHolder2 = string.Empty;

                            while ((dNum.Length - dNum.IndexOf('.') - 1 > fl["dec"]) && (dNum.IndexOfAny(new char[] { 'E', 'e' }) == -1) && (dNum.IndexOf('.') != -1))
                            {
                                int decPlaces = dNum.Length - dNum.IndexOf('.') - 1;
                                dNum = $"{Math.Round(doubleR, decPlaces - 1)}";
                            }

                            if (Encoding.UTF8.GetBytes(dNum).Length > fl["max"])
                            {
                                string dec = new string('#', fl["max"] - ((doubleR > 0) ? 6 : 7));
                                dec = "0." + dec + "e+00";
                                dNum = doubleR.ToString(dec);
                            }

                            if (fl["max"] - Encoding.UTF8.GetBytes(dNum).Length > 0)
                            {
                                spaceHolder2 = new string(' ', fl["max"] - Encoding.UTF8.GetBytes(dNum).Length);
                            }

                            return spaceHolder2 + dNum;
                        }
                    }
                    catch (InvalidCastException)
                    {
                        switch (fieldRecord)
                        {
                            case bool @bool:
                                return (@bool ? "1" : "0");

                            case char @char:
                                string spaceHolder1 = new string(' ', fl["max"] - Encoding.UTF8.GetBytes($"{@char}").Length);
                                return $"{@char}" + spaceHolder1;

                            case string @string:
                                string temp = @string;
                                while (Encoding.UTF8.GetBytes($"{temp}").Length > fl["max"]) temp = temp.Substring(0, temp.Length - 1);

                                if (Encoding.UTF8.GetBytes($"{temp}").Length < fl["max"])
                                {
                                    string spaceHolder2 = new string(' ', fl["max"] - Encoding.UTF8.GetBytes($"{temp}").Length);
                                    return $"{temp}" + spaceHolder2;
                                }
                                else
                                {
                                    return temp;
                                }
                        }
                        // Other unexpected inputs will be handle here. （其餘未預期的輸入值會在這裡處理。 ）
                        return new string(' ', fl["max"]);
                    }
                }

                BU.AddBytes(bList, " ");

                foreach (string name in propNames)
                {
                    try
                    {
                        object data = obj.Properties.TryGetValue(name, out object v) ? v : null;

                        switch (data)
                        {
                            case int2 @int2:
                                BU.AddBytes(bList, trimValue(int2.x, name));
                                BU.AddBytes(bList, trimValue(int2.y, name));
                                break;

                            case uint2 @uint2:
                                BU.AddBytes(bList, trimValue(uint2.x, name));
                                BU.AddBytes(bList, trimValue(uint2.y, name));
                                break;

                            case float2 @float2:
                                BU.AddBytes(bList, trimValue(float2.x, name));
                                BU.AddBytes(bList, trimValue(float2.y, name));
                                break;

                            case double2 @double2:
                                BU.AddBytes(bList, trimValue(double2.x, name));
                                BU.AddBytes(bList, trimValue(double2.y, name));
                                break;

                            case int3 @int3:
                                BU.AddBytes(bList, trimValue(int3.x, name));
                                BU.AddBytes(bList, trimValue(int3.y, name));
                                BU.AddBytes(bList, trimValue(int3.z, name));
                                break;

                            case uint3 @uint3:
                                BU.AddBytes(bList, trimValue(uint3.x, name));
                                BU.AddBytes(bList, trimValue(uint3.y, name));
                                BU.AddBytes(bList, trimValue(uint3.z, name));
                                break;

                            case float3 @float3:
                                BU.AddBytes(bList, trimValue(float3.x, name));
                                BU.AddBytes(bList, trimValue(float3.y, name));
                                BU.AddBytes(bList, trimValue(float3.z, name));
                                break;

                            case double3 @double3:
                                BU.AddBytes(bList, trimValue(double3.x, name));
                                BU.AddBytes(bList, trimValue(double3.y, name));
                                BU.AddBytes(bList, trimValue(double3.z, name));
                                break;

                            case Address @Address:
                                BU.AddBytes(bList, trimValue(Address.District, $"{name}.district"));
                                BU.AddBytes(bList, trimValue(Address.Street, $"{name}.street"));
                                BU.AddBytes(bList, trimValue(Address.Number, $"{name}.number"));
                                break;

                            default:
                                BU.AddBytes(bList, trimValue(data, name));
                                break;
                        }
                    }
                    catch (NullReferenceException)
                    {
                        for (int j = 0; j < fieldLength[ExportUtils.ExportFieldTypes[name].Name]["max"]; j++)
                        {
                            BU.AddBytes(bList, " ");
                        }
                    }
                }

                dbf.Append(bList.ToArray());
                bList.Clear();
                _id++;
            }

            _bound += (float3)_trans;
            shp.ReplaceBytes(24, BU.FlipEndian(shp.Length / 2));    // Complete Byte 24 (File length).（完成位元組24（檔案長度）。）
            shp.ReplaceBytes(36, (double)_bound.min.x);             // Complete Byte 36 (Bounding box). （完成位元組36（邊界框範圍）。）
            shp.ReplaceBytes(44, (double)_bound.min.z);
            shp.ReplaceBytes(52, (double)_bound.max.x);
            shp.ReplaceBytes(60, (double)_bound.max.z);
            shp.ReplaceBytes(68, (double)_bound.min.y);
            shp.ReplaceBytes(76, (double)_bound.max.y);
            shx.ReplaceBytes(36, (double)_bound.min.x);
            shx.ReplaceBytes(44, (double)_bound.min.z);
            shx.ReplaceBytes(52, (double)_bound.max.x);
            shx.ReplaceBytes(60, (double)_bound.max.z);
            shx.ReplaceBytes(68, (double)_bound.min.y);
            shx.ReplaceBytes(76, (double)_bound.max.y);

            for (int i = 0; i < objs.Count; i++) BU.AddBytes(bList, BU.FlipEndian(_offset[i]), BU.FlipEndian(_conLen[i]));
            shx.Append(bList.ToArray());
            bList.Clear();
            shx.ReplaceBytes(24, BU.FlipEndian(shx.Length / 2));    // Complete Byte 24 (File length).（完成位元組24（檔案長度）。）

            dbf.ReplaceBytes(10, (short)((dbf.Length - _dbfHL) / objs.Count));          // Complete Byte 10 (Length per record).（完成位元組10（每筆記錄長度）。）
            dbf.Append(new byte[1] { 26 });                                             // Byte -1: Terminator, 26 （位元組-1：終止符，26。）

            // Write the projection file. （寫出投影檔案。）
            var prj = new StringBuilder();
            prj.AppendLine($"PROJCS[\"WGS_1984_UTM_Zone_{_center.z}{_center.h}\",");
            prj.AppendLine("\tGEOGCS[\"GCS_WGS_1984\",");
            prj.AppendLine("\t\tDATUM[\"D_WGS_1984\",");
            prj.AppendLine("\t\t\tSPHEROID[\"WGS_1984\",6378137.0,298.257223563]],");
            prj.AppendLine("\t\tPRIMEM[\"Greenwich\",0.0],");
            prj.AppendLine("\t\tUNIT[\"Degree\",0.0174532925199433]],");
            prj.AppendLine($"\tPROJECTION[\"Transverse_Mercator\"],");
            prj.AppendLine("\tPARAMETER[\"False_Easting\",500000.0],");
            prj.AppendLine($"\tPARAMETER[\"False_Northing\",{(_center.h == "N" ? 0 : 10000000)}.0],");
            prj.AppendLine($"\tPARAMETER[\"Central_Meridian\",{(_center.z - 1) * 6 + 3 - 180}.0],");
            prj.AppendLine("\tPARAMETER[\"Scale_Factor\",0.9996],");
            prj.AppendLine("\tPARAMETER[\"Latitude_Of_Origin\",0.0],");
            prj.AppendLine($"\tUNIT[\"Meter\",1.0]]");
            File.WriteAllText(prjPath, string.Empty, Encoding.UTF8);
            File.AppendAllText(prjPath, prj.ToString(), Encoding.UTF8);

            // Write the codepage file.（寫出字元編碼頁檔案。）
            File.WriteAllText(cpgPath, string.Empty, Encoding.UTF8);
            File.AppendAllText(cpgPath, "UTF-8", Encoding.UTF8);

            m_Log.Info($"`{fileName}` has been exported. `{fileName}`已經輸出完成。");
        }
    }
}