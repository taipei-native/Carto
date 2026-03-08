using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Carto
{
    /// <summary>
    /// The class that manages the mod's options.
    /// （管理模組設定的類別。）
    /// </summary>
    [SettingsUIGroupOrder(GeneralGeneralGroup, GeneralDangerGroup,
                          FeatureButtonGroup, FeatureVectorGroup, FeatureRasterGroup,
                          PropertiesSelectorGroup, PropertiesGeometryGroup, PropertiesPropertyGroup,
                          ProjectionBasicGroup, ProjectionEllipsoidGroup, ProjectionProjectionGroup, ProjectionUTMGroup,
                          MiscellaneousFileGroup, MiscellaneousGeometryGroup, MiscellaneousPropertyGroup)]
    [SettingsUIShowGroupName(FeatureVectorGroup, FeatureRasterGroup,
                             PropertiesGeometryGroup, PropertiesPropertyGroup,
                             ProjectionBasicGroup, ProjectionEllipsoidGroup, ProjectionProjectionGroup, ProjectionUTMGroup,
                             MiscellaneousFileGroup, MiscellaneousGeometryGroup, MiscellaneousPropertyGroup)]
    [SettingsUITabOrder(GeneralTab, FeatureTab, PropertiesTab, ProjectionTab, MiscellaneousTab)]
    [FileLocation("ModsSettings/" + nameof(Carto) + "/" + nameof(Carto) + "_v1")]
    public class Settings : ModSetting
    {
        public Settings(IMod mod) : base(mod) { SetDefaults(); }

        // NOTE: Update these properties / methods when adding a new one:
        //           （當添加新屬性／方法時，更新以下屬性：）
        // - DisableFeaturesButton
        // - DisablePropertiesButton
        // - EnableFeaturesButton
        // - EnablePropertiesButton
        // - GetOptions()
        // - SetDefaults()
        // - Carto.IO.IO.LogExportOptions()

        /// <summary>
        /// Reset all mod default settings.
        /// （重置所有模組設定。）
        /// </summary>
        public override void SetDefaults()
        {
            CustomNamingFormat = "{City}_{Now}_{Feature}";
            ExportNamingFormat = IO.NamingFormat.Feature;
            ExportVectorFormat = 2;
            ExportGeoTiffFormat = IO.GeoTiffFormat.Int16;
            SystemArea = true;
            FeatureDistrict = true;
            FeatureMapTile = true;
            SystemBuilding = true;
            FeatureBuilding = true;
            FeatureLandfill = true;
            FeatureExtractor = true;
            SystemNetwork = true;
            FeaturePathway = true;
            FeatureRoad = true;
            FeatureRunwayAndTaxiway = true;
            FeatureWaterway = true;
            FeatureTrack = true;
            SystemPOI = true;
            FeaturePOIPrivate = true;
            FeaturePOIPublic = true;
            FeaturePOITransport = true;
            FeaturePOIUtility = true;
            SystemRoute = true;
            FeatureRouteCargo = true;
            FeatureRoutePassenger = true;
            SystemZoning = true;
            FeatureTerrain = true;
            FeatureWater = true;
            PropertiesSystemSelector = SelectorValueArea;
            GeometryBoundaryArea = true;
            GeometryBoundaryBuilding = true;
            GeometryBoundaryNetwork = true;
            GeometryBoundaryZoning = true;
            GeometryCenterlineNetwork = true;
            GeometryCenterlineRoute = true;
            GeometryLocationPOI = true;
            GeometryDepthWater = true;
            GeometryElevationTerrain = true;
            GeometryWorldDepthWater = false;
            GeometryWorldElevationTerrain = false;
            PropertyNameArea = true;
            PropertyNameBuilding = true;
            PropertyNameNetwork = true;
            PropertyNamePOI = true;
            PropertyNameRoute = true;
            PropertyNameZoning = true;
            PropertyAddressBuilding = true;
            PropertyAddressPOI = true;
            PropertyAgeArea = false;
            PropertyAgeBuilding = false;
            PropertyAreaArea = false;
            PropertyAssetBuilding = true;
            PropertyAssetNetwork = true;
            PropertyBrandBuilding = false;
            PropertyCategoryBuilding = true;
            PropertyCategoryNetwork = true;
            PropertyCategoryPOI = true;
            PropertyColorRoute = true;
            PropertyColorZoning = true;
            PropertyCompanyArea = false;
            PropertyDensityZoning = false;
            PropertyDirectionNetwork = true;
            PropertyElevationBuilding = false;
            PropertyElevationNetwork = true;
            PropertyEmployeeArea = true;
            PropertyEmployeeBuilding = true;
            PropertyFormNetwork = true;
            PropertyHouseholdArea = false;
            PropertyHouseholdBuilding = false;
            PropertyLaborArea = false;
            PropertyLaborBuilding = false;
            PropertyLaneNetwork = true;
            PropertyLengthNetwork = false;
            PropertyLengthRoute = true;
            PropertyLevelBuilding = false;
            PropertyLimitNetwork = true;
            PropertyModelRoute = false;
            PropertyObjectArea = true;
            PropertyObjectBuilding = true;
            PropertyObjectNetwork = true;
            PropertyObjectPOI = true;
            PropertyObjectRoute = true;
            PropertyObjectZoning = true;
            PropertyPassengerRoute = true;
            PropertyProductBuilding = false;
            PropertyProfitArea = false;
            PropertyProfitBuilding = false;
            PropertyResidentArea = true;
            PropertyResidentBuilding = true;
            PropertyRouteRoute = false;
            PropertySexRatioArea = false;
            PropertySexRatioBuilding = false;
            PropertyStopRoute = true;
            PropertyThemeBuilding = false;
            PropertyThemeZoning = false;
            PropertyTransportRoute = true;
            PropertyUnlockedArea = true;
            PropertyUsageRoute = true;
            PropertyVehicleRoute = true;
            PropertyVolumeNetwork = false;
            PropertyWageArea = false;
            PropertyWageBuilding = false;
            PropertyWeightRoute = true;
            PropertyWidthNetwork = false;
            PropertyZoneBuilding = false;
            PropertyZoningBuilding = true;
            PropertyZoningZoning = true;
            SourceCRS = IO.CRS.WGS84;
            SourceXCoord = "0";
            SourceYCoord = "0";
            SourceHemisphere = Geodata.Hemisphere.North;
            SourceUTMZone = "31";
            SourceEllipsoid = IO.Ellipsoid.GRS80;
            SourceEllipsoidSemiMajorAxis = "6378137";
            SourceEllipsoidInverseFlattening = "298.257222101";
            SourceCRSOriginLongitude = "0";
            SourceCRSOriginLatitude = "0";
            SourceCRSFalseEasting = "0";
            SourceCRSFalseNorthing = "0";
            SourceCRSScaleFactor = "0.9996";
            SourceCRSTransform = "0 0 0 0 0 0 0";
            OutputElevation = false;
            OutputMinimizedGeoJSON = false;
            PlayCompletionSound = true;
            ShowCompletionDialog = true;
            GeometryInactiveRoute = false;
            GeometrySeparateServiceUpgrade = false;
            GeometryUnzoned = false;
            PropertyGeneralHomeless = true;
            PropertyGeneralMapTileStatistics = true;
            PropertyCategoryBuildingDisplayMode = IO.Display.All;
            PropertyCategoryNetworkDisplayMode = IO.Display.All;
            PropertyCategoryPOIDisplayMode = IO.Display.All;
            PropertyCategoryRoadClassification = IO.RoadClassification.Vanilla;
            PropertyColorZcc = true;
            PropertyPassengerPet = true;
            PropertyResidentSeparateBySex = false;
            PropertyRouteXtm = true;
            PropertyThemeAssetPack = true;
            PropertyWageTaxable = false;
            PropertyZoningDisplayMode = IO.Display.All;
        }

        // The tab and group names.（分頁與群組名稱。）
        public const string GeneralTab = "GeneralTab";
        public const string GeneralGeneralGroup = "GeneralGeneralGroup";
        public const string GeneralGeneralGroupButtons = "GeneralGeneralGroupButtons";
        public const string GeneralDangerGroup = "GeneralDangerGroup";
        public const string GeneralDangerGroupButtons = "GeneralDangerGroupButtons";
        public const string FeatureTab = "FeatureTab";
        public const string FeatureButtonGroup = "FeatureButtonGroup";
        public const string FeatureButtonGroupButtons = "FeatureButtonGroupButtons";
        public const string FeatureVectorGroup = "FeatureVectorGroup";
        public const string FeatureRasterGroup = "FeatureRasterGroup";
        public const string PropertiesTab = "PropertiesTab";
        public const string PropertiesSelectorGroup = "PropertiesSelectorGroup";
        public const string PropertiesSelectorGroupButtons = "PropertiesSelectorGroupButtons";
        public const string PropertiesGeometryGroup = "PropertiesGeometryGroup";
        public const string PropertiesPropertyGroup = "PropertiesPropertyGroup";
        public const string ProjectionTab = "ProjectionTab";
        public const string ProjectionBasicGroup = "ProjectionBasicGroup";
        public const string ProjectionEllipsoidGroup = "ProjectionEllipsoidGroup";
        public const string ProjectionProjectionGroup = "ProjectionProjectionGroup";
        public const string ProjectionUTMGroup = "ProjectionUTMGroup";
        public const string MiscellaneousTab = "MiscellaneousTab";
        public const string MiscellaneousFileGroup = "MiscellaneousFileGroup";
        public const string MiscellaneousGeometryGroup = "MiscellaneousGeometryGroup";
        public const string MiscellaneousPropertyGroup = "MiscellaneousPropertyGroup";

        // The assigned values used in the properties tab.（屬性分頁中指定的值。）
        public const int SelectorValueArea = (int)IO.System.Area;
        public const int SelectorValueBuilding = (int)IO.System.Building;
        public const int SelectorValueNetwork = (int)IO.System.Network;
        public const int SelectorValuePOI = (int)IO.System.POI;
        public const int SelectorValueRoute = (int)IO.System.Route;
        public const int SelectorValueZoning = (int)IO.System.Zoning;
        public const int SelectorValueDeposit = (int)IO.System.Raster + 1;
        public const int SelectorValueLandValue = (int)IO.System.Raster + 2;
        public const int SelectorValuePollution = (int)IO.System.Raster + 3;
        public const int SelectorValueTerrain = (int)IO.System.Raster + 4;
        public const int SelectorValueWater = (int)IO.System.Raster + 5;
        public const int SelectorValueWind = (int)IO.System.Raster + 6;

        // The shared locale ids used in the properties tab.（屬性分頁中共用的語系檔案代碼。）
        public const string DisableAll = "Carto.Carto.Mod.Settings.DisableAll";
        public const string EnableAll = "Carto.Carto.Mod.Settings.EnableAll";
        public const string GeometryBoundary = "Carto.Carto.Mod.Settings.GeometryBoundary";
        public const string GeometryCenterline = "Carto.Carto.Mod.Settings.GeometryCenterline";
        public const string GeometryFootprint = "Carto.Carto.Mod.Settings.GeometryFootprint";
        public const string GeometryLocation = "Carto.Carto.Mod.Settings.GeometryLocation";
        public const string GeometryAirPollution = "Carto.Carto.Mod.Settings.GeometryAirPollution";
        public const string GeometryDepth = "Carto.Carto.Mod.Settings.GeometryDepth";
        public const string GeometryElevation = "Carto.Carto.Mod.Settings.GeometryElevation";
        public const string GeometryFertileDeposit = "Carto.Carto.Mod.Settings.GeometryFertileDeposit";
        public const string GeometryFishDeposit = "Carto.Carto.Mod.Settings.GeometryFishDeposit";
        public const string GeometryFlowDirection = "Carto.Carto.Mod.Settings.GeometryFlowDirection";
        public const string GeometryFlowSpeed = "Carto.Carto.Mod.Settings.GeometryFlowSpeed";
        public const string GeometryGroundPollution = "Carto.Carto.Mod.Settings.GeometryGroundPollution";
        public const string GeometryGroundWaterDeposit = "Carto.Carto.Mod.Settings.GeometryGroundWaterDeposit";
        public const string GeometryGroundWaterPollution = "Carto.Carto.Mod.Settings.GeometryGroundWaterPollution";
        public const string GeometryLandValue = "Carto.Carto.Mod.Settings.GeometryLandValue";
        public const string GeometryNoisePollution = "Carto.Carto.Mod.Settings.GeometryNoisePollution";
        public const string GeometryOilDeposit = "Carto.Carto.Mod.Settings.GeometryOilDeposit";
        public const string GeometryOreDeposit = "Carto.Carto.Mod.Settings.GeometryOreDeposit";
        public const string GeometryWaterPollution = "Carto.Carto.Mod.Settings.GeometryWaterPollution";
        public const string GeometryWindDirection = "Carto.Carto.Mod.Settings.GeometryWindDirection";
        public const string GeometryWindSpeed = "Carto.Carto.Mod.Settings.GeometryWindSpeed";
        public const string GeometryWoodDeposit = "Carto.Carto.Mod.Settings.GeometryWoodDeposit";
        public const string GeometryWorldDepth = "Carto.Carto.Mod.Settings.GeometryWorldDepth";
        public const string GeometryWorldElevation = "Carto.Carto.Mod.Settings.GeometryWorldElevation";
        public const string PropertyAddress = "Carto.Carto.Mod.Settings.PropertyAddress";
        public const string PropertyAge = "Carto.Carto.Mod.Settings.PropertyAge";
        public const string PropertyArea = "Carto.Carto.Mod.Settings.PropertyArea";
        public const string PropertyAsset = "Carto.Carto.Mod.Settings.PropertyAsset";
        public const string PropertyBrand = "Carto.Carto.Mod.Settings.PropertyBrand";
        public const string PropertyCapacity = "Carto.Carto.Mod.Settings.PropertyCapacity";
        public const string PropertyCategory = "Carto.Carto.Mod.Settings.PropertyCategory";
        public const string PropertyColor = "Carto.Carto.Mod.Settings.PropertyColor";
        public const string PropertyCompany = "Carto.Carto.Mod.Settings.PropertyCompany";
        public const string PropertyDensity = "Carto.Carto.Mod.Settings.PropertyDensity";
        public const string PropertyDirection = "Carto.Carto.Mod.Settings.PropertyDirection";
        public const string PropertyDischarge = "Carto.Carto.Mod.Settings.PropertyDischarge";
        public const string PropertyElevation = "Carto.Carto.Mod.Settings.PropertyElevation";
        public const string PropertyEmployee = "Carto.Carto.Mod.Settings.PropertyEmployee";
        public const string PropertyForm = "Carto.Carto.Mod.Settings.PropertyForm";
        public const string PropertyHeight = "Carto.Carto.Mod.Settings.PropertyHeight";
        public const string PropertyHousehold = "Carto.Carto.Mod.Settings.PropertyHousehold";
        public const string PropertyLabor = "Carto.Carto.Mod.Settings.PropertyLabor";
        public const string PropertyLane = "Carto.Carto.Mod.Settings.PropertyLane";
        public const string PropertyLength = "Carto.Carto.Mod.Settings.PropertyLength";
        public const string PropertyLevel = "Carto.Carto.Mod.Settings.PropertyLevel";
        public const string PropertyLimit = "Carto.Carto.Mod.Settings.PropertyLimit";
        public const string PropertyLoad = "Carto.Carto.Mod.Settings.PropertyLoad";
        public const string PropertyModel = "Carto.Carto.Mod.Settings.PropertyModel";
        public const string PropertyName = "Carto.Carto.Mod.Settings.PropertyName";
        public const string PropertyObject = "Carto.Carto.Mod.Settings.PropertyObject";
        public const string PropertyPassenger = "Carto.Carto.Mod.Settings.PropertyPassenger";
        public const string PropertyProduct = "Carto.Carto.Mod.Settings.PropertyProduct";
        public const string PropertyProfit = "Carto.Carto.Mod.Settings.PropertyProfit";
        public const string PropertyResident = "Carto.Carto.Mod.Settings.PropertyResident";
        public const string PropertyRoute = "Carto.Carto.Mod.Settings.PropertyRoute";
        public const string PropertySexRatio = "Carto.Carto.Mod.Settings.PropertySexRatio";
        public const string PropertyStop = "Carto.Carto.Mod.Settings.PropertyStop";
        public const string PropertyStorey = "Carto.Carto.Mod.Settings.PropertyStorey";
        public const string PropertyTheme = "Carto.Carto.Mod.Settings.PropertyTheme";
        public const string PropertyTransport = "Carto.Carto.Mod.Settings.PropertyTransport";
        public const string PropertyUnlocked = "Carto.Carto.Mod.Settings.PropertyUnlocked";
        public const string PropertyUsage = "Carto.Carto.Mod.Settings.PropertyUsage";
        public const string PropertyValue = "Carto.Carto.Mod.Settings.PropertyValue";
        public const string PropertyVehicle = "Carto.Carto.Mod.Settings.PropertyVehicle";
        public const string PropertyVolume = "Carto.Carto.Mod.Settings.PropertyVolume";
        public const string PropertyWage = "Carto.Carto.Mod.Settings.PropertyWage";
        public const string PropertyWeight = "Carto.Carto.Mod.Settings.PropertyWeight";
        public const string PropertyWidth = "Carto.Carto.Mod.Settings.PropertyWidth";
        public const string PropertyZone = "Carto.Carto.Mod.Settings.PropertyZone";
        public const string PropertyZoning = "Carto.Carto.Mod.Settings.PropertyZoning";

        /// <summary>
        /// The file naming format.
        /// （檔案命名格式。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        public IO.NamingFormat ExportNamingFormat { get; set; } = IO.NamingFormat.Feature;

        /// <summary>
        /// The custom file naming format.
        /// （自訂檔案命名格式。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        [SettingsUITextInput]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsCustomNamingFormat), invert: true)]
        public string CustomNamingFormat { get; set; } = "{City}_{Now}_{Feature}";

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
                // TODO: Uncomment when the GeoPackage the export function is implemented.
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

        /// <summary>
        /// The button to reveal the export file directory.
        /// （開啟輸出檔案目錄的按鈕。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        [SettingsUIButton]
        [SettingsUIButtonGroup(GeneralGeneralGroupButtons)]
        public bool OpenButton
        {
            set { Utils.IOUtils.RevealInFileExplorer(Instance.CartoDataPath); }
        }

        /// <summary>
        /// The button to export files.
        /// （輸出檔案的按鈕。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        [SettingsUIButton]
        [SettingsUIButtonGroup(GeneralGeneralGroupButtons)]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsInGameOrEditor), invert: true)]
        public bool ExportButton
        {
            set { IO.IO.Export(); }
        }

