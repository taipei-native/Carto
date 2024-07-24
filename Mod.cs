namespace Carto
{
    using Carto.Systems;
    using Carto.Utils;
    using Colossal.IO.AssetDatabase;
    using Colossal.Json;
    using Game;
    using Game.Modding;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The mod instance using IMod interface.
    /// （使用 IMod 介面的模組實例。）
    /// </summary>
    public sealed class Mod : IMod
    {
        /// <summary>
        /// This event triggers when the mod instance is loaded.
        /// （這是當模組實例載入完成時，會被觸發的事件。）
        /// </summary>
        /// <param name="updateSystem">
        /// The system managing ingame status updates.
        /// （管理遊戲內狀態更新的系統）
        /// </param>
        public void OnLoad(UpdateSystem updateSystem)
        {
            // Create the content folder.
            // （創造內容資料夾。）
            Directory.CreateDirectory(Setting.ContentFolder);
            Directory.CreateDirectory(Setting.SettingFolder);

            // Register settings in the options page.
            // （將設定註冊在選項頁面。）
            Instance.Settings = new Setting(this);
            Instance.Settings.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(nameof(Carto), Instance.Settings, new Setting(this));
            Instance.Settings.LoadLocalSettings();

            // Append locales to the existing ones.
            // （將語系檔案添加至既有的檔案。）
            LocaleUtils.BatchAddLocaleFromEmbedded();

            // Register system instances into the game.
            // （將系統實例註冊在遊戲中。）
            updateSystem.UpdateBefore<AreaSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<NetSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<TerrainSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<WaterSystem>(SystemUpdatePhase.GameSimulation);

            Instance.Log.Info("Mod instance loaded. 模組實例載入完成。");
        }

        /// <summary>
        /// This event triggers when the mod instance is disposed of.
        /// （這是當模組實例被銷毀時，會被觸發的事件。）
        /// </summary>
        public void OnDispose()
        {
            if (Instance.Settings != null)
            {
                Instance.Settings.UnregisterInOptionsUI();
                Instance.Settings = null;
            }

            Instance.Log.Info("Mod instance is being disposed of. 模組實例正被銷毀。");
        }
    }
}
