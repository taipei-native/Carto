using Carto.Systems;
using Colossal.IO.AssetDatabase.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Carto.IO
{
    /// <summary>
    /// The class that provides the interface to input / output files.
    /// （提供輸入／輸出檔案介面的類別。）<br/>
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// The look-up table of composite property's sub-field name.
        /// （每個複合屬性的子欄位名稱對照表。）
        /// </summary>
        public static readonly Dictionary<Property, Dictionary<FileFormat, string[]>> CompositePropertyTable = new()
        {
            {
                Property.Address,
                new Dictionary<FileFormat, string[]>
                {
                    { FileFormat.Unknown, new string[3] { "Address_District", "Address_Street", "Address_Number" } },
                    { FileFormat.Shapefile, new string[3] { "Addr_dist", "Addr_strt", "Addr_nmbr" } }
                }
            }
        };

        /// <summary>
        /// The look-up table of each file format's extension.
        /// （每個檔案格式副檔名的對照表。）
        /// </summary>
        public static readonly Dictionary<FileFormat, string> FileExtensionTable = new()
        {
            { FileFormat.GeoJSON, "json" },
            { FileFormat.GeoPackage, "gpkg" },
            { FileFormat.GeoTIFF, "tif" },
            { FileFormat.Shapefile, "shp" }
        };

        /// <summary>
        /// The look-up table of each <see cref="Property"/>'s corresponding type.<br/>
        /// （每個 <see cref="Property"/> 的對應型別表。）
        /// </summary>
        public static readonly Dictionary<Property, Type> PropertyTypeTable = new()
        {
            { Property.Address, typeof(object[]) },
            { Property.Area, typeof(float) },
            { Property.Asset, typeof(string) },
            { Property.Brand, typeof(string) },
            { Property.Capacity, typeof(int) },
            { Property.Category, typeof(string) },
            { Property.Color, typeof(string) },
            { Property.Company, typeof(int) },
            { Property.Density, typeof(string) },
            { Property.Direction, typeof(string) },
            { Property.Discharge, typeof(float) },
            { Property.Elevation, typeof(float) },
            { Property.Employee, typeof(int) },
            { Property.Form, typeof(string) },
            { Property.Height, typeof(float) },
            { Property.Household, typeof(int) },
            { Property.Length, typeof(float) },
            { Property.Level, typeof(int) },
            { Property.Limit, typeof(float) },
            { Property.Load, typeof(float) },
            { Property.Model, typeof(string) },
            { Property.Name, typeof(string) },
            { Property.Object, typeof(string) },
            { Property.Passenger, typeof(int) },
            { Property.Product, typeof(string) },
            { Property.Resident, typeof(int) },
            { Property.Stop, typeof(int) },
            { Property.Story, typeof(int) },
            { Property.Theme, typeof(string) },
            { Property.Transport, typeof(string) },
            { Property.Unlocked, typeof(bool) },
            { Property.Value, typeof(float) },
            { Property.Vehicle, typeof(int) },
            { Property.Volume, typeof(float) },
            { Property.Wealth, typeof(string) },
            { Property.Width, typeof(float) },
            { Property.Zoning, typeof(string) }
        };
        
        /// <summary>
        /// Export in-game objects into geospatial files.
        /// （將遊戲內物體輸出為地理空間格式檔案。）
        /// </summary>
        public static void Export(Options options)
        {
            
        }

        /// <summary>
        /// Export 
        /// </summary>
        /// <param name="options"></param>
        public static void WriteGeoJSONFile(Options options)
        {
            using (StreamWriter sw = new(options.FilePath))
            using (JsonTextWriter writer = new(sw))
            {
                writer.WriteStartObject();
                GeoJson.WritePropertyPair(writer, "type", "FeatureCollection");
                writer.WritePropertyName("crs");
                writer.WriteStartObject();
                    GeoJson.WritePropertyPair(writer, "type", "name");
                    writer.WritePropertyName("properties");
                    writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "name", "urn:ogc:def:crs:OGC:1.3:CRS84");
                    writer.WriteEndObject();
                writer.WriteEndObject();
                writer.WritePropertyName("features");
                writer.WriteStartArray();
                Instance.Dummy.WriteFeatures(writer, options); // Writes many { "type": "Feature", "properties": {...}, "geometry": {...} }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
        }
    }
}