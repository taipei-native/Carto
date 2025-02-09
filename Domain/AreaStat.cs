using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The container of area statistics.
    /// （區域統計資料的容器。）
    /// </summary>
    public struct AreaStat
    {
        /// <summary>
        /// The area entity.
        /// （區域實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The total age of the residents in the area in days.
        /// （區域內居民以天數計算的年齡總和。）
        /// </summary>
        public float age;

        /// <summary>
        /// The number of companies in the area.
        /// （區域內的公司數量。）
        /// </summary>
        public float company;

        /// <summary>
        /// The number of employee in the area.
        /// （區域內的員工數量。）
        /// </summary>
        public int employee;

        /// <summary>
        /// The number of households in the area.
        /// （區域內家庭的數量。）
        /// </summary>
        public int household;

        /// <summary>
        /// The amount of profit earned by the company in the area.
        /// （區域內公司賺取的利潤金額。）
        /// </summary>
        public int profit;

        /// <summary>
        /// The number of female residents in the area.
        /// （區域內的女性居民數量。）
        /// </summary>
        public int residentFemale;

        /// <summary>
        /// The number of male residents in the area.
        /// （區域內的男性居民數量。）
        /// </summary>
        public int residentMale;

        /// <summary>
        /// The total amount of wage earned by the residents in the area.
        /// （區域內居民所賺取的薪資總額。）
        /// </summary>
        public int wage;
    }
}