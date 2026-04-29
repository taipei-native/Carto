using Carto.Utils;
using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The container of address information.
    /// （地址資訊的容器。）
    /// </summary>
    public struct Address
    {
        /// <summary>
        /// The district entity.
        /// （行政區實體。）
        /// </summary>
        public Entity district;

        /// <summary>
        /// The house number.
        /// （門牌號碼。）
        /// </summary>
        public int number; 

        /// <summary>
        /// The street aggregation entity, in other words, the network entity with <see cref="Game.Net.AggregateElement"/> buffer.<br/>
        /// （街道的聚合實體。換句話說，擁有 <see cref="Game.Net.AggregateElement"/> 組件的網路實體。）
        /// </summary>
        public Entity street;

        public Address(Entity street, int houseNumber)
        {
            district = Entity.Null;
            this.street = street;
            number = houseNumber;
        }

        public Address(Entity district, Entity street, int houseNumber)
        {
            this.district = district;
            this.street = street;
            number = houseNumber;
        }

        /// <summary>
        /// An empty address.
        /// （空地址。）
        /// </summary>
        public static Address Null
        {
            get
            {
                return new Address()
                {
                    district = Entity.Null,
                    number = 0,
                    street = Entity.Null
                };
            }
        }

        /// <summary>
        /// Convert the entity address to literal one.
        /// （將實體地址轉換為文字地址。）
        /// </summary>
        /// <param name="nameManager">The wrapper managing names.（管理名稱的包裝器。）</param>
        /// <returns>The literal address of the struct.（結構的文字地址。）</returns>
        public readonly LiteralAddress ToLiteral(NameManager nameManager)
        {
            LiteralAddress literal = default;

            if ((nameManager != null) && nameManager.IsValid)
            {
                if (district != Entity.Null)
                {
                    literal.district = nameManager.GetLabelName(district);
                }
                else
                {
                    literal.district = LocaleUtils.TryTranslate("Carto.Address.NULL[District]", out string unincorporated) ? unincorporated : string.Empty;
                }

                literal.street = (street != Entity.Null) ? nameManager.GetLabelName(street) : string.Empty;
                literal.number = number;
            }

            return literal;
        }

        public override readonly string ToString()
        {
            return $"Address - Number [{number}], Street [{street.Index}:{street.Version}], District [{district.Index}:{district.Version}]";
        }
    }
}