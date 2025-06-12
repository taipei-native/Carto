using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Settings;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;

namespace Carto
{
    /// <summary>
    /// The class that manages the mod's options.
    /// （管理模組設定的類別。）
    /// </summary>
    [SettingsUIGroupOrder(FeatureVectorGroup, FeatureRasterGroup, ProjectionBasicGroup, ProjectionEllipsoidGroup, ProjectionProjectionGroup, ProjectionUTMGroup)]
    [SettingsUIShowGroupName(FeatureVectorGroup, FeatureRasterGroup, ProjectionBasicGroup, ProjectionEllipsoidGroup, ProjectionProjectionGroup, ProjectionUTMGroup)]
    [SettingsUITabOrder(GeneralTab, FeatureTab, ProjectionTab)]
    [FileLocation("ModsSettings/" + nameof(Carto) + "/" + nameof(Carto) + "_v1")]
    public class Settings : ModSetting
    {
        public Settings(IMod mod) : base(mod) { SetDefaults(); }

        /// <summary>
        /// Reset all mod default settings.
        /// （重置所有模組設定。）
        /// </summary>
        public override void SetDefaults() { }

        public const string GeneralTab = "GeneralTab";
        public const string GeneralGeneralGroup = "GeneralGeneralGroup";
        public const string FeatureTab = "FeatureTab";
        public const string FeatureVectorGroup = "FeatureVectorGroup";
        public const string FeatureRasterGroup = "FeatureRasterGroup";
        public const string ProjectionTab = "ProjectionTab";
        public const string ProjectionBasicGroup = "ProjectionBasicGroup";
        public const string ProjectionEllipsoidGroup = "ProjectionEllipsoidGroup";
        public const string ProjectionProjectionGroup = "ProjectionProjectionGroup";
        public const string ProjectionUTMGroup = "ProjectionUTMGroup";

        /// <summary>
        /// The file format of the vector files.
        /// （向量檔案的格式。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        [SettingsUIDropdown(typeof(Settings), nameof(GetVectorFormats))]
        public int ExportVectorFormat { get; set; } = 2;

        public DropdownItem<int>[] GetVectorFormats()
        {
            return new DropdownItem<int>[]
            {
                new()
                {
                    value = 0,
                    displayName = "Options.Carto.Carto.Mod.FILEFORMAT[GeoJSON]"
                },
                // TODO: Uncommet when the GeoPackage the export function is implemented.
                /*new()
                {
                    value = 1,
                    displayName = "Options.Carto.Carto.Mod.FILEFORMAT[GeoPackage]"
                },*/
                new()
                {
                    value = 2,
                    displayName = "Options.Carto.Carto.Mod.FILEFORMAT[Shapefile]"
                },
            };
        }

