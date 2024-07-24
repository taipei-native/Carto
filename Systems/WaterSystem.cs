namespace Carto.Systems
{
    using Carto.Geodata;
    using Colossal.Logging;
    using Game;
    using Game.Simulation;
    using Purpose = Colossal.Serialization.Entities.Purpose;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Carto.Utils;

    /// <summary>
    /// The system instance that manages the water body properties, inheriting from GameSystemBase.
    /// （管理水體屬性的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class WaterSystem : GameSystemBase
    {
        private static readonly ILog m_Log = Instance.Log;
        private static readonly Game.Simulation.WaterSystem m_Water = Instance.GameWater;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Log.Debug("WaterSystem instance created. 水體系統實例創造完成。");
        }

        /// <summary>
        /// This event triggers when the game is loaded.
        /// （這是當遊戲載入完成時，會被觸發的事件。）
        /// </summary>
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode) { }

        /// <summary>
        /// This event triggers when the system is updated.
        /// （這是當系統實例被更新時，會被觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// This event triggers when the system is destroyed.
        /// （這是當系統實例被銷毀時，會被觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// Searching for properties of existing water bodies.
        /// （搜尋現有水體的屬性。）
        /// </summary>
        public static CartoObject GetWaterProperties()
        {
            var values = new Dictionary<string, float[,]>();
            var props = new Dictionary<string, object>();
            var type = new Dictionary<string, string>();

            try
            {
                if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Water, "Depth"))
                {
                    WaterSurfaceData _water = m_Water.GetSurfaceData(out _);
                    var watrOriginalData = _water.depths.ToArray();
                    props["Object"] = "Water";

                    // Retrive the raster properties.
                    //（獲取網格屬性。）
                    Dictionary<string, object> watrMatrix = new Dictionary<string, object>
                    {
                        { "offset", _water.offset },
                        { "resolution", _water.resolution },
                        { "scale", _water.scale }
                    };
                    props["Matrix"] = watrMatrix;

                    // Retrieve the water depth matrices.
                    // （獲取水深矩陣。）
                    var watrDepth = new float[_water.resolution.x, _water.resolution.z];

                    Task[] tasks = new Task[_water.resolution.z];

                    for (int i = 0; i < _water.resolution.z; i++)
                    {
                        int z = i;
                        tasks[z] = Task.Run(() =>
                        {
                            for (int j = 0; j < _water.resolution.x; j++)
                            {
                                float val = watrOriginalData[j + (_water.resolution.z - 1 - z) * _water.resolution.x].m_Depth;
                                val = (val == 0) ? 1.70141E+38f : val;
                                watrDepth[z, j] = val;
                            }
                        });
                    }

                    Task.WaitAll(tasks);
                    values["Depth"] = watrDepth;
                    type["Depth"] = CartoObject.R;
                }
            }
            catch (Exception ex)
            {
                m_Log.Error($"An error occured at GetWaterProperties(); 於 GetWaterProperties() 發生一個錯誤； {ex}");
            }

            return new CartoObject(values, props, type);
        }
    }
}