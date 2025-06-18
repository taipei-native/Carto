using Carto.IO;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Simulation;
using System;
using System.IO;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Carto.Systems
{
    /// <summary>
    /// The system that handles grid data.
    /// （處理網格資料的系統。）
    /// </summary>
    public partial class RasterSystem : GameSystemBase
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The system collecting shared data.（收集共享資料的系統。）<br/>
        /// See <see cref="Instance.Shared"/> for more information.
        /// </summary>
        static readonly SharedDataCollectionSystem _shared = Instance.Shared;

        /// <summary>
        /// The system managing the terrain.（管理地形的系統。）<br/>
        /// See <see cref="Instance.Terrain"/> for more information.
        /// </summary>
        static readonly TerrainSystem _terrain = Instance.Terrain;

        /// <summary>
        /// The system managing the water bodies.（管理水體的系統。）<br/>
        /// See <see cref="Instance.Water"/> for more information.
        /// </summary>
        static readonly WaterSystem _water = Instance.Water;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            base.OnCreate();
            _log.Debug("RasterSystem instance created. 網格系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// De-normalize the value to given interval.
        /// （將數值反標準化至給定的區間。）
        /// </summary>
        /// <param name="value">The input value.（輸入的數值。）</param>
        /// <param name="bounds">The bounds of the output value.（輸出數值的界限。）</param>
        /// <returns>The de-normalized value.（反標準化的數值。）</returns>
        private float DenormalizeToFloat(ushort value, Bounds1 bounds)
        {
            return (float)Math.Round(value * (bounds.max - bounds.min) / 65535, 4, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// De-normalize the value to given interval.
        /// （將數值反標準化至給定的區間。）
        /// </summary>
        /// <param name="value">The input value.（輸入的數值。）</param>
        /// <param name="bounds">The bounds of the output value.（輸出數值的界限。）</param>
        /// <returns>The de-normalized value.（反標準化的數值。）</returns>
        private short DenormalizeToShort(ushort value, Bounds1 bounds)
        {
            return (short)Math.Round(value * (bounds.max - bounds.min) / 65535, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Normalize the value from the given interval.
        /// （由給定的區間將數值標準化。）
        /// </summary>
        /// <param name="value">The input value.（輸入的數值。）</param>
        /// <param name="bounds">The bounds of the input value.（輸入數值的界限。）</param>
        /// <returns>The normalized value.（標準化的數值。）</returns>
        private ushort NormalizeToUShort(float value, Bounds1 bounds)
        {
            return (ushort)Math.Round(math.clamp((value - bounds.min) / (bounds.max - bounds.min), 0, 1) * 65535, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Write the depth to the file.
        /// （寫入水深至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public void WriteDepth(BinaryWriter writer, ref GeoTiff.Parameter param)
        {
            // Prepare metadata.（準備元資料。）
            WaterSurfaceData data = _water.GetSurfaceData(out _);
            param.imageHeight = data.resolution.z;
            param.imageWidth = data.resolution.x;
            param.scaleX = Math.Abs(Math.Round(1d / data.scale.x, 4));
            param.scaleY = Math.Abs(Math.Round(1d / data.scale.z, 4));
            GeoTiff.WriteHeader(writer, ref param);
            GeoTiff.Parameter _param = param;

            try
            {
                Task writerThread = Task.Run(() =>
                {
                    NativeArray<SurfaceWater> depth = data.depths;
                    GeoTiff.ValidateGrid(ref depth, _param);

                    switch (_param.format)
                    {
                        case GeoTiffFormat.Float32:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    float depthInPlace = depth[i * _param.imageWidth + j].m_Depth;
                                    writer.Write(BitConverter.GetBytes(depthInPlace == 0 ? _param.nodata : depthInPlace));
                                }
                            }
                            break;

                        case GeoTiffFormat.Int16:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    short depthInPlace = (short)Math.Round(depth[i * _param.imageWidth + j].m_Depth, MidpointRounding.AwayFromZero);
                                    writer.Write(BitConverter.GetBytes(depthInPlace <= 0 ? (short) _param.nodata : depthInPlace));
                                }
                            }
                            break;

                        case GeoTiffFormat.Norm16:
                            Bounds3 bounds = new(-data.offset, (data.resolution - 1) / data.scale - data.offset);
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    float depthInPlace = NormalizeToUShort(depth[i * _param.imageWidth + j].m_Depth, bounds.y);
                                    writer.Write(BitConverter.GetBytes(depthInPlace));
                                }
                            }
                            break;
                    }

                    GeoTiff.WriteGridDataCommon(writer, _param.BytesPerStrip(), _param.imageHeight);
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        /// Write the elevation to the file.
        /// （寫入高程至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public void WriteElevation(BinaryWriter writer, ref GeoTiff.Parameter param)
        {
            // Prepare metadata.（準備元資料。）
            TerrainHeightData data = _terrain.GetHeightData();
            param.bounds = TerrainUtils.GetBounds(ref data).y;
            param.imageHeight = data.resolution.z;
            param.imageWidth = data.resolution.x;
            param.scaleX = Math.Abs(Math.Round(1d / data.scale.x, 4));
            param.scaleY = Math.Abs(Math.Round(1d / data.scale.z, 4));
            GeoTiff.WriteHeader(writer, ref param);
            GeoTiff.Parameter _param = param;

            try
            {
                Task writerThread = Task.Run(() =>
                {
                    NativeArray<ushort> elevation = data.heights;
                    GeoTiff.ValidateGrid(ref elevation, _param);
                    
                    switch (_param.format)
                    {
                        case GeoTiffFormat.Float32:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(DenormalizeToFloat(elevation[i * _param.imageWidth + j], _param.bounds)));
                                }
                            }
                            break;

                        case GeoTiffFormat.Int16:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(DenormalizeToShort(elevation[i * _param.imageWidth + j], _param.bounds)));
                                }
                            }
                            break;

                        case GeoTiffFormat.Norm16:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(elevation[i * _param.imageWidth + j]));
                                }
                            }
                            break;
                    }

                    GeoTiff.WriteGridDataCommon(writer, _param.BytesPerStrip(), _param.imageHeight);
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        /// Write the "world" depth to the file.
        /// （寫入「世界」水深至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public void WriteWorldDepth(BinaryWriter writer, ref GeoTiff.Parameter param)
        {
            // Prepare metadata.（準備元資料。）
            TerrainHeightData data = _terrain.GetHeightData();
            Texture map = _terrain.worldHeightmap;
            param.bounds = TerrainUtils.GetBounds(ref data).y;
            param.imageHeight = map.height;
            param.imageWidth = map.width;
            param.scaleX = Math.Abs(Math.Round(_terrain.worldSize.x / map.width, 4, MidpointRounding.AwayFromZero));
            param.scaleY = Math.Abs(Math.Round(_terrain.worldSize.y / map.height, 4, MidpointRounding.AwayFromZero));
            GeoTiff.WriteHeader(writer, ref param);
            GeoTiff.Parameter _param = param;
            float seaLevel = WaterSystem.SeaLevel;

            float GetDepthAsFloat(ushort value, Bounds1 bounds, float nodata)
            {
                float depth = (float)Math.Round(seaLevel - value * (bounds.max - bounds.min) / 65535, 4, MidpointRounding.AwayFromZero);
                return depth > 0 ? depth : nodata;
            }

            short GetDepthAsShort(ushort value, Bounds1 bounds, float nodata)
            {
                short depth = (short)Math.Round(seaLevel - value * (bounds.max - bounds.min) / 65535, MidpointRounding.AwayFromZero);
                return depth > 0 ? depth : (short)Math.Round((double)nodata, MidpointRounding.AwayFromZero);
            }

            ushort GetDepthAsUShort(ushort value, Bounds1 bounds, float nodata)
            {
                float depth = seaLevel / (bounds.max - bounds.min) * 65535 - value;
                if (depth > 0)
                {
                    if ((depth < ushort.MaxValue) & (depth > ushort.MinValue))
                    {
                        return (ushort)Math.Round((double)depth, MidpointRounding.AwayFromZero);
                    }
                }

                return (ushort) nodata;
            }

            // Note: The lifecycle of `elevation` is managed by SharedDataCollectionSystem, and disposing of `elevation` directly would result in double disposal.
            // （註：`elevation` 的生命週期由 SharedDataCollectionSystem 管理，直接丟棄將導致重複丟棄的情況。）
            NativeArray<ushort> elevation = _shared.WorldElevation;

            try
            {
                Task writerThread = Task.Run(() =>
                {
                    GeoTiff.ValidateGrid(ref elevation, _param);

                    switch (_param.format)
                    {
                        case GeoTiffFormat.Float32:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(GetDepthAsFloat(elevation[i * _param.imageWidth + j], _param.bounds, _param.nodata)));
                                }
                            }
                            break;

                        case GeoTiffFormat.Int16:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(GetDepthAsShort(elevation[i * _param.imageWidth + j], _param.bounds, _param.nodata)));
                                }
                            }
                            break;

                        case GeoTiffFormat.Norm16:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(GetDepthAsUShort(elevation[i * _param.imageWidth + j], _param.bounds, _param.nodata)));
                                }
                            }
                            break;
                    }

                    GeoTiff.WriteGridDataCommon(writer, _param.BytesPerStrip(), _param.imageHeight);
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        /// Write the "world" elevation to the file.
        /// （寫入「世界」高程至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public void WriteWorldElevation(BinaryWriter writer, ref GeoTiff.Parameter param)
        {
            // Prepare metadata.（準備元資料。）
            TerrainHeightData data = _terrain.GetHeightData();
            Texture map = _terrain.worldHeightmap;
            param.bounds = TerrainUtils.GetBounds(ref data).y;
            param.imageHeight = map.height;
            param.imageWidth = map.width;
            param.scaleX = Math.Abs(Math.Round(_terrain.worldSize.x / map.width, 4, MidpointRounding.AwayFromZero));
            param.scaleY = Math.Abs(Math.Round(_terrain.worldSize.y / map.height, 4, MidpointRounding.AwayFromZero));
            GeoTiff.WriteHeader(writer, ref param);
            GeoTiff.Parameter _param = param;

            // Note: The lifecycle of `elevation` is managed by SharedDataCollectionSystem, and disposing of `elevation` directly would result in double disposal.
            // （註：`elevation` 的生命週期由 SharedDataCollectionSystem 管理，直接丟棄將導致重複丟棄的情況。）
            NativeArray<ushort> elevation = _shared.WorldElevation;

            try
            {
                Task writerThread = Task.Run(() =>
                {
                    GeoTiff.ValidateGrid(ref elevation, _param);

                    switch (_param.format)
                    {
                        case GeoTiffFormat.Float32:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(DenormalizeToFloat(elevation[i * _param.imageWidth + j], _param.bounds)));
                                }
                            }
                            break;

                        case GeoTiffFormat.Int16:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(DenormalizeToShort(elevation[i * _param.imageWidth + j], _param.bounds)));
                                }
                            }
                            break;

                        case GeoTiffFormat.Norm16:
                            for (int i = _param.imageHeight - 1; i > -1; i--)
                            {
                                for (int j = 0; j < _param.imageWidth; j++)
                                {
                                    writer.Write(BitConverter.GetBytes(elevation[i * _param.imageWidth + j]));
                                }
                            }
                            break;
                    }

                    GeoTiff.WriteGridDataCommon(writer, _param.BytesPerStrip(), _param.imageHeight);
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
    }
}