using Carto.Domain;
using Carto.Systems;
using Colossal.Localization;
using Colossal.Logging;
using Colossal.PSI.Environment;
using Game;
using Game.Audio;
using Game.City;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Settings;
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
        /// The system managing sound effects.
        /// （管理音效的系統。）
        /// </summary>
        public static AudioManager Audio => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<AudioManager>();

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
        /// The manager of the mod assemblies.
        /// （模組組件的管理者。）
        /// </summary>
        public static ModManager Mod => GameManager.instance.modManager;

        /// <summary>
        /// The system querying map data.
        /// （查詢地圖資料的系統。）
        /// </summary>
        public static MapMetadataSystem Map => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<MapMetadataSystem>();

        /// <summary>
        /// The system managing the prefabricated data.
        /// （管理預製模板資料的系統。）
        /// </summary>
        public static PrefabSystem Prefab => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();

        /// <summary>
        /// The vanilla settings.
        /// （原版遊戲的設定。）
        /// </summary>
        public static SharedSettings SharedSettings => GameManager.instance.settings;

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
        /// The user interface manager of the game.
        /// （遊戲的使用者介面管理器。）
        /// </summary>
        public static UserInterface UI => GameManager.instance.userInterface;

        /// <summary>
        /// The path to the user data folder.
        /// （指向使用者資料的路徑。）
        /// </summary>
        public static string UserDataPath => EnvPath.kUserDataPath;

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
        /// The system that searches buildings.
        /// （搜尋建築的系統。）
        /// </summary>
        public static BuildingSystem Building => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<BuildingSystem>();

        /// <summary>
        /// The path to Carto's data directory.
        /// （指向 Carto 資料目錄的路徑。）
        /// </summary>
        public static string CartoDataPath => Utils.IOUtils.CombinePath(UserDataPath, "ModsData", nameof(Carto));

        /// <summary>
        /// The dedicated logger documenting the information, warnings, and errors.
        /// （記錄執行時資訊、警告或錯誤的記錄器。）
        /// </summary>
        public static ILog Log { get; } = LogManager.GetLogger(nameof(Carto)).SetShowsErrorsInUI(true);

        /// <summary>
        /// The warapper of vanilla <see cref="NameSystem"/>, which manages the name of each entity.
        /// （遊戲原生 <see cref="NameSystem"/> 的包裝器，該系統管理著各實體的名稱。）
        /// </summary>
        public static NameManager Name { get; } = new();

        /// <summary>
        /// The system that searches networks.
        /// （搜尋網路的系統。）
        /// </summary>
        public static NetworkSystem Network => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NetworkSystem>();

        /// <summary>
        /// The system that searches point of interests (POIs).
        /// （搜尋興趣點（POI）的系統。）
        /// </summary>
        public static POISystem POI => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<POISystem>();

        /// <summary>
        /// The system that handles grid data.
        /// （處理網格資料的系統。）
        /// </summary>
        public static RasterSystem Raster => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<RasterSystem>();

        /// <summary>
        /// The wrapper of the assembly of Road Builder mod developed by TDW.<br/>
        /// （由 TDW 開發的 Road Builder 模組組件的包裝器。）
        /// </summary>
        public static RoadBuilder Rb { get; } = new();

        /// <summary>
        /// The system that searches transportation routes.
        /// （搜尋運輸服務路線的系統。）
        /// </summary>
        public static RouteSystem Route => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<RouteSystem>();

        /// <summary>
        /// The wrapper of the assembly of Road Speed Adjuster mod developed by DanielVNZ.<br/>
        /// （由 DanielVNZ 開發的 Road Speed Adjuster 模組組件的包裝器。）
        /// </summary>
        public static RoadSpeedAdjuster Rsa { get; } = new();

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

        /// <summary>
        /// The system that manages sounds.
        /// （管理聲音的系統。）
        /// </summary>
        public static SoundSystem Sound => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SoundSystem>();

        /// <summary>
        /// The current running Carto version.
        /// （目前執行中的 Carto 版本。）
        /// </summary>
        public static string Version => Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

        /// <summary>
        /// The wrapper of the assembly of Extended Transport Manager mod developed by klyte45.<br/>
        /// （由 klyte45 開發的 Extended Transport Manager 模組組件的包裝器。）
        /// </summary>
        public static ExtendedTransportManager Xtm { get; } = new(); 

        /// <summary>
        /// The wrapper of the assembly of Zone Color Changer mod developed by TDW.<br/>
        /// （由 TDW 開發的 Zone Color Changer 模組組件的包裝器。）
        /// </summary>
        public static ZoneColorChanger Zcc { get; } = new();

        /// <summary>
        /// The system that searches zoning blocks.
        /// （搜尋分區的系統。）
        /// </summary>
        public static ZoningSystem Zoning => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<ZoningSystem>();
    }
}