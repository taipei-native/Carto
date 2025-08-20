using Game.Prefabs;
using Game.UI;
using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The wrapper of the vanilla <see cref="NameSystem"/> and <see cref="PrefabSystem"/>.
    /// （遊戲原生的 <see cref="NameSystem"/> 及 <see cref="PrefabSystem"/> 的包裝器。） 
    /// </summary>
    public class NameManager
    {
        /// <summary>
        /// The vanilla name system instance.
        /// （遊戲原生的名稱系統實例。）
        /// </summary>
        private NameSystem _name;

        /// <summary>
        /// The vanilla prefab system instance.
        /// （遊戲原生的預製模板實例。）
        /// </summary>
        private PrefabSystem _prefab;

        public NameManager()
        {
            TryGet();
        }

        /// <summary>
        /// Check the validity of the system.
        /// （確認系統的有效性。）
        /// </summary>
        public bool IsValid => (_name != null) && (_prefab != null);

        /// <summary>
        /// Retrieve the debug name of the <paramref name="entity" />.
        /// （獲得 <paramref name="entity" /> 的偵錯名稱。）
        /// </summary>
        /// <param name="entity">The input entity.（輸入的實體。）</param>
        /// <returns>The debug name.（偵錯名稱。）</returns>
        public string GetDebugName(Entity entity)
        {
            if (IsValid && (entity != null) && (entity != Entity.Null))
            {
                return _name.GetDebugName(entity);
            }

            if (IsValid && (entity == null) && (entity != Entity.Null))
            {
                Instance.Log.Debug($"The input entity {entity.Index}:{entity.Version} is null.");
            }

            return string.Empty;
        }

        /// <summary>
        /// Retrieve the rendered label name of the <paramref name="entity" />.
        /// （獲得 <paramref name="entity" /> 渲染的標籤名稱。）
        /// </summary>
        /// <param name="entity">The input entity.（輸入的實體。）</param>
        /// <returns>The label name.（標籤名稱。）</returns>
        public string GetLabelName(Entity entity)
        {
            if (IsValid && (entity != null) && (entity != Entity.Null))
            {
                return _name.GetRenderedLabelName(entity);
            }

            if (IsValid && (entity == null) && (entity != Entity.Null))
            {
                Instance.Log.Debug($"The input entity {entity.Index}:{entity.Version} is null.");
            }

            return string.Empty;
        }

        /// <summary>
        /// Retrieve the prefab name of the <paramref name="entity" />.
        /// （獲得 <paramref name="entity" /> 的預製模板名稱。）
        /// </summary>
        /// <param name="entity">The input entity.（輸入的實體。）</param>
        /// <returns>The prefab name.（預製模板名稱。）</returns>
        public string GetPrefabName(Entity entity)
        {
            if (IsValid && (entity != null) && (entity != Entity.Null))
            {
                return _prefab.GetPrefabName(entity);
            }

            if (IsValid && (entity == null) && (entity != Entity.Null))
            {
                Instance.Log.Debug($"The input entity {entity.Index}:{entity.Version} is null.");
            }

            return string.Empty;
        }

        /// <summary>
        /// Try to retrieve the current <see cref="NameSystem"/> and <see cref="PrefabSystem"/> instances.
        /// （嘗試獲得目前的 <see cref="NameSystem"/> 及 <see cref="PrefabSystem"/> 實例。） 
        /// </summary>
        /// <returns>Whether the attempt is successful.（嘗試是否成功？）</returns>
        public bool TryGet()
        {
            _name = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NameSystem>();
            _prefab = Instance.Prefab;
            return IsValid;
        }
    }
}