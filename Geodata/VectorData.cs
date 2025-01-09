using Carto.IO;
using System.Collections.Generic;

namespace Carto.Geodata
{
    /// <summary>
    /// The class to store and export vector geodata.
    /// （儲存與輸出向量地理資料的類別。）
    /// </summary>
    public class VectorData : Geodata
    {
        /// <summary>
        /// Define all features.
        /// （定義所有圖徵。）
        /// </summary>
        public List<Feature> Features { get; set; }

        /// <summary>
        /// The maximum length in characters of each property.
        /// （各屬性欄位以字元計的最大長度。）
        /// </summary>
        public Dictionary<Property, int> Lengths { get; set; }

        public VectorData(List<Feature> features, Dictionary<Property, int> lengths)
        {
            Features = features;
            Lengths = lengths;
        }

        /// <summary>
        /// The VectorKind in the features.
        /// （圖徵內的向量類型。）
        /// </summary>
        public VectorKind Kinds
        {
            get
            {
                if (Features == null | Features.Count == 0)
                {
                    return VectorKind.Unknown;
                }
                
                VectorKind kinds = VectorKind.Unknown;

                for (int i = 0; i < Features.Count; i++)
                {
                    Feature feature = Features[i];
                    if (feature?.Geometries != null)
                    {
                        foreach (VectorKind kind in feature.Geometries.Keys)
                        {
                            kinds |= kind;
                        }
                    }
                }

                return kinds;
            }
        }

        /// <summary>
        /// Write the GeoJSON file. (*.geojson).
        /// （寫出 GeoJSON 檔案（*.geojson）。）<br/>
        /// See also: <seealso href="https://doi.org/10.17487/rfc7946">The GeoJSON format</seealso>
        /// </summary>
        /// <param name="fileName">The name of the exported file.（檔案名稱。）</param>
        /// <param name="options">The export options.（輸出選項。）</param>
        public void ToGeoJSON(string fileName, Options options = null)
        {
            /*
                # References: （資料來源：）

                * Gillies, S., Butler, H. J., Daly, M., Doyle, A., & Schaub, T. (2016). The GeoJSON format
                    https://doi.org/10.17487/rfc7946
            */

            options ??= new Options();
        }
    }
}