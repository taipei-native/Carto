using Carto.IO;
using Colossal.Logging;
using Game;
using Game.Areas;
using Game.Common;
using Game.Tools;
using Newtonsoft.Json;
using Unity.Entities;

namespace Carto.Systems
{
    /// <summary>
    /// The dummy system that is only used for development purposes.
    /// （用於開發用途的虛假系統。）
    /// </summary>
    public partial class DummySystem : GameSystemBase
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;
        
        /// <summary>
        /// The query for existing districts.（現有行政區的查詢。）
        /// </summary>
        static EntityQuery _districtQuery;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _districtQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<District>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            base.OnCreate();
            _log.Debug("DummySystem instance created. 虛假系統實例創造完成。");
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
        /// Write features (geometries and properties) to the designated file.
        /// （寫出圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        public void WriteFeatures(JsonTextWriter writer, Options options)
        {
            
        }
    }
}