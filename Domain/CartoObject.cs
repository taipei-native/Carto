namespace Carto.Domain
{
    using GeometryType = Utils.ExportUtils.GeometryType;
    using Unity.Mathematics;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The class storing retrieved in-game information.
    /// （儲存已收集的遊戲內資訊的類別。）
    /// </summary>
    public class CartoObject : IEquatable<CartoObject>
    {
        /// <summary>
        /// The constructor of Carto.Domain.CartoObject.
        /// （Carto.Domain.CartoObject 的建構函式。）
        /// </summary>
        public CartoObject() { }

        /// <summary>
        /// The constructor of Carto.Domain.CartoObject.
        /// （Carto.Domain.CartoObject 的建構函式。）
        /// </summary>
        public CartoObject(Dictionary<string, List<float3>> edges, Dictionary<string, object> properties, Dictionary<string, GeometryType> type)
        {
            Edges = edges;
            Properties = properties;
            Type = type;
            Values = null;
        }

        /// <summary>
        /// The constructor of Carto.Domain.CartoObject.
        /// （Carto.Domain.CartoObject 的建構函式。）
        /// </summary>
        public CartoObject(Dictionary<string, float[,]> values, Dictionary<string, object> properties, Dictionary<string, GeometryType> type)
        {
            Edges = null;
            Properties = properties;
            Type = type;
            Values = values;
        }

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
        public Dictionary<string, GeometryType> Type { get; set; }

        /// <summary>
        /// The raster spatial data fields of the CartoObject.
        /// （CartoObject 的網格空間資料欄位。）
        /// </summary>
        public Dictionary<string, float[,]> Values { get; set; }

        public bool Equals(CartoObject other)
        {
            return (Edges == other.Edges) && (Properties == other.Properties) && (Type == other.Type) && (Values == other.Values);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (!(obj is CartoObject)) return false;
            return Equals((CartoObject) obj);
        }

        public override int GetHashCode()
        {
            return $"{Edges.GetHashCode()}, {Properties.GetHashCode()}, {Type.GetHashCode()}, {Values.GetHashCode()}".GetHashCode();
        }
    }
}