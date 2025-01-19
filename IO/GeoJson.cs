using Carto.Geodata;
using Carto.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Unity.Mathematics;

namespace Carto.IO
{
    /// <summary>
    /// The class that provides utility functions to write GeoJSON.
    /// （提供寫出 GeoJSON 功能的類別。）<br/>
    /// </summary>
    public static class GeoJson
    {
        /// <summary>
        /// Write the GeoJSON file.
        /// （寫出 GeoJSON 檔案。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="writeFeaturesMethod">The static WriteFeatures() method implemented in each system.（各系統實作的靜態 WriteFeatures() 方法。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public static void Write(Options options, Action<JsonTextWriter, Options, Action<string, int>> writeFeaturesMethod, Action<string, int> onReportMethod)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if ((options == null) || (writeFeaturesMethod == null)) throw new ArgumentNullException("The parameters cannot be null. 參數不可為空值。");
            string filePath = options.FilePath;

            using StreamWriter sw = new(filePath, false, Encoding.UTF8);
            using JsonTextWriter writer = new(sw);

            // Set formatting.（設定格式。）
            writer.Formatting = options.Minimized ? Formatting.None : Formatting.Indented;
            writer.Indentation = options.Minimized ? 0 : 2;

            writer.WriteStartObject();

            // Type definition.（型別定義。）
            WritePropertyPair(writer, "type", "FeatureCollection");

            // CRS definition.（坐標參考系統定義。）
            writer.WritePropertyName("crs");
            writer.WriteStartObject();
            WritePropertyPair(writer, "type", "name");
            writer.WritePropertyName("properties");
            writer.WriteStartObject();
            WritePropertyPair(writer, "name", "urn:ogc:def:crs:OGC:1.3:CRS84");
            writer.WriteEndObject();
            writer.WriteEndObject();

            // Feature definition.（圖徵定義。）
            writer.WritePropertyName("features");
            writer.WriteStartArray();
            writeFeaturesMethod(writer, options, onReportMethod);
            writer.WriteEndArray();

            writer.WriteEndObject();
            Instance.Log.Debug($"Write '{Path.GetFileName(filePath)}' in {CommonUtils.FormatTimeSpan(stopwatch.Elapsed)}.");
        }
        
