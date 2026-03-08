using Game.Modding;
using System;
using System.Reflection;
using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The wrapper of the assembly of <see href="https://github.com/JadHajjar/RoadBuilder-CSII">Road Builder</see> mod developed by TDW.
    /// （由 TDW 開發的 <see href="https://github.com/JadHajjar/RoadBuilder-CSII">Road Builder</see> 模組組件的包裝器。）
    /// </summary>
    public class RoadBuilder : IAssembly
    {
        private const string _assemblyName = "RoadBuilder";
        private const string _rbNetworkTypeName = "RoadBuilder.Domain.Components.RoadBuilderNetwork";

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
        /// The version of the assembly.
        /// （組件的版本。）
        /// </summary>
        private string _version = string.Empty;

        public Assembly Assembly => _assembly;

        public bool Accessible => _accessible;

        public string Name => _assemblyName;

        public string VerifiedVersion => "0.6.4.0";

        public string Version => _version;

        public RoadBuilder() { }

        public void Dispose()
        {
            _accessible = false;
            _assembly = null;
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

                    if (verbose) Instance.Log.Debug($"Successfully retrieve the assembly of Road Builder [{_version}]. 成功獲取 Road Builder [{_version}] 模組組件。");
                    return true;
                }
            }

            if (verbose) Instance.Log.Debug("Failed to retrieve the assembly of Road Builder. 無法獲取 Road Builder 模組組件。");
            return false;
        }

        /// <summary>
        /// Try to retrieve the component type of Road Builder network component.
        /// （嘗試取得 Road Builder 網路組件的組件型別。）
        /// </summary>
        /// <param name="rbNetworkComponent">The component type of the component.（組件的組件型別。）</param>
        /// <returns>If true, the component type is retrievable.（若為真，則組件型別可取得。）</returns>
        public bool TryGetRbNetworkComponentType(out ComponentType rbNetworkComponent)
        {
            rbNetworkComponent = default;
            if (!_accessible || !this.TryGetType(_rbNetworkTypeName, out Type rbNetworkType)) return false;
            try
            {
                rbNetworkComponent = ComponentType.ReadOnly(TypeManager.GetTypeIndex(rbNetworkType));
            }
            catch (Exception)
            {
                // Handle the situation TypeManager is not working.（處理 TypeManager 異常的情況。）
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            string status = _accessible ? _version : "Not accessible";
            return $"Road Builder({status})";
        }
    }
}