        /// <summary>
        /// The format of the GeoTIFF files.
        /// （GeoTIFF 檔案的格式。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        public IO.GeoTiffFormat ExportGeoTiffFormat { get; set; } = IO.GeoTiffFormat.Int16;

        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        [SettingsUIButton]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsInGameOrEditor), invert: true)]
        public bool ExportButton
        {
            set { IO.IO.Export(); }
        }

        /// <summary>
        /// Whether to export area features.
        /// （是否輸出區域圖徵。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        public bool SystemArea { get; set; } = true;

        /// <summary>
        /// Whether to export districts.
        /// （是否輸出行政區。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemArea), invert: true)]
        public bool FeatureDistrict { get; set; } = true;

        /// <summary>
        /// Whether to export map tiles.
        /// （是否輸出地圖區塊。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemArea), invert: true)]
        public bool FeatureMapTile { get; set; } = true;

        /// <summary>
        /// Whether to export building features.
        /// （是否輸出建築圖徵。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        public bool SystemBuilding { get; set; } = true;

        /// <summary>
        /// Whether to export buildings.
        /// （是否輸出建築。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemBuilding), invert: true)]
        public bool FeatureBuilding { get; set; } = true;

        /// <summary>
        /// Whether to export landfills.
        /// （是否輸出垃圾掩埋場。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemBuilding), invert: true)]
        public bool FeatureLandfill { get; set; } = true;

        /// <summary>
        /// Whether to export specialized industries.
        /// （是否輸出專精工業。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemBuilding), invert: true)]
        public bool FeatureExtractor { get; set; } = true;

        /// <summary>
        /// Whether to export network features.
        /// （是否輸出網路圖徵。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        public bool SystemNetwork { get; set; } = true;

        /// <summary>
        /// Whether to export paths.
        /// （是否輸出人行通道。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemNetwork), invert: true)]
        public bool FeaturePathway { get; set; } = true;

        /// <summary>
        /// Whether to export roads.
        /// （是否輸出道路。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemNetwork), invert: true)]
        public bool FeatureRoad { get; set; } = true;

        /// <summary>
        /// Whether to export runways & taxiways.
        /// （是否輸出跑道與滑行道。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemNetwork), invert: true)]
        public bool FeatureRunwayAndTaxiway { get; set; } = true;

        /// <summary>
        /// Whether to export seaways.
        /// （是否輸出航道。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemNetwork), invert: true)]
        public bool FeatureWaterway { get; set; } = true;

        /// <summary>
        /// Whether to export tracks.
        /// （是否輸出軌道。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemNetwork), invert: true)]
        public bool FeatureTrack { get; set; } = true;

        /// <summary>
        /// Whether to export POI (point of interest) features.
        /// （是否輸出 POI（興趣點）圖徵。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        public bool SystemPOI { get; set; } = true;

        /// <summary>
        /// Whether to export private facilities' POIs.
        /// （是否輸出私人設施的 POI。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemPOI), invert: true)]
        public bool FeaturePOIPrivate { get; set; } = true;

        /// <summary>
        /// Whether to export public facilities' POIs.
        /// （是否輸出公共設施的 POI。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemPOI), invert: true)]
        public bool FeaturePOIPublic { get; set; } = true;

        /// <summary>
        /// Whether to export transport facilities' POIs.
        /// （是否輸出交通設施的 POI。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemPOI), invert: true)]
        public bool FeaturePOITransport { get; set; } = true;

        /// <summary>
        /// Whether to export utility facilities' POIs.
        /// （是否輸出民生設施的 POI。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemPOI), invert: true)]
        public bool FeaturePOIUtility { get; set; } = true;

        /// <summary>
        /// Whether to export route features.
        /// （是否輸出路線圖徵。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        public bool SystemRoute { get; set; } = true;

        /// <summary>
        /// Whether to export cargo routes.
        /// （是否輸出貨運交通路線。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemRoute), invert: true)]
        public bool FeatureRouteCargo { get; set; } = true;

        /// <summary>
        /// Whether to export passenger routes.
        /// （是否輸出客運交通路線。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        [SettingsUIAdvanced]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(SystemRoute), invert: true)]
        public bool FeatureRoutePassenger { get; set; } = true;

        /// <summary>
        /// Whether to export zoning features.
        /// （是否輸出分區圖徵。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureVectorGroup)]
        public bool SystemZoning { get; set; } = true;

        /// <summary>
        /// Whether to export the terrain.
        /// （是否輸出地形。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureRasterGroup)]
        public bool FeatureTerrain { get; set; } = true;

        /// <summary>
        /// Whether to export water bodies.
        /// （是否輸出水體。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureRasterGroup)]
        public bool FeatureWater { get; set; } = true;

        /// <summary>
        /// The source coordinate reference system.
        /// （來源的坐標參考系統。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionBasicGroup)]
        public IO.CRS SourceCRS { get; set; } = IO.CRS.WGS84;

        /// <summary>
        /// The X value of the coordinate.
        /// （坐標的 X 值。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionBasicGroup)]
        [SettingsUITextInput]
        public string SourceXCoord { get; set; } = "0";

        /// <summary>
        /// The Y value of the coordinate.
        /// （坐標的 Y 值。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionBasicGroup)]
        [SettingsUITextInput]
        public string SourceYCoord { get; set; } = "0";

        /// <summary>
        /// The hemisphere where the coordinate located (UTM only.)
        /// （坐標所在的半球，UTM 投影限定。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionUTMGroup)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsUTM), invert: true)]
        public Geodata.Hemisphere SourceHemisphere { get; set; } = Geodata.Hemisphere.North;

        /// <summary>
        /// The zone where the coordinate located (UTM only.)
        /// （坐標所在的分區，UTM 投影限定。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionUTMGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsUTM), invert: true)]
        public string SourceUTMZone { get; set; } = "31";

        /// <summary>
        /// The ellipsoid which is used by the projection.
        /// （投影法使用的橢球體。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionEllipsoidGroup)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsTransverseMercator), invert: true)]
        public IO.Ellipsoid SourceEllipsoid { get; set; } = IO.Ellipsoid.GRS80;

        /// <summary>
        /// The length of the semi-major axis of the ellipsoid.
        /// （橢球體半長軸的長度。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionEllipsoidGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsCustomEllipsoid), invert: true)]
        public string SourceEllipsoidSemiMajorAxis { get; set; } = "6378137";

        /// <summary>
        /// The inverse flattening of the ellipsoid.
        /// （橢球體的扁平率倒數。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionEllipsoidGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsCustomEllipsoid), invert: true)]
        public string SourceEllipsoidInverseFlattening { get; set; } = "298.257222101";

        /// <summary>
        /// The WGS84 decimal longitude of the projection's origin.
        /// （投影法原點的 WGS84 十進位小數經度。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionProjectionGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsTransverseMercator), invert: true)]
        public string SourceCRSOriginLongitude { get; set; } = "0";

        /// <summary>
        /// The WGS84 decimal latitude of the projection's origin.
        /// （投影法原點的 WGS84 十進位小數緯度。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionProjectionGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsTransverseMercator), invert: true)]
        public string SourceCRSOriginLatitude { get; set; } = "0";

        /// <summary>
        /// The shift of the projection's origin on X-axis.
        /// （投影法原點在 X 軸上的位移。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionProjectionGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsTransverseMercator), invert: true)]
        public string SourceCRSFalseEasting { get; set; } = "0";

        /// <summary>
        /// The shift of the projection's origin on Y-axis.
        /// （投影法原點在 Y 軸上的位移。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionProjectionGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsTransverseMercator), invert: true)]
        public string SourceCRSFalseNorthing { get; set; } = "0";

        /// <summary>
        /// The scale factor on the projection's origin.
        /// （投影法原點的尺度係數。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionProjectionGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsTransverseMercator), invert: true)]
        public string SourceCRSScaleFactor { get; set; } = "0.9996";

        /// <summary>
        /// The Helmert Transform parameters to transform the datum to WGS84.
        /// （用於將大地基準轉換為 WGS84 的赫爾默特轉換參數。）
        /// </summary>
        [SettingsUISection(ProjectionTab, ProjectionProjectionGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsTransverseMercator), invert: true)]
        public string SourceCRSTransform { get; set; } = "0 0 0 0 0 0 0";

        [SettingsUIHidden]
        public bool IsCustomEllipsoid => IsTransverseMercator && (SourceEllipsoid == IO.Ellipsoid.Custom);

        [SettingsUIHidden]
        public bool IsInGameOrEditor => GameMode.GameOrEditor.HasFlag(Instance.GameMode);

        [SettingsUIHidden]
        public bool IsTransverseMercator => SourceCRS == IO.CRS.TransverseMercator;

        [SettingsUIHidden]
        public bool IsUTM => SourceCRS == IO.CRS.UTM;

        /// <summary>
        /// Retrieve export options from the current settings.
        /// （由目前的設定獲得輸出設定。）
        /// </summary>
        /// <returns>The export options.（輸出設定。）</returns>
        public IO.Options GetOptions()
        {
            // Check projection parameters integrity.（確認投影參數的完整性。）
            Geodata.Coord sourceCoordinates = default;
            Geodata.CRS sourceCRS = Geodata.CRS.Unknown;
            Geodata.CRS targetCRS = Geodata.CRS.Unknown;
            IO.Ellipsoid ellipsoid = IO.Ellipsoid.WGS84;
            IO.Feature feature = IO.Feature.None;
            IO.FileFormat vectorFileFormat = IO.FileFormat.Shapefile;
            IO.System system = IO.System.Unknown;
            Geodata.ProjectionDefinition projectionDefinition = default;
            projectionDefinition.transform = new(new double[0]);
            Dictionary<string, IO.Error> errors = new()
            {
                { GetOptionLabelLocaleID(nameof(SourceXCoord)), Utils.IOUtils.TryGetNumber(SourceXCoord, out double sourceX) },
                { GetOptionLabelLocaleID(nameof(SourceYCoord)), Utils.IOUtils.TryGetNumber(SourceYCoord, out double sourceY) }
            };

            switch (ExportVectorFormat)
            {
                case 0:
                    vectorFileFormat = IO.FileFormat.GeoJSON;
                    break;

                case 1:
                    vectorFileFormat = IO.FileFormat.GeoPackage;
                    break;

                case 2:
                    vectorFileFormat = IO.FileFormat.Shapefile;
                    break;
            }

            if (SystemArea) system |= IO.System.Area;
            if (SystemBuilding) system |= IO.System.Building;
            if (SystemNetwork) system |= IO.System.Network;
            if (SystemPOI) system |= IO.System.POI;
            if (SystemRoute) system |= IO.System.Route;
            if (SystemZoning) system |= IO.System.Zoning;
            if (FeatureTerrain || FeatureWater) system |= IO.System.Raster;

            if (FeatureBuilding) feature |= IO.Feature.Building;
            if (FeatureDistrict) feature |= IO.Feature.District;
            if (FeatureExtractor) feature |= IO.Feature.Extractor;
            if (FeatureLandfill) feature |= IO.Feature.Landfill;
            if (FeatureMapTile) feature |= IO.Feature.MapTile;
            if (FeaturePathway) feature |= IO.Feature.Pathway;
            if (FeaturePOIPrivate) feature |= IO.Feature.POIPrivate;
            if (FeaturePOIPublic) feature |= IO.Feature.POIPublic;
            if (FeaturePOITransport) feature |= IO.Feature.POITransport;
            if (FeaturePOIUtility) feature |= IO.Feature.POIUtility;
            if (FeatureRoad) feature |= IO.Feature.Road;
            if (FeatureRouteCargo) feature |= IO.Feature.RouteCargo;
            if (FeatureRoutePassenger) feature |= IO.Feature.RoutePassenger;
            if (FeatureRunwayAndTaxiway) feature |= (IO.Feature.Runway & IO.Feature.Taxiway);
            if (FeatureTrack) feature |= IO.Feature.Track;
            if (FeatureWaterway) feature |= IO.Feature.Waterway;
            if (SystemZoning) feature |= IO.Feature.Zoning;

            switch (SourceCRS)
            {
                case IO.CRS.TransverseMercator:
                    sourceCRS = Geodata.CRS.TransverseMercator;
                    targetCRS = sourceCRS;
                    ellipsoid = SourceEllipsoid;
                    Geodata.EllipsoidDefinition ellipsoidDefinition;

                    if (SourceEllipsoid != IO.Ellipsoid.Custom)
                    {
                        if (!IO.IO.EllipsoidTable.TryGetValue(SourceEllipsoid, out ellipsoidDefinition))
                        {
                            Instance.Log.Warn($"The ellipsoid `{SourceEllipsoid}` is not defined. 橢球體 `{SourceEllipsoid}` 並未被定義。");
                        }
                    }
                    else
                    {
                        errors.Add(GetOptionLabelLocaleID(nameof(SourceEllipsoidSemiMajorAxis)), Utils.IOUtils.TryGetLength(SourceEllipsoidSemiMajorAxis, out double ellipsoidSemiMajorAxis));
                        errors.Add(GetOptionLabelLocaleID(nameof(SourceEllipsoidInverseFlattening)), Utils.IOUtils.TryGetNumber(SourceEllipsoidInverseFlattening, out double ellipsoidInverseFlattening));
                        ellipsoidDefinition = new(ellipsoidSemiMajorAxis, ellipsoidInverseFlattening);
                    }

                    errors.Add(GetOptionLabelLocaleID(nameof(SourceCRSOriginLongitude)), Utils.IOUtils.TryGetLongitude(SourceCRSOriginLongitude, out double sourceOriginLongitude));
                    errors.Add(GetOptionLabelLocaleID(nameof(SourceCRSOriginLatitude)), Utils.IOUtils.TryGetLatitude(SourceCRSOriginLatitude, out double sourceOriginLatitude));
                    errors.Add(GetOptionLabelLocaleID(nameof(SourceCRSFalseEasting)), Utils.IOUtils.TryGetNumber(SourceCRSFalseEasting, out double sourceFalseEasting));
                    errors.Add(GetOptionLabelLocaleID(nameof(SourceCRSFalseNorthing)), Utils.IOUtils.TryGetNumber(SourceCRSFalseNorthing, out double sourceFalseNorthing));
                    errors.Add(GetOptionLabelLocaleID(nameof(SourceCRSScaleFactor)), Utils.IOUtils.TryGetNumber(SourceCRSScaleFactor, out double sourceScaleFactor));
                    errors.Add(GetOptionLabelLocaleID(nameof(SourceCRSTransform)), Utils.IOUtils.TryGetTransform(SourceCRSTransform, out double[] sourceTransform));
                    projectionDefinition = new(ellipsoidDefinition, sourceOriginLongitude, sourceOriginLatitude, sourceFalseEasting, sourceFalseNorthing, sourceScaleFactor, new(sourceTransform));
                    sourceCoordinates = new(sourceX, sourceY, sourceCRS);
                    break;

                case IO.CRS.UTM:
                    sourceCRS = Geodata.CRS.UTM;
                    targetCRS = sourceCRS;
                    errors.Add(GetOptionLabelLocaleID(nameof(SourceUTMZone)), Utils.IOUtils.TryGetUTMZone(SourceUTMZone, out int sourceUTMZone));
                    sourceCoordinates = new(sourceX, sourceY, SourceHemisphere, sourceUTMZone);
                    break;

                case IO.CRS.WGS84:
                    sourceCRS = Geodata.CRS.WGS84;
                    targetCRS = Geodata.CRS.UTM;
                    sourceCoordinates = new(sourceX, sourceY, sourceCRS);
                    break;
            }

            // TODO: Implement the function to stop error user input

            return new()
            {
                AssetPack = true,
                Created = DateTime.Now,
                Display = new Dictionary<(IO.Property, IO.System), bool>
                {
                    { (IO.Property.Category, IO.System.Building), true },
                    { (IO.Property.Category, IO.System.Network), true },
                    { (IO.Property.Category, IO.System.POI), false },
                    { (IO.Property.Object, IO.System.Unknown), false },
                    { (IO.Property.Zoning, IO.System.Unknown), true }
                },
                Elevation = false,
                Features = feature,
                FileName = "OPZ_{Feature}",
                GeoTiffFormat = ExportGeoTiffFormat,
                Homeless = true,
                Minimized = false,
                Properties = new Dictionary<IO.System, HashSet<IO.Property>>
                {
                    { IO.System.Area, new() { IO.Property.Name, IO.Property.Object, IO.Property.Age, IO.Property.Area, IO.Property.Company, IO.Property.Employee, IO.Property.Household, IO.Property.Labor, IO.Property.Profit, IO.Property.Resident, IO.Property.SexRatio, IO.Property.Unlocked, IO.Property.Wage} },
                    { IO.System.Building, new() { IO.Property.Name, IO.Property.Object, IO.Property.Address, IO.Property.Age, IO.Property.Asset, IO.Property.Brand, IO.Property.Category, IO.Property.Elevation, IO.Property.Employee, IO.Property.Height, IO.Property.Household, IO.Property.Labor, IO.Property.Level, IO.Property.Product, IO.Property.Profit, IO.Property.Resident, IO.Property.SexRatio, IO.Property.Story, IO.Property.Theme, IO.Property.Value, IO.Property.Wage, IO.Property.Zoning } },
                    { IO.System.Zoning, new() { IO.Property.Name, IO.Property.Object, IO.Property.Color, IO.Property.Density, IO.Property.Theme, IO.Property.Zoning } }
                },
                RasterFormat = IO.FileFormat.GeoTIFF,
                RasterKinds = IO.RasterKind.WorldDepth | IO.RasterKind.WorldElevation | IO.RasterKind.Depth | IO.RasterKind.Elevation,
                SeparateResident = false,
                SourceCoordinates = sourceCoordinates,
                SourceProjection = sourceCRS,
                SourceProjectionDefinition = projectionDefinition,
                StatisticsMapTile = true,
                Systems = system,
                TargetEllipsoid = ellipsoid,
                TargetProjection = targetCRS,
                TargetProjectionDefinition = projectionDefinition,
                Taxable = false,
                Unzoned = true,
                VectorFormat = vectorFileFormat,
                VectorKinds = new Dictionary<IO.System, IO.VectorKind>
                {
                    { IO.System.Area, IO.VectorKind.Boundary },
                    { IO.System.Building, IO.VectorKind.Boundary },
                    { IO.System.Network, IO.VectorKind.Centerline },
                    { IO.System.Zoning, IO.VectorKind.Boundary }
                },
                ZccColor = true
            };
        }
    }
}
