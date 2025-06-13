namespace Carto.Domain
{
    /// <summary>
    /// The container of literal address information.
    /// （文字地址的容器。）
    /// </summary>
    public struct LiteralAddress
    {
        /// <summary>
        /// The administrative body.
        /// （行政區。）
        /// </summary>
        public string district;

        /// <summary>
        /// The house number.
        /// （門牌號碼。）
        /// </summary>
        public int number;

        /// <summary>
        /// The street aggregation name.
        /// （街道的聚合名稱。）
        /// </summary>
        public string street;

        /// <summary>
        /// Convert the struct to write-friendly object array.
        /// （將結構轉換成寫出友善的物件陣列。）
        /// </summary>
        /// <returns>The object array.（物件陣列。）</returns>
        public readonly object[] ToArray()
        {
            return new object[] { district, street, number };
        }

        public override readonly string ToString()
        {
            return $"LiteralAddress [{number}, {street}, {district}]";
        }
    }
}