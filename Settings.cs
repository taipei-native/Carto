namespace Carto
{
    using Carto.Utils;
    using Colossal.IO.AssetDatabase;
    using Colossal.PSI.Environment;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Game.Settings;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The class that handle the mod's options.
    /// （處理模組設定選項的類別。）
    /// </summary>
    [FileLocation("ModsSettings" + "\\" + nameof(Carto) + "\\" + nameof(Carto))]
    [SettingsUITabOrder(GeneralTab, CustomExportTab, MiscellaneousTab)]
    [SettingsUIShowGroupName(ExportGroup, DangerGroup, SpatialFieldGroup, NonSpatialFieldGroup)]
    [SettingsUIGroupOrder(ExportGroup, DangerGroup, FeatureGroup, SelectorGroup, SpatialFieldGroup, NonSpatialFieldGroup, MiscellaneousGroup)]
    public class Setting : ModSetting
    {
        /// <summary>
        /// The constructor of Carto.Setting.
        /// （Carto.Setting 的建構函式。）
        /// </summary>
        /// <param name="mod">The mod instance.（模組實例。）</param>
        public Setting(IMod mod) : base(mod) { SetDefaults(); }

        public bool m_Address = true;
        public bool m_Area = true;
        public bool m_Asset = true;
        public bool m_Brand = true;
        public bool m_Category = true;
        public bool m_Center = true;
        public bool m_CenterLine = true;
        public bool m_Color = true;
        public bool m_Density = true;
        public bool m_Depth = true;
        public bool m_Direction = true;
        public bool m_Edge = true;
        public bool m_Employee = true;
        public bool m_Elevation = true;
        public bool m_Form = true;
        public bool m_Height = true;
        public bool m_Household = true;
        public bool m_Length = true;
        public bool m_Level = true;
        public bool m_Object = true;
        public bool m_Product = true;
        public bool m_Resident = true;
        public bool m_Theme = true;
        public bool m_Unlocked = true;
        public bool m_Width = true;
        public bool m_Zoning = true;
        public ExportUtils.FeatureType m_Feature;

        /// <summary>
        /// Gets or sets a value indicating whether: Used to force saving of Modsettings if settings would result in empty Json.
        /// （用以指示當前的狀態是否需要強制儲存模組設定（以確保不會產生空白JSON）的參數。）
        /// </summary>
        [SettingsUIHidden]
        public bool Contra { get; set; }

        /// <summary>
        /// Detects whether current file format supports raster data.
        /// （偵測目前的檔案格式是否支援網格資料。）
        /// </summary>
        [SettingsUIHidden]
        public bool IsRasterFormat => ExportFormat == ExportUtils.ExportFormats.GeoTIFF;

        /// <summary> 
        /// Detects whether current file format supports vector data.
        /// （偵測目前的檔案格式是否支援向量資料。）
        /// </summary>
        [SettingsUIHidden]
        public bool IsVectorFormat => ExportFormat != ExportUtils.ExportFormats.GeoTIFF;

        /// <summary> 
        /// Detects whether current file name is custom.
        /// （偵測目前的檔案格式是否支援向量資料。）
        /// </summary>
        [SettingsUIHidden]
        public bool NotCustomName => NamingFormat != ExportUtils.NamingFormat.Custom;

        /// <summary>
        /// Detects whether Zone Color Changer mod is loaded.
        /// （偵測 Zone Color Changer 模組是否被載入。）
        /// </summary>
        [SettingsUIHidden]
        public bool ZCCNotReady
        {
            get
            {
                return !GameManager.instance.modManager.ListModsEnabled().Any(mod => mod.StartsWith("ZoneColorChanger"));
            }
        }

        /// <summary>
        /// Detects whether current field is in the dictionary.
        /// （偵測目前的欄位是否在字典中。）
        /// </summary>
        /// <param name="fieldName">Field name.（欄位名稱。）</param>
        public bool NotInFieldArray(string fieldName) => !ExportFieldArray[FeatureSelector].Contains(fieldName);

        public bool NotInFieldArrayAddress => NotInFieldArray("Address");
        public bool NotInFieldArrayArea => NotInFieldArray("Area");
        public bool NotInFieldArrayAsset => NotInFieldArray("Asset");
        public bool NotInFieldArrayBrand => NotInFieldArray("Brand");
        public bool NotInFieldArrayCategory => NotInFieldArray("Category");
        public bool NotInFieldArrayCenter => NotInFieldArray("Center");
        public bool NotInFieldArrayCenterLine => NotInFieldArray("CenterLine");
        public bool NotInFieldArrayColor => NotInFieldArray("Color");
        public bool NotInFieldArrayDensity => NotInFieldArray("Density");
        public bool NotInFieldArrayDepth => NotInFieldArray("Depth");
        public bool NotInFieldArrayDirection => NotInFieldArray("Direction");
        public bool NotInFieldArrayEdge => NotInFieldArray("Edge");
        public bool NotInFieldArrayElevation => NotInFieldArray("Elevation");
        public bool NotInFieldArrayEmployee => NotInFieldArray("Employee");
        public bool NotInFieldArrayForm => NotInFieldArray("Form");
        public bool NotInFieldArrayHeight => NotInFieldArray("Height");
        public bool NotInFieldArrayHousehold => NotInFieldArray("Household");
        public bool NotInFieldArrayLength => NotInFieldArray("Length");
        public bool NotInFieldArrayLevel => NotInFieldArray("Level");
        public bool NotInFieldArrayObject => NotInFieldArray("Object");
        public bool NotInFieldArrayProduct => NotInFieldArray("Product");
        public bool NotInFieldArrayResident => NotInFieldArray("Resident");
        public bool NotInFieldArrayTheme => NotInFieldArray("Theme");
        public bool NotInFieldArrayUnlocked => NotInFieldArray("Unlocked");
        public bool NotInFieldArrayWidth => NotInFieldArray("Width");
        public bool NotInFieldArrayZoning => NotInFieldArray("Zoning");

        /// <summary>
        /// Detects whether current user interface is in either game or editor.
        /// （偵測目前的UI介面是否為遊戲或編輯器內。）
        /// </summary>
        [SettingsUIHidden]
        public bool NotInGameOrEditor => !GameMode.GameOrEditor.HasFlag(Instance.Mode);

        /// <summary>
        /// All availible fields for each feature type.
        /// （對於各個圖徵類型，所有可使用的欄位。）
        /// </summary>
        [SettingsUIHidden]
        public Dictionary<ExportUtils.FeatureType, string[]> ExportFieldArray => MiscUtils.DeepCopy(ExportUtils.ExportFieldSettings).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Keys.ToArray());

        /// <summary>
        /// The error code encountered during exporting.
        /// （輸出時遇到的錯誤代碼。）
        /// </summary>
        [SettingsUIHidden]
        public static ExportUtils.ExportErrors ExportError { get; set; } = 0;

        /// <summary>
        /// The WGS84 coordinate of the map center.
        /// （地圖中心的 WGS84 坐標。）
        /// </summary>
        [SettingsUIHidden]
        public static (double Lat, double Lon) MapCenter { get; set; } = (0, 0);

        /// <summary>
        /// The path to the content folder location.
        /// （指向內容資料夾的路徑。）
        /// </summary>
        [SettingsUIHidden]
        public static string ContentFolder => MiscUtils.CombinePath(EnvPath.kUserDataPath, "ModsData", nameof(Carto));

        /// <summary>
        /// The path to the execute file folder location.
        /// （指向執行檔案資料夾的路徑。）
        /// </summary>
        [SettingsUIHidden]
        public static string ExecuteFolder => MiscUtils.CombinePath(EnvPath.kUserDataPath, "Mods", nameof(Carto));

        /// <summary>
        /// The path to the setting folder location.
        /// （指向設定資料夾的路徑。）
        /// </summary>
        [SettingsUIHidden]
        public static string SettingFolder => MiscUtils.CombinePath(EnvPath.kUserDataPath, "ModsSettings", nameof(Carto));

        /// <summary>
        /// Load the previous field status and set Contra to prevent producing invalid settings file.
        /// （載入先前的欄位狀態，並且設置 Contra 以防止產生無效的設定檔案。）
        /// </summary>
        public void LoadLocalSettings()
        {
            Contra = false;
            UpdateFieldVisual(FeatureSelector);
        }

        /// <summary>
        /// Update the field status both in the dictionary and the serialized JSON string.
        /// （更新字典與序列化JSON字串中的欄位狀態。）
        /// </summary>
        /// <param name="fieldName">The field name.（欄位名稱。）</param>
        /// <param name="newValue">The new field value.（新欄位值。）</param>
        public void UpdateFieldStatus(string fieldName, bool newValue)
        {
            if (ExportFieldArray[FeatureSelector].Contains(fieldName))
            {
                GetType().GetField($"m_{fieldName}").SetValue(this, newValue);
                FieldStatus[FeatureSelector][fieldName] = newValue;
                Instance.Log.Verbose($"Setting.UpdateFieldStatus(): FieldStatus[{FeatureSelector}][{fieldName}] set to `{newValue}`");
                ApplyAndSave();
            }
            else
            {
                Instance.Log.Debug($"Attempt to modify `{fieldName}` failed since it didn't belong to current feature type ({FeatureSelector});  試圖改變 `{fieldName}` 的嘗試失敗了，因為它不屬於目前的圖徵類型（{FeatureSelector}）。");
            }
        }

        /// <summary>
        /// Update the field status when changing feature type in FieldSelector.
        /// （當FieldSelector的圖徵類型改變時，更新欄位的狀態。）
        /// </summary>
        public void UpdateFieldVisual(ExportUtils.FeatureType featureType)
        {
            Instance.Log.Verbose($"Setting.UpdateFieldVisual(): FieldSelector set to `{featureType}`");
            
            foreach (KeyValuePair<string, bool> field in FieldStatus[featureType].ToDictionary(entry => entry.Key, entry => entry.Value))
            {
                GetType().GetField($"m_{field.Key}").SetValue(this, field.Value);
                Instance.Log.Verbose($"{new string(' ', 28)} Configurate m_{field.Key}");
                ApplyAndSave();
            }
        }

        public const string GeneralTab = "GeneralTab";
        public const string ExportGroup = "ExportGroup";
        public const string ExportButtons = "ExportButtons";
        public const string DangerGroup = "DangerGroup";
        public const string CustomExportTab = "CustomExportTab";
        public const string FeatureGroup = "FeatureGroup";
        public const string SelectorGroup = "SelectorGroup";
        public const string SpatialFieldGroup = "SpatialFieldGroup";
        public const string NonSpatialFieldGroup = "NonSpatialFieldGroup";
        public const string MiscellaneousTab = "MiscellaneousTab";
        public const string MiscellaneousGroup = "MiscellaneousGroup";

        /// <summary>
        /// The user-customized exported file format.
        /// （使用者自訂的輸出檔案格式。）
        /// </summary>
        [SettingsUISection(GeneralTab, ExportGroup)]
        public ExportUtils.ExportFormats ExportFormat { get; set; } = ExportUtils.ExportFormats.Shapefile;

        /// <summary>
        /// The user-customized exported file naming format.
        /// （使用者自訂的輸出檔案命名格式。）
        /// </summary>
        [SettingsUISection(GeneralTab, ExportGroup)]
        public ExportUtils.NamingFormat NamingFormat { get; set; } = ExportUtils.NamingFormat.Feature;

        /// <summary>
        /// The user-customized file naming format.
        /// （使用者自訂的檔案命名格式。）
        /// </summary>
        [SettingsUITextInput]
        [SettingsUISection(GeneralTab, ExportGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotCustomName))]
        public string CustomFileName { get; set; } = string.Empty;

        /// <summary>
        /// The user-customized map center latitude.
        /// （使用者自訂的地圖中心緯度。）
        /// </summary>
        [SettingsUITextInput]
        [SettingsUISection(GeneralTab, ExportGroup)]
        public string Latitude { get; set; } = "0";

        /// <summary>
        /// The user-customized map center longitude.
        /// （使用者自訂的地圖中心經度。）
        /// </summary>
        [SettingsUITextInput]
        [SettingsUISection(GeneralTab, ExportGroup)]
        public string Longitude { get; set; } = "0";

        /// <summary>
        /// The button to reveal the output directory.
        /// （顯示輸出目錄的按鈕。）
        /// </summary>
        [SettingsUIButton]
        [SettingsUISection(GeneralTab, ExportGroup)]
        [SettingsUIButtonGroup(ExportButtons)]
        public bool OpenButton
        {
            set
            {
                MiscUtils.RevealInFileExplorer(ContentFolder);
            }
        }

        /// <summary>
        /// The button to trigger export.
        /// （觸發輸出事件的按鈕。）
        /// </summary>
        [SettingsUIButton]
        [SettingsUISection(GeneralTab, ExportGroup)]
        [SettingsUIButtonGroup(ExportButtons)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(NotInGameOrEditor))]
        public bool ExportButton
        {
            set { ExportUtils.ExportToFile(); }
        }

        /// <summary>
        /// The button to enable all fields to be exportted.
        /// （用以啟用所有欄位輸出狀態的按鈕。）
        /// </summary>
        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUISection(GeneralTab, DangerGroup)]
        public bool EnableAllButton
        {
            set
            {
                var duplicate = MiscUtils.DeepCopy(FieldStatus);

                foreach (KeyValuePair<ExportUtils.FeatureType, Dictionary<string, bool>> dict in duplicate)
                {
                    foreach (string key in new List<string>(dict.Value.Keys))
                    {
                        dict.Value[key] = true;
                    }
                }

                FieldStatus = MiscUtils.DeepCopy(duplicate);
                ApplyAndSave();
                UpdateFieldVisual(FeatureSelector);
            }
        }

        /// <summary>
        /// The button to reset settings.
        /// （用以重置設定的按鈕。）
        /// </summary>
        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUISection(GeneralTab, DangerGroup)]
        public bool ResetButton
        {
            set
            {
                SetDefaults();
                Contra = !Contra;
                ApplyAndSave();
                UpdateFieldVisual(FeatureSelector);
            }
        }

        /// <summary>
        /// Whether to export area features.
        /// （決定是否輸出區域圖徵。）
        /// </summary>
        [SettingsUISection(CustomExportTab, FeatureGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsRasterFormat))]
        public bool ExportArea { get; set; } = true;

        /// <summary>
        /// Whether to export building features.
        /// （決定是否輸出建築圖徵。）
        /// </summary>
        [SettingsUISection(CustomExportTab, FeatureGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsRasterFormat))]
        public bool ExportBuilding { get; set; } = true;

        /// <summary>
        /// Whether to export network features.
        /// （決定是否輸出網絡圖徵。）
        /// </summary>
        [SettingsUISection(CustomExportTab, FeatureGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsRasterFormat))]
        public bool ExportNet { get; set; } = true;

        /// <summary>
        /// Whether to export terrain.
        /// （決定是否輸出地形。）
        /// </summary>
        [SettingsUISection(CustomExportTab, FeatureGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsVectorFormat))]
        public bool ExportTerrain { get; set; } = true;

        /// <summary>
        /// Whether to export water bodies.
        /// （決定是否輸出水體。）
        /// </summary>
        [SettingsUISection(CustomExportTab, FeatureGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsVectorFormat))]
        public bool ExportWater { get; set; } = true;

        /// <summary>
        /// Whether to export zonings.
        /// （決定是否輸出分區。）
        /// </summary>
        [SettingsUISection(CustomExportTab, FeatureGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsRasterFormat))]
        public bool ExportZoning { get; set; } = true;

        /// <summary>
        /// Field export settings.
        /// （欄位輸出設定。）
        /// </summary>
        [SettingsUIHidden]
        public Dictionary<ExportUtils.FeatureType, Dictionary<string, bool>> FieldStatus { get; set; } = MiscUtils.DeepCopy(ExportUtils.ExportFieldSettings);

        /// <summary>
        /// The dropdown to configurate each individual field's exporting status of every feature type.
        /// （控制每個圖徵類別的個別欄位輸出狀態的下拉式選單。）
        /// </summary>
        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, SelectorGroup)]
        public ExportUtils.FeatureType FeatureSelector
        {
            get => m_Feature;
            set { m_Feature = value; UpdateFieldVisual(value); }
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, SpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayCenterLine))]
        public bool ExportFieldCenterLine
        {
            get => m_CenterLine;
            set => UpdateFieldStatus("CenterLine", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, SpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayDepth))]
        public bool ExportFieldDepth
        {
            get => m_Depth;
            set => UpdateFieldStatus("Depth", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, SpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayEdge))]
        public bool ExportFieldEdge
        {
            get => m_Edge;
            set => UpdateFieldStatus("Edge", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, SpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayElevation))]
        public bool ExportFieldElevation
        {
            get => m_Elevation;
            set => UpdateFieldStatus("Elevation", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayAddress))]
        public bool ExportFieldAddress
        {
            get => m_Address;
            set => UpdateFieldStatus("Address", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof (Setting), nameof(NotInFieldArrayArea))]
        public bool ExportFieldArea
        {
            get => m_Area;
            set => UpdateFieldStatus("Area", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayAsset))]
        public bool ExportFieldAsset
        {
            get => m_Asset;
            set => UpdateFieldStatus("Asset", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayBrand))]
        public bool ExportFieldBrand
        {
            get => m_Brand;
            set => UpdateFieldStatus("Brand", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayCategory))]
        public bool ExportFieldCategory
        {
            get => m_Category;
            set => UpdateFieldStatus("Category", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayCenter))]
        public bool ExportFieldCenter
        {
            get => m_Center;
            set => UpdateFieldStatus("Center", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayColor))]
        public bool ExportFieldColor
        {
            get => m_Color;
            set => UpdateFieldStatus("Color", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayDensity))]
        public bool ExportFieldDensity
        {
            get => m_Density;
            set => UpdateFieldStatus("Density", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayDirection))]
        public bool ExportFieldDirection
        {
            get => m_Direction;
            set => UpdateFieldStatus("Direction", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayEmployee))]
        public bool ExportFieldEmployee
        {
            get => m_Employee;
            set => UpdateFieldStatus("Employee", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayForm))]
        public bool ExportFieldForm
        {
            get => m_Form;
            set => UpdateFieldStatus("Form", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayHeight))]
        public bool ExportFieldHeight
        {
            get => m_Height;
            set => UpdateFieldStatus("Height", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayHousehold))]
        public bool ExportFieldHousehold
        {
            get => m_Household;
            set => UpdateFieldStatus("Household", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayLength))]
        public bool ExportFieldLength
        {
            get => m_Length;
            set => UpdateFieldStatus("Length", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayLevel))]
        public bool ExportFieldLevel
        {
            get => m_Level;
            set => UpdateFieldStatus("Level", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayObject))]
        public bool ExportFieldObject
        {
            get => m_Object;
            set => UpdateFieldStatus("Object", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayProduct))]
        public bool ExportFieldProduct
        {
            get => m_Product;
            set => UpdateFieldStatus("Product", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayResident))]
        public bool ExportFieldResident
        {
            get => m_Resident;
            set => UpdateFieldStatus("Resident", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayTheme))]
        public bool ExportFieldTheme
        {
            get => m_Theme;
            set => UpdateFieldStatus("Theme", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayUnlocked))]
        public bool ExportFieldUnlocked
        {
            get => m_Unlocked;
            set => UpdateFieldStatus("Unlocked", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayWidth))]
        public bool ExportFieldWidth
        {
            get => m_Width;
            set => UpdateFieldStatus("Width", value);
        }

        [SettingsUIAdvanced]
        [SettingsUISection(CustomExportTab, FeatureGroup, NonSpatialFieldGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(NotInFieldArrayZoning))]
        public bool ExportFieldZoning
        {
            get => m_Zoning;
            set => UpdateFieldStatus("Zoning", value);
        }

        /// <summary>
        /// Whether to export unzoned zoning cells.
        /// （決定是否要輸出未分區的分區單元。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousGroup)]
        public bool UseUnzoned { get; set; } = false;

        /// <summary>
        /// Whether to export the colors from Zone Color Changer mod instead of vanilla's.
        /// （決定是否要輸出 Zone Color Changer 模組的顏色，而非遊戲原本的顏色。）
        /// </summary>
        [SettingsUISection(MiscellaneousTab, MiscellaneousGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(ZCCNotReady))]
        public bool UseZCC { get; set; } = true;

        /// <summary>
        /// This is the method to reset settings.
        /// （這是用來重置設定的方法。）
        /// </summary>
        public override void SetDefaults()
        {
            Contra = true;
            CustomFileName = string.Empty;
            ExportArea = true;
            ExportBuilding = true;
            ExportError = ExportUtils.ExportErrors.None;
            ExportFormat = ExportUtils.ExportFormats.Shapefile;
            ExportNet = true;
            ExportTerrain = true;
            ExportWater = true;
            ExportZoning = true;
            FeatureSelector = ExportUtils.FeatureType.Area;
            FieldStatus = MiscUtils.DeepCopy(ExportUtils.ExportFieldSettings);
            Latitude = "0";
            Longitude = "0";
            m_Address = true;
            m_Area = true;
            m_Asset = true;
            m_Brand = true;
            m_Category = true;
            m_Center = true;
            m_CenterLine = true;
            m_Color = true;
            m_Density = true;
            m_Depth = true;
            m_Direction = true;
            m_Edge = true;
            m_Elevation = true;
            m_Employee = true;
            m_Form = true;
            m_Height = true;
            m_Household = true;
            m_Length = true;
            m_Level = true;
            m_Object = true;
            m_Product = true;
            m_Resident = true;
            m_Theme = true;
            m_Unlocked = true;
            m_Width = true;
            m_Zoning = true;
            MapCenter = (0, 0);
            NamingFormat = ExportUtils.NamingFormat.Feature;
            UseUnzoned = false;
            UseZCC = true;
        }
    }
}