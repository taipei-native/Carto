using Carto.Systems;
using Carto.Utils;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;

namespace Carto
{
    public class Mod : IMod
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The event triggered when the mod instance is being loaded.
        /// （當模組實例被載入時觸發的事件。）
        /// </summary>
        /// <param name="updateSystem">The system managing game updates.（管理遊戲狀態更新的系統。）</param>
        public void OnLoad(UpdateSystem updateSystem)
        {
            // Register settings in the options page.
            // （將設定註冊在選項頁面。）
            Instance.Settings = new Settings(this);
            Instance.Settings.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(nameof(Carto), Instance.Settings, new Settings(this));

            // Load the locales to the game.
            // （載入語系檔案至遊戲中。）
            LocaleUtils.Load();

            // Register system instances into the game.
            // （將系統實例註冊在遊戲中。）
            updateSystem.UpdateBefore<AreaSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<BuildingSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<NetworkSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<RasterSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<SharedDataCollectionSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<ZoningSystem>(SystemUpdatePhase.GameSimulation);

            _log.Info("Mod instance loaded. 模組實例載入完成。");
        }

        /// <summary>
        /// The event triggered when the mod instance is being disposed of.
        /// （當模組實例被銷毀時觸發的事件。）
        /// </summary>
        public void OnDispose()
        {
            if (Instance.Settings != null)
            {
                Instance.Settings.UnregisterInOptionsUI();
                Instance.Settings = null;
            }

            _log.Info("Mod instance had been disposed of. 模組實例已被銷毀。");
        }
    }
}
