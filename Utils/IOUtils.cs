using Carto.IO;
using Colossal.IO;
using Colossal.Logging;
using Colossal.PSI.Environment;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides utility functions to file input / outputs.
    /// （提供檔案輸出／輸入相關功能的類別。）
    /// </summary>
    public static class IOUtils
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// Combine input directories into one path.
        /// （將輸入的目錄結合成一個路徑。）
        /// </summary>
        /// <param name="paths">Directories along the path.（路徑上的目錄。）</param>
        /// <returns>The combined path with OS platform considerations.（考慮作業系統平臺情況下合併的路徑。）</returns>
        public static string CombinePath(params string[] paths)
        {
            return GetOSPLatform() switch
            {
                Platform.Windows => Path.Combine(paths).Replace("/", "\\"),
                _ => Path.Combine(paths).Replace("\\", "/"),
            };
        }

        /// <summary>
        /// Create the user data directories if the folders are not exist.
        /// （若目錄不存在，創造使用者資料目錄。）
        /// </summary>
        public static void CreateUserDataDirectories()
        {
            // Check for ModsData/Carto.（檢查 ModsData/Carto。）
            if (!Directory.Exists(Instance.CartoDataPath)) Directory.CreateDirectory(Instance.CartoDataPath);

            // Check for GeoJSON, GeoTIFF, Shapefile & Styles.（檢查 GeoJSON、GeoTIFF、Shapefile 及 Styles。）
            string geoJSON = CombinePath(Instance.CartoDataPath, "GeoJSON");
            string geoTIFF = CombinePath(Instance.CartoDataPath, "GeoTIFF");
            string shapefile = CombinePath(Instance.CartoDataPath, "Shapefile");
            string styles = CombinePath(Instance.CartoDataPath, "Styles");

            if (!Directory.Exists(geoJSON)) Directory.CreateDirectory(geoJSON);
            if (!Directory.Exists(geoTIFF)) Directory.CreateDirectory(geoTIFF);
            if (!Directory.Exists(shapefile)) Directory.CreateDirectory(shapefile);
            if (!Directory.Exists(styles))
            {
                Directory.CreateDirectory(styles);
            }
            else
            {
                // Force refresh the folder.
                // （強制重新載入資料夾。）
                Directory.Delete(styles, recursive: true);
                Directory.CreateDirectory(styles);
            }
        }

        /// <summary>
        /// Convert the <see cref="Display"/> to boolean.
        /// （將 <see cref="Display"/> 轉換為布林值。）
        /// </summary>
        /// <param name="displayMode">The input enum value.（輸入的枚舉值。）</param>
        /// <returns>Whether to assign all tags.（是否要列出所有標籤？）</returns>
        public static bool DisplayModeToBoolean(Display displayMode)
        {
            return displayMode switch
            {
                Display.All => true,
                Display.Single => false,
                _ => false,
            };
        }

        /// <summary>
        /// Extract the embedded QGIS style resources to the disk.
        /// （將嵌入的 QGIS 樣式資源擷取至硬碟。）
        /// </summary>
        /// <param name="dir">The path to the output directory.（指向輸出目錄的路徑。）</param>
        public static void ExtractEmbeddedStyles(string dir)
        {            
            string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (string embedded in resources)
            {
                if (embedded == "Carto.Styles.zip")
                {
                    string zipPath = Path.Combine(EnvPath.kTempDataPath, "Carto_Mod_QGIS_Styles.zip");

                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embedded))
                    {
                        using (FileStream fs = new(zipPath, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(fs);
                        }
                    }

                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    try
                    {
                        Unzip(zipPath, dir);
                    }
                    catch (Exception ex)
                    {
                        Instance.Log.Warn($"Cannot extract QGIS style presets to the target directory. 無法將 QGIS 預設樣式表擷取至目標目錄。{ex}");
                    }

                    break;
                }
            }
        }

        public static byte[] GetBytes<T>(T value, bool stringify = false)
        {
            switch (value)
            {
                case byte @byte:
                    return new byte[1] { @byte };

                case byte[] bytes:
                    return bytes;

                case double @double:
                    return BitConverter.GetBytes(@double);

                case float @float:
                    return BitConverter.GetBytes(@float);

                case int @int:
                    return BitConverter.GetBytes(@int);

                case short @short:
                    return BitConverter.GetBytes(@short);

                case string @string:
                    return Encoding.UTF8.GetBytes(@string);

                case ushort @ushort:
                    return BitConverter.GetBytes(@ushort);

                default:
                    if (stringify)
                    {
                        return Encoding.UTF8.GetBytes(value.ToString());
                    }
                    throw new NotSupportedException($"The type `{typeof(T).Name}` is not supported. 不支援 `{typeof(T).Name}` 型別。");
            }
        }

        /// <summary>
        /// Retrieve the byte array with flipped endianess.
        /// （獲得端序翻轉的位元組陣列。）
        /// </summary>
        /// <param name="value">The input value.（輸入值。）</param>
        /// <returns>The flipped byte array with length of 8.（長度為 8、已翻轉的位元組陣列。）</returns>
        public static byte[] GetFlippedBytes(double value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Retrieve the byte array with flipped endianess.
        /// （獲得端序翻轉的位元組陣列。）
        /// </summary>
        /// <param name="value">The input value.（輸入值。）</param>
        /// <returns>The flipped byte array with length of 4.（長度為 4、已翻轉的位元組陣列。）</returns>
        public static byte[] GetFlippedBytes(float value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Retrieve the byte array with flipped endianess.
        /// （獲得端序翻轉的位元組陣列。）
        /// </summary>
        /// <param name="value">The input value.（輸入值。）</param>
        /// <returns>The flipped byte array with length of 4.（長度為 4、已翻轉的位元組陣列。）</returns>
        public static byte[] GetFlippedBytes(int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Retrieve the byte array with flipped endianess.
        /// （獲得端序翻轉的位元組陣列。）
        /// </summary>
        /// <param name="value">The input value.（輸入值。）</param>
        /// <returns>The flipped byte array with length of 2.（長度為 2、已翻轉的位元組陣列。）</returns>
        public static byte[] GetFlippedBytes(short value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Retrieve the byte array with flipped endianess.
        /// （獲得端序翻轉的位元組陣列。）
        /// </summary>
        /// <param name="value">The input value.（輸入值。）</param>
        /// <returns>The flipped byte array.（已翻轉的位元組陣列。）</returns>
        public static byte[] GetFlippedBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Retrieve the byte array with flipped endianess.
        /// （獲得端序翻轉的位元組陣列。）
        /// </summary>
        /// <param name="value">The input value.（輸入值。）</param>
        /// <returns>The flipped byte array with length of 2.（長度為 2、已翻轉的位元組陣列。）</returns>
        public static byte[] GetFlippedBytes(ushort value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        public static byte[] GetFlippedBytes<T>(T value, bool stringify = false)
        {
            return GetBytes(value, stringify).Reverse().ToArray();
        }

        /// <summary>
        /// Retrieve the embedded JSON resource as a dictionary.
        /// （獲得代表嵌入 JSON 資源的字典。）
        /// </summary>
        /// <param name="resourceName">The resource's name.（資源的名稱。）</param>
        /// <param name="jsonConverter">The custom JSON converter.（客製化的 JSON 轉換器。）</param>
        /// <returns>The JSON object as a dictionary of strings.（由字串組成的字典表示的 JSON 物件。）</returns>
        public static Dictionary<string, string> GetJsonResource(string resourceName, JsonConverter jsonConverter = null)
        {
            using Stream stream = Instance.Assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException(resourceName);
            using StreamReader streamReader = new(stream);
            using JsonReader reader = new JsonTextReader(streamReader);
            JsonSerializer serializer = new();
            if (jsonConverter is not null) serializer.Converters.Add(jsonConverter);
            return serializer.Deserialize<Dictionary<string, string>>(reader) ?? new();
        }

        /// <summary>
        /// Retrieve the list of locked files.
        /// （獲得被鎖定的檔案列表。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <returns>The list of locked files' paths.（包含被鎖定檔案路徑的列表。）</returns>
        public static List<string> GetLockedFiles(Options options)
        {
            List<string> GetLockedGeoJSONFiles(IO.System system, VectorKind vectors)
            {
                List<string> lockedFiles = new();
                if (vectors == VectorKind.Unknown) return lockedFiles;
                foreach (VectorKind vector in CommonUtils.GetFlagComponents(vectors))
                {
                    string filePath = options.GetFilePath(system, vector);
                    if (IsFileLocked(filePath)) lockedFiles.Add(filePath);
                }
                return lockedFiles;
            }

            List<string> GetLockedShapefiles(IO.System system, VectorKind vectors)
            {
                List<string> lockedFiles = new();
                if (vectors == VectorKind.Unknown) return lockedFiles;
                foreach (VectorKind vector in CommonUtils.GetFlagComponents(vectors))
                {
                    string filePath = options.GetFilePath(system, vector);
                    string cpgPath = Path.ChangeExtension(filePath, "cpg");
                    string dbfPath = Path.ChangeExtension(filePath, "dbf");
                    string prjPath = Path.ChangeExtension(filePath, "prj");
                    string shxPath = Path.ChangeExtension(filePath, "shx");
                    if (IsFileLocked(filePath)) lockedFiles.Add(filePath);
                    if (IsFileLocked(cpgPath)) lockedFiles.Add(cpgPath);
                    if (IsFileLocked(dbfPath)) lockedFiles.Add(dbfPath);
                    if (IsFileLocked(prjPath)) lockedFiles.Add(prjPath);
                    if (IsFileLocked(shxPath)) lockedFiles.Add(shxPath);
                }
                return lockedFiles;
            }
            
            bool useArea = options.Systems.HasFlag(IO.System.Area);
            bool useBuilding = options.Systems.HasFlag(IO.System.Building);
            bool useNetwork = options.Systems.HasFlag(IO.System.Network);
            bool usePOI = options.Systems.HasFlag(IO.System.POI);
            bool useRaster = options.Systems.HasFlag(IO.System.Raster);
            bool useRoute = options.Systems.HasFlag(IO.System.Route);
            bool useZoning = options.Systems.HasFlag(IO.System.Zoning);
            bool useVector = useArea || useBuilding || useNetwork || usePOI || useRoute || useZoning;

            List<string> lockedFiles = new();

            if (useVector)
            {
                switch (options.VectorFormat)
                {
                    case FileFormat.GeoJSON:
                        if (useArea && options.VectorKinds.TryGetValue(IO.System.Area, out VectorKind areaVector))
                        {
                            lockedFiles.AddRange(GetLockedGeoJSONFiles(IO.System.Area, areaVector));
                        }
                        if (useBuilding && options.VectorKinds.TryGetValue(IO.System.Building, out VectorKind buildingVector))
                        {
                            lockedFiles.AddRange(GetLockedGeoJSONFiles(IO.System.Building, buildingVector));
                        }
                        if (useNetwork && options.VectorKinds.TryGetValue(IO.System.Network, out VectorKind networkVector))
                        {
                            lockedFiles.AddRange(GetLockedGeoJSONFiles(IO.System.Network, networkVector));
                        }
                        if (usePOI && options.VectorKinds.TryGetValue(IO.System.POI, out VectorKind poiVector))
                        {
                            lockedFiles.AddRange(GetLockedGeoJSONFiles(IO.System.POI, poiVector));
                        }
                        if (useRoute && options.VectorKinds.TryGetValue(IO.System.Route, out VectorKind routeVector))
                        {
                            lockedFiles.AddRange(GetLockedGeoJSONFiles(IO.System.Route, routeVector));
                        }
                        if (useZoning && options.VectorKinds.TryGetValue(IO.System.Zoning, out VectorKind zoningVector))
                        {
                            lockedFiles.AddRange(GetLockedGeoJSONFiles(IO.System.Zoning, zoningVector));
                        }
                        break;

                    case FileFormat.Shapefile:
                        if (useArea && options.VectorKinds.TryGetValue(IO.System.Area, out VectorKind areaVectors))
                        {
                            lockedFiles.AddRange(GetLockedShapefiles(IO.System.Area, areaVectors));
                        }
                        if (useBuilding && options.VectorKinds.TryGetValue(IO.System.Building, out VectorKind buildingVectors))
                        {
                            lockedFiles.AddRange(GetLockedShapefiles(IO.System.Building, buildingVectors));
                        }
                        if (useNetwork && options.VectorKinds.TryGetValue(IO.System.Network, out VectorKind networkVectors))
                        {
                            lockedFiles.AddRange(GetLockedShapefiles(IO.System.Network, networkVectors));
                        }
                        if (usePOI && options.VectorKinds.TryGetValue(IO.System.POI, out VectorKind poiVectors))
                        {
                            lockedFiles.AddRange(GetLockedShapefiles(IO.System.POI, poiVectors));
                        }
                        if (useRoute && options.VectorKinds.TryGetValue(IO.System.Route, out VectorKind routeVectors))
                        {
                            lockedFiles.AddRange(GetLockedShapefiles(IO.System.Route, routeVectors));
                        }
                        if (useZoning && options.VectorKinds.TryGetValue(IO.System.Zoning, out VectorKind zoningVectors))
                        {
                            lockedFiles.AddRange(GetLockedShapefiles(IO.System.Zoning, zoningVectors));
                        }
                        break;
                }
            }

            if (useRaster)
            {
                switch (options.RasterFormat)
                {
                    case FileFormat.GeoTIFF:
                        RasterKind[] rasters = CommonUtils.GetFlagComponents(options.RasterKinds);
                        foreach (RasterKind raster in rasters)
                        {
                            string filePath = options.GetFilePath(raster);
                            if (IsFileLocked(filePath)) lockedFiles.Add(filePath);
                        }
                        break;
                }
            }

            return lockedFiles;
        }

        /// <summary>
        /// Get the OS platform the game is running on.
        /// （獲得遊戲運行的作業系統平臺。）
        /// </summary>
        /// <returns>The current OS platform.（目前的作業系統平臺。）</returns>
        public static Platform GetOSPLatform()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return Platform.Linux;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return Platform.OSX;
                }

                return Platform.Windows;
            }
            catch (Exception)
            {
                _log.Warn("An error occured at GetOSPlatform(), returning default value `Platform.Windows`. 於 GetOSPlatform() 發生一個錯誤，回傳預設值 `Platform.Windows`。");
                return Platform.Windows;
            }
        }

        /// <summary>
        /// Retrieve current stream's position.
        /// （獲得目前資料流的位置。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <returns>The position.（目前位置。）</returns>
        public static int GetPosition(BinaryWriter writer)
        {
            long position = writer.BaseStream.Position;
            if (position > int.MaxValue) return int.MaxValue;
            return Convert.ToInt32(position);
        }

        /// <summary>
        /// Retrieve the target projection details fro the export options.
        /// （由輸出設定獲得目標投影的資訊。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="targetCRS">The target Coordinate Reference System.（目標坐標參考系統。）</param>
        /// <param name="targetProjection">The target projection defintion.（目標投影定義。）</param>
        public static void GetTargetProjections(Options options, out Geodata.CRS targetCRS, out Geodata.ProjectionDefinition targetProjection)
        {
            targetCRS = Geodata.CRS.WGS84;
            targetProjection = default;

            switch (options.VectorFormat)
            {
                case FileFormat.GeoJSON:
                    break;

                case FileFormat.Shapefile:
                    targetCRS = options.TargetProjection;
                    targetProjection = options.TargetProjectionDefinition;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Check whether a file is locked by other threads.
        /// （確認檔案是否被其他執行緒鎖定。）
        /// </summary>
        /// <param name="path">The path to the file.（指向檔案的連結。）</param>
        /// <returns>If true, the file is locked.（若為真，檔案被鎖定。）</returns>
        public static bool IsFileLocked(string path)
        {
            /*
                # References: （資料來源：）
                
                * ChrisW. (2009). Is there a way to check if a file is in use?
                    https://stackoverflow.com/a/937558
            */

            if (!File.Exists(path)) return false;

            try
            {
                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    fs.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove invalid characters for file naming from the input string.
        /// （移除字串中的檔案命名非法字元。）
        /// </summary>
        /// <param name="input">The unsanitized string.（未處理的字串。）</param>
        /// <returns>The sanitized string.（處理後的字串。）</returns>
        public static string RemoveInvalidChars(string input)
        {
            return new string(input.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
        }

        /// <summary>
        /// Reveal the file in the platform-specific file explorer. (Windows - File Explorer, Mac OS - Finder, Linux - Gnome)
        /// （在檔案瀏覽器中顯示指定目錄。）
        /// </summary>
        /// <param name="path">The path to the directory.（指向目錄的路徑。）</param>
        public static void RevealInFileExplorer(string path)
        {
            /*
                # References: （資料來源：）

                * manuc66. (2022). From dotnet how to open file in containing folder in the Linux file manager?
                    https://stackoverflow.com/a/73409251
            */

            if (!Directory.Exists(path)) return;

            try
            {
                switch (GetOSPLatform())
                {
                    case Platform.Linux:
                        Process.Start("xdg-open", path);
                        break;

                    case Platform.OSX:
                        Process.Start("open", path);
                        break;

                    case Platform.Windows:
                        Process.Start("explorer", path);
                        break;

                    default:
                        throw new NotSupportedException("No support for the platforms other than Linux, OSX and Windows.（不支援 Linux、OSX 與 Windows 以外的平臺。）");
                }
            }
            catch (Exception ex)
            {
                Instance.Log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Skip any number of byte in the file.
        /// （跳過檔案中任意數量的位元組。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="count">The number of skipped bytes.（跳過的位元組數量。）</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SkipBytes(BinaryWriter writer, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("count", "The count should be larger or equal to 0. 數量應大於等於 0。");
            writer.Write(new byte[count]);
        }

        /// <summary>
        /// Trim the numerical user-input string.
        /// （裁減使用者輸入的數值字串。）
        /// </summary>
        /// <param name="text">Input string.（輸入的字串。）</param>
        /// <returns>The trimmed string.（裁剪後的字串。）</returns>
        private static string TrimNumericInput(string text)
        {
            text = text.Trim();
            return string.IsNullOrEmpty(text) ? "0" : text;
        }

        /// <summary>
        /// Try to retrieve the latitude value from the user input.
        /// （嘗試從使用者輸入值中獲得緯度。）
        /// </summary>
        /// <param name="text">Input string.（輸入的字串。）</param>
        /// <param name="latitude">The converted latitude.（轉換的緯度。）</param>
        /// <returns>The error during the parsing process.（轉換過程中遇到的錯誤。）</returns>
        public static Error TryGetLatitude(string text, out double latitude)
        {
            latitude = 0;
            if (!double.TryParse(TrimNumericInput(text), out double _latitude)) return Error.Nan;
            double absLatitude = Math.Abs(_latitude);
            if (absLatitude > 90) return Error.Latitude;
            latitude = _latitude;
            return Error.None;
        }

        /// <summary>
        /// Try to retrieve the length value from the user input.
        /// （嘗試從使用者輸入值中獲得長度。）
        /// </summary>
        /// <param name="text">Input string.（輸入的字串。）</param>
        /// <param name="length">The converted length.（轉換的長度。）</param>
        /// <returns>The error during the parsing process.（轉換過程中遇到的錯誤。）</returns>
        public static Error TryGetLength(string text, out double length)
        {
            length = 0;
            if (!double.TryParse(TrimNumericInput(text), out double _length)) return Error.Nan;
            if (length <= 0) return Error.Negative;
            length = _length;
            return Error.None;
        }

        /// <summary>
        /// Try to retrieve the longitude value from the user input.
        /// （嘗試從使用者輸入值中獲得經度。）
        /// </summary>
        /// <param name="text">Input string.（輸入的字串。）</param>
        /// <param name="longitude">The converted longitude.（轉換的經度。）</param>
        /// <returns>The error during the parsing process.（轉換過程中遇到的錯誤。）</returns>
        public static Error TryGetLongitude(string text, out double longitude)
        {
            longitude = 0;
            if (!double.TryParse(TrimNumericInput(text), out double _longitude)) return Error.Nan;
            double absLatitude = Math.Abs(_longitude);
            if (absLatitude > 180) return Error.Longitude;
            longitude = _longitude;
            return Error.None;
        }

        /// <summary>
        /// Try to retrieve the numerical value from the user input.
        /// （嘗試從使用者輸入值中獲得數值。）
        /// </summary>
        /// <param name="text">Input string.（輸入的字串。）</param>
        /// <param name="number">The converted number.（轉換的數值。）</param>
        /// <returns>The error during the parsing process.（轉換過程中遇到的錯誤。）</returns>
        public static Error TryGetNumber(string text, out double number)
        {
            number = 0;
            if (!double.TryParse(TrimNumericInput(text), out double _number)) return Error.Nan;
            number = _number;
            return Error.None;
        }

        /// <summary>
        /// Try to retrieve the Helmert Transform parameters from the user input.
        /// （嘗試從使用者輸入值中獲得赫爾默特轉換參數。）
        /// </summary>
        /// <param name="text">Input string.（輸入的字串。）</param>
        /// <param name="transform">The Helmert Transform parameters.（赫爾默特轉換參數。）</param>
        /// <returns>The error during the parsing process.（轉換過程中遇到的錯誤。）</returns>
        public static Error TryGetTransform(string text, out double[] transform)
        {
            text = text.Trim();
            if (string.IsNullOrWhiteSpace(text) || text == string.Empty)
            {
                transform = new double[0] { };
                return Error.None;
            }

            string[] parts = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3 && parts.Length != 7)
            {
                transform = new double[0] { };
                return Error.TransformLength;
            }

            transform = new double[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!double.TryParse(parts[i], out double param))
                {
                    return Error.Transform;
                }

                transform[i] = param;
            }

            return Error.None;
        }

        /// <summary>
        /// Try to retrieve the UTM zone number from the user input.
        /// （嘗試從使用者輸入值中獲得 UTM 分區代號。）
        /// </summary>
        /// <param name="text">Input string.（輸入的字串。）</param>
        /// <param name="zone">The UTM zone number.（UTM 分區代號。）</param>
        /// <returns>The error during the parsing process.（轉換過程中遇到的錯誤。）</returns>
        public static Error TryGetUTMZone(string text, out int zone)
        {
            zone = 0;
            if (!int.TryParse(TrimNumericInput(text), out int _zone)) return Error.Nan;
            if ((_zone < 1) || (_zone > 60)) return Error.UTMZone;
            zone = _zone;
            return Error.None;
        }

        /// <summary>
        /// The local version of <see cref="ZipUtilities.Unzip"/>.
        /// （<see cref="ZipUtilities.Unzip"/> 的本地版本。）
        /// </summary>
        /// <param name="file">The path to the zip file.（壓縮檔案路徑。）</param>
        /// <param name="outputPath">The path to the output directory.（輸出目錄的路徑。）</param>
        public static void Unzip(string file, string outputPath)
        {
            outputPath = Path.GetFullPath(outputPath).Replace('\\', '/').TrimEnd('/');
            Colossal.IO.IOUtils.EnsureDirectory(outputPath);

            using ZipInputStream stream = new(File.OpenRead(file));
            ZipEntry nextEntry;

            while ((nextEntry = stream.GetNextEntry()) != null)
            {
                string fullPath = Path.GetFullPath(Path.Combine(outputPath, nextEntry.Name)).Replace('\\', '/');

                if (!fullPath.StartsWith(outputPath + '/', StringComparison.OrdinalIgnoreCase))
                    continue;

                if (nextEntry.IsDirectory)
                {
                    LongDirectory.CreateDirectory(fullPath);
                    continue;
                }

                string dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir)) LongDirectory.CreateDirectory(dir);

                using (FileStream dest = File.Create(fullPath))
                    Colossal.IO.IOUtils.CopyStream(stream, dest);
            }
        }

        /// <summary>
        /// Write data in little endian.
        /// （以小端序寫入資料。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The input float.（輸入的 單精度浮點數。）</param>
        public static void WriteLE(BinaryWriter writer, float value)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BitConverter.GetBytes(value));
            }
            else
            {
                writer.Write(GetFlippedBytes(value));
            }
        }

        /// <summary>
        /// Write data in little endian.
        /// （以小端序寫入資料。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The input integer.（輸入的 32 位元整數。）</param>
        public static void WriteLE(BinaryWriter writer, int value)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BitConverter.GetBytes(value));
            }
            else
            {
                writer.Write(GetFlippedBytes(value));
            }
        }

        /// <summary>
        /// Write data in little endian.
        /// （以小端序寫入資料。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The input short.（輸入的 16 位元整數。）</param>
        public static void WriteLE(BinaryWriter writer, short value)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BitConverter.GetBytes(value));
            }
            else
            {
                writer.Write(GetFlippedBytes(value));
            }
        }

        /// <summary>
        /// Write data in little endian.
        /// （以小端序寫入資料。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The input string.（輸入的字串。）</param>
        public static void WriteLE(BinaryWriter writer, string value)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(Encoding.UTF8.GetBytes(value));
            }
            else
            {
                writer.Write(GetFlippedBytes(value));
            }
        }

        /// <summary>
        /// Write data in little endian.
        /// （以小端序寫入資料。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The input ushort.（輸入的 16 位元整數。）</param>
        public static void WriteLE(BinaryWriter writer, ushort value)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BitConverter.GetBytes(value));
            }
            else
            {
                writer.Write(GetFlippedBytes(value));
            }
        }

        /// <summary>
        ///  Write data in little endian.
        /// （以小端序寫入資料。）
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.（<paramref name="value"/> 的型別。）</typeparam>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The input value.（輸入的數值。）</param>
        /// <param name="stringify">Turn the value into the UTF-8 string, if no conversions available.（若沒有合適的轉換，將數值變成字串。）</param>
        /// <exception cref="NotSupportedException"></exception>
        public static void WriteLE<T>(BinaryWriter writer, T value, bool stringify = false)
        {
            byte[] bytes = GetBytes(value, stringify);
            if (bytes != null)
            {
                if (BitConverter.IsLittleEndian)
                {
                    writer.Write(bytes);
                }
                else
                {
                    writer.Write(bytes.Reverse().ToArray());
                }
            }
        }
    }
}