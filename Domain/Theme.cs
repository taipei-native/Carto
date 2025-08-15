using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The container of theme information.
    /// （建築風格資訊的容器。）
    /// </summary>
    public struct Theme
    {
        /// <summary>
        /// The theme entity.
        /// （建築風格實體。）
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The prefab name of the theme.
        /// （建築風格的預製模板名稱。）
        /// </summary>
        public string name;

        public override readonly string ToString()
        {
            return $"Theme ({entity.Index}:{entity.Version}) - Name [{name}]";
        }
    }
}