using System.Collections.Generic;

namespace Carto.Geodata
{
    /// <summary>
    /// The class to store and export geodata.
    /// （儲存與輸出地理資料的類別。）
    /// </summary>
    public abstract class Geodata
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract CRS Projection { get; set; }
        
        /// <summary>
        /// Write the OGC GeoPackage file (*.gpkg).
        /// （寫出 OGC GeoPackage 檔案（*.gpkg）。）<br/>
        /// See also: <seealso href="https://www.geopackage.org/spec140/">OGC® GeoPackage Encoding Standard</seealso>
        /// </summary>
        /// <param name="fileName">The name of the exported file.（檔案名稱。）</param>
        public virtual void ToGeoPackage(string fileName) { }
    }
}