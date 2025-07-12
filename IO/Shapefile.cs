using Carto.Domain;
using Carto.Geodata;
using Carto.Utils;
using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
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
        /// The delegate of the WriteDBF() methods implemented in each system.
        /// （在各個系統實作的 WriteDBF() 方法的委派。）
        /// </summary>
        /// <typeparam name="T">The type of the sync targets.（同步對象的型別。）</typeparam>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="syncList">The reference of synchronization.（同步的參考。）</param>
        /// <param name="fieldMap">The map between the property and the field lengths.（屬性與欄位長度的映射表。）</param>
        public delegate void WriteDBFMethod<T>(BinaryWriter writer, Options options, HashSet<Property> validatedFields, List<T> syncList, out Dictionary<Property, FieldInfo> fieldMap);

        /// <summary>
        /// The delegate of the WriteSHP() methods implemented in each system.
        /// （在各個系統實作的 WriteSHP() 方法的委派。）
        /// </summary>
        /// <typeparam name="T">The type of the sync targets.（同步對象的型別。）</typeparam>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        /// <param name="syncList">The reference of synchronization.（同步的參考。）</param>
        public delegate void WriteSHPMethod<T>(BinaryWriter writer, Options options, out List<IndexPair> indexPairs, out Bounds3 bounds, out List<T> syncList);

        /// <summary>
        /// Apply the decimal length constraint of the field info.
        /// （對欄位資訊增加小數點後位數長度的限制。）
        /// </summary>
        /// <param name="property">The property enum value.（屬性枚舉值。）</param>
        /// <param name="constraint">The input field info.（輸入的欄位資訊。）</param>
        /// <returns>The field info with updated decimal length constraint.（附有更新後小數長度的欄位資訊。）</returns>
        public static FieldInfo ApplyFieldInfoConstraint(Property property, FieldInfo constraint)
        {
            if (IO.PropertyDecimalConstraintTable.TryGetValue(property, out int maxLength))
            {
                int newDecimalLength = constraint.decimalLength > maxLength ? maxLength : constraint.decimalLength;
                return new(newDecimalLength, constraint.length, constraint.scientific, constraint.type);
            }

            return constraint;
        }

        /// <summary>
        /// Combine the <see cref="FieldInfo"/> created by jobs and those created manually into a single dictionary.<br/>
        /// （將工作與手動產生的 <see cref="FieldInfo"/> 合併成單一字典。）
        /// </summary>
        /// <param name="validatedFields">The fields actually written into the .dbf file.（實際寫入 .dbf 檔案的欄位。）</param>
        /// <param name="propertyFieldMap">The native and multi map between property and the field information.（屬性與欄位資訊間的原生及複合映射表。）</param>
        /// <param name="fieldMap">The map between property and the field information.（屬性與欄位資訊間的映射表。）</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CombineFieldInfos(ref NativeParallelHashSet<EnumWrapper<Property>> validatedFields, ref NativeParallelMultiHashMap<EnumWrapper<Property>, FieldInfo> propertyFieldMap, Dictionary<Property, FieldInfo> fieldMap)
        {
            if (fieldMap == null) throw new ArgumentNullException("The fieldMap is not initiated. fieldMap 尚未被初始化。");
            NativeHashMap<EnumWrapper<Property>, FieldInfo> nativeFieldMap = new(validatedFields.Capacity, Allocator.Persistent);
            AggregateFieldInfoJob aggregateJob = new()
            {
                validatedFields = validatedFields,
                propertyFieldMap = propertyFieldMap,
                nativeFieldMap = nativeFieldMap
            };
            JobHandle aggregateHandle = aggregateJob.Schedule(default);
            aggregateHandle.Complete();

            NativeHashMap<EnumWrapper<Property>, FieldInfo>.Enumerator nativeFieldInfo = nativeFieldMap.GetEnumerator();
            while (nativeFieldInfo.MoveNext())
            {
                Property key = nativeFieldInfo.Current.Key;
                FieldInfo constraint = nativeFieldInfo.Current.Value;
                fieldMap.Add(key, ApplyFieldInfoConstraint(key, constraint));
            }

            CommonUtils.Dispose(ref nativeFieldMap);
        }

        /// <summary>
        /// Retrieve Burst compatible properties in the input hashset.
        /// （獲得輸入集合中可用於 Burst 的屬性。）
        /// </summary>
        /// <param name="managedSet">The input hashset.（輸入的集合。）</param>
        /// <param name="nativeSet">The native hashset to be written.（將被輸入的原生集合。）</param>
        public static void FilterBurstCompatibleProperties(HashSet<Property> managedSet, ref NativeParallelHashSet<EnumWrapper<Property>> nativeSet)
        {
            HashSet<Property>.Enumerator enumerator = managedSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!IO.BurstImcompatiblePropertyTable.Contains(enumerator.Current))
                {
                    nativeSet.Add(enumerator.Current);
                }
            }
        }

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
        /// Map each entity in <paramref name="syncList"/> to its index in the <paramref name="statList"/>.
        /// （將每個 <paramref name="statList"/> 內的實體映射至其在 <paramref name="statList"/> 的索引值。）
        /// </summary>
        /// <typeparam name="T">The statistical object type.（統計物件型別。）</typeparam>
        /// <param name="syncList">The list of entities.（實體的列表。）</param>
        /// <param name="statList">The list of statistical objects.（統計物件的列表。）</param>
        /// <param name="syncMap">The output map.（輸出的映射表。）</param>
        public static void SyncStatsToIndex<T>(List<Entity> syncList, ref NativeList<T> statList, ref NativeParallelHashMap<Entity, int> syncMap) where T : unmanaged, IStat
        {
            NativeArray<Entity> nativeSyncList = new(syncList.ToArray(), Allocator.TempJob);
            SyncStatsToIndexJob<T> syncJob = new()
            {
                syncList = nativeSyncList,
                statList = statList,
                syncMap = syncMap.AsParallelWriter()
            };
            JobHandle syncHandle = syncJob.Schedule(syncList.Count, 4, default);
            syncHandle.Complete();
            CommonUtils.Dispose(ref nativeSyncList);
        }

        /// <summary>
        /// Update the .dbf file header.
        /// （更新 .dbf 檔案的標頭。）
        /// </summary>
        /// <param name="fs">The current file stream.（目前的檔案資料流。）</param>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="fieldMap">The map between the property and the field lengths.（屬性與欄位長度的映射表。）</param>
        private static void UpdateDBFHeader(FileStream fs, BinaryWriter writer, Options options, HashSet<Property> validatedFields, Dictionary<Property, FieldInfo> fieldMap)
        {
            int currentPosition = 32;
            int recordLength = 0;
            fs.Seek(currentPosition, SeekOrigin.Begin);
            HashSet<Property>.Enumerator fieldEnumerator = validatedFields.GetEnumerator();
            while (fieldEnumerator.MoveNext())
            {
                Property property = fieldEnumerator.Current;
                if (!IO.IsCompositeProperty(property, options))
                {
                    if (fieldMap.TryGetValue(property, out FieldInfo fieldInfo))
                    {
                        UpdateFieldDescriptor(fs, writer, fieldInfo, ref currentPosition, ref recordLength);
                    }
                    continue;
                }
                else
                {
                    if ((property == Property.Address) && fieldMap.TryGetValue(property, out FieldInfo addressField))
                    {
                        UpdateFieldDescriptor(fs, writer, addressField, ref currentPosition, ref recordLength);
                        UpdateFieldDescriptor(fs, writer, addressField, ref currentPosition, ref recordLength);
                        UpdateFieldDescriptor(fs, writer, new(100000), ref currentPosition, ref recordLength);
                    }
                    if ((property == Property.Resident) && fieldMap.TryGetValue(property, out FieldInfo residentField))
                    {
                        UpdateFieldDescriptor(fs, writer, residentField, ref currentPosition, ref recordLength);
                        UpdateFieldDescriptor(fs, writer, residentField, ref currentPosition, ref recordLength);
                    }
                }
            }

            fs.Seek(10, SeekOrigin.Begin);
            byte[] recordLengthBytes = BitConverter.IsLittleEndian ? BitConverter.GetBytes((ushort) recordLength + 1) : IOUtils.GetFlippedBytes((ushort) recordLength + 1);
            writer.Write(recordLengthBytes);
        }

        /// <summary>
        /// Update the field descriptor in the .dbf file's header.
        /// （更新 .dbf 檔案標頭的欄位描述。）
        /// </summary>
        /// <param name="fs">The current file stream.（目前的檔案資料流。）</param>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="fieldInfo">The field's information.（欄位的資訊。）</param>
        /// <param name="currentPosition">The current position of the data stream.（目前資料流的位置。）</param>
        /// <param name="recordLength">The length of a record.（一個紀錄的長度。）</param>
        private static void UpdateFieldDescriptor(FileStream fs, BinaryWriter writer, FieldInfo fieldInfo, ref int currentPosition, ref int recordLength)
        {
            currentPosition += 16;
            fs.Seek(currentPosition, SeekOrigin.Begin);
            writer.Write(fieldInfo.length > byte.MaxValue ? byte.MaxValue : (byte)fieldInfo.length);
            writer.Write(fieldInfo.decimalLength > byte.MaxValue ? byte.MaxValue : (byte)fieldInfo.decimalLength);
            currentPosition += 16;
            recordLength += fieldInfo.length;
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
        /// Validate whether the field is a registered composite field or not.
        /// （驗證欄位是否為已註冊的複合欄位。）
        /// </summary>
        /// <param name="field">The input field.（輸入的欄位。）</param>
        /// <param name="fieldName">The field's title.（欄位的名稱。）</param>
        /// <param name="subFieldTitles">The composite field's titles.（複合欄位的名稱。）</param>
        /// <param name="fieldSymbols">The composite field's types.（複合欄位代表的型別。）</param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidateCompositeField(Property field, string fieldName, out string[] subFieldTitles, out char[] fieldSymbols)
        {
            if (!IO.CompositePropertyTable.TryGetValue(field, out Dictionary<FileFormat, string[]> subFieldTitleTable))
            {
                throw new KeyNotFoundException($"The property `{fieldName}` is not in CompositePropertyTable. 屬性 `{fieldName}` 未紀錄於 CompositePropertyTable。");
            }

            if (!subFieldTitleTable.TryGetValue(FileFormat.Shapefile, out subFieldTitles) &&
                !subFieldTitleTable.TryGetValue(FileFormat.Unknown, out subFieldTitles))
            {
                throw new KeyNotFoundException($"Fallback titles missing for property `{fieldName}` in CompositePropertyTable. CompositePropertyTable 未紀錄屬性 {fieldName} 的後備標題。");
            }

            if (!CompositeFieldTypeTable.TryGetValue(field, out fieldSymbols))
            {
                throw new KeyNotFoundException($"The property `{fieldName}` is not in CompositeFieldTypeTable. 屬性 `{fieldName}` 未紀錄於 CompositeFieldTypeTable。");
            }

            if (fieldSymbols.Length != subFieldTitles.Length)
            {
                throw new ArgumentException($"Array length mismatch: expected {subFieldTitles.Length}, but got {fieldSymbols.Length}. 陣列長度不符：預期為 {subFieldTitles.Length}，實際為 {fieldSymbols.Length}。");
            }
        }

        /// <summary>
        /// Write the ESRI Shapefile.
        /// （寫出 ESRI Shapefile。）
        /// </summary>
        /// <typeparam name="T">The type of the sync targets.（同步對象的型別。）</typeparam>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="systemName">The exporting system's name.（輸出系統的名稱。）</param>
        /// <param name="vectorKind">The classification of exported vector objects.（對輸出向量物體的分類。）</param>
        /// <param name="writeSHPMethod">The WriteSHP() method implemented in each system.（各系統實作的 WriteSHP() 方法。）</param>
        /// <param name="writeDBFMethod">The WriteDBF() method implemented in each system.（各系統實作的 WriteDBF() 方法。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public static void Write<T>(Options options, System systemName, VectorKind vectorKind, WriteSHPMethod<T> writeSHPMethod, WriteDBFMethod<T> writeDBFMethod, Action<string, int> onReportMethod)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (options == null) throw new ArgumentNullException("The parameters cannot be null. 參數不可為空值。");
            string filePath = options.GetFilePath(systemName, vectorKind);
            string cpgPath = Path.ChangeExtension(filePath, "cpg");
            string dbfPath = Path.ChangeExtension(filePath, "dbf");
            string prjPath = Path.ChangeExtension(filePath, "prj");
            string shxPath = Path.ChangeExtension(filePath, "shx");
            Bounds3 bounds;
            int shape = GetShapeType(vectorKind, options.Elevation);
            List<T> syncList;
            List<IndexPair> indexPairs;

            // The shapefile is a format composed by at least three sidecar files - which means multiple files have to be generated.
            // （Shapefile 是一個由至少三個檔案組成的檔案格式－這表示需要產生出複數個檔案。）

            // Unlike TIFF, which supports writing in both endian, Shapefile has strict rule when writing files.
            // （不同於 TIFF 支援以兩種端序寫入，Shapefile 對於端序有嚴格的要求。）

            using (FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using BinaryWriter writer = new(fs);
                WriteSHPHeader(writer, shape);
                writeSHPMethod.Invoke(writer, options, out indexPairs, out bounds, out syncList);
                if (indexPairs.Count > 0)
                {
                    IndexPair lastIndexPair = indexPairs[^1];
                    UpdateSHPHeader(fs, writer, bounds, lastIndexPair.offset + lastIndexPair.length + 4, options.Elevation);
                }
                else
                {
                    UpdateSHPHeader(fs, writer, bounds, 50, options.Elevation); // The empty header.（空白標頭。）
                }
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
                writeDBFMethod.Invoke(writer, options, validatedFields, syncList, out Dictionary<Property, FieldInfo> fieldMap);
                UpdateDBFHeader(fs, writer, options, validatedFields, fieldMap);
            }

            using (FileStream fs = new(cpgPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using BinaryWriter writer = new(fs);
                WriteCPG(writer);
            }

            using (FileStream fs = new(prjPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using StreamWriter writer = new(fs, new UTF8Encoding(false));
                WritePRJ(writer, options);
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
        /// Write the .cpg file.
        /// （寫入 .cpg 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        private static void WriteCPG(BinaryWriter writer)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(Encoding.UTF8.GetBytes("UTF-8"));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes("UTF-8"));
            }
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
                WriteFieldDescriptors(writer, options, subsetCopy.OrderBy(p => p, new PropertyComparer()).ToHashSet(), out validatedFields);
            }

            writer.Write((byte)13);

            int headerLength = IOUtils.GetPosition(writer);
            fs.Seek(8, SeekOrigin.Begin);

            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BitConverter.GetBytes((ushort)headerLength));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes((ushort)headerLength));
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
            IOUtils.SkipBytes(writer, 19);
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
                ValidateCompositeField(field, fieldName, out string[] subFieldTitles, out char[] fieldSymbols);

                for (int i = 0; i < subFieldTitles.Length; i++)
                {
                    WriteFieldDescriptor(writer, subFieldTitles[i], fieldSymbols[i]);
                }

                validatedFields.Add(field);
            }
        }

        /// <summary>
        /// Write the geometry to the file in big endian.
        /// （以大端序寫入幾何圖形至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="id">The unique identifier of the feature.（圖徵的獨特識別碼。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        /// <param name="geometry">The geometry of the feature.（圖徵的幾何圖形。）</param>
        /// <param name="indexPair">The index pair used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box of the feature.（圖徵的定界框。）</param>
        public static void WriteGeometryBE(BinaryWriter writer, int id, int shape, Geometry geometry, out IndexPair indexPair, out Bounds3 bounds)
        {
            int numParts = geometry.GetParts(out int numPoints, out List<int> pointCounts, out bounds);
            int offset = IOUtils.GetPosition(writer) / 2;
            int length = 0;

            writer.Write(BitConverter.GetBytes(id));                // Record number.（紀錄編號。）

            switch (shape)
            {
                case shapeTypePoint:
                    length = 10;
                    break;

                case shapeTypePolyLine:
                    length = 24 + 8 * numPoints;
                    break;

                case shapeTypePolygon:
                    length = 22 + 2 * numParts + 8 * (numPoints + numParts);
                    break;

                case shapeTypePointZ:
                    length = 18;
                    break;

                case shapeTypePolyLineZ:
                    length = 32 + 12 * numPoints;
                    break;

                case shapeTypePolygonZ:
                    length = 30 + 2 * numParts + 12 * (numPoints + numParts);
                    break;
            }

            indexPair = new(offset, length);
            writer.Write(BitConverter.GetBytes(length));            // Content length.（內容長度。）
            writer.Write(IOUtils.GetFlippedBytes(shape));           // Shape type.（幾何形狀。）

            switch (shape)
            {
                case shapeTypePoint:
                    WritePointBE(writer, geometry.Inclusions[0][0], false);
                    return;

                case shapeTypePointZ:
                    WritePointBE(writer, geometry.Inclusions[0][0], true);
                    return;
            }

            WriteBoxBE(writer, bounds);                                     // Box.（定界框。）
            writer.Write(IOUtils.GetFlippedBytes(numParts));                // NumParts.（部件的數量。）

            // NumPoints.（點的數量。）
            if ((shape == shapeTypePolyLine) || (shape == shapeTypePolyLineZ))
            {
                writer.Write(IOUtils.GetFlippedBytes(numPoints));
            }
            else
            {
                writer.Write(IOUtils.GetFlippedBytes(numPoints + numParts));
            }

            WritePointArraysBE(writer, geometry, pointCounts, (shape == shapeTypePolygon) || (shape == shapeTypePolygonZ), out List<double> zArray);

            if ((shape == shapeTypePolyLineZ) || (shape == shapeTypePolygonZ))
            {
                WriteZArrayBE(writer, bounds, zArray);
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
                    length = 10;
                    break;

                case shapeTypePolyLine:
                    length = 24 + 8 * numPoints;
                    break;

                case shapeTypePolygon:
                    length = 22 + 2 * numParts + 8 * (numPoints + numParts);
                    break;

                case shapeTypePointZ:
                    length = 18;
                    break;

                case shapeTypePolyLineZ:
                    length = 32 + 12 * numPoints;
                    break;

                case shapeTypePolygonZ:
                    length = 30 + 2 * numParts + 12 * (numPoints + numParts);
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
            
            WriteBoxLE(writer, bounds);                                 // Box.（定界框。）
            writer.Write(BitConverter.GetBytes(numParts));              // NumParts.（部件的數量。）

            // NumPoints.（點的數量。）
            if ((shape == shapeTypePolyLine) || (shape == shapeTypePolyLineZ))
            {
                writer.Write(BitConverter.GetBytes(numPoints));  
            }
            else
            {
                writer.Write(BitConverter.GetBytes(numPoints + numParts));
            }

            WritePointArraysLE(writer, geometry, pointCounts, (shape == shapeTypePolygon) || (shape == shapeTypePolygonZ), out List<double> zArray);

            if ((shape == shapeTypePolyLineZ) || (shape == shapeTypePolygonZ))
            {
                WriteZArrayLE(writer, bounds, zArray);
            }
        }

        /// <summary>
        /// Write the point array to the file in big endian.
        /// （以大端序寫入點陣列至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="geometry">The value waiting to be written.（等待被寫出的數值。）</param>
        /// <param name="pointCounts">The number of points in each part.（每個部件包含的點數。）</param>
        /// <param name="isRing">Whether the array represents a ring or not.（陣列是否為一個環？）</param>
        /// <param name="zArray">The array of z values.（Z值的陣列。）</param>
        private static void WritePointArraysBE(BinaryWriter writer, Geometry geometry, List<int> pointCounts, bool isRing, out List<double> zArray)
        {
            int pointIndex = 0;
            for (int i = 0; i < pointCounts.Count; i++)
            {
                writer.Write(IOUtils.GetFlippedBytes(pointIndex));
                pointIndex += pointCounts[i];
                if (isRing) pointIndex++;
            }

            zArray = new();
            for (int i = 0; i < geometry.Inclusions.Length; i++)
            {
                for (int j = 0; j < geometry.Inclusions[i].Length; j++)
                {
                    WritePointBE(writer, geometry.Inclusions[i][j], false);
                    zArray.Add(geometry.Inclusions[i][j].z);
                }
                if (isRing)
                {
                    WritePointBE(writer, geometry.Inclusions[i][0], false);
                    zArray.Add(geometry.Inclusions[i][0].z);
                }
            }

            for (int i = 0; i < geometry.Exclusions.Length; i++)
            {
                for (int j = 0; j < geometry.Exclusions[i].Length; j++)
                {
                    for (int k = 0; k < geometry.Exclusions[i][j].Length; k++)
                    {
                        WritePointBE(writer, geometry.Exclusions[i][j][k], false);
                        zArray.Add(geometry.Exclusions[i][j][k].z);
                    }
                    if (isRing)
                    {
                        WritePointBE(writer, geometry.Exclusions[i][j][0], false);
                        zArray.Add(geometry.Exclusions[i][j][0].z);
                    }
                }
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
        private static void WritePointBE(BinaryWriter writer, double3 value, bool writeElevation)
        {
            writer.Write(IOUtils.GetFlippedBytes(value.x));
            writer.Write(IOUtils.GetFlippedBytes(value.y));
            if (writeElevation)
            {
                writer.Write(IOUtils.GetFlippedBytes(value.z));
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
        private static void WritePointLE(BinaryWriter writer, double3 value, bool writeElevation)
        {
            writer.Write(BitConverter.GetBytes(value.x));
            writer.Write(BitConverter.GetBytes(value.y));
            if (writeElevation)
            {
                writer.Write(BitConverter.GetBytes(value.z));
                IOUtils.SkipBytes(writer, 8);
            }
        }

        /// <summary>
        /// Write the .prj file.
        /// （寫入 .prj 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        private static void WritePRJ(StreamWriter writer, Options options)
        {
            CultureInfo invariant = CultureInfo.InvariantCulture;
            string format = "0.0######";
            Coord center = Transform.Apply(options.SourceCoordinates, options.SourceProjection, options.TargetProjection, options.SourceProjectionDefinition, options.TargetProjectionDefinition);
            switch (options.TargetProjection)
            {
                case Geodata.CRS.TransverseMercator:
                    ProjectionDefinition projection = options.TargetProjectionDefinition;
                    EllipsoidDefinition ellipsoid = projection.ellipsoid;
                    writer.Write("PROJCS[\"User Defined Transverse Mercator\",GEOGCS[\"User Defined GCS\",DATUM[\"User Defined Datum\",SPHEROID[\"");
                    writer.Write(options.TargetEllipsoid == Ellipsoid.Custom ? "User Defined Ellipsoid" : Epsg.Ellipsoid.GetName(options.TargetEllipsoid));
                    writer.Write("\",");
                    writer.Write(ellipsoid.a.ToString(format, invariant));
                    writer.Write(",");
                    writer.Write(ellipsoid.rf.ToString(format, invariant));
                    writer.Write("],TOWGS84[");
                    
                    switch (projection.transform.paramCount)
                    {
                        case 3:
                            for (int i = 0; i < 3; i++)
                            {
                                writer.Write(projection.transform[i].ToString(invariant));
                                writer.Write(",");
                            }
                            writer.Write("0,0,0,0");
                            break;

                        case 7:
                            for (int i = 0; i < 7; i++)
                            {
                                writer.Write(projection.transform[i].ToString(invariant));
                                if (i != 6) writer.Write(",");
                            }
                            break;

                        default:
                            writer.Write("0,0,0,0,0,0,0");
                            break;
                    }

                    writer.Write($"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"{Epsg.Meridian.Greenwich}\"]],UNIT[\"Degree\",0.0174532925199433,");
                    writer.Write($"AUTHORITY[\"EPSG\",\"{Epsg.Uom.Degree}\"]],AUTHORITY[\"EPSG\",\"{Epsg.UserDefined}\"]],");
                    writer.Write("PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"Latitude_Of_Origin\",");
                    writer.Write(projection.origin.y.ToString(format, invariant));
                    writer.Write("],PARAMETER[\"Central_Meridian\",");
                    writer.Write(projection.origin.x.ToString(format, invariant));
                    writer.Write("],PARAMETER[\"Scale_Factor\",");
                    writer.Write(projection.scaleFactor.ToString(format, invariant));
                    writer.Write("],PARAMETER[\"False_Easting\",");
                    writer.Write(projection.shift.x.ToString(format, invariant));
                    writer.Write("],PARAMETER[\"False_Northing\",");
                    writer.Write(projection.shift.y.ToString(format, invariant));
                    writer.Write($"],UNIT[\"Metre\",1.0,AUTHORITY[\"EPSG\",\"{Epsg.Uom.Metre}\"]],");
                    writer.Write($"AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"{Epsg.UserDefined}\"]]");
                    break;

                case Geodata.CRS.UTM:
                    writer.Write("PROJCS[\"WGS_1984_UTM_Zone_");
                    writer.Write(center.UTMZone);
                    writer.Write(center.Hemisphere == Hemisphere.North ? "N" : "S");
                    writer.Write("\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",");
                    writer.Write("SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],");
                    writer.Write("UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],");
                    writer.Write("PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",");
                    writer.Write(center.Hemisphere == Hemisphere.North ? "0" : "10000000");
                    writer.Write(".0],PARAMETER[\"Central_Meridian\",");
                    writer.Write((center.UTMZone - 1) * 6 + 3 - 180);
                    writer.Write(".0],PARAMETER[\"Scale_Factor\",0.9996],");
                    writer.Write("PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]");
                    break;

                case Geodata.CRS.WGS84:
                    writer.Write("GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],");
                    writer.Write("PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]");
                    break;
            }
        }

        /// <summary>
        /// Write a boolean to the .dbf file.
        /// （寫入一個布林值至 .dbf 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="field">The field's constriant.（欄位限制。）</param>
        /// <param name="value">The input boolean.（輸入的布林值。）</param>
        public static void WriteRecord(BinaryWriter writer, FieldInfo field, bool value)
        {
            int valueInt = value ? 1 : 0;
            WriteRecordCore(writer, field.length, true, valueInt.ToString());
        }

        /// <summary>
        /// Write a floating-point number to the .dbf file.
        /// （寫入一個浮點數至 .dbf 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="field">The field's constriant.（欄位限制。）</param>
        /// <param name="value">The input floating-point number.（輸入的浮點數。）</param>
        public static void WriteRecord(BinaryWriter writer, FieldInfo field, float value)
        {
            string valueString;
            
            if (field.scientific)
            {
                int place = field.length - 5;
                string formatter = $"#.{new string('#', place)}e+00";
                valueString = value.ToString(formatter);

                while (Encoding.UTF8.GetByteCount(valueString) > field.length)
                {
                    place--;
                    formatter = $"#.{new string('#', place)}e+00";
                    valueString = value.ToString(formatter);
                }
            }
            else
            {
                string formatter = $"0.{new string('#', field.decimalLength)}";
                valueString = value.ToString(formatter);
            }

            WriteRecordCore(writer, field.length, true, valueString);
        }

        /// <summary>
        /// Write an integer to the .dbf file.
        /// （寫入一個整數至 .dbf 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="field">The field's constriant.（欄位限制。）</param>
        /// <param name="value">The input integer.（輸入的整數。）</param>
        public static void WriteRecord(BinaryWriter writer, FieldInfo field, int value)
        {
            WriteRecordCore(writer, field.length, true, value.ToString());
        }

        /// <summary>
        /// Write a string to the .dbf file.
        /// （寫入一個字串至 .dbf 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="field">The field's constriant.（欄位長度限制。）</param>
        /// <param name="value">The input string.（輸入的字串。）</param>
        public static void WriteRecord(BinaryWriter writer, FieldInfo field, string value)
        {
            while (Encoding.UTF8.GetByteCount(value) > field.length && value.Length > 0)
            {
                value = value.Substring(0, value.Length - 1);
            }

            WriteRecordCore(writer, field.length, false, value);
        }

        /// <summary>
        /// The core method to write a field.
        /// （用於寫入欄位的核心方法。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="fieldLength">The field's length constriant.（欄位長度限制。）</param>
        /// <param name="alignRight">Whether to align the field to the right.（是否要將欄位向右對齊？）</param>
        /// <param name="value">The input string.（輸入的字串。）</param>
        private static void WriteRecordCore(BinaryWriter writer, int fieldLength, bool alignRight, string value)
        {
            if (Encoding.UTF8.GetByteCount(value) < fieldLength)
            {
                string padding = new(' ', fieldLength - Encoding.UTF8.GetByteCount(value));
                string padded = alignRight ? padding + value : value + padding;

                if (BitConverter.IsLittleEndian)
                {
                    writer.Write(Encoding.UTF8.GetBytes(padded));
                }
                else
                {
                    writer.Write(IOUtils.GetFlippedBytes(padded));
                }
            }
            else
            {
                if (BitConverter.IsLittleEndian)
                {
                    writer.Write(Encoding.UTF8.GetBytes(value));
                }
                else
                {
                    writer.Write(IOUtils.GetFlippedBytes(value));
                }
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
        /// Write the z coordinates in big endian.
        /// （以大端序寫入 Z 坐標至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box of the features.（圖徵的定界框。）</param>
        /// <param name="zArray">The list of z values.（Z 值的列表。）</param>
        private static void WriteZArrayBE(BinaryWriter writer, Bounds3 bounds, List<double> zArray)
        {
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.min));
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.max));
            for (int i = 0; i < zArray.Count; i++)
            {
                writer.Write(IOUtils.GetFlippedBytes(zArray[i]));
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

        /// <summary>
        /// Aggregate multiple <see cref="FieldInfo"/> to a single one for each <see cref="Property"/>.<br/>
        /// （對於每個 <see cref="Property"/>，將數個 <see cref="FieldInfo"/> 聚合成一個。）
        /// </summary>
        [BurstCompile]
        public partial struct AggregateFieldInfoJob : IJob
        {
            [ReadOnly]
            public NativeParallelHashSet<EnumWrapper<Property>> validatedFields;

            [ReadOnly]
            public NativeParallelMultiHashMap<EnumWrapper<Property>, FieldInfo> propertyFieldMap;

            [WriteOnly]
            public NativeHashMap<EnumWrapper<Property>, FieldInfo> nativeFieldMap;

            public void Execute()
            {
                NativeParallelHashSet<EnumWrapper<Property>>.Enumerator field = validatedFields.GetEnumerator();
                while (field.MoveNext())
                {
                    if (propertyFieldMap.TryGetFirstValue(field.Current, out FieldInfo fieldInfo, out NativeParallelMultiHashMapIterator<EnumWrapper<Property>> it))
                    {
                        FieldInfo internalFieldInfo = fieldInfo;
                        
                        while (propertyFieldMap.TryGetNextValue(out fieldInfo, ref it))
                        {
                            internalFieldInfo += fieldInfo;
                        }

                        nativeFieldMap.Add(field.Current, internalFieldInfo);
                    }
                }
            }
        }

        /// <summary>
        /// Map each entity in <see cref="syncList"/> to its index in the <see cref="statList"/>.
        /// （將每個 <see cref="syncList"/> 內的實體映射至其在 <see cref="statList"/> 的索引值。）
        /// </summary>
        /// <typeparam name="T">The statistical object type.（統計物件型別。）</typeparam>
        [BurstCompile]
        public partial struct SyncStatsToIndexJob<T> : IJobParallelFor
            where T : unmanaged, IStat
        {
            [ReadOnly]
            public NativeArray<Entity> syncList;

            [ReadOnly]
            public NativeList<T> statList;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int>.ParallelWriter syncMap;

            public void Execute(int index)
            {
                Entity entity = syncList[index];
                for (int i = 0; i < statList.Length; i++)
                {
                    if (statList[i].Entity == entity)
                    {
                        syncMap.TryAdd(entity, i);
                        return;
                    }
                }
            }
        }
    }
}