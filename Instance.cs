namespace Carto
{
    using Carto.Systems;
    using Carto.Utils;
    using Colossal.Localization;
    using Colossal.Logging;
    using Game;
    using Game.City;
    using Game.Prefabs;
    using Game.SceneFlow;
    using Game.UI;
    using Unity.Entities;

    public class Instance
    {
        /// <summary>
        /// The Carto mod's AreaSystem instance.
        /// （Carto模組的AreaSystem個體。）
        /// </summary>
        public static AreaSystem Area => World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AreaSystem>();
        
        /// <summary>
        /// The system managing the city starting options.
        /// （管理城市開始選項的系統。）
        /// </summary>
        public static CityConfigurationSystem City => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CityConfigurationSystem>();

        /// <summary>
        /// The name of the current city.
        /// （目前城市的名稱。）
        /// </summary>
        public static string CityName
        {
            get
            {
                if (Mode == GameMode.Game) return City.cityName;
                return LocaleUtils.Translate("Carto.export.UNKNOWN[City]");
            }
        }

        /// <summary>
        /// The container dealing with the localization.
        /// （處理在地化的容器。）
        /// </summary>
        public static LocalizationManager Localization => GameManager.instance.localizationManager;

        /// <summary>
        /// The logger documenting the information, warnings, and errors as needed.
        /// （記錄執行時資訊、警告或錯誤的記錄器。）
        /// </summary>
        public static ILog Log => LogManager.GetLogger(nameof(Carto)).SetShowsErrorsInUI(false);

        /// <summary>
        /// The system querying the map abstracts.
        /// （查詢地圖概要的系統。）
        /// </summary>
        public static MapMetadataSystem Map => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<MapMetadataSystem>();

        /// <summary>
        /// The name of the current map.
        /// （目前地圖的名稱。）
        /// </summary>
        public static string MapName
        {
            get
            {
                if (Mode == GameMode.Game) return LocaleUtils.Translate($"Maps.MAP_TITLE[{Map.mapName}]");
                return LocaleUtils.Translate("Carto.export.UNKNOWN[Map]");
            }
        }

        /// <summary>
        /// The current game mode (main menu, game, or editor.)
        /// （目前的遊戲模式（主目錄、遊戲內、編輯器）。）
        /// </summary>
        public static GameMode Mode => GameManager.instance.gameMode;

        /// <summary>
        /// The system managing names of objects.
        /// （管理物件名稱的系統。）
        /// </summary>
        public static NameSystem Name => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NameSystem>();

        /// <summary>
        /// The Carto mod's NetSystem instance.
        /// （Carto模組的NetSystem個體。）
        /// </summary>
        public static NetSystem Net => World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<NetSystem>();

        /// <summary>
        /// The system managing prefabricated templates of objects.
        /// （管理物件預製組件模板的系統。）
        /// </summary>
        public static PrefabSystem Prefab => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();

        /// <summary>
        /// The options to change how the mods execute.
        /// （改變模組運作的選項。）
        /// </summary>
        public static Setting Settings { get; set; }

        /// <summary>
        /// The system managing the transformation from the heightmap to the world terrain.
        /// （管理由高度圖至世界地形轉換的系統。）
        /// </summary>
        public static Game.Simulation.TerrainSystem GameTerrain => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<Game.Simulation.TerrainSystem>();

        /// <summary>
        /// The Carto mod's TerrainSystem instance.
        /// （Carto模組的TerrainSystem個體。）
        /// </summary>
        public static TerrainSystem ModTerrain => World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<TerrainSystem>();

        /// <summary>
        /// The traffic handedness of the current save.
        /// （目前存檔的交通通行方向。）
        /// </summary>
        public static string Traffic
        {
            get
            {
                if (City.leftHandTraffic) return "Left";
                return "Right";
            }
        }

        /// <summary>
        /// The system managing the world water allocation.
        /// （管理世界內水的安置的系統。）
        /// </summary>
        public static Game.Simulation.WaterSystem GameWater => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<Game.Simulation.WaterSystem>();

        /// <summary>
        /// The Carto mod's WaterSystem instance.
        /// （Carto模組的WaterSystem個體。）
        /// </summary>
        public static WaterSystem ModWater => World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<WaterSystem>();
    }
}