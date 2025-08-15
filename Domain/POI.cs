using Carto.IO;
using Unity.Entities;
using Unity.Mathematics;

namespace Carto.Domain
{
    /// <summary>
    /// The container of POI's information.
    /// （興趣點資訊的容器。）
    /// </summary>
    public struct POI : IStat
    {
        /// <summary>
        /// See <see cref="entity"/>.（詳見 <see cref="entity"/>。）
        /// </summary>
        public Entity Entity { readonly get => entity; set => entity = value; }

        /// <summary>
        /// The entity that the POI refers to.
        /// （興趣點指涉的實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The address of the POI.
        /// （興趣點的地址。）
        /// </summary>
        public Address address;
        
        /// <summary>
        /// The index of the brand that operates the building.
        /// （營運建築的品牌索引值。）
        /// </summary>
        public int brand;

        /// <summary>
        /// The in-game position of the POI.
        /// （興趣點的遊戲內位置。）
        /// </summary>
        public float3 inGamePosition;

        /// <summary>
        /// Whether this POI has a custom name or not.
        /// （這個興趣點是否有客製化的名稱？）
        /// </summary>
        public bool hasCustomName;

        /// <summary>
        /// Whether the address of this `POI` is mapped to a district or not.
        /// （這個 `POI` 是否已映射至一個行政區？）
        /// </summary>
        public bool isAddressVerified;

        /// <summary>
        /// Whether this `POI` represents a private POI or not.
        /// （這個 `POI` 是否表示一個私人興趣點？）
        /// </summary>
        public bool isPrivate;

        /// <summary>
        /// The location of the POI.
        /// （興趣點的位置。）
        /// </summary>
        public double3 location;

        /// <summary>
        /// The feature classification.
        /// （圖徵分類。）
        /// </summary>
        public Feature objectType;

        public override readonly string ToString()
        {
            return $"POI({entity.Index}:{entity.Version}) - Brand [{brand}], Has Custom Name [{hasCustomName}], Is Address Verified [{isAddressVerified}], Is Privte [{isPrivate}], Object Type [{objectType}]";
        }
    }
}