namespace Carto.Domain
{
    using Carto.Utils;
    using Game.Areas;
    using Game.Buildings;
    using System;
    using System.Globalization;
    using Unity.Entities;

    /// <summary>
    /// The class to store the address of the buildings.
    /// （儲存建築地址的類別。）
    /// </summary>
    public class Address : IComparable, IEquatable<Address>, IFormattable
    {
        /// <summary>
        /// The district name that the address is situated.
        /// （地址所在的行政區名稱。）
        /// </summary>
        public string District;

        /// <summary>
        /// The housenumber of the address.
        /// （地址的門牌號碼。）
        /// </summary>
        public int Number;

        /// <summary>
        /// The street name that the address is grouped by.
        /// （地址所在的街道名稱。）
        /// </summary>
        public string Street;

        public Address(Entity building, EntityManager entityManager)
        {
            if (entityManager.HasComponent<Building>(building) && BuildingUtils.GetAddress(entityManager, building, out Entity road, out int number))
            {
                Number = number;
                Street = Instance.Name.GetRenderedLabelName(road);
            }
            else
            {
                Number = 0;
                Street = string.Empty;
            }

            if (entityManager.HasComponent<CurrentDistrict>(building))
            {
                Entity buildDistrict = entityManager.GetComponentData<CurrentDistrict>(building).m_District;
                District = (buildDistrict == Entity.Null) ? LocaleUtils.Translate("Carto.address.UNINCORPORATED[District]") : Instance.Name.GetRenderedLabelName(buildDistrict);
            }
            else
            {
                District = string.Empty;
            }
        }

        public Address(int number, string street)
        {
            District = string.Empty;
            Number = number;
            Street = street;
        }

        public Address(int number, string street, string district)
        {
            District = district;
            Number = number;
            Street = street;
        }

        /// <summary>
        /// The long address including the city name.
        /// （包含城市名稱的長地址。）
        /// </summary>
        public string LongName => LocaleUtils.Translate("Carto.address.LONG_ADDRESS_FORMAT").Replace("{NUMBER}", Number.ToString()).Replace("{STREET}", Street).Replace("{DISTRICT}", District).Replace("{CITY}", Instance.CityName);

        /// <summary>
        /// The address using Carto format.
        /// （Carto 的地址格式。）
        /// </summary>
        public string Name => LocaleUtils.Translate("Carto.address.MEDIUM_ADDRESS_FORMAT").Replace("{NUMBER}", Number.ToString()).Replace("{STREET}", Street).Replace("{DISTRICT}", District);

        /// <summary>
        /// The short address without the district and the city name.
        /// （不包含行政區與城市名稱的短地址。）
        /// </summary>
        public string ShortName => LocaleUtils.Translate("Carto.address.SHORT_ADDRESS_FORMAT").Replace("{NUMBER}", Number.ToString()).Replace("{STREET}", Street);

        /// <summary>
        /// The address using in-game format.
        /// （遊戲內的地址格式。）
        /// </summary>
        public string VanillaName => LocaleUtils.Translate("Assets.ADDRESS_NAME_FORMAT").Replace("{NUMBER}", Number.ToString()).Replace("{ROAD}", Street);

        public int CompareTo(object obj)
        {
            if (obj is Address addressObj)
            {
                int PropertyComparer(object propertyA, object propertyB)
                {
                    int PropertyEvaluator(object property)
                    {
                        if (property is int intProperty)
                        {
                            if (intProperty == 0)
                            {
                                return 1;
                            }
                        }
                        if (property is string stringProperty)
                        {
                            if (stringProperty == LocaleUtils.Translate("Carto.address.UNINCORPORATED[District]"))
                            {
                                return 1;
                            }
                            if (string.IsNullOrEmpty(stringProperty))
                            {
                                return 2;
                            }
                        }

                        return 0;
                    }

                    int rankA = PropertyEvaluator(propertyA);
                    int rankB = PropertyEvaluator(propertyB);
                    int comparer = rankA.CompareTo(rankB);
                    if (comparer != 0) return comparer;

                    if (propertyA is int intPropertyA)
                    {
                        return intPropertyA.CompareTo((int)propertyB);
                    }
                    else if (propertyA is string stringPropertyA)
                    {
                        return stringPropertyA.CompareTo((string)propertyB);
                    }

                    return comparer;
                }

                int districtComparer = PropertyComparer(District, addressObj.District);
                if (districtComparer != 0) return districtComparer;

                int streetComparer = PropertyComparer(Street, addressObj.Street);
                if (streetComparer != 0) return streetComparer;

                int numberComparer = PropertyComparer(Number, addressObj.Number);
                if (numberComparer != 0) return numberComparer;

                return Number.CompareTo(addressObj.Number);
            }
            else
            {
                throw new ArgumentException($"Expect `Address` but recieve `{obj.GetType().Name}`");
            }
        }

        public bool Equals(Address other)
        {
            return (District == other.District) && (Number == other.Number) && (Street == other.Street);
        }

        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (!(other is Address)) return false;
            return Equals((Address)other);
        }

        public override int GetHashCode()
        {
            return string.Join(".", new object[] { Number, Street, District }).GetHashCode();
        }

        private string Mask(string input)
        {
            if (input == null) return "`null`";
            if (input == string.Empty) return "`string.Empty`";
            return input;
        }

        public override string ToString()
        {
            return ToString("N", CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "N";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "D":
                    return $"Address(Number = {Number}, Street = {Mask(Street)}, District = {Mask(District)})";

                case "L":
                    return $"Address({LongName})".ToString(provider);

                case "N":
                default:
                    return $"Address({Name})".ToString(provider);

                case "S":
                    return $"Address({ShortName})".ToString(provider);

                case "V":
                    return $"Address({VanillaName})".ToString(provider);
            }
        }
    }
}