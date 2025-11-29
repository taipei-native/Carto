using Carto.Utils;
using Game.Modding;
using System;
using System.Reflection;
using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The wrapper of the assembly of <see href="https://github.com/DanielVNZ/RoadSpeedAdjuster">Road Speed Adjuster</see> mod developed by DanielVNZ.<br/>
    /// （由 DanielVNZ 開發的 <see href="https://github.com/DanielVNZ/RoadSpeedAdjuster">Road Speed Adjuster</see> 模組組件的包裝器。）
    /// </summary>
    public class RoadSpeedAdjuster : IAssembly
    {
        private const string _assemblyName = "RoadSpeedAdjuster";
        private const string _rsaCustomSpeedTypeName = "RoadSpeedAdjuster.Components.CustomSpeed";
        private const string _rsaSpeedFieldName = "m_Speed";

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

        private ComponentType _rsaCustomSpeedComponent;

        private Func<EntityManager, Entity, object> _rsaCustomSpeedComponentGetter;

        private Func<object, float> _speedGetter;

        private Type _rsaCustomSpeedType;

        /// <summary>
        /// The version of the assembly.
        /// （組件的版本。）
        /// </summary>
        private string _version = string.Empty;

        public Assembly Assembly => _assembly;

        public bool Accessible => _accessible;

        public ComponentType CustomSpeedComponent => _rsaCustomSpeedComponent;

        public string Name => _assemblyName;

        public string VerifiedVersion => "1.0.0.0";

        public string Version => _version;

        public RoadSpeedAdjuster() { }

        public void Dispose()
        {
            _accessible = false;
            _assembly = null;
            _rsaCustomSpeedComponent = default;
            _rsaCustomSpeedComponentGetter = null;
            _rsaCustomSpeedType = null;
            _speedGetter = null;
            _version = string.Empty;
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

                    if (verbose) Instance.Log.Debug($"Successfully retrieve the assembly of Road Speed Adjuster [{_version}]. 成功獲取 Road Speed Adjuster [{_version}] 模組組件。");
                    return true;
                }
            }

            if (verbose) Instance.Log.Debug("Failed to retrieve the assembly of Road Speed Adjuster. 無法獲取 Road Speed Adjuster 模組組件。");
            return false;
        }

        /// <summary>
        /// Try to retrieve the speed of the network defined in the Road Speed Adjuster mod.
        /// （嘗試獲得在 Road Speed Adjuster 模組中定義的速度。）
        /// </summary>
        /// <param name="entityManager">The entity manager.（實體管理器。）</param>
        /// <param name="entity">The network entity.（網路實體。）</param>
        /// <param name="speed">The network speed.（網路速度。）</param>
        /// <returns>Whether the retrieval is successful.（是否成功獲得速度？）</returns>
        public bool TryGetNetworkCustomSpeed(EntityManager entityManager, Entity entity, out float speed)
        {
            speed = 0f;
            if (!_accessible ||
                (_rsaCustomSpeedType == null) ||
                !entityManager.HasComponent(entity, _rsaCustomSpeedType) ||
                (_speedGetter == null) ||
                (_rsaCustomSpeedComponentGetter == null)) return false;

            speed = _speedGetter(_rsaCustomSpeedComponentGetter(entityManager, entity));
            return true;
        }

        /// <summary>
        /// Try to retrieve the type of custom speed component.
        /// （嘗試取得自訂速度組件的型別。）
        /// </summary>
        /// <returns>Whether the query success or not.（查詢是否成功？）</returns>
        public bool TryGetRsaCustomSpeed() => TryGetRsaCustomSpeed(out _rsaCustomSpeedType, out _rsaCustomSpeedComponent, out _rsaCustomSpeedComponentGetter, out _speedGetter);

        /// <summary>
        /// Try to retrieve the type of custom speed component.
        /// （嘗試取得自訂速度組件的型別。）
        /// </summary>
        /// <param name="rsaCustomSpeedType">The type of the component.（組件的類別。）</param>
        /// <param name="rsaCustomSpeedComponent">The component.（組件。）</param>
        /// <param name="rsaCustomSpeedComponentGetterFunction">The function to retrieve the component.（用於獲得組件的函式。）</param>
        /// <param name="speedGetterFunction">The function to retrieve the speed.（用於獲得速度的函式。）</param>
        /// <returns>Whether the query success or not.（查詢是否成功？）</returns>
        private bool TryGetRsaCustomSpeed(out Type rsaCustomSpeedType,
                                          out ComponentType rsaCustomSpeedComponent,
                                          out Func<EntityManager, Entity, object> rsaCustomSpeedComponentGetterFunction,
                                          out Func<object, float> speedGetterFunction)
        {
            rsaCustomSpeedType = null;
            rsaCustomSpeedComponent = default;
            rsaCustomSpeedComponentGetterFunction = null;
            speedGetterFunction = null;
            if (!_accessible || !this.TryGetType(_rsaCustomSpeedTypeName, out rsaCustomSpeedType)) return false;

            try
            {
                rsaCustomSpeedComponent = ComponentType.ReadOnly(TypeManager.GetTypeIndex(rsaCustomSpeedType));
            }
            catch (Exception)
            {
                // Handle the situation TypeManager is not working.（處理 TypeManager 異常的情況。）
                return false;
            }

            FieldInfo speedField = rsaCustomSpeedType.GetField(_rsaSpeedFieldName, BindingFlags.Public | BindingFlags.Instance);
            if ((speedField == null) || (speedField.FieldType != typeof(float))) return false;
            speedGetterFunction = (obj) => (float)speedField.GetValue(obj);

            return CommonUtils.TryGetComponentDataMethod(rsaCustomSpeedType, out rsaCustomSpeedComponentGetterFunction);
        }

        public override string ToString()
        {
            string status = _accessible ? _version : "Not accessible";
            return $"Road Speed Adjuster({status})";
        }
    }
}