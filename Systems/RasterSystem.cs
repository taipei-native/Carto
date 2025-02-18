using Carto.IO;
using Colossal.Logging;
using Game;
using System.IO;

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
        /// 
        /// </summary>
        public void WriteElevation(BinaryWriter writer, ref GeoTiff.Parameter param)
        {

        }
    }
}