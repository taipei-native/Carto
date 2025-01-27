using Game.Prefabs;
using Unity.Entities;
using UnityEngine;

namespace Carto.Domain
{
    public struct ZoningType
    {
        /// <summary>
        /// The zoning type entity.
        /// （分區類型實體。）
        /// </summary>
        public Entity entity;
        
        /// <summary>
        /// The category of the zoning type.
        /// （分區類型的分類。）
        /// </summary>
        public ZoningCategory category;

        /// <summary>
        /// The color of the zoning type.
        /// （分區類型的顏色。）
        /// </summary>
        public Color color;

        /// <summary>
        /// The density of the zoning type.
        /// （分區類型的發展強度。）
        /// </summary>
        public ZoningDensity density;

        /// <summary>
        /// The unique index of each zoning types loaded in-game.
        /// （遊戲中已載入分區類型的唯一識別碼。）
        /// </summary>
        public ushort id;

        /// <summary>
        /// The prefab information of the entity.
        /// （實體的預製模板資訊。）
        /// </summary>
        public PrefabData prefabData;

        /// <summary>
        /// The index of the theme that the zoning type belongs to.
        /// （分區類型所屬的風格索引值。）
        /// </summary>
        public int theme;

        public override readonly string ToString()
        {
            return $"Zoning ({entity.Index}:{entity.Version}) - Category [{category}], Color [{color}], Density [{density}], Id [{id}], PrefabData [{prefabData.m_Index}], Theme [{theme}]";
        }
    }
}