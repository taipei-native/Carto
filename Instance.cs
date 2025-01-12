using Carto.Systems;
using Colossal.Logging;
using Game.UI;
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
        /// The system managing the names of each entity.
        /// （管理各實體名稱的系統。）
        /// </summary>
        public static NameSystem Name => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NameSystem>();

        // Carto instances（Carto 的實例）
        /// <summary>
        /// The dummy system that is only used for development purposes.
        /// （用於開發用途的虛假系統。）
        /// </summary>
        public static DummySystem Dummy => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<DummySystem>();

        /// <summary>
        /// The dedicated logger documenting the information, warnings, and errors.
        /// （記錄執行時資訊、警告或錯誤的記錄器。）
        /// </summary>
        public static ILog Log { get; } = LogManager.GetLogger(nameof(Carto)).SetShowsErrorsInUI(false);

        /// <summary>
        /// The options to change mod behaviors.
        /// （改變模組執行方式的設定。）
        /// </summary>
        public static Settings Settings { get; set; }
    }
}