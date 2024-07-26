namespace Carto.Systems
{
    using Carto.Geodata;
    using Carto.Utils;
    using Colossal.Logging;
    using Game;
    using Game.Common;
    using Game.Prefabs;
    using Game.Tools;
    using Game.Zones;
    using Purpose = Colossal.Serialization.Entities.Purpose;
    using Unity.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Unity.Collections;
    using UnityEngine;
    using Unity.Mathematics;

    /// <summary>
    /// The system instance that manages the zoning properties, inheriting from GameSystemBase.
    /// （管理分區屬性的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class ZoningSystem : GameSystemBase
    {
        private static readonly ILog m_Log = Instance.Log;
        private static readonly PrefabSystem m_Prefab = Instance.Prefab;

        // The entity query instance that searches for several instances, using Unity.Entities.EntityQuery.
        // （用於搜尋數種實例的Unity實體查詢實例。）
        private static EntityQuery _buildingRefQuery;
        private static EntityQuery _themeQuery;
        private static EntityQuery _zoningQuery;
        private static EntityQuery _zoningTypeQuery;

        /// <summary>
        /// The enum storing zoning categories.
        /// （儲存分區類別的類舉。）
        /// </summary>
        [Flags]
        public enum ZoningCategory
        {
            None = 0,
            Residential = 1,
            Commercial = 2,
            Industrial = 4,
            Office = 8
        }

        /// <summary>
        /// The enum storing zoning densities.
        /// （儲存分區密度的列舉。）
        /// </summary>
        public enum ZoningDensity
        {
            Generic,
            Low,
            Medium,
            High
        }

        /// <summary>
        /// The dictionary that holds all nodes with roundabouts and the vertices that form the roundabout's left outer edge.
        /// （記載所有包含圓環（島）的節點與組成其左側外緣的字典。）
        /// </summary>
        public static Dictionary<int, ZoningTypeInfo> ZoningTypes = new Dictionary<int, ZoningTypeInfo>();

        /// <summary>
        /// The class to store the metadata of each zoning type.
        /// （儲存各個分區類型基礎資料的類別。）
        /// </summary>
        public class ZoningTypeInfo
        {
            /// <summary>
            /// The category of the zoning type.
            /// （分區類型的分類。）
            /// </summary>
            public ZoningCategory? Category { get; set; }

            /// <summary>
            /// The rendering color of the zoning type.
            /// （分區類型的渲染顏色。）
            /// </summary>
            public string Color { get; set; }

            /// <summary>
            /// The estimated density of the zoning type.
            /// （分區類型的密度。）
            /// </summary>
            public ZoningDensity? Density { get; set; }

            /// <summary>
            /// The zoning type entity itself.
            /// （分區類型實體本身。）
            /// </summary>
            public Entity Entity { get; set; }

            /// <summary>
            /// The UID of each zoning type.
            /// （各個分區類型的唯一代碼。）
            /// </summary>
            public int? Index { get; set; }

            /// <summary>
            /// The localized name of the zoning type.
            /// （分區類型的在地化名稱。）
            /// </summary>
            public string Name => LocaleUtils.Translate($"Assets.NAME[{PrefabName}]");

            /// <summary>
            /// The prefab name of the zoning type.
            /// （分區類型的預製模板名稱。）
            /// </summary>
            public string PrefabName { get; set; }

            /// <summary>
            /// The theme of the zoning type.
            /// （分區類型的主題風格。）
            /// </summary>
            public string Theme { get; set; }

            /// <summary>
            /// The method to turn the class into a string.
            /// （用以將類別轉換為字串的方法。）
            /// </summary>
            public override string ToString()
            {
                string notSet = "Not Set";
                string category = (Category == null) ? notSet : $"{Category}";
                string density = (Density == null) ? notSet : $"{Density}";
                string index = (Index == null) ? notSet : $"{Index}";
                string name = (PrefabName == null) ? notSet : Name;
                return $"ZoningTypeInfo({index} [{name}], Category = {{{category}}}, Density = {density})";
            }
        }

        /// <summary>
        /// This event triggers when the system is created.
        /// （這是當系統實例被創造時，會被觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _buildingRefQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<SpawnableBuildingData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            
            _themeQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<ThemeData>(),
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            
            _zoningTypeQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<ZoneData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _zoningQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<Block>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            base.OnCreate();
            m_Log.Debug("ZoningSystem instance created. 分區系統實例創造完成。");
        }

        /// <summary>
        /// This event triggers when the game is loaded.
        /// （這是當遊戲載入完成時，會被觸發的事件。）
        /// </summary>
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            if (GameMode.GameOrEditor.HasFlag(mode))
            {
                List<CartoObject> zones = GetZoningProperties();
                Geodata geodata = new Geodata(zones);
                geodata.ToShapefile("ZoningBlockCenter.shp", "Center", true);
            }
        }

        /// <summary>
        /// This event triggers when the system is updated.
        /// （這是當系統實例被更新時，會被觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// This event triggers when the system is destroyed.
        /// （這是當系統實例被銷毀時，會被觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// Searching for properties of all existing zonings.
        /// （搜尋現有所有分區的屬性。）
        /// </summary>
        public List<CartoObject> GetZoningProperties()
        {
            bool zccReady = Instance.ZCCMod.Ready;
            List<CartoObject> zoneList = new List<CartoObject>(); 
            Dictionary<ThemePrefab, string> themePrefix = new Dictionary<ThemePrefab, string>();
            Dictionary<Entity, ZoningCategory> zoningTypeCategory = new Dictionary<Entity, ZoningCategory>();

            Assembly zcc = zccReady ? Instance.ZCCMod.Assembly : Assembly.GetExecutingAssembly();
            Type zccConfigUtil  = zccReady ? zcc.GetType("ZoneColorChanger.Utilities.ConfigUtil") : GetType();
            Type zccHslColor    = zccReady ? zcc.GetType("ZoneColorChanger.Domain.HslColor") : GetType();
            Type zccMod         = zccReady ? zcc.GetType("ZoneColorChanger.Mod") : GetType();
            Type zccSetting     = zccReady ? zcc.GetType("ZoneColorChanger.Setting") : GetType();
            object zccConfigUtilInstance = zccReady ? zccConfigUtil.GetProperty("Instance").GetValue(zccConfigUtil) : new object();         // public static ConfigUtil Instance { get; }
            object zccColorMode = zccReady ? zccConfigUtil.GetMethod("GetColorMode").Invoke(zccConfigUtilInstance, null) : new object();    // public ColorMode GetColorMode()
            object zccSettingsInstance = zccReady ? zccMod.GetProperty("Settings").GetValue(zccMod) : new object();                         // public static Setting Settings { get; }
            bool zccGroupThemes = zccReady ? (bool) zccSetting.GetProperty("GroupThemes").GetValue(zccSettingsInstance) : false;            // public bool GroupThemes { get; set; }

            foreach (Entity _buildingRef in _buildingRefQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    Entity buildingZonePrefab = EntityManager.GetComponentData<SpawnableBuildingData>(_buildingRef).m_ZonePrefab;

                    if (!zoningTypeCategory.TryGetValue(buildingZonePrefab, out _))
                    {
                        ZoningCategory zoningCategory = ZoningCategory.None;
                        EntityArchetype buildingObjectArchetype = EntityManager.GetComponentData<ObjectData>(_buildingRef).m_Archetype;
                        NativeArray<ComponentType> buildingEntityComponentTypes = buildingObjectArchetype.GetComponentTypes();
                        string[] buildingEntityComponentTypeNames = buildingEntityComponentTypes.Select(c => c.GetManagedType().Name).ToArray();

                        if (buildingEntityComponentTypeNames.Contains("CommercialProperty")) zoningCategory |= ZoningCategory.Commercial;
                        if (buildingEntityComponentTypeNames.Contains("IndustrialProperty") && !buildingEntityComponentTypeNames.Contains("OfficeProperty")) zoningCategory |= ZoningCategory.Industrial;
                        if (buildingEntityComponentTypeNames.Contains("OfficeProperty")) zoningCategory |= ZoningCategory.Office;
                        if (buildingEntityComponentTypeNames.Contains("ResidentialProperty")) zoningCategory |= ZoningCategory.Residential;

                        zoningTypeCategory[buildingZonePrefab] = zoningCategory;
                    }
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetZoningProperties(); 於 GetZoningProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _theme in _themeQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    PrefabData themePrefabData = EntityManager.GetComponentData<PrefabData>(_theme);
                    ThemePrefab themePrefab = m_Prefab.GetPrefab<ThemePrefab>(themePrefabData);
                    themePrefix[themePrefab] = LocaleUtils.Translate($"Assets.THEME[{m_Prefab.GetPrefabName(_theme)}]");
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetZoningProperties(); 於 GetZoningProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _zoningType in _zoningTypeQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    ZoningTypeInfo zoningTypeInfo = new ZoningTypeInfo();
                    ZoneData zoningTypeData = EntityManager.GetComponentData<ZoneData>(_zoningType);
                    PrefabData zoningTypePrefabData = EntityManager.GetComponentData<PrefabData>(_zoningType);
                    ZonePrefab zoningTypePrefab = m_Prefab.GetPrefab<ZonePrefab>(zoningTypePrefabData);
                    zoningTypeInfo.Entity = _zoningType;

                    // Retrieve the category of the zoning type. Expected output: ZoningCategory.Residential
                    // （獲取分區的分類。預期輸出：ZoningCategory.Residential）
                    ZoningCategory zoningCategory = zoningTypeCategory.TryGetValue(_zoningType, out ZoningCategory value) ? value : ZoningCategory.None;
                    zoningTypeInfo.Category = zoningCategory;

                    // Retrieve the density of the zoning type. Expected output: ZoningDensity.Low
                    // （獲取分區的密度。預期輸出：ZoningDensity.Low）
                    ZoningDensity zoningTypeDensity = ZoningDensity.Generic;

                    if (zoningCategory.HasFlag(ZoningCategory.Residential))
                    {
                        if (zoningTypeData.m_MaxHeight < 12)
                        {
                            zoningTypeDensity = ZoningDensity.Low;
                        }
                        else if (zoningTypeData.m_MaxHeight < 60)
                        {
                            zoningTypeDensity = ZoningDensity.Medium;
                        }
                        else
                        {
                            zoningTypeDensity = ZoningDensity.High;
                        }
                    }
                    else if (zoningCategory.HasFlag(ZoningCategory.Commercial) || zoningCategory.HasFlag(ZoningCategory.Office))
                    {
                        if (zoningTypeData.m_MaxHeight < 20)
                        {
                            zoningTypeDensity = ZoningDensity.Low;
                        }
                        else
                        {
                            zoningTypeDensity = ZoningDensity.High;
                        }
                    }

                    zoningTypeInfo.Density = zoningTypeDensity;

                    // Retrieve the color of the zoning type. Expected output: "#FFAA00"
                    // （獲取分區的顏色。預期輸出："#FFAA00"）
                    zoningTypeInfo.Color = "#" + ColorUtility.ToHtmlStringRGB(zoningTypePrefab.m_Color);

                    if (zccReady && (zccColorMode.ToString() != "Default"))
                    {
                        string zoningTypePrefabName = m_Prefab.GetPrefabName(_zoningType);
                        string zccZoneName = zccGroupThemes ? Regex.Replace(zoningTypePrefabName, @"^\w{2} ", string.Empty) : zoningTypePrefabName;
                        object[] zccTryGetCustomColorParams = new object[] { zccZoneName, null };
                        bool zccTryGetCustomColor = (bool)zccConfigUtil.GetMethod("TryGetCustomColor").Invoke(zccConfigUtilInstance, zccTryGetCustomColorParams); // public bool TryGetCustomColor(string key, out HslColor color)

                        if (zccTryGetCustomColor)
                        {
                            object zccCustomHslColor = zccTryGetCustomColorParams[1];
                            float zccCustomColorHue = (float)zccHslColor.GetProperty("Hue").GetValue(zccCustomHslColor);    // public float Hue { get; set; }
                            float zccCustomColorSat = (float)zccHslColor.GetProperty("Sat").GetValue(zccCustomHslColor);    // public float Sat { get; set; }
                            float zccCustomColorLum = (float)zccHslColor.GetProperty("Lum").GetValue(zccCustomHslColor);    // public float Lum { get; set; }
                            Color zccCustomRGBColor = Color.HSVToRGB(zccCustomColorHue, zccCustomColorSat, zccCustomColorLum);
                            zoningTypeInfo.Color = "#" + ColorUtility.ToHtmlStringRGB(zccCustomRGBColor);
                        }
                    }

                    // Retrieve the UID of the zoning type.
                    // （獲取分區類型的唯一代碼。）
                    int zoningTypeIndex = zoningTypeData.m_ZoneType.m_Index;
                    zoningTypeInfo.Index = zoningTypeIndex;

                    // Retrieve the name of the zoning type.
                    // （獲取分區類型的名稱。）
                    zoningTypeInfo.PrefabName = m_Prefab.GetPrefabName(_zoningType);

                    // Retrieve the theme of the zoning type. Expected output: "European"
                    // （獲取分區類型的主題風格。預期輸出："歐式"）
                    if (zoningTypePrefab.Has<ThemeObject>())
                    {
                        ThemePrefab zoningTypeThemePrefab = zoningTypePrefab.GetComponent<ThemeObject>().m_Theme;
                        zoningTypeInfo.Theme = themePrefix[zoningTypeThemePrefab];
                    }
                    else
                    {
                        zoningTypeInfo.Theme = LocaleUtils.Translate("Assets.THEME[Carto Generic]");
                    }

                    ZoningTypes[zoningTypeIndex] = zoningTypeInfo;
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetZoningProperties(); 於 GetZoningProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _zoning in _zoningQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, string>();

                    Block zoningBlock = EntityManager.GetComponentData<Block>(_zoning);
                    float3 zoningBlockCenter    = zoningBlock.m_Position;
                    float2 zoningBlockDirection = zoningBlock.m_Direction;
                    int2 zoningBlockShape       = zoningBlock.m_Size;

                    edges["Center"] = new List<float3> { zoningBlockCenter };
                    type["Center"] = CartoObject.T;

                    zoneList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetZoningProperties(); 於 GetZoningProperties() 發生一個錯誤； {ex}");
                }
            }

            return zoneList;
        }
    }
}