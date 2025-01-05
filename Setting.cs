using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;

namespace Carto
{
    /// <summary>
    /// The class that manages the mod's options.
    /// （管理模組設定的類別。）
    /// </summary>
    [FileLocation("ModsSettings" + "\\" + nameof(Carto) + "\\" + nameof(Carto) + "_v1")]
    public class Setting : ModSetting
    {
        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        public override void SetDefaults() { }
    }
}
