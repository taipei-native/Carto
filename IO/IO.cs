using Colossal.Logging;
using System;
using System.Collections.Generic;

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
            { Property.SexRatio, typeof(float) },
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
            // Export options.（輸出設定。）
            //Options option = new()
            //{
            //    Display = new Dictionary<(Property, System), bool>
            //    {
            //        { (Property.Category, System.Building), true },
            //        { (Property.Category, System.Net), true },
            //        { (Property.Category, System.POI), false },
            //        { (Property.Object, System.Unknown), false },
            //        { (Property.Zoning, System.Unknown), true }
            //    },
            //    Features = Feature.District | Feature.MapTile,
            //    FileFormat = FileFormat.GeoJSON,
            //    FileName = "Area",
            //    Minimized = true,
            //    Properties = new Dictionary<System, HashSet<Property>>
            //    {
            //        { System.Area, new HashSet<Property> { Property.Area, Property.Name, Property.Object, Property.Unlocked } }
            //    },
            //    RasterKinds = RasterKind.Unknown,
            //    SourceCoordinates = new Coord(new double3(302717, 2770282, 0)),
            //    SourceProjection = CRS.TransverseMercator,
            //    SourceProjectionDefinition = new ProjectionDefinition
            //    (
            //        new EllipsoidDefinition(Ellipsoid.GRS80),
            //        (121, 0), (250000, 0), 0.9999, new double[0]
            //    ),
            //    Systems = System.Area,
            //    VectorKinds = new Dictionary<System, VectorKind>
            //    {
            //        { System.Area, VectorKind.Boundary }
            //    }
            //};

            // Retrieve zoning types information.（獲取分區類別的資訊。）
            Instance.Shared.GetZoningTypes();

            try
            {
                if (Instance.Shared.ZoningTypes.IsCreated)
                {
                    for (int i = 0; i < Instance.Shared.ZoningTypes.Length; i++)
                    {
                        _log.Info(Instance.Shared.ZoningTypesNames[i].ToString());
                        _log.Info(Instance.Shared.ZoningTypes[i].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }

            //NativeQueue<BuildingStat> buildingQueue = Instance.Shared.GetBuildingStats(Allocator.TempJob);
            //while (buildingQueue.TryDequeue(out BuildingStat stat))
            //{
            //    _log.Info(stat.ToString());
            //}
            //buildingQueue.Dispose();

            //if (option.Systems.HasFlag(System.Area)) GeoJson.Write(option, Instance.Dummy.WriteFeatures, OnReport);
        }
    }
}