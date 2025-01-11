using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.Settings;

namespace Carto
{
    /// <summary>
    /// The class that manages the mod's options.
    /// （管理模組設定的類別。）
    /// </summary>
    [FileLocation("ModsSettings" + "\\" + nameof(Carto) + "\\" + nameof(Carto) + "_v1")]
    public class Settings : ModSetting
    {
        public Settings(IMod mod) : base(mod) { SetDefaults(); }

        /// <summary>
        /// Reset all mod default settings.
        /// （重置所有模組設定。）
        /// </summary>
        public override void SetDefaults() { }

        [SettingsUIHidden]
        public bool InGameOrEditor => GameMode.GameOrEditor.HasFlag(GameManager.instance.gameMode);

        [SettingsUIButton]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(InGameOrEditor), true)]
        public bool ExportButton
        {
            set { IO.IO.Export(); }
        }
    }
}
