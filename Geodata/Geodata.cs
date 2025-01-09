using Carto.IO;
using System;

namespace Carto.Geodata
{
    /// <summary>
    /// The class to store and export geodata.
    /// （儲存與輸出地理資料的類別。）
    /// </summary>
    public abstract class Geodata
    {
        /// <summary>
        /// Write the OGC GeoPackage file (*.gpkg).
        /// （寫出 OGC GeoPackage 檔案（*.gpkg）。）<br/>
        /// See also: <seealso href="https://www.geopackage.org/spec140/">OGC® GeoPackage Encoding Standard</seealso>
        /// </summary>
        /// <param name="fileName">The name of the exported file.（檔案名稱。）</param>
        /// <param name="options">The export options.（輸出選項。）</param>
        public virtual void ToGeoPackage(string fileName, Options options = null) { throw new NotImplementedException(); }

        /// <summary>
        /// Write the file into the format that can be used in Carto's web map (experimental).
        /// （寫出可用於 Carto 網路地圖的檔案格式（實驗性）。）
        /// </summary>
        /// <param name="fileName">The name of the exported file.（檔案名稱。）</param>
        /// <param name="options">The export options.（輸出選項。）</param>
        public virtual void ToWebMap(string fileName, Options options = null) { throw new NotImplementedException(); }
    }
}