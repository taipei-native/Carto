using System.Collections.Generic;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of a feature.
    /// （圖徵的定義。）
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// Define the geometries of each VectorKind.
        /// （定義各種向量類型的幾何圖形。）
        /// </summary>
        public Dictionary<VectorKind, Geometry> Geometries { get; set; }

        /// <summary>
        /// Define the content of each Property.
        /// （定義各種屬性的內容。）
        /// </summary>
        public Dictionary<Property, object> Properties { get; set; }

        public Feature(Dictionary<VectorKind, Geometry> geometries, Dictionary<Property, object> properties)
        {
            Geometries = geometries;
            Properties = properties;
        }
    }
}