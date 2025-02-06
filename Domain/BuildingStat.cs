using Game.Economy;
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
        /// The total age of the residents in the building in days.
        /// （建築內居民以天數計算的年齡總和。）
        /// </summary>
        public float age;

        /// <summary>
        /// The brand entity that operates the building.
        /// （營運建築的品牌實體。）
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
        /// The upgrade progess of the building.
        /// （建築的升級進度。）
        /// </summary>
        public int level;

        /// <summary>
        /// The merchandise sold by the companies in the building.
        /// （建築內公司銷售的商品。）
        /// </summary>
        public Resource product;

        /// <summary>
        /// The number of female residents in the building.
        /// （建築內的女性居民數量。）
        /// </summary>
        public int residentFemale;

        /// <summary>
        /// The number of male residents in the building.
        /// （建築內的男性居民數量。）
        /// </summary>
        public int residentMale;

        /// <summary>
        /// The index of the zoning type that the building spawns in.
        /// （生成建築的分區類型索引值。）
        /// </summary>
        public int zoning;

        public override readonly string ToString()
        {
            return $"Building ({entity.Index}:{entity.Version}) - Age [{age}], Brand [{brand}], Company [{company}], Employee [{employee}], Household [{household}], Product [{product}], ResidentFemale [{residentFemale}], ResidentMale [{residentMale}], Zoning [{zoning}]";
        }
    }
}