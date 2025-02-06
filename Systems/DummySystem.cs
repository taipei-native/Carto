using Carto.Geodata;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Game;
using Game.Areas;
using Game.Common;
using Game.Tools;
using Game.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Carto.Systems
{
    /// <summary>
    /// The dummy system that is only used for development purposes.
    /// （用於開發用途的虛假系統。）
    /// </summary>
    public partial class DummySystem : GameSystemBase
    {
        /// <summary>
        /// The exclusion filters in the entity query.
        /// （實體查詢中排除的篩選條件。）
        /// </summary>
        static readonly List<ComponentType> _filters = new()
        {
            ComponentType.ReadOnly<Deleted>(),
            ComponentType.ReadOnly<Navigation>(),
            ComponentType.ReadOnly<Space>(),
            ComponentType.ReadOnly<Temp>()
        };
        
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The system managing names.（管理名稱的系統。）<br/>
        /// See <see cref="Instance.Name"/> for more information.
        /// </summary>
        static readonly NameSystem _name = Instance.Name;

        /// <summary>
        /// The query for existing areas.（現有區域的查詢。）
        /// </summary>
        static EntityQueryDesc _queryDesc;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>()
                }
            };
            
            base.OnCreate();
            _log.Debug("DummySystem instance created. 虛假系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Write features (geometries and properties) to the designated file.
        /// （寫出圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public void WriteFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            Feature featureFlag = options.Features;
            if (!featureFlag.HasFlag(Feature.District)) _filters.Add(ComponentType.ReadOnly<District>());
            if (!featureFlag.HasFlag(Feature.Extractor)) _filters.Add(ComponentType.ReadOnly<Extractor>());
            if (!featureFlag.HasFlag(Feature.Landfill)) _filters.Add(ComponentType.ReadOnly<Storage>());
            if (!featureFlag.HasFlag(Feature.MapTile)) _filters.Add(ComponentType.ReadOnly<MapTile>());
            if (!featureFlag.HasFlag(Feature.Surface)) _filters.Add(ComponentType.ReadOnly<Surface>());
            _queryDesc.None = _filters.ToArray();
            EntityQuery query = GetEntityQuery(_queryDesc);

            bool hasName = options.Contains(Property.Name, IO.System.Area);
            bool hasArea = options.Contains(Property.Area, IO.System.Area);
            bool hasCompany = options.Contains(Property.Company, IO.System.Area);
            bool hasEmployee = options.Contains(Property.Employee, IO.System.Area);
            bool hasHousehold = options.Contains(Property.Household, IO.System.Area);
            bool hasObject = options.Contains(Property.Object, IO.System.Area);
            bool hasResident = options.Contains(Property.Resident, IO.System.Area);
            bool hasUnlocked = options.Contains(Property.Unlocked, IO.System.Area);
            bool hasWealth = options.Contains(Property.Wealth, IO.System.Area);

            foreach (Entity _area in query.ToEntityArray(Allocator.Temp))
            {
                // Write feature header.（寫出圖徵檔頭。）
                writer.WriteStartObject();
                GeoJson.WritePropertyPair(writer, "type", "Feature");

                // Write feature geometry.（寫出圖徵幾何圖形。）
                writer.WritePropertyName("geometry");
                DynamicBuffer<Node> buffer = EntityManager.GetBuffer<Node>(_area);
                float3[] boundary = new float3[buffer.Length];
                bool isCounterClockwise = EntityManager.GetComponentData<Area>(_area).m_Flags.HasFlag(AreaFlags.CounterClockwise);

                if (isCounterClockwise)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        Coord coord = new(options.SourceCoordinates.Double3 + buffer[i].m_Position.xzy, options.SourceCoordinates);
                        boundary[i] = Transform.Apply(coord, options.SourceProjection, CRS.WGS84, options.SourceProjectionDefinition, new ProjectionDefinition()).Float3;
                    }
                }
                else
                {
                    for (int i = buffer.Length - 1; i > -1; i--)
                    {
                        Coord coord = new(options.SourceCoordinates.Double3 + buffer[i].m_Position.xzy, options.SourceCoordinates);
                        boundary[i] = Transform.Apply(coord, options.SourceProjection, CRS.WGS84, options.SourceProjectionDefinition, new ProjectionDefinition()).Float3;
                    }
                }

                GeoJson.WriteGeometry(writer, new Geodata.Geometry(new float3[1][] { boundary }), Shape.Polygon, options.Elevation);

                // Write feature properties.（寫出圖徵）
                writer.WritePropertyName("properties");
                writer.WriteStartObject();
                HashSet<Property> properties = options.Properties[IO.System.Area];
                Feature featureType = IOUtils.GetFeatureType(EntityManager, _area);

                bool isDistrict = featureType.HasFlag(Feature.District);
                bool isMapTile = featureType.HasFlag(Feature.MapTile);

                if (hasName)
                {
                    string name = isDistrict ? _name.GetRenderedLabelName(_area) : _name.GetDebugName(_area);
                    GeoJson.WriteProperty(writer, Property.Name, name);
                }
                if (hasArea)
                {
                    GeoJson.WriteProperty(writer, Property.Area, EntityManager.GetComponentData<Game.Areas.Geometry>(_area).m_SurfaceArea);
                }
                if (hasCompany)
                {

                }
                if (hasEmployee)
                {

                }
                if (hasHousehold)
                {

                }
                if (hasObject)
                {
                    Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? featureType : Utils.CommonUtils.GetFirstMatch(featureType, IO.IO.FeatureDisplayOrder);
                    GeoJson.WriteProperty(writer, Property.Object, displayType.ToString("G"));
                }
                if (hasResident)
                {

                }
                if (hasUnlocked)
                {
                    bool unlocked = featureType.HasFlag(Feature.MapTile) && !EntityManager.HasComponent<Native>(_area);
                    GeoJson.WriteProperty(writer, Property.Unlocked, unlocked);
                }
                if (hasWealth)
                {

                }
                
                writer.WriteEndObject();
                writer.WriteEndObject();

                // Report method, not filled temporary.
                onReportMethod?.Invoke(string.Empty, 0);
            }
        }
    }
}