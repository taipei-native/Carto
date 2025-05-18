using Carto.IO;
using Colossal.Logging;
using Game.Areas;
using Game.Buildings;
using Game.Net;
using Game.Prefabs;
using Game.Routes;
using Game.Zones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Entities;

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
        /// Retrieve the category of a feature.
        /// （獲得圖徵的分類。）
        /// </summary>
        /// <param name="entityManager">The manager of in-game entities.（遊戲內實體的管理者。）</param>
        /// <param name="feature">The feature entity.（圖徵實體。）</param>
        /// <returns>The applicable feature types.（適合的圖徵分類。）</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Feature GetFeatureType(EntityManager entityManager, Entity feature)
        {
            if ((entityManager == null) | (feature == null) | (feature == Entity.Null)) throw new ArgumentNullException("The parameters must not be null. 參數不應為空值。");
            if (!entityManager.HasComponent<PrefabRef>(feature)) throw new ArgumentException("The feature should be an instance of the prefab. 圖徵應為預製部件的實例。");
            Feature featureType = Feature.None;

            Entity prefabRef = entityManager.GetComponentData<PrefabRef>(feature).m_Prefab;

            // Area features.（區域圖徵。）
            if (entityManager.HasComponent<Area>(feature))
            {
                if (entityManager.HasComponent<District>(feature))
                {
                    featureType |= Feature.District;
                }
                else if (entityManager.HasComponent<Extractor>(feature))
                {
                    featureType |= Feature.Extractor;
                }
                else if (entityManager.HasComponent<Storage>(feature))
                {
                    featureType |= Feature.Landfill;
                }
                else if (entityManager.HasComponent<MapTile>(feature))
                {
                    featureType |= Feature.MapTile;
                }
                else if (entityManager.HasComponent<Surface>(feature))
                {
                    featureType |= Feature.Surface;
                }
            }

            // Building features.（建築圖徵。）
            if (entityManager.HasComponent<Building>(feature))
            {
                featureType |= Feature.Building;
            }

            // Network features.（網路圖徵。）
            if (entityManager.HasChunkComponent<Curve>(feature))
            {
                bool isMarker = entityManager.HasComponent<Marker>(feature);
                bool isRoad = entityManager.HasComponent<Road>(feature);
                bool isTaxiway = entityManager.HasComponent<Taxiway>(feature);
                bool isTrack = entityManager.HasComponent<SubwayTrack>(feature) ||
                               entityManager.HasComponent<TrainTrack>(feature) ||
                               entityManager.HasComponent<TramTrack>(feature);
                bool isWaterway = entityManager.HasComponent<Waterway>(feature);

                // Stand-alone networks.（獨立網路。）
                if (!isMarker & !isRoad & !isTaxiway & !isTrack & !isWaterway)
                {
                    if (entityManager.HasComponent<PathwayData>(prefabRef))
                    {
                        featureType |= Feature.Pathway;
                    }
                    else if (entityManager.HasComponent<Game.Net.ElectricityConnection>(feature))
                    {
                        featureType |= Feature.Cable;
                    }
                    else if (entityManager.HasComponent<Game.Net.WaterPipeConnection>(feature))
                    {
                        featureType |= Feature.Pipe;
                    }
                }
                if (isRoad)
                {
                    featureType |= Feature.Road;
                }
                if (isTaxiway)
                {
                    if (entityManager.HasComponent<TaxiwayData>(prefabRef))
                    {
                        TaxiwayFlags flag = entityManager.GetComponentData<TaxiwayData>(prefabRef).m_Flags;

                        if (flag.HasFlag(TaxiwayFlags.Runway))
                        {
                            featureType |= Feature.Runway;
                        }
                        else if (!flag.HasFlag(TaxiwayFlags.Airspace))
                        {
                            featureType |= Feature.Taxiway;
                        }
                    }
                }
                if (isTrack)
                {
                    featureType |= Feature.Track;
                }
                if (isWaterway)
                {
                    featureType |= Feature.Waterway;
                }

                // Lanes.（車道。）
                if (entityManager.HasComponent<Game.Net.UtilityLane>(feature))
                {
                    if (entityManager.HasComponent<UtilityLaneData>(prefabRef))
                    {
                        UtilityTypes flag = entityManager.GetComponentData<UtilityLaneData>(prefabRef).m_UtilityTypes;

                        if (flag.HasFlag(UtilityTypes.Catenary) ||
                            flag.HasFlag(UtilityTypes.LowVoltageLine) ||
                            flag.HasFlag(UtilityTypes.HighVoltageLine))
                        {
                            featureType |= Feature.Cable;
                        }
                        if (flag.HasFlag(UtilityTypes.Fence))
                        {
                            featureType |= Feature.Fence;
                        }
                        if (flag.HasFlag(UtilityTypes.SewagePipe) ||
                            flag.HasFlag(UtilityTypes.StormwaterPipe) ||
                            flag.HasFlag(UtilityTypes.WaterPipe))
                        {
                            featureType |= Feature.Pipe;
                        }
                    }
                }
            }

            // Route features.（路線圖徵。）
            if (entityManager.HasComponent<TransportLine>(feature))
            {
                if (entityManager.HasComponent<TransportLineData>(prefabRef))
                {
                    TransportLineData transportLineData = entityManager.GetComponentData<TransportLineData>(prefabRef);
                    if (transportLineData.m_CargoTransport) featureType |= Feature.RouteCargo;
                    if (transportLineData.m_PassengerTransport) featureType |= Feature.RoutePassenger;
                }
            }

            // Zoning features.（分區圖徵。）
            if (entityManager.HasComponent<Block>(feature))
            {
                featureType |= Feature.Zoning;
            }

            // Fallback value.（後備回傳值。）
            return featureType;
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