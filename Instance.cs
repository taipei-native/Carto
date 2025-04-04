using Carto.Systems;
using Colossal.Localization;
using Colossal.Logging;
using Game;
using Game.City;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using Game.UI;
using System.Reflection;
using Unity.Entities;

namespace Carto
{
    /// <summary>
    /// The class that provides unified access to current instances.
    /// （提供統一存取現有實例途徑的類別。）
    /// </summary>
    public static class Instance
    {
        // Game instances（遊戲的實例）
        /// <summary>
        /// The system managing the initializing options.
        /// （管理程式初始化選項的系統。）
        /// </summary>
        public static CityConfigurationSystem City => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CityConfigurationSystem>();

        /// <summary>
        /// The current game mode.
        /// （目前的遊戲模式。）
        /// </summary>
        public static GameMode GameMode => GameManager.instance.gameMode;

        /// <summary>
        /// The manager of the localization data.
        /// （語系資料的管理者。）
        /// </summary>
        public static LocalizationManager Localization => GameManager.instance.localizationManager;

        /// <summary>
        /// The system querying map data.
        /// （查詢地圖資料的系統。）
        /// </summary>
        public static MapMetadataSystem Map => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<MapMetadataSystem>();

        /// <summary>
        /// The system managing the names of each entity.
        /// （管理各實體名稱的系統。）
        /// </summary>
        public static NameSystem Name => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NameSystem>();

        /// <summary>
        /// The system managing the prefabricated data.
        /// （管理預製模板資料的系統。）
        /// </summary>
        public static PrefabSystem Prefab => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();

        /// <summary>
        /// The system managing the game simulation.
        /// （管理遊戲模擬的系統。）
        /// </summary>
        public static SimulationSystem Simulation => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SimulationSystem>();

        /// <summary>
        /// The system managing the terrain.
        /// （管理地形的系統。）
        /// </summary>
        public static TerrainSystem Terrain => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<TerrainSystem>();

        /// <summary>
        /// The system managing simulation time synchronization.
        /// （管理遊戲模擬時間同步的系統。）
        /// </summary>
        public static TimeSystem Time => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<TimeSystem>();

        /// <summary>
        /// The system managing the water.
        /// （管理水體的系統。）
        /// </summary>
        public static WaterSystem Water => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<WaterSystem>();

        // Carto instances（Carto 的實例）
        /// <summary>
        /// The system that searches areas.
        /// （搜尋區域的系統。）
        /// </summary>
        public static AreaSystem Area => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<AreaSystem>();

        /// <summary>
        /// The assembly of the Carto mod.
        /// （Carto 的程式組件。）
        /// </summary>
        public static Assembly Assembly => Assembly.GetExecutingAssembly();

        /// <summary>
        /// The dedicated logger documenting the information, warnings, and errors.
        /// （記錄執行時資訊、警告或錯誤的記錄器。）
        /// </summary>
        public static ILog Log { get; } = LogManager.GetLogger(nameof(Carto)).SetShowsErrorsInUI(false);

        /// <summary>
        /// The system that handles grid data.
        /// （處理網格資料的系統。）
        /// </summary>
        public static RasterSystem Raster => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<RasterSystem>();

        /// <summary>
        /// The options to change mod behaviors.
        /// （改變模組執行方式的設定。）
        /// </summary>
        public static Settings Settings { get; set; }

        /// <summary>
        /// The system that collects shared data across various systems.
        /// （收集多種系統所需之共享資料的系統。）
        /// </summary>
        public static SharedDataCollectionSystem Shared => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SharedDataCollectionSystem>();
    }
}