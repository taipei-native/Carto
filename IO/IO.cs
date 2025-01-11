using Carto.Geodata;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

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
        
        public static void OnReport(string file, int progress)
        {

        }

        /// <summary>
        /// Export in-game objects into geospatial files.
        /// （將遊戲內物體輸出為地理空間格式檔案。）
        /// </summary>
        public static void Export()
        {
            Options option = new()
            {
                FileFormat = FileFormat.GeoJSON,
                FileName = "Map Tile",
                SourceCoordinates = new Coord(new double3(302717, 2770282, 0)),
                SourceProjection = CRS.TransverseMercator,
                SourceProjectionDefinition = new ProjectionDefinition
                (
                    new EllipsoidDefinition(Ellipsoid.GRS80),
                    (121, 0), (250000, 0), 0.9999, new double[0]
                )
            };
            
            GeoJson.Write(option, Instance.Dummy.WriteFeatures, OnReport);
        }
    }
}