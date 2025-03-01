using Carto.Geodata;
using Colossal.Logging;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Carto.IO
{
    /// <summary>
    /// The class that provides the interface to input / output files.
    /// （提供輸入／輸出檔案介面的類別。）<br/>
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        public static readonly Dictionary<System, HashSet<Property>> AvailablePropertyTable = new()
        {
            { System.Unknown, new() { } },
            { System.Area, new() { Property.Name, Property.Object, Property.Age, Property.Area, Property.Company, Property.Employee, Property.Household, Property.Labor, Property.Profit, Property.Resident, Property.SexRatio, Property.Unlocked, Property.Wage} },
            { System.Building, new() { Property.Name, Property.Object, Property.Address, Property.Age, Property.Asset, Property.Brand, Property.Category, Property.Elevation, Property.Employee, Property.Height, Property.Household, Property.Labor, Property.Level, Property.Product, Property.Profit, Property.Resident, Property.SexRatio, Property.Story, Property.Theme, Property.Value, Property.Wage, Property.Zoning } },
            { System.Net, new() { Property.Name, Property.Object, Property.Asset, Property.Capacity, Property.Category, Property.Direction, Property.Discharge, Property.Elevation, Property.Form, Property.Length, Property.Limit, Property.Load, Property.Volume, Property.Width } },
            { System.POI, new() { Property.Name, Property.Object, Property.Address, Property.Category} },
            { System.Route, new() { Property.Name, Property.Object, Property.Length, Property.Model, Property.Passenger, Property.Stop, Property.Transport, Property.Vehicle} },
            { System.Zoning, new() { Property.Name, Property.Object, Property.Color, Property.Density, Property.Theme, Property.Zoning} }
        };

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
            },
            {
                Property.Resident,
                new Dictionary<FileFormat, string[]>
                {
                    { FileFormat.Unknown, new string[2] { "Resident_Female", "Resident_Male" } },
                    { FileFormat.Shapefile, new string[2] { "Rsdt_fmle", "Rsdt_male" } }
                }
            }
        };

        /// <summary>
        /// The array sorted by each <see cref="Feature"/>'s display order.
        /// （根據每個 <see cref="Feature"/> 顯示順序排序的陣列。）
        /// </summary>
        public static readonly Feature[] FeatureDisplayOrder = new Feature[]
        {
            // Group A: Area（A 組：區域）
            Feature.District, Feature.MapTile, Feature.Extractor, Feature.Landfill, Feature.Surface,

            // Group B: Networks（B 組：網路）
            Feature.Runway, Feature.Taxiway, Feature.Road, Feature.Track, Feature.Pathway,
            Feature.Waterway, Feature.Cable, Feature.Pipe, Feature.Fence,

            // Group C: Buildings（C 組：建築）
            Feature.Building,

            // Group D: Routes（D 組：路線）
            Feature.RoutePassenger, Feature.RouteCargo,

            // Group E: POIs（E 組：興趣點）
            Feature.POITransport, Feature.POIPublic, Feature.POIPrivate, Feature.POIUtility,

            // Group F: Zonings（F 組：分區）
            Feature.Zoning,

            // Group G: Fallback value（G 組：後備值）
            Feature.None
        };

        public static readonly Dictionary<Ellipsoid, EllipsoidDefinition> EllipsoidTable = new()
        {
            { Ellipsoid.Clrk66, new(6378206.4, 294.978698213898) },
            { Ellipsoid.Evrst37, new(6377276.345, 300.8017) },
            { Ellipsoid.GRS80, new(Geodata.Ellipsoid.GRS80) },
            { Ellipsoid.Intl24, new(6378388, 297) },
            { Ellipsoid.WGS84, new(Geodata.Ellipsoid.WGS84) },
            { Ellipsoid.Custom, new() }
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
            { Property.Age, typeof(float) },
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
            { Property.Labor, typeof(int) },
            { Property.Length, typeof(float) },
            { Property.Level, typeof(int) },
            { Property.Limit, typeof(float) },
            { Property.Load, typeof(float) },
            { Property.Model, typeof(string) },
            { Property.Name, typeof(string) },
            { Property.Object, typeof(string) },
            { Property.Passenger, typeof(int) },
            { Property.Product, typeof(string) },
            { Property.Profit, typeof(float) },
            { Property.Resident, typeof(int) },
            { Property.SexRatio, typeof(float) },
            { Property.Stop, typeof(int) },
            { Property.Story, typeof(int) },
            { Property.Theme, typeof(string) },
            { Property.Transport, typeof(string) },
            { Property.Unlocked, typeof(bool) },
            { Property.Value, typeof(float) },
            { Property.Vehicle, typeof(int) },
            { Property.Volume, typeof(float) },
            { Property.Wage, typeof(float) },
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
            // Export options.（輸出設定。）
            Options option = new()
            {
                AssetPack = true,
                Display = new Dictionary<(Property, System), bool>
                {
                    { (Property.Category, System.Building), true },
                    { (Property.Category, System.Net), true },
                    { (Property.Category, System.POI), false },
                    { (Property.Object, System.Unknown), false },
                    { (Property.Zoning, System.Unknown), true }
                },
                Features = Feature.District | Feature.MapTile,
                FileFormat = FileFormat.GeoTIFF,
                FileName = "Raster",
                GeoTiffFormat = GeoTiffFormat.Norm16,
                Homeless = true,
                Minimized = true,
                Properties = new Dictionary<System, HashSet<Property>>
                {
                    //{ System.Area, new() { Property.Area, Property.Name, Property.Object, Property.Unlocked } },
                    //{ System.Area, new() { Property.Name, Property.Object, Property.Age, Property.Area, Property.Company, Property.Employee, Property.Household, Property.Labor, Property.Profit, Property.Resident, Property.SexRatio, Property.Unlocked, Property.Wage} },
                    //{ System.Building, new() { Property.Age, Property.Brand, Property.Theme, Property.Zoning } }
                },
                RasterKinds = RasterKind.Unknown,
                SeparateResident = false,
                SourceCoordinates = new Coord(new double3(327700, 2736000, 0)),
                SourceProjection = CRS.TransverseMercator,
                SourceProjectionDefinition = new ProjectionDefinition
                (
                    EllipsoidTable[Ellipsoid.GRS80],
                    (121, 0), (250000, 0), 0.9999, new double[0]
                ),
                StatisticsMapTile = false,
                Systems = System.Raster,
                TargetEllipsoid = Ellipsoid.WGS84,
                TargetProjection = CRS.TransverseMercator,
                TargetProjectionDefinition = new ProjectionDefinition
                (
                    EllipsoidTable[Ellipsoid.GRS80],
                    (121, 0), (250000, 0), 0.9999, new double[0]
                ),
                Taxable = false,
                VectorKinds = new Dictionary<System, VectorKind>
                {
                    { System.Area, VectorKind.Boundary }
                }
            };

            try
            {
                // Shorthanded variables to determine whther to run any system.（縮寫變數，用於決定是否執行任何系統。）
                bool useArea = option.Systems.HasFlag(System.Area);
                bool useBuilding = option.Systems.HasFlag(System.Building);
                bool useNet = option.Systems.HasFlag(System.Net);
                bool usePOI = option.Systems.HasFlag (System.POI);
                bool useRaster = option.Systems.HasFlag(System.Raster);
                bool useRoute = option.Systems.HasFlag(System.Route);
                bool useZoning = option.Systems.HasFlag(System.Zoning);

                // Collect shared data.（收集共享資料。）
                if (useArea || useBuilding)
                {
                    // Retrieve building statistics.（獲取建築的統計資料。）
                    Instance.Shared.GetBuildingStats(option);
                }
                else if (useZoning)
                {
                    // Retrieve zoning types information.（獲取分區類別的資訊。）
                    Instance.Shared.GetZoningTypes(option);
                }

                if (useZoning)
                {

                }
                if (useBuilding)
                {

                }
                if (useArea)
                {
                    GeoJson.Write(option, Instance.Area.WriteFeatures, OnReport);
                }
                if (useRaster)
                {
                    GeoTiff.Write(option, Instance.Raster.WriteElevation, OnReport);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Instance.Shared.Dispose();
            }
        }

        /// <summary>
        /// Retrieve the given <see cref="Property"/>'s type.
        /// （獲得給定 <see cref="Property"/> 的型別。）
        /// </summary>
        /// <param name="property">The property enumeration.（欄位枚舉。）</param>
        /// <param name="propertyName">The title of the field.（欄位的名稱。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <returns>The expected type of the <see cref="Property"/>.（<see cref="Property"/> 的預期型別。）</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Type GetPropertyType(Property property, string propertyName, Options options = null)
        {
            if (options != null)
            {
                if ((property == Property.Resident) && options.SeparateResident)
                {
                    return typeof(int[]);
                }
            }

            if (!PropertyTypeTable.TryGetValue(property, out Type expectedType))
            {
                throw new ArgumentException($"Unknown property `{propertyName}`. 未知的屬性 `{propertyName}`。");
            }

            return expectedType;
        }
    }
}