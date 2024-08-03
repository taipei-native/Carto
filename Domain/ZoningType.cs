namespace Carto.Domain
{
    using Carto.Utils;
    using Unity.Entities;
    using ZoningCategory = Systems.ZoningSystem.ZoningCategory;
    using ZoningDensity = Systems.ZoningSystem.ZoningDensity;

    /// <summary>
    /// The class to store the metadata of each zoning type.
    /// （儲存各個分區類型基礎資料的類別。）
    /// </summary>
    public class ZoningType
    {
        /// <summary>
        /// The category of the zoning type.
        /// （分區類型的分類。）
        /// </summary>
        public ZoningCategory? Category { get; set; }

        /// <summary>
        /// The rendering color of the zoning type.
        /// （分區類型的渲染顏色。）
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// The estimated density of the zoning type.
        /// （分區類型的密度。）
        /// </summary>
        public ZoningDensity? Density { get; set; }

        /// <summary>
        /// The zoning type entity itself.
        /// （分區類型實體本身。）
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// The UID of each zoning type.
        /// （各個分區類型的唯一代碼。）
        /// </summary>
        public ushort? Index { get; set; }

        /// <summary>
        /// The localized name of the zoning type.
        /// （分區類型的在地化名稱。）
        /// </summary>
        public string Name => LocaleUtils.Translate($"Assets.NAME[{PrefabName}]");

        /// <summary>
        /// The prefab name of the zoning type.
        /// （分區類型的預製模板名稱。）
        /// </summary>
        public string PrefabName { get; set; }

        /// <summary>
        /// The theme of the zoning type.
        /// （分區類型的主題風格。）
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// The method to turn the class into a string.
        /// （用以將類別轉換為字串的方法。）
        /// </summary>
        public override string ToString()
        {
            string notSet = "Not Set";
            string category = (Category == null) ? notSet : $"{Category}";
            string color = (Color == null) ? notSet : Color;
            string density = (Density == null) ? notSet : $"{Density}";
            string index = (Index == null) ? notSet : $"{Index}";
            string name = (PrefabName == null) ? notSet : Name;
            string theme = (Theme == null) ? notSet : Theme;
            return $"ZoningTypeInfo({index} [{name}], Category = {{{category}}}, Density = {density}, Theme = {theme}, Color = {color})";
        }
    }
}