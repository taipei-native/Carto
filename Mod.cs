using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;

namespace Carto
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger(nameof(Carto)).SetShowsErrorsInUI(false);
        private Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            //GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(nameof(Carto), m_Setting, new Setting(this));
        }

        public void OnDispose()
        {
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}
