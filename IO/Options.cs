using Carto.Geodata;
using Carto.Utils;
using Colossal.PSI.Environment;
using System;
using System.Collections.Generic;
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
        /// Whether to export all applicable categories for the specific property or not.<br/>
        /// （是否要輸出特定屬性的所有適合分類？）
        /// </summary>
        public Dictionary<(Property, System), bool> Display { get; set; }

        /// <summary>
        /// Whether to export the elevation or not.
        /// （是否要輸出高程？）
        /// </summary>
        public bool Elevation { get; set; } = false;

        /// <summary>
        /// The feature types about to export.
        /// （即將輸出的向量圖徵分類。）
        /// </summary>
        public Feature Features { get; set; } = Feature.None;

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
        public string FilePath
        {
            get
            {
                string formatDirectory = Enum.GetName(typeof(FileFormat), FileFormat);
                string extension = IO.FileExtensionTable.TryGetValue(FileFormat, out string _extension) ? _extension : null;
                return Path.ChangeExtension(IOUtils.CombinePath(Directory, formatDirectory, FileName), extension);
            }
        }

        /// <summary>
        /// Whether to export the minimized JSON file or not.
        /// （是否要輸出最小化的 JSON？）
        /// </summary>
        public bool Minimized { get; set; } = true;

        /// <summary>
        /// The properties about to export.
        /// （即將輸出的屬性。）
        /// </summary>
        public Dictionary<System, HashSet<Property>> Properties { get; set; }

        /// <summary>
        /// The raster grids about to export.
        /// （即將輸出的網格。）
        /// </summary>
        public RasterKind RasterKinds { get; set; } = RasterKind.Unknown;

        /// <summary>
        /// The map center's coordinates in Transverse Mercator.
        /// （橫麥卡托投影中的地圖中心坐標。）
        /// </summary>
        public Coord SourceCoordinates { get; set; } = new Coord((0, 0), 0);

        /// <summary>
        /// The coordinate reference system (CRS) of source coordinates.<br/>
        /// （來源坐標的坐標參考系統。）
        /// </summary>
        public CRS SourceProjection { get; set; } = CRS.UTM;

        /// <summary>
        /// The custom source projection defined by the user.
        /// （使用者定義的來源投影法。）
        /// </summary>
        public ProjectionDefinition SourceProjectionDefinition { get; set; } = new
        (
            new EllipsoidDefinition(Ellipsoid.WGS84),
            (0, 0),
            (5E6, 0),
            0.9996,
            new double[0]
        );

        /// <summary>
        /// The systems engaged in the export.
        /// （參與輸出的系統。）
        /// </summary>
        public System Systems { get; set; } = System.Unknown;

        /// <summary>
        /// The coordinate reference system (CRS) of target coordinates.<br/>
        /// （目標的坐標參考系統。）
        /// </summary>
        public CRS TargetProjection { get; set; } = CRS.UTM;

        /// <summary>
        /// The custom target projection defined by the user.
        /// （使用者定義的目標投影法。）
        /// </summary>
        public ProjectionDefinition TargetProjectionDefinition { get; set; } = new
        (
            new EllipsoidDefinition(Ellipsoid.WGS84),
            (0, 0),
            (5E6, 0),
            0.9996,
            new double[0]
        );

        /// <summary>
        /// The vector geometries about to export.
        /// （即將輸出的向量圖形。）
        /// </summary>
        public Dictionary<System, VectorKind> VectorKinds { get; set; }
    }
}