using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The container of building statistics.
    /// （建築統計資料的容器。）
    /// </summary>
    public struct BuildingStat
    {
        /// <summary>
        /// The building entity.
        /// （建築實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The index of the brand that operates the building.
        /// （營運建築的品牌索引值。）
        /// </summary>
        public int brand;

        /// <summary>
        /// The number of companies in the building.
        /// （建築內的公司數量。）
        /// </summary>
        public int company;

        /// <summary>
        /// The number of employee in the building.
        /// （建築內的員工數量。）
        /// </summary>
        public int employee;

        /// <summary>
        /// The number of households in the building.
        /// （建築內家庭的數量。）
        /// </summary>
        public int household;

        /// <summary>
        /// The merchandise sold by the companies in the building.
        /// （建築內公司銷售的商品。）
        /// </summary>
        public ulong product;

        /// <summary>
        /// The number of residents in the building.
        /// （建築內的居民數量。）
        /// </summary>
        public int resident;
    }
}