# if false
        [SettingsUISection(GeneralTab, GeneralGeneralGroup)]
        [SettingsUIButton]
        [SettingsUIDeveloper]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsInGameOrEditor), invert: true)]
        public bool TestButton
        {
            set
            {
                //Colossal.ScreenUtility.CaptureScreenshot();
                //Game.SceneFlow.GameManager.instance.TakeScreenshot();
                //Colossal.PSI.Common.PlatformManager.instance.TakeScreenshot();
            }
        }
#endif

        /// <summary>
        /// The current running Carto version.
        /// （目前執行中的Carto版本。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralDangerGroup)]
        public string ModVersion => Instance.Version;

        /// <summary>
        /// The button to open user manual.
        /// （開啟使用手冊的按鈕。）
        /// </summary>
        [SettingsUIButton]
        [SettingsUISection(GeneralTab, GeneralDangerGroup)]
        [SettingsUIButtonGroup(GeneralDangerGroupButtons)]
        public bool ManualButton
        {
            set
            {
                try
                {
                    Application.OpenURL("https://github.com/taipei-native/Carto/wiki");
                }
                catch
                {
                    MessageDialog urlErrorDialog = new("Common.WARNING", "Carto.Common.ERROR[Manual]", "Common.OK");
                    Instance.UI.appBindings.ShowMessageDialog(urlErrorDialog, null);
                    Instance.Log.Debug($"Settings.ManualButton: Cannot open user manual. 無法開啟使用手冊。");
                }
            }
        }

        /// <summary>
        /// The button to reset settings.
        /// （用以重置設定的按鈕。）
        /// </summary>
        [SettingsUISection(GeneralTab, GeneralDangerGroup)]
        [SettingsUIButton]
        [SettingsUIButtonGroup(GeneralDangerGroupButtons)]
        [SettingsUIConfirmation]
        public bool ResetButton
        {
            set { SetDefaults(); }
        }

        /// <summary>
        /// The button to disable all features.
        /// （用以關閉所有圖徵的按鈕。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureButtonGroup)]
        [SettingsUIButton]
        [SettingsUIButtonGroup(FeatureButtonGroupButtons)]
        [SettingsUIDisplayName(overrideId: DisableAll)]
        public bool DisableFeaturesButton
        {
            set
            {
                SystemArea = false;
                FeatureDistrict = false;
                FeatureMapTile = false;
                SystemBuilding = false;
                FeatureBuilding = false;
                FeatureLandfill = false;
                FeatureExtractor = false;
                SystemNetwork = false;
                FeaturePathway = false;
                FeatureRoad = false;
                FeatureRunwayAndTaxiway = false;
                FeatureWaterway = false;
                FeatureTrack = false;
                SystemPOI = false;
                FeaturePOIPrivate = false;
                FeaturePOIPublic = false;
                FeaturePOITransport = false;
                FeaturePOIUtility = false;
                SystemRoute = false;
                FeatureRouteCargo = false;
                FeatureRoutePassenger = false;
                SystemZoning = false;
                FeatureTerrain = false;
                FeatureWater = false;
            }
        }

        /// <summary>
        /// The button to enable all features.
        /// （用以啟用所有圖徵的按鈕。）
        /// </summary>
        [SettingsUISection(FeatureTab, FeatureButtonGroup)]
        [SettingsUIButton]
        [SettingsUIButtonGroup(FeatureButtonGroupButtons)]
        [SettingsUIDisplayName(overrideId: EnableAll)]
        public bool EnableFeaturesButton
        {
            set
            {
                SystemArea = true;
                FeatureDistrict = true;
                FeatureMapTile = true;
                SystemBuilding = true;
                FeatureBuilding = true;
                FeatureLandfill = true;
                FeatureExtractor = true;
                SystemNetwork = true;
                FeaturePathway = true;
                FeatureRoad = true;
                FeatureRunwayAndTaxiway = true;
                FeatureWaterway = true;
                FeatureTrack = true;
                SystemPOI = true;
                FeaturePOIPrivate = true;
                FeaturePOIPublic = true;
                FeaturePOITransport = true;
                FeaturePOIUtility = true;
                SystemRoute = true;
                FeatureRouteCargo = true;
                FeatureRoutePassenger = true;
                SystemZoning = true;
                FeatureTerrain = true;
                FeatureWater = true;
            }
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
        /// （是否輸出公用設施的 POI。）
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
        /// The button to disable all geometries and properties.
        /// （用以關閉所有幾何與屬性的按鈕。）
        /// </summary>
        [SettingsUISection(PropertiesTab, PropertiesSelectorGroup)]
        [SettingsUIButton]
        [SettingsUIButtonGroup(PropertiesSelectorGroupButtons)]
        [SettingsUIDisplayName(overrideId: DisableAll)]
        public bool DisablePropertiesButton
        {
            set
            {
                GeometryBoundaryArea = false;
                GeometryBoundaryBuilding = false;
                GeometryBoundaryNetwork = false;
                GeometryBoundaryZoning = false;
                GeometryCenterlineNetwork = false;
                GeometryCenterlineRoute = false;
                GeometryLocationPOI = false;
                GeometryDepthWater = false;
                GeometryElevationTerrain = false;
                GeometryWorldDepthWater = false;
                GeometryWorldElevationTerrain = false;
                PropertyNameArea = false;
                PropertyNameBuilding = false;
                PropertyNameNetwork = false;
                PropertyNamePOI = false;
                PropertyNameRoute = false;
                PropertyNameZoning = false;
                PropertyAddressBuilding = false;
                PropertyAddressPOI = false;
                PropertyAgeArea = false;
                PropertyAgeBuilding = false;
                PropertyAreaArea = false;
                PropertyAssetBuilding = false;
                PropertyAssetNetwork = false;
                PropertyBrandBuilding = false;
                PropertyCategoryBuilding = false;
                PropertyCategoryNetwork = false;
                PropertyCategoryPOI = false;
                PropertyColorRoute = false;
                PropertyColorZoning = false;
                PropertyCompanyArea = false;
                PropertyDensityZoning = false;
                PropertyDirectionNetwork = false;
                PropertyElevationBuilding = false;
                PropertyElevationNetwork = false;
                PropertyEmployeeArea = false;
                PropertyEmployeeBuilding = false;
                PropertyFormNetwork = false;
                PropertyHouseholdArea = false;
                PropertyHouseholdBuilding = false;
                PropertyLaborArea = false;
                PropertyLaborBuilding = false;
                PropertyLaneNetwork = false;
                PropertyLengthNetwork = false;
                PropertyLengthRoute = false;
                PropertyLevelBuilding = false;
                PropertyLimitNetwork = false;
                PropertyModelRoute = false;
                PropertyObjectArea = false;
                PropertyObjectBuilding = false;
                PropertyObjectNetwork = false;
                PropertyObjectPOI = false;
                PropertyObjectRoute = false;
                PropertyObjectZoning = false;
                PropertyPassengerRoute = false;
                PropertyProductBuilding = false;
                PropertyProfitArea = false;
                PropertyProfitBuilding = false;
                PropertyResidentArea = false;
                PropertyResidentBuilding = false;
                PropertyRouteRoute = false;
                PropertySexRatioArea = false;
                PropertySexRatioBuilding = false;
                PropertyStopRoute = false;
                PropertyThemeBuilding = false;
                PropertyThemeZoning = false;
                PropertyTransportRoute = false;
                PropertyUnlockedArea = false;
                PropertyUsageRoute = false;
                PropertyVehicleRoute = false;
                PropertyVolumeNetwork = false;
                PropertyWageArea = false;
                PropertyWageBuilding = false;
                PropertyWeightRoute = false;
                PropertyWidthNetwork = false;
                PropertyZoneBuilding = false;
                PropertyZoningBuilding = false;
                PropertyZoningZoning = false;
            }
        }

        /// <summary>
        /// The button to enable all geometries and properties.
        /// （用以啟用所有幾何與屬性的按鈕。）
        /// </summary>
        [SettingsUISection(PropertiesTab, PropertiesSelectorGroup)]
        [SettingsUIButton]
        [SettingsUIButtonGroup(PropertiesSelectorGroupButtons)]
        [SettingsUIDisplayName(overrideId: EnableAll)]
        public bool EnablePropertiesButton
        {
            set
            {
                GeometryBoundaryArea = true;
                GeometryBoundaryBuilding = true;
                GeometryBoundaryNetwork = true;
                GeometryBoundaryZoning = true;
                GeometryCenterlineNetwork = true;
                GeometryCenterlineRoute = true;
                GeometryLocationPOI = true;
                GeometryDepthWater = true;
                GeometryElevationTerrain = true;
                GeometryWorldDepthWater = true;
                GeometryWorldElevationTerrain = true;
                PropertyNameArea = true;
                PropertyNameBuilding = true;
                PropertyNameNetwork = true;
                PropertyNamePOI = true;
                PropertyNameRoute = true;
                PropertyNameZoning = true;
                PropertyAddressBuilding = true;
                PropertyAddressPOI = true;
                PropertyAgeArea = true;
                PropertyAgeBuilding = true;
                PropertyAreaArea = true;
                PropertyAssetBuilding = true;
                PropertyAssetNetwork = true;
                PropertyBrandBuilding = true;
                PropertyCategoryBuilding = true;
                PropertyCategoryNetwork = true;
                PropertyCategoryPOI = true;
                PropertyColorRoute = true;
                PropertyColorZoning = true;
                PropertyCompanyArea = true;
                PropertyDensityZoning = true;
                PropertyDirectionNetwork = true;
                PropertyElevationBuilding = true;
                PropertyElevationNetwork = true;
                PropertyEmployeeArea = true;
                PropertyEmployeeBuilding = true;
                PropertyFormNetwork = true;
                PropertyHouseholdArea = true;
                PropertyHouseholdBuilding = true;
                PropertyLaborArea = true;
                PropertyLaborBuilding = true;
                PropertyLaneNetwork = true;
                PropertyLengthNetwork = true;
                PropertyLengthRoute = true;
                PropertyLevelBuilding = true;
                PropertyLimitNetwork = true;
                PropertyModelRoute = true;
                PropertyObjectArea = true;
                PropertyObjectBuilding = true;
                PropertyObjectNetwork = true;
                PropertyObjectPOI = true;
                PropertyObjectRoute = true;
                PropertyObjectZoning = true;
                PropertyPassengerRoute = true;
                PropertyProductBuilding = true;
                PropertyProfitArea = true;
                PropertyProfitBuilding = true;
                PropertyResidentArea = true;
                PropertyResidentBuilding = true;
                PropertyRouteRoute = true;
                PropertySexRatioArea = true;
                PropertySexRatioBuilding = true;
                PropertyStopRoute = true;
                PropertyThemeBuilding = true;
                PropertyThemeZoning = true;
                PropertyTransportRoute = true;
                PropertyUnlockedArea = true;
                PropertyUsageRoute = true;
                PropertyVehicleRoute = true;
                PropertyVolumeNetwork = true;
                PropertyWageArea = true;
                PropertyWageBuilding = true;
                PropertyWeightRoute = true;
                PropertyWidthNetwork = true;
                PropertyZoneBuilding = true;
                PropertyZoningBuilding = true;
                PropertyZoningZoning = true;
            }
        }

        /// <summary>
        /// The drop-down menu to configure each system.
        /// （用於調整各系統的下拉式選單。）
        /// </summary>
        [SettingsUISection(PropertiesTab, PropertiesSelectorGroup)]
        [SettingsUIDropdown(typeof(Settings), nameof(GetSystems))]
        public int PropertiesSystemSelector { get; set; } = SelectorValueArea;

        public DropdownItem<int>[] GetSystems()
        {
            return new DropdownItem<int>[]
            {
                new()
                {
                    value = SelectorValueArea,
                    displayName = GetOptionLabelLocaleID(nameof(SystemArea))
                },
                new()
                {
                    value = SelectorValueBuilding,
                    displayName = GetOptionLabelLocaleID(nameof(SystemBuilding))
                },
                new()
                {
                    value = SelectorValueNetwork,
                    displayName = GetOptionLabelLocaleID(nameof(SystemNetwork))
                },
                new()
                {
                    value = SelectorValuePOI,
                    displayName = GetOptionLabelLocaleID(nameof(SystemPOI))
                },
                new()
                {
                    value = SelectorValueRoute,
                    displayName = GetOptionLabelLocaleID(nameof(SystemRoute))
                },
                new()
                {
                    value = SelectorValueZoning,
                    displayName = GetOptionLabelLocaleID(nameof(SystemZoning))
                },
                // TODO: Uncomment when the relevant export functions are implemented.
                /*new()
                {
                    value = SelectorValueDeposit,
                    displayName = GetOptionLabelLocaleID(nameof(FeatureDeposit))
                },*/
                /*new()
                {
                    value = SelectorValueLandValue,
                    displayName = GetOptionLabelLocaleID(nameof(FeatureLandValue))
                },*/
                /*new()
                {
                    value = SelectorValuePollution,
                    displayName = GetOptionLabelLocaleID(nameof(FeaturePollution))
                },*/
                new()
                {
                    value = SelectorValueTerrain,
                    displayName = GetOptionLabelLocaleID(nameof(FeatureTerrain))
                },
                new()
                {
                    value = SelectorValueWater,
                    displayName = GetOptionLabelLocaleID(nameof(FeatureWater))
                },
                /*new()
                {
                    value = SelectorValueWind,
                    displayName = GetOptionLabelLocaleID(nameof(FeatureWind))
                },*/
            };
        }

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryBoundary)]
        [SettingsUIDescription(overrideId: GeometryBoundary)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool GeometryBoundaryArea { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryBoundary)]
        [SettingsUIDescription(overrideId: GeometryBoundary)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool GeometryBoundaryBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryBoundary)]
        [SettingsUIDescription(overrideId: GeometryBoundary)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool GeometryBoundaryNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryBoundary)]
        [SettingsUIDescription(overrideId: GeometryBoundary)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectZoning), invert: true)]
        public bool GeometryBoundaryZoning { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryCenterline)]
        [SettingsUIDescription(overrideId: GeometryCenterline)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool GeometryCenterlineNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryCenterline)]
        [SettingsUIDescription(overrideId: GeometryCenterline)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool GeometryCenterlineRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryLocation)]
        [SettingsUIDescription(overrideId: GeometryLocation)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectPOI), invert: true)]
        public bool GeometryLocationPOI { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryDepth)]
        [SettingsUIDescription(overrideId: GeometryDepth)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectWater), invert: true)]
        public bool GeometryDepthWater { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryElevation)]
        [SettingsUIDescription(overrideId: GeometryElevation)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectTerrain), invert: true)]
        public bool GeometryElevationTerrain { get; set; } = true;
        
        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryWorldDepth)]
        [SettingsUIDescription(overrideId: GeometryWorldDepth)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectWater), invert: true)]
        public bool GeometryWorldDepthWater { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesGeometryGroup)]
        [SettingsUIDisplayName(overrideId: GeometryWorldElevation)]
        [SettingsUIDescription(overrideId: GeometryWorldElevation)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectTerrain), invert: true)]
        public bool GeometryWorldElevationTerrain { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyName)]
        [SettingsUIDescription(overrideId: PropertyName)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyNameArea { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyName)]
        [SettingsUIDescription(overrideId: PropertyName)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyNameBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyName)]
        [SettingsUIDescription(overrideId: PropertyName)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyNameNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyName)]
        [SettingsUIDescription(overrideId: PropertyName)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectPOI), invert: true)]
        public bool PropertyNamePOI { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyName)]
        [SettingsUIDescription(overrideId: PropertyName)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyNameRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyName)]
        [SettingsUIDescription(overrideId: PropertyName)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectZoning), invert: true)]
        public bool PropertyNameZoning { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyAddress)]
        [SettingsUIDescription(overrideId: PropertyAddress)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyAddressBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyAddress)]
        [SettingsUIDescription(overrideId: PropertyAddress)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectPOI), invert: true)]
        public bool PropertyAddressPOI { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyAge)]
        [SettingsUIDescription(overrideId: PropertyAge)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyAgeArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyAge)]
        [SettingsUIDescription(overrideId: PropertyAge)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyAgeBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyArea)]
        [SettingsUIDescription(overrideId: PropertyArea)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyAreaArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyAsset)]
        [SettingsUIDescription(overrideId: PropertyAsset)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyAssetBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyAsset)]
        [SettingsUIDescription(overrideId: PropertyAsset)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyAssetNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyBrand)]
        [SettingsUIDescription(overrideId: PropertyBrand)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyBrandBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyCategory)]
        [SettingsUIDescription(overrideId: PropertyCategory)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyCategoryBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyCategory)]
        [SettingsUIDescription(overrideId: PropertyCategory)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyCategoryNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyCategory)]
        [SettingsUIDescription(overrideId: PropertyCategory)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectPOI), invert: true)]
        public bool PropertyCategoryPOI { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyColor)]
        [SettingsUIDescription(overrideId: PropertyColor)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyColorRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyColor)]
        [SettingsUIDescription(overrideId: PropertyColor)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectZoning), invert: true)]
        public bool PropertyColorZoning { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyCompany)]
        [SettingsUIDescription(overrideId: PropertyCompany)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyCompanyArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyDensity)]
        [SettingsUIDescription(overrideId: PropertyDensity)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectZoning), invert: true)]
        public bool PropertyDensityZoning { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyDirection)]
        [SettingsUIDescription(overrideId: PropertyDirection)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyDirectionNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyElevation)]
        [SettingsUIDescription(overrideId: PropertyElevation)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyElevationBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyElevation)]
        [SettingsUIDescription(overrideId: PropertyElevation)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyElevationNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyEmployee)]
        [SettingsUIDescription(overrideId: PropertyEmployee)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyEmployeeArea { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyEmployee)]
        [SettingsUIDescription(overrideId: PropertyEmployee)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyEmployeeBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyForm)]
        [SettingsUIDescription(overrideId: PropertyForm)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyFormNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyHousehold)]
        [SettingsUIDescription(overrideId: PropertyHousehold)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyHouseholdArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyHousehold)]
        [SettingsUIDescription(overrideId: PropertyHousehold)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyHouseholdBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyLabor)]
        [SettingsUIDescription(overrideId: PropertyLabor)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyLaborArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyLabor)]
        [SettingsUIDescription(overrideId: PropertyLabor)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyLaborBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyLane)]
        [SettingsUIDescription(overrideId: PropertyLane)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyLaneNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyLength)]
        [SettingsUIDescription(overrideId: PropertyLength)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyLengthNetwork { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyLength)]
        [SettingsUIDescription(overrideId: PropertyLength)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyLengthRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyLevel)]
        [SettingsUIDescription(overrideId: PropertyLevel)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyLevelBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyLimit)]
        [SettingsUIDescription(overrideId: PropertyLimit)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyLimitNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyModel)]
        [SettingsUIDescription(overrideId: PropertyModel)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyModelRoute { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyObject)]
        [SettingsUIDescription(overrideId: PropertyObject)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyObjectArea { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyObject)]
        [SettingsUIDescription(overrideId: PropertyObject)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyObjectBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyObject)]
        [SettingsUIDescription(overrideId: PropertyObject)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyObjectNetwork { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyObject)]
        [SettingsUIDescription(overrideId: PropertyObject)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectPOI), invert: true)]
        public bool PropertyObjectPOI { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyObject)]
        [SettingsUIDescription(overrideId: PropertyObject)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyObjectRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyObject)]
        [SettingsUIDescription(overrideId: PropertyObject)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectZoning), invert: true)]
        public bool PropertyObjectZoning { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyPassenger)]
        [SettingsUIDescription(overrideId: PropertyPassenger)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyPassengerRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyProduct)]
        [SettingsUIDescription(overrideId: PropertyProduct)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyProductBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyProfit)]
        [SettingsUIDescription(overrideId: PropertyProfit)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyProfitArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyProfit)]
        [SettingsUIDescription(overrideId: PropertyProfit)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyProfitBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyResident)]
        [SettingsUIDescription(overrideId: PropertyResident)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyResidentArea { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyResident)]
        [SettingsUIDescription(overrideId: PropertyResident)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyResidentBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyRoute)]
        [SettingsUIDescription(overrideId: PropertyRoute)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyRouteRoute { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertySexRatio)]
        [SettingsUIDescription(overrideId: PropertySexRatio)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertySexRatioArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertySexRatio)]
        [SettingsUIDescription(overrideId: PropertySexRatio)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertySexRatioBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyStop)]
        [SettingsUIDescription(overrideId: PropertyStop)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyStopRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyTheme)]
        [SettingsUIDescription(overrideId: PropertyTheme)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyThemeBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyTheme)]
        [SettingsUIDescription(overrideId: PropertyTheme)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectZoning), invert: true)]
        public bool PropertyThemeZoning { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyTransport)]
        [SettingsUIDescription(overrideId: PropertyTransport)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyTransportRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyUnlocked)]
        [SettingsUIDescription(overrideId: PropertyUnlocked)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyUnlockedArea { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyUsage)]
        [SettingsUIDescription(overrideId: PropertyUsage)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyUsageRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyVehicle)]
        [SettingsUIDescription(overrideId: PropertyVehicle)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyVehicleRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyVolume)]
        [SettingsUIDescription(overrideId: PropertyVolume)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyVolumeNetwork { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyWage)]
        [SettingsUIDescription(overrideId: PropertyWage)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectArea), invert: true)]
        public bool PropertyWageArea { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyWage)]
        [SettingsUIDescription(overrideId: PropertyWage)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyWageBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyWeight)]
        [SettingsUIDescription(overrideId: PropertyWeight)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectRoute), invert: true)]
        public bool PropertyWeightRoute { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyWidth)]
        [SettingsUIDescription(overrideId: PropertyWidth)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectNetwork), invert: true)]
        public bool PropertyWidthNetwork { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyZone)]
        [SettingsUIDescription(overrideId: PropertyZone)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyZoneBuilding { get; set; } = false;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyZoning)]
        [SettingsUIDescription(overrideId: PropertyZoning)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectBuilding), invert: true)]
        public bool PropertyZoningBuilding { get; set; } = true;

        [SettingsUISection(PropertiesTab, PropertiesPropertyGroup)]
        [SettingsUIDisplayName(overrideId: PropertyZoning)]
        [SettingsUIDescription(overrideId: PropertyZoning)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(DoesUserSelectZoning), invert: true)]
        public bool PropertyZoningZoning { get; set; } = true;

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

        /// <summary>
        /// Whether to export elevation of the coordinate in the file.
        /// （是否輸出坐標高程至檔案中？。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousFileGroup)]
        public bool OutputElevation { get; set; } = false;

        /// <summary>
        /// Whether to export minimized GeoJSON file.
        /// （是否輸出最小化的 GeoJSON 檔案。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousFileGroup)]
        public bool OutputMinimizedGeoJSON { get; set; } = false;

        /// <summary>
        /// Whether to play a completion sound when the export completes.
        /// （是否要在輸出完成後播放一段音效？）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousFileGroup)]
        public bool PlayCompletionSound { get; set; } = true;

        /// <summary>
        /// Whether to show a dialog when the export completes.
        /// （是否要在輸出完成後顯示對話框？）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousFileGroup)]
        public bool ShowCompletionDialog { get; set; } = true;

        /// <summary>
        /// Whether to export inactive transportation routes.
        /// （是否輸出未啟用的運輸服務路線。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousGeometryGroup)]
        public bool GeometryInactiveRoute { get; set; } = false;

        /// <summary>
        /// Whether to export service upgrade buildings separately.
        /// （是否獨立輸出服務升級建築。)
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousGeometryGroup)]
        public bool GeometrySeparateServiceUpgrade { get; set; } = false;

        /// <summary>
        /// Whether to exported unzoned zoning cells.
        /// （是否輸出無分區的分區單元。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousGeometryGroup)]
        public bool GeometryUnzoned { get; set; } = false;

        /// <summary>
        /// Whether to count region statistics for the map tiles.
        /// （是否要輸出地圖區塊的區域統計資料。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public bool PropertyGeneralMapTileStatistics { get; set; } = true;

        /// <summary>
        /// Whether to count homeless households and residents.
        /// （是否計入無家可歸的家庭與居民。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public bool PropertyGeneralHomeless { get; set; } = true;

        /// <summary>
        /// The display mode for the category field of the building features.
        /// （建築圖徵的分類欄位顯示模式。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public IO.Display PropertyCategoryBuildingDisplayMode { get; set; } = IO.Display.All;

        /// <summary>
        /// The display mode for the category field of the network features.
        /// （網路圖徵的分類欄位顯示模式。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public IO.Display PropertyCategoryNetworkDisplayMode { get; set; } = IO.Display.All;

        /// <summary>
        /// The display mode for the category field of the POI features.
        /// （興趣點圖徵的分類欄位顯示模式。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public IO.Display PropertyCategoryPOIDisplayMode { get; set; } = IO.Display.All;

        /// <summary>
        /// The method to classify road networks.
        /// （分類道路網路的方式。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public IO.RoadClassification PropertyCategoryRoadClassification { get; set; } = IO.RoadClassification.Vanilla;

        /// <summary>
        /// Whether to use Zone Color Changer's zone color, instead of vanilla's.
        /// （是否使用 Zone Color Chanager 的顏色，而非遊戲原版的分區色彩。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsZccEnabled), invert: true)]
        public bool PropertyColorZcc { get; set; } = true;

        /// <summary>
        /// Whether to regard the pets as regular passengers.
        /// （是否將寵物視為一般乘客。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public bool PropertyPassengerPet { get; set; } = true;

        /// <summary>
        /// Whether to separate resident statistics by sex.
        /// （是否依生理性別分離居民統計資料。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public bool PropertyResidentSeparateBySex { get; set; } = false;

        /// <summary>
        /// Whether to use Extended Transport Manager's route acronym, instead of vanilla's route number.
        /// （是否使用 Extended Transport Manager 的路線縮寫，而非遊戲原版的路線編號。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        [SettingsUIHideByCondition(typeof(Settings), nameof(IsXtmEnabled), invert: true)]
        public bool PropertyRouteXtm { get; set; } = true;

        /// <summary>
        /// Whether to classify the asset packs as individual themes.
        /// （是否將資產包視為獨立的風格。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public bool PropertyThemeAssetPack { get; set; } = true;

        /// <summary>
        /// Whether to count building's or area's taxable income, instead of the gross income.
        /// （是否計算建築或區域的可納稅所得，而非總所得。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public bool PropertyWageTaxable { get; set; } = false;

        /// <summary>
        /// The display mode for the zoning field.
        /// （分區欄位顯示模式。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousPropertyGroup)]
        public IO.Display PropertyZoningDisplayMode { get; set; } = IO.Display.All;

        [SettingsUIHidden]
        public bool DoesUserSelectArea => PropertiesSystemSelector == SelectorValueArea;

        [SettingsUIHidden]
        public bool DoesUserSelectBuilding => PropertiesSystemSelector == SelectorValueBuilding;

        [SettingsUIHidden]
        public bool DoesUserSelectNetwork => PropertiesSystemSelector == SelectorValueNetwork;

        [SettingsUIHidden]
        public bool DoesUserSelectPOI => PropertiesSystemSelector == SelectorValuePOI;

        [SettingsUIHidden]
        public bool DoesUserSelectRoute => PropertiesSystemSelector == SelectorValueRoute;

        [SettingsUIHidden]
        public bool DoesUserSelectZoning => PropertiesSystemSelector == SelectorValueZoning;

        [SettingsUIHidden]
        public bool DoesUserSelectDeposit => PropertiesSystemSelector == SelectorValueDeposit;

        [SettingsUIHidden]
        public bool DoesUserSelectLandValue => PropertiesSystemSelector == SelectorValueLandValue;

        [SettingsUIHidden]
        public bool DoesUserSelectPollution => PropertiesSystemSelector == SelectorValuePollution;

        [SettingsUIHidden]
        public bool DoesUserSelectTerrain => PropertiesSystemSelector == SelectorValueTerrain;

        [SettingsUIHidden]
        public bool DoesUserSelectWater => PropertiesSystemSelector == SelectorValueWater;

        [SettingsUIHidden]
        public bool DoesUserSelectWind => PropertiesSystemSelector == SelectorValueWind;

        [SettingsUIHidden]
        public bool IsCustomEllipsoid => IsTransverseMercator && (SourceEllipsoid == IO.Ellipsoid.Custom);

        [SettingsUIHidden]
        public bool IsCustomNamingFormat => ExportNamingFormat == IO.NamingFormat.Custom;

        [SettingsUIHidden]
        public bool IsInGameOrEditor => GameMode.GameOrEditor.HasFlag(Instance.GameMode);

        [SettingsUIHidden]
        public bool IsTransverseMercator => SourceCRS == IO.CRS.TransverseMercator;

        [SettingsUIHidden]
        public bool IsUTM => SourceCRS == IO.CRS.UTM;

        [SettingsUIHidden]
        public bool IsXtmEnabled => Instance.Xtm.TryGet(false);

        [SettingsUIHidden]
        public bool IsZccEnabled => Instance.Zcc.TryGet(false);

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
            IO.RasterKind rasterKinds = IO.RasterKind.Unknown;
            IO.System system = IO.System.Unknown;
            Geodata.ProjectionDefinition projectionDefinition = default;
            projectionDefinition.transform = new(new double[0]);
            Dictionary<string, IO.Error> errors = new()
            {
                { GetOptionLabelLocaleID(nameof(SourceXCoord)), Utils.IOUtils.TryGetNumber(SourceXCoord, out double sourceX) },
                { GetOptionLabelLocaleID(nameof(SourceYCoord)), Utils.IOUtils.TryGetNumber(SourceYCoord, out double sourceY) }
            };
            Dictionary<IO.System, HashSet<IO.Property>> properties = new();
            Dictionary<IO.System, IO.VectorKind> vectorKinds = new();

            string fileBaseFormat = "{Feature}";

            switch (ExportNamingFormat)
            {
                case IO.NamingFormat.Custom:
                    if (!string.IsNullOrEmpty(CustomNamingFormat) && !string.IsNullOrWhiteSpace(CustomNamingFormat)) fileBaseFormat = CustomNamingFormat;
                    break;

                case IO.NamingFormat.Feature:
                    break;

                case IO.NamingFormat.CityNameFeature:
                    fileBaseFormat = "{City}_{Feature}";
                    break;

                case IO.NamingFormat.MapNameFeature:
                    fileBaseFormat = "{Map}_{Feature}";
                    break;
            }

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

            if (SystemArea)
            {
                system |= IO.System.Area;
                if (FeatureDistrict) feature |= IO.Feature.District;
                if (FeatureMapTile) feature |= IO.Feature.MapTile;
                if (GeometryBoundaryArea) TryAddVectorKindEntry(vectorKinds, IO.System.Area, IO.VectorKind.Boundary);
                if (PropertyNameArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Name);
                if (PropertyAgeArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Age);
                if (PropertyAreaArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Area);
                if (PropertyCompanyArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Company);
                if (PropertyEmployeeArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Employee);
                if (PropertyHouseholdArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Household);
                if (PropertyLaborArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Labor);
                if (PropertyObjectArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Object);
                if (PropertyProfitArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Profit);
                if (PropertyResidentArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Resident);
                if (PropertySexRatioArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.SexRatio);
                if (PropertyUnlockedArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Unlocked);
                if (PropertyWageArea) TryAddPropertyEntry(properties, IO.System.Area, IO.Property.Wage);
            }
            if (SystemBuilding)
            {
                system |= IO.System.Building;
                if (FeatureBuilding) feature |= IO.Feature.Building;
                if (FeatureExtractor) feature |= IO.Feature.Extractor;
                if (FeatureLandfill) feature |= IO.Feature.Landfill;
                if (GeometryBoundaryBuilding) TryAddVectorKindEntry(vectorKinds, IO.System.Building, IO.VectorKind.Boundary);
                if (PropertyNameBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Name);
                if (PropertyAddressBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Address);
                if (PropertyAgeBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Age);
                if (PropertyAssetBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Asset);
                if (PropertyBrandBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Brand);
                if (PropertyCategoryBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Category);
                if (PropertyElevationBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Elevation);
                if (PropertyEmployeeBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Employee);
                if (PropertyHouseholdBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Household);
                if (PropertyLaborBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Labor);
                if (PropertyLevelBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Level);
                if (PropertyObjectBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Object);
                if (PropertyProductBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Product);
                if (PropertyProfitBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Profit);
                if (PropertyResidentBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Resident);
                if (PropertySexRatioBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.SexRatio);
                if (PropertyThemeBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Theme);
                if (PropertyWageBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Wage);
                if (PropertyZoneBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Zone);
                if (PropertyZoningBuilding) TryAddPropertyEntry(properties, IO.System.Building, IO.Property.Zoning);
            }
            if (SystemNetwork)
            {
                system |= IO.System.Network;
                if (FeaturePathway) feature |= IO.Feature.Pathway;
                if (FeatureRoad) feature |= IO.Feature.Road;
                if (FeatureRunwayAndTaxiway) feature |= IO.Feature.Runway | IO.Feature.Taxiway;
                if (FeatureTrack) feature |= IO.Feature.Track;
                if (FeatureWaterway) feature |= IO.Feature.Waterway;
                if (GeometryBoundaryNetwork) TryAddVectorKindEntry(vectorKinds, IO.System.Network, IO.VectorKind.Boundary);
                if (GeometryCenterlineNetwork) TryAddVectorKindEntry(vectorKinds, IO.System.Network, IO.VectorKind.Centerline);
                if (PropertyNameNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Name);
                if (PropertyAssetNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Asset);
                if (PropertyCategoryNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Category);
                if (PropertyDirectionNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Direction);
                if (PropertyElevationNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Elevation);
                if (PropertyFormNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Form);
                if (PropertyLaneNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Lane);
                if (PropertyLengthNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Length);
                if (PropertyLimitNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Limit);
                if (PropertyObjectNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Object);
                if (PropertyVolumeNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Volume);
                if (PropertyWidthNetwork) TryAddPropertyEntry(properties, IO.System.Network, IO.Property.Width);
            }
            if (SystemPOI)
            {
                system |= IO.System.POI;
                if (FeaturePOIPrivate) feature |= IO.Feature.POIPrivate;
                if (FeaturePOIPublic) feature |= IO.Feature.POIPublic;
                if (FeaturePOITransport) feature |= IO.Feature.POITransport;
                if (FeaturePOIUtility) feature |= IO.Feature.POIUtility;
                if (GeometryLocationPOI) TryAddVectorKindEntry(vectorKinds, IO.System.POI, IO.VectorKind.Location);
                if (PropertyNamePOI) TryAddPropertyEntry(properties, IO.System.POI, IO.Property.Name);
                if (PropertyAddressPOI) TryAddPropertyEntry(properties, IO.System.POI, IO.Property.Address);
                if (PropertyCategoryPOI) TryAddPropertyEntry(properties, IO.System.POI, IO.Property.Category);
                if (PropertyObjectPOI) TryAddPropertyEntry(properties, IO.System.POI, IO.Property.Object);
            }
            if (SystemRoute)
            {
                system |= IO.System.Route;
                if (FeatureRouteCargo) feature |= IO.Feature.RouteCargo;
                if (FeatureRoutePassenger) feature |= IO.Feature.RoutePassenger;
                if (GeometryCenterlineRoute) TryAddVectorKindEntry(vectorKinds, IO.System.Route, IO.VectorKind.Centerline);
                if (PropertyNameRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Name);
                if (PropertyColorRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Color);
                if (PropertyLengthRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Length);
                if (PropertyModelRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Model);
                if (PropertyObjectRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Object);
                if (PropertyPassengerRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Passenger);
                if (PropertyRouteRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Route);
                if (PropertyStopRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Stop);
                if (PropertyTransportRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Transport);
                if (PropertyUsageRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Usage);
                if (PropertyVehicleRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Vehicle);
                if (PropertyWeightRoute) TryAddPropertyEntry(properties, IO.System.Route, IO.Property.Weight);
            }
            if (SystemZoning)
            {
                system |= IO.System.Zoning;
                feature |= IO.Feature.Zoning;
                if (GeometryBoundaryZoning) TryAddVectorKindEntry(vectorKinds, IO.System.Zoning, IO.VectorKind.Boundary);
                if (PropertyNameZoning) TryAddPropertyEntry(properties, IO.System.Zoning, IO.Property.Name);
                if (PropertyColorZoning) TryAddPropertyEntry(properties, IO.System.Zoning, IO.Property.Color);
                if (PropertyDensityZoning) TryAddPropertyEntry(properties, IO.System.Zoning, IO.Property.Density);
                if (PropertyObjectZoning) TryAddPropertyEntry(properties, IO.System.Zoning, IO.Property.Object);
                if (PropertyThemeZoning) TryAddPropertyEntry(properties, IO.System.Zoning, IO.Property.Theme);
                if (PropertyZoningZoning) TryAddPropertyEntry(properties, IO.System.Zoning, IO.Property.Zoning);
            }

            if (FeatureTerrain || FeatureWater) system |= IO.System.Raster;
            if (FeatureTerrain)
            {
                if (GeometryElevationTerrain) rasterKinds |= IO.RasterKind.Elevation;
                if (GeometryWorldElevationTerrain) rasterKinds |= IO.RasterKind.WorldElevation;
            }
            if (FeatureWater)
            {
                if (GeometryDepthWater) rasterKinds |= IO.RasterKind.Depth;
                if (GeometryWorldDepthWater) rasterKinds |= IO.RasterKind.WorldDepth;
            }

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
                    errors[GetOptionLabelLocaleID(nameof(SourceXCoord))] = Utils.IOUtils.TryGetLongitude(SourceXCoord, out sourceX);
                    errors[GetOptionLabelLocaleID(nameof(SourceYCoord))] = Utils.IOUtils.TryGetLatitude(SourceYCoord, out sourceY);
                    sourceCoordinates = new(sourceX, sourceY, sourceCRS);
                    break;
            }

            return new()
            {
                AssetPack = PropertyThemeAssetPack,
                Created = DateTime.Now,
                CompletionDialog = ShowCompletionDialog,
                CompletionSound = PlayCompletionSound,
                Display = new Dictionary<(IO.Property, IO.System), bool>
                {
                    { (IO.Property.Category, IO.System.Building), Utils.IOUtils.DisplayModeToBoolean(PropertyCategoryBuildingDisplayMode) },
                    { (IO.Property.Category, IO.System.Network),  Utils.IOUtils.DisplayModeToBoolean(PropertyCategoryNetworkDisplayMode) },
                    { (IO.Property.Category, IO.System.POI),      Utils.IOUtils.DisplayModeToBoolean(PropertyCategoryPOIDisplayMode) },
                    { (IO.Property.Object, IO.System.Unknown),    false },
                    { (IO.Property.Zoning, IO.System.Unknown),    Utils.IOUtils.DisplayModeToBoolean(PropertyZoningDisplayMode) }
                },
                Elevation = OutputElevation,
                Errors = errors,
                Features = feature,
                FileName = fileBaseFormat,
                FileNameFormat = ExportNamingFormat,
                GeoTiffFormat = ExportGeoTiffFormat,
                Homeless = PropertyGeneralHomeless,
                InactiveRoute = GeometryInactiveRoute,
                Minimized = OutputMinimizedGeoJSON,
                PetPassenger = PropertyPassengerPet,
                Properties = properties,
                RasterFormat = IO.FileFormat.GeoTIFF,
                RasterKinds = rasterKinds,
                RoadClassification = PropertyCategoryRoadClassification,
                SeparateResident = PropertyResidentSeparateBySex,
                SeparateServiceUpgrade = GeometrySeparateServiceUpgrade,
                SourceCoordinates = sourceCoordinates,
                SourceProjection = sourceCRS,
                SourceProjectionDefinition = projectionDefinition,
                StatisticsMapTile = PropertyGeneralMapTileStatistics,
                Systems = system,
                TargetEllipsoid = ellipsoid,
                TargetProjection = targetCRS,
                TargetProjectionDefinition = projectionDefinition,
                Taxable = PropertyWageTaxable,
                Unzoned = GeometryUnzoned,
                VectorFormat = vectorFileFormat,
                VectorKinds = vectorKinds,
                XtmAcronym = PropertyRouteXtm,
                ZccColor = PropertyColorZcc
            };
        }

        /// <summary>
        /// Try to add a <see cref="IO.Property"/> to the target library.
        /// （嘗試添加一個 <see cref="IO.Property"/> 至指定的字典中。）
        /// </summary>
        /// <param name="propertiesDictionary">The dictionary storing each vector system's properties.（儲存各向量系統屬性種類的字典。）</param>
        /// <param name="system">The system enum.（系統枚舉。）</param>
        /// <param name="property">The property category.（屬性分類。）</param>
        private void TryAddPropertyEntry(Dictionary<IO.System, HashSet<IO.Property>> propertiesDictionary, IO.System system, IO.Property property)
        {
            propertiesDictionary ??= new();

            if (propertiesDictionary.TryGetValue(system, out HashSet<IO.Property> previousProperties))
            {
                previousProperties.Add(property);
            }
            else
            {
                propertiesDictionary.Add(system, new() { property });
            }
        }

        /// <summary>
        /// Try to add a <see cref="IO.VectorKind"/> to the target dictionary.
        /// （嘗試添加一個 <see cref="IO.VectorKind"/> 至指定的字典中。）
        /// </summary>
        /// <param name="vectorKindsDictionary">The dictionary storing each vector system's vector kinds.（儲存各向量系統向量種類的字典。）</param>
        /// <param name="system">The system enum.（系統枚舉。）</param>
        /// <param name="vectorKind">The vector geometry type.（向量幾何類別。）</param>
        private void TryAddVectorKindEntry(Dictionary<IO.System, IO.VectorKind> vectorKindsDictionary, IO.System system, IO.VectorKind vectorKind)
        {
            vectorKindsDictionary ??= new();

            if (vectorKindsDictionary.TryGetValue(system, out IO.VectorKind previousVectorKind))
            {
                vectorKindsDictionary[system] = previousVectorKind | vectorKind;
            }
            else
            {
                vectorKindsDictionary.Add(system, vectorKind);
            }
        }
    }
}
