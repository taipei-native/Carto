using Game.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Carto.Domain
{
    /// <summary>
    /// The wrapper of the assembly of <see href="https://github.com/JadHajjar/ZoneColorChanger-CSII">Zone Color Changer</see> mod developed by TDW.<br/>
    /// （由 TDW 開發的 <see href="https://github.com/JadHajjar/ZoneColorChanger-CSII">Zone Color Changer</see> 模組組件的包裝器。）
    /// </summary>
    public class ZoneColorChanger : IAssembly
    {
        private const string _assemblyName = "ZoneColorChanger";
        private const string _groupThemesPropertyName = "GroupThemes";
        private const string _hslColorTypeName = "ZoneColorChanger.Domain.HslColor";
        private const string _implicitOperator = "op_Implicit";
        private const string _vanillaColorsFieldName = "_vanillaColors";
        private const string _zccModTypeName = "ZoneColorChanger.Mod";
        private const string _zccSettingTypeName = "ZoneColorChanger.Setting";
        private const string _zccSettingsPropertyName = "Settings";
        private const string _zccSystemTypeName = "ZoneColorChanger.Systems.ZoneColorChangerSystem";

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

        public string VerifiedVersion => "1.2.3.0";

        public string Version => _version;

        public ZoneColorChanger() { }

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
                    if (verbose) Instance.Log.Debug($"Successfully retrieve the assembly of Zone Color Changer [{_version}]. 成功獲取 Zone Color Changer [{_version}] 模組組件。");
                    return true;
                }
            }

            if (verbose) Instance.Log.Debug("Failed to retrieve the assembly of Zone Color Changer. 無法獲取 Zone Color Changer 模組組件。");
            return false;
        }

        /// <summary>
        /// Try to retrieve the color map between vanilla game and the zoning type.<br/>
        /// （嘗試取得原版分區顏色與分區類別的映射表。）
        /// </summary>
        /// <param name="map">The vanilla game's color map.（原版的顏色映射表。）</param>
        /// <param name="grouped">Whether the zoning types are grouped or not.（分區類型是否被分組？）</param>
        /// <returns>Whether the query success or not.（查詢是否成功？）</returns>
        public bool TryGetColorMap(out Dictionary<string, Color> map, out bool grouped)
        {
            map = null;
            grouped = false;
            if (!_accessible ||
                !this.TryGetType(_hslColorTypeName, out Type hslColorType) ||
                !this.TryGetType(_zccModTypeName, out Type zccModType) ||
                !this.TryGetType(_zccSystemTypeName, out Type zccSystemType) ||
                !this.TryGetType(_zccSettingTypeName, out Type zccSettingType)) return false;
            ComponentSystemBase zccSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged(zccSystemType);

            FieldInfo vanillaColorField = zccSystemType.GetField(_vanillaColorsFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (vanillaColorField == null) return false;

            object vanillaColor = vanillaColorField.GetValue(zccSystem);
            if (vanillaColor is not IDictionary vanillaColorDictionary) return false;

            PropertyInfo zccSettingsProperty = zccModType.GetProperty(_zccSettingsPropertyName, BindingFlags.Static | BindingFlags.Public);
            if (zccSettingsProperty == null) return false;

            object zccSettings = zccSettingsProperty.GetValue(zccSystem);
            if (zccSettings == null) return false;

            PropertyInfo groupThemesProperty = zccSettingType.GetProperty(_groupThemesPropertyName, BindingFlags.Instance | BindingFlags.Public);
            if (groupThemesProperty == null) return false;

            object groupThemes = groupThemesProperty.GetValue(zccSettings);
            if (groupThemes is not bool @bool) return false;

            MethodInfo implicitOperator = null;
            foreach (MethodInfo method in hslColorType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                // To avoid the ambiguity, the method info must be carefully checked.（為了避免定義模糊，方法必須被仔細檢查。）
                if ((method.Name == _implicitOperator) && (method.ReturnType == typeof(Color)) && (method.GetParameters().Length == 1) && (method.GetParameters()[0].ParameterType == hslColorType))
                {
                    implicitOperator = method;
                    break;
                }
            }
            if (implicitOperator == null) return false;

            grouped = @bool;
            map = new();
            foreach (DictionaryEntry entry in vanillaColorDictionary)
            {
                if (entry.Key is not string zoningTypeName) continue;
                Color color = (Color)implicitOperator.Invoke(null, new[] { entry.Value });
                map.Add(zoningTypeName, color);
            }

            return true;
        }

        public override string ToString()
        {
            string status = _accessible ? _version : "Not accessible";
            return $"Zone Color Changer({status})";
        }
    }
}