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
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;
        
        /// <summary>
        /// The query for existing districts.（現有行政區的查詢。）
        /// </summary>
        static EntityQuery _districtQuery;

        /// <summary>
        /// The query for existing map tiles.（現有）
        /// </summary>
        static EntityQuery _mapTileQuery;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _districtQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<District>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _mapTileQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
               {
                    ComponentType.ReadOnly<MapTile>()
               },
                None = new ComponentType[]
               {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
               }
            });

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
            NameSystem name = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NameSystem>();

            foreach (Entity _mapTile in _mapTileQuery.ToEntityArray(Allocator.Temp))
            {
                // Write feature header.（寫出圖徵檔頭。）
                writer.WriteStartObject();
                GeoJson.WritePropertyPair(writer, "type", "Feature");

                // Write feature geometry.（寫出圖徵幾何圖形。）
                writer.WritePropertyName("geometry");
                DynamicBuffer<Node> buffer = EntityManager.GetBuffer<Node>(_mapTile);
                float3[] boundary = new float3[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    Coord coord = new(options.SourceCoordinates.Double3 + buffer[i].m_Position.xzy, options.SourceCoordinates);
                    Coord transformed = Transform.Apply(coord, options.SourceProjection, CRS.WGS84, options.SourceProjectionDefinition, new ProjectionDefinition());
                    boundary[i] = transformed.Float3;
                }
                GeoJson.WriteGeometry(writer, new Geodata.Geometry(new float3[1][] { boundary }), Shape.Polygon, options.Elevation);

                // Write feature properties.（寫出圖徵）
                writer.WritePropertyName("properties");
                writer.WriteStartObject();
                GeoJson.WriteProperty(writer, Property.Name, name.GetDebugName(_mapTile));
                writer.WriteEndObject();

                writer.WriteEndObject();

                // Report method, not filled temporary.
                onReportMethod("", 0);
            }
        }
    }
}