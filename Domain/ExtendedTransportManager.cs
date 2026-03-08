using Carto.Utils;
using Game.Modding;
using System;
using System.Reflection;
using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The wrapper of the assembly of <see href="https://github.com/klyte45/CS2-ExtendedTransportManager">Extended Transport Manager</see> mod developed by klyte45.<br/>
    /// （由 klyte45 開發的 <see href="https://github.com/klyte45/CS2-ExtendedTransportManager">Extended Transport Manager</see> 模組組件的包裝器。）
    /// </summary>
    public class ExtendedTransportManager : IAssembly
    {
        private const string _assemblyName = "ExtendedTransportManager";
        private const string _acronymPropertyName = "Acronym";
        private const string _xtmRouteExtraDataTypeName = "BelzontTLM.XTMRouteExtraData";

        /// <summary>
        /// The method to obtain acronym from the component.
        /// （由組件獲取縮寫的方法。）
        /// </summary>
        private Func<object, string> _acronymGetter;

        /// <summary>
        /// The external assembly object.
        /// （外部組件物件。）
        /// </summary>
        private Assembly _assembly;

        /// <summary>
        /// Whether accessing the assembly is posible or not.
        /// （是否可以存取組件？）
        /// </summary>
        private bool _accessible;

        /// <summary>
        /// See <see cref="XtmRouteExtraData"/>.
        /// </summary>
        private Type _xtmRouteExtraDataType;

        /// <summary>
        /// The method to obtain the <see cref="XtmRouteExtraData"/> component.
        /// （獲得 <see cref="XtmRouteExtraData"/> 組件的方法。）
        /// </summary>
        private Func<EntityManager, Entity, object> _xtmRouteExtraDataComponentGetter;

        /// <summary>
        /// The version of the assembly.
        /// （組件的版本。）
        /// </summary>
        private string _version = string.Empty;

        public Assembly Assembly => _assembly;

        public bool Accessible => _accessible;

        public string Name => _assemblyName;

        public string VerifiedVersion => "0.1.5.1";

        public string Version => _version;

        /// <summary>
        /// The type of the extra route data component.
        /// （額外路線資料組件的型別。）
        /// </summary>
        public Type XtmRouteExtraData => _xtmRouteExtraDataType;

        public ExtendedTransportManager() { }

        public void Dispose()
        {
            _acronymGetter = null;
            _accessible = false;
            _assembly = null;
            _version = string.Empty;
            _xtmRouteExtraDataType = null;
            _xtmRouteExtraDataComponentGetter = null;
        }

        public bool TryGet(bool verbose = true)
        {
            if (_accessible) return true;

            foreach (ModManager.ModInfo mod in Instance.Mod)
            {
                if (mod.name.StartsWith(_assemblyName))
                {
                    _assembly = mod.asset.assembly;
                    if (_assembly == null) return false;    // In case of the assembly is not yet initiated but presented.（預防組件雖出現但未初始化。）

                    _accessible = true;
                    _version = _assembly.GetName().Version.ToString();

                    if (verbose) Instance.Log.Debug($"Successfully retrieve the assembly of Extended Transport Manager [{_version}]. 成功獲取 Extended Transport Manager [{_version}] 模組組件。");
                    return true;
                }
            }

            if (verbose) Instance.Log.Debug("Failed to retrieve the assembly of Extended Transport Manager. 無法獲取 Extended Transport Manager 模組組件。");
            return false;
        }

        /// <summary>
        /// Try to retrieve the acronym name of the route defined in the Extended Transport Manager mod.
        /// （嘗試獲得在 Extended Transport Manager 模組中定義的路線縮寫。）
        /// </summary>
        /// <param name="entityManager">The entity manager.（實體管理器。）</param>
        /// <param name="entity">The route entity.（路線實體。）</param>
        /// <param name="acronym">The route acronym.（路線縮寫。）</param>
        /// <returns>Whether the retrieval is successful.（是否成功獲得縮寫？）</returns>
        public bool TryGetRouteAcronym(EntityManager entityManager, Entity entity, out string acronym)
        {
            acronym = string.Empty;
            if (!_accessible ||
                (_xtmRouteExtraDataType == null) ||
                !entityManager.HasComponent(entity, _xtmRouteExtraDataType) ||
                (_acronymGetter == null) ||
                (_xtmRouteExtraDataComponentGetter == null)) return false;
            
            acronym = _acronymGetter(_xtmRouteExtraDataComponentGetter(entityManager, entity));
            return true;
        }

        /// <summary>
        /// Try to retrieve the type of extra route data component.
        /// （嘗試取得額外路線資訊組件的型別。）
        /// </summary>
        /// <returns>Whether the query success or not.（查詢是否成功？）</returns>
        public bool TryGetXtmRouteExtraData() => TryGetXtmRouteExtraData(out _xtmRouteExtraDataType, out _xtmRouteExtraDataComponentGetter, out _acronymGetter);

        /// <summary>
        /// Try to retrieve the type of extra route data component.
        /// （嘗試取得額外路線資訊組件的型別。）
        /// </summary>
        /// <param name="xtmRouteExtraDataType">The type of the component.（組件的類別。）</param>
        /// <param name="xtmRouteExtraDataComponentGetterFunction">The function to retrieve the component.（用於獲得組件的函式。）</param>
        /// <param name="acronymGetterFunction">The function to retrieve the acronym.（用於獲得縮寫的函式。）</param>
        /// <returns>Whether the query success or not.（查詢是否成功？）</returns>
        private bool TryGetXtmRouteExtraData(out Type xtmRouteExtraDataType,
                                             out Func<EntityManager, Entity, object> xtmRouteExtraDataComponentGetterFunction,
                                             out Func<object, string> acronymGetterFunction)
        {
            xtmRouteExtraDataType = null;
            xtmRouteExtraDataComponentGetterFunction = null;
            acronymGetterFunction = null;
            if (!_accessible || !this.TryGetType(_xtmRouteExtraDataTypeName, out xtmRouteExtraDataType)) return false;

            PropertyInfo acronymProperty = xtmRouteExtraDataType.GetProperty(_acronymPropertyName, BindingFlags.Instance | BindingFlags.Public);
            if ((acronymProperty == null) || (acronymProperty.PropertyType != typeof(string))) return false;
            acronymGetterFunction = (obj) => (string)acronymProperty.GetValue(obj);

            return CommonUtils.TryGetComponentDataMethod(xtmRouteExtraDataType, out xtmRouteExtraDataComponentGetterFunction);
        }

        public override string ToString()
        {
            string status = _accessible ? _version : "Not accessible";
            return $"Extended Transport Manager({status})";
        }
    }
}