using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Settings;
using System;
using System.Collections.Generic;

namespace Carto
{
    /// <summary>
    /// The class that manages the mod's options.
    /// （管理模組設定的類別。）
    /// </summary>
    [SettingsUIGroupOrder(ProjectionBasicGroup, ProjectionEllipsoidGroup, ProjectionProjectionGroup, ProjectionUTMGroup)]
    [SettingsUIShowGroupName(ProjectionBasicGroup, ProjectionEllipsoidGroup, ProjectionProjectionGroup, ProjectionUTMGroup)]
    [SettingsUITabOrder(GeneralTab, ProjectionTab)]
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
        public const string ProjectionTab = "ProjectionTab";
        public const string ProjectionBasicGroup = "ProjectionBasicGroup";
        public const string ProjectionEllipsoidGroup = "ProjectionEllipsoidGroup";
        public const string ProjectionProjectionGroup = "ProjectionProjectionGroup";
        public const string ProjectionUTMGroup = "ProjectionUTMGroup";

        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        [SettingsUIButton]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsInGameOrEditor), invert: true)]
        public bool ExportButton
        {
            set { IO.IO.Export(); }
        }

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
            Geodata.ProjectionDefinition projectionDefinition = default;
            projectionDefinition.transform = new(new double[0]);
            Dictionary<string, IO.Error> errors = new()
            {
                { GetOptionLabelLocaleID(nameof(SourceXCoord)), Utils.IOUtils.TryGetNumber(SourceXCoord, out double sourceX) },
                { GetOptionLabelLocaleID(nameof(SourceYCoord)), Utils.IOUtils.TryGetNumber(SourceYCoord, out double sourceY) }
            };

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
                    { (IO.Property.Category, IO.System.Net), true },
                    { (IO.Property.Category, IO.System.POI), false },
                    { (IO.Property.Object, IO.System.Unknown), false },
                    { (IO.Property.Zoning, IO.System.Unknown), true }
                },
                Elevation = false,
                Features = IO.Feature.District | IO.Feature.MapTile,
                FileName = "OPZ_{Feature}",
                GeoTiffFormat = IO.GeoTiffFormat.Float32,
                Homeless = true,
                Minimized = false,
                Properties = new Dictionary<IO.System, HashSet<IO.Property>>
                {
                    { IO.System.Area, new() { IO.Property.Name, IO.Property.Object, IO.Property.Age, IO.Property.Area, IO.Property.Company, IO.Property.Employee, IO.Property.Household, IO.Property.Labor, IO.Property.Profit, IO.Property.Resident, IO.Property.SexRatio, IO.Property.Unlocked, IO.Property.Wage} },
                    //{ IO.System.Building, new() { IO.Property.Age, IO.Property.Brand, IO.Property.Theme, IO.Property.Zoning } }
                    { IO.System.Zoning, new() { IO.Property.Name, IO.Property.Object, IO.Property.Color, IO.Property.Density, IO.Property.Theme, IO.Property.Zoning } }
                },
                RasterFormat = IO.FileFormat.GeoTIFF,
                RasterKinds = IO.RasterKind.WorldDepth | IO.RasterKind.WorldElevation | IO.RasterKind.Depth | IO.RasterKind.Elevation,
                SeparateResident = false,
                SourceCoordinates = sourceCoordinates,
                SourceProjection = sourceCRS,
                SourceProjectionDefinition = projectionDefinition,
                StatisticsMapTile = true,
                Systems = IO.System.Area | IO.System.Zoning,
                TargetEllipsoid = ellipsoid,
                TargetProjection = targetCRS,
                TargetProjectionDefinition = projectionDefinition,
                Taxable = false,
                Unzoned = false,
                VectorFormat = IO.FileFormat.Shapefile,
                VectorKinds = new Dictionary<IO.System, IO.VectorKind>
                {
                    { IO.System.Area, IO.VectorKind.Boundary },
                    { IO.System.Zoning, IO.VectorKind.Boundary }
                },
                ZccColor = true
            };
        }
    }
}
