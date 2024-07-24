namespace Carto.Geodata
{
    using System;
    using System.Collections.Generic;
    using Unity.Mathematics;

    public class CartoObject
    {
        /// <summary>
        /// The constructor of Carto.Geodata.CartoObject.
        /// （Carto.Geodata.CartoObject 的建構函式。）
        /// </summary>
        public CartoObject() { }

        /// <summary>
        /// The constructor of Carto.Geodata.CartoObject.
        /// （Carto.Geodata.CartoObject 的建構函式。）
        /// </summary>
        public CartoObject(Dictionary<string, List<float3>> edges, Dictionary<string, object> properties, Dictionary<string, string> type)
        {
            Edges = edges;
            Properties = properties;
            Type = type;
            Values = null;
        }

        /// <summary>
        /// The constructor of Carto.Geodata.CartoObject.
        /// （Carto.Geodata.CartoObject 的建構函式。）
        /// </summary>
        public CartoObject(Dictionary<string, float[,]> values, Dictionary<string, object> properties, Dictionary<string, string> type)
        {
            Edges = null;
            Properties = properties;
            Type = type;
            Values = values;
        }

        public static Dictionary<string, Type> PropertyTypes = new Dictionary<string, Type>
        {
            { "Area", typeof(float) },
            { "Asset", typeof(string) },
            { "Category", typeof(string) },
            { "Center", typeof(float3) },
            { "Direction", typeof(string) },
            { "Form", typeof(string) },
            { "Height", typeof(float) },
            { "Length", typeof(float) },
            { "Name", typeof(string) },
            { "Object", typeof(string) },
            { "Unlocked", typeof(bool) },
            { "Width", typeof(float) }
        };
        
        public const string L = "LineString";
        public const string P = "Polygon";
        public const string R = "Raster";
        public const string T = "Point";

        /// <summary>
        /// The vector spatial data field of the CartoObject.
        /// （CartoObject 的向量空間資料欄位。）
        /// </summary>
        public Dictionary<string, List<float3>> Edges { get; set; }

        /// <summary>
        /// The non-spatial data fields of the CartoObject.
        /// （CartoObject 的非空間資料欄位。）
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// The spatial data type of the CartoObject.
        /// （CartoObject 的空間資料類型。）
        /// </summary>
        public Dictionary<string, string> Type { get; set; }

        /// <summary>
        /// The raster spatial data fields of the CartoObject.
        /// （CartoObject 的網格空間資料欄位。）
        /// </summary>
        public Dictionary<string, float[,]> Values { get; set; }
    }
}