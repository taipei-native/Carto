using Carto.IO;
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
        /// The extent of the area in square meters.
        /// （區域以平方公尺計算的面積。）
        /// </summary>
        public float area;

        /// <summary>
        /// The number of companies in the area.
        /// （區域內的公司數量。）
        /// </summary>
        public int company;

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
        /// The number of labor force reside in the area.
        /// （區域內居住的勞工人數。）
        /// </summary>
        public int labor;

        /// <summary>
        /// The feature classification.
        /// （圖徵分類。）
        /// </summary>
        public Feature objectType;

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
        /// The purchase status of the map tile.
        /// （地圖區塊的購買狀態。）
        /// </summary>
        public bool unlocked;

        /// <summary>
        /// The total amount of wage earned by the residents in the area.
        /// （區域內居民所賺取的薪資總額。）
        /// </summary>
        public int wage;

        public override readonly string ToString()
        {
            return $"Area ({entity.Index}:{entity.Version}) - Age [{age}], Company [{company}], Employee [{employee}], Household [{household}], Labor [{labor}], ObjectType [{objectType}], Profit [{profit}], ResidentFemale [{residentFemale}], ResidentMale [{residentMale}], Unlocked [{unlocked}], Wage [{wage}]";
        }
    }
}