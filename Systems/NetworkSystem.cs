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
using System.Collections.Generic;
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
        /// The query to collect all utility service network entities.
        /// （收集所有公用事業管線網路實體的查詢。）
        /// </summary>
        //static EntityQuery _utilityServiceNetworkQuery;

        /// <summary>
        /// The query for networks.（網路的查詢。）
        /// </summary>
        static EntityQueryDesc _networkEntityQueryDesc;

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
                    ComponentType.ReadOnly<Pillar>(),
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
            Utils.CommonUtils.Dispose(ref _localNetworkStats);
            Utils.CommonUtils.Dispose(ref _roundaboutEntityMap);
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
            NativeParallelHashSet<Entity> rbNetworks = default; // Handle by GetRbNetworks()

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset(ref roundaboutEntityMap, roundaboutCount);
            Utils.CommonUtils.Reset(ref stats, networkCount);

            try
            {
                // Retrieve the set of Road Builder networks.
                // （獲得 Road Builder 製作的網路集合。）
                GetRbNetworks(ref rbNetworks);

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
                GetRoundabouts(hasCenterline, ref stats, ref roundaboutEntityMap);

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
                    waterPipeNodeConnectionLookup = GetComponentLookup<WaterPipeNodeConnection>(true),
                    waterwayCompositionLookup = GetComponentLookup<WaterwayComposition>(true),
                    feature = featureFlag,
                    laneEntityMap = laneEntityMap,
                    roundaboutEntityMap = roundaboutEntityMap,
                    rbNetworks = rbNetworks,
                    list = stats.AsParallelWriter()
                };
                JobHandle collectNetworksHandle = collectNetworksJob.ScheduleParallel(_networkQuery, default);
                collectNetworksHandle.Complete();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref laneEntityMap);
                Utils.CommonUtils.Dispose(ref rbNetworks);
            }
        } 

        /// <summary>
        /// Retrieve the networks created by Road Builder - these networks require extra care.<br/>
        /// （獲得由 Road Builder 製作的道路－這些道路需要額外照料。）
        /// </summary>
        /// <param name="rbNetworks">The set of Road Builder network entities.（Road Builder 網路實體的集合。）</param>
        private void GetRbNetworks(ref NativeParallelHashSet<Entity> rbNetworks)
        {
            if (Instance.Rb.TryGet(false) && Instance.Rb.TryGetRbNetworkComponentType(out ComponentType roadBuilderNetworkComponent))
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
                JobHandle collectRbNetworksHandle = collectRbNetworksJob.Schedule(rbNetworkQuery, default);
                collectRbNetworksHandle.Complete();
            }
            else
            {
                Utils.CommonUtils.Reset(ref rbNetworks, 1, Allocator.Persistent);
            }
        }

        /// <summary>
        /// Retrieve the roundabout information.
        /// （獲得圓環資訊。）
        /// </summary>
        /// <param name="hasCenterline">Whether the export options has the geometry Centerline.（輸出設定是否含有中心線幾何？）</param>
        /// <param name="stats">The list of network statistics.（網路統計資訊的列表。）</param>
        /// <param name="roundaboutEntityMap">The map between roundabout node and its statistics.（圓環節點與統計資訊的映射表。）</param>
        private void GetRoundabouts(bool hasCenterline, ref NativeList<NetworkStat> stats, ref NativeParallelHashMap<Entity, Domain.Roundabout> roundaboutEntityMap)
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
                compositionLookup = GetComponentLookup<Composition>(true),
                netCompositionDataLookup = GetComponentLookup<NetCompositionData>(true),
                prefabRefLookup = GetComponentLookup<PrefabRef>(true),
                roadLookup = GetComponentLookup<Road>(true),
                roadCompositionLookup = GetComponentLookup<RoadComposition>(true),
                subwayTrackLookup = GetComponentLookup<SubwayTrack>(true),
                trackCompositionLookup = GetComponentLookup<TrackComposition>(true),
                trainTrackLookup = GetComponentLookup<TrainTrack>(true),
                tramTrackLookup = GetComponentLookup<TramTrack>(true),
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
            public NativeParallelHashSet<Entity> rbNetworks;

            [WriteOnly]
            public NativeList<NetworkStat>.ParallelWriter list;

            public void Execute(in Composition composition, in Curve curve, in Edge edge, in EdgeGeometry edgeGeometry, in PrefabRef prefabRef, Entity network)
            {
                NetworkStat stat = new()
                {
                    entity = network,
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
                    range = new(0f, 1f),
                    start = edge.m_Start,
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

                    if (rbNetworks.Contains(network))
                    {
                        stat.category |= NetworkCategory.RoadBuilder;
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
            public ComponentLookup<Composition> compositionLookup;

            [ReadOnly]
            public ComponentLookup<NetCompositionData> netCompositionDataLookup;

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
                int maxLaneCount = 0;
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

                        if (hasCenterline && netCompositionLaneBufferLookup.TryGetBuffer(composition, out DynamicBuffer<NetCompositionLane> lanes))
                        {
                            int laneCount = 0;
                            for (int j = 0; j < lanes.Length; j++)
                            {
                                NetCompositionLane lane = lanes[j];
                                if (((lane.m_Flags & LaneFlags.Road) != 0) & ((lane.m_Flags & LaneFlags.Master) == 0)) laneCount++;
                            }
                            
                            maxLaneCount = math.max(maxLaneCount, laneCount);
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
                    aggregation = roundabout.attached,
                    capacity = 0f,
                    category = highwayConnection ? NetworkCategory.Highway : NetworkCategory.Car,
                    direction = leftHandTraffic ? Direction.Forward : Direction.Backward,
                    discharge = 0f,
                    elevation = node.m_Position.y,
                    end = Entity.Null,
                    form = allTunnelConnection ? Form.Tunnel : form,
                    isRoundabout = true,
                    lane = maxLaneCount,
                    length = 2 * math.PI * ((roundabout.outerRingRadius - roundabout.innerRingRadius) / 2 + roundabout.innerRingRadius),
                    limit = allTrackConnection ? trackLimit : roadLimit,
                    load = 0f,
                    range = new(0f, 1f),
                    roundabout = new(false, false),
                    start = Entity.Null,
                    volume = roadLookup.TryGetComponent(entity, out Road road) ? math.max(0f, GetVolume(road)) : 0f,
                    width = width
                };

                stats.AddNoResize(stat);
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

                if (isCircular && (size.x == size.z))
                {
                    radius = size.x / 2f;
                }
                if (isCircularLeg && (legSize.x == legSize.z))
                {
                    if (isCircular)
                    {
                        radius = math.min(radius, legSize.x / 2f);
                    }
                    else
                    {
                        radius = legSize.x / 2f;
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
                entityMap.TryAdd(attached.m_Parent, attachment);
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