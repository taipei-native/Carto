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
        /// The street aggregation entity, in other words, the network entity with <see cref="Game.Net.Aggregated"/> component.<br/>
        /// （街道的聚合實體。換句話說，擁有 <see cref="Game.Net.Aggregated"/> 組件的網路實體。）
        /// </summary>
        public Entity street;

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

        public override readonly string ToString()
        {
            return $"Address - Number [{number}], Street [{street.Index}:{street.Version}], District [{district.Index}:{district.Version}]";
        }
    }
}