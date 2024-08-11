namespace Carto.Domain
{
    using Game.Buildings;
    using Game.Citizens;
    using Game.Prefabs;
    using Game.UI.InGame;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.Entities;
    using Unity.Mathematics;

    /// <summary>
    /// The class recording complicated statistic data.
    /// （紀錄複雜統計資料的類別。）
    /// </summary>
    // TODO: Implement Carto.Statistics extension module（待辦：實作 Carto.Statistics 延伸模組。）
    public class StatsObject
    {
        private CitizenMatrix citizens = new CitizenMatrix();

        /// <summary>
        /// The citizen data.
        /// （市民資料。）
        /// </summary>
        public CitizenMatrix Citizens
        {
            get { return citizens; }
            set { citizens = value; }
        }
    }

    /// <summary>
    /// The class recording simple statistic data.
    /// （紀錄簡單統計資料的類別。）
    /// </summary>
    public class SimpleStatsObject
    {
        /// <summary>
        /// The list of brands within the statistical area.
        /// （統計區域內的品牌列表。）
        /// </summary>
        public List<string> Brands { get; set; } = new List<string>();

        /// <summary>
        /// The number of the companies within the statistical area.
        /// （統計區域內的公司數量。）
        /// </summary>
        public int Company { get; set; } = 0;

        /// <summary>
        /// The number of the employees within the statistical area.
        /// （統計區域內的員工數量。）
        /// </summary>
        public int Employee { get; set; } = 0;

        /// <summary>
        /// The number of the households within the statistical area.
        /// （統計區域內的家庭數量。）
        /// </summary>
        public int Household { get; set; } = 0;
        
        /// <summary>
        /// The list of products manufactured within the statistical area.
        /// （統計區域內生產的產品列表。）
        /// </summary>
        public List<string> Products { get; set; } = new List<string>();

        /// <summary>
        /// The number of the residents within the statistical area.
        /// （統計區域內居住的市民數量。）
        /// </summary>
        public int Resident { get; set; } = 0;

        public static SimpleStatsObject operator + (SimpleStatsObject self, int other)
        {
            return new SimpleStatsObject
            {
                Brands = self.Brands,
                Company = self.Company + other,
                Employee = self.Employee + other,
                Household = self.Household + other,
                Products = self.Products,
                Resident = self.Resident + other
            };
        }

        public static SimpleStatsObject operator + (SimpleStatsObject self, SimpleStatsObject other)
        {
            List<string> brands = new List<string>();
            if (self.Brands.Count > 0) brands.AddRange(self.Brands);
            if (other.Brands.Count > 0) brands.AddRange(other.Brands);

            List<string> products = new List<string>();
            if (self.Products.Count > 0) products.AddRange(self.Products);
            if (other.Products.Count > 0) products.AddRange(other.Products);
            
            return new SimpleStatsObject
            {
                Brands = brands,
                Company = self.Company + other.Company,
                Employee = self.Employee + other.Employee,
                Household = self.Household + other.Household,
                Products = products,
                Resident = self.Resident + other.Resident
            };
        }
    }

    /// <summary>
    /// The structure storing the citizen data.
    /// （儲存市民資料的結構。）
    /// </summary>
    public struct CitizenMatrix
    {
        /// <summary>
        /// The five-dimensional array representing the citizen data.
        /// （表示市民資料的五維陣列。）
        /// </summary>
        public int[,,,,] Matrix;

        /// <summary>
        /// The total number of citizens.
        /// （市民的總數。）
        /// </summary>
        public int Total => Matrix.Cast<int>().Sum();

        public CitizenMatrix(int[,,,,] matrix)
        {
            int r0 = matrix.GetLength(0);
            int r1 = matrix.GetLength(1);
            int r2 = matrix.GetLength(2);
            int r3 = matrix.GetLength(3);
            int r4 = matrix.GetLength(4);
            if ((r0 != 2) || (r1 != 4) || (r2 != 5) || (r3 != 5) || (r4 != 9)) throw new ArgumentException($"Expect `int[2, 4, 5, 5, 9]`, but received `int[{r0}, {r1}, {r2}, {r3}, {r4}]`");

            Matrix = matrix;
        }

        public CitizenMatrix(Entity buildingEntity, EntityManager entityManager)
        {
            if (!entityManager.HasComponent<Building>(buildingEntity)) throw new ArgumentException($"Only accepts entities with the component `Game.Buildings.Building`.");
            if (entityManager.HasBuffer<Renter>(buildingEntity))
            {
                int[,,,,] matrix = new int[2, 4, 5, 5, 9];
                CitizenHappinessParameterData chpd = new CitizenHappinessParameterData { m_WealthyMoneyAmount = new int4(500, 2500, 4500, 8000) };

                foreach (Renter renter in entityManager.GetBuffer<Renter>(buildingEntity))
                {
                    Entity renterEntity = renter.m_Renter;

                    if (entityManager.HasComponent<Household>(renterEntity))
                    {
                        int wealth = (int) CitizenUIUtils.GetHouseholdWealth(entityManager, renterEntity, chpd);

                        foreach (HouseholdCitizen householdCitizen in entityManager.GetBuffer<HouseholdCitizen>(renterEntity))
                        {
                            Entity householdCitizenEntity = householdCitizen.m_Citizen;

                            if (!CitizenUtils.IsCorpsePickedByHearse(entityManager, householdCitizenEntity))
                            {
                                Citizen citizenProperties = entityManager.GetComponentData<Citizen>(householdCitizenEntity);

                                int sex = citizenProperties.m_State.HasFlag(CitizenFlags.Male) ? 1 : 0;
                                int age = (int)citizenProperties.GetAge();
                                int education = citizenProperties.GetEducationLevel();
                                int job = (int)CitizenUIUtils.GetOccupation(entityManager, householdCitizenEntity);

                                matrix[sex, age, education, wealth, job]++;
                            }
                        } 
                    }
                }

                Matrix = matrix;
            }
            else
            {
                Matrix = new int[2, 4, 5, 5, 9];
            }
        }

        public override string ToString()
        {
            int[,] age = new int[4, 2];
            int[,] education = new int[5, 2];
            int[,] occupation = new int[9, 2];
            int[,] wealth = new int[5, 2];
            
            for (int i = 0; i < Matrix.GetLength(0); i ++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j ++)
                {
                    for (int k = 0; k < Matrix.GetLength(2); k ++)
                    {
                        for (int l = 0; l < Matrix.GetLength(3); l ++)
                        {
                            for (int m = 0; m < Matrix.GetLength(4); m ++)
                            {
                                int num = Matrix[i, j, k, l, m];
                                age[j, i] += num;
                                education[k, i] += num;
                                occupation[m, i] += num;
                                wealth[l, i] += num;
                            }
                        }
                    }
                }
            }

            string Builder(string source, string[] array, int[,] data)
            {
                string result = source;
                result += new string(' ', 11);
                result += string.Join(new string(' ', 5), array);
                result += "\n";

                for (int j = 0; j < data.GetLength(1); j++)
                {
                    result += (j > 0) ? "Male  " : "Female";

                    for (int i = 0; i < data.GetLength(0); i++)
                    {
                        result += $" {data[i, j],7}";
                    }

                    result += "\n";
                }

                return result;
            }

            string printer = "\n";
            printer = Builder(printer, new string[] { "KID", "TEE", "ADT", "ELD" }, age);
            printer = Builder(printer, new string[] { "UED", "POR", "EDU", "WEL", "HGH" }, education);
            printer = Builder(printer, new string[] { "UEP", "WOK", "STU", "RET", "CRI", "ROB", "TOR", "NON", "UKN" }, occupation);
            return    Builder(printer, new string[] { "WRE", "POR", "MOD", "COM", "WLT" }, wealth);
        }
    
        public static CitizenMatrix operator + (CitizenMatrix self, CitizenMatrix other)
        {
            int[,,,,] newMatrix = self.Matrix;
            
            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < newMatrix.GetLength(2); k++)
                    {
                        for (int l = 0; l < newMatrix.GetLength(3); l++)
                        {
                            for (int m = 0; m < newMatrix.GetLength(4); m++)
                            {
                                newMatrix[i, j, k, l, m] += other.Matrix[i, j, k, l, m];
                            }
                        }
                    }
                }
            }

            return new CitizenMatrix { Matrix = newMatrix };
        }

        public static CitizenMatrix operator + (CitizenMatrix self, int other)
        {
            int[,,,,] newMatrix = self.Matrix;

            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < newMatrix.GetLength(2); k++)
                    {
                        for (int l = 0; l < newMatrix.GetLength(3); l++)
                        {
                            for (int m = 0; m < newMatrix.GetLength(4); m++)
                            {
                                newMatrix[i, j, k, l, m] += other;
                            }
                        }
                    }
                }
            }

            return new CitizenMatrix { Matrix = newMatrix };
        }

        public static CitizenMatrix operator - (CitizenMatrix self, CitizenMatrix other)
        {
            int[,,,,] newMatrix = self.Matrix;

            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < newMatrix.GetLength(2); k++)
                    {
                        for (int l = 0; l < newMatrix.GetLength(3); l++)
                        {
                            for (int m = 0; m < newMatrix.GetLength(4); m++)
                            {
                                newMatrix[i, j, k, l, m] -= other.Matrix[i, j, k, l, m];
                            }
                        }
                    }
                }
            }

            return new CitizenMatrix { Matrix = newMatrix };
        }

        public static CitizenMatrix operator - (CitizenMatrix self, int other)
        {
            int[,,,,] newMatrix = self.Matrix;

            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < newMatrix.GetLength(2); k++)
                    {
                        for (int l = 0; l < newMatrix.GetLength(3); l++)
                        {
                            for (int m = 0; m < newMatrix.GetLength(4); m++)
                            {
                                newMatrix[i, j, k, l, m] -= other;
                            }
                        }
                    }
                }
            }

            return new CitizenMatrix { Matrix = newMatrix };
        }

        public static CitizenMatrix operator * (CitizenMatrix self, CitizenMatrix other)
        {
            int[,,,,] newMatrix = self.Matrix;

            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < newMatrix.GetLength(2); k++)
                    {
                        for (int l = 0; l < newMatrix.GetLength(3); l++)
                        {
                            for (int m = 0; m < newMatrix.GetLength(4); m++)
                            {
                                newMatrix[i, j, k, l, m] *= other.Matrix[i, j, k, l, m];
                            }
                        }
                    }
                }
            }

            return new CitizenMatrix { Matrix = newMatrix };
        }

        public static CitizenMatrix operator * (CitizenMatrix self, int other)
        {
            int[,,,,] newMatrix = self.Matrix;

            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < newMatrix.GetLength(2); k++)
                    {
                        for (int l = 0; l < newMatrix.GetLength(3); l++)
                        {
                            for (int m = 0; m < newMatrix.GetLength(4); m++)
                            {
                                newMatrix[i, j, k, l, m] *= other;
                            }
                        }
                    }
                }
            }

            return new CitizenMatrix { Matrix = newMatrix };
        }
    }
}