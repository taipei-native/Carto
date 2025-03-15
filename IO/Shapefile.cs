using Carto.Geodata;
using Carto.Utils;
using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Unity.Mathematics;

namespace Carto.IO
{
    /// <summary>
    /// The class that provides utility functions to write ESRI Shapefile.<br/>
    /// （提供寫出 ESRI Shapefile 功能的類別。）
    /// </summary>
    public static class Shapefile
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

        /// <summary>
        /// The character string.
        /// （字串。）
        /// </summary>
        public const char fieldTypeCharacter = 'C';
        
        /// <summary>
        /// The floating-point number.
        /// （浮點數。）
        /// </summary>
        public const char fieldTypeFloat = 'F';

        /// <summary>
        /// The integer number.
        /// （整數。）
        /// </summary>
        public const char fieldTypeNumber = 'N';

        /// <summary>
        /// Points on the X-Y plane.
        /// （XY 平面上的點。）
        /// </summary>
        public const int shapeTypePoint = 1;

        /// <summary>
        /// Line segments on the X-Y plane.
        /// （XY 平面上的線段。）
        /// </summary>
        public const int shapeTypePolyLine = 3;

        /// <summary>
        /// Polygons on the X-Y plane.
        /// （XY 平面上的多邊形。）
        /// </summary>
        public const int shapeTypePolygon = 5;

        /// <summary>
        /// Points in the X-Y-Z space.
        /// （XYZ 空間內的點。）
        /// </summary>
        public const int shapeTypePointZ = 11;

        /// <summary>
        /// Line segments in the X-Y-Z space.
        /// （XYZ 空間內的線段。）
        /// </summary>
        public const int shapeTypePolyLineZ = 13;

        /// <summary>
        /// Polygons in the X-Y-Z space.
        /// （XYZ 空間內的多邊形。）
        /// </summary>
        public const int shapeTypePolygonZ = 15;

        /// <summary>
        /// The look-up table for each composite field's type.
        /// （每個複合欄位代表型別的對照表。）
        /// </summary>
        public static readonly Dictionary<Property, char[]> CompositeFieldTypeTable = new()
        {
            { Property.Address, new char[3] { fieldTypeCharacter, fieldTypeCharacter, fieldTypeNumber } },
            { Property.Resident, new char[2] { fieldTypeNumber, fieldTypeNumber } }
        };

        /// <summary>
        /// The look-up table for each field's type.
        /// （每個欄位代表型別的對照表。）
        /// </summary>
        public static readonly Dictionary<Type, char> FieldTypeTable = new()
        {
            { typeof(bool), fieldTypeNumber },
            { typeof(int), fieldTypeNumber },
            { typeof(float), fieldTypeFloat },
            { typeof(string), fieldTypeCharacter }
        };

        /// <summary>
        /// The index pair in the .shx file.
        /// （.shx 檔案的索引對。）
        /// </summary>
        public struct IndexPair
        {
            /// <summary>
            /// The length of the feature record in 16-bit.
            /// （圖徵紀錄以 16 位元計的長度。）
            /// </summary>
            public int length;
            
            /// <summary>
            /// The offset of the feature in 16-bit.
            /// （圖徵以 16 位元計的偏移量。）
            /// </summary>
            public int offset;

            public IndexPair(int offset, int length)
            {
                this.offset = offset;
                this.length = length;
            }
        }

        /// <summary>
        /// The delegate of the WriteSHP() methods implemented in each system.
        /// （在各個系統實作的 WriteSHP() 方法的委派。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        public delegate void WriteSHPMethod(BinaryWriter writer, Options options, out List<IndexPair> indexPairs, out Bounds3 bounds);

