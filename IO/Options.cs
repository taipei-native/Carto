using Carto.Utils;
using Colossal.PSI.Environment;
using System.IO;

namespace Carto.IO
{
    /// <summary>
    /// The class to store input / output options.
    /// （儲存輸出選項的類別。）
    /// </summary>
    public class Options
    {
        /// <summary>
        /// The path to the target directory.
        /// （目標目錄的路徑。）
        /// </summary>
        public string Directory { get; set; } = IOUtils.CombinePath(EnvPath.kUserDataPath, "ModsData", nameof(Carto));

        /// <summary>
        /// The format of the target file.
        /// （目標檔案的格式。）
        /// </summary>
        public FileFormat FileFormat { get; set; } = FileFormat.Unknown;

        /// <summary>
        /// The target file's name.
        /// （目標檔案的名稱。）
        /// </summary>
        public string FileName { get; set; } = "output";

        /// <summary>
        /// The path to the target file.
        /// （目標檔案的路徑。）
        /// </summary>
        public string FilePath => Path.ChangeExtension(IOUtils.CombinePath(Directory, FileName), IO.FileExtensionTable.TryGetValue(FileFormat, out string extension) ? extension : null);

        /// <summary>
        /// Whether to export the minimized JSON file or not.
        /// （是否要輸出最小化的 JSON？）
        /// </summary>
        public bool Minimize { get; set; } = true;
    }
}