using Carto.Domain;
using Carto.IO;
using Colossal.Logging;
using Game;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using Unity.Burst;
using Unity.Collections;
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
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

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
        /// The query to collect the attachment of roundabouts.
        /// （收集圓環附件的查詢。）
        /// </summary>
        static EntityQuery _roundaboutAttachmentQuery;

        /// <summary>
        /// The query to collect all roundabouts.
        /// （收集所有圓環的查詢。）
        /// </summary>
        static EntityQuery _roundaboutQuery;

        /// <summary>
        /// The query to collect all utility service network entities.
        /// （收集所有公共服務管線網路實體的查詢。）
        /// </summary>
        static EntityQuery _utilityServiceNetworkQuery;

        /// <summary>
        /// The list of all network's statistics in the savegame.
        /// （遊戲存檔內所有網路的統計數據。）
        /// </summary>
        public ref NativeList<NetworkStat> NetworkStats => ref _networkStats;

        /// <summary>
        /// See <see cref="NetworkStats"/>.
        /// </summary>
        private NativeList<NetworkStat> _networkStats;

        /// <summary>
        /// The map between network entities and their index in <see cref="NetworkStats"/>.
        /// （網路實體與其在 <see cref="NetworkStats"/> 索引值的映射表。）
        /// </summary>
        public ref NativeParallelHashMap<Entity, int> NetworkStatsEntityMap => ref _networkStatsEntityMap;

        /// <summary>
        /// See <see cref="NetworkStatsEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _networkStatsEntityMap;

        /// <summary>
        /// The list of in-game roundabouts.
        /// （遊戲內圓環的列表。）
        /// </summary>
        public ref NativeList<Domain.Roundabout> Roundabouts => ref _roundabouts;

        /// <summary>
        /// See <see cref="Roundabouts"/>.
        /// </summary>
        public NativeList<Domain.Roundabout> _roundabouts;

        /// <summary>
        /// The map between roundabout nodes and their index in <see cref="Roundabouts"/>.
        /// （圓環節點實體與其在 <see cref="Roundabouts"/> 索引值的映射表。）
        /// </summary>
        public ref NativeParallelHashMap<Entity, int> RoundaboutsEntityMap => ref _roundaboutsEntityMap;

        /// <summary>
        /// See <see cref="RoundaboutsEntityMap"/>.
        /// </summary>
        private NativeParallelHashMap<Entity, int> _roundaboutsEntityMap;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
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

            _networkQuery = GetEntityQuery(new EntityQueryDesc()
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
                    ComponentType.ReadOnly<Pillar>(),
                    ComponentType.ReadOnly<Game.Routes.TransportStop>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

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

            _utilityServiceNetworkQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Edge>()
                },
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<ElectricityNodeConnection>(),
                    ComponentType.ReadOnly<WaterPipeNodeConnection>()
                }
            });

            base.OnCreate();
            _log.Debug("NetworkSystem instance created. 網路系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy()
        {
            Dispose();
            base.OnDestroy();
        }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Try disposing of all properties stored in unmanaged memory.
        /// （嘗試丟棄儲存於未控管記憶體的屬性。）
        /// </summary>
        public void Dispose()
        {
            Utils.CommonUtils.Dispose(ref _networkStats);
            Utils.CommonUtils.Dispose(ref _networkStatsEntityMap);
            Utils.CommonUtils.Dispose(ref _roundabouts);
            Utils.CommonUtils.Dispose(ref _roundaboutsEntityMap);
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
        /// Retrieve network entities' statistical data.
        /// （獲取網路實體的統計資料。）
        /// </summary>
        /// <param name="options">The export options.（檔案輸出選項。）</param>
        public void GetNetworkStats(Options options)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref NativeParallelHashMap<Entity, int> entityMap = ref _networkStatsEntityMap;
            ref NativeList<Domain.Roundabout> roundabouts = ref _roundabouts;
            ref NativeParallelHashMap<Entity, int> roundaboutsEntityMap = ref _roundaboutsEntityMap;
            ref NativeList<NetworkStat> stats = ref _networkStats;

            // Export options.（輸出設定。）
            Feature featureFlag = options.Features;
            bool hasCenterline = (
                (options.VectorKinds.TryGetValue(IO.System.Network, out VectorKind networkKinds) && (networkKinds & VectorKind.Centerline) != 0) ||
                (options.VectorKinds.TryGetValue(IO.System.Route, out VectorKind routeKinds) && (routeKinds & VectorKind.Centerline) != 0)
            );
            bool hasUtilityServices = ((featureFlag & Feature.Cable) != 0) || ((featureFlag & Feature.Pipe) != 0);

            // Initialize native containers.（初始化原生容器。）
            int lanePrefabCount = _lanePrefabQuery.CalculateEntityCount();
            int roundaboutAttachmentCount = _roundaboutAttachmentQuery.CalculateEntityCount();
            int roundaboutCount = _roundaboutQuery.CalculateEntityCount();
            int utilityServiceNetworkCount = GetUtilityServiceNetworkSegmentsCount();
            int networkCount = _networkQuery.CalculateEntityCount();
            if (hasCenterline) networkCount += roundaboutCount;
            if (hasUtilityServices) networkCount += utilityServiceNetworkCount;
            NativeParallelHashMap<Entity, Domain.Lane> lanesEntityMap = new(lanePrefabCount, Allocator.Persistent);
            NativeParallelHashMap<Entity, Entity> attachmentEntityMap = new(roundaboutAttachmentCount, Allocator.Persistent);
            NativeParallelHashSet<Entity> utilityLanes = new(lanePrefabCount, Allocator.Persistent);

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset(ref entityMap, networkCount);
            Utils.CommonUtils.Reset(ref roundabouts, roundaboutCount);
            Utils.CommonUtils.Reset(ref roundaboutsEntityMap, roundaboutCount);
            Utils.CommonUtils.Reset(ref stats, networkCount);

            try
            {
                CollectLanesJob collectLanesJob = new()
                {
                    carLaneDataLookup = GetComponentLookup<CarLaneData>(),
                    parkingLaneDataLookup = GetComponentLookup<ParkingLaneData>(),
                    trackLaneDataLookup = GetComponentLookup<TrackLaneData>(),
                    utilityLaneDataLookup = GetComponentLookup<UtilityLaneData>(),
                    entityMap = lanesEntityMap.AsParallelWriter(),
                    utilityLanes = utilityLanes.AsParallelWriter()
                };
                JobHandle collectLanesHandle = collectLanesJob.ScheduleParallel(_lanePrefabQuery, default);
                collectLanesHandle.Complete();

                MapRoundaboutAttachmentsJob mapAttachmentJob = new()
                {
                    entityMap = attachmentEntityMap.AsParallelWriter(),
                };
                JobHandle mapAttachmentHandle = mapAttachmentJob.ScheduleParallel(_roundaboutAttachmentQuery, default);
                mapAttachmentHandle.Complete();

                CollectRoundaboutsJob collectRoundaboutsJob = new()
                {
                    hasCenterline = hasCenterline,
                    leftHandTraffic = Instance.City.leftHandTraffic,
                    compositionLookup = GetComponentLookup<Composition>(),
                    netCompositionDataLookup = GetComponentLookup<NetCompositionData>(),
                    netGeometryDataLookup = GetComponentLookup<NetGeometryData>(),
                    objectGeometryDataLookup = GetComponentLookup<ObjectGeometryData>(),
                    placeableObjectDataLookup = GetComponentLookup<PlaceableObjectData>(),
                    roadLookup = GetComponentLookup<Road>(),
                    roadCompositionLookup = GetComponentLookup<RoadComposition>(),
                    subwayTrackLookup = GetComponentLookup<SubwayTrack>(),
                    trackCompositionLookup = GetComponentLookup<TrackComposition>(),
                    trainTrackLookup = GetComponentLookup<TrainTrack>(),
                    tramTrackLookup = GetComponentLookup<TramTrack>(),
                    attachmentEntityMap = attachmentEntityMap,
                    list = roundabouts.AsParallelWriter(),
                    stats = stats.AsParallelWriter()
                };
                JobHandle collectRoundaboutsHandle = collectRoundaboutsJob.ScheduleParallel(_roundaboutQuery, default);
                collectRoundaboutsHandle.Complete();

                MapRoundaboutsIndexJob mapIndexJob = new()
                {
                    list = roundabouts,
                    map = roundaboutsEntityMap
                };
                JobHandle mapIndexHandle = mapIndexJob.Schedule(roundaboutCount, 16);
                mapIndexHandle.Complete();

                CollectNetworkStatsJob collectNetworkStatsJob = new()
                {
                    hasUtilityServiceNetworks = hasUtilityServices,
                    connectedBuildingBufferLookup = GetBufferLookup<ConnectedBuilding>(),
                    connectedFlowEdgeBufferLookup = GetBufferLookup<ConnectedFlowEdge>(),
                    netCompositionPieceBufferLookup = GetBufferLookup<NetCompositionPiece>(),
                    netPieceLaneBufferLookup = GetBufferLookup<NetPieceLane>(),
                    subLaneBufferLookup = GetBufferLookup<Game.Net.SubLane>(),
                    buildingLookup = GetComponentLookup<Building>(),
                    electricityConsumerLookup = GetComponentLookup<ElectricityConsumer>(),
                    electricityFlowEdgeLookup = GetComponentLookup<ElectricityFlowEdge>(),
                    electricityNodeConnectionLookup = GetComponentLookup<ElectricityNodeConnection>(),
                    markerLookup = GetComponentLookup<Game.Net.Marker>(),
                    netCompositionDataLookup = GetComponentLookup<NetCompositionData>(),
                    pathwayCompositionLookup = GetComponentLookup<PathwayComposition>(),
                    pipelineDataLookup = GetComponentLookup<PipelineData>(),
                    powerLineDataLookup = GetComponentLookup<PowerLineData>(),
                    prefabRefLookup = GetComponentLookup<PrefabRef>(),
                    roadLookup = GetComponentLookup<Road>(),
                    roadCompositionLookup = GetComponentLookup<RoadComposition>(),
                    taxiwayCompositionLookup = GetComponentLookup<TaxiwayComposition>(),
                    trackCompositionLookup = GetComponentLookup<TrackComposition>(),
                    waterPipeEdgeLookup = GetComponentLookup<WaterPipeEdge>(),
                    waterPipeNodeConnectionLookup = GetComponentLookup<WaterPipeNodeConnection>(),
                    waterwayCompositionLookup = GetComponentLookup<WaterwayComposition>(),
                    lanesEntityMap = lanesEntityMap,
                    roundaboutsEntityMap = roundaboutsEntityMap,
                    list = stats.AsParallelWriter()
                };
                JobHandle collectNetworkStatsHandle = collectNetworkStatsJob.ScheduleParallel(_networkQuery, default);
                collectNetworkStatsHandle.Complete();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref attachmentEntityMap);
                Utils.CommonUtils.Dispose(ref lanesEntityMap);
                Utils.CommonUtils.Dispose(ref utilityLanes);
            }

            for (int i = 0; i < _networkStats.Length; i++)
            {
                _log.Info(_networkStats[i].ToString());
            }

            // Temporary disposal
            Dispose();
        }

        /// <summary>
        /// Retrieve the number of utility service network segments.
        /// （獲得公共服務管線路段的數量。）
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
                    electricityConnectionLookup = GetComponentLookup<Game.Net.ElectricityConnection>(),
                    pipeLineDataLookup = GetComponentLookup<PipelineData>(),
                    powerLineDataLookup = GetComponentLookup<PowerLineData>(),
                    waterPipeConnectionLookup = GetComponentLookup<Game.Net.WaterPipeConnection>(),
                    queue = individualSegmentCounts.AsParallelWriter()
                };
                JobHandle countIndividualHandle = countIndividualJob.ScheduleParallel(_networkQuery, default);
                countIndividualHandle.Complete();

                CountIntegratedUtilityServiceNetworkSegmentsJob countIntegratedJob = new()
                {
                    connectedBuildingBufferLookup = GetBufferLookup<ConnectedBuilding>(),
                    connectedFlowEdgeBufferLookup = GetBufferLookup<ConnectedFlowEdge>(),
                    connectedNodeBufferLookup = GetBufferLookup<ConnectedNode>(),
                    buildingLookup = GetComponentLookup<Building>(),
                    electricityBuildingConnectionLookup = GetComponentLookup<ElectricityBuildingConnection>(),
                    electricityConsumerLookup = GetComponentLookup<ElectricityConsumer>(),
                    electricityFlowEdgeLookup = GetComponentLookup<ElectricityFlowEdge>(),
                    electricityNodeConnectionLookup = GetComponentLookup<ElectricityNodeConnection>(),
                    placeholderLookup = GetComponentLookup<Placeholder>(),
                    waterConsumerLookup = GetComponentLookup<WaterConsumer>(),
                    waterPipeConnectionLookup = GetComponentLookup<Game.Net.WaterPipeConnection>(),
                    waterPipeEdgeLookup = GetComponentLookup<WaterPipeEdge>(),
                    waterPipeNodeConnectionLookup = GetComponentLookup<WaterPipeNodeConnection>(),
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
        /// Get the volume of the road from the Road component.
        /// （由 Road 部件獲得道路的流量。）
        /// </summary>
        /// <param name="road">The Road component.（Road 部件。）</param>
        /// <returns>The number of vehicles using the road.（使用道路的車輛數量。）</returns>
        private static float GetVolume(Road road)
        {
            // According to `Game.UI.InGame.RoadSection`, the volume data is calculated every 6 hours.
            // The x, y, z, and w property represent the distance / duration at 00:00, 06:00, 12:00 and 18:00.
            // Carto use the traffic volume at noon to show the traffic in peak hour.
            // （根據 `Game.UI.InGame.RoadSection`，流量每 6 小時計算一次，其中 x、y、z、w 屬性分別代表 00:00、06:00、12:00、18:00 的距離／持續時間。）
            // （為展示尖峰時間的車流量，Carto 使用中午的數據。）
            return (float)Math.Round((road.m_TrafficFlowDistance0.z + road.m_TrafficFlowDistance1.z) * 8f / 3f, 2);
        }

        /// <summary>
        /// Round the speed limit to more "beautiful" numbers.
        /// （將速度限制數字取整為「漂亮的」數字。）
        /// </summary>
        /// <param name="input">The input value.（輸入的數值。）</param>
        /// <returns>The speed limit value that is divisable by 5.（可被 5 整除的速度限制數值。）</returns>
        private static float RoundSpeedLimit(float input) => (float)(Math.Round(input * 2 / 5.0) * 5.0);

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

            [WriteOnly]
            public NativeParallelHashSet<Entity>.ParallelWriter utilityLanes;

            public void Execute(in NetLaneData netLaneData, Entity lane)
            {
                Domain.Lane laneStruct = new()
                {
                    entity = lane,
                    category = NetworkCategory.None
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
                            utilityLanes.Add(lane);
                        }
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
            public bool hasUtilityServiceNetworks;

            [ReadOnly]
            public BufferLookup<ConnectedBuilding> connectedBuildingBufferLookup;

            [ReadOnly]
            public BufferLookup<ConnectedFlowEdge> connectedFlowEdgeBufferLookup;

            [ReadOnly]
            public BufferLookup<NetCompositionPiece> netCompositionPieceBufferLookup;

            [ReadOnly]
            public BufferLookup<NetPieceLane> netPieceLaneBufferLookup;

            [ReadOnly]
            public BufferLookup<Game.Net.SubLane> subLaneBufferLookup;

            [ReadOnly]
            public ComponentLookup<Building> buildingLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityConsumer> electricityConsumerLookup;

            [ReadOnly]
            public ComponentLookup<ElectricityFlowEdge> electricityFlowEdgeLookup;

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
            public ComponentLookup<Placeholder> placeholderLookup;

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
            public ComponentLookup<WaterConsumer> waterConsumerLookup;

            [ReadOnly]
            public ComponentLookup<WaterPipeEdge> waterPipeEdgeLookup;

            [ReadOnly]
            public ComponentLookup<WaterPipeNodeConnection> waterPipeNodeConnectionLookup;

            [ReadOnly]
            public ComponentLookup<WaterwayComposition> waterwayCompositionLookup;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Domain.Lane> lanesEntityMap;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> roundaboutsEntityMap;

            [WriteOnly]
            public NativeList<NetworkStat>.ParallelWriter list;

            public void Execute(in Composition composition, in Curve curve, in Edge edge, in EdgeGeometry edgeGeometry, in PrefabRef prefabRef, in DynamicBuffer<Game.Net.SubLane> subLanes, Entity network)
            {
                NetworkStat stat = new()
                {
                    entity = network,
                    capacity = 0f,
                    category = NetworkCategory.None,
                    direction = Direction.None,
                    discharge = 0f,
                    elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                    end = 1f,
                    endRoundaboutIndex = roundaboutsEntityMap.TryGetValue(edge.m_End, out int endRoundaboutIndex) ? endRoundaboutIndex : -1,
                    form = Form.Normal,
                    isRoundabout = false,
                    length = curve.m_Length,
                    limit = 0f,
                    load = 0f,
                    start = 0f,
                    startRoundaboutIndex = roundaboutsEntityMap.TryGetValue(edge.m_Start, out int startRoundaboutIndex) ? startRoundaboutIndex : -1,
                    volume = 0f,
                    width = 0f
                };

                DynamicBuffer<ConnectedFlowEdge> cableEdges = default;
                DynamicBuffer<ConnectedFlowEdge> pipeEdges = default;

                bool isPipeline = pipelineDataLookup.TryGetComponent(prefabRef, out _) && waterPipeNodeConnectionLookup.TryGetComponent(network, out WaterPipeNodeConnection pipeNodeConnection) && connectedFlowEdgeBufferLookup.TryGetBuffer(pipeNodeConnection.m_WaterPipeNode, out pipeEdges) && (pipeEdges.Length >= 2);
                bool isPowerLine = powerLineDataLookup.TryGetComponent(prefabRef, out _) && electricityNodeConnectionLookup.TryGetComponent(network, out ElectricityNodeConnection cableNodeConnection) && connectedFlowEdgeBufferLookup.TryGetBuffer(cableNodeConnection.m_ElectricityNode, out cableEdges) && (cableEdges.Length >= 2);
                bool isNotUtilityServiceNetwork = !(isPipeline || isPowerLine);
                bool overrideForm = false;
                Entity networkComposition = composition.m_Edge;
                bool hasComposition = networkComposition != Entity.Null;
                bool isTaxiway = taxiwayCompositionLookup.TryGetComponent(networkComposition, out TaxiwayComposition taxiwayComposition);

                if (markerLookup.TryGetComponent(network, out _) && !isTaxiway) return;

                if (isNotUtilityServiceNetwork)
                {
                    for (int i = 0; i < subLanes.Length; i++)
                    {
                        Entity subLane = subLanes[i].m_SubLane;
                        if (prefabRefLookup.TryGetComponent(subLane, out PrefabRef subLanePrefab) && lanesEntityMap.TryGetValue(subLanePrefab.m_Prefab, out Domain.Lane laneType) && !laneType.IsUtilityLane)
                        {
                            stat.category |= laneType.category;
                        }
                    }

                    if (hasComposition)
                    {
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
                            TaxiwayFlags taxiwayFlags = taxiwayComposition.m_Flags;
                            if (taxiwayFlags == TaxiwayFlags.Airspace) return;  // Exporting air space is not supported.（不支援輸出空域。）
                            bool hasRunwayFlag = (taxiwayFlags & TaxiwayFlags.Runway) != 0;
                            stat.category = hasRunwayFlag ? NetworkCategory.Runway : NetworkCategory.Taxiway;
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

                            stat.width = netCompositionData.m_Width;
                        }
                    }

                    // Calculate the traffic volume.（計算交通流量。）
                    if (roadLookup.TryGetComponent(network, out Road roadComponent))
                    {
                        stat.volume = GetVolume(roadComponent);
                    }

                    list.AddNoResize(stat);
                }

                // Handle the individual utility service networks.（處理獨立公共管線網路。）
                if (hasUtilityServiceNetworks && !isNotUtilityServiceNetwork)
                {
                    // Generate the `NetworkStat` for the pipe.（產生水管的 `NetworkStat`。）
                    if (isPipeline && waterPipeEdgeLookup.TryGetComponent(pipeEdges[0].m_Edge, out WaterPipeEdge pipeFlow))
                    {
                        if (pipeFlow.m_FreshCapacity > 0)
                        {
                            NetworkStat water = new()
                            {
                                entity = network,
                                capacity = pipeFlow.m_FreshCapacity,
                                category = NetworkCategory.WaterPipe,
                                direction = Direction.None,
                                discharge = Math.Abs(pipeFlow.m_FreshFlow),
                                elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                                end = 1f,
                                endRoundaboutIndex = -1,
                                form = Form.Normal,
                                isRoundabout = false,
                                length = curve.m_Length,
                                limit = 0f,
                                load = 0f,
                                start = 0f,
                                startRoundaboutIndex = -1,
                                volume = 0f,
                                width = 0f
                            };

                            switch (pipeFlow.m_FreshFlow)
                            {
                                case > 0:
                                    water.direction = Direction.Forward;
                                    break;

                                case < 0:
                                    water.direction = Direction.Backward;
                                    break;

                                default:
                                    break;
                            }

                            if (hasComposition)
                            {
                                if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositiondata))
                                {
                                    CompositionFlags.General generalCompositions = netCompositiondata.m_Flags.m_General;
                                    if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) water.form = Form.Tunnel;
                                }

                                bool pieceFound = false;
                                float pieceWidth = 1.5f; // The vanilla water pipe's width.（原版遊戲自來水管的寬度。）

                                if (netCompositionPieceBufferLookup.TryGetBuffer(networkComposition, out DynamicBuffer<NetCompositionPiece> netCompositionPieces))
                                {
                                    for (int i = 0; i < netCompositionPieces.Length; i++)
                                    {
                                        if (netPieceLaneBufferLookup.TryGetBuffer(netCompositionPieces[i].m_Piece, out DynamicBuffer<NetPieceLane> netPieceLanes))
                                        {
                                            for (int j = 0; j < netPieceLanes.Length; j++)
                                            {
                                                if (lanesEntityMap.TryGetValue(netPieceLanes[j].m_Lane, out Domain.Lane lane) && lane.category == NetworkCategory.WaterPipe)
                                                {
                                                    pieceFound = true;
                                                    pieceWidth = Math.Abs(netCompositionPieces[i].m_Size.x);
                                                    break;
                                                }
                                            }
                                        }

                                        if (pieceFound) break;
                                    }

                                    water.width = pieceWidth;
                                }
                            }

                            list.AddNoResize(water);
                        }

                        if (pipeFlow.m_SewageCapacity > 0)
                        {
                            NetworkStat sewage = new()
                            {
                                entity = network,
                                capacity = pipeFlow.m_SewageCapacity,
                                category = NetworkCategory.SewagePipe,
                                direction = Direction.None,
                                discharge = Math.Abs(pipeFlow.m_SewageFlow),
                                elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                                end = 1f,
                                endRoundaboutIndex = -1,
                                form = Form.Normal,
                                isRoundabout = false,
                                length = curve.m_Length,
                                limit = 0f,
                                load = 0f,
                                start = 0f,
                                startRoundaboutIndex = -1,
                                volume = 0f,
                                width = 0f
                            };

                            switch (pipeFlow.m_SewageFlow)
                            {
                                // The default direction of the sewage water is opposite to that of fresh water.（汙水的方向與自來水的方向預設相反。）
                                case > 0:
                                    sewage.direction = Direction.Backward;
                                    break;

                                case < 0:
                                    sewage.direction = Direction.Forward;
                                    break;

                                default:
                                    break;
                            }

                            if (hasComposition)
                            {
                                if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositiondata))
                                {
                                    CompositionFlags.General generalCompositions = netCompositiondata.m_Flags.m_General;
                                    if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) sewage.form = Form.Tunnel;
                                }

                                bool pieceFound = false;
                                float pieceWidth = 2f; // The vanilla sewage pipe's width.（原版遊戲污水管的寬度。）

                                if (netCompositionPieceBufferLookup.TryGetBuffer(networkComposition, out DynamicBuffer<NetCompositionPiece> netCompositionPieces))
                                {
                                    for (int i = 0; i < netCompositionPieces.Length; i++)
                                    {
                                        if (netPieceLaneBufferLookup.TryGetBuffer(netCompositionPieces[i].m_Piece, out DynamicBuffer<NetPieceLane> netPieceLanes))
                                        {
                                            for (int j = 0; j < netPieceLanes.Length; j++)
                                            {
                                                if (lanesEntityMap.TryGetValue(netPieceLanes[j].m_Lane, out Domain.Lane lane) && lane.category == NetworkCategory.WaterPipe)
                                                {
                                                    pieceFound = true;
                                                    pieceWidth = Math.Abs(netCompositionPieces[i].m_Size.x);
                                                    break;
                                                }
                                            }
                                        }

                                        if (pieceFound) break;
                                    }

                                    sewage.width = pieceWidth;
                                }
                            }

                            list.AddNoResize(sewage);
                        }

                        // TODO: Uncomment the line when the storm pipes are added... if (pipeFlow.m_StormCapacity > 0)
                    }

                    // Generate the `NetworkStat` for the cable.（產生電纜的 `NetworkStat`。）
                    if (isPowerLine && electricityFlowEdgeLookup.TryGetComponent(cableEdges[0].m_Edge, out ElectricityFlowEdge cableFlow))
                    {
                        NetworkStat cable = new()
                        {
                            entity = network,
                            capacity = cableFlow.m_Capacity / 10f,
                            category = NetworkCategory.None,
                            direction = Direction.None,
                            discharge = 0f,
                            elevation = ((edgeGeometry.m_Bounds.max + edgeGeometry.m_Bounds.min) / 2).y,
                            end = 1f,
                            endRoundaboutIndex = -1,
                            form = Form.Elevated, // The above ground cables are always overhead-ed.（地表之上的電纜是架空的。）
                            isRoundabout = false,
                            length = curve.m_Length,
                            limit = 0f,
                            load = Math.Abs(cableFlow.m_Flow) / 10f,
                            start = 0f,
                            startRoundaboutIndex = -1,
                            volume = 0f,
                            width = 0f
                        };

                        for (int i = 0; i < subLanes.Length; i++)
                        {
                            Entity subLane = subLanes[i].m_SubLane;
                            if (prefabRefLookup.TryGetComponent(subLane, out PrefabRef subLanePrefab) && lanesEntityMap.TryGetValue(subLanePrefab.m_Prefab, out Domain.Lane laneType) && laneType.IsUtilityLane)
                            {
                                cable.category |= laneType.category;
                            }
                        }

                        switch (cableFlow.m_Flow)
                        {
                            case > 0:
                                cable.direction = Direction.Forward;
                                break;

                            case < 0:
                                cable.direction = Direction.Backward;
                                break;

                            default:
                                break;
                        }

                        if (hasComposition)
                        {
                            if (netCompositionDataLookup.TryGetComponent(networkComposition, out NetCompositionData netCompositiondata))
                            {
                                CompositionFlags.General generalCompositions = netCompositiondata.m_Flags.m_General;
                                if ((generalCompositions & CompositionFlags.General.Tunnel) != 0) cable.form = Form.Tunnel;
                                cable.width = netCompositiondata.m_Width;
                            }
                        }

                        list.AddNoResize(cable);
                    }
                }

                // TODO: Handle the integrated utility service networks.（處理整合公共管線網路。）
                /*
                //if (hasUtilityServiceNetworks && isNotUtilityServiceNetwork)
                //{
                //    Entity startNode = edge.m_Start;
                //    Entity endNode = edge.m_End;
                    
                //    // Generate the `NetworkStat` for the cable.（產生電纜的 `NetworkStat`。）
                //    if (electricityNodeConnectionLookup.TryGetComponent(network, out ElectricityNodeConnection electricityNodeCenter) &&
                //        electricityNodeConnectionLookup.TryGetComponent(startNode, out ElectricityNodeConnection electricityNodeStart) &&
                //        electricityNodeConnectionLookup.TryGetComponent(endNode, out ElectricityNodeConnection electricityNodeEnd) &&
                //        connectedFlowEdgeBufferLookup.TryGetBuffer(electricityNodeCenter.m_ElectricityNode, out DynamicBuffer<ConnectedFlowEdge> electricityFlows) &&
                //        electricityFlows.Length >= 2)
                //    {
                //        ElectricityFlowEdge electricityFlowFromStart = default;
                //        ElectricityFlowEdge electricityFlowToEnd = default;

                //        for (int i = 0; i < electricityFlows.Length; i++)
                //        {
                //            if (TryGetElectricityFlowEdge(electricityNodeStart.m_ElectricityNode,
                //                                          electricityNodeCenter.m_ElectricityNode,
                //                                          ref connectedFlowEdgeBufferLookup,
                //                                          ref electricityFlowEdgeLookup,
                //                                          out ElectricityFlowEdge electricityFlowA))
                //            {
                //                electricityFlowFromStart = electricityFlowA;
                //            }
                //            if (TryGetElectricityFlowEdge(electricityNodeCenter.m_ElectricityNode,
                //                                          electricityNodeEnd.m_ElectricityNode,
                //                                          ref connectedFlowEdgeBufferLookup,
                //                                          ref electricityFlowEdgeLookup,
                //                                          out ElectricityFlowEdge electricityFlowB))
                //            {
                //                electricityFlowToEnd = electricityFlowB;
                //            }
                //        }

                //        FixedList4096Bytes<float> curvePositions = default;

                //        if ((electricityFlowFromStart.m_Start == electricityNodeStart.m_ElectricityNode) &&
                //            (electricityFlowToEnd.m_End == electricityNodeEnd.m_ElectricityNode) &&
                //            connectedBuildingBufferLookup.TryGetBuffer(network, out DynamicBuffer<ConnectedBuilding> connectedBuildings))
                //        {
                //            for (int i = 0; i < connectedBuildings.Length; i++)
                //            {
                //                Entity buildingEntity = connectedBuildings[i].m_Building;
                //                if ((!buildingLookup.TryGetComponent(buildingEntity, out Building buildingComponent) &&
                //                     !electricityConsumerLookup.TryGetComponent(buildingEntity, out ElectricityConsumer electricityConsumer)) ||
                //                    placeholderLookup.TryGetComponent(buildingEntity, out _))
                //                {
                //                    continue;
                //                }

                //                curvePositions.Add(buildingComponent.m_CurvePosition);
                //            }
                //        }
                //    }
                //}
                */
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
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

            [ReadOnly]
            public ComponentLookup<NetGeometryData> netGeometryDataLookup;

            [ReadOnly]
            public ComponentLookup<ObjectGeometryData> objectGeometryDataLookup;

            [ReadOnly]
            public ComponentLookup<PlaceableObjectData> placeableObjectDataLookup;

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
            public NativeParallelHashMap<Entity, Entity> attachmentEntityMap;

            [WriteOnly]
            public NativeList<Domain.Roundabout>.ParallelWriter list;

            [WriteOnly]
            public NativeList<NetworkStat>.ParallelWriter stats;

            public void Execute(in Game.Net.Node node, in PrefabRef prefabRef, in Game.Net.Roundabout roundaboutComponent, in DynamicBuffer<ConnectedEdge> connectedEdges, in DynamicBuffer<Game.Objects.SubObject> subObjects, Entity entity)
            {
                bool allTrackConnection = true;
                bool allTunnelConnection = true;
                bool highwayConnection = false;
                float innerRadius = 0f;
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
                    width = 0f
                };

                // Reference（參考資料）: `Game.Net.GeometrySystem.CalculateEdgeGeometryJob.CalculateMiddleRadius`
                for (int i = 0; i < subObjects.Length; i++)
                {
                    Entity subObject = subObjects[i].m_SubObject;
                    if (placeableObjectDataLookup.TryGetComponent(subObject, out PlaceableObjectData placeableObjectData) &&
                        ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.RoadNode) != 0) &&
                        objectGeometryDataLookup.TryGetComponent(subObject, out ObjectGeometryData objectGeometryData))
                    {
                        float subObjectRadius = math.cmax(objectGeometryData.m_Size.xz) / 2f;

                        if (((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != 0) &&
                            netGeometryDataLookup.TryGetComponent(prefabRef.m_Prefab, out NetGeometryData netGeometryData))
                        {
                            float lyingRadius = math.cmax(objectGeometryData.m_LegSize.xz) / 2f;

                            if (netGeometryData.m_DefaultHeightRange.max > objectGeometryData.m_LegSize.y)
                            {
                                subObjectRadius = math.max(subObjectRadius, lyingRadius);
                            }
                            else
                            {
                                subObjectRadius = lyingRadius;
                            }
                        }

                        innerRadius = math.max(innerRadius, subObjectRadius);
                    }
                }

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

                        if (roadCompositionLookup.TryGetComponent(connectedComposition.m_Edge, out RoadComposition roadComposition))
                        {
                            allTrackConnection = false;
                            highwayConnection |= (roadComposition.m_Flags & Game.Prefabs.RoadFlags.UseHighwayRules) != 0;
                            roadLimit = math.max(roadLimit, RoundSpeedLimit(roadComposition.m_SpeedLimit));
                        }

                        if (trackCompositionLookup.TryGetComponent(connectedComposition.m_Edge, out TrackComposition trackComposition))
                        {
                            trackLimit = math.max(trackLimit, RoundSpeedLimit(trackComposition.m_SpeedLimit));
                        }
                    }
                }

                roundabout.innerRingRadius = innerRadius > 0 ? innerRadius : roundabout.outerRingRadius - width;
                roundabout.width = width;

                list.AddNoResize(roundabout);

                if (!hasCenterline) return;

                // Append roundabout's statistics.（添加圓環的統計資訊。）
                NetworkStat stat = new()
                {
                    entity = entity,
                    capacity = 0f,
                    category = highwayConnection ? NetworkCategory.Highway : NetworkCategory.Car,
                    direction = leftHandTraffic ? Direction.Forward : Direction.Backward,
                    discharge = 0f,
                    elevation = node.m_Position.y,
                    end = 1f,
                    endRoundaboutIndex = -1,
                    form = allTunnelConnection ? Form.Tunnel : form,
                    isRoundabout = true,
                    length = 2 * math.PI * (roundabout.outerRingRadius - width / 2),
                    limit = allTrackConnection ? trackLimit : roadLimit,
                    load = 0f,
                    start = 0f,
                    startRoundaboutIndex = -1,
                    volume = roadLookup.TryGetComponent(entity, out Road road) ? math.max(0f, GetVolume(road)) : 0f,
                    width = width
                };

                stats.AddNoResize(stat);
            }
        }

        /// <summary>
        /// The job to count the number of segments required to represent the individual (NOT included in the roads) utility service networks.
        /// （計算表達獨立公共服務管線（未包含在道路中的）所需路段數量的工作。）
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
        /// （計算表達整合公共服務管線（包含在道路中的）所需路段數量的工作。）
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

        /// <summary>
        /// The job to map roundabout attachements to a native hashmap.
        /// （將圓環附件映射至原生映射表的工作。）
        /// </summary>
        public partial struct MapRoundaboutAttachmentsJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, Entity>.ParallelWriter entityMap;

            public void Execute(in Attached attached, Entity attachment)
            {
                entityMap.TryAdd(attached.m_Parent, attachment);
            }
        }

        /// <summary>
        /// The job to map roundabout node instance to theor index in <see cref="_roundabouts"/>.
        /// （將圓環節點個體映射至在 <see cref="_roundabouts"/> 的索引的工作。）
        /// </summary>
        public partial struct MapRoundaboutsIndexJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeList<Domain.Roundabout> list;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int> map;

            public void Execute(int index)
            {
                map.TryAdd(list[index].node, index);
            }
        }
    }
}