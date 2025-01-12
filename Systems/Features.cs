using Carto.IO;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Carto.Systems
{
    /// <summary>
    /// The container of area feature's geometry and properties.
    /// （區域圖徵的幾何圖形與屬性容器。）
    /// </summary>
    public struct AreaFeature
    {
        /// <summary>
        /// The extent of the feature in square meters (m²).<br/>
        /// （面積，物件的占地面積，單位為平方公尺（m²）。）
        /// </summary>
        public float area;

        /// <summary>
        /// The index of the boundary for this feature.
        /// （此圖徵邊界的索引位置。）
        /// </summary>
        public int boundaryIndex;

        /// <summary>
        /// The number of boundary points.
        /// （邊界的點數。）
        /// </summary>
        public int boundaryLength;

        /// <summary>
        /// Number of companies in the area.
        /// （公司，區域內的公司數量。）
        /// </summary>
        public int company;

        /// <summary>
        /// The number of employees in the area.
        /// （員工，區域內的受雇員工數量。）
        /// </summary>
        public int employee;

        /// <summary>
        /// The feature's corresponding entity.
        /// （圖徵的對應實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The number of households in the area.
        /// （家庭，區域內的家庭數量。）
        /// </summary>
        public int household;

        /// <summary>
        /// Carto's classification of in-game objects.
        /// （物體，Carto 對遊戲內物體的分類。）
        /// </summary>
        public Feature objectType;

        /// <summary>
        /// The number of residents in the area.
        /// （居民，區域內的居民數量。）
        /// </summary>
        public int resident;

        /// <summary>
        /// The purchase status of the map tiles.
        /// （解鎖，地圖區塊的購買狀態。）
        /// </summary>
        public bool unlocked;

        public AreaFeature(float area, int boundaryIndex, int boundaryLength, int company, int employee, Entity entity, int household, Feature objectType, int resident, bool unlocked)
        {
            this.area = area;
            this.boundaryIndex = boundaryIndex;
            this.boundaryLength = boundaryLength;
            this.company = company;
            this.employee = employee;
            this.entity = entity;
            this.household = household;
            this.objectType = objectType;
            this.resident = resident;
            this.unlocked = unlocked;
        }
    }
}