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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using UnityEngine;

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
        public static Dictionary<ushort, ZoningTypeInfo> ZoningTypes = new Dictionary<ushort, ZoningTypeInfo>();

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
            public ushort? Index { get; set; }

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
                string color = (Color == null) ? notSet : Color;
                string density = (Density == null) ? notSet : $"{Density}";
                string index = (Index == null) ? notSet : $"{Index}";
                string name = (PrefabName == null) ? notSet : Name;
                string theme = (Theme == null) ? notSet : Theme;
                return $"ZoningTypeInfo({index} [{name}], Category = {{{category}}}, Density = {density}, Theme = {theme}, Color = {color})";
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
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode) { }

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
            Assembly zcc = Assembly.GetExecutingAssembly();
            bool zccIntegrity = true;
            bool zccReady    = Instance.ZCCMod.Ready;
            bool useCategory = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Zoning, "Category");
            bool useColor    = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Zoning, "Color");
            bool useDensity  = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Zoning, "Density");
            bool useTheme    = ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Zoning, "Theme");
            ComponentSystemBase zccSystemInstance = Instance.Area;
            List<CartoObject> zoneList = new List<CartoObject>();
            Dictionary<ThemePrefab, string> themePrefix = new Dictionary<ThemePrefab, string>();
            Dictionary<string, Color> vanillaColors = new Dictionary<string, Color>();
            Dictionary<Entity, ZoningCategory> zoningTypeCategory = new Dictionary<Entity, ZoningCategory>();
            object zccConfigUtilInstance = new object();
            Type zccColor  = GetType();
            Type zccSystem = GetType();

            if (zccReady)
            {
                zcc = Instance.ZCCMod.Assembly;

                if (zcc.GetTypes().Any(type => type.FullName == "ZoneColorChanger.Systems.ZoneColorChangerSystem"))
                {
                    zccSystem = zcc.GetType("ZoneColorChanger.Systems.ZoneColorChangerSystem");
                }
                else
                {
                    zccIntegrity = false;
                    m_Log.Debug("ZoningSystem.GetZoningProperties(): Couldn't find the type `ZoneColorChanger.Systems.ZoneColorChangerSystem`. 無法找到型別 `ZoneColorChanger.Systems.ZoneColorChangerSystem`。");
                }

                if (zcc.GetTypes().Any(type => type.FullName == "ZoneColorChanger.Domain.HslColor"))
                {
                    zccColor = zcc.GetType("ZoneColorChanger.Domain.HslColor");
                }
                else
                {
                    zccIntegrity = false;
                    m_Log.Debug("ZoningSystem.GetZoningProperties(): Couldn't find the type `ZoneColorChanger.Domain.HslColor`. 無法找到型別 `ZoneColorChanger.Domain.HslColor`。");
                }

                Color HslConverter(object zccHslColor)
                {
                    if (zccIntegrity)
                    {
                        float hue = (float)zccColor.GetProperty("Hue").GetValue(zccHslColor, null);
                        float sat = (float)zccColor.GetProperty("Sat").GetValue(zccHslColor, null);
                        float lum = (float)zccColor.GetProperty("Lum").GetValue(zccHslColor, null);
                        return Color.HSVToRGB(hue, sat, lum);
                    }
                    else
                    {
                        return new Color();
                    }
                }

                zccSystemInstance = World.GetExistingSystemManaged(zccSystem);
                IDictionary temp = (IDictionary) zccSystem.GetField("_vanillaColors", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(zccSystemInstance);
                foreach (DictionaryEntry kvp in temp) vanillaColors[kvp.Key as string] = HslConverter(kvp.Value);
            }

            if (useCategory || useDensity)
            {
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
            }

            if (useTheme)
            {
                foreach(Entity _theme in _themeQuery.ToEntityArray(Allocator.Temp))
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
                    if (useCategory || useDensity || Instance.Settings.UseUnzoned)
                    {
                        ZoningCategory zoningCategory = zoningTypeCategory.TryGetValue(_zoningType, out ZoningCategory value) ? value : ZoningCategory.None;
                        zoningTypeInfo.Category = zoningCategory;

                        // Retrieve the density of the zoning type. Expected output: ZoningDensity.Low
                        // （獲取分區的密度。預期輸出：ZoningDensity.Low）
                        if (useDensity)
                        {
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
                        }
                    }

                    // Retrieve the color of the zoning type. Expected output: "#FFAA00"
                    // （獲取分區的顏色。預期輸出："#FFAA00"）
                    if (useColor) zoningTypeInfo.Color = "#" + ColorUtility.ToHtmlStringRGB(zoningTypePrefab.m_Color);
                    if (useColor && !Instance.Settings.UseZCC && zccIntegrity) zoningTypeInfo.Color = "#" + ColorUtility.ToHtmlStringRGB(vanillaColors[m_Prefab.GetPrefabName(_zoningType)]);

                    // Retrieve the UID of the zoning type.
                    // （獲取分區類型的唯一代碼。）
                    ushort zoningTypeIndex = zoningTypeData.m_ZoneType.m_Index;
                    zoningTypeInfo.Index = zoningTypeIndex;

                    // Retrieve the name of the zoning type.
                    // （獲取分區類型的名稱。）
                    zoningTypeInfo.PrefabName = m_Prefab.GetPrefabName(_zoningType);

                    // Retrieve the theme of the zoning type. Expected output: "European"
                    // （獲取分區類型的主題風格。預期輸出："歐式"）
                    if (useTheme)
                    {
                        if (zoningTypePrefab.Has<ThemeObject>())
                        {
                            ThemePrefab zoningTypeThemePrefab = zoningTypePrefab.GetComponent<ThemeObject>().m_Theme;
                            zoningTypeInfo.Theme = themePrefix[zoningTypeThemePrefab];
                        }
                        else
                        {
                            zoningTypeInfo.Theme = LocaleUtils.Translate("Assets.THEME[Carto Generic]");
                        }
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
                    Block zoningBlock = EntityManager.GetComponentData<Block>(_zoning);
                    NativeArray<Cell> zoningCells = EntityManager.GetBuffer<Cell>(_zoning).ToNativeArray(Allocator.Temp);
                    float m = zoningBlock.m_Direction.x;
                    float n = zoningBlock.m_Direction.y;
                    int r = zoningBlock.m_Size.x;
                    int s = zoningBlock.m_Size.y;
                    float l = math.sqrt(m * m + n * n);
                    float3 u = new float3(-n / l, 0, m / l) * 8;
                    float3 v = new float3(m / l, 0, n / l) * 8;
                    float3 p0 = zoningBlock.m_Position - r * u / 2 - s * v / 2;

                    for (int i = 0; i < s; i++)
                    {
                        for (int j = 0; j < r; j++)
                        {
                            var edges = new Dictionary<string, List<float3>>();
                            var props = new Dictionary<string, object>();
                            var type = new Dictionary<string, string>();

                            Cell cell = zoningCells[j + r * (s - i - 1)];
                            CellFlags cellStatus = cell.m_State;
                            ZoningTypeInfo zoningTypeInfo = ZoningTypes[cell.m_Zone.m_Index];

                            if (!cellStatus.HasFlag(CellFlags.Blocked))
                            {
                                if (!Instance.Settings.UseUnzoned && (zoningTypeInfo.Category == ZoningCategory.None)) continue;

                                // Retrieve the zoning type of each zoning block cell. Expected output: "Low Rent Housing"
                                // （獲得每個分區單元的分區類別。預期輸出："低租金住宅"）
                                props["Name"] = zoningTypeInfo.Name;

                                if (useCategory) props["Category"] = zoningTypeInfo.Category.ToString();
                                if (useColor) props["Color"] = zoningTypeInfo.Color;
                                if (useDensity) props["Density"] = zoningTypeInfo.Density.ToString();

                                // Retrieve the boundary of each zoning block cell. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                                // （獲得每個分區單元的邊界。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                                if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Zoning, "Edge"))
                                {
                                    float3 p1 = p0 + j * u + i * v;
                                    float3 p2 = p0 + j * u + (i + 1) * v;
                                    float3 p3 = p0 + (j + 1) * u + (i + 1) * v;
                                    float3 p4 = p0 + (j + 1) * u + i * v;
                                    edges["Edge"] = new List<float3> { p1, p2, p3, p4 };
                                    type["Edge"] = CartoObject.P;
                                }

                                if (useTheme) props["Theme"] = zoningTypeInfo.Theme;

                                if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Zoning, "Object")) props["Object"] = "Zoning";

                                zoneList.Add(new CartoObject(edges, props, type));
                            }
                        }
                    }
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