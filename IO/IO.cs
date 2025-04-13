using Carto.Domain;
using Carto.Geodata;
using Colossal.Logging;
using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// The list of available properties for each system.
        /// （各系統可用的屬性列表。）
        /// </summary>
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
        /// The properties that are imcompatible with Burst compile.
        /// （與 Burst 編譯不合的屬性集合。）
        /// </summary>
        public static readonly HashSet<Property> BurstImcompatiblePropertyTable = new()
        {
            Property.Asset, Property.Brand, Property.Category, Property.Color, Property.Density, Property.Direction, Property.Form, Property.Model, Property.Name, Property.Object,
            Property.Product, Property.Theme, Property.Transport, Property.Zoning
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

        /// <summary>
        /// The table between the property and the custom maximum decimal length.
        /// （屬性與客製化的最長小數點位數對照表。）
        /// </summary>
        public static readonly Dictionary<Property, int> PropertyDecimalConstraintTable = new()
        {
            { Property.Age, 1 },
            { Property.Area, 2 },
            { Property.Discharge, 2 },
            { Property.Elevation, 4 },
            { Property.Height, 4 },
            { Property.Length, 4 },
            { Property.Limit, 4 },
            { Property.Load, 2 },
            { Property.Profit, 2 },
            { Property.SexRatio, 4 },
            { Property.Value, 2 },
            { Property.Wage, 2 },
            { Property.Width, 4 }
        };

        /// <summary>
        /// The array sorted by each <see cref="ZoningCategory"/>'s display order.
        /// （根據每個 <see cref="ZoningCategory"/> 顯示順序排序的陣列。）
        /// </summary>
        public static readonly ZoningCategory[] ZoningDisplayOrder = new ZoningCategory[]
        {
            ZoningCategory.Commercial, ZoningCategory.Office, ZoningCategory.Industrial, ZoningCategory.Residential, ZoningCategory.None
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
            Options options = Instance.Settings.GetOptions();
            options.Initialize();

            try
            {
                // Shorthanded variables to determine whther to run any system.（縮寫變數，用於決定是否執行任何系統。）
                bool useArea = options.Systems.HasFlag(System.Area);
                bool useBuilding = options.Systems.HasFlag(System.Building);
                bool useNet = options.Systems.HasFlag(System.Net);
                bool usePOI = options.Systems.HasFlag (System.POI);
                bool useRaster = options.Systems.HasFlag(System.Raster);
                bool useRoute = options.Systems.HasFlag(System.Route);
                bool useZoning = options.Systems.HasFlag(System.Zoning);
                bool useVector = useArea || useBuilding || useNet || usePOI || useRoute || useZoning;

                if (useVector)
                {
                    // Collect vector shared data.（收集向量共享資料。）
                    if (useArea || useBuilding)
                    {
                        // Retrieve building statistics.（獲取建築的統計資料。）
                        Instance.Shared.GetBuildingStats(options);
                    }
                    else if (useZoning)
                    {
                        // Retrieve zoning types information.（獲取分區類別的資訊。）
                        Instance.Shared.GetZoningTypes(options);
                    }

                    bool areaHasBoundary = options.Has(System.Area, VectorKind.Boundary);
                    bool zoningHasBoundary = options.Has(System.Zoning, VectorKind.Boundary);

                    // Write vector data.（寫入向量資料。）
                    switch (options.VectorFormat)
                    {
                        case FileFormat.GeoJSON:
                            if (useZoning)
                            {
                                if (zoningHasBoundary)
                                {
                                    GeoJson.Write(options, System.Zoning, VectorKind.Boundary, Instance.Zoning.WriteBoundaryFeatures, OnReport);
                                }
                            }
                            if (useBuilding)
                            {

                            }
                            if (useArea)
                            {
                                if (areaHasBoundary)
                                {
                                    GeoJson.Write(options, System.Area, VectorKind.Boundary, Instance.Area.WriteBoundaryFeatures, OnReport);
                                }
                            }
                            break;

                        case FileFormat.Shapefile:
                            if (useZoning)
                            {

                            }
                            if (useBuilding)
                            {

                            }
                            if (useArea)
                            {
                                if (areaHasBoundary)
                                {
                                    Shapefile.Write(options, System.Area, VectorKind.Boundary, Instance.Area.WriteBoundarySHP, Instance.Area.WriteBoundaryDBF, OnReport);
                                }
                            }
                            break;
                    }
                }

                if (useRaster)
                {
                    bool hasWorldDepth = options.RasterKinds.HasFlag(RasterKind.WorldDepth);
                    bool hasWorldElevation = options.RasterKinds.HasFlag(RasterKind.WorldElevation);
                    bool hasWorldTerrain = hasWorldDepth || hasWorldElevation;

                    // Write raster data.（寫入網格資料。）
                    switch (options.RasterFormat)
                    {
                        case FileFormat.GeoTIFF:
                            // Handle the grids using shared data first.（首先處理使用共享資料的網格。）
                            if (hasWorldTerrain)
                            {
                                Instance.Shared.GetWorldElevation(options);

                                if (hasWorldDepth)
                                {
                                    GeoTiff.Write(options, RasterKind.WorldDepth, Instance.Raster.WriteWorldDepth, OnReport);
                                }
                                if (hasWorldElevation)
                                {
                                    GeoTiff.Write(options, RasterKind.WorldElevation, Instance.Raster.WriteWorldElevation, OnReport);
                                }

                                Instance.Shared.Dispose(DisposePhase.AfterTerrainRelated);
                            }

                            // ... then handle the grids using data independent from others later.（接著處理獨立的網格。）
                            if (options.RasterKinds.HasFlag(RasterKind.Depth))
                            {
                                GeoTiff.Write(options, RasterKind.Depth, Instance.Raster.WriteDepth, OnReport);
                            }
                            if (options.RasterKinds.HasFlag(RasterKind.Elevation))
                            {
                                GeoTiff.Write(options, RasterKind.Elevation, Instance.Raster.WriteElevation, OnReport);
                            }
                            break;
                    }
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

        /// <summary>
        /// Check whether the property is a composite property or not.
        /// （確認屬性是否是一個複合屬性。）
        /// </summary>
        /// <param name="property">The property enumeration.（欄位枚舉。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <returns>True if the property is a composite property.（若屬性是複合屬性，回傳真值。）</returns>
        public static bool IsCompositeProperty(Property property, Options options = null) => GetPropertyType(property, property.ToString(), options).IsArray;
    }
}