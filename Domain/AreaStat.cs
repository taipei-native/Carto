using Carto.IO;
using System;
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

        /// <summary>
        /// Retrieve the average age of the residents in the area.
        /// （獲得區域內居民的平均年齡。）
        /// </summary>
        /// <returns>The age in days.（以日數計的年齡。）</returns>
        public readonly float GetAverageAge() => (residentFemale + residentMale) > 0 ? (float)Math.Round(age / (residentFemale + residentMale), 1) : 0f;

        /// <summary>
        /// Retrieve the average amount of profit earned by the company in the area.
        /// （獲得區域內公司的平均利潤。）
        /// </summary>
        /// <returns>The profit in ₡ per month.（以 ₡／月 計的利潤。）</returns>
        public readonly float GetAverageProfit() => company > 0 ? (float)Math.Round((double)profit / company, 2) : 0f;

        /// <summary>
        /// Retrieve the average amount of wage earned by the labors in the area.
        /// （獲得區域內勞工的平均薪資。）
        /// </summary>
        /// <returns>The wage in ₡ per month.（以 ₡／月 計的薪資。）</returns>
        public readonly float GetAverageWage() => labor > 0 ? (float)Math.Round((double)wage / labor, 2) : 0f;

        /// <summary>
        /// Retrieve the sex ratio of the area.
        /// （獲得區域的性別比。）
        /// </summary>
        /// <returns>The sex ratio in percentage.（以百分比計的性別比。）</returns>
        public readonly float GetSexRatio() => residentFemale > 0 ? (float)Math.Round((double)residentMale / residentFemale * 100, 4) : 0f;

        public override readonly string ToString()
        {
            return $"Area ({entity.Index}:{entity.Version}) - Age [{age}], Company [{company}], Employee [{employee}], Household [{household}], Labor [{labor}], ObjectType [{objectType}], Profit [{profit}], ResidentFemale [{residentFemale}], ResidentMale [{residentMale}], Unlocked [{unlocked}], Wage [{wage}]";
        }
    }
}