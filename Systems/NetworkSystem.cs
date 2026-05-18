using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Carto.Systems
{
    /// <summary>
    /// The system that searches networks.
    /// （搜尋網路的系統。）
    /// </summary>
    public partial class NetworkSystem : GameSystemBase
    {
        /// <summary>
        /// The interpolation gap for the nodes.
        /// （節點的內插間隔。）
        /// </summary>
        public const float NodeInterpolationGap = 0.5f;

        /// <summary>
        /// The interpolation threshold of the nodes.
        /// （節點的內插閾值。）
        /// </summary>
        public const float NodeInterpolationThreshold = 1f;

        /// <summary>
        /// The interpolation gap for the roundabouts.
        /// （圓環的內插間隔。）
        /// </summary>
        public const float RoundaboutInterpolationGap = 0.2f;

        /// <summary>
        /// The interpolation threshold of the roundabouts.
        /// （圓環的內插閾值。）
        /// </summary>
        public const float RoundaboutInterpolationThreshold = 0.5f;

        /// <summary>
        /// The interpolation gap for the segments.
        /// （路段的內插間隔。）
        /// </summary>
        public const float SegmentInterpolationGap = 1f;

        /// <summary>
        /// The interpolation threshold of the segments.
        /// （路段的內插閾值。）
        /// </summary>
        public const float SegmentInterpolationThreshold = 1f;
        
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The wrapper managing names.（管理名稱的包裝器。）<br/>
        /// See <see cref="Instance.Name"/> for more information.
        /// </summary>
        static readonly NameManager _name = Instance.Name;

        /// <summary>
        /// The assembly of Road Builder mod.（Road Builder 模組組件。）<br/>
        /// See <see cref="Instance.Rb"/> for more information.
        /// </summary>
        static readonly RoadBuilder _rb = Instance.Rb;

        /// <summary>
        /// The assembly of Road Speed Adjuster mod.（Road Speed Adjuster 模組組件。）<br/>
        /// See <see cref="Instance.Rsa"/> for more information.
        /// </summary>
        static readonly RoadSpeedAdjuster _rsa = Instance.Rsa;

        /// <summary>
        /// The query to collect all lane prefabs.
        /// （收集所有車道網路預製模板的查詢。）
        /// </summary>
        static EntityQuery _lanePrefabQuery;

        /// <summary>
        /// The query to collect all network entities.
        /// （收集所有網路實體的查詢。）
        /// </summary>
        static EntityQuery _networkQuery;

        /// <summary>
        /// The query to collect all roundabouts.
        /// （收集所有圓環的查詢。）
        /// </summary>
        static EntityQuery _roundaboutQuery;

        /// <summary>
        /// The query to collect the attachment of roundabouts.
        /// （收集圓環附件的查詢。）
        /// </summary>
        static EntityQuery _roundaboutAttachmentQuery;

        /// <summary>
        /// The query to collect the prefab of roundabouts attachments.
        /// （收集圓環附件預製模板的查詢。）
        /// </summary>
        static EntityQuery _roundaboutAttachmentPrefabQuery;

        /// <summary>
        /// The query to collect UI object categories.
        /// （收集 UI 物件分類的查詢。）
        /// </summary>
        static EntityQuery _uiCategoryQuery;

        /// <summary>
        /// The query to collect all utility service network entities.
        /// （收集所有公用事業管線網路實體的查詢。）
        /// </summary>
        //static EntityQuery _utilityServiceNetworkQuery;

        /// <summary>
        /// The query for networks.（網路的查詢。）
        /// </summary>
        static EntityQueryDesc _networkEntityQueryDesc;

        /// <summary>
        /// The category of the networks.
        /// （網路的分類。）
        /// </summary>
        private List<NetworkCategory> _networkCategories;

        /// <summary>
        /// The asset of the networks.
        /// （網路的資產。）
        /// </summary>
        private List<string> _networkAssets;

        /// <summary>
        /// The title of the networks.
        /// （網路的標題。）
        /// </summary>
        private List<string> _networkNames;

        /// <summary>
        /// The list of all network's statistics in the savegame.
        /// （遊戲存檔內所有網路的統計數據。）
        /// </summary>
        private NativeList<NetworkStat> _localNetworkStats;

        /// <summary>
        /// The map between roundabout node and its statistics.
        /// （圓環節點與統計資訊的映射表。）
        /// </summary>
        private NativeParallelHashMap<Entity, Domain.Roundabout> _roundaboutEntityMap;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _networkEntityQueryDesc = new()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Composition>(),
                    ComponentType.ReadOnly<Curve>(),
                    ComponentType.ReadOnly<Edge>(),
                    ComponentType.ReadOnly<EdgeGeometry>(),
                    ComponentType.ReadOnly<Game.Net.SubLane>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            };
            
            _lanePrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<NetLaneData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<SecondaryLaneData>()
                }
            });

            _networkQuery = GetEntityQuery(_networkEntityQueryDesc);

            _roundaboutQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ConnectedEdge>(),
                    ComponentType.ReadOnly<Node>(),
                    ComponentType.ReadOnly<Game.Net.Roundabout>(),
                    ComponentType.ReadOnly<Game.Objects.SubObject>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _roundaboutAttachmentQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Attached>(),
                    ComponentType.ReadOnly<Game.Objects.NetObject>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Building>(),
                    ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
                    ComponentType.ReadOnly<Game.Routes.TransportStop>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _roundaboutAttachmentPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ObjectGeometryData>(),
                    ComponentType.ReadOnly<NetObjectData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _uiCategoryQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<UIAssetCategoryData>(),
                    ComponentType.ReadOnly<UIObjectData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            //_utilityServiceNetworkQuery = GetEntityQuery(new EntityQueryDesc()
            //{
            //    All = new ComponentType[]
            //    {
            //        ComponentType.ReadOnly<Edge>()
            //    },
            //    Any = new ComponentType[]
            //    {
            //        ComponentType.ReadOnly<ElectricityNodeConnection>(),
            //        ComponentType.ReadOnly<WaterPipeNodeConnection>()
            //    }
            //});

            base.OnCreate();
            _log.Debug("NetworkSystem instance created. 網路系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy()
        {
            _rb.Dispose();
            Dispose();
            base.OnDestroy();
        }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Construct the network centerline.
        /// （建構網路中心線。）
        /// </summary>
        /// <param name="networkStat">The list of all network's statistics in the savegame.（遊戲存檔內所有網路的統計數據。）</param>
        /// <param name="nodes">The list of vertices.（頂點列表。）</param>
        /// <param name="roundaboutEntityMap">The map between roundabout node and its statistics.（圓環節點與統計資訊的映射表。）</param>
        /// <param name="curveLookup">The lookup that searches for <see cref="Curve"/>.（搜尋 <see cref="Curve"/> 的查詢。）</param>
        /// <param name="edgeLookup">The lookup that searches for <see cref="Edge"/>.（搜尋 <see cref="Edge"/> 的查詢。）</param>
        /// <param name="endNodeGeometryLookup">The lookup that searches for <see cref="EndNodeGeometry"/>.（搜尋 <see cref="EndNodeGeometry"/> 的查詢。）</param>
        /// <param name="nodeLookup">The lookup that searches for <see cref="Node"/>.（搜尋 <see cref="Node"/> 的查詢。）</param>
        /// <param name="startNodeGeometryLookup">The lookup that searches for <see cref="StartNodeGeometry"/>.（搜尋 <see cref="StartNodeGeometry"/> 的查詢。）</param>
        private static void ConstructCenterlineNodes(NetworkStat networkStat, ref NativeList<double3> nodes, ref NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap,
                                                     ref ComponentLookup<Curve> curveLookup, ref ComponentLookup<Edge> edgeLookup, ref ComponentLookup<EndNodeGeometry> endNodeGeometryLookup,
                                                     ref ComponentLookup<Node> nodeLookup, ref ComponentLookup<StartNodeGeometry> startNodeGeometryLookup)
        {
            Entity network = networkStat.entity;
            if (curveLookup.TryGetComponent(network, out Curve curveComponent) &&
                endNodeGeometryLookup.TryGetComponent(network, out EndNodeGeometry endNodeGeometryComponent) &&
                startNodeGeometryLookup.TryGetComponent(network, out StartNodeGeometry startNodeGeometryComponent) &&
                edgeLookup.TryGetComponent(network, out Edge edgeComponent) &&
                nodeLookup.TryGetComponent(edgeComponent.m_Start, out Node startNodeComponent) &&
                nodeLookup.TryGetComponent(edgeComponent.m_End, out Node endNodeComponent))
            {
                Bezier4x3 endCurve = endNodeGeometryComponent.m_Geometry.m_Middle;
                Bezier4x3 mainCurve = curveComponent.m_Bezier;
                Bezier4x3 startCurve = startNodeGeometryComponent.m_Geometry.m_Middle;
                float3 endNodePosition = endNodeComponent.m_Position;
                float3 startNodePosition = startNodeComponent.m_Position;
                bool includeEndCurve = Colossal.Mathematics.MathUtils.Length(endCurve) > 0;
                bool includeStartCurve = Colossal.Mathematics.MathUtils.Length(startCurve) > 0;

                // Detect whether the curve is reversed. A normal curve should be arranged in the order of "start.a → start.d → (main) → end.a → end.d".
                // （偵測曲線是否被反轉。一個正常的曲線應該以「start.a → start.d → (main) → end.a → end.d」的順序排列。）
                Utils.MathUtils.IsContinuous(startCurve, mainCurve, out float tStart, true);
                Utils.MathUtils.IsContinuous(mainCurve, endCurve, out float tEnd, false);

                // Trim the excessive parts of the main curve.
                // （裁剪主曲線的多餘部分。）
                Bezier4x3 trimmedMainCurve = Utils.MathUtils.Trim(mainCurve, includeStartCurve ? tStart : 0f, includeEndCurve ? tEnd : 1f);

                // Extend the trimmed main curve to the start & end nodes.
                // （將裁剪後的主曲線延伸至起訖節點。）
                if (includeEndCurve)
                {
                    Line3.Segment endVector = new(trimmedMainCurve.d, endNodePosition);
                    Line3.Segment endReverseVector = Utils.MathUtils.StartReflect(new(trimmedMainCurve.d, trimmedMainCurve.c));
                    float3 controlPoint = Utils.MathUtils.StartTrim(endReverseVector, math.distance(endNodePosition, Colossal.Mathematics.MathUtils.Position(endVector, 0.33f))).b;
                    endCurve = new(trimmedMainCurve.d, controlPoint, endNodePosition, endNodePosition);
                }

                if (includeStartCurve)
                {
                    Line3.Segment startVector = new(startNodePosition, trimmedMainCurve.a);
                    Line3.Segment startReverseVector = Utils.MathUtils.StartReflect(new(trimmedMainCurve.a, trimmedMainCurve.b));
                    float3 controlPoint = Utils.MathUtils.StartTrim(startReverseVector, math.distance(startNodePosition, Colossal.Mathematics.MathUtils.Position(startVector, 0.33f))).b;
                    startCurve = new(startNodePosition, startNodePosition, controlPoint, trimmedMainCurve.a);
                }

                // Interpolate and combine the curve.
                // （內插及結合曲線。）
                if (includeStartCurve)
                {
                    Utils.MathUtils.Interpolate(startCurve, ref nodes, NodeInterpolationThreshold, NodeInterpolationGap, false, out _,
                                                new(0d, 0d), Geodata.CRS.Game, Geodata.CRS.Game, default, default);
                }

                Utils.MathUtils.Interpolate(trimmedMainCurve, ref nodes, SegmentInterpolationThreshold, SegmentInterpolationGap, !includeEndCurve, out _,
                                            new(0d, 0d), Geodata.CRS.Game, Geodata.CRS.Game, default, default);

                if (includeEndCurve)
                {
                    Utils.MathUtils.Interpolate(endCurve, ref nodes, NodeInterpolationThreshold, NodeInterpolationGap, true, out _,
                                                new(0d, 0d), Geodata.CRS.Game, Geodata.CRS.Game, default, default);
                }

                // Handle roundabouts.
                // （處理圓環。）
                if (networkStat.roundabout.x && roundaboutEntityMap.TryGetValue(edgeComponent.m_Start, out Domain.Roundabout startRoundabout))
                {
                    Utils.MathUtils.Intersection(ref nodes, startRoundabout, isStartNode: true, out int startConnectionIndex, out double3 startConnection);
                    Utils.CommonUtils.Insert(ref nodes, startConnectionIndex, startConnection);
                    nodes.RemoveRange(0, startConnectionIndex);
                }

                if (networkStat.roundabout.y && roundaboutEntityMap.TryGetValue(edgeComponent.m_End, out Domain.Roundabout endRoundabout))
                {
                    Utils.MathUtils.Intersection(ref nodes, endRoundabout, isStartNode: false, out int endConnectionIndex, out double3 endConnection);
                    Utils.CommonUtils.Insert(ref nodes, endConnectionIndex, endConnection);
                    nodes.ResizeUninitialized(endConnectionIndex + 1);
                }
            }
        }

        /// <summary>
        /// Try disposing of all properties stored in unmanaged memory.
        /// （嘗試丟棄儲存於未控管記憶體的屬性。）
        /// </summary>
        public void Dispose()
        {
            Utils.CommonUtils.Dispose(ref _localNetworkStats);
            Utils.CommonUtils.Dispose(ref _roundaboutEntityMap);
            _networkAssets = null;
            _networkCategories = null;
            _networkNames = null;
        }

        // TODO: Unfinished.（未完成。）
        /// <summary>
        /// Get the bicycle volume from the sublane buffer.
        /// （由 sublane 緩衝區獲得自行車的流量。）
        /// </summary>
        /// <param name="subLanes">The buffer of sub lanes.（子車道的緩衝區。）</param>
        /// <param name="secondaryFlowLookup">The lookup for <see cref="SecondaryFlow"/>.（搜尋 <see cref="SecondaryFlow"/> 的查詢。）</param>
        /// <returns>The number of bikes using the road.（使用道路的自行車數量。）</returns>
        private static float GetBikeVolume(DynamicBuffer<Game.Net.SubLane> subLanes, ref ComponentLookup<SecondaryFlow> secondaryFlowLookup)
        {
            for (int i = 0; i < subLanes.Length; i++)
            {
                Game.Net.SubLane subLane = subLanes[i];
                if (((subLane.m_PathMethods & Game.Pathfind.PathMethod.Bicycle) != 0) && secondaryFlowLookup.TryGetComponent(subLane.m_SubLane, out SecondaryFlow secondaryFlow))
                {
                    // How to correctly calculate the volume?（如何正確計算流量？）
                }
            }

            return 0f;
        }

        /// <summary>
        /// Retrieve the boundary of the networks.
        /// （獲得網路的邊界。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="nodeEntityMap">The map between boundary nodes and the networks.（網路與邊界節點的映射表。）</param>
        private void GetBoundaries(Options options, ref NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap)
        {
            int networkCount = _localNetworkStats.Length;
            NativeParallelHashMap<Entity, int> nodeCountEntityMap = new(networkCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, int> statsIndexMap = new(networkCount, Allocator.Persistent);
            IOUtils.GetTargetProjections(options, out Geodata.CRS targetCRS, out ProjectionDefinition targetProjection);

            PopulateBoundaryPreRequirementsJob populatePreRequirementsJob = new()
            {
                edgeGeometryLookup = GetComponentLookup<EdgeGeometry>(true),
                endNodeGeometryLookup = GetComponentLookup<EndNodeGeometry>(true),
                startNodeGeometryLookup = GetComponentLookup<StartNodeGeometry>(true),
                networkStats = _localNetworkStats,
                nodeCountMap = nodeCountEntityMap.AsParallelWriter(),
                statsIndexMap = statsIndexMap.AsParallelWriter(),
            };
            JobHandle populatePreRequirementsHandle = populatePreRequirementsJob.Schedule(networkCount, 32, default);
            populatePreRequirementsHandle.Complete();

            CollectBoundariesJob collectBoundariesJob = new()
            {
                connectedEdgeBufferLookup = GetBufferLookup<ConnectedEdge>(true),
                compositionLookup = GetComponentLookup<Composition>(true),
                edgeGeometryLookup = GetComponentLookup<EdgeGeometry>(true),
                endNodeGeometryLookup = GetComponentLookup<EndNodeGeometry>(true),
                localConnectLookup = GetComponentLookup<LocalConnect>(true),
                nodeLookup = GetComponentLookup<Node>(true),
                roadCompositionLookup = GetComponentLookup<RoadComposition>(true),
                startNodeGeometryLookup = GetComponentLookup<StartNodeGeometry>(true),
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                networkStats = _localNetworkStats,
                nodeCountEntityMap = nodeCountEntityMap,
                roundaboutEntityMap = _roundaboutEntityMap,
                statsEntityMap = statsIndexMap,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                nodeEntityMap = nodeEntityMap.AsParallelWriter()
            };
            JobHandle collectBoundariesHandle = collectBoundariesJob.Schedule(networkCount, 16, default);
            collectBoundariesHandle.Complete();

            Utils.CommonUtils.Dispose(ref nodeCountEntityMap);
            Utils.CommonUtils.Dispose(ref statsIndexMap);
        }

        /// <summary>
        /// Retrieve the network category from <paramref name="networkStat"/>.
        /// （由 <paramref name="networkStat"/> 獲得網路分類。）
        /// </summary>
        /// <param name="roadClassification">The classification method of road networks.（道路網路的分類方式。）</param>
        /// <param name="networkStat">The statistics of the network.（網路的統計資訊。）</param>
        /// <param name="roadCategoryUIGroups">The dictionary between the UI group entity and the netowrk category.（UI 群組實體與網路分類的字典。）</param>
        /// <returns>The category of the network.（網路的分類。）</returns>
        private static NetworkCategory GetCategory(RoadClassification roadClassification, NetworkStat networkStat, Dictionary<Entity, NetworkCategory> roadCategoryUIGroups)
        {
            NetworkCategory categories = networkStat.category;
            if ((categories & NetworkCategory.Car) == 0) return categories;

            categories &= ~NetworkCategory.Car;

            switch (roadClassification)
            {
                case RoadClassification.Limit:
                    float limit = networkStat.limit;
                    if (limit >= 60)
                    {
                        categories |= NetworkCategory.Large;
                    }
                    else if (limit >= 50)
                    {
                        categories |= NetworkCategory.Medium;
                    }
                    else
                    {
                        categories |= NetworkCategory.Small;
                    }
                    break;

                case RoadClassification.Vanilla:
                    if (roadCategoryUIGroups.TryGetValue(networkStat.uiGroup, out NetworkCategory roadCategory))
                    {
                        categories |= roadCategory;
                    }
                    else
                    {
                        categories = GetCategory(RoadClassification.Width, networkStat, roadCategoryUIGroups);
                    }
                    break;

                case RoadClassification.Width:
                    float width = networkStat.width;
                    if (width >= 32)
                    {
                        categories |= NetworkCategory.Large;
                    }
                    else if (width >= 24)
                    {
                        categories |= NetworkCategory.Medium;
                    }
                    else
                    {
                        categories |= NetworkCategory.Small;
                    }
                    break;
            }

            return categories;
        }

        /// <summary>
        /// Retrieve the centerline of the networks.
        /// （獲得網路的中心線。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="nodeEntityMap">The map between centerline nodes and the networks.（網路與中心線節點的映射表。）</param>
        private void GetCenterlines(Options options, ref NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap)
        {
            int networkCount = _localNetworkStats.Length;
            NativeParallelHashMap<Entity, int> nodeCountEntityMap = new(networkCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, int> statsIndexMap = new(networkCount, Allocator.Persistent);
            IOUtils.GetTargetProjections(options, out Geodata.CRS targetCRS, out ProjectionDefinition targetProjection);

            PopulateCenterlinePreRequirementsJob populatePreRequirementsJob = new()
            {
                connectedEdgeBufferLookup = GetBufferLookup<ConnectedEdge>(true),
                curveLookup = GetComponentLookup<Curve>(true),
                endNodeGeometryLookup = GetComponentLookup<EndNodeGeometry>(true),
                startNodeGeometryLookup = GetComponentLookup<StartNodeGeometry>(true),
                networkStats = _localNetworkStats,
                roundaboutEntityMap = _roundaboutEntityMap,
                nodeCountEntityMap = nodeCountEntityMap.AsParallelWriter(),
                statsIndexMap = statsIndexMap.AsParallelWriter(),
            };
            JobHandle populatePreRequirementsHandle = populatePreRequirementsJob.Schedule(networkCount, 16, default);
            populatePreRequirementsHandle.Complete();

            CollectCenterlinesJob collectCenterlinesJob = new()
            {
                connectedEdgeBufferLookup = GetBufferLookup<ConnectedEdge>(true),
                curveLookup = GetComponentLookup<Curve>(true),
                edgeLookup = GetComponentLookup<Edge>(true),
                endNodeGeometryLookup = GetComponentLookup<EndNodeGeometry>(true),
                nodeLookup = GetComponentLookup<Node>(true),
                startNodeGeometryLookup = GetComponentLookup<StartNodeGeometry>(true),
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                networkStats = _localNetworkStats,
                nodeCountEntityMap = nodeCountEntityMap,
                statsIndexMap = statsIndexMap,
                roundaboutEntityMap = _roundaboutEntityMap,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                nodeEntityMap = nodeEntityMap.AsParallelWriter()
            };
            JobHandle collectCenterlinesHandle = collectCenterlinesJob.Schedule(networkCount, 16, default);
            collectCenterlinesHandle.Complete();

            Utils.CommonUtils.Dispose(ref nodeCountEntityMap);
            Utils.CommonUtils.Dispose(ref statsIndexMap);
        }

        /// <summary>
        /// Get the form of the road from the NetCompositionData component.
        /// （由 NetCompositionData 部件獲得道路的形式。）
        /// </summary>
        /// <param name="netCompositionData">The NetCompositionData component.（NetCompositionData 部件。）</param>
        /// <returns>The form of the road.（道路的形式。）</returns>
        private static Form GetForm(NetCompositionData netCompositionData) 
        {
            Form form = Form.Normal;
            CompositionFlags.General generalCompositions = netCompositionData.m_Flags.m_General;
            if ((generalCompositions & CompositionFlags.General.Elevated) != 0) form = Form.Elevated;
            if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) form = Form.Tunnel;
            return form;
        }

        /// <summary>
        /// Retrieve the field infos.
        /// （獲得欄位資訊。）
        /// </summary>
        /// <param name="fieldMap">The managed map between property and field infos.（屬性與欄位資訊的受控管記憶體映射表。）</param>
        private void GetFieldInfos(Dictionary<Property, FieldInfo> fieldMap)
        {
            NativeHashMap<EnumWrapper<Property>, FieldInfo> propertyFieldInfoNativeMap = new(IO.IO.PropertyCount, Allocator.Persistent);

            AggregateFieldInfosJob aggregateJob = new()
            {
                networkStats = _localNetworkStats,
                propertyFieldInfoMap = propertyFieldInfoNativeMap
            };
            JobHandle aggregateHandle = aggregateJob.Schedule();
            aggregateHandle.Complete();

            NativeHashMap<EnumWrapper<Property>, FieldInfo>.Enumerator enumerator = propertyFieldInfoNativeMap.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Property property = enumerator.Current.Key.value;
                FieldInfo fieldInfo = enumerator.Current.Value;
                fieldMap.Add(property, Shapefile.ApplyFieldInfoConstraint(property, fieldInfo));
            }

            Utils.CommonUtils.Dispose(ref propertyFieldInfoNativeMap);
        }

        /// <summary>
        /// Retrieve the statistics of the network lanes.
        /// （獲得網路的車道統計資訊。）
        /// </summary>
        /// <param name="compositionLanes">The buffer of network lanes.（網路車道的緩衝區。）</param>
        /// <param name="laneEntityMap">The map between lane prefab entities and the statistics.（車道預製模板與統計資訊的映射表。）</param>
        /// <param name="count">The number of motorized vehicle lanes.（機動車輛車道的數量。）</param>
        /// <param name="categories">The category of the network.（網路的分類。）</param>
        private static void GetLaneStatistics(DynamicBuffer<NetCompositionLane> compositionLanes, ref NativeParallelHashMap<Entity, Domain.Lane> laneEntityMap,
                                              out int count, out NetworkCategory categories)
        {
            categories = NetworkCategory.None;
            count = 0;
            for (int i = 0; i < compositionLanes.Length; i++)
            {
                NetCompositionLane lane = compositionLanes[i];
                if (((lane.m_Flags & LaneFlags.Road) != 0) & ((lane.m_Flags & LaneFlags.Master) == 0)) count++;
                if (laneEntityMap.TryGetValue(lane.m_Lane, out Domain.Lane laneStruct) && !laneStruct.IsUtilityLane) categories |= laneStruct.category;
            }
        }

        /// <summary>
        /// Retrieve the statistics of networks,
        /// （獲得網路的統計資訊。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        private void GetNetworkStats(Options options)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap = ref _roundaboutEntityMap;
            ref NativeList<NetworkStat> stats = ref _localNetworkStats;

            // Export options.（輸出設定。）
            Feature featureFlag = options.Features;
            bool hasVectorKind = options.VectorKinds.TryGetValue(IO.System.Network, out VectorKind networkKinds);
            bool hasCenterline = hasVectorKind && ((networkKinds & VectorKind.Centerline) != 0);

            // Initialize native containers.（初始化原生容器。）
            int roundaboutCount = _roundaboutQuery.CalculateEntityCount();
            int networkCount = _networkQuery.CalculateEntityCount();
            if (hasCenterline) networkCount += roundaboutCount;
            NativeParallelHashMap<Entity, Domain.Lane> laneEntityMap = new(_lanePrefabQuery.CalculateEntityCount(), Allocator.Persistent);
            NativeParallelHashMap<Entity, float> rsaNetworkSpeedMap = default; // Handle by GetRsaNetworkMap()
            NativeParallelHashSet<Entity> rbNetworks = default; // Handle by GetRbNetworks()

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset(ref roundaboutEntityMap, roundaboutCount);
            Utils.CommonUtils.Reset(ref stats, networkCount);

            try
            {
                // Retrieve the set of Road Builder networks.
                // （獲得 Road Builder 製作的網路集合。）
                GetRbNetworks(ref rbNetworks);

                // Retrieve the custom speeds defined in Road Speed Adjuster.
                // （獲得在 Road Speed Adjuster 中自定的速度。）
                GetRsaNetworkMap(ref rsaNetworkSpeedMap);

                // Retrieve lane prefabs.
                // （獲得車道預製模板。）
                CollectLanesJob collectLanesJob = new()
                {
                    carLaneDataLookup = GetComponentLookup<CarLaneData>(true),
                    parkingLaneDataLookup = GetComponentLookup<ParkingLaneData>(true),
                    trackLaneDataLookup = GetComponentLookup<TrackLaneData>(true),
                    utilityLaneDataLookup = GetComponentLookup<UtilityLaneData>(true),
                    entityMap = laneEntityMap.AsParallelWriter()
                };
                JobHandle collectLanesHandle = collectLanesJob.ScheduleParallel(_lanePrefabQuery, default);
                collectLanesHandle.Complete();

                // Collect roundabout information.
                // （收集圓環資訊。）
                GetRoundabouts(hasCenterline, options.Features, ref stats, ref roundaboutEntityMap);

                // Finally, collect the network statistics.
                //（最後，收集網路統計資訊。）
                CollectNetworkStatsJob collectNetworksJob = new()
                {
                    netCompositionLaneBufferLookup = GetBufferLookup<NetCompositionLane>(true),
                    aggregatedLookup = GetComponentLookup<Aggregated>(true),
                    electricityNodeConnectionLookup = GetComponentLookup<ElectricityNodeConnection>(true),
                    markerLookup = GetComponentLookup<Game.Net.Marker>(true),
                    netCompositionDataLookup = GetComponentLookup<NetCompositionData>(true),
                    pathwayCompositionLookup = GetComponentLookup<PathwayComposition>(true),
                    pipelineDataLookup = GetComponentLookup<PipelineData>(true),
                    powerLineDataLookup = GetComponentLookup<PowerLineData>(true),
                    prefabRefLookup = GetComponentLookup<PrefabRef>(true),
                    roadLookup = GetComponentLookup<Road>(true),
                    roadCompositionLookup = GetComponentLookup<RoadComposition>(true),
                    taxiwayCompositionLookup = GetComponentLookup<TaxiwayComposition>(true),
                    trackCompositionLookup = GetComponentLookup<TrackComposition>(true),
                    uiObjectDataLookup = GetComponentLookup<UIObjectData>(true),
                    waterPipeNodeConnectionLookup = GetComponentLookup<WaterPipeNodeConnection>(true),
                    waterwayCompositionLookup = GetComponentLookup<WaterwayComposition>(true),
                    feature = featureFlag,
                    laneEntityMap = laneEntityMap,
                    roundaboutEntityMap = roundaboutEntityMap,
                    rbNetworks = rbNetworks,
                    rsaNetworkSpeedMap = rsaNetworkSpeedMap,
                    list = stats.AsParallelWriter()
                };
                JobHandle collectNetworksHandle = collectNetworksJob.ScheduleParallel(_networkQuery, default);
                collectNetworksHandle.Complete();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                Utils.CommonUtils.Dispose(ref laneEntityMap);
                Utils.CommonUtils.Dispose(ref rbNetworks);
                Utils.CommonUtils.Dispose(ref rsaNetworkSpeedMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                // Don't dispose `_localNetworkStats` and `_roundaboutEntityMap`, they are required in `Write__DBF()` or `Write__Features()`.
                // （不要拋棄 `_localNetworkStats` 和 `_roundaboutEntityMap`，它們仍會被 `Write__DBF()` 或 `Write__Features()` 呼叫。）
                Utils.CommonUtils.Dispose(ref laneEntityMap);
                Utils.CommonUtils.Dispose(ref rbNetworks);
                Utils.CommonUtils.Dispose(ref rsaNetworkSpeedMap);
            }
        } 

        /// <summary>
        /// Retrieve the networks created by Road Builder - these networks require extra care.<br/>
        /// （獲得由 Road Builder 製作的道路－這些道路需要額外照料。）
        /// </summary>
        /// <param name="rbNetworks">The set of Road Builder network entities.（Road Builder 網路實體的集合。）</param>
        private void GetRbNetworks(ref NativeParallelHashSet<Entity> rbNetworks)
        {
            if (_rb.TryGet(false) && _rb.TryGetRbNetworkComponentType(out ComponentType roadBuilderNetworkComponent))
            {
                List<ComponentType> rbNetworkQueryComponents = new();
                rbNetworkQueryComponents.AddRange(_networkEntityQueryDesc.All);
                rbNetworkQueryComponents.Add(roadBuilderNetworkComponent);
                EntityQuery rbNetworkQuery = GetEntityQuery(new EntityQueryDesc()
                {
                    All = rbNetworkQueryComponents.ToArray(),
                    None = _networkEntityQueryDesc.None
                });

                rbNetworks = new(rbNetworkQuery.CalculateEntityCount(), Allocator.Persistent);

                CollectRbNetworksJob collectRbNetworksJob = new()
                {
                    rbNetworks = rbNetworks.AsParallelWriter()
                };
                JobHandle collectRbNetworksHandle = collectRbNetworksJob.ScheduleParallel(rbNetworkQuery, default);
                collectRbNetworksHandle.Complete();
            }
            else
            {
                Utils.CommonUtils.Reset(ref rbNetworks, 1, Allocator.Persistent);
            }
        }

        /// <summary>
        /// Retrieve the map between the network entities with the custom speed component and their speed defined in Road Speed Adjuster mod.
        /// （獲得有自訂速度組件的網路實體與其在 Road Speed Adjuster 模組中定義的速度映射表。）
        /// </summary>
        /// <param name="rsaCustomSpeedMap">The map.（映射表。）</param>
        private void GetRsaNetworkMap(ref NativeParallelHashMap<Entity, float> rsaCustomSpeedMap)
        {
            if (_rsa.TryGet(false) && _rsa.TryGetRsaCustomSpeed())
            {
                List<ComponentType> rsaNetworkCustomSpeedQueryComponents = new();
                rsaNetworkCustomSpeedQueryComponents.AddRange(_networkEntityQueryDesc.All);
                rsaNetworkCustomSpeedQueryComponents.Add(_rsa.CustomSpeedComponent);
                EntityQuery rsaNetworkCustomSpeedQuery = GetEntityQuery(new EntityQueryDesc()
                {
                    All = rsaNetworkCustomSpeedQueryComponents.ToArray(),
                    None = _networkEntityQueryDesc.None
                });

                NativeArray<Entity> rsaCustomSpeedNetworks = rsaNetworkCustomSpeedQuery.ToEntityArray(Allocator.Temp);
                Utils.CommonUtils.Reset(ref rsaCustomSpeedMap, capacity: rsaCustomSpeedNetworks.Length);

                for (int i = 0; i < rsaCustomSpeedNetworks.Length; i++)
                {
                    Entity network = rsaCustomSpeedNetworks[i];
                    if (_rsa.TryGetNetworkCustomSpeed(EntityManager, network, out float speed)) rsaCustomSpeedMap.TryAdd(network, speed);
                }
            }
            else
            {
                Utils.CommonUtils.Reset(ref rsaCustomSpeedMap, 1, Allocator.Persistent);
            }
        }

        /// <summary>
        /// Retrieve the vanilla road UI groups.
        /// （獲得遊戲原版的道路 UI 群組。）
        /// </summary>
        /// <returns>The dictionary between the UI group entity and the netowrk category.（UI 群組實體與網路分類的字典。）</returns>
        private Dictionary<Entity, NetworkCategory> GetRoadCategoryUIGroups()
        {
            Dictionary<Entity, NetworkCategory> roadCategoryUIGroups = new();
            NativeArray<Entity> uiGroups = _uiCategoryQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < uiGroups.Length; i++)
            {
                Entity uiGroup = uiGroups[i];
                string groupName = _name.GetPrefabName(uiGroup);
                switch (groupName)
                {
                    case "RoadsSmallRoads":
                        roadCategoryUIGroups.Add(uiGroup, NetworkCategory.Small);
                        break;

                    case "RoadsMediumRoads":
                        roadCategoryUIGroups.Add(uiGroup, NetworkCategory.Medium);
                        break;

                    case "RoadsLargeRoads":
                        roadCategoryUIGroups.Add(uiGroup, NetworkCategory.Large);
                        break;

                    default:
                        break;
                }
            }
            return roadCategoryUIGroups;
        }

        /// <summary>
        /// Retrieve the roundabout information.
        /// （獲得圓環資訊。）
        /// </summary>
        /// <param name="hasCenterline">Whether the export options has the geometry Centerline.（輸出設定是否含有中心線幾何？）</param>
        /// <param name="feature">The export options.（輸出設定。）</param>
        /// <param name="stats">The list of network statistics.（網路統計資訊的列表。）</param>
        /// <param name="roundaboutEntityMap">The map between roundabout node and its statistics.（圓環節點與統計資訊的映射表。）</param>
        private void GetRoundabouts(bool hasCenterline, Feature feature,
                                    ref NativeList<NetworkStat> stats, ref NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap)
        {
            NativeParallelHashMap<Entity, Entity> attachmentEntityMap = new(_roundaboutAttachmentQuery.CalculateEntityCount(), Allocator.Persistent);
            NativeParallelHashMap<Entity, float> attachmentRadiusMap = new(_roundaboutAttachmentPrefabQuery.CalculateEntityCount(), Allocator.Persistent);

            MapRoundaboutAttachmentsJob mapAttachmentsJob = new() { entityMap = attachmentEntityMap.AsParallelWriter() };
            JobHandle mapAttachmentHandle = mapAttachmentsJob.ScheduleParallel(_roundaboutAttachmentQuery, default);
            mapAttachmentHandle.Complete();

            CollectRoundaboutAttachmentPrefabsJob collectAttachmentsJob = new() { attachementRadiusMap = attachmentRadiusMap.AsParallelWriter() };
            JobHandle collectAttachmentsHandle = collectAttachmentsJob.ScheduleParallel(_roundaboutAttachmentPrefabQuery, default);
            collectAttachmentsHandle.Complete();

            CollectRoundaboutsJob collectRoundaboutsJob = new()
            {
                hasCenterline = hasCenterline,
                leftHandTraffic = Instance.City.leftHandTraffic,
                netCompositionLaneBufferLookup = GetBufferLookup<NetCompositionLane>(true),
                aggregatedLookup = GetComponentLookup<Aggregated>(true),
                compositionLookup = GetComponentLookup<Composition>(true),
                edgeLookup = GetComponentLookup<Edge>(true),
                netCompositionDataLookup = GetComponentLookup<NetCompositionData>(true),
                pillarLookup = GetComponentLookup<Pillar>(true),
                prefabRefLookup = GetComponentLookup<PrefabRef>(true),
                roadLookup = GetComponentLookup<Road>(true),
                roadCompositionLookup = GetComponentLookup<RoadComposition>(true),
                subwayTrackLookup = GetComponentLookup<SubwayTrack>(true),
                trackCompositionLookup = GetComponentLookup<TrackComposition>(true),
                trainTrackLookup = GetComponentLookup<TrainTrack>(true),
                tramTrackLookup = GetComponentLookup<TramTrack>(true),
                uiObjectDataLookup = GetComponentLookup<UIObjectData>(true),
                feature = feature,
                attachmentEntityMap = attachmentEntityMap,
                attachmentRadiusMap = attachmentRadiusMap,
                stats = stats.AsParallelWriter(),
                roundaboutEntityMap = roundaboutEntityMap.AsParallelWriter()
            };
            JobHandle collectRoundaboutsHandle = collectRoundaboutsJob.ScheduleParallel(_roundaboutQuery, default);
            collectRoundaboutsHandle.Complete();

            Utils.CommonUtils.Dispose(ref attachmentEntityMap);
            Utils.CommonUtils.Dispose(ref attachmentRadiusMap);
        }

        /// <summary>
        /// Get the volume of the road from the Road component.
        /// （由 Road 部件獲得道路的流量。）
        /// </summary>
        /// <param name="road">The Road component.（Road 部件。）</param>
        /// <returns>The number of vehicles using the road.（使用道路的車輛數量。）</returns>
        private static float GetVolume(Road road)
        {
            // According to `Game.UI.InGame.RoadSection`, the volume data is calculated every 6 hours.
            // The x, y, z, and w property represent the distance / duration at 00:00, 06:00, 12:00 and 18:00.
            // Carto uses the traffic volume at noon to show the traffic in peak hour.
            // （根據 `Game.UI.InGame.RoadSection`，流量每 6 小時計算一次，其中 x、y、z、w 屬性分別代表 00:00、06:00、12:00、18:00 的距離／持續時間。）
            // （為展示尖峰時間的車流量，Carto 使用中午的數據。）
            return (float)Math.Round((road.m_TrafficFlowDistance0.z + road.m_TrafficFlowDistance1.z) * 8f / 3f, 2);
        }

        /// <summary>
        /// Prepare all network statistics before exporting files.
        /// （在輸出檔案前準備好網路統計資料。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        private void PrepareNetworkStats(Options options)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Network);
            bool hasAsset = options.Contains(Property.Asset, IO.System.Network);
            bool hasCategory = options.Contains(Property.Category, IO.System.Network);

            // Retrieve network statistics.（獲得網路的統計資訊。）
            GetNetworkStats(options);

            /*
             * The `_localNetworkStats` and `_roundaboutEntityMap` properties should be initialized in `GetNetworkStats()`.
             * The additional validity check is performed to ensure safety.
             * （`_localNetworkStats` 及 `_roundaboutEntityMap` 屬性應在 `GetNetworkStats()` 被初始化。為確保安全，將執行額外的驗證。）
             */
            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref _localNetworkStats, true);
            Utils.CommonUtils.ValidateIntegrity(ref _roundaboutEntityMap, true);

            // Initialize managed containers.（初始化控管容器。）
            _networkAssets ??= new();
            _networkCategories ??= new();
            _networkNames ??= new();
            _networkAssets.Clear();
            _networkCategories.Clear();
            _networkNames.Clear();
            Dictionary<Entity, NetworkCategory> roadCategoryUIGroups = GetRoadCategoryUIGroups();

            // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
            if (hasName || hasAsset || hasCategory)
            {
                for (int i = 0; i < _localNetworkStats.Length; i++)
                {
                    NetworkStat networkStat = _localNetworkStats[i];
                    string aggregationName = (networkStat.aggregation != Entity.Null) ? _name.GetLabelName(networkStat.aggregation) : string.Empty;
                    Match assetTitleMatch = Regex.Match(aggregationName, @"Assets\.NAME\[(.*?)\]");
                    if (assetTitleMatch.Success)
                    {
                        string prefabName = assetTitleMatch.Groups[1].Value;
                        if (LocaleUtils.TryTranslate(aggregationName, out string assetName))
                        {
                            aggregationName = assetName;
                        }
                        else if (LocaleUtils.TryTranslate($"SubServices.NAME[{prefabName}s]", out string subServiceName))
                        {
                            // Pathways（人行道）
                            aggregationName = subServiceName;
                        }
                        else if (LocaleUtils.TryTranslate($"Infoviews.INFOMODE[{prefabName}s]", out string infoviewName))
                        {
                            // Subway & train tracks（地鐵與火車軌道）
                            aggregationName = infoviewName;
                        }
                        else if ((prefabName == "Seaway") && LocaleUtils.TryTranslate("Infoviews.INFOMODE[Waterways]", out string infoviewWaterwayName))
                        {
                            // Waterway（航道）
                            aggregationName = infoviewWaterwayName;
                        }
                        else
                        {
                            aggregationName = string.Empty;
                        }
                    }

                    _networkNames.Add(aggregationName);

                    if (networkStat.isRoundabout)
                    {
                        _networkAssets.Add(LocaleUtils.TryTranslate($"Assets.NAME[{_name.GetPrefabName(networkStat.prefab)}]", out string assetName) ? assetName : string.Empty);
                    }
                    else
                    {
                        _networkAssets.Add(_name.GetLabelName(networkStat.entity));
                    }

                    _networkCategories.Add(GetCategory(options.RoadClassification, networkStat, roadCategoryUIGroups));
                }
            }
        }

        /// <summary>
        /// Round the speed limit to more "beautiful" numbers.
        /// （將速度限制數字取整為「漂亮的」數字。）
        /// </summary>
        /// <param name="input">The input value.（輸入的數值。）</param>
        /// <returns>The speed limit value.（速度限制數值。）</returns>
        private static float RoundSpeedLimit(float input) => (float) Math.Round(input * 1.8);

        /// <summary>
        /// Write boundary features (geometries and properties) to the designated file.
        /// （寫出邊界圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        private void WriteBoundaryFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap = new(_networkQuery.CalculateEntityCount(), Allocator.Persistent);

            try
            {
                GetBoundaries(options, ref nodeEntityMap);

                Task writerThread = Task.Run(() =>
                {
                    for (int i = 0; i < _localNetworkStats.Length; i++)
                    {
                        NetworkStat networkStat = _localNetworkStats[i];
                        if (!nodeEntityMap.TryGetValue(networkStat.entity, out NativeList<double3> nodes)) continue;

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");

                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        GeoJson.WriteGeometry(writer, new Geometry(ref nodes), Shape.Polygon, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();

                        WriteGeoJson(writer, options, networkStat, i, _networkNames, _networkAssets, _networkCategories);

                        writer.WriteEndObject();
                        writer.WriteEndObject();

                        // Report method, not filled temporary.
                        onReportMethod?.Invoke(string.Empty, 0);
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
            }
        }

        /// <summary>
        /// Write boundary geometries to the designated file.
        /// （寫出邊界幾何至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        private void WriteBoundarySHP(BinaryWriter writer, Options options, out List<Shapefile.IndexPair> indexPairs, out Bounds3 bounds, out List<Entity> entitySyncList)
        {
            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap = new(_networkQuery.CalculateEntityCount(), Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<Entity> _entitySyncList = new();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                GetBoundaries(options, ref nodeEntityMap);

                Task writerThread = Task.Run(() =>
                {
                    int enumeratorIndex = 0;
                    int shapeId = Shapefile.GetShapeType(VectorKind.Boundary, options.Elevation);
                    NativeParallelHashMap<Entity, NativeList<double3>>.Enumerator enumerator = nodeEntityMap.GetEnumerator();

                    if (BitConverter.IsLittleEndian)
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeList<double3>> feature = enumerator.Current;
                            Shapefile.WriteGeometryLE(writer, enumeratorIndex, shapeId, new(ref feature.Value), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(feature.Key);
                            _indexPairs.Add(indexPair);
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeList<double3>> feature = enumerator.Current;
                            Shapefile.WriteGeometryBE(writer, enumeratorIndex, shapeId, new(ref feature.Value), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(feature.Key);
                            _indexPairs.Add(indexPair);
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                bounds = _bounds;
                entitySyncList = _entitySyncList;
                indexPairs = _indexPairs;
            }
        }

        /// <summary>
        /// Write centerline features (geometries and properties) to the designated file.
        /// （寫出中線圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        private void WriteCenterlineFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap = new(_networkQuery.CalculateEntityCount(), Allocator.Persistent);

            try
            {
                GetCenterlines(options, ref nodeEntityMap);
                
                Task writerThread = Task.Run(() =>
                {
                    for (int i = 0; i < _localNetworkStats.Length; i++)
                    {
                        NetworkStat networkStat = _localNetworkStats[i];
                        if (!nodeEntityMap.TryGetValue(networkStat.entity, out NativeList<double3> nodes)) continue;

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");

                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        GeoJson.WriteGeometry(writer, new Geometry(ref nodes), Shape.LineString, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();

                        WriteGeoJson(writer, options, networkStat, i, _networkNames, _networkAssets, _networkCategories);

                        writer.WriteEndObject();
                        writer.WriteEndObject();

                        // Report method, not filled temporary.
                        onReportMethod?.Invoke(string.Empty, 0);
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
            }
        }

        /// <summary>
        /// Write centerline geometries to the designated file.
        /// （寫出中心線幾何至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        private void WriteCenterlineSHP(BinaryWriter writer, Options options, out List<Shapefile.IndexPair> indexPairs, out Bounds3 bounds, out List<Entity> entitySyncList)
        {
            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap = new(_networkQuery.CalculateEntityCount(), Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<Entity> _entitySyncList = new();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                GetCenterlines(options, ref nodeEntityMap);

                Task writerThread = Task.Run(() =>
                {
                    int enumeratorIndex = 0;
                    int shapeId = Shapefile.GetShapeType(VectorKind.Centerline, options.Elevation);
                    NativeParallelHashMap<Entity, NativeList<double3>>.Enumerator enumerator = nodeEntityMap.GetEnumerator();

                    if (BitConverter.IsLittleEndian)
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeList<double3>> feature = enumerator.Current;
                            Shapefile.WriteGeometryLE(writer, enumeratorIndex, shapeId, new(ref feature.Value), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(feature.Key);
                            _indexPairs.Add(indexPair);
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeList<double3>> feature = enumerator.Current;
                            Shapefile.WriteGeometryBE(writer, enumeratorIndex, shapeId, new(ref feature.Value), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _entitySyncList.Add(feature.Key);
                            _indexPairs.Add(indexPair);
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                bounds = _bounds;
                entitySyncList = _entitySyncList;
                indexPairs = _indexPairs;
            }
        }

        /// <summary>
        /// Write attributes to the designated DBF file.
        /// （寫出屬性至指定的 DBF 檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        /// <param name="fieldMap">The map between the property and the fields.（屬性與欄位的映射表。）</param>
        private void WriteDBF(BinaryWriter writer, Options options, HashSet<Property> validatedFields, List<Entity> entitySyncList, out Dictionary<Property, FieldInfo> fieldMap)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Network) && validatedFields.Contains(Property.Name);
            bool hasAsset = options.Contains(Property.Asset, IO.System.Network) && validatedFields.Contains(Property.Asset);
            bool hasCapacity = options.Contains(Property.Capacity, IO.System.Network) && validatedFields.Contains(Property.Capacity);
            bool hasCategory = options.Contains(Property.Category, IO.System.Network) && validatedFields.Contains(Property.Category);
            bool hasDirection = options.Contains(Property.Direction, IO.System.Network) && validatedFields.Contains(Property.Direction);
            bool hasDischarge = options.Contains(Property.Discharge, IO.System.Network) && validatedFields.Contains(Property.Discharge);
            bool hasElevation = options.Contains(Property.Elevation, IO.System.Network) && validatedFields.Contains(Property.Elevation);
            bool hasForm = options.Contains(Property.Form, IO.System.Network) && validatedFields.Contains(Property.Form);
            bool hasLane = options.Contains(Property.Lane, IO.System.Network) && validatedFields.Contains(Property.Lane);
            bool hasLength = options.Contains(Property.Length, IO.System.Network) && validatedFields.Contains(Property.Length);
            bool hasLimit = options.Contains(Property.Limit, IO.System.Network) && validatedFields.Contains(Property.Limit);
            bool hasLoad = options.Contains(Property.Load, IO.System.Network) && validatedFields.Contains(Property.Load);
            bool hasObject = options.Contains(Property.Object, IO.System.Network) && validatedFields.Contains(Property.Object);
            bool hasVolume = options.Contains(Property.Volume, IO.System.Network) && validatedFields.Contains(Property.Volume);
            bool hasWidth = options.Contains(Property.Width, IO.System.Network) && validatedFields.Contains(Property.Width);

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref _localNetworkStats, true);

            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, int> syncMap = new(_localNetworkStats.Length, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            Dictionary<Property, FieldInfo> _fieldMap = new();

            try
            {
                FieldInfo assetField = new(0, 0, false, FieldType.String);
                FieldInfo categoryField = new(0, 0, false, FieldType.String);
                FieldInfo directionField = new(Direction.Backward.ToString("G"));
                FieldInfo formField = new(Form.Elevated.ToString("G"));
                FieldInfo nameField = new(0, 0, false, FieldType.String);
                FieldInfo objectField = new(0, 0, false, FieldType.String);

                // Sync the entity order with that of the .shp file.（與 .shp 檔案的實體順序同步。）
                Shapefile.SyncStatsToIndex(entitySyncList, ref _localNetworkStats, ref syncMap);

                // Retrieve the field info for Burst-compatible fields.（獲得可 Burst 編譯屬性的欄位資訊。）
                GetFieldInfos(_fieldMap);

                for (int index = 0; index < entitySyncList.Count; index++)
                {
                    if (!syncMap.TryGetValue(entitySyncList[index], out int i))
                    {
                        _log.Error($"Couldn't find the statistical object of {entitySyncList[index]} at index {index}. 無法找到位於索引值 {index} 的實體 {entitySyncList[index]} 之統計物件。");
                        continue;
                    }

                    NetworkCategory category = _networkCategories[i];
                    NetworkStat networkStat = _localNetworkStats[i];
                    Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? networkStat.Object : Utils.CommonUtils.GetFirstMatch(networkStat.Object, IO.IO.FeatureDisplayOrder);
                    category = options.Display[(Property.Category, IO.System.Network)] ? category : Utils.CommonUtils.GetFirstMatch(category, IO.IO.NetworkCategoryDisplayOrder);

                    nameField  += new FieldInfo(_networkNames[i]);
                    assetField += new FieldInfo(_networkAssets[i]);
                    categoryField += new FieldInfo(category.ToString("G"));
                    objectField += new FieldInfo(displayType.ToString("G"));
                }

                _fieldMap.Add(Property.Asset, assetField);
                _fieldMap.Add(Property.Category, categoryField);
                _fieldMap.Add(Property.Direction, directionField);
                _fieldMap.Add(Property.Form, formField);
                _fieldMap.Add(Property.Name, nameField);
                _fieldMap.Add(Property.Object, objectField);
                
                Task writerThread = Task.Run(() =>
                {
                    for (int index = 0; index < entitySyncList.Count; index++)
                    {
                        if (!syncMap.TryGetValue(entitySyncList[index], out int i))
                        {
                            // Do nothing. The order of records have to be consistent across .dbf, .shp, and .shx files.
                            // （不做任何事。記錄的順序必須在 .dbf、.shp 和 .shx 檔案間保持一致。）
                        }
                        
                        NetworkStat networkStat = _localNetworkStats[i];
                        Entity network = networkStat.entity;
                        writer.Write((byte)32);

                        if (hasName)
                        {
                            Shapefile.WriteRecord(writer, nameField, _networkNames[i]);
                        }
                        if (hasAsset)
                        {
                            Shapefile.WriteRecord(writer, assetField, _networkAssets[i]);
                        }
                        //if (hasCapacity)
                        //{
                        // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasCategory)
                        {
                            NetworkCategory category = _networkCategories[i];
                            category = options.Display[(Property.Category, IO.System.Network)] ? category : Utils.CommonUtils.GetFirstMatch(category, IO.IO.NetworkCategoryDisplayOrder);
                            Shapefile.WriteRecord(writer, categoryField, category.ToString("G"));
                        }
                        if (hasDirection)
                        {
                            Shapefile.WriteRecord(writer, directionField, networkStat.direction.ToString("G"));
                        }
                        //if (hasDischarge)
                        //{
                        // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasElevation && _fieldMap.TryGetValue(Property.Elevation, out FieldInfo elevationField))
                        {
                            Shapefile.WriteRecord(writer, elevationField, networkStat.elevation);
                        }
                        if (hasForm)
                        {
                            Shapefile.WriteRecord(writer, formField, networkStat.form.ToString("G"));
                        }
                        if (hasLane && _fieldMap.TryGetValue(Property.Lane, out FieldInfo laneField))
                        {
                            Shapefile.WriteRecord(writer, laneField, networkStat.lane);
                        }
                        if (hasLength && _fieldMap.TryGetValue(Property.Length, out FieldInfo lengthField))
                        {
                            Shapefile.WriteRecord(writer, lengthField, networkStat.length);
                        }
                        if (hasLimit && _fieldMap.TryGetValue(Property.Limit, out FieldInfo limitField))
                        {
                            Shapefile.WriteRecord(writer, limitField, networkStat.limit);
                        }
                        //if (hasLoad)
                        //{
                        // TODO: Not available in 1.0.0 release.
                        //}
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? networkStat.Object : Utils.CommonUtils.GetFirstMatch(networkStat.Object, IO.IO.FeatureDisplayOrder);
                            Shapefile.WriteRecord(writer, objectField, displayType.ToString("G"));
                        }
                        if (hasVolume && _fieldMap.TryGetValue(Property.Volume, out FieldInfo volumeField))
                        {
                            Shapefile.WriteRecord(writer, volumeField, networkStat.volume);
                        }
                        if (hasWidth && _fieldMap.TryGetValue(Property.Width, out FieldInfo widthField))
                        {
                            Shapefile.WriteRecord(writer, widthField, networkStat.width);
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                Utils.CommonUtils.Dispose(ref syncMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                // The lifecycle of system native containers is managed by `WriteShapefiles()`. DON'T dispose of them here.
                // （系統原生容器的生命週期是由 `WriteShapefiles()` 管理的。「不要」在此拋棄它們。）
                fieldMap = _fieldMap;
                Utils.CommonUtils.Dispose(ref syncMap);
            }
        }

        /// <summary>
        /// Write features (geometries and properties) to the designated file.
        /// （寫出圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        /// <param name="filesCount">The number of exported files.（輸出的檔案數量。）</param>
        public void WriteFeatures(Options options, Action<string, int> onReportMethod, out int filesCount)
        {
            filesCount = 0;

            try
            {
                PrepareNetworkStats(options);

                if (options.Has(IO.System.Network, VectorKind.Boundary))
                {
                    GeoJson.Write(options, IO.System.Network, VectorKind.Boundary, WriteBoundaryFeatures, onReportMethod);
                    filesCount++;
                }
                if (options.Has(IO.System.Network, VectorKind.Centerline))
                {
                    GeoJson.Write(options, IO.System.Network, VectorKind.Centerline, WriteCenterlineFeatures, onReportMethod);
                    filesCount++;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                IO.IO.DisposeAll();
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Write <see cref="NetworkStat"/> as GeoJSON.
        /// （將 <see cref="NetworkStat"/> 寫出為 GeoJSON。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="networkStat">The network statistic container.（網路統計資訊容器。）</param>
        /// <param name="statsIndex"><paramref name="networkStat"/>'s index in <see cref="_localNetworkStats"/>.（<paramref name="networkStat"/> 在 <see cref="_localNetworkStats"/> 的索引。）</param>
        /// <param name="networkNames">The list of network names.（網路名稱列表。）</param>
        /// <param name="networkAssets">The list of network asset titles.（網路資產名稱列表。）</param>
        /// <param name="networkCategories">The list of network categories.（網路分類列表。）</param>
        private static void WriteGeoJson(JsonTextWriter writer, Options options, NetworkStat networkStat, int statsIndex,
                                        List<string> networkNames, List<string> networkAssets, List<NetworkCategory> networkCategories)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Network);
            bool hasAsset = options.Contains(Property.Asset, IO.System.Network);
            bool hasCapacity = options.Contains(Property.Capacity, IO.System.Network);
            bool hasCategory = options.Contains(Property.Category, IO.System.Network);
            bool hasDirection = options.Contains(Property.Direction, IO.System.Network);
            bool hasDischarge = options.Contains(Property.Discharge, IO.System.Network);
            bool hasElevation = options.Contains(Property.Elevation, IO.System.Network);
            bool hasForm = options.Contains(Property.Form, IO.System.Network);
            bool hasLane = options.Contains(Property.Lane, IO.System.Network);
            bool hasLength = options.Contains(Property.Length, IO.System.Network);
            bool hasLimit = options.Contains(Property.Limit, IO.System.Network);
            bool hasLoad = options.Contains(Property.Load, IO.System.Network);
            bool hasObject = options.Contains(Property.Object, IO.System.Network);
            bool hasVolume = options.Contains(Property.Volume, IO.System.Network);
            bool hasWidth = options.Contains(Property.Width, IO.System.Network);

            if (hasName)
            {
                GeoJson.WriteProperty(writer, Property.Name, networkNames[statsIndex]);
            }
            if (hasAsset)
            {
                GeoJson.WriteProperty(writer, Property.Asset, networkAssets[statsIndex]);
            }
            //if (hasCapacity)
            //{
            // TODO: Not available in 1.0.0 release.
            //}
            if (hasCategory)
            {
                NetworkCategory category = networkCategories[statsIndex];
                category = options.Display[(Property.Category, IO.System.Network)] ? category : Utils.CommonUtils.GetFirstMatch(category, IO.IO.NetworkCategoryDisplayOrder);
                GeoJson.WriteProperty(writer, Property.Category, category.ToString("G"));
            }
            if (hasDirection)
            {
                GeoJson.WriteProperty(writer, Property.Direction, networkStat.direction.ToString("G"));
            }
            //if (hasDischarge)
            //{
            // TODO: Not available in 1.0.0 release.
            //}
            if (hasElevation)
            {
                GeoJson.WriteProperty(writer, Property.Elevation, networkStat.elevation);
            }
            if (hasForm)
            {
                GeoJson.WriteProperty(writer, Property.Form, networkStat.form.ToString("G"));
            }
            if (hasLane)
            {
                GeoJson.WriteProperty(writer, Property.Lane, networkStat.lane);
            }
            if (hasLength)
            {
                GeoJson.WriteProperty(writer, Property.Length, networkStat.length);
            }
            if (hasLimit)
            {
                GeoJson.WriteProperty(writer, Property.Limit, networkStat.limit);
            }
            //if (hasLoad)
            //{
            // TODO: Not available in 1.0.0 release.
            //}
            if (hasObject)
            {
                Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? networkStat.Object : Utils.CommonUtils.GetFirstMatch(networkStat.Object, IO.IO.FeatureDisplayOrder);
                GeoJson.WriteProperty(writer, Property.Object, displayType.ToString("G"));
            }
            if (hasVolume)
            {
                GeoJson.WriteProperty(writer, Property.Volume, networkStat.volume);
            }
            if (hasWidth)
            {
                GeoJson.WriteProperty(writer, Property.Width, networkStat.width);
            }
        }

        /// <summary>
        /// Write features (geometries and properties) to the designated Shapefile.
        /// （寫出圖徵（幾何與屬性）至指定的 Shapefile 中。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        /// <param name="filesCount">The number of exported files.（輸出的檔案數量。）</param>
        public void WriteShapefiles(Options options, Action<string, int> onReportMethod, out int filesCount)
        {
            filesCount = 0;

            try
            {
                PrepareNetworkStats(options);

                if (options.Has(IO.System.Network, VectorKind.Boundary))
                {
                    Shapefile.Write<Entity>(options, IO.System.Network, VectorKind.Boundary, WriteBoundarySHP, WriteDBF, onReportMethod);
                    filesCount += 5;
                }
                if (options.Has(IO.System.Network, VectorKind.Centerline))
                {
                    Shapefile.Write<Entity>(options, IO.System.Network, VectorKind.Centerline, WriteCenterlineSHP, WriteDBF, onReportMethod);
                    filesCount += 5;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                IO.IO.DisposeAll();
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// The job to aggregate network's field info.
        /// （聚合網路欄位資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct AggregateFieldInfosJob : IJob
        {
            [ReadOnly]
            public NativeList<NetworkStat> networkStats;

            [WriteOnly]
            public NativeHashMap<EnumWrapper<Property>, FieldInfo> propertyFieldInfoMap;

            public void Execute()
            {
                FieldInfo capacityField = new(0f);
                FieldInfo dischargeField = new(0f);
                FieldInfo elevationField = new(0f);
                FieldInfo laneField = new(0);
                FieldInfo lengthField = new(0f);
                FieldInfo limitField = new(0f);
                FieldInfo loadField = new(0f);
                FieldInfo volumeField = new(0f);
                FieldInfo widthField = new(0f);
                
                for (int i = 0; i < networkStats.Length; i++)
                {
                    NetworkStat networkStat = networkStats[i];
                    capacityField  += new FieldInfo(networkStat.capacity);
                    dischargeField += new FieldInfo(networkStat.discharge);
                    elevationField += new FieldInfo(networkStat.elevation);
                    laneField      += new FieldInfo(networkStat.lane);
                    lengthField    += new FieldInfo(networkStat.length);
                    limitField     += new FieldInfo(networkStat.limit);
                    loadField      += new FieldInfo(networkStat.load);
                    volumeField    += new FieldInfo(networkStat.volume);
                    widthField     += new FieldInfo(networkStat.width);
                }

                propertyFieldInfoMap.Add(new(Property.Capacity),  capacityField);
                propertyFieldInfoMap.Add(new(Property.Discharge), dischargeField);
                propertyFieldInfoMap.Add(new(Property.Elevation), elevationField);
                propertyFieldInfoMap.Add(new(Property.Lane),      laneField);
                propertyFieldInfoMap.Add(new(Property.Length),    lengthField);
                propertyFieldInfoMap.Add(new(Property.Limit),     limitField);
                propertyFieldInfoMap.Add(new(Property.Load),      loadField);
                propertyFieldInfoMap.Add(new(Property.Volume),    volumeField);
                propertyFieldInfoMap.Add(new(Property.Width),     widthField);
            }
        }

        /// <summary>
        /// The job to collect network's boundaries.
        /// （收集網路邊界的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectBoundariesJob : IJobParallelFor
        {
            [ReadOnly]
            public BufferLookup<ConnectedEdge> connectedEdgeBufferLookup;

            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<EdgeGeometry> edgeGeometryLookup;

            [ReadOnly]
            public ComponentLookup<EndNodeGeometry> endNodeGeometryLookup;

            [ReadOnly]
            public ComponentLookup<LocalConnect> localConnectLookup;

            [ReadOnly]
            public ComponentLookup<Node> nodeLookup;

            [ReadOnly]
            public ComponentLookup<RoadComposition> roadCompositionLookup;

            [ReadOnly]
            public ComponentLookup<StartNodeGeometry> startNodeGeometryLookup;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public NativeList<NetworkStat> networkStats;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> nodeCountEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> statsEntityMap;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeList<double3>>.ParallelWriter nodeEntityMap;

            public void Execute(int index)
            {
                NetworkStat networkStat = networkStats[index];
                Entity network = networkStat.entity;
                if (!nodeCountEntityMap.TryGetValue(network, out int count) ||
                     count <= 0) return;

                bool endNodeIsRoundabout = networkStat.roundabout.y;
                bool endNodeIsTerminus;
                bool startNodeIsRoundabout = networkStat.roundabout.x;
                bool startNodeIsTerminus;
                Entity endNodeEntity = networkStat.end;
                Entity startNodeEntity = networkStat.start;
                NativeList<double3> nodes = new(count, Allocator.Persistent);

                if (connectedEdgeBufferLookup.TryGetBuffer(endNodeEntity, out DynamicBuffer<ConnectedEdge> endConnectedEdges) &&
                    connectedEdgeBufferLookup.TryGetBuffer(startNodeEntity, out DynamicBuffer<ConnectedEdge> startConnectedEdges))
                {
                    endNodeIsTerminus = IsTerminusNode(networkStat, endNodeEntity, endConnectedEdges, ref statsEntityMap, ref networkStats, ref compositionLookup, ref roadCompositionLookup, localConnectLookup.HasComponent(endNodeEntity), endNodeIsRoundabout);
                    startNodeIsTerminus = IsTerminusNode(networkStat, startNodeEntity, startConnectedEdges, ref statsEntityMap, ref networkStats, ref compositionLookup, ref roadCompositionLookup, localConnectLookup.HasComponent(startNodeEntity), startNodeIsRoundabout);
                }
                else
                {
                    return;
                }

                if (!edgeGeometryLookup.TryGetComponent(network, out EdgeGeometry edgeGeometry) ||
                    !endNodeGeometryLookup.TryGetComponent(network, out EndNodeGeometry endNodeGeometry) ||
                    !nodeLookup.TryGetComponent(endNodeEntity, out Node endNode) ||
                    !nodeLookup.TryGetComponent(startNodeEntity, out Node startNode) ||
                    !startNodeGeometryLookup.TryGetComponent(network, out StartNodeGeometry startNodeGeometry)) return;

                // The road segment can be obtained by sketching along the curves in the order of:
                // （道路區段的邊界可以沿著以下順序描繪獲得：）
                //   mainCurveLeft → endCurveLeft → (inverted) endCurveRight → (inverted) mainCurveRight → (inverted) startCurveRight → startCurveLeft → (loop complete)
                Bezier4x3 mainCurveLeftEnd = edgeGeometry.m_End.m_Left;
                Bezier4x3 mainCurveLeftStart = edgeGeometry.m_Start.m_Left;
                Bezier4x3 mainCurveRightEnd = Colossal.Mathematics.MathUtils.Invert(edgeGeometry.m_End.m_Right);
                Bezier4x3 mainCurveRightStart = Colossal.Mathematics.MathUtils.Invert(edgeGeometry.m_Start.m_Right);

                float scaler = networkStat.width > 8f ? 1f : networkStat.width / 8f;
                float adjustedNodeGap = NodeInterpolationGap * scaler;
                float adjustedNodeThreshold = NodeInterpolationThreshold * scaler;
                float adjustedRoundaboutGap = RoundaboutInterpolationGap * scaler;
                float adjustedRoundaboutThreshold = RoundaboutInterpolationThreshold * scaler;
                float adjustedSegmentGap = SegmentInterpolationGap * scaler;
                float adjustedSegmentThreshold = SegmentInterpolationThreshold * scaler;

                if (endNodeIsRoundabout)
                {
                    if (!roundaboutEntityMap.TryGetValue(endNodeEntity, out Domain.Roundabout endRoundabout)) return;
                    
                    // Version 0.3.6: To solve the defect of the roundabout algorithm introduced back in version 0.1, the code now correctly uses the relevant node geometry's Bezier curve.
                    // （0.3.6 版本：為了解決 0.1 版本圓環演算法的缺陷，程式現在正確地使用相關的節點幾何貝茲曲線。）

                    Bezier4x3 endLeftFrontCurve = endNodeGeometry.m_Geometry.m_Left.m_Left;                                             // The front part of the left node boundary when facing the roundabout.（面對圓環時，節點左側靠前的邊界。）
                    Bezier4x3 endLeftRearCurve = endNodeGeometry.m_Geometry.m_Right.m_Left;                                             // The rear part of the left node boundary when facing the roundabout.（面對圓環時，節點左側靠後的邊界。）
                    Bezier4x3 endRightFrontCurve = Colossal.Mathematics.MathUtils.Invert(endNodeGeometry.m_Geometry.m_Left.m_Right);    // The front part of the right node boundary when facing the roundabout.（面對圓環時，節點右側靠前的邊界。）
                    Bezier4x3 endRightRearCurve = Colossal.Mathematics.MathUtils.Invert(endNodeGeometry.m_Geometry.m_Right.m_Right);    // The rear part of the right node boundary when facing the roundabout.（面對圓環時，節點右側靠後的邊界。）

                    bool isLeftContinuous = mainCurveLeftEnd.d.Equals(endLeftFrontCurve.a);
                    bool isRightContinuous = mainCurveRightEnd.a.Equals(endRightFrontCurve.d);
                    double leftAzimuth = Utils.MathUtils.Azimuth(endRoundabout.position, endLeftRearCurve.d);
                    double rightAzimuth = Utils.MathUtils.Azimuth(endRoundabout.position, endRightRearCurve.a);

                    Utils.MathUtils.Interpolate(mainCurveLeftEnd, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, !isLeftContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(endLeftFrontCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(endLeftRearCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, !endNodeIsTerminus, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);

                    if (!endNodeIsTerminus)
                    {
                        Utils.MathUtils.Interpolate(endRoundabout, ref nodes, adjustedRoundaboutGap, leftAzimuth, rightAzimuth,
                                                    center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    }

                    Utils.MathUtils.Interpolate(endRightRearCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(endRightFrontCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, !isRightContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(mainCurveRightEnd, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                }
                else
                {
                    Bezier4x3 endCurveLeft = endNodeGeometry.m_Geometry.m_Left.m_Left;
                    Bezier4x3 endCurveRight = Colossal.Mathematics.MathUtils.Invert(endNodeGeometry.m_Geometry.m_Right.m_Right);

                    // Version 0.3.3: To solve the problem of the parking lot roads and the covered pedestrian bridge missing part of their segmentC and segmentG,
                    //                the last vertices of these segments are tracked. If the position of the first vertex from the next segment is different, the last vertex would be re-added.
                    // （0.3.3 版本：為了解決停車巷以及有頂人行天橋遺失一部分 segmentC 及 segmentG 的問題，這兩個路段的最後一個頂點將會被追蹤。
                    //               若下個路段的首個頂點與該頂點位置不同，該頂點將被重新加入。）

                    bool isLeftContinuous = mainCurveLeftEnd.d.Equals(endCurveLeft.a);
                    bool isRightContinuous = mainCurveRightEnd.a.Equals(endCurveRight.d);
                    double3 endNodePosition = Geodata.Transform.Apply(center.Shift(endNode.m_Position.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();

                    Utils.MathUtils.Interpolate(mainCurveLeftEnd, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, !isLeftContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(endCurveLeft, ref nodes, adjustedNodeThreshold, adjustedNodeGap, true, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    if (!endNodeIsTerminus) nodes.Add(endNodePosition);
                    Utils.MathUtils.Interpolate(endCurveRight, ref nodes, adjustedNodeThreshold, adjustedNodeGap, !isRightContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(mainCurveRightEnd, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                }

                if (startNodeIsRoundabout)
                {
                    if (!roundaboutEntityMap.TryGetValue(startNodeEntity, out Domain.Roundabout startRoundabout)) return;

                    // Version 0.3.6: To solve the defect of the roundabout algorithm introduced back in version 0.1, the code now correctly uses the relevant node geometry's Bezier curve.
                    // （0.3.6 版本：為了解決 0.1 版本圓環演算法的缺陷，程式現在正確地使用相關的節點幾何貝茲曲線。）

                    Bezier4x3 startLeftFrontCurve = Colossal.Mathematics.MathUtils.Invert(startNodeGeometry.m_Geometry.m_Left.m_Right); // The front part of the left node boundary when facing AWAY from the roundabout.（背對圓環時，節點左側靠前的邊界。）
                    Bezier4x3 startLeftRearCurve = Colossal.Mathematics.MathUtils.Invert(startNodeGeometry.m_Geometry.m_Right.m_Right); // The rear part of the left node boundary when facing AWAY from the roundabout.（背對圓環時，節點左側靠後的邊界。）
                    Bezier4x3 startRightFrontCurve = startNodeGeometry.m_Geometry.m_Left.m_Left;                                        // The front part of the right node boundary when facing AWAY from the roundabout.（背對圓環時，節點右側靠前的邊界。）
                    Bezier4x3 startRightRearCurve = startNodeGeometry.m_Geometry.m_Right.m_Left;                                        // The rear part of the right node boundary when facing AWAY from the roundabout.（背對圓環時，節點右側靠後的邊界。）

                    bool isLeftContinuous = mainCurveLeftStart.a.Equals(startLeftFrontCurve.d);
                    bool isRightContinuous = mainCurveRightStart.d.Equals(startRightFrontCurve.a);
                    double leftAzimuth = Utils.MathUtils.Azimuth(startRoundabout.position, startLeftRearCurve.a);
                    double rightAzimuth = Utils.MathUtils.Azimuth(startRoundabout.position, startRightRearCurve.d);

                    Utils.MathUtils.Interpolate(mainCurveRightStart, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, !isRightContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(startRightFrontCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(startRightRearCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, !startNodeIsTerminus, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);

                    if (!startNodeIsTerminus)
                    {
                        Utils.MathUtils.Interpolate(startRoundabout, ref nodes, adjustedRoundaboutGap, rightAzimuth, leftAzimuth,
                                                    center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    }

                    Utils.MathUtils.Interpolate(startLeftRearCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(startLeftFrontCurve, ref nodes, adjustedRoundaboutThreshold, adjustedRoundaboutGap, !isLeftContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(mainCurveLeftStart, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                }
                else
                {
                    Bezier4x3 startCurveLeft = Colossal.Mathematics.MathUtils.Invert(startNodeGeometry.m_Geometry.m_Right.m_Right); 
                    Bezier4x3 startCurveRight = startNodeGeometry.m_Geometry.m_Left.m_Left;

                    // Version 0.3.3: To solve the problem of the parking lot roads and the covered pedestrian bridge missing part of their segmentC and segmentG,
                    //                the last vertices of these segments are tracked. If the position of the first vertex from the next segment is different, the last vertex would be re-added.
                    // （0.3.3 版本：為了解決停車巷以及有頂人行天橋遺失一部分 segmentC 及 segmentG 的問題，這兩個路段的最後一個頂點將會被追蹤。
                    //               若下個路段的首個頂點與該頂點位置不同，該頂點將被重新加入。）

                    bool isLeftContinuous = mainCurveLeftStart.a.Equals(startCurveLeft.d);
                    bool isRightContinuous = mainCurveRightStart.d.Equals(startCurveRight.a);
                    double3 startNodePosition = Geodata.Transform.Apply(center.Shift(startNode.m_Position.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();

                    Utils.MathUtils.Interpolate(mainCurveRightStart, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, !isRightContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(startCurveRight, ref nodes, adjustedNodeThreshold, adjustedNodeGap, true, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    if (!startNodeIsTerminus) nodes.Add(startNodePosition);
                    Utils.MathUtils.Interpolate(startCurveLeft, ref nodes, adjustedNodeThreshold, adjustedNodeGap, !isLeftContinuous, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                    Utils.MathUtils.Interpolate(mainCurveLeftStart, ref nodes, adjustedSegmentThreshold, adjustedSegmentGap, false, out _,
                                                center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                }

                nodeEntityMap.TryAdd(network, nodes);
            }

            /// <summary>
            /// Check whether the node is a terminus node (a node has only one connected network.)
            /// （確認節點是否為「死路」節點（只有一個網路連接的節點）。）
            /// </summary>
            /// <param name="stat">The network statistic data.（網路統計資料。）</param>
            /// <param name="node">The target node.（目標節點。）</param>
            /// <param name="edges">The buffer of connected edges at the node.（與節點相連的路段緩衝區。）</param>
            /// <param name="statsIndexMap">The map between the network entity and its index in <see cref="_localNetworkStats"/>.（網路實體與其在 <see cref="_localNetworkStats"/> 索引值的映射表。）></param>
            /// <param name="networkStats">See <see cref="_localNetworkStats"/>.（參見 <see cref="_localNetworkStats"/>。）</param>
            /// <param name="hasLocalConnect">Whether the node has <see cref="LocalConnect"/> component.（節點是否有 <see cref="LocalConnect"/> 組件？）</param>
            /// <param name="isRoundabout">Whether the node is a roundabout.（節點是否為圓環？）</param>
            /// <returns>If true, the node is a terminus node.（若為真，節點為「死路」節點。）</returns>
            private static bool IsTerminusNode(NetworkStat stat, Entity node, DynamicBuffer<ConnectedEdge> edges, ref NativeParallelHashMap<Entity, int> statsIndexMap, ref NativeList<NetworkStat> networkStats, ref ComponentLookup<Composition> compositionLookup, ref ComponentLookup<RoadComposition> roadCompositionLookup, bool hasLocalConnect, bool isRoundabout)
            {
                bool isTerminus = false;
                bool hasRoadComposition = compositionLookup.TryGetComponent(stat.entity, out Composition networkComposition) && roadCompositionLookup.HasComponent(networkComposition.m_Edge);
                int pathwayCount = 0;
                int pathRoadCount = 0;
                int roadCount = 0;
                int taxiwayCount = 0;
                int trackCount = 0;
                int waterwayCount = 0;
                NetworkCategory pathwayCategory = NetworkCategory.Bicycle | NetworkCategory.Pathway;
                NetworkCategory roadCategory = NetworkCategory.Car | NetworkCategory.Highway;
                NetworkCategory taxiwayCategory = NetworkCategory.Runway | NetworkCategory.Taxiway;
                NetworkCategory trackCategory = NetworkCategory.Train | NetworkCategory.Subway | NetworkCategory.Tram;

                for (int i = 0; i < edges.Length; i++)
                {
                    Entity edge = edges[i].m_Edge;
                    if (statsIndexMap.TryGetValue(edge, out int listIndex) && (listIndex >= 0) && (listIndex < networkStats.Length))
                    {
                        NetworkStat edgeStat = networkStats[listIndex];
                        if (!edgeStat.HasNode(node)) continue;              // Handle the situation of pathway local connection.（處理路徑的當地連結情形。）

                        NetworkCategory edgeCategory = edgeStat.category;
                        if ((edgeCategory & pathwayCategory) != 0)
                        {
                            if (compositionLookup.TryGetComponent(edge, out Composition edgeComposition) && roadCompositionLookup.HasComponent(edgeComposition.m_Edge))
                            {
                                pathRoadCount++;
                            }
                            else
                            {
                                pathwayCount++;
                            }  
                        }
                        if ((edgeCategory & roadCategory) != 0)
                        {
                            roadCount++;
                        }
                        else if ((edgeCategory & taxiwayCategory) != 0)
                        {
                            taxiwayCount++;
                        }
                        else if ((edgeCategory & trackCategory) != 0)
                        {
                            trackCount++;
                        }
                        else if ((edgeCategory & NetworkCategory.Waterway) != 0)
                        {
                            waterwayCount++;
                        }
                    }
                }

                if (((stat.category & pathwayCategory) != 0) && !hasRoadComposition)
                {
                    // Version 1.0.7: To solve the roads created by Road Builder being recognized as pathways, Carto now checks whether the network is a road.
                    // （1.0.7 版本：為了解決 Road Builder 創造的網路被視為人行道的問題，Carto 現在會檢查網路是否為一條路。）

                    if ((pathwayCount <= 2) || !hasLocalConnect) isTerminus = true;
                    if ((stat.category & roadCategory) != 0) isTerminus = false;                    // Force the road with bicycle lanes to the next check.（強迫含有自行車專用道的道路進入下一個檢查。）
                }

                if (((stat.category & roadCategory) != 0) || hasRoadComposition)
                {
                    if ((roadCount + pathRoadCount <= 2) && (trackCount == 0)) isTerminus = true;   // Handle the situation that a node has only two connected segments.（處理只有兩個連接路段的節點。）
                    if (isTerminus && isRoundabout && (roadCount > 1)) isTerminus = false;          // Force to include the inner ring for roundabouts with two connected segments.（強迫納入只有兩個連接路段圓環的內環。）
                }

                if ((stat.category & taxiwayCategory) != 0)
                {
                    if (taxiwayCount <= 2) isTerminus = true;
                }

                if ((stat.category & trackCategory) != 0)
                {
                    if ((trackCount <= 2) && (roadCount == 0)) isTerminus = true;           // Handle the situation that a node has only two connected segments.（處理只有兩個連接路段的節點。）
                }

                if ((stat.category & NetworkCategory.Waterway) != 0)
                {
                    if (waterwayCount <= 2) isTerminus = true;
                }

                return isTerminus;
            }
        }

        /// <summary>
        /// The job to collect network's centerlines.
        /// （收集網路中心線的工作。）
        /// </summary>
        public partial struct CollectCenterlinesJob : IJobParallelFor
        {            
            /*
             * This job can't be a Burst-compile job, since it has to dynamically create and dispose native containers through managed method.
             * （這個工作無法使用 Burst 編譯，因為它需要使用受控記憶體方法去動態創造與拋棄原生容器。）
             */
            
            [ReadOnly]
            public BufferLookup<ConnectedEdge> connectedEdgeBufferLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<Edge> edgeLookup;

            [ReadOnly]
            public ComponentLookup<EndNodeGeometry> endNodeGeometryLookup;

            [ReadOnly]
            public ComponentLookup<Node> nodeLookup;

            [ReadOnly]
            public ComponentLookup<StartNodeGeometry> startNodeGeometryLookup;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public NativeList<NetworkStat> networkStats;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> nodeCountEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> statsIndexMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeList<double3>>.ParallelWriter nodeEntityMap;

            public void Execute(int index)
            {
                NetworkStat networkStat = networkStats[index];
                Entity network = networkStat.entity;
                if (!nodeCountEntityMap.TryGetValue(network, out int count) ||
                     count <= 0) return;

                NativeList<double3> nodes = new(count, Allocator.Persistent);

                if (networkStat.isRoundabout)
                {
                    if (roundaboutEntityMap.TryGetValue(network, out Domain.Roundabout roundabout) &&
                        connectedEdgeBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedEdge> connectedEdges) &&
                        nodeLookup.TryGetComponent(network, out Node nodeComponent))
                    {
                        NativeHashMap<double, double3> azimuthMap = new(count, Allocator.Persistent);
                        Entity node = networkStat.entity;
                        float3 nodePosition = nodeComponent.m_Position;
                        double radius = (roundabout.innerRingRadius + roundabout.outerRingRadius) / 2d;
                        int nodeCount = Utils.MathUtils.CountInterpolationPoints((float)radius, 0.5f);
                        double angle = math.PI2_DBL / nodeCount;
                        double3 startPoint = Geodata.Transform.Apply(center.Shift(nodePosition.xzy).Shift(0d, radius, 0d), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();

                        for (int i = 0; i < nodeCount; i++)
                        {
                            double rotation = (i + 1) * angle;
                            double deltaX = math.sin(rotation) * radius;
                            double deltaY = math.cos(rotation) * radius;
                            azimuthMap.Add(rotation,
                                           Geodata.Transform.Apply(center.Shift(nodePosition.xzy).Shift(deltaX, deltaY, 0d), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3());
                        }

                        for (int i = 0; i < connectedEdges.Length; i++)
                        {
                            Entity connectedEdge = connectedEdges[i].m_Edge;
                            if (!nodeCountEntityMap.TryGetValue(connectedEdge, out int edgeNodeCount) ||
                                !statsIndexMap.TryGetValue(connectedEdge, out int listIndex) ||
                                (listIndex >= networkStats.Length) ||
                                (listIndex < 0)) continue;

                            NetworkStat edgeStat = networkStats[listIndex];
                            bool roundaboutIsEndNode = node.Equals(edgeStat.end);
                            bool roundaboutIsStartNode = node.Equals(edgeStat.start);
                            if (!roundaboutIsEndNode && !roundaboutIsStartNode) continue;

                            NativeList<double3> edgeNodes = new(edgeNodeCount, Allocator.Persistent);

                            ConstructCenterlineNodes(edgeStat, ref edgeNodes, ref roundaboutEntityMap,
                                                     ref curveLookup, ref edgeLookup, ref endNodeGeometryLookup,
                                                     ref nodeLookup, ref startNodeGeometryLookup);

                            double3 connectionPoint = default;
                            if (roundaboutIsEndNode) connectionPoint = edgeNodes[^1];
                            if (roundaboutIsStartNode) connectionPoint = edgeNodes[0];

                            double azimuth = Utils.MathUtils.Azimuth(nodePosition.xz, connectionPoint.xy);
                            azimuthMap.Add(azimuth,
                                           Geodata.Transform.Apply(center.Shift(connectionPoint.x, connectionPoint.y, connectionPoint.z), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3());

                            Utils.CommonUtils.Dispose(ref edgeNodes);
                        }

                        NativeArray<double> azimuthsArray = azimuthMap.GetKeyArray(Allocator.Persistent);
                        Utils.CommonUtils.Sort(ref azimuthsArray);
                        nodes.Capacity = azimuthsArray.Length + 2;
                        
                        nodes.Add(startPoint);

                        for (int i = azimuthsArray.Length - 1; i >= 0; i--)
                        {
                            if (azimuthMap.TryGetValue(azimuthsArray[i], out double3 coordinate)) nodes.Add(coordinate);
                        }

                        nodes.Add(startPoint);

                        Utils.CommonUtils.Dispose(ref azimuthsArray);
                        Utils.CommonUtils.Dispose(ref azimuthMap);
                    }
                }
                else
                {
                    ConstructCenterlineNodes(networkStat, ref nodes, ref roundaboutEntityMap,
                                             ref curveLookup, ref edgeLookup, ref endNodeGeometryLookup,
                                             ref nodeLookup, ref startNodeGeometryLookup);

                    for (int i = 0; i < nodes.Length; i++)
                    {
                        double3 node = nodes[i];
                        nodes[i] = Geodata.Transform.Apply(center.Shift(node.x, node.y, node.z), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3();
                    }
                }

                nodeEntityMap.TryAdd(network, nodes);
            }
        }

        /// <summary>
        /// The job to collect network lane's information.
        /// （收集車道資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectLanesJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<CarLaneData> carLaneDataLookup;

            [ReadOnly]
            public ComponentLookup<ParkingLaneData> parkingLaneDataLookup;

            [ReadOnly]
            public ComponentLookup<TrackLaneData> trackLaneDataLookup;

            [ReadOnly]
            public ComponentLookup<UtilityLaneData> utilityLaneDataLookup;

            [WriteOnly]
            public NativeParallelHashMap<Entity, Domain.Lane>.ParallelWriter entityMap;

            public void Execute(in NetLaneData netLaneData, Entity lane)
            {
                Domain.Lane laneStruct = new()
                {
                    entity = lane,
                    category = NetworkCategory.None,
                    direction = Direction.None
                };

                if (!parkingLaneDataLookup.TryGetComponent(lane, out ParkingLaneData _))
                {
                    LaneFlags laneGeneralFlag = netLaneData.m_Flags;

                    if ((laneGeneralFlag & LaneFlags.OnWater) != 0)
                    {
                        laneStruct.category |= NetworkCategory.Waterway;
                    }

                    if ((laneGeneralFlag & LaneFlags.PublicOnly) != 0)
                    {
                        laneStruct.category |= NetworkCategory.Bus;
                    }

                    if (carLaneDataLookup.TryGetComponent(lane, out CarLaneData carLaneData))
                    {
                        RoadTypes roadTypes = carLaneData.m_RoadTypes;

                        if (roadTypes == RoadTypes.Bicycle) laneStruct.category |= NetworkCategory.Bicycle;
                        if ((roadTypes & RoadTypes.Car) != 0) laneStruct.category |= NetworkCategory.Car;
                        if ((roadTypes & RoadTypes.Watercraft) != 0) laneStruct.category |= NetworkCategory.Waterway;
                    }

                    if (trackLaneDataLookup.TryGetComponent(lane, out TrackLaneData trackLaneData))
                    {
                        TrackTypes trackTypes = trackLaneData.m_TrackTypes;

                        if ((trackTypes & TrackTypes.Train) != 0) laneStruct.category |= NetworkCategory.Train;
                        if ((trackTypes & TrackTypes.Tram) != 0) laneStruct.category |= NetworkCategory.Tram;
                        if ((trackTypes & TrackTypes.Subway) != 0) laneStruct.category |= NetworkCategory.Subway;
                    }

                    if (utilityLaneDataLookup.TryGetComponent(lane, out UtilityLaneData utilityLaneData))
                    {
                        UtilityTypes utilityTypes = utilityLaneData.m_UtilityTypes;

                        if ((utilityTypes != UtilityTypes.None) && ((utilityTypes & UtilityTypes.Catenary) == 0))
                        {
                            if ((utilityTypes & UtilityTypes.Fence) != 0) laneStruct.category |= NetworkCategory.Fence;
                            if ((utilityTypes & UtilityTypes.HighVoltageLine) != 0) laneStruct.category |= NetworkCategory.HighCable;
                            if ((utilityTypes & UtilityTypes.LowVoltageLine) != 0) laneStruct.category |= NetworkCategory.LowCable;
                            if ((utilityTypes & UtilityTypes.SewagePipe) != 0) laneStruct.category |= NetworkCategory.SewagePipe;
                            if ((utilityTypes & UtilityTypes.StormwaterPipe) != 0) laneStruct.category |= NetworkCategory.StormPipe;
                            if ((utilityTypes & UtilityTypes.WaterPipe) != 0) laneStruct.category |= NetworkCategory.WaterPipe;
                        }
                    }

                    if ((laneGeneralFlag & LaneFlags.Twoway) != 0)
                    {
                        laneStruct.direction = Direction.Both;
                    }
                    else
                    {
                        laneStruct.direction = Direction.Forward;
                    }
                }

                entityMap.TryAdd(lane, laneStruct);
            }
        }

        /// <summary>
        /// The job to collect network statistics.
        /// （收集網路統計資料的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectNetworkStatsJob : IJobEntity
        {
            [ReadOnly]
            public BufferLookup<NetCompositionLane> netCompositionLaneBufferLookup;

            [ReadOnly]
            public ComponentLookup<Aggregated> aggregatedLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityNodeConnection> electricityNodeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<Game.Net.Marker> markerLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<PathwayComposition> pathwayCompositionLookup;

            [ReadOnly]
            public ComponentLookup<PipelineData> pipelineDataLookup;

            [ReadOnly]
            public ComponentLookup<PowerLineData> powerLineDataLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Road> roadLookup;

            [ReadOnly]
            public ComponentLookup<RoadComposition> roadCompositionLookup;

            [ReadOnly]
            public ComponentLookup<TaxiwayComposition> taxiwayCompositionLookup;

            [ReadOnly]
            public ComponentLookup<TrackComposition> trackCompositionLookup;

            [ReadOnly]
            public ComponentLookup<UIObjectData> uiObjectDataLookup;

            [ReadOnly]
            public ComponentLookup<WaterPipeNodeConnection> waterPipeNodeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<WaterwayComposition> waterwayCompositionLookup;

            [ReadOnly]
            public Feature feature;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Domain.Lane> laneEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, float> rsaNetworkSpeedMap;

            [ReadOnly]
            public NativeParallelHashSet<Entity> rbNetworks;

            [WriteOnly]
            public NativeList<NetworkStat>.ParallelWriter list;

            public void Execute(in Composition composition, in Curve curve, in Edge edge, in EdgeGeometry edgeGeometry, in PrefabRef prefabRef, Entity network, in DynamicBuffer<Game.Net.SubLane> subLanes)
            {
                NetworkStat stat = new()
                {
                    entity = network,
                    aggregation = Entity.Null,
                    bike = 0f,
                    capacity = 0f,
                    category = NetworkCategory.None,
                    direction = Direction.None,
                    discharge = 0f,
                    elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                    end = edge.m_End,
                    form = Form.Normal,
                    lane = 0,
                    length = curve.m_Length,
                    limit = 0f,
                    load = 0f,
                    prefab = prefabRef.m_Prefab,
                    range = new(0f, 1f),
                    start = edge.m_Start,
                    uiGroup = uiObjectDataLookup.TryGetComponent(prefabRef.m_Prefab, out UIObjectData uiObjectData) ? uiObjectData.m_Group : Entity.Null,
                    volume = 0f,
                    width = 0f
                };
                stat.InitiateRoundaboutProperties(ref roundaboutEntityMap);

                bool isPipeline = pipelineDataLookup.TryGetComponent(prefabRef, out _) && waterPipeNodeConnectionLookup.TryGetComponent(network, out _);
                bool isPowerLine = powerLineDataLookup.TryGetComponent(prefabRef, out _) && electricityNodeConnectionLookup.TryGetComponent(network, out _);
                bool isNotUtilityServiceNetwork = !(isPipeline || isPowerLine);
                bool overrideForm = false;
                Entity networkComposition = composition.m_Edge;
                bool isTaxiway = taxiwayCompositionLookup.TryGetComponent(networkComposition, out TaxiwayComposition taxiwayComposition);

                if (markerLookup.TryGetComponent(network, out _) && !isTaxiway) return;

                if (aggregatedLookup.TryGetComponent(network, out Aggregated aggregatedComponent))
                {
                    stat.aggregation = aggregatedComponent.m_Aggregate;
                }
                else
                {
                    stat.aggregation = network;
                }

                if (isNotUtilityServiceNetwork)
                {
                    if (networkComposition != Entity.Null)
                    {
                        if (netCompositionLaneBufferLookup.TryGetBuffer(networkComposition, out DynamicBuffer<NetCompositionLane> lanes))
                        {
                            GetLaneStatistics(lanes, ref laneEntityMap, out int count, out NetworkCategory categories);
                            stat.lane = count;
                            stat.category = categories;
                        }

                        // If there are multiple compositions present, the priority of speed limit value would be pathway < waterway < taxiway < track < road.
                        //（若出現多種配置，優先順序為路徑 < 航路 < 滑行道 < 軌道 < 道路。）

                        if (pathwayCompositionLookup.TryGetComponent(networkComposition, out PathwayComposition pathwayComposition))
                        {
                            stat.category |= NetworkCategory.Pathway;
                            stat.direction = Direction.Both;
                            stat.limit = RoundSpeedLimit(pathwayComposition.m_SpeedLimit);
                        }

                        if (waterwayCompositionLookup.TryGetComponent(networkComposition, out WaterwayComposition waterwayComposition))
                        {
                            stat.elevation = edgeGeometry.m_Bounds.min.y;
                            stat.form = Form.Normal;    // Force the form to be normal; the vanilla setting is `Elevated`.（強制將形式變為一般；原版設定是 `Elevated`。）
                            stat.limit = RoundSpeedLimit(waterwayComposition.m_SpeedLimit);
                            overrideForm = true;
                        }

                        if (isTaxiway)
                        {
                            Direction direction = Direction.None;
                            TaxiwayFlags taxiwayFlags = taxiwayComposition.m_Flags;
                            if (taxiwayFlags == TaxiwayFlags.Airspace) return;  // Exporting air space is not supported.（不支援輸出空域。）
                            bool hasRunwayFlag = (taxiwayFlags & TaxiwayFlags.Runway) != 0;
                            for (int i = 0; i < subLanes.Length; i++)
                            {
                                Entity subLane = subLanes[i].m_SubLane;
                                if (prefabRefLookup.TryGetComponent(subLane, out PrefabRef subLanePrefabRef) &&
                                    laneEntityMap.TryGetValue(subLanePrefabRef.m_Prefab, out Domain.Lane lanePrefab))
                                {
                                    direction |= lanePrefab.direction;
                                }
                            }
                            stat.category = hasRunwayFlag ? NetworkCategory.Runway : NetworkCategory.Taxiway;
                            stat.direction = direction;
                            stat.elevation = edgeGeometry.m_Bounds.min.y;
                            stat.limit = RoundSpeedLimit(taxiwayComposition.m_SpeedLimit);
                        }

                        if (trackCompositionLookup.TryGetComponent(networkComposition, out TrackComposition trackComposition))
                        {
                            stat.limit = RoundSpeedLimit(trackComposition.m_SpeedLimit);
                        }

                        if (roadCompositionLookup.TryGetComponent(networkComposition, out RoadComposition roadComposition))
                        {
                            bool hasHighwayRule = (roadComposition.m_Flags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0;
                            if (hasHighwayRule)
                            {
                                stat.category &= ~NetworkCategory.Car;
                                stat.category |= NetworkCategory.Highway;
                            }
                            stat.limit = RoundSpeedLimit(roadComposition.m_SpeedLimit);
                        }

                        if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositionData))
                        {
                            // The network direction.（網路方向。）
                            CompositionState state = netCompositionData.m_State;
                            if ((state & CompositionState.HasForwardRoadLanes) != 0) stat.direction |= Direction.Forward;
                            if ((state & CompositionState.HasForwardTrackLanes) != 0) stat.direction |= Direction.Forward;
                            if ((state & CompositionState.HasBackwardRoadLanes) != 0) stat.direction |= Direction.Backward;
                            if ((state & CompositionState.HasBackwardTrackLanes) != 0) stat.direction |= Direction.Backward;

                            // The network form.（網路形式。）
                            if (!overrideForm) stat.form = GetForm(netCompositionData);

                            // The network category.（網路分類。）
                            if ((state & CompositionState.HasPedestrianLanes) == 0) stat.category &= ~NetworkCategory.Pathway;

                            stat.width = netCompositionData.m_Width;
                        }
                    }

                    // Calculate the traffic volume.（計算交通流量。）
                    if (roadLookup.TryGetComponent(network, out Road roadComponent))
                    {
                        stat.volume = GetVolume(roadComponent);
                    }

                    if ((stat.category & NetworkCategory.Bicycle) != 0)
                    {
                        //stat.bike = ;
                    }

                    if (rbNetworks.Contains(network))
                    {
                        stat.category |= NetworkCategory.RoadBuilder;
                    }

                    if (rsaNetworkSpeedMap.TryGetValue(network, out float speed))
                    {
                        stat.limit = (float)Math.Round(speed, 2);
                    }

                    if ((stat.Object & feature) != 0) list.AddNoResize(stat);
                    return;
                }
            }
        }

        /// <summary>
        /// The job to collect Road Builder-made networks.
        /// （收集由 Road Builder 製作的網路的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectRbNetworksJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashSet<Entity>.ParallelWriter rbNetworks;

            public void Execute(Entity network)
            {
                rbNetworks.Add(network);
            }
        }

        /// <summary>
        /// The job to collect roundabouts.
        /// （收集圓環的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectRoundaboutsJob : IJobEntity
        {
            [ReadOnly]
            public bool hasCenterline;

            [ReadOnly]
            public bool leftHandTraffic;

            [ReadOnly]
            public BufferLookup<NetCompositionLane> netCompositionLaneBufferLookup;

            [ReadOnly]
            public ComponentLookup<Aggregated> aggregatedLookup;

            [ReadOnly]
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<Edge> edgeLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<Pillar> pillarLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Road> roadLookup;

            [ReadOnly]
            public ComponentLookup<RoadComposition> roadCompositionLookup;

            [ReadOnly]
            public ComponentLookup<SubwayTrack> subwayTrackLookup;

            [ReadOnly]
            public ComponentLookup<TrackComposition> trackCompositionLookup;

            [ReadOnly]
            public ComponentLookup<TrainTrack> trainTrackLookup;

            [ReadOnly]
            public ComponentLookup<TramTrack> tramTrackLookup;

            [ReadOnly]
            public ComponentLookup<UIObjectData> uiObjectDataLookup;

            [ReadOnly]
            public Feature feature;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Entity> attachmentEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, float> attachmentRadiusMap;

            [WriteOnly]
            public NativeList<NetworkStat>.ParallelWriter stats;

            [WriteOnly]
            public NativeParallelHashMap<Entity, Domain.Roundabout>.ParallelWriter roundaboutEntityMap;

            public void Execute(in Node node, in Game.Net.Roundabout roundaboutComponent, in DynamicBuffer<ConnectedEdge> connectedEdges, Entity entity)
            {
                bool allTrackConnection = true;
                bool allTunnelConnection = true;
                bool highwayConnection = false;
                Entity edgeAggregation = Entity.Null;
                Entity edgePrefab = Entity.Null;
                int maxLaneCount = -1;
                float roadLimit = 0f;
                float trackLimit = 0f;
                float width = 0f;
                Form form = Form.Normal;

                Domain.Roundabout roundabout = new()
                {
                    attached = attachmentEntityMap.TryGetValue(entity, out Entity attachment) ? attachment : Entity.Null,
                    innerRingRadius = 0f,
                    node = entity,
                    outerRingRadius = roundaboutComponent.m_Radius,
                    position = node.m_Position,
                    width = 0f
                };

                bool createdByPillar = pillarLookup.HasComponent(roundabout.attached);

                for (int i = 0; i < connectedEdges.Length; i++)
                {
                    Entity connectedNetwork = connectedEdges[i].m_Edge;
                    if (!roadLookup.TryGetComponent(connectedNetwork, out _) &&
                        !tramTrackLookup.TryGetComponent(connectedNetwork, out _) &&
                        !subwayTrackLookup.TryGetComponent(connectedNetwork, out _) &&
                        !trainTrackLookup.TryGetComponent(connectedNetwork, out _))
                    {
                        continue;
                    }

                    if (compositionLookup.TryGetComponent(connectedNetwork, out Composition connectedComposition) &&
                        netCompositionDataLookup.TryGetComponent(connectedComposition.m_Edge, out NetCompositionData connectedCompositionData))
                    {
                        Entity composition = connectedComposition.m_Edge;
                        float compositionWidth = connectedCompositionData.m_Width / 2f;
                        width = math.max(width, compositionWidth);

                        Form connectedForm = GetForm(connectedCompositionData);
                        if (allTunnelConnection && connectedForm != Form.Tunnel)
                        {
                            allTunnelConnection = false;
                            form = connectedForm;
                        }
                        if ((form != Form.Elevated) && connectedForm == Form.Elevated)
                        {
                            form = connectedForm;
                        }

                        if (roadCompositionLookup.TryGetComponent(composition, out RoadComposition roadComposition))
                        {
                            allTrackConnection = false;
                            highwayConnection |= (roadComposition.m_Flags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0;
                            roadLimit = math.max(roadLimit, RoundSpeedLimit(roadComposition.m_SpeedLimit));
                        }

                        if (trackCompositionLookup.TryGetComponent(composition, out TrackComposition trackComposition))
                        {
                            trackLimit = math.max(trackLimit, RoundSpeedLimit(trackComposition.m_SpeedLimit));
                        }

                        if (hasCenterline &&
                            edgeLookup.TryGetComponent(connectedNetwork, out Edge edge) &&
                            netCompositionLaneBufferLookup.TryGetBuffer(composition, out DynamicBuffer<NetCompositionLane> lanes))
                        {
                            bool roundaboutIsStartNode = edge.m_Start.Equals(entity);
                            int laneCount = 0;
                            for (int j = 0; j < lanes.Length; j++)
                            {
                                NetCompositionLane lane = lanes[j];
                                if (((lane.m_Flags & LaneFlags.Road) != 0) & ((lane.m_Flags & LaneFlags.Master) == 0))
                                {
                                    if (roundaboutIsStartNode ^ ((lane.m_Flags & LaneFlags.Invert) == 0)) laneCount++;
                                }
                            }

                            if (laneCount > maxLaneCount)
                            {
                                maxLaneCount = laneCount;
                                edgeAggregation = aggregatedLookup.TryGetComponent(connectedNetwork, out Aggregated aggregatedComponent) ? aggregatedComponent.m_Aggregate : connectedNetwork;
                                edgePrefab = prefabRefLookup.TryGetComponent(connectedNetwork, out PrefabRef networkPrefab) ? networkPrefab.m_Prefab : Entity.Null;
                            }
                        }
                    }
                }

                if (prefabRefLookup.TryGetComponent(roundabout.attached, out PrefabRef attachmentPrefab) && attachmentRadiusMap.TryGetValue(attachmentPrefab, out float attachmentRadius))
                {
                    roundabout.innerRingRadius = attachmentRadius;
                }
                else
                {
                    roundabout.innerRingRadius = roundabout.outerRingRadius - width;
                }

                roundabout.width = width;

                roundaboutEntityMap.TryAdd(entity, roundabout);

                if (!hasCenterline) return;

                // Append roundabout's statistics.（添加圓環的統計資訊。）
                NetworkStat stat = new()
                {
                    entity = entity,
                    aggregation = createdByPillar ? edgeAggregation : roundabout.attached,
                    capacity = 0f,
                    category = highwayConnection ? NetworkCategory.Highway : NetworkCategory.Car,
                    direction = leftHandTraffic ? Direction.Backward : Direction.Forward,
                    discharge = 0f,
                    elevation = node.m_Position.y,
                    end = Entity.Null,
                    form = allTunnelConnection ? Form.Tunnel : form,
                    isRoundabout = true,
                    lane = maxLaneCount,
                    length = 2 * math.PI * ((roundabout.outerRingRadius - roundabout.innerRingRadius) / 2 + roundabout.innerRingRadius),
                    limit = allTrackConnection ? trackLimit : roadLimit,
                    load = 0f,
                    prefab = createdByPillar ? edgePrefab : (prefabRefLookup.TryGetComponent(roundabout.attached, out PrefabRef attachedPrefab) ? attachedPrefab.m_Prefab : Entity.Null),
                    range = new(0f, 1f),
                    roundabout = new(false, false),
                    start = Entity.Null,
                    uiGroup = uiObjectDataLookup.TryGetComponent(edgePrefab, out UIObjectData uiObjectData) ? uiObjectData.m_Group : Entity.Null,
                    volume = roadLookup.TryGetComponent(entity, out Road road) ? math.max(0f, GetVolume(road)) : 0f,
                    width = width
                };

                if ((stat.Object & feature) != 0) stats.AddNoResize(stat);
            }
        }

        /// <summary>
        /// The job to collect the roundabout attachment prefabs.
        /// （收集圓環附件預製模板的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectRoundaboutAttachmentPrefabsJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, float>.ParallelWriter attachementRadiusMap;
            
            public void Execute(in NetObjectData netObjectData, in ObjectGeometryData objectGeometryData, Entity entity)
            {
                if ((netObjectData.m_CompositionFlags.m_General & CompositionFlags.General.Roundabout) == 0) return;

                Game.Objects.GeometryFlags geometryFlags = objectGeometryData.m_Flags;
                bool isCircular = (geometryFlags & Game.Objects.GeometryFlags.Circular) != 0;
                bool isCircularLeg = (geometryFlags & Game.Objects.GeometryFlags.CircularLeg) != 0;
                float radius = 0f;
                float3 legSize = objectGeometryData.m_LegSize;
                float3 size = objectGeometryData.m_Size;

                if (isCircular)
                {
                    radius = math.cmax(size.xz) / 2f;
                }
                if (isCircularLeg)
                {
                    if (isCircular)
                    {
                        radius = math.min(radius, math.cmax(legSize.xz) / 2f);
                    }
                    else
                    {
                        radius = math.cmax(legSize.xz) / 2f;
                    }
                }

                if (radius > 0f) attachementRadiusMap.TryAdd(entity, radius);
            }
        }

        /// <summary>
        /// The job to map roundabout attachements to a roundabout nodes.
        /// （將圓環附件映射至圓環節點的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct MapRoundaboutAttachmentsJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, Entity>.ParallelWriter entityMap;

            public void Execute(in Attached attached, Entity attachment)
            {
                if (attached.m_Parent != Entity.Null) entityMap.TryAdd(attached.m_Parent, attachment);
            }
        }

        /// <summary>
        /// The job to populate the pre-requirements for boundary geometries.
        /// （填入邊界幾何所需的前置資料的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct PopulateBoundaryPreRequirementsJob : IJobParallelFor
        {
            [ReadOnly]
            public ComponentLookup<EdgeGeometry> edgeGeometryLookup;

            [ReadOnly]
            public ComponentLookup<EndNodeGeometry> endNodeGeometryLookup;

            [ReadOnly]
            public ComponentLookup<StartNodeGeometry> startNodeGeometryLookup;

            [ReadOnly]
            public NativeList<NetworkStat> networkStats;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int>.ParallelWriter nodeCountMap;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int>.ParallelWriter statsIndexMap;

            public void Execute(int index)
            {
                NetworkStat networkStat = networkStats[index];
                Entity network = networkStat.entity;
                statsIndexMap.TryAdd(network, index);

                if (!edgeGeometryLookup.TryGetComponent(network, out EdgeGeometry edgeGeometry) ||
                    !endNodeGeometryLookup.TryGetComponent(network, out EndNodeGeometry endNodeGeometry) ||
                    !startNodeGeometryLookup.TryGetComponent(network, out StartNodeGeometry startNodeGeometry)) return;

                bool endNodeIsRoundabout = networkStat.roundabout.y;
                bool startNodeIsRoundabout = networkStat.roundabout.x;
                int count = 0;

                count += Utils.MathUtils.CountInterpolationPoints(edgeGeometry.m_End.m_Left, SegmentInterpolationThreshold, SegmentInterpolationGap);
                count += Utils.MathUtils.CountInterpolationPoints(edgeGeometry.m_Start.m_Left, SegmentInterpolationThreshold, SegmentInterpolationGap);
                count += Utils.MathUtils.CountInterpolationPoints(edgeGeometry.m_End.m_Right, SegmentInterpolationThreshold, SegmentInterpolationGap);
                count += Utils.MathUtils.CountInterpolationPoints(edgeGeometry.m_Start.m_Right, SegmentInterpolationThreshold, SegmentInterpolationGap);

                if (endNodeIsRoundabout)
                {
                    count += Utils.MathUtils.CountInterpolationPoints(endNodeGeometry.m_Geometry.m_Left.m_Left, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(endNodeGeometry.m_Geometry.m_Right.m_Left, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(endNodeGeometry.m_Geometry.m_Left.m_Right, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(endNodeGeometry.m_Geometry.m_Right.m_Right, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                }
                else
                {
                    count += Utils.MathUtils.CountInterpolationPoints(endNodeGeometry.m_Geometry.m_Left.m_Left, NodeInterpolationThreshold, NodeInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(endNodeGeometry.m_Geometry.m_Right.m_Right, NodeInterpolationThreshold, NodeInterpolationGap);
                }

                if (startNodeIsRoundabout)
                {
                    count += Utils.MathUtils.CountInterpolationPoints(startNodeGeometry.m_Geometry.m_Left.m_Right, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(startNodeGeometry.m_Geometry.m_Right.m_Right, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(startNodeGeometry.m_Geometry.m_Left.m_Left, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(startNodeGeometry.m_Geometry.m_Right.m_Left, RoundaboutInterpolationThreshold, RoundaboutInterpolationGap);
                }
                else
                {
                    count += Utils.MathUtils.CountInterpolationPoints(startNodeGeometry.m_Geometry.m_Left.m_Left, NodeInterpolationThreshold, NodeInterpolationGap);
                    count += Utils.MathUtils.CountInterpolationPoints(startNodeGeometry.m_Geometry.m_Right.m_Right, NodeInterpolationThreshold, NodeInterpolationGap);
                }

                nodeCountMap.TryAdd(network, count);
            }
        }

        /// <summary>
        /// The job to populate the pre-requirements for centerline geometries.
        /// （填入中心線幾何所需的前置資料的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct PopulateCenterlinePreRequirementsJob : IJobParallelFor
        {
            [ReadOnly]
            public BufferLookup<ConnectedEdge> connectedEdgeBufferLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<EndNodeGeometry> endNodeGeometryLookup;

            [ReadOnly]
            public ComponentLookup<StartNodeGeometry> startNodeGeometryLookup;

            [ReadOnly]
            public NativeList<NetworkStat> networkStats;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int>.ParallelWriter nodeCountEntityMap;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int>.ParallelWriter statsIndexMap;

            public void Execute(int index)
            {
                NetworkStat networkStat = networkStats[index];
                Entity network = networkStat.entity;
                int count = 0;

                if (networkStat.isRoundabout)
                {
                    if (roundaboutEntityMap.TryGetValue(network, out Domain.Roundabout roundabout) &&
                        connectedEdgeBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedEdge> connectedEdges))
                    {
                        count += Utils.MathUtils.CountInterpolationPoints((roundabout.innerRingRadius + roundabout.outerRingRadius) / 2, 0.5f) +
                                 connectedEdges.Length;
                    }
                }
                else
                {
                    if (curveLookup.TryGetComponent(network, out Curve curveComponent) &&
                        endNodeGeometryLookup.TryGetComponent(network, out EndNodeGeometry endNodeGeometryComponent) &&
                        startNodeGeometryLookup.TryGetComponent(network, out StartNodeGeometry startNodeGeometryComponent))
                    {
                        Bezier4x3 endCurve = endNodeGeometryComponent.m_Geometry.m_Middle;
                        Bezier4x3 mainCurve = curveComponent.m_Bezier;
                        Bezier4x3 startCurve = startNodeGeometryComponent.m_Geometry.m_Middle;

                        /*
                          The number of centerline vertices depends on three components:
                           - The main curve (`Game.Net.Curve`)
                           - The start node curve (`Game.Net.StartNodeGeometry`)
                           - The end node curve (`Game.Net.EndNodeGeometry`)

                          The vertex count lies within the range:

                              [2, N(main) + N(start) + N(end)]

                          where N(x) is the number of interpolation points in curve x.

                          The lower bound (2) corresponds to a straight segment with no noticeable* curvature.
                          As for the upper bound, while the main curve may be trimmed at intersections (e.g., roundabouts),
                          its contribution will never exceed the number of points in the original curve.
                           * For the definition of 'noticeable', please refer to `Carto.Utils.MathUtils.IsStarightLine()`.

                          中心線的頂點數量與以下三個組件有關：
                           - 主曲線（`Game.Net.Curve`）
                           - 起點節點曲線（`Game.Net.StartNodeGeometry`）
                           - 終點節點曲線（`Game.Net.EndNodeGeometry`）

                          據此，頂點數量將落在以下區間：

                              [2, N(主曲線) + N(起點節點曲線) + N(終點節點曲線)]

                          其中 N(x) 表示曲線 x 的內插頂點數量。

                          區間下界（2）表示一條沒有明顯彎曲*的直線段。
                          至於區間上界，儘管主曲線在路口（如圓環）可能會被裁剪，其數量並不會超越原始曲線的頂點數量。
                           * 「明顯彎曲」的定義請參見 `Carto.Utils.MathUtils.IsStraightLine()`。
                         */

                        count += Utils.MathUtils.CountInterpolationPoints(mainCurve, 1f, 1f);

                        // If there are valid end node segments...（如果終點節點線段有效……）
                        if (Colossal.Mathematics.MathUtils.Length(endCurve) > 0)
                        {
                            count += Utils.MathUtils.CountInterpolationPoints(endCurve, 1f, 0.5f);
                        }

                        // If there are valid start node segments...（如果起點節點線段有效……）
                        if (Colossal.Mathematics.MathUtils.Length(startCurve) > 0)
                        {
                            count += Utils.MathUtils.CountInterpolationPoints(startCurve, 1f, 0.5f);
                        }
                    }
                }

                nodeCountEntityMap.TryAdd(network, count);
                statsIndexMap.TryAdd(network, index);
            }
        }

# if false
        /// <summary>
        /// The job and methods that is not part of the 1.0 release.
        /// （非 1.0 版本的工作與方法。）
        /// </summary>
        public partial class ToBeRewritten
        {
            /// <summary>
            /// Retrieve the number of utility service network segments.
            /// （獲得公用事業管線路段的數量。）
            /// </summary>
            /// <returns>The number of segments.（路段的數量。）</returns>
            private int GetUtilityServiceNetworkSegmentsCount()
            {
                int count = 0;

                // Initialize native containers.（初始化原生容器。）
                NativeQueue<int> individualSegmentCounts = new(Allocator.Persistent);
                NativeQueue<int> integratedSegmentCounts = new(Allocator.Persistent);

                try
                {
                    CountIndividualUtilityServiceNetworkSegmentsJob countIndividualJob = new()
                    {
                        //electricityConnectionLookup = GetComponentLookup<Game.Net.ElectricityConnection>(),
                        //pipeLineDataLookup = GetComponentLookup<PipelineData>(),
                        //powerLineDataLookup = GetComponentLookup<PowerLineData>(),
                        //waterPipeConnectionLookup = GetComponentLookup<Game.Net.WaterPipeConnection>(),
                        queue = individualSegmentCounts.AsParallelWriter()
                    };
                    JobHandle countIndividualHandle = countIndividualJob.ScheduleParallel(_networkQuery, default);
                    countIndividualHandle.Complete();

                    CountIntegratedUtilityServiceNetworkSegmentsJob countIntegratedJob = new()
                    {
                        //connectedBuildingBufferLookup = GetBufferLookup<ConnectedBuilding>(),
                        //connectedFlowEdgeBufferLookup = GetBufferLookup<ConnectedFlowEdge>(),
                        //connectedNodeBufferLookup = GetBufferLookup<ConnectedNode>(),
                        //buildingLookup = GetComponentLookup<Building>(),
                        //electricityBuildingConnectionLookup = GetComponentLookup<ElectricityBuildingConnection>(),
                        //electricityConsumerLookup = GetComponentLookup<ElectricityConsumer>(),
                        //electricityFlowEdgeLookup = GetComponentLookup<ElectricityFlowEdge>(),
                        //electricityNodeConnectionLookup = GetComponentLookup<ElectricityNodeConnection>(),
                        //placeholderLookup = GetComponentLookup<Placeholder>(),
                        //waterConsumerLookup = GetComponentLookup<WaterConsumer>(),
                        //waterPipeConnectionLookup = GetComponentLookup<Game.Net.WaterPipeConnection>(),
                        //waterPipeEdgeLookup = GetComponentLookup<WaterPipeEdge>(),
                        //waterPipeNodeConnectionLookup = GetComponentLookup<WaterPipeNodeConnection>(),
                        queue = integratedSegmentCounts.AsParallelWriter()
                    };
                    JobHandle countIntegratedHandle = countIntegratedJob.ScheduleParallel(_utilityServiceNetworkQuery, default);
                    countIntegratedHandle.Complete();

                    count += Utils.CommonUtils.Sum(ref individualSegmentCounts);
                    count += Utils.CommonUtils.Sum(ref integratedSegmentCounts);
                }
                catch (Exception ex)
                {
                    _log.Error(ex.ToString());
                }
                finally
                {
                    Utils.CommonUtils.Dispose(ref individualSegmentCounts);
                    Utils.CommonUtils.Dispose(ref integratedSegmentCounts);
                }

                return count;
            }

            /// <summary>
            /// Try to retrieve the electricity link. This is a Burst-compatible version of <see cref="ElectricityGraphUtils.TryGetFlowEdge"/>.<br/>
            /// （嘗試取得電流的連結。這是 <see cref="ElectricityGraphUtils.TryGetFlowEdge"/> 的可 Burst 編譯版本。）
            /// </summary>
            /// <param name="startNode">The entity that holds the start node of the link.（擁有連結起始節點的實體。）</param>
            /// <param name="endNode">The entity that holds the end node of the link.（擁有連結結尾節點的實體。）</param>
            /// <param name="flowEdges">The look-up of <see cref="ConnectedFlowEdge"/>.（<see cref="ConnectedFlowEdge"/> 的查詢。）</param>
            /// <param name="flowEdgeLookup">The look-up of <see cref="ElectricityFlowEdge"/>.（<see cref="ElectricityFlowEdge"/> 的查詢。）</param>
            /// <param name="flowEdge">The electricity link.（電流連結。）</param>
            /// <returns>Whether the link exists or not.（連結是否存在？）</returns>
            private static bool TryGetElectricityFlowEdge(Entity startNode, Entity endNode, ref BufferLookup<ConnectedFlowEdge> flowEdges, ref ComponentLookup<ElectricityFlowEdge> flowEdgeLookup, out ElectricityFlowEdge flowEdge)
            {
                flowEdge = default;
                if ((startNode.Index <= 0) || (endNode.Index <= 0)) return false;
                if (!flowEdges.TryGetBuffer(startNode, out DynamicBuffer<ConnectedFlowEdge> connectedFlowEdges)) return false;

                for (int i = 0; i < connectedFlowEdges.Length; i++)
                {
                    ConnectedFlowEdge connectedFlowEdge = connectedFlowEdges[i];
                    if (flowEdgeLookup.TryGetComponent(connectedFlowEdge.m_Edge, out ElectricityFlowEdge edgeCandidate) && (edgeCandidate.m_Start == startNode) && (edgeCandidate.m_End == endNode))
                    {
                        flowEdge = edgeCandidate;
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Try to retrieve the water link. This is a Burst-compatible version of <see cref="WaterPipeGraphUtils.TryGetFlowEdge"/>.<br/>
            /// （嘗試取得水源的連結。這是 <see cref="WaterPipeGraphUtils.TryGetFlowEdge"/> 的可 Burst 編譯版本。）
            /// </summary>
            /// <param name="startNode">The entity that holds the start node of the link.（擁有連結起始節點的實體。）</param>
            /// <param name="endNode">The entity that holds the end node of the link.（擁有連結結尾節點的實體。）</param>
            /// <param name="flowEdges">The look-up of <see cref="ConnectedFlowEdge"/>.（<see cref="ConnectedFlowEdge"/> 的查詢。）</param>
            /// <param name="flowEdgeLookup">The look-up of <see cref="WaterPipeEdge"/>.（<see cref="WaterPipeEdge"/> 的查詢。）</param>
            /// <param name="flowEdge">The water pipe link.（水流連結。）</param>
            /// <returns>Whether the link exists or not.（連結是否存在？）</returns>
            private static bool TryGetWaterPipeEdge(Entity startNode, Entity endNode, ref BufferLookup<ConnectedFlowEdge> flowEdges, ref ComponentLookup<WaterPipeEdge> flowEdgeLookup, out WaterPipeEdge flowEdge)
            {
                flowEdge = default;
                if ((startNode.Index <= 0) || (endNode.Index <= 0)) return false;
                if (!flowEdges.TryGetBuffer(startNode, out DynamicBuffer<ConnectedFlowEdge> connectedFlowEdges)) return false;

                for (int i = 0; i < connectedFlowEdges.Length; i++)
                {
                    ConnectedFlowEdge connectedFlowEdge = connectedFlowEdges[i];
                    if (flowEdgeLookup.TryGetComponent(connectedFlowEdge.m_Edge, out WaterPipeEdge edgeCandidate) && (edgeCandidate.m_Start == startNode) && (edgeCandidate.m_End == endNode))
                    {
                        flowEdge = edgeCandidate;
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// The job to count the number of segments required to represent the individual (NOT included in the roads) utility service networks.
            /// （計算表達獨立公用事業管線（未包含在道路中的）所需路段數量的工作。）
            /// </summary>
            public partial struct CountIndividualUtilityServiceNetworkSegmentsJob : IJobEntity
            {
                [ReadOnly]
                public ComponentLookup<Game.Net.ElectricityConnection> electricityConnectionLookup;

                [ReadOnly]
                public ComponentLookup<PipelineData> pipeLineDataLookup;

                [ReadOnly]
                public ComponentLookup<PowerLineData> powerLineDataLookup;

                [ReadOnly]
                public ComponentLookup<Game.Net.WaterPipeConnection> waterPipeConnectionLookup;

                [WriteOnly]
                public NativeQueue<int>.ParallelWriter queue;

                public void Execute(in PrefabRef prefabRef, Entity network)
                {
                    int count = 0;
                    Entity prefab = prefabRef.m_Prefab;

                    if (pipeLineDataLookup.TryGetComponent(prefab, out _) && waterPipeConnectionLookup.TryGetComponent(network, out Game.Net.WaterPipeConnection waterPipeData))
                    {
                        if (waterPipeData.m_FreshCapacity > 0) count++;
                        if (waterPipeData.m_SewageCapacity > 0) count++;
                        // TODO: Uncomment the line when the storm pipes are added... if (waterPipeData.m_StormCapacity > 0) count++;
                    }

                    if (powerLineDataLookup.TryGetComponent(prefab, out _) && electricityConnectionLookup.TryGetComponent(network, out _))
                    {
                        count++;
                    }

                    queue.Enqueue(count);
                }
            }

            /// <summary>
            /// The job to count the number of segments required to represent the integrated (included in the roads) utility service networks.
            /// （計算表達整合公用事業管線（包含在道路中的）所需路段數量的工作。）
            /// </summary>
            public partial struct CountIntegratedUtilityServiceNetworkSegmentsJob : IJobEntity
            {
                [ReadOnly]
                public BufferLookup<ConnectedBuilding> connectedBuildingBufferLookup;

                [ReadOnly]
                public BufferLookup<ConnectedFlowEdge> connectedFlowEdgeBufferLookup;

                [ReadOnly]
                public BufferLookup<ConnectedNode> connectedNodeBufferLookup;

                [ReadOnly]
                public ComponentLookup<Building> buildingLookup;

                [ReadOnly]
                public ComponentLookup<ElectricityBuildingConnection> electricityBuildingConnectionLookup;

                [ReadOnly]
                public ComponentLookup<ElectricityConsumer> electricityConsumerLookup;

                [ReadOnly]
                public ComponentLookup<ElectricityFlowEdge> electricityFlowEdgeLookup;

                [ReadOnly]
                public ComponentLookup<ElectricityNodeConnection> electricityNodeConnectionLookup;

                [ReadOnly]
                public ComponentLookup<Placeholder> placeholderLookup;

                [ReadOnly]
                public ComponentLookup<WaterConsumer> waterConsumerLookup;

                [ReadOnly]
                public ComponentLookup<Game.Net.WaterPipeConnection> waterPipeConnectionLookup;

                [ReadOnly]
                public ComponentLookup<WaterPipeNodeConnection> waterPipeNodeConnectionLookup;

                [ReadOnly]
                public ComponentLookup<WaterPipeEdge> waterPipeEdgeLookup;

                [WriteOnly]
                public NativeQueue<int>.ParallelWriter queue;

                public void Execute(Entity network)
                {
                    bool hasCable = electricityNodeConnectionLookup.TryGetComponent(network, out ElectricityNodeConnection electricityConnCenter);
                    bool hasPipe = waterPipeNodeConnectionLookup.TryGetComponent(network, out WaterPipeNodeConnection waterPipeConnCenter);
                    if (!hasCable && !hasPipe) return;

                    int count = 0;

                    if (hasCable)
                    {
                        count++;

                        // Handle the connection between user-drawn cables.（處理與使用者繪製的電纜相接處。）
                        if (connectedNodeBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedNode> connectedNodes))
                        {
                            for (int i = 0; i < connectedNodes.Length; i++)
                            {
                                ConnectedNode connectedNode = connectedNodes[i];
                                if (electricityNodeConnectionLookup.TryGetComponent(connectedNode.m_Node, out ElectricityNodeConnection electricityConnStart) &&
                                    TryGetElectricityFlowEdge(electricityConnStart.m_ElectricityNode,
                                                              electricityConnCenter.m_ElectricityNode,
                                                              ref connectedFlowEdgeBufferLookup,
                                                              ref electricityFlowEdgeLookup,
                                                              out _))
                                {
                                    count++;
                                }
                            }
                        }

                        // Handle building connections.（處理建築連結。）
                        if (connectedBuildingBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedBuilding> connectedBuildings))
                        {
                            /* 
                             * The list can hold at least 1000 floats (4,000 bytes).
                             * This design ensures even when the user stretches the network really long [with Mod] and creates lots of connected buildings, the list capacity is still enough in most extreme cases.
                             * （列表可以裝下至少 1000 個單精度浮點數（4,000 位元組）。）
                             * （這個設計確保即使使用者［透過模組］將路段拉得非常長，並創造許多相連建築時，列表容量依然充裕。）
                             */
                            FixedList4096Bytes<float> curvePositions = default;
                            int uniqueBuildingsCount = 0;

                            for (int i = 0; i < connectedBuildings.Length; i++)
                            {
                                Entity connectedBuilding = connectedBuildings[i].m_Building;
                                if (buildingLookup.TryGetComponent(connectedBuilding, out Building buildingComponent) &&
                                    electricityConsumerLookup.TryGetComponent(connectedBuilding, out _) &&
                                    !placeholderLookup.TryGetComponent(connectedBuilding, out _))
                                {
                                    bool recorded = false;
                                    float curvePosition = buildingComponent.m_CurvePosition;
                                    for (int j = 0; j < curvePositions.Length; j++)
                                    {
                                        if (curvePositions[j] == curvePosition)
                                        {
                                            recorded = true;
                                            break;
                                        }
                                    }

                                    if (!recorded)
                                    {
                                        curvePositions.Add(curvePosition);
                                    }

                                    uniqueBuildingsCount++;
                                }
                            }

                            // For each connected building, it splits an existing cable and create a cable to the building.
                            //（每棟相接的建築皆會切分既有的電纜，並且創造連接建築的電纜。）
                            count += curvePositions.Length + uniqueBuildingsCount;
                        }
                    }

                    if (hasPipe)
                    {
                        int pipeTypes = 0;
                        if (waterPipeConnectionLookup.TryGetComponent(network, out Game.Net.WaterPipeConnection waterPipeData))
                        {
                            if (waterPipeData.m_FreshCapacity > 0) pipeTypes++;
                            if (waterPipeData.m_SewageCapacity > 0) pipeTypes++;
                            count += pipeTypes;
                        }

                        // Handle the connection between user-drawn pipes.（處理與使用者繪製的水管相接處。）
                        if (connectedNodeBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedNode> connectedNodes))
                        {
                            for (int i = 0; i < connectedNodes.Length; i++)
                            {
                                ConnectedNode connectedNode = connectedNodes[i];
                                if (waterPipeNodeConnectionLookup.TryGetComponent(connectedNode.m_Node, out WaterPipeNodeConnection waterPipeConnStart) &&
                                    TryGetWaterPipeEdge(waterPipeConnStart.m_WaterPipeNode,
                                                        waterPipeConnCenter.m_WaterPipeNode,
                                                        ref connectedFlowEdgeBufferLookup,
                                                        ref waterPipeEdgeLookup,
                                                        out _))
                                {
                                    count++;
                                }
                            }
                        }

                        // Handle building connections.（處理建築連結。）
                        if (connectedBuildingBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedBuilding> connectedBuildings))
                        {
                            FixedList4096Bytes<float> curvePositions = default;
                            int uniqueBuildingsCount = 0;

                            for (int i = 0; i < connectedBuildings.Length; i++)
                            {
                                Entity connectedBuilding = connectedBuildings[i].m_Building;
                                if (buildingLookup.TryGetComponent(connectedBuilding, out Building buildingComponent) &&
                                    waterConsumerLookup.TryGetComponent(connectedBuilding, out _) &&
                                    !placeholderLookup.TryGetComponent(connectedBuilding, out _))
                                {
                                    bool recorded = false;
                                    float curvePosition = buildingComponent.m_CurvePosition;
                                    for (int j = 0; j < curvePositions.Length; j++)
                                    {
                                        if (curvePositions[j] == curvePosition)
                                        {
                                            recorded = true;
                                            break;
                                        }
                                    }

                                    if (!recorded)
                                    {
                                        curvePositions.Add(curvePosition);
                                    }

                                    uniqueBuildingsCount++;
                                }
                            }

                            // For each connected building, it splits existing pipes and create pipes to the building.
                            //（每棟相接的建築皆會切分既有的水管，並且創造連接建築的水管。）
                            count += (curvePositions.Length + uniqueBuildingsCount) * pipeTypes;
                        }
                    }

                    queue.Enqueue(count);
                }
            }
        }
# endif
    }
}