using Carto.IO;
using Colossal.Logging;
using Game.Areas;
using Game.Buildings;
using Game.Net;
using Game.Prefabs;
using Game.Routes;
using Game.Zones;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        /// <returns>The flipped byte array with length of 4.（長度為 4、已翻轉的位元組陣列。）</returns>
        public static byte[] GetFlippedBytes(int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
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
        /// Remove invalid characters for file naming from the input string.
        /// （移除字串中的檔案命名非法字元。）
        /// </summary>
        /// <param name="input">The unsanitized string.（未處理的字串。）</param>
        /// <returns>The sanitized string.（處理後的字串。）</returns>
        public static string RemoveInvalidChars(string input)
        {
            return new string(input.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
        }
    }
}