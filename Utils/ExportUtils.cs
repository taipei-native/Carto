namespace Carto.Utils
{
    using Carto.Domain;
    using Carto.Geodata;
    using Carto.Systems;
    using Colossal.Logging;
    using System;
    using System.Collections.Generic;
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
            Longitude = 2
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
            { FeatureType.Area, new Dictionary<string, bool>{ { "Area", false }, { "Center", false }, { "Edge", true }, { "Object", true }, { "Unlocked", true } }},
            { FeatureType.Building, new Dictionary<string, bool>{ { "Address", true }, { "Asset", true }, { "Brand", false }, { "Category", true }, { "Edge", true }, { "Employee", true }, { "Household", false }, { "Level", false }, { "Object", true }, { "Product", false }, { "Resident", true }, { "Zoning", true }  }},
            { FeatureType.Net, new Dictionary<string, bool> { { "Asset", true }, { "Category", true }, { "CenterLine", true }, { "Direction", true }, { "Edge", true }, { "Form", true }, { "Height", false }, { "Length", false }, { "Object", true }, { "Width", false } }},
            { FeatureType.Terrain, new Dictionary<string, bool> { { "Elevation", true } }},
            { FeatureType.Water, new Dictionary<string, bool> { { "Depth", true } }},
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
            { "Address.district", typeof(string) },
            { "Address.street", typeof(string) },
            { "Address.number", typeof(int) },
            { "Asset", typeof(string) },
            { "Brand", typeof(string) },
            { "Category", typeof(string) },
            { "Center", typeof(float3) },
            { "Color", typeof(string) },
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
        /// The method handling the validation of settings and export of the files.
        /// （處理驗證設定與輸出檔案的方法。）
        /// </summary>
        public static void ExportToFile()
        {
            string cityName = MiscUtils.RemoveInvalidChars(Instance.CityName);
            string mapName = MiscUtils.RemoveInvalidChars(Instance.MapName);

            void ExportRasterGeodata(CartoObject obj, Geodata input, string type)
            {
                if (obj.Values.Keys.Count > 1)
                {
                    foreach (string field in obj.Values.Keys)
                    {
                        ExportToFormat(input, GetFileName($"{type}_{field}"), field);
                    }
                }
                else
                {
                    ExportToFormat(input, GetFileName(type), obj.Values.Keys.First());
                }
            }

            void ExportVectorGeodata(List<CartoObject> objs, Geodata input, string type, bool suppressFieldError = false)
            {
                List<string> fields = objs.SelectMany(obj => obj.Edges.Keys.ToList()).Distinct().ToList();

                if (fields.Count > 1)
                {
                    foreach (string field in fields)
                    {
                        ExportToFormat(input, GetFileName($"{type}_{field}"), field, suppressFieldError);
                    }
                }
                else
                {
                    ExportToFormat(input, GetFileName(type), fields[0], suppressFieldError);
                }
            }

            void ExportToFormat(Geodata input, string fileName, string target, bool suppressFieldError = false)
            {
                switch (Instance.Settings.ExportFormat)
                {
                    case ExportFormats.GeoJSON:
                        input.ToGeoJSON($"{fileName}.json", target, suppressFieldError);
                        break;

                    //case ExportFormats.GeoPackage:
                    //    input.ToGeoPackage($"{fileName}.gkpg"); NOT Implement yet! （*尚未* 實作！）
                    //    break;

                    case ExportFormats.GeoTIFF:
                        input.ToGeoTIFF($"{fileName}.tif", target);
                        break;

                    case ExportFormats.Shapefile:
                        input.ToShapefile($"{fileName}.shp", target, suppressFieldError);
                        break;
                }
            }

            string GetFileName(string feature)
            {
                string translated = string.Empty;

                if (feature.Split('_').Length > 1)
                {
                    List<string> stringList = new List<string>();

                    foreach (string section in feature.Split('_'))
                    {
                        if (LocaleUtils.TryTranslate($"Options.OPTION[Carto.Carto.Mod.Setting.Export{section}]", out string featureType))
                        {
                            stringList.Add(featureType);
                        }
                        else if (LocaleUtils.TryTranslate($"Options.OPTION[Carto.Carto.Mod.Setting.ExportField{section}]", out string fieldName))
                        {
                            stringList.Add(fieldName);
                        }
                        else
                        {
                            stringList.Add(section);
                        }
                    }

                    translated = string.Join("_", stringList);
                }
                else
                {
                    translated = LocaleUtils.Translate($"Options.OPTION[Carto.Carto.Mod.Setting.Export{feature}]");
                }

                switch (Instance.Settings.NamingFormat)
                {
                    case NamingFormat.Custom:
                        string customName = MiscUtils.RemoveInvalidChars(Instance.Settings.CustomFileName.Replace("{City}", cityName).Replace("{Map}", mapName).Replace("{Feature}", translated));
                        if ((customName != null) && (customName != string.Empty)) return customName;
                        m_Log.Info($"The custom file name is invalid, changing to the default name. 自訂檔案名無效，改為使用預設名稱。");
                        return translated;

                    case NamingFormat.Feature:
                        return translated;

                    case NamingFormat.CityName_Feature:
                        if ((cityName != null) && (cityName != string.Empty)) return $"{cityName}_{translated}";
                        return $"{LocaleUtils.Translate("Carto.export.UNKNOWN[City]")}_{translated}";

                    case NamingFormat.MapName_Feature:
                        if ((mapName != null) && (mapName != string.Empty)) return $"{mapName}_{translated}";
                        return $"{LocaleUtils.Translate("Carto.export.UNKNOWN[Map]")}_{translated}";

                    default:
                        return translated;
                }
            }

            if (!MiscUtils.TryGetLatitude(Instance.Settings.Latitude, out double lat))
            {
                Setting.ExportError |= ExportErrors.Latitude;
                m_Log.Warn($"Unable to parse latitude `{Instance.Settings.Latitude}`. 無法解析緯度字串 `{Instance.Settings.Latitude}。`");
            }
            if (!MiscUtils.TryGetLongtitude(Instance.Settings.Longitude, out double lon))
            {
                Setting.ExportError |= ExportErrors.Longitude;
                m_Log.Warn($"Unable to parse longitude `{Instance.Settings.Longitude}`. 無法解析經度字串 `{Instance.Settings.Longitude}`。");
            }
            else
            {
                Setting.MapCenter = (lat, lon);
            }

            if (Setting.ExportError.HasFlag(ExportErrors.Latitude) || Setting.ExportError.HasFlag(ExportErrors.Longitude))
            {
                return;  // TODO: Raise the error prompt UI
            };

            m_Log.Info(new string('=', 30));
            m_Log.Info("Export Settings");
            m_Log.Info(new string('-', 30));
            m_Log.Info($"FORMAT   : {Instance.Settings.ExportFormat}");
            if (Instance.Settings.NamingFormat != NamingFormat.Custom) m_Log.Info($"NAMING   : {Instance.Settings.NamingFormat}");
            if (Instance.Settings.NamingFormat == NamingFormat.Custom) m_Log.Info($"NAMING   : Custom ({Instance.Settings.CustomFileName})");
            m_Log.Info($"LATITUDE : {lat}");
            m_Log.Info($"LONGITUDE: {lon}");
            m_Log.Info(new string('-', 30));
            m_Log.Info($"UseUnzoned: {Instance.Settings.UseUnzoned}");
            m_Log.Info($"UseZCC    : {Instance.Settings.UseZCC}");
            m_Log.Info(new string('=', 30));

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
                    areas.AddRange(Instance.Area.GetDistrictsProperties());
                    areas.AddRange(Instance.Area.GetMapTilesProperties());
                    geodata = new Geodata(areas);
                    ExportVectorGeodata(areas, geodata, "Area");
                }
                if (Instance.Settings.ExportBuilding)
                {
                    List<CartoObject> buildings = new List<CartoObject>();
                    buildings.AddRange(Instance.Building.GetBuildingsProperties());
                    geodata = new Geodata(buildings);
                    ExportVectorGeodata(buildings, geodata, "Building");
                }
                if (Instance.Settings.ExportNet)
                {
                    List<CartoObject> nets = new List<CartoObject>();
                    nets.AddRange(Instance.Net.GetRoadsProperties());
                    nets.AddRange(Instance.Net.GetTracksProperties());
                    nets.AddRange(Instance.Net.GetPathwayProperties());
                    geodata = new Geodata(nets);
                    ExportVectorGeodata(nets, geodata, "Net", true);
                }
                if (Instance.Settings.ExportZoning)
                {
                    List<CartoObject> zones = new List<CartoObject>();
                    zones.AddRange(Instance.Zoning.GetZoningProperties());
                    geodata = new Geodata(zones);
                    ExportVectorGeodata(zones, geodata, "Zoning");
                }
            }

            m_Log.Info(string.Empty);
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