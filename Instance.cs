namespace Carto
{
    using Carto.Systems;
    using Carto.Utils;
    using Colossal.Localization;
    using Colossal.Logging;
    using Game;
    using Game.City;
    using Game.Modding;
    using Game.Prefabs;
    using Game.SceneFlow;
    using Game.UI;
    using System.IO;
    using System.Reflection;
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

        /// <summary>
        /// The Carto mod's ZoningSystem instance.
        /// （Carto模組的ZoningSystem個體。）
        /// </summary>
        public static ZoningSystem Zoning => World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<ZoningSystem>();

        /// <summary>
        /// Zone Color Changer mod developed by TDW.
        /// （由TDW開發的Zone Color Changer模組。）
        /// </summary>
        //  PDX Mods link: https://mods.paradoxplaza.com/mods/81568/Windows
        //  （PDX Mods 連結：https://mods.paradoxplaza.com/mods/81568/Windows）
        public static class ZCCMod
        {
            private static Assembly assembly;
            private static bool ready = false;
            private static string version = string.Empty;

            /// <summary>
            /// The constructor of Carto.Instance.ZCCMod.
            /// （Carto.Instance.ZCCMod的建構函式。）
            /// </summary>
            static ZCCMod()
            {
                try
                {
                    foreach (ModManager.ModInfo modInfo in GameManager.instance.modManager)
                    {
                        if (modInfo.name.StartsWith("ZoneColorChanger"))
                        {
                            assembly = modInfo.asset.assembly;
                            ready = true;
                            version = assembly.GetName().Version.ToString();
                            Log.Debug($"Instance.ZCCMod: Successfully retrieve the assembly of Zone Color Changer [{version}]. 成功獲取 Zone Color Changer [{version}] 模組組件。");
                            break;
                        }
                    }

                    if (!ready) Log.Debug($"Instance.ZCCMod: Failed to retrieve the assembly of Zone Color Changer. 無法獲取 Zone Color Changer 模組組件。");
                }
                catch
                {
                    Log.Debug($"Instance.ZCCMod: Zone Color Changer mod is not loaded. Zone Color Changer 模組尚未載入。");
                }
            }

            /// <summary>
            /// The assembly of the mod.
            /// （模組的組件。）
            /// </summary>
            public static Assembly Assembly
            {
                get
                {
                    if (assembly == null )
                    {
                        throw new FileNotFoundException("Zone Color Changer mod is not loaded. Zone Color Changer 模組尚未載入。");
                    }
                    return assembly;
                }
            }

            /// <summary>
            /// The activation status of the mod.
            /// （模組的啟用狀態。）
            /// </summary>
            public static bool Ready => ready;

            /// <summary>
            /// The version of the assembly of the mod which works smoothly with Carto mod.
            /// （能與Carto模組正常運行的模組組件版本。）
            /// </summary>
            public const string SuggestedVersion = "1.1.0.0";

            /// <summary>
            /// The version of the assembly.
            /// （模組組件的版本。）
            /// </summary>
            public static string Version => version;
        }
    }
}