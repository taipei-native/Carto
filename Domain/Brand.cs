using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The container of brand / enterprise information.
    /// （品牌／企業資訊的容器。）
    /// </summary>
    public struct Brand
    {
        /// <summary>
        /// The brand entity.
        /// （品牌實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The prefab name of the brand.
        /// （品牌的預製模板名稱。）
        /// </summary>
        public string name;

        public override readonly string ToString()
        {
            return $"Brand ({entity.Index}:{entity.Version}) - Name [{name}]";
        }
    }
}