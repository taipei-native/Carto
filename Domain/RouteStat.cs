using System;
using Unity.Entities;
using UnityEngine;

namespace Carto.Domain
{
    /// <summary>
    /// The container of route statistics.
    /// （運輸服務路線資訊的容器。）
    /// </summary>
    public struct RouteStat : IStat
    {
        // <summary>
        /// See <see cref="entity"/>.（詳見 <see cref="entity"/>。）
        /// </summary>
        public Entity Entity { readonly get => entity; set => entity = value; }

        /// <summary>
        /// The route entity.
        /// （路線實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The capacity of the route.
        /// （路線的容量。）
        /// </summary>
        public int capacity;

        /// <summary>
        /// The color of the route.
        /// （路線的顏色。）
        /// </summary>
        public Color color;

        /// <summary>
        /// Whether this route has a custom name or not.
        /// （這個運輸服務路線是否有客製化的名稱？）
        /// </summary>
        public bool hasCustomName;

        /// <summary>
        /// Whether this route transport cargo or not.
        /// （這個運輸服務路線是否輸送貨物？）
        /// </summary>
        public bool isCargo;

        /// <summary>
        /// Whether this route transport passengers or not.
        /// （這個運輸服務路線是否輸送旅客？）
        /// </summary>
        public bool isPassenger;

        /// <summary>
        /// The length of the route.
        /// （運輸服務路線的長度。）
        /// </summary>
        public float length;

        /// <summary>
        /// The model used on the route.
        /// （在運輸服務路線上服務的車型。）
        /// </summary>
        public Entity model;

        /// <summary>
        /// The serial number of the route.
        /// （運輸服務路線的流水號。）
        /// </summary>
        public int number;

        /// <summary>
        /// The number of passengers on the route.
        /// （運輸服務路線的旅客數量。）
        /// </summary>
        public int passenger;

        /// <summary>
        /// The prefab reference of the route.
        /// （運輸服務路線的預製模板。）
        /// </summary>
        public Entity prefab;

        /// <summary>
        /// The number of stops along the route.
        /// （運輸服務路線的站點數量。）
        /// </summary>
        public int stop;

        /// <summary>
        /// The type of the transport.
        /// （運輸路線的種類。）
        /// </summary>
        public Game.Prefabs.TransportType transport;

        /// <summary>
        /// The number of vehicles serving on the route.
        /// （在運輸服務路線上服務的車輛數量。）
        /// </summary>
        public int vehicle;

        /// <summary>
        /// The weight of the cargo transported on the route.
        /// （透過運輸服務路線輸送的貨物重量。）
        /// </summary>
        public int weight;

        /// <summary>
        /// The feature type of the route.
        /// （運輸服務路線的圖徵類別。）
        /// </summary>
        public readonly IO.Feature Object
        {
            get
            {
                IO.Feature feature = IO.Feature.None;
                if (isCargo) feature |= IO.Feature.RouteCargo;
                if (isPassenger) feature |= IO.Feature.RoutePassenger;
                return feature;
            }
        }

        /// <summary>
        /// The usage rate of the route.
        /// （運輸服務路線的使用率。）
        /// </summary>
        public readonly float Usage
        {
            get
            {
                if (capacity == 0) return 0f;
                if (isCargo) return (float)Math.Round((float)weight / capacity, 4);
                return (float)Math.Round((float)passenger / capacity, 4);
            }
        }

        public override readonly string ToString()
        {
            return $"Route({entity.Index}:{entity.Version}) - Capacity [{capacity}], Color [{color}], Has Custom Name [{hasCustomName}], Is Cargo [{isCargo}], Is Passenger [{isPassenger}], Length [{length}], Model [{model.Index}:{model.Version}], Number [{number}], Passenger [{passenger}], Prefab [{prefab.Index}:{prefab.Version}], Stop [{stop}], Transport [{transport}], Vehicle [{vehicle}], Weight [{weight}]";
        }
    }
}