        /// <summary>
        /// Retrieve the shape type code from vector kinds.
        /// （由向量類別獲得幾何種類代號。）
        /// </summary>
        /// <param name="vectorKind">The classification of exported vector objects.（對輸出向量物體的分類。）</param>
        /// <param name="includeElevation">Whether to export the elevation or not.（是否要輸出高程？）</param>
        /// <returns>The shape code used in the shapefile.（在 Shapefile 中使用的幾何代號。）</returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetShapeType(VectorKind vectorKind, bool includeElevation)
        {
            return vectorKind switch
            {
                VectorKind.Boundary => includeElevation ? shapeTypePolygonZ : shapeTypePolygon,
                VectorKind.Centerline => includeElevation ? shapeTypePolyLineZ : shapeTypePolyLine,
                VectorKind.Footprint => includeElevation ? shapeTypePolygonZ : shapeTypePolygon,
                VectorKind.Location => includeElevation ? shapeTypePointZ : shapeTypePoint,
                _ => throw new ArgumentException("Only points, line strings and (multi-) polygons can be exported as Shapefile. 只有點、線段和（複合）多邊形可以被輸出為 Shapefile。")
            };
        }

        /// <summary>
        /// Update the .shp file header.
        /// （更新 .shp 檔案的標頭。）
        /// </summary>
        /// <param name="fs">The current file stream.（目前的檔案資料流。）</param>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box of the features.（圖徵的定界框。）</param>
        /// <param name="length">The length of the file.（檔案的長度。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        private static void UpdateSHPHeader(FileStream fs, BinaryWriter writer, Bounds3 bounds, int length, bool writeElevation)
        {
            if (BitConverter.IsLittleEndian)
            {
                fs.Seek(24, SeekOrigin.Begin);
                writer.Write(IOUtils.GetFlippedBytes(length), 0, 4);
                fs.Seek(36, SeekOrigin.Begin);
                writer.Write(BitConverter.GetBytes((double)bounds.x.min), 0, 8);
                writer.Write(BitConverter.GetBytes((double)bounds.y.min), 0, 8);
                writer.Write(BitConverter.GetBytes((double)bounds.x.max), 0, 8);
                writer.Write(BitConverter.GetBytes((double)bounds.y.max), 0, 8);
                if (writeElevation)
                {
                    writer.Write(BitConverter.GetBytes((double)bounds.z.min), 0, 8);
                    writer.Write(BitConverter.GetBytes((double)bounds.z.max), 0, 8);
                }
            }
            else
            {
                fs.Seek(24, SeekOrigin.Begin);
                writer.Write(BitConverter.GetBytes(length), 0, 4);
                fs.Seek(36, SeekOrigin.Begin);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.min), 0, 8);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.min), 0, 8);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.max), 0, 8);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.max), 0, 8);
                if (writeElevation)
                {
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.min), 0, 8);
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.max), 0, 8);
                }
            }
        }

        /// <summary>
        /// Write the ESRI Shapefile.
        /// （寫出 ESRI Shapefile。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="systemName">The exporting system's name.（輸出系統的名稱。）</param>
        /// <param name="vectorKind">The classification of exported vector objects.（對輸出向量物體的分類。）</param>
        /// <param name="writeSHPMethod">The WriteSHP() method implemented in each system.（各系統實作的 WriteSHP() 方法。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public static void Write(Options options, System systemName, VectorKind vectorKind, WriteSHPMethod writeSHPMethod, Action<string, int> onReportMethod)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (options == null) throw new ArgumentNullException("The parameters cannot be null. 參數不可為空值。");
            string filePath = options.GetFilePath(systemName, vectorKind);
            string dbfPath = Path.ChangeExtension(filePath, "dbf");
            string shxPath = Path.ChangeExtension(filePath, "shx");
            Bounds3 bounds;
            int shape = GetShapeType(vectorKind, options.Elevation);
            List<IndexPair> indexPairs;

            // The shapefile is a format composed by at least three sidecar files - which means multiple files have to be generated.
            // （Shapefile 是一個由至少三個檔案組成的檔案格式－這表示需要產生出複數個檔案。）

            // Unlike TIFF, which supports writing in both endian, Shapefile has strict rule when writing files.
            // （不同於 TIFF 支援以兩種端序寫入，Shapefile 對於端序有嚴格的要求。）

            using (FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using BinaryWriter writer = new(fs);
                WriteSHPHeader(writer, shape);
                writeSHPMethod.Invoke(writer, options, out indexPairs, out bounds);
                IndexPair lastIndexPair = indexPairs[indexPairs.Count - 1];
                UpdateSHPHeader(fs, writer, bounds, lastIndexPair.offset + lastIndexPair.length, options.Elevation);
            }

            using (FileStream fs = new(shxPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using BinaryWriter writer = new(fs);
                WriteSHX(writer, shape, bounds, indexPairs);
            }

            using (FileStream fs = new(dbfPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using BinaryWriter writer = new(fs);
                WriteDBFHeader(fs, writer, options, systemName, indexPairs.Count, out HashSet<Property> validatedFields);
            }

            stopwatch.Stop();
            Instance.Log.Debug($"Write '{Path.GetFileName(filePath)}' in {CommonUtils.FormatTimeSpan(stopwatch.Elapsed)}.");
        }

        /// <summary>
        /// Write the bounding box object to the file in big endian.
        /// （以大端序寫入定界框至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        private static void WriteBoxBE(BinaryWriter writer, Bounds3 bounds)
        {
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.min));
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.min));
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.max));
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.max));
        }

        /// <summary>
        /// Write the bounding box object to the file in little endian.
        /// （以小端序寫入定界框至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        private static void WriteBoxLE(BinaryWriter writer, Bounds3 bounds)
        {
            writer.Write(BitConverter.GetBytes((double)bounds.x.min));
            writer.Write(BitConverter.GetBytes((double)bounds.y.min));
            writer.Write(BitConverter.GetBytes((double)bounds.x.max));
            writer.Write(BitConverter.GetBytes((double)bounds.y.max));
        }

        /// <summary>
        /// Write the file header for .dbf files.
        /// （寫入 .dbf 檔案的標頭。）
        /// </summary>
        /// <param name="fs">The current file stream.（目前的檔案資料流。）</param>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="systemName">The exporting system's name.（輸出系統的名稱。）</param>
        /// <param name="count">The number of features in the .shp file.（.shp 檔案中的圖徵數量。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        private static void WriteDBFHeader(FileStream fs, BinaryWriter writer, Options options, System systemName, int count, out HashSet<Property> validatedFields)
        {
            validatedFields = new();
            
            writer.Write((byte)3);
            writer.Write((byte)math.clamp((DateTime.UtcNow.Year - 1900) % 256, 0, 255)); // In case of some naughty users change their date beyond the year 2155.（以防有調皮的使用者將日期設在 2155 年以後。） 
            writer.Write((byte)DateTime.UtcNow.Month);
            writer.Write((byte)DateTime.UtcNow.Day);

            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BitConverter.GetBytes(count));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes(count));
            }

            IOUtils.SkipBytes(writer, 24);

            if (IO.AvailablePropertyTable.TryGetValue(systemName, out HashSet<Property> superset) && options.Properties.TryGetValue(systemName, out HashSet<Property> subset))
            {
                HashSet<Property> subsetCopy = new(subset);
                subsetCopy.IntersectWith(superset);
                WriteFieldDescriptors(writer, options, subsetCopy, out validatedFields);
            }

            writer.Write((byte)13);

            int headerLength = IOUtils.GetPosition(writer);
            fs.Seek(8, SeekOrigin.Begin);

            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BitConverter.GetBytes((short)headerLength));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes((short)headerLength));
            }

            fs.Seek(headerLength, SeekOrigin.Begin);
        }

        /// <summary>
        /// Write a field descriptor to the .dbf file.
        /// （寫入一個欄位描述至 .dbf 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="fieldName">The field's title.（欄位的標題。）</param>
        /// <param name="symbol">The type of the field.（欄位的型別。）</param>
        private static void WriteFieldDescriptor(BinaryWriter writer, string fieldName, char symbol)
        {
            int fieldNameLength = Encoding.UTF8.GetByteCount(fieldName);

            while (fieldNameLength > 10)
            {
                fieldName = fieldName.Substring(0, fieldName.Length - 1);
                fieldNameLength = Encoding.UTF8.GetByteCount(fieldName);
            }

            if (BitConverter.IsLittleEndian)
            {
                writer.Write(Encoding.UTF8.GetBytes(fieldName));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes(fieldName));
            }

            IOUtils.SkipBytes(writer, 11 - fieldNameLength);
            writer.Write(BitConverter.GetBytes(symbol));
            IOUtils.SkipBytes(writer, 20);
        }

        /// <summary>
        /// Write field decriptors to the .dbf file.
        /// （寫入欄位描述至 .dbf 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="fields">The fields to write.（預計寫入的欄位。）</param>
        /// <param name="validatedFields">The fields actually written.（實際寫入的欄位。）</param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private static void WriteFieldDescriptors(BinaryWriter writer, Options options, HashSet<Property> fields, out HashSet<Property> validatedFields)
        {
            HashSet<Property>.Enumerator enumerator = fields.GetEnumerator();
            validatedFields = new();

            while (enumerator.MoveNext())
            {
                Property field = enumerator.Current;
                string fieldName = Enum.GetName(typeof(Property), field);

                // Validate property registration.（檢驗屬性是否已被註冊。）
                Type fieldType = IO.GetPropertyType(field, fieldName, options);

                // Write non-composite fields.（寫入非複合欄位。）
                if (FieldTypeTable.TryGetValue(fieldType, out char fieldSymbol))
                {
                    WriteFieldDescriptor(writer, fieldName, fieldSymbol);
                    validatedFields.Add(field);
                    continue;
                }

                // Write composite fields.（寫入複合欄位。）

                if (!IO.CompositePropertyTable.TryGetValue(field, out Dictionary<FileFormat, string[]> subFieldTitleTable))
                {
                    throw new KeyNotFoundException($"The property `{fieldName}` is not in CompositePropertyTable. 屬性 `{fieldName}` 未紀錄於 CompositePropertyTable。");
                }

                if (!subFieldTitleTable.TryGetValue(FileFormat.Shapefile, out string[] subFieldTitles) &&
                    !subFieldTitleTable.TryGetValue(FileFormat.Unknown, out subFieldTitles))
                {
                    throw new KeyNotFoundException($"Fallback titles missing for property `{fieldName}` in CompositePropertyTable. CompositePropertyTable 未紀錄屬性 {fieldName} 的後備標題。");
                }

                if (!CompositeFieldTypeTable.TryGetValue(field, out char[] fieldSymbols))
                {
                    throw new KeyNotFoundException($"The property `{fieldName}` is not in CompositeFieldTypeTable. 屬性 `{fieldName}` 未紀錄於 CompositeFieldTypeTable。");
                }

                if (fieldSymbols.Length != subFieldTitles.Length)
                {
                    throw new ArgumentException($"Array length mismatch: expected {subFieldTitles.Length}, but got {fieldSymbols.Length}. 陣列長度不符：預期為 {subFieldTitles.Length}，實際為 {fieldSymbols.Length}。");
                }

                for (int i = 0; i < subFieldTitles.Length; i++)
                {
                    WriteFieldDescriptor(writer, subFieldTitles[i], fieldSymbols[i]);
                }

                validatedFields.Add(field);
            }
        }

        /// <summary>
        /// Write the geometry to the file in little endian.
        /// （以小端序寫入幾何圖形至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="id">The unique identifier of the feature.（圖徵的獨特識別碼。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        /// <param name="geometry">The geometry of the feature.（圖徵的幾何圖形。）</param>
        /// <param name="indexPair">The index pair used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box of the feature.（圖徵的定界框。）</param>
        public static void WriteGeometryLE(BinaryWriter writer, int id, int shape, Geometry geometry, out IndexPair indexPair, out Bounds3 bounds)
        {
            int numParts = geometry.GetParts(out int numPoints, out List<int> pointCounts, out bounds);
            int offset = IOUtils.GetPosition(writer) / 2;
            int length = 0;

            writer.Write(IOUtils.GetFlippedBytes(id));              // Record number.（紀錄編號。）

            switch (shape)
            {
                case shapeTypePoint:
                    length = 14;
                    break;

                case shapeTypePolyLine:
                    length = 28 + 8 * numPoints;
                    break;

                case shapeTypePolygon:
                    length = 26 + 2 * numParts + 8 * (numPoints + numParts);
                    break;

                case shapeTypePointZ:
                    length = 22;
                    break;

                case shapeTypePolyLineZ:
                    length = 36 + 12 * numPoints;
                    break;

                case shapeTypePolygonZ:
                    length = 34 + 2 * numParts + 12 * (numPoints + numParts);
                    break;
            }

            indexPair = new(offset, length);
            writer.Write(IOUtils.GetFlippedBytes(length));          // Content length.（內容長度。）
            writer.Write(BitConverter.GetBytes(shape));             // Shape type.（幾何形狀。）

            switch (shape)
            {
                case shapeTypePoint:
                    WritePointLE(writer, geometry.Inclusions[0][0], false);
                    return;

                case shapeTypePointZ:
                    WritePointLE(writer, geometry.Inclusions[0][0], true);
                    return;
            }
            
            WriteBoxLE(writer, bounds);                             // Box.（定界框。）
            writer.Write(BitConverter.GetBytes(numParts));          // NumParts.（部件的數量。）
            writer.Write(BitConverter.GetBytes(numPoints));         // NumPoints.（點的數量。）

            WritePointArraysLE(writer, geometry, pointCounts, (shape == shapeTypePolygon) || (shape == shapeTypePolygonZ), out List<double> zArray);

            if ((shape == shapeTypePolyLineZ) || (shape == shapeTypePolygonZ))
            {
                WriteZArrayLE(writer, bounds, zArray);
            }
        }

        /// <summary>
        /// Write the point array to the file in little endian.
        /// （以小端序寫入點陣列至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="geometry">The value waiting to be written.（等待被寫出的數值。）</param>
        /// <param name="pointCounts">The number of points in each part.（每個部件包含的點數。）</param>
        /// <param name="isRing">Whether the array represents a ring or not.（陣列是否為一個環？）</param>
        /// <param name="zArray">The array of z values.（Z值的陣列。）</param>
        private static void WritePointArraysLE(BinaryWriter writer, Geometry geometry, List<int> pointCounts, bool isRing, out List<double> zArray)
        {
            int pointIndex = 0;
            for (int i = 0; i < pointCounts.Count; i++)
            {
                writer.Write(BitConverter.GetBytes(pointIndex));
                pointIndex += pointCounts[i];
                if (isRing) pointIndex++;
            }

            zArray = new();
            for (int i = 0; i < geometry.Inclusions.Length; i++)
            {
                for (int j = 0; j < geometry.Inclusions[i].Length; j++)
                {
                    WritePointLE(writer, geometry.Inclusions[i][j], false);
                    zArray.Add(geometry.Inclusions[i][j].z);
                }
                if (isRing)
                {
                    WritePointLE(writer, geometry.Inclusions[i][0], false);
                    zArray.Add(geometry.Inclusions[i][0].z);
                }
            }

            for (int i = 0; i < geometry.Exclusions.Length; i++)
            {
                for (int j = 0; j < geometry.Exclusions[i].Length; j++)
                {
                    for (int k = 0; k < geometry.Exclusions[i][j].Length; k++)
                    {
                        WritePointLE(writer, geometry.Exclusions[i][j][k], false);
                        zArray.Add(geometry.Exclusions[i][j][k].z);
                    }
                    if (isRing)
                    {
                        WritePointLE(writer, geometry.Exclusions[i][j][0], false);
                        zArray.Add(geometry.Exclusions[i][j][0].z);
                    }
                }
            }
        }

        /// <summary>
        /// Write the point to the file in big endian.
        /// （以大端序寫入點至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The value waiting to be written.（等待被寫入的數值。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        private static void WritePointBE(BinaryWriter writer, float3 value, bool writeElevation)
        {
            writer.Write(IOUtils.GetFlippedBytes((double)value.x));
            writer.Write(IOUtils.GetFlippedBytes((double)value.y));
            if (writeElevation)
            {
                writer.Write(IOUtils.GetFlippedBytes((double)value.z));
                IOUtils.SkipBytes(writer, 8);
            }
        }

        /// <summary>
        /// Write the point to the file in little endian.
        /// （以小端序寫入點至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The value waiting to be written.（等待被寫入的數值。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        private static void WritePointLE(BinaryWriter writer, float3 value, bool writeElevation)
        {
            writer.Write(BitConverter.GetBytes((double)value.x));
            writer.Write(BitConverter.GetBytes((double)value.y));
            if (writeElevation)
            {
                writer.Write(BitConverter.GetBytes((double)value.z));
                IOUtils.SkipBytes(writer, 8);
            }
        }

        /// <summary>
        /// Write the file header for .shp files.
        /// （寫入 .shp 檔案的標頭。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        private static void WriteSHPHeader(BinaryWriter writer, int shape)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(IOUtils.GetFlippedBytes(9994));
                IOUtils.SkipBytes(writer, 24);
                writer.Write(BitConverter.GetBytes(1000));
                writer.Write(BitConverter.GetBytes(shape));
            }
            else
            {
                writer.Write(BitConverter.GetBytes(9994));
                IOUtils.SkipBytes(writer, 24);
                writer.Write(IOUtils.GetFlippedBytes(1000));
                writer.Write(IOUtils.GetFlippedBytes(shape));
            }

            IOUtils.SkipBytes(writer, 64);
        }

        /// <summary>
        /// Write the .shx file.
        /// （寫入 .shx 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        /// <param name="bounds">The bounding box of the features.（圖徵的定界框。）</param>
        /// <param name="indexPairs">The index pairs indicating the offset and the length of each record.（顯示每個紀錄偏移與長度的索引對。）</param>
        private static void WriteSHX(BinaryWriter writer, int shape, Bounds3 bounds, List<IndexPair> indexPairs)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(IOUtils.GetFlippedBytes(9994));
                IOUtils.SkipBytes(writer, 20);
                writer.Write(IOUtils.GetFlippedBytes(50 + indexPairs.Count * 4));
                writer.Write(BitConverter.GetBytes(1000));
                writer.Write(BitConverter.GetBytes(shape));
                writer.Write(BitConverter.GetBytes((double)bounds.x.min));
                writer.Write(BitConverter.GetBytes((double)bounds.y.min));
                writer.Write(BitConverter.GetBytes((double)bounds.x.max));
                writer.Write(BitConverter.GetBytes((double)bounds.y.max));
            }
            else
            {
                writer.Write(BitConverter.GetBytes(9994));
                IOUtils.SkipBytes(writer, 20);
                writer.Write(BitConverter.GetBytes(50 + indexPairs.Count * 4));
                writer.Write(IOUtils.GetFlippedBytes(1000));
                writer.Write(IOUtils.GetFlippedBytes(shape));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.min));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.min));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.max));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.max));
            }

            if ((shape == shapeTypePointZ) || (shape == shapeTypePolyLineZ) || (shape == shapeTypePolygonZ))
            {
                if (BitConverter.IsLittleEndian)
                {
                    writer.Write(BitConverter.GetBytes((double)bounds.z.min));
                    writer.Write(BitConverter.GetBytes((double)bounds.z.max));
                }
                else
                {
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.min));
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.max));
                }

                IOUtils.SkipBytes(writer, 16);
            }
            else
            {
                IOUtils.SkipBytes(writer, 32);
            }

            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < indexPairs.Count; i++)
                {
                    writer.Write(IOUtils.GetFlippedBytes(indexPairs[i].offset));
                    writer.Write(IOUtils.GetFlippedBytes(indexPairs[i].length));
                }
            }
            else
            {
                for (int i = 0; i < indexPairs.Count; i++)
                {
                    writer.Write(BitConverter.GetBytes(indexPairs[i].offset));
                    writer.Write(BitConverter.GetBytes(indexPairs[i].length));
                }
            }
        }


        /// <summary>
        /// Write the z coordinates in little endian.
        /// （以小端序寫入 Z 坐標至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box of the features.（圖徵的定界框。）</param>
        /// <param name="zArray">The list of z values.（Z 值的列表。）</param>
        private static void WriteZArrayLE(BinaryWriter writer, Bounds3 bounds, List<double> zArray)
        {
            writer.Write(BitConverter.GetBytes((double)bounds.z.min));
            writer.Write(BitConverter.GetBytes((double)bounds.z.max));
            for (int i = 0; i < zArray.Count; i++)
            {
                writer.Write(BitConverter.GetBytes(zArray[i]));
            }
        }
    }
}