using Carto.IO;
using Game.Economy;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The container of building statistics.
    /// （建築統計資料的容器。）
    /// </summary>
    public struct BuildingStat : IStat
    {
        /// <summary>
        /// See <see cref="entity"/>.（詳見 <see cref="entity"/>。）
        /// </summary>
        public Entity Entity { readonly get => entity; set => entity = value; }
        
        /// <summary>
        /// The building entity.
        /// （建築實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The address of the building.
        /// （建築的地址。）
        /// </summary>
        public Address address;

        /// <summary>
        /// The total age of the residents in the building in days.
        /// （建築內居民以天數計算的年齡總和。）
        /// </summary>
        public float age;

        /// <summary>
        /// The index of the brand that operates the building.
        /// （營運建築的品牌索引值。）
        /// </summary>
        public int brand;

        /// <summary>
        /// The category of the building.
        /// （建築的分類。）
        /// </summary>
        public BuildingCategory category;

        /// <summary>
        /// The number of companies in the building.
        /// （建築內的公司數量。）
        /// </summary>
        public int company;

        /// <summary>
        /// The bottom elevation of the building.
        /// （建築底部的高度。）
        /// </summary>
        public float elevation;

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
        /// The number of labor force reside in the building.
        /// （建築內居住的勞工人數。）
        /// </summary>
        public int labor;

        /// <summary>
        /// The upgrade progess of the building.
        /// （建築的升級進度。）
        /// </summary>
        public int level;

        /// <summary>
        /// The main building entity of the affliated area.
        /// （附屬區域的主建築實體。）
        /// </summary>
        public Entity mainBuilding;

        /// <summary>
        /// The feature classification.
        /// （圖徵分類。）
        /// </summary>
        public Feature objectType;

        /// <summary>
        /// The prefab template of the building.
        /// （建築的預製模板。）
        /// </summary>
        public Entity prefab;

        /// <summary>
        /// The merchandise sold by the companies in the building.
        /// （建築內公司銷售的商品。）
        /// </summary>
        public Resource product;

        /// <summary>
        /// The amount of profit earned by the company in the building.
        /// （建築內公司賺取的利潤金額。）
        /// </summary>
        public int profit;

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
        /// The total amount of wage earned by the residents in the building.
        /// （建築內居民所賺取的薪資總額。）
        /// </summary>
        public int wage;

        /// <summary>
        /// The index of the zoning type that the building spawns in.
        /// （生成建築的分區類型索引值。）
        /// </summary>
        public int zoning;

        /// <summary>
        /// Retrieve the average age of the residents in the building.
        /// （獲得建築內居民的平均年齡。）
        /// </summary>
        /// <returns>The age in days.（以日數計的年齡。）</returns>
        public readonly float GetAverageAge() => (residentFemale + residentMale) > 0 ? (float)Math.Round(age / (residentFemale + residentMale), 1) : 0f;

        /// <summary>
        /// Retrieve the average amount of profit earned by the company in the building.
        /// （獲得建築內公司的平均利潤。）
        /// </summary>
        /// <returns>The profit in ₡ per month.（以 ₡／月 計的利潤。）</returns>
        public readonly float GetAverageProfit() => company > 0 ? (float)Math.Round((double)profit / company, 2) : 0f;

        /// <summary>
        /// Retrieve the average amount of wage earned by the labors in the building.
        /// （獲得建築內勞工的平均薪資。）
        /// </summary>
        /// <returns>The wage in ₡ per month.（以 ₡／月 計的薪資。）</returns>
        public readonly float GetAverageWage() => labor > 0 ? (float)Math.Round((double)wage / labor, 2) : 0f;

        /// <summary>
        /// Retrieve the products in localized name.
        /// （獲得翻譯後的產品名稱。）
        /// </summary>
        /// <param name="resourceTranslationMap">The map between the resource enum value and the translation.（資源枚舉值與翻譯的映射表。）</param>
        /// <returns>The string containing localized product names.（含有已翻譯產品名稱的字串。）</returns>
        public readonly string GetLocalizedProducts(Dictionary<Resource, string> resourceTranslationMap)
        {
            StringBuilder productNames = new();
            Resource[] products = Utils.CommonUtils.GetFlagComponents(product);

            for (int j = 0; j < products.Length; j++)
            {
                string productName = resourceTranslationMap.TryGetValue(products[j], out string localizedName) ? localizedName : products[j].ToString("G");
                productNames.Append(productName);

                if (j != products.Length - 1)
                {
                    productNames.Append(", ");
                }
            }
            return productNames.ToString();
        }

        /// <summary>
        /// Retrieve the sex ratio of the building.
        /// （獲得建築的性別比。）
        /// </summary>
        /// <returns>The sex ratio in percentage.（以百分比計的性別比。）</returns>
        public readonly float GetSexRatio() => residentFemale > 0 ? (float)Math.Round((double)residentMale / residentFemale * 100, 4) : 0f;

        public override readonly string ToString()
        {
            return $"Building ({entity.Index}:{entity.Version}) - Address [{address}], Age [{age}], Brand [{brand}], Category [{category}], Company [{company}], Elevation [{elevation}], Employee [{employee}], Household [{household}], Labor [{labor}], Level [{level}], MainBuilding [{mainBuilding.Index}:{mainBuilding.Version}], ObjectType [{objectType}], Product [{product}], Prefab [{prefab.Index}:{prefab.Version}], Profit [{profit}], ResidentFemale [{residentFemale}], ResidentMale [{residentMale}], Wage [{wage}], Zoning [{zoning}]";
        }
    }
}