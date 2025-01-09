namespace Carto.IO
{
    /// <summary>
    /// The class to store input / output options.
    /// （儲存輸出選項的類別。）
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Whether to export the minimized JSON file or not.
        /// （是否要輸出最小化的 JSON？）
        /// </summary>
        public bool Minimize { get; set; } = true;
    }
}