        /// <summary>
        /// Write <see cref="float3"/> to the file.
        /// （寫出 <see cref="float3"/> 至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The value waiting to be written.（等待被寫出的數值。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        public static void WriteFloat3(JsonTextWriter writer, float3 value, bool writeElevation = false)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            if (writeElevation) writer.WriteValue(value.z);
            writer.WriteEndArray();
        }

        /// <summary>
        /// Write <see cref="float3"/> array to the file.
        /// （寫出 <see cref="float3"/> 陣列至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="array">The value waiting to be written.（等待被寫出的數值。）</param>
        /// <param name="isRing">Whether the array represents a ring or not.（陣列是否為一個環？）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        public static void WriteFloat3Array(JsonTextWriter writer, float3[] array, bool isRing, bool writeElevation = false)
        {
            writer.WriteStartArray();
            for (int i = 0; i < array.Length; i++) WriteFloat3(writer, array[i]);
            if (isRing) WriteFloat3(writer, array[0], writeElevation);
            writer.WriteEndArray();
        }

        /// <summary>
        /// Write <see cref="Geometry"/> to the file.
        /// （寫出 <see cref="Geometry"/> 至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        /// <param name="geometry">The geometry of the feature.（圖徵的幾何圖形。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        public static void WriteGeometry(JsonTextWriter writer, Geometry geometry, Shape shape = Shape.Point, bool writeElevation = false)
        {
            writer.WriteStartObject();

            // Write geometry type.（寫出幾何圖形型別。）
            writer.WritePropertyName("type");
            writer.WriteValue(Enum.GetName(typeof(Shape), shape));

            // Write coordinates.（寫出座標。）
            writer.WritePropertyName("coordinates");

            switch (shape)
            {
                case Shape.Point:
                    WriteFloat3(writer, geometry.Inclusions[0][0], writeElevation);
                    break;

                case Shape.LineString:
                    WriteFloat3Array(writer, geometry.Inclusions[0], false, writeElevation);
                    break;

                case Shape.Polygon:
                    WritePolygon(writer, geometry, 0, writeElevation);
                    break;

                case Shape.MultiPolygon:
                    writer.WriteStartArray();
                    for (int i = 0; i < geometry.Inclusions.Length; i++) WritePolygon(writer, geometry, i, writeElevation);
                    writer.WriteEndArray();
                    break;

                default:
                    throw new ArgumentException("Only points, line strings and (multi-) polygons can be exported as GeoJSON. 只有點、線段和（複合）多邊形可以被輸出為 GeoJSON。");
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Write the designated polygon to the file.
        /// （寫出指定多邊形至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="polygonIndex">The designated polygon's index in <see cref="Geometry.Inclusions"/>.<br/>（指定多邊形在 <see cref="Geometry.Inclusions"/> 的索引。）</param>
        /// <param name="geometry">The geometry of the feature.（圖徵的幾何圖形。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        private static void WritePolygon(JsonTextWriter writer, Geometry geometry, int polygonIndex, bool writeElevation = false)
        {
            writer.WriteStartArray();

            // Write the exterior ring.（寫出外環。）
            WriteFloat3Array(writer, geometry.Inclusions[polygonIndex], true, writeElevation);

            // Write the interior rings, if any exists.（若內環存在，將其寫出。）
            if (geometry.ExclusionIndexTable.TryGetValue(polygonIndex, out int exclusionIndex))
            {
                for (int j = 0; j < geometry.Exclusions[exclusionIndex].Length; j++)
                    WriteFloat3Array(writer, geometry.Exclusions[exclusionIndex][j], true, writeElevation);
            }

            writer.WriteEndArray();
        }

        /// <summary>
        /// Write <see cref="Property"/> and its value to the file.
        /// （寫出 <see cref="Property"/> 與其數值至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="property">The property's enumeration.（屬性的枚舉。）</param>
        /// <param name="value">The value of the property.（屬性的數值。）</param>
        public static void WriteProperty(JsonTextWriter writer, Property property, object value)
        {
            string propertyName = Enum.GetName(typeof(Property), property);

            // Validate property registration.（檢驗屬性是否已被註冊。）
            if (!IO.PropertyTypeTable.TryGetValue(property, out Type expectedType))
            {
                throw new ArgumentException($"Unknown property `{propertyName}`. 未知的屬性 `{propertyName}`。");
            }

            // Validate value's type.（檢驗數值的型別。）
            Type actualType = value?.GetType() ?? throw new ArgumentNullException(nameof(value), "The value cannot be null. 數值不可為空值。");
            if (expectedType != actualType)
            {
                throw new ArgumentException($"Type mismatch: expected `{expectedType.Name}`, but got `{actualType.Name}`. 型別不符：預期 `{expectedType.Name}`，實際為 `{actualType.Name}`。");
            }

            // Write non-composite fields.（寫入非複合欄位。）
            if (!expectedType.IsArray)
            {
                WritePropertyPair(writer, propertyName, value);
                return;
            }

            // Write composite fields.（寫入複合欄位。）

            // Validate sub-fields registration.（檢驗子欄位是否被註冊。）
            if (!IO.CompositePropertyTable.TryGetValue(property, out Dictionary<FileFormat, string[]> subFieldTitleTable))
            {
                throw new KeyNotFoundException($"The property `{propertyName}` is not in CompositePropertyTable. 屬性 `{propertyName}` 未紀錄於 CompositePropertyTable。");
            }

            if (!subFieldTitleTable.TryGetValue(FileFormat.GeoJSON, out string[] subFieldTitles) &&
                !subFieldTitleTable.TryGetValue(FileFormat.Unknown, out subFieldTitles))
            {
                throw new KeyNotFoundException($"Fallback titles missing for property `{propertyName}` in CompositePropertyTable. CompositePropertyTable 未紀錄屬性 {propertyName} 的後備標題。");
            }

            // Validate value's length against subFieldTitles' length.（檢驗數值的長度是否與 subFieldTitles 相同。）
            Array array = value as Array ?? throw new ArgumentException("Expected an array but received a non-array value. 數值預期為一個陣列，但實際非陣列。");
            if (array.Length != subFieldTitles.Length)
            {
                throw new ArgumentException($"Array length mismatch: expected {subFieldTitles.Length}, but got {array.Length}. 陣列長度不符：預期為 {subFieldTitles.Length}，實際為 {array.Length}。");
            }

            for (int i = 0; i < subFieldTitles.Length; i++)
            {
                WritePropertyPair(writer, subFieldTitles[i], array.GetValue(i));
            }
        }

        /// <summary>
        /// A helper function to write the property name and its value simultaneously.
        /// （同時寫入屬性名稱與其值的輔助函數。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="propertyName">The title of the property.（屬性的標題。）</param>
        /// <param name="value">The content of the property.（屬性的內容。）</param>
        public static void WritePropertyPair(JsonTextWriter writer, string propertyName, object value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value);
        }
    }
}