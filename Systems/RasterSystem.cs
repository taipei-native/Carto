using Carto.IO;
using Colossal.Logging;
using Game;
using Game.Simulation;
using System;
using System.IO;
using System.Threading.Tasks;
using Unity.Collections;

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
        /// The system managing the terrain.（管理地形的系統。）<br/>
        /// See <see cref="Instance.Terrain"/> for more information.
        /// </summary>
        static readonly TerrainSystem _terrain = Instance.Terrain;

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
        /// Write the elevation to the file.
        /// （寫入高程至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="param">GeoTIFF's meta data.（GeoTIFF 的元資料。）</param>
        public void WriteElevation(BinaryWriter writer, ref GeoTiff.Parameter param)
        {
            float GetFloatElevation(GeoTiff.Parameter p, ushort value)
            {
                return (float)Math.Round(value * (p.bounds.max - p.bounds.min) / 65535, 4);
            }

            short GetShortElevation(GeoTiff.Parameter p, ushort value)
            {
                return (short)Math.Round(value * (p.bounds.max - p.bounds.min) / 65535);
            }

            // Prepare metadata.（準備元資料。）
            TerrainHeightData data = _terrain.GetHeightData();
            param.bounds = TerrainUtils.GetBounds(ref data).y;
            param.imageHeight = data.resolution.z;
            param.imageWidth = data.resolution.x;
            GeoTiff.WriteHeader(writer, ref param);
            GeoTiff.Parameter _param = param;

            try
            {
                Task writerThread = Task.Run(() =>
                {
                    NativeArray<ushort> elevation = data.heights;
                    
                    switch (_param.format)
                    {
                        case GeoTiffFormat.Float32:
                            GeoTiff.WriteGridData(writer, ref elevation, _param, GetFloatElevation);
                            break;

                        case GeoTiffFormat.Int16:
                            GeoTiff.WriteGridData(writer, ref elevation, _param, GetShortElevation);
                            break;

                        case GeoTiffFormat.Norm16:
                            GeoTiff.WriteGridData<ushort, ushort>(writer, ref elevation, _param); 
                            break;
                    }
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