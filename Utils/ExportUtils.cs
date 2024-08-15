namespace Carto.Utils
{
    using Carto.Domain;
    using Carto.Geodata;
    using Carto.Systems;
    using Colossal.Logging;
    using Game.UI;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Unity.Mathematics;

    /// <summary>
    /// The class to store export properties.
    /// （用以儲存輸出屬性的類別。）
    /// </summary>
    public class ExportUtils
    {
        private static readonly ILog m_Log = Instance.Log;

        /// <summary>
        /// The enum storing the export errors.
        /// （儲存輸出錯誤的列舉。）
        /// </summary>
        [Flags]
        public enum ExportErrors
        {
            None = 0,
            Latitude = 1,
            Longitude = 2,
            ShareViolation = 4,
            MissingSpatialFields = 8
        }

        /// <summary>
        /// The enum storing the exported file format.
        /// （儲存輸出檔案格式的列舉。）
        /// </summary>
        public enum ExportFormats
        {
            GeoJSON,
            GeoTIFF,
            Shapefile
        }

        /// <summary>
        /// The enum storing the reason why the export is ignored.
        /// （儲存為何輸出被忽略原因的列舉。）
        /// </summary>
        public enum ExportIgnoreReasons
        {
            None,
            Unknown,
            NoMatch
        }

        /// <summary>
        /// The enum storing the feature types.
        /// （儲存圖徵類別的列舉。）
        /// </summary>
        public enum FeatureType
        {
            Area,
            Building,
            Net,
            Terrain,
            Water,
            Zoning
        }

        /// <summary>
        /// The enum storing the geometry types.
        /// （儲存幾何類型的列舉。）
        /// </summary>
        public enum GeometryType
        {
            None,
            Point,
            LineString,
            Polygon,
            Raster
        }

        /// <summary>
        /// The enum storing the availible GeoTIFF formats.
        /// （儲存GeoTIFF格式的列舉。）
        /// </summary>
        public enum GeoTIFFFormat
        {
            Int16,
            Norm16,
            Float32
        }

        /// <summary>
        /// The enum storing the file naming format.
        /// （儲存檔案命名格式的列舉。）
        /// </summary>
        public enum NamingFormat
        {
            Custom,
            Feature,
            CityName_Feature,
            MapName_Feature,
        }

        /// <summary>
        /// The default field export settings.
        /// （預設的欄位輸出設定。）
        /// </summary>
        public static Dictionary<FeatureType, Dictionary<string, bool>> ExportFieldSettings = new Dictionary<FeatureType, Dictionary<string, bool>>
        {
            { FeatureType.Area, new Dictionary<string, bool>{ { "Area", false }, { "Center", false }, { "Company", false }, { "Edge", true }, { "Employee", true }, { "Household", false }, { "Object", true }, { "Resident", true }, { "Unlocked", true } }},
            { FeatureType.Building, new Dictionary<string, bool>{ { "Address", true }, { "Asset", true }, { "Brand", false }, { "Category", true }, { "Edge", true }, { "Employee", true }, { "Household", false }, { "Level", false }, { "Object", true }, { "Product", false }, { "Resident", true }, { "Theme", false }, { "Zoning", true }  }},
            { FeatureType.Net, new Dictionary<string, bool> { { "Asset", true }, { "Category", true }, { "CenterLine", true }, { "Direction", true }, { "Edge", true }, { "Form", true }, { "Height", false }, { "Length", false }, { "Object", true }, { "Width", false } }},
            { FeatureType.Terrain, new Dictionary<string, bool> { { "Elevation", true }, { "WorldElevation", false } }},
            { FeatureType.Water, new Dictionary<string, bool> { { "Depth", true }, { "WorldDepth", false } }},
            { FeatureType.Zoning, new Dictionary<string, bool> { { "Color", false }, { "Density", false }, { "Edge", true }, { "Object", true }, { "Theme", false }, { "Zoning", true } }}
        };

        /// <summary>
        /// The default field types.
        /// （預設的欄位型別。）
        /// </summary>
        public static Dictionary<string, Type> ExportFieldTypes = new Dictionary<string, Type>
        {
            { "Area", typeof(float) },
            { "Address", typeof(Address) },
            { "Address_district", typeof(string) },
            { "Address_street", typeof(string) },
            { "Address_number", typeof(int) },
            { "Asset", typeof(string) },
            { "Brand", typeof(string) },
            { "Category", typeof(string) },
            { "Center", typeof(float3) },
            { "Color", typeof(string) },
            { "Company", typeof(int) },
            { "Density", typeof(string) },
            { "Direction", typeof(string) },
            { "Employee", typeof(int) },
            { "Form", typeof(string) },
            { "Height", typeof(float) },
            { "Household", typeof(int) },
            { "Length", typeof(float) },
            { "Level", typeof(short) },
            { "Name", typeof(string) },
            { "Object", typeof(string) },
            { "Product", typeof(string) },
            { "Resident", typeof(int) },
            { "Theme", typeof(string) },
            { "Unlocked", typeof(bool) },
            { "Width", typeof(float) },
            { "Zoning", typeof(string) }
        };

        /// <summary>
        /// The features available in the raster type.
        /// （適用於網格類型的圖徵。）
        /// </summary>
        public static FeatureType[] RasterFeatures => new FeatureType[] { FeatureType.Terrain, FeatureType.Water};

        /// <summary>
        /// The title of spatial fields.
        /// （空間欄位的名稱。）
        /// </summary>
        public static Dictionary<FeatureType, string[]> SpatialFields => new Dictionary<FeatureType, string[]>
        {
            { FeatureType.Area,     new string[] { "Edge" } },
            { FeatureType.Building, new string[] { "Edge" } },
            { FeatureType.Net,      new string[] { "CenterLine", "Edge" } },
            { FeatureType.Terrain,  new string[] { "Elevation", "WorldElevation" } },
            { FeatureType.Water,    new string[] { "Depth", "WorldDepth" } },
            { FeatureType.Zoning,   new string[] { "Edge" } },
        };

        /// <summary>
        /// The features available in the vector type.
        /// （適用於向量類型的圖徵。）
        /// </summary>
        public static FeatureType[] VectorFeatures => new FeatureType[] { FeatureType.Area, FeatureType.Building, FeatureType.Net, FeatureType.Zoning };

        /// <summary>
        /// The method handling the validation of settings and export of the files.
        /// （處理驗證設定與輸出檔案的方法。）
        /// </summary>
        public static void ExportToFile()
        {
            bool anyIgnored = false;
            Dictionary<ExportIgnoreReasons, int> ignored_count_helper = new Dictionary<ExportIgnoreReasons, int>();
            Dictionary<ExportIgnoreReasons, List<string>> ignored_name_helper = new Dictionary<ExportIgnoreReasons, List<string>>();
            string cityName = MiscUtils.RemoveInvalidChars(Instance.CityName);
            string isoDate = Instance.Time.GetCurrentDateTime().ToString("yyyy-dd", CultureInfo.InvariantCulture);
            string mapName = MiscUtils.RemoveInvalidChars(Instance.MapName);
            string time = Instance.Time.GetCurrentDateTime().ToString("HHmm");

            void ExportRasterGeodata(CartoObject obj, Geodata input, string type)
            {
                if (obj.Values.Keys.Count > 1)
                {
                    foreach (string field in obj.Values.Keys)
                    {
                        ExportToFormat(input, GetFileName($"{type}_{field}"), field, out _, out _, out _);
                    }
                }
                else if (obj.Values.Keys.Count == 1)
                {
                    ExportToFormat(input, GetFileName(type), obj.Values.Keys.First(), out _, out _, out _);
                }
                else
                {
                    FeatureType typeEnum = (FeatureType) Enum.Parse(typeof(FeatureType), type, true);
                    List<string> enabledFields = Instance.Settings.FieldStatus[typeEnum].Where(kvp => kvp.Value & SpatialFields[typeEnum].ToList().Contains(kvp.Key)).Select(kvp => kvp.Key).ToList();

                    if (enabledFields.Count > 1)
                    {
                        foreach (string field in enabledFields)
                        {
                            ExportToFormat(input, GetFileName($"{type}_{field}"), "", out int ignored_count, out string ignored_name, out ExportIgnoreReasons ignore_reason);
                            if (ignored_count > 0)
                            {
                                ignored_count_helper[ignore_reason] = ignored_count_helper.TryGetValue(ignore_reason, out int num) ? num + ignored_count : ignored_count;
                                ignored_name_helper[ignore_reason] = ignored_name_helper.TryGetValue(ignore_reason, out List<string> file) ? file.Concat(new List<string> { ignored_name }).ToList() : new List<string> { ignored_name };
                            }
                        }
                    }
                    else
                    {
                        ExportToFormat(input, GetFileName(type), "", out int ignored_count, out string ignored_name, out ExportIgnoreReasons ignore_reason);
                        if (ignored_count > 0)
                        {
                            ignored_count_helper[ignore_reason] = ignored_count_helper.TryGetValue(ignore_reason, out int num) ? num + ignored_count : ignored_count;
                            ignored_name_helper[ignore_reason] = ignored_name_helper.TryGetValue(ignore_reason, out List<string> file) ? file.Concat(new List<string> { ignored_name }).ToList() : new List<string> { ignored_name };
                        }
                    }
                }
            }

            void ExportVectorGeodata(List<CartoObject> objs, Geodata input, string type, bool suppressFieldError = false)
            {
                List<string> fields = objs.SelectMany(obj => obj.Edges.Keys.ToList()).Distinct().ToList();

                if (fields.Count > 1)
                {
                    foreach (string field in fields)
                    {
                        ExportToFormat(input, GetFileName($"{type}_{field}"), field, out _, out _, out _, suppressFieldError);
                    }
                }
                else if (fields.Count == 1)
                {
                    ExportToFormat(input, GetFileName(type), fields[0], out _, out _, out _, suppressFieldError);
                }
                else
                {
                    FeatureType typeEnum = (FeatureType)Enum.Parse(typeof(FeatureType), type, true);
                    List<string> enabledFields = Instance.Settings.FieldStatus[typeEnum].Where(kvp => kvp.Value & SpatialFields[typeEnum].ToList().Contains(kvp.Key)).Select(kvp => kvp.Key).ToList();

                    if (enabledFields.Count > 1)
                    {
                        foreach (string field in enabledFields)
                        {
                            ExportToFormat(input, GetFileName($"{type}_{field}"), "", out int ignored_count, out string ignored_name, out ExportIgnoreReasons ignore_reason, suppressFieldError);
                            if (ignored_count > 0)
                            {
                                ignored_count_helper[ignore_reason] = ignored_count_helper.TryGetValue(ignore_reason, out int num) ? num + ignored_count : ignored_count;
                                ignored_name_helper[ignore_reason] = ignored_name_helper.TryGetValue(ignore_reason, out List<string> file) ? file.Concat(new List<string> { ignored_name }).ToList() : new List<string> { ignored_name };
                            }
                        }
                    }
                    else
                    {
                        ExportToFormat(input, GetFileName(type), "", out int ignored_count, out string ignored_name, out ExportIgnoreReasons ignore_reason, suppressFieldError);
                        if (ignored_count > 0)
                        {
                            ignored_count_helper[ignore_reason] = ignored_count_helper.TryGetValue(ignore_reason, out int num) ? num + ignored_count : ignored_count;
                            ignored_name_helper[ignore_reason] = ignored_name_helper.TryGetValue(ignore_reason, out List<string> file) ? file.Concat(new List<string> { ignored_name }).ToList() : new List<string> { ignored_name };
                        }
                    }
                }
            }

            void ExportToFormat(Geodata input, string fileName, string target, out int ignored_count, out string ignored_name, out ExportIgnoreReasons ignored_reason, bool suppressFieldError = false)
            {
                ignored_count = default;
                ignored_name = default;
                ignored_reason = default;
                
                if (input.Objects.Count > 0)
                {
                    switch (Instance.Settings.ExportFormat)
                    {
                        case ExportFormats.GeoJSON:
                            input.ToGeoJSON($"{fileName}.json", target, suppressFieldError);
                            m_Log.Info($"File exported. 已輸出檔案。({fileName}.json)");
                            break;

                        //case ExportFormats.GeoPackage:
                        //    input.ToGeoPackage($"{fileName}.gkpg"); NOT Implement yet! （*尚未* 實作！）
                        //    break;

                        case ExportFormats.GeoTIFF:
                            int depth = Instance.Settings.TIFFFormat == GeoTIFFFormat.Float32 ? 32 : 16;
                            input.ToGeoTIFF($"{fileName}.tif", target, depth);
                            m_Log.Info($"File exported. 已輸出檔案。({fileName}.tif)");
                            break;

                        case ExportFormats.Shapefile:
                            input.ToShapefile($"{fileName}.shp", target, suppressFieldError);
                            m_Log.Info($"File exported. 已輸出檔案。({fileName}.shp)");
                            break;
                    }
                }
                else
                {
                    anyIgnored = true;
                    ignored_reason = ExportIgnoreReasons.NoMatch;

                    switch (Instance.Settings.ExportFormat)
                    {
                        case ExportFormats.GeoJSON:
                            m_Log.Warn($"File ignored. 已忽略檔案。({fileName}.json)");
                            ignored_count = 1;
                            ignored_name = $"{fileName}.json";
                            break;

                        case ExportFormats.GeoTIFF:
                            m_Log.Warn($"File ignored. 已忽略檔案。({fileName}.tif)");
                            ignored_count = 1;
                            ignored_name = $"{fileName}.tif";
                            break;

                        case ExportFormats.Shapefile:
                            m_Log.Warn($"File ignored. 已忽略檔案。({fileName}.shp)");
                            ignored_count = 5;
                            ignored_name = $"{fileName}.shp";
                            break;
                    }
                }
            }

            string GetFileName(string feature, bool verbose = true)
            {
                switch (Instance.Settings.NamingFormat)
                {
                    case NamingFormat.Custom:
                        string customName = MiscUtils.RemoveInvalidChars(Instance.Settings.CustomFileName.Replace("{City}", cityName).Replace("{Map}", mapName).Replace("{Feature}", feature).Replace("{Date}", isoDate).Replace("{Time}", time));
                        if ((customName != null) && (customName != string.Empty)) return customName;
                        if (verbose) m_Log.Info($"The custom file name is invalid, changing to the default name. 自訂檔案名無效，改為使用預設名稱。");
                        return feature;

                    case NamingFormat.Feature:
                        return feature;

                    case NamingFormat.CityName_Feature:
                        if ((cityName != null) && (cityName != string.Empty)) return $"{cityName}_{feature}";
                        return $"{LocaleUtils.Translate("Carto.export.UNKNOWN[City]")}_{feature}";

                    case NamingFormat.MapName_Feature:
                        if ((mapName != null) && (mapName != string.Empty)) return $"{mapName}_{feature}";
                        return $"{LocaleUtils.Translate("Carto.export.UNKNOWN[Map]")}_{feature}";

                    default:
                        return feature;
                }
            }

            List<string> GetSettingsString()
            {
                List<string> fieldSettings = new List<string>();
                FeatureType[] systems = Enum.GetValues(typeof(FeatureType)).Cast<FeatureType>().ToArray();
                systems = (Instance.Settings.ExportFormat == ExportFormats.GeoTIFF) ? systems.Where(s => RasterFeatures.Contains(s)).ToArray() : systems.Where(s => VectorFeatures.Contains(s)).ToArray();

                for (int i = 0; i < systems.Length; i++)
                {
                    string system = systems[i].ToString().ToUpper();
                    string titleSpaces = new string(' ', 14 - system.Length);
                    bool systemStatus  = (bool) Instance.Settings.GetType().GetProperty($"Export{systems[i]}").GetValue(Instance.Settings, null);
                    fieldSettings.Add($"{system}{titleSpaces}{systemStatus}");

                    if (systemStatus)
                    {
                        string[] fieldNames = Instance.Settings.ExportFieldArray[systems[i]];

                        for (int j = 0; j < fieldNames.Length; j++)
                        {
                            string field = (fieldNames[j].Length > 11) ? $"{fieldNames[j].Substring(0, 9)}..".ToLower() : fieldNames[j].ToLower();
                            string fieldSpaces = new string(' ', 12 - field.Length);
                            string fieldStatus = Instance.Settings.FieldStatus[systems[i]][fieldNames[j]] ? "-" : "False";
                            string decoration = (j == fieldNames.Length - 1) ? "└ " : "├ ";
                            fieldSettings.Add($"{decoration}{field}{fieldSpaces}{fieldStatus}");
                        }
                    }
                    else
                    {
                        fieldSettings.Add("└ (omitted)   -");
                    }

                    if (i != systems.Length - 1) fieldSettings.Add(string.Empty);
                }

                return fieldSettings;
            }

            bool GuaranteeFileAccess(out List<string> accessFailed, out List<string> noSpatialFields, out int fileCount)
            {
                accessFailed = new List<string>();
                noSpatialFields = new List<string>();
                List<string> extension = new List<string>();
                fileCount = 0;
                string root = string.Empty;
                bool result = true;

                switch (Instance.Settings.ExportFormat)
                {
                    case ExportFormats.GeoJSON:
                        extension.Add("json");
                        root = Path.Combine(Setting.ContentFolder, "GeoJSON");
                        break;
                    
                    //case ExportFormats.GeoPackage:

                    case ExportFormats.GeoTIFF:
                        extension.Add("tif");
                        root = Path.Combine(Setting.ContentFolder, "GeoTIFF");
                        break;
                    
                    case ExportFormats.Shapefile:
                        extension.AddRange(new List<string> { "cpg", "dbf", "prj", "shp", "shx" });
                        root = Path.Combine(Setting.ContentFolder, "Shapefile");
                        break;
                }

                FeatureType[] features = Enum.GetValues(typeof(FeatureType)).Cast<FeatureType>().ToArray();
                features = (Instance.Settings.ExportFormat == ExportFormats.GeoTIFF) ? features.Where(s => RasterFeatures.Contains(s)).ToArray() : features.Where(s => VectorFeatures.Contains(s)).ToArray();

                foreach (FeatureType feature in features)
                {
                    if ((bool) Instance.Settings.GetType().GetProperty($"Export{feature}").GetValue(Instance.Settings, null))
                    {
                        string[] validFields = SpatialFields[feature].Where(f => Instance.Settings.FieldStatus[feature][f]).ToArray();

                        if (validFields.Length > 0)
                        {
                            foreach (string field in validFields)
                            {
                                string fileNameFormat = validFields.Length == 1 ? $"{feature}" : $"{feature}_{field}";
                                string[] fileNames = extension.Select(ext => Path.Combine(root, $"{GetFileName(fileNameFormat, false)}.{ext}")).ToArray();
                                
                                foreach(string fileName in fileNames)
                                {
                                    if (MiscUtils.IsFileLocked(fileName))
                                    {
                                        result = false;
                                        accessFailed.Add(fileName);
                                    }

                                    fileCount ++;
                                }
                            }
                        }
                        else
                        {
                            noSpatialFields.Add(feature.ToString());
                        }
                    }
                }

                return result;
            }

            m_Log.Info(new string('=', 30));
            string version = Instance.Version;
            string versionSpaces = new string(' ', 15 - version.Length);
            m_Log.Info($"Export Settings{versionSpaces}{version}");
            m_Log.Info(new string('-', 30));
            m_Log.Info($"FORMAT        {Instance.Settings.ExportFormat}");
            if (Instance.Settings.NamingFormat != NamingFormat.Custom) m_Log.Info($"NAMING        {Instance.Settings.NamingFormat}");
            if (Instance.Settings.NamingFormat == NamingFormat.Custom) m_Log.Info($"NAMING        Custom ({Instance.Settings.CustomFileName})");
            m_Log.Info($"LATITUDE      {Instance.Settings.Latitude}");
            m_Log.Info($"LONGITUDE     {Instance.Settings.Longitude}");
            m_Log.Info($"TIFF_FORMAT   {Instance.Settings.TIFFFormat}");
            m_Log.Info($"HOMELESS      {Instance.Settings.UseHomeless}");
            m_Log.Info($"UNZONED       {Instance.Settings.UseUnzoned}");
            m_Log.Info($"ZCC_COLORS    {Instance.Settings.UseZCC}");
            m_Log.Info(new string('-', 30));
            m_Log.Info($"RB            {Instance.RBMod.Status()}");
            m_Log.Info($"ZCC           {Instance.ZCCMod.Status()}");
            m_Log.Info(new string('-', 30));
            GetSettingsString().ForEach(line => m_Log.Info(line));
            m_Log.Info(new string('=', 30));

            Setting.ExportError = default;

            if (!MiscUtils.TryGetLatitude(Instance.Settings.Latitude, out double lat))
            {
                Setting.ExportError |= ExportErrors.Latitude;
                m_Log.Warn($"Unable to parse latitude string.  無法解析緯度字串。(\"{Instance.Settings.Latitude}\")");
            }
            if (!MiscUtils.TryGetLongtitude(Instance.Settings.Longitude, out double lon))
            {
                Setting.ExportError |= ExportErrors.Longitude;
                m_Log.Warn($"Unable to parse longitude string. 無法解析經度字串。(\"{Instance.Settings.Longitude}\")");
            }
            else
            {
                Setting.MapCenter = (lat, lon);
            }

            if (!GuaranteeFileAccess(out List<string> failed, out List<string> noSpatial, out int fileCountTotal))
            {
                Setting.ExportError |= ExportErrors.ShareViolation;
                m_Log.Warn("Unable to access following files: 無法存取以下檔案：");
                failed.ForEach(file => m_Log.Warn($"  {file}"));
            }

            if (noSpatial.Count > 0)
            {
                Setting.ExportError |= ExportErrors.MissingSpatialFields;
                m_Log.Info("Following feature types don't have any spatial fields selected: 以下圖徵類型未選取任何空間欄位：");
                noSpatial.ForEach(feature => m_Log.Info($"  {feature}"));
            }

            if (Setting.ExportError != ExportErrors.None)
            {
                List<string> errors = new List<string>();

                foreach (ExportErrors error in MiscUtils.GetFlagComponents(Setting.ExportError))
                {
                    string errorString = LocaleUtils.Translate($"Carto.export.ERROR[{error}]");
                    if (error == ExportErrors.ShareViolation) errorString = errorString.Replace("{FILES}", string.Join(LocaleUtils.Translate("Carto.common.SEPARATER"), failed.Select(path => $"`{path}`").ToArray()));
                    if (error == ExportErrors.MissingSpatialFields)
                    {
                        List<string> features = new List<string>();
                        foreach (string feature in noSpatial) features.Add(LocaleUtils.Translate($"Options.Carto.Carto.Mod.FEATURETYPE[{feature}]"));
                        errorString = errorString.Replace("{FEATURES}", string.Join(LocaleUtils.Translate("Carto.common.SEPARATER"), features));
                    }

                    errors.Add(errorString);
                }

                ErrorDialog exportErrorDialog = new ErrorDialog { severity = ErrorDialog.Severity.Warning, localizedTitle = "Common.WARNING", localizedMessage = "Carto.export.ERRORTEXT", errorDetails = string.Join("\n\n", errors), actions = ErrorDialog.Actions.None };
                ErrorDialogManager.ShowErrorDialog(exportErrorDialog);
                return;
            };

            if (!Directory.Exists(Setting.ContentFolder)) Directory.CreateDirectory(Setting.ContentFolder);

            Geodata geodata;

            if (Instance.Settings.IsRasterFormat)
            {
                if (Instance.Settings.ExportTerrain)
                {
                    CartoObject terrain = TerrainSystem.GetTerrainProperties();
                    geodata = new Geodata(terrain);
                    ExportRasterGeodata(terrain, geodata, "Terrain");
                }
                if (Instance.Settings.ExportWater)
                {
                    CartoObject water = WaterSystem.GetWaterProperties();
                    geodata = new Geodata(water);
                    ExportRasterGeodata(water, geodata, "Water");
                }
            }

            if (Instance.Settings.IsVectorFormat)
            {
                if (Instance.Settings.ExportArea)
                {
                    List<CartoObject> areas = new List<CartoObject>();
                    Dictionary<string, int> areaFields = new Dictionary<string, int>();
                    areas.AddRange(Instance.Area.GetDistrictsProperties(out Dictionary<string, int> districtFields));
                    areas.AddRange(Instance.Area.GetMapTilesProperties(out Dictionary<string, int> mapTileFields));
                    geodata = new Geodata(areas, MiscUtils.CombineFieldLengthDictionaries(districtFields, mapTileFields));
                    ExportVectorGeodata(areas, geodata, "Area");
                }
                if (Instance.Settings.ExportBuilding)
                {
                    List<CartoObject> buildings = new List<CartoObject>();
                    buildings.AddRange(Instance.Building.GetBuildingsProperties(out Dictionary<string, int> buildingField));
                    geodata = new Geodata(buildings, buildingField);
                    ExportVectorGeodata(buildings, geodata, "Building");
                }
                if (Instance.Settings.ExportNet)
                {
                    List<CartoObject> nets = new List<CartoObject>();
                    nets.AddRange(Instance.Net.GetRoadsProperties(out Dictionary<string, int> roadFields));
                    nets.AddRange(Instance.Net.GetTracksProperties(out Dictionary<string, int> trackFields));
                    nets.AddRange(Instance.Net.GetPathwayProperties(out Dictionary<string, int> pathFields));
                    geodata = new Geodata(nets, MiscUtils.CombineFieldLengthDictionaries(roadFields, trackFields, pathFields));
                    ExportVectorGeodata(nets, geodata, "Net", true);
                }
                if (Instance.Settings.ExportZoning)
                {
                    List<CartoObject> zones = new List<CartoObject>();
                    zones.AddRange(Instance.Zoning.GetZoningProperties(out Dictionary<string, int> zoningField));
                    geodata = new Geodata(zones, zoningField);
                    ExportVectorGeodata(zones, geodata, "Zoning");
                }
            }

            m_Log.Info(string.Empty);
            string exportCompleteString = default;

            if (anyIgnored)
            {
                int successFiles = fileCountTotal;

                foreach (ExportIgnoreReasons reason in Enum.GetValues(typeof(ExportIgnoreReasons)))
                {
                    if (ignored_count_helper.TryGetValue(reason, out int ignore_count)) successFiles -= ignore_count;
                }
                
                if (successFiles > 0) exportCompleteString = LocaleUtils.Translate("Carto.export.SUCCESSTEXT").Replace("{NUMBER}", $"{successFiles}");
                
                foreach (ExportIgnoreReasons reason in Enum.GetValues(typeof(ExportIgnoreReasons)))
                {
                    if (ignored_count_helper.TryGetValue(reason, out int ignore_count))
                    {
                        exportCompleteString = exportCompleteString + "\n" + LocaleUtils.Translate("Carto.export.IGNORETEXT").Replace("{NUMBER}", $"{ignore_count}").Replace("{REASON}", LocaleUtils.Translate($"Carto.export.IGNORE[{reason}]")).Replace("{FILES}", string.Join(", ", ignored_name_helper[reason]));
                    }
                }
            }
            else
            {
                exportCompleteString = LocaleUtils.Translate("Carto.export.SUCCESSTEXT").Replace("{NUMBER}", $"{fileCountTotal}");
            }

            MessageDialog exportSuccessDialog = new MessageDialog("Options.SECTION[Carto.Carto.Mod]", exportCompleteString, "Common.OK");
            Instance.UI.appBindings.ShowMessageDialog(exportSuccessDialog, null);
        }

        /// <summary>
        /// Get the field status of the designated feature type.
        /// （獲得指定圖徵類型的欄位狀態。）
        /// </summary>
        /// <param name="featureType">The type of the feature.（圖徵的類型。）</param>
        /// <param name="fieldName">The title of the field.（欄位的名稱。）</param>
        public static bool GetFieldStatus(FeatureType featureType, string fieldName)
        {
            if (Instance.Settings.FieldStatus.TryGetValue(featureType, out Dictionary<string, bool> entries))
            {
                if (entries.ContainsKey(fieldName))
                {
                    return entries[fieldName];
                }
            }

            return false;
        }
    }
}