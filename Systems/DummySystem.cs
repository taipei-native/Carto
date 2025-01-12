using Carto.Geodata;
using Carto.IO;
using Colossal.Logging;
using Game;
using Game.Areas;
using Game.Common;
using Game.Tools;
using Game.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using static Game.Rendering.OverlayRenderSystem;

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

            NativeList<float3> boundaries = new();
            NativeArray<AreaFeature> features = new(query.CalculateEntityCount(), Allocator.TempJob);
            HashSet<Property> properties = options.Properties[IO.System.Area];

            CollectAreaFeaturesJob job = new()
            {
                areaLookup = GetComponentLookup<Area>(true),
                districtLookup = GetComponentLookup<District>(true),
                extractorLookup = GetComponentLookup<Extractor>(true),
                geometryLookup = GetComponentLookup<Game.Areas.Geometry>(true),
                mapTileLookup = GetComponentLookup<MapTile>(true),
                nativeLookup = GetComponentLookup<Native>(true),
                storageLookup = GetComponentLookup<Storage>(true),
                surfaceLookup = GetComponentLookup<Surface>(true),
                nodesLookup = GetBufferLookup<Node>(true),
                features = features,
                boundaries = boundaries,
                useArea = properties.Contains(Property.Area),
                useObject = properties.Contains(Property.Object),
                useUnlocked = properties.Contains(Property.Unlocked),
                sourceCoordinates = options.SourceCoordinates,
                sourceProjectionDefinition = options.SourceProjectionDefinition,
            };

            JobHandle handle = job.ScheduleParallel(query, default);
            handle.Complete();

            for (int i = 0; i < features.Length; i++)
            {
                AreaFeature feature = features[i];
                writer.WriteStartObject();
                GeoJson.WritePropertyPair(writer, "type", "Feature");

                writer.WritePropertyName("geometry");
                float3[] boundary = new float3[feature.boundaryLength];
                for (int j = 0; j < boundary.Length; j++) boundary[j] = boundaries[feature.boundaryIndex + j];
                GeoJson.WriteGeometry(writer, new Geodata.Geometry(new float3[1][] { boundary }), Shape.Polygon, options.Elevation);

                writer.WritePropertyName("properties");
                writer.WriteStartObject();

                if (properties.Contains(Property.Name))
                {
                    GeoJson.WriteProperty(writer, Property.Name, _name.GetDebugName(feature.entity));
                }
                if (properties.Contains(Property.Area))
                {
                    GeoJson.WriteProperty(writer, Property.Area, feature.area);
                }
                if (properties.Contains(Property.Object))
                {
                    GeoJson.WriteProperty(writer, Property.Object, Enum.GetName(typeof(Feature), feature.objectType));
                }
                if (properties.Contains(Property.Unlocked))
                {
                    GeoJson.WriteProperty(writer, Property.Unlocked, feature.unlocked);
                }

                writer.WriteEndObject();

                // Report method, not filled temporary.
                onReportMethod?.Invoke(string.Empty, 0);
            }

            boundaries.Dispose();
            features.Dispose();

            //foreach (Entity _mapTile in query.ToEntityArray(Allocator.Temp))
            //{
            //    // Write feature header.（寫出圖徵檔頭。）
            //    writer.WriteStartObject();
            //    GeoJson.WritePropertyPair(writer, "type", "Feature");

            //    // Write feature geometry.（寫出圖徵幾何圖形。）
            //    writer.WritePropertyName("geometry");
            //    DynamicBuffer<Node> buffer = EntityManager.GetBuffer<Node>(_mapTile);
            //    float3[] boundary = new float3[buffer.Length];
            //    for (int i = 0; i < buffer.Length; i++)
            //    {
            //        Coord coord = new(options.SourceCoordinates.Double3 + buffer[i].m_Position.xzy, options.SourceCoordinates);
            //        boundary[i] = Transform.Apply(coord, options.SourceProjection, CRS.WGS84, options.SourceProjectionDefinition, new ProjectionDefinition()).Float3;
            //    }
            //    GeoJson.WriteGeometry(writer, new Geodata.Geometry(new float3[1][] { boundary }), Shape.Polygon, options.Elevation);

            //    // Write feature properties.（寫出圖徵）
            //    writer.WritePropertyName("properties");
            //    writer.WriteStartObject();
            //    GeoJson.WriteProperty(writer, Property.Name, _name.GetDebugName(_mapTile));
            //    writer.WriteEndObject();

            //    writer.WriteEndObject();

            //    // Report method, not filled temporary.
            //    onReportMethod?.Invoke(string.Empty, 0);
            //}
        }

        /// <summary>
        /// The job to obtain area features.
        /// （獲得區域圖徵的工作。）
        /// </summary>
        [BurstCompile]
        private partial struct CollectAreaFeaturesJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<Area> areaLookup;
            
            [ReadOnly]
            public ComponentLookup<District> districtLookup;

            [ReadOnly]
            public ComponentLookup<Extractor> extractorLookup;

            [ReadOnly]
            public ComponentLookup<Game.Areas.Geometry> geometryLookup;

            [ReadOnly]
            public ComponentLookup<MapTile> mapTileLookup;

            [ReadOnly]
            public ComponentLookup<Native> nativeLookup;

            [ReadOnly]
            public ComponentLookup<Storage> storageLookup;

            [ReadOnly]
            public ComponentLookup<Surface> surfaceLookup;

            [ReadOnly]
            public BufferLookup<Node> nodesLookup;

            [WriteOnly]
            public NativeList<float3> boundaries;

            [WriteOnly]
            public NativeArray<AreaFeature> features;

            public bool useArea;
            public bool useObject;
            public bool useUnlocked;
            public Coord sourceCoordinates;
            public ProjectionDefinition sourceProjectionDefinition;

            /// <summary>
            /// Execute the job. （執行工作。）
            /// </summary>
            /// <param name="entity">The entity that passed in.（傳入的實體。）</param>
            /// <exception cref="ArgumentNullException"></exception>
            public void Execute(Entity entity, [EntityIndexInQuery] int index)
            {
                // Initialize the container.（初始化容器。）
                AreaFeature feature = new(0f, 0, 0, 0, 0, entity, 0, Feature.None, 0, false);

                // Handle geometries.（處理幾何圖形。）
                bool isCounterClockwise = areaLookup[entity].m_Flags.HasFlag(AreaFlags.CounterClockwise);
                DynamicBuffer<Node> nodes = nodesLookup[entity];
                feature.boundaryIndex = boundaries.Length;
                feature.boundaryLength = nodes.Length;

                if (isCounterClockwise)
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        Coord coord = new(sourceCoordinates.Double3 + nodes[i].m_Position.xzy, sourceCoordinates);
                        boundaries.Add(Transform.Apply(coord, CRS.TransverseMercator, CRS.WGS84, sourceProjectionDefinition, default).Float3);
                    }
                }
                else
                {
                    for (int i = nodes.Length - 1; i > -1; i--)
                    {
                        Coord coord = new(sourceCoordinates.Double3 + nodes[i].m_Position.xzy, sourceCoordinates);
                        boundaries.Add(Transform.Apply(coord, CRS.TransverseMercator, CRS.WGS84, sourceProjectionDefinition, default).Float3);
                    }
                }

                // Handle properties.（處理屬性。）
                bool isDistrict = districtLookup.HasComponent(entity);
                bool isExtractor = extractorLookup.HasComponent(entity);
                bool isMapTile = mapTileLookup.HasComponent(entity);
                bool isStorage = storageLookup.HasComponent(entity);
                bool isSurface = surfaceLookup.HasComponent(entity);

                // Area.（面積。）
                if (useArea)
                {
                    feature.area = geometryLookup[entity].m_SurfaceArea;
                }

                // Object.（物件。）
                if (useObject)
                {
                    if (isDistrict) feature.objectType |= Feature.District;
                    if (isExtractor) feature.objectType |= Feature.Extractor;
                    if (isStorage) feature.objectType |= Feature.Landfill;
                    if (isMapTile) feature.objectType |= Feature.MapTile;
                    if (isSurface) feature.objectType |= Feature.Surface;
                }

                // Unlocked.（解鎖狀態。）
                if (useUnlocked)
                {
                    feature.unlocked = isMapTile && nativeLookup.HasComponent(entity);
                }

                features[index] = feature;
            }
        }
    }
}