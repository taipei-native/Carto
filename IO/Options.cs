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
        /// Whether to regard asset packs as themes?
        /// （是否要將資產包視為建築風格？）
        /// </summary>
        public bool AssetPack { get; set; } = true;

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
        /// Whether to count homeless citizens in the household / resident field or not.<br/>
        /// （是否要在家庭／居民欄位計入無家可歸的市民？）
        /// </summary>
        public bool Homeless { get; set; } = true;

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
        /// Whether to export female and male residents separately or not.<br/>
        /// （是否要將男性與女性市民分別輸出？）
        /// </summary>
        public bool SeparateResident { get; set; } = false;

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
        /// Whether to export statistics (e.g. age, company, employee, household, ...) for map tiles?
        /// （是否要輸出地圖區塊的統計資料？（例如年齡、公司、員工、家庭……）？）
        /// </summary>
        public bool StatisticsMapTile { get; set; } = false;

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
        /// Whether to export taxable income. If false, the gross income are exported.<br/>
        /// （是否要輸出應納稅所得。若為否，則輸出總所得。）
        /// </summary>
        public bool Taxable { get; set; } = false;

        /// <summary>
        /// The vector geometries about to export.
        /// （即將輸出的向量圖形。）
        /// </summary>
        public Dictionary<System, VectorKind> VectorKinds { get; set; }

        /// <summary>
        /// Check whether a property exist in any system.
        /// （確認屬性是否存在於任意系統。）
        /// </summary>
        /// <param name="property">The property to be checked.（待檢查的屬性。）</param>
        /// <returns>Return <see cref="true"/> if the property exists.（若屬性存在，回傳 <see cref="true"/>。）</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Contains(Property property)
        {
            PropertiesChecker();
            HashSet<Property> properties = new();
            foreach (KeyValuePair<System, HashSet<Property>> kvp in Properties)
            {
                if (kvp.Value != null)
                {
                    properties.UnionWith(kvp.Value);
                }
            }
            return properties.Contains(property);
        }

        /// <summary>
        /// Check whether a property exist in a system.
        /// （確認屬性是否存在於特定系統。）
        /// </summary>
        /// <param name="property">The property to be checked.（待檢查的屬性。）</param>
        /// <param name="system">The specific system.（特定的系統。）</param>
        /// <returns>Return <see cref="true"/> if the property exists.（若屬性存在，回傳 <see cref="true"/>。）</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Contains(Property property, System system)
        {
            PropertiesChecker();
            if (Properties.TryGetValue(system, out HashSet<Property> properties))
            {
                if (properties != null)
                {
                    return properties.Contains(property);
                }
            }
            return false;
        }

        /// <summary>
        /// Check whether all properties exist in at least one system.
        /// （確認所有屬性至少存在於任意一個系統。）
        /// </summary>
        /// <param name="properties">The properties to be checked.（待檢查的屬性。）</param>
        /// <returns>Return <see cref="true"/> if the all properties exist.（若所有屬性皆存在，回傳 <see cref="true"/>。）</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ContainsAll(params Property[] properties)
        {
            PropertiesChecker(properties);
            HashSet<Property> propertiesInputed = new(properties);
            HashSet<Property> propertiesRecorded = new();
            foreach (KeyValuePair<System, HashSet<Property>> kvp in Properties)
            {
                if (kvp.Value != null)
                {
                    propertiesRecorded.UnionWith(kvp.Value);
                }
            }
            return propertiesInputed.IsSubsetOf(propertiesRecorded);
        }

        /// <summary>
        /// Check whether all properties exist in the specific system.
        /// （確認所有屬性存在於特定系統。）
        /// </summary>
        /// <param name="system">The specific system.（特定的系統。）</param>
        /// <param name="properties">The properties to be checked.（待檢查的屬性。）</param>
        /// <returns>Return <see cref="true"/> if the all properties exist.（若所有屬性皆存在，回傳 <see cref="true"/>。）</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ContainsAll(System system, params Property[] properties)
        {
            PropertiesChecker(properties);
            if (Properties.TryGetValue(system, out HashSet<Property> propertiesRecorded))
            {
                if (propertiesRecorded != null)
                {
                    return new HashSet<Property>(properties).IsSubsetOf(propertiesRecorded);
                }
            }
            return false;
        }

        /// <summary>
        /// Check whether any one property exist in at least one system.
        /// （確認至少一個指定屬性存在於任意一個系統。）
        /// </summary>
        /// <param name="properties">The properties to be checked.（待檢查的屬性。）</param>
        /// <returns>Return <see cref="true"/> if any property exist.（若任意屬性存在，回傳 <see cref="true"/>。）</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ContainsAny(params Property[] properties)
        {
            PropertiesChecker(properties);
            HashSet<Property> propertiesInputed = new(properties);
            HashSet<Property> propertiesRecorded = new();
            foreach (KeyValuePair<System, HashSet<Property>> kvp in Properties)
            {
                if (kvp.Value != null)
                {
                    propertiesRecorded.UnionWith(kvp.Value);
                }
            }
            propertiesInputed.IntersectWith(propertiesRecorded);
            return propertiesInputed.Count > 0;
        }

        /// <summary>
        /// Check whether any one property exist in the specific system.
        /// （確認至少一個指定屬性存在於特定系統。）
        /// </summary>
        /// <param name="system">The specific system.（特定的系統。）</param>
        /// <param name="properties">The properties to be checked.（待檢查的屬性。）</param>
        /// <returns>Return <see cref="true"/> if any property exist.（若任意屬性存在，回傳 <see cref="true"/>。）</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ContainsAny(System system, params Property[] properties)
        {
            PropertiesChecker(properties);
            if (Properties.TryGetValue(system, out HashSet<Property> propertiesRecorded))
            {
                if (propertiesRecorded != null)
                {
                    propertiesRecorded.IntersectWith(new HashSet<Property>(properties));
                    return propertiesRecorded.Count > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Check <see cref="Properties"/>' integrity.
        /// （確認 <see cref="Properties"/> 的完整性。）
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void PropertiesChecker()
        {
            if (Properties == null)
            {
                throw new NullReferenceException("The Properties proprty is null. Properties 屬性為空值。");
            }
        }

        /// <summary>
        /// Check <see cref="Properties"/>' and input parameters' integrity.
        /// （確認 <see cref="Properties"/> 和輸入參數的完整性。）
        /// </summary>
        /// <param name="properties">The input proprty parameters.（輸入的屬性參數。）</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private void PropertiesChecker(Property[] properties)
        {
            if (Properties == null)
            {
                throw new NullReferenceException("The Properties proprty is null. Properties 屬性為空值。");
            }
            if (properties == null)
            {
                throw new ArgumentNullException("The parameter properties is null. 參數 properties 為空值。");
            }
        }
    }
}