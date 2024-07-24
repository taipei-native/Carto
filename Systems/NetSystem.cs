namespace Carto.Systems
{
    using Carto.Geodata;
    using Carto.Utils;
    using Colossal.Logging;
    using Colossal.Mathematics;
    using Game;
    using Game.Buildings;
    using Game.Common;
    using Game.Net;
    using Game.Objects;
    using Game.Prefabs;
    using Game.Tools;
    using Game.UI;
    using Purpose = Colossal.Serialization.Entities.Purpose;
    using System;
    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using System.Linq;

    /// <summary>
    /// The system instance that manages the network properties, inheriting from GameSystemBase.
    /// （管理網絡屬性的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class NetSystem : GameSystemBase
    {
        private readonly ILog m_Log = Instance.Log;
        private readonly NameSystem m_Name = Instance.Name;
        private readonly PrefabSystem m_Prefab = Instance.Prefab;

        // The entity query instance that searches for several instances, using Unity.Entities.EntityQuery.
        // （用於搜尋數種實例的Unity實體查詢實例。）
        private EntityQuery _pathwayQuery;
        private EntityQuery _roadQuery;
        private EntityQuery _roundaboutAssetQuery;
        private EntityQuery _roundaboutQuery;
        private EntityQuery _trackQuery;

        /// <summary>
        /// The network category, which is a slightly different version of `Game.Net.Layer`.
        /// （網絡的分類，是與 `Game.Net.Layer` 稍微不同的版本。）
        /// </summary>
        public static Dictionary<string, string> Category = new Dictionary<string, string>
        {
            { "RoadsSmallRoads", "Small" }, { "RoadsMediumRoads", "Medium" }, { "RoadsLargeRoads", "Large" }, { "RoadsHighways", "Highway" },
            { "TrainTracks", "Train" }, { "SubwayTracks", "Subway" }, { "TramTracks", "Tram" },
            { "Pathway", "Pathway" }
            //Taxiway, Waterway, Fence, WaterPipe, SewagePipe, PowerHigh, PowerLow
        };

        /// <summary>
        /// The dictionary that holds all nodes with roundabouts and the attached roundabout upgrades.
        /// （記載所有包含圓環（島）的節點與其圓環島升級的字典。）
        /// </summary>
        public static Dictionary<Entity, Entity> RoundaboutAttachments = new Dictionary<Entity, Entity>();

        /// <summary>
        /// The dictionary that holds all nodes with roundabouts and the vertices that form the roundabouts' centerline.
        /// （記載所有包含圓環（島）的節點與組成其中心線頂點的字典。）
        /// </summary>
        public static Dictionary<Entity, List<float3>> RoundaboutCenterVertices = new Dictionary<Entity, List<float3>>();

        /// <summary>
        /// The dictionary that holds all nodes with roundabouts and the vertices that form the roundabout's inner edge.
        /// （記載所有包含圓環（島）的節點與組成其內緣的字典。）
        /// </summary>
        public static Dictionary<Entity, Dictionary<Entity, List<float3>>> RoundaboutInnerVertices = new Dictionary<Entity, Dictionary<Entity, List<float3>>>();

        /// <summary>
        /// The dictionary that holds all nodes with roundabouts and the vertices that form the roundabout's left outer edge.
        /// （記載所有包含圓環（島）的節點與組成其左側外緣的字典。）
        /// </summary>
        public static Dictionary<Entity, Dictionary<Entity, List<float3>>> RoundaboutOuterLeftVertices = new Dictionary<Entity, Dictionary<Entity, List<float3>>>();

        /// <summary>
        /// The dictionary that holds all nodes with roundabouts and the vertices that form the roundabout's right outer edge.
        /// （記載所有包含圓環（島）的節點與組成其右側外緣的字典。）
        /// </summary>
        public static Dictionary<Entity, Dictionary<Entity, List<float3>>> RoundaboutOuterRightVertices = new Dictionary<Entity, Dictionary<Entity, List<float3>>>();

        /// <summary>
        /// The dictionary that holds all tram track segments which extend to the center of the roundabouts.
        /// （記載所有延伸至圓環中心的電車軌道。）
        /// </summary>
        public static List<Dictionary<Entity, List<float3>>> RoundaboutTramTracks = new List<Dictionary<Entity, List<float3>>>();

        /// <summary>
        /// The dictionary that holds all nodes with roundabouts and the width of the roundabout.
        /// （記載所有包含圓環（島）的節點與其寬度的字典。）
        /// </summary>
        public static Dictionary<Entity, float> RoundaboutWidth = new Dictionary<Entity, float>();


        /// <summary>
        /// This event triggers when the system is created.
        /// （這是當系統實例被創造時，會被觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _pathwayQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Aggregated>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Road>(),
                    ComponentType.ReadOnly<TrainTrack>(),
                    ComponentType.ReadOnly<SubwayTrack>(),
                    ComponentType.ReadOnly<TramTrack>(),
                    ComponentType.ReadOnly<Waterway>(),
                    ComponentType.ReadOnly<Game.Net.Marker>(),

                    ComponentType.ReadOnly<Node>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            
            _roadQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<Road>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Node>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _roundaboutAssetQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Attached>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Pillar>(),
                    ComponentType.ReadOnly<Game.Routes.TransportStop>(),
                    ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
                    ComponentType.ReadOnly<ExtractorProperty>(),

                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _roundaboutQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Roundabout>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _trackQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<TrainTrack>(),
                    ComponentType.ReadOnly<SubwayTrack>(),
                    ComponentType.ReadOnly<TramTrack>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Node>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            base.OnCreate();
            m_Log.Debug("NetSystem instance created. 網絡系統實例創造完成。");
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
        /// The method to get network's centerlines.
        /// （獲得網絡中心線的方法。）
        /// </summary>
        /// <param name="net">Input network entity.（輸入的網絡實體。）</param>
        /// <param name="em">The entity manager.（實體管理器。）</param>
        /// <param name="gap">The gap between each interpolated points.（各個內插點之間的間隔。）</param>
        /// <param name="threshold">The maximum acceptable distance between the true curve and interpolated lines.（實際曲線和內插線段之間最大的可容許距離。）</param>
        /// <param name="nodeGap">The interpolation gap near the both end of the network.（網絡兩端附近的內插間隔。）</param>
        /// <param name="nodeThreshold">The interpolation threshold near the both end of the network.（網絡兩端附近的內插閾值。）</param>
        public static List<float3> GetNetCenterLine(Entity net, EntityManager em, float gap = 4, float threshold = 2, float nodeGap = 1, float nodeThreshold = 0.5f)
        {
            Bezier4x3 mainCurve  = em.GetComponentData<Curve>(net).m_Bezier;
            Bezier4x3 startCurve = em.GetComponentData<StartNodeGeometry>(net).m_Geometry.m_Middle;
            Bezier4x3 endCurve   = em.GetComponentData<EndNodeGeometry>(net).m_Geometry.m_Middle;
            Bezier4x3 newStartCurve;
            Bezier4x3 newEndCurve;
            bool useStart = MathUtils.Length(startCurve) > 0;
            bool useEnd = MathUtils.Length(endCurve) > 0;
            bool roundaboutStart = false;
            bool roundaboutEnd   = false;
            CompositionFlags.General startFlag = em.GetComponentData<NetCompositionData>(em.GetComponentData<Composition>(net).m_StartNode).m_Flags.m_General;
            CompositionFlags.General endFlag   = em.GetComponentData<NetCompositionData>(em.GetComponentData<Composition>(net).m_EndNode).m_Flags.m_General;
            Entity startNode = em.GetComponentData<Edge>(net).m_Start;
            Entity endNode   = em.GetComponentData<Edge>(net).m_End;
            float3 startLocation = em.GetComponentData<Node>(startNode).m_Position;
            float3 endLocation   = em.GetComponentData<Node>(endNode).m_Position;
            int nodeListStartIndex = 0;
            int nodeListEndIndex;
            List<float3> endCurveNodes   = new List<float3>();
            List<float3> startCurveNodes = new List<float3>();

            /// <summary>
            /// Handle the vertices of the network if there are roundabouts on either or both ends.
            /// （在網絡兩端之中含有圓環的情況下，處理網絡的頂點。）
            /// </summary>
            void HandleRoundabouts(Entity roundaboutNode, float3 position, List<float3> originalLine, bool useStartCriteria, out int originalLineIndex)
            {
                int intersectionIndex = 0;
                int _originalLineIndex = useStartCriteria ? 0 : originalLine.Count - 1;
                List<float3> intersections = RoundaboutIntersection(roundaboutNode, position, originalLine, out List<float3> roundaboutShape, out List<int> circleIndex, out List<int> lineIndex);

                if (intersections.Count > 1)
                {
                    // Under rare conditions, if there are multiple intersections, for start, use the last one; for end, use the first one.
                    // （在罕見情況下，假如有複數個交點，對於起點將使用最後一個交點、對於終點將使用第一個交點。）
                    intersectionIndex = useStartCriteria ? MiscUtils.GetIndexOfMaximum(lineIndex) : MiscUtils.GetIndexOfMinimum(lineIndex);
                }

                if (intersections.Count >= 1)
                {
                    _originalLineIndex = lineIndex[intersectionIndex];
                    originalLine.Insert(_originalLineIndex, intersections[intersectionIndex]);
                    roundaboutShape.Insert(circleIndex[intersectionIndex], intersections[intersectionIndex]);
                }

                originalLineIndex = _originalLineIndex;
                RoundaboutCenterVertices[roundaboutNode] = roundaboutShape;
            }

            /// <summary>
            /// Finds all intersection between the centerline and the roundabout.
            ///（找出中心線上所有和圓環相交的點。）
            /// </summary>
            List<float3> RoundaboutIntersection(Entity roundaboutNode, float3 position, List<float3> originalLine, out List<float3> roundaboutShape, out List<int> circleIndex, out List<int> lineIndex)
            {
                List<float3> _roundaboutShape;

                if (RoundaboutCenterVertices.TryGetValue(roundaboutNode, out List<float3> v))
                {
                    _roundaboutShape = v;
                }
                else
                {
                    // Handle the roundabouts generated from placing pillars over nodes.
                    // （處理由於在節點上方放置柱子而形成的圓環。）
                    Bounds3 roundaboutNodeBound = em.GetComponentData<NodeGeometry>(roundaboutNode).m_Bounds;
                    float roundaboutMaxWidth = 0;
                    float roundaboutRadius = ((roundaboutNodeBound.max.z - roundaboutNodeBound.min.z) / 4 + (roundaboutNodeBound.max.x - roundaboutNodeBound.min.x) / 4) / 2;
                    Circle3 roundaboutCircle = new Circle3(roundaboutRadius, position, new quaternion(0, 0, 0, 0));
                    _roundaboutShape = GeometryUtils.Interpolate(roundaboutCircle);
                    RoundaboutAttachments[roundaboutNode] = Entity.Null;

                    var roundaboutEdges = em.GetBuffer<ConnectedEdge>(roundaboutNode);
                    foreach (var cEdge in roundaboutEdges)
                    {
                        float tempWidth = em.GetComponentData<NetCompositionData>(em.GetComponentData<Composition>(cEdge.m_Edge).m_Edge).m_Width / 2;
                        if (tempWidth > roundaboutMaxWidth) roundaboutMaxWidth = tempWidth;
                    }
                    RoundaboutWidth[roundaboutNode] = roundaboutMaxWidth;
                }

                List<float3> intersection = GeometryUtils.Intersection(_roundaboutShape, originalLine, out List<int> _circleIndex, out List<int> _lineIndex);
                circleIndex = _circleIndex;
                lineIndex = _lineIndex;
                roundaboutShape = _roundaboutShape;
                return intersection;
            }

            // Detect whether the node is a roundabout. If yes, the start / end location will be cut back to make room for the roundabout's centerline.
            // （偵測節點是否為圓環。如果是圓環的話，起訖點將會退縮以提供圓環中心線空間。）
            // Worth to notice, tram tracks never go around the roundabouts but go through the centers instead, so they should be handled differently.
            // （值得注意的是，電車軌道並不會沿著圓環鋪設，而是直接穿過圓環中心，因此需要另外處理它們。）
            if (startFlag.HasFlag(CompositionFlags.General.Roundabout)) roundaboutStart = true;
            if (endFlag.HasFlag(CompositionFlags.General.Roundabout))   roundaboutEnd   = true;

            // Detect whether the curve is reversed. A normal curve should be arranged in the order of "start.a → start.d → (main) → end.a → end.d".
            // （偵測曲線是否被反轉。一個正常的曲線應該以「start.a → start.d → (main) → end.a → end.d」的順序排列）
            _ = GeometryUtils.SameDirection(mainCurve, startCurve, out float tStart, true);
            _ = GeometryUtils.SameDirection(mainCurve, endCurve, out float tEnd, false);

            // Trim the excessive parts of the main curve.
            // （裁剪主曲線的多餘部分。）
            Bezier4x3 trimmedMainCurve = GeometryUtils.Trim(mainCurve, useStart ? tStart : 0, useEnd ? tEnd : 1);

            // Extend the trimmed main curve to the start & end nodes.
            // （將裁剪後的主曲線延伸至起訖節點。）
            if (useStart)
            {
                Line3.Segment startVector = new Line3.Segment(startLocation, trimmedMainCurve.a);
                Line3.Segment startReverseVector = GeometryUtils.StartReflect(new Line3.Segment(trimmedMainCurve.a, trimmedMainCurve.b));
                float3 newStartCurveC = GeometryUtils.StartTrim(startReverseVector, math.distance(startLocation, MathUtils.Position(startVector, 0.33f))).b;
                newStartCurve = new Bezier4x3(startLocation, startLocation, newStartCurveC, trimmedMainCurve.a);
                startCurveNodes = GeometryUtils.Interpolate(newStartCurve, nodeGap, nodeThreshold);
            }

            if (useEnd)
            {
                Line3.Segment endVector = new Line3.Segment(trimmedMainCurve.d, endLocation);
                Line3.Segment endReverseVector = GeometryUtils.StartReflect(new Line3.Segment(trimmedMainCurve.d, trimmedMainCurve.c));
                float3 newEndCurveB = GeometryUtils.StartTrim(endReverseVector, math.distance(endLocation, MathUtils.Position(endVector, 0.33f))).b;
                newEndCurve = new Bezier4x3(trimmedMainCurve.d, newEndCurveB, endLocation, endLocation);
                endCurveNodes = GeometryUtils.Interpolate(newEndCurve, nodeGap, nodeThreshold);
            }

            // Interpolate and combine the curve.
            // （內插及結合曲線。）
            List<float3> nodeList = new List<float3>();
            List<float3> mainCurveNodes = GeometryUtils.Interpolate(trimmedMainCurve, gap, threshold);
            if (useStart) nodeList.AddRange(startCurveNodes);
            nodeList.AddRange(mainCurveNodes.GetRange(useStart ? 1 : 0, mainCurveNodes.Count - 1));
            if (useEnd) nodeList.AddRange(endCurveNodes.GetRange(1, endCurveNodes.Count - 1));

            // Handle roundabouts.
            // （處理圓環。）
            if (roundaboutStart)
            {
                HandleRoundabouts(startNode, startLocation, nodeList, true, out int _nodeStartIndex);
                nodeListStartIndex = _nodeStartIndex;

                if (em.HasComponent<TramTrack>(net))
                {
                    RoundaboutTramTracks.Add(new Dictionary<Entity, List<float3>> { { net, nodeList.GetRange(0, nodeListStartIndex + 1) } });
                }
            }

            nodeListEndIndex = nodeList.Count - 1;

            if (roundaboutEnd)
            {
                HandleRoundabouts(endNode, endLocation, nodeList, true, out int _nodeEndIndex);
                nodeListEndIndex = _nodeEndIndex;

                if (em.HasComponent<TramTrack>(net))
                {
                    RoundaboutTramTracks.Add(new Dictionary<Entity, List<float3>> { { net, nodeList.GetRange(nodeListEndIndex, nodeList.Count - nodeListEndIndex) } });
                }
            }

            nodeList = nodeList.GetRange(nodeListStartIndex, nodeListEndIndex - nodeListStartIndex + 1);
            return nodeList;
        }

        public static List<float3> GetNetEdge(Entity net, EntityManager em, float gap = 4, float threshold = 2, float nodeGap = 1, float nodeThreshold = 0.1f)
        {
            EdgeGeometry mainCurves = em.GetComponentData<EdgeGeometry>(net);
            Bezier4x3 mainCurveLeftStart  = mainCurves.m_Start.m_Left;
            Bezier4x3 mainCurveLeftEnd    = mainCurves.m_End.m_Left;
            Bezier4x3 mainCurveRightStart = mainCurves.m_Start.m_Right;
            Bezier4x3 mainCurveRightEnd   = mainCurves.m_End.m_Right;
            StartNodeGeometry startCurves = em.GetComponentData<StartNodeGeometry>(net);
            Bezier4x3 startCurveLeft  = startCurves.m_Geometry.m_Left.m_Left;
            Bezier4x3 startCurveRight = startCurves.m_Geometry.m_Right.m_Right;
            EndNodeGeometry endCurves = em.GetComponentData<EndNodeGeometry>(net);
            Bezier4x3 endCurveLeft  = endCurves.m_Geometry.m_Left.m_Left;
            Bezier4x3 endCurveRight = endCurves.m_Geometry.m_Right.m_Right;
            Entity startNode = em.GetComponentData<Edge>(net).m_Start;
            Entity endNode   = em.GetComponentData<Edge>(net).m_End;
            float3 startLocation = em.GetComponentData<Node>(startNode).m_Position;
            float3 endLocation   = em.GetComponentData<Node>(endNode).m_Position;
            Entity startComposition = em.GetComponentData<Composition>(net).m_StartNode;
            Entity endComposition   = em.GetComponentData<Composition>(net).m_EndNode;
            bool startRoundabout = em.GetComponentData<NetCompositionData>(startComposition).m_Flags.m_General.HasFlag(CompositionFlags.General.Roundabout);
            bool endRoundabout   = em.GetComponentData<NetCompositionData>(endComposition).m_Flags.m_General.HasFlag(CompositionFlags.General.Roundabout);
            bool startNodeTerminus = false;
            bool endNodeTerminus   = false;
            int startNodeRoadCount = 0;
            int endNodeRoadCount   = 0;
            List<float3> nodeList = new List<float3>();

            foreach (ConnectedEdge startNodeEdge in em.GetBuffer<ConnectedEdge>(startNode))
            {
                if (em.HasComponent<Road>(startNodeEdge.m_Edge) || em.HasComponent<TrainTrack>(startNodeEdge.m_Edge) || em.HasComponent<SubwayTrack>(startNodeEdge.m_Edge) || em.HasComponent<TramTrack>(startNodeEdge.m_Edge))
                {
                    startNodeRoadCount ++;
                }
            }

            foreach (ConnectedEdge endNodeEdge in em.GetBuffer<ConnectedEdge>(endNode))
            {
                if (em.HasComponent<Road>(endNodeEdge.m_Edge) || em.HasComponent<TrainTrack>(endNodeEdge.m_Edge) || em.HasComponent<SubwayTrack>(endNodeEdge.m_Edge) || em.HasComponent<TramTrack>(endNodeEdge.m_Edge))
                {
                    endNodeRoadCount ++;
                }
            }

            if (startNodeRoadCount == 1) startNodeTerminus = true;
            if (endNodeRoadCount == 1) endNodeTerminus = true;

            // The road segment can be obtained by sketching along the curves in the order of:
            // （道路區段的邊界可以沿著以下順序描繪獲得：）
            //     (inverted) startCurveRight → mainCurveLeft → endCurveLeft → (inverted) endCurveRight → (inverted) mainCurveRight → startCurveLeft → (loop complete)
            List<float3> segmentA = GeometryUtils.Interpolate(MathUtils.Invert(startCurveRight), nodeGap, nodeThreshold);
            List<float3> segmentB = GeometryUtils.Interpolate(mainCurveLeftStart, gap, threshold);
            List<float3> segmentC = GeometryUtils.Interpolate(mainCurveLeftEnd, gap, threshold);
            List<float3> segmentD = GeometryUtils.Interpolate(endCurveLeft, nodeGap, nodeThreshold);
            List<float3> segmentE = GeometryUtils.Interpolate(MathUtils.Invert(endCurveRight), nodeGap, nodeThreshold);
            List<float3> segmentF = GeometryUtils.Interpolate(MathUtils.Invert(mainCurveRightEnd), gap, threshold);
            List<float3> segmentG = GeometryUtils.Interpolate(MathUtils.Invert(mainCurveRightStart), gap, threshold);
            List<float3> segmentH = GeometryUtils.Interpolate(startCurveLeft, nodeGap, nodeThreshold);

            nodeList.AddRange(segmentB.GetRange(0, segmentB.Count - 1));
            nodeList.AddRange(segmentC.GetRange(0, segmentC.Count - 1));

            if (endRoundabout)
            {
                nodeList.AddRange(RoundaboutOuterLeftVertices[endNode][net]);
                nodeList.AddRange(RoundaboutInnerVertices[endNode][net]);
                List<float3> segmentD3 = RoundaboutOuterRightVertices[endNode][net];
                nodeList.AddRange(segmentD3.GetRange(0, segmentD3.Count - 1));
            }
            else
            {
                nodeList.AddRange(segmentD);
                if (!endNodeTerminus) nodeList.Add(endLocation);
                nodeList.AddRange(segmentE.GetRange(0, segmentE.Count - 1));
            }

            nodeList.AddRange(segmentF.GetRange(0, segmentF.Count - 1));
            nodeList.AddRange(segmentG.GetRange(0, segmentG.Count - 1));

            if (startRoundabout)
            {
                nodeList.AddRange(RoundaboutOuterRightVertices[startNode][net]);
                nodeList.AddRange(RoundaboutInnerVertices[startNode][net]);
                List<float3> segmentH3 = RoundaboutOuterLeftVertices[startNode][net];
                nodeList.AddRange(segmentH3.GetRange(0, segmentH3.Count - 1));
            }
            else
            {
                nodeList.AddRange(segmentH);
                if (!startNodeTerminus) nodeList.Add(startLocation);
                nodeList.AddRange(segmentA.GetRange(0, segmentA.Count - 1));
            }

            return nodeList;
        }

        /// <summary>
        /// Searching for properties of all existing pathways.
        /// （搜尋現有所有小徑的屬性。）
        /// </summary>
        public List<CartoObject> GetPathwayProperties()
        {
            List<CartoObject> pathList = new List<CartoObject>();

            foreach (Entity _path in _pathwayQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, string>();

                    // Retrieve the custom name for the pathway. Expected output: "Pathway"
                    // （獲取小徑的自訂名稱。預期輸出："Pathway"）
                    string pathName = string.Empty;

                    if (EntityManager.HasComponent<Aggregated>(_path))
                    {
                        pathName = m_Name.GetRenderedLabelName(EntityManager.GetComponentData<Aggregated>(_path).m_Aggregate);
                        if (LocaleUtils.TryTranslate(pathName, out string translated)) pathName = translated;
                    }

                    props["Name"] = pathName;

                    // Retrieve the assset name for the pathway. Expected output: "Three-Lane One-Way Highway"
                    // （獲取小徑的資產名稱。預期輸出："人行道"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Asset")) props["Asset"] = m_Name.GetRenderedLabelName(_path);

                    // Retrieve the category of the pathway. Expected output: "Pathway"
                    // （獲取小徑的分類。預期輸出："Pathway"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Category")) props["Category"] = Category["Pathway"];

                    // Retrieve the nodes forming the centerline of the pathway segment. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成小徑中心線的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "CenterLine"))
                    {
                        edges["CenterLine"] = GetNetCenterLine(_path, EntityManager, 1, 1);
                        type["CenterLine"] = CartoObject.L;
                    }

                    // Retrieve the pedestrian walking direction on the pathway. Expected output: "Both"
                    // （獲取行人在小徑上的步行方向。預期輸出："Both"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Direction")) props["Direction"] = "Both";

                    // Retrieve the edge of the pathway. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成小徑邊緣的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Edge"))
                    {
                        edges["Edge"] = GetNetEdge(_path, EntityManager, 3, 1);
                        type["Edge"] = CartoObject.P;
                    }

                    // Retrieve the form of the pathway. Expected output: "Normal"
                    // （獲取小徑的形式。預期輸出："Normal"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Form"))
                    {
                        Entity pathEdgeComposition = EntityManager.GetComponentData<Composition>(_path).m_Edge;
                        NetCompositionData pathCompositionData = EntityManager.GetComponentData<NetCompositionData>(pathEdgeComposition);
                        CompositionFlags.General pathFormFlag = pathCompositionData.m_Flags.m_General;
                        string pathForm = "Normal";
                        if (pathFormFlag.HasFlag(CompositionFlags.General.Elevated)) pathForm = "Elevated";
                        if (pathFormFlag.HasFlag(CompositionFlags.General.Tunnel)) pathForm = "Tunnel";
                        props["Form"] = pathForm;
                    }

                    // Retrieve the mean height of the pathway segment. Expected output: 145.881424
                    // （獲取小徑區段的平均高度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Height"))
                    {
                        Bounds1 pathHeightBounds = EntityManager.GetComponentData<EdgeGeometry>(_path).m_Bounds.y;
                        props["Height"] = (pathHeightBounds.max + pathHeightBounds.min) / 2;
                    }

                    // Retrieve the length of the pathway segment. Expected output: 145.881424
                    // （獲取小徑區段的長度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Length")) props["Length"] = EntityManager.GetComponentData<Curve>(_path).m_Length;

                    // Retrieve the width of the pathway segment. Expected output: 3
                    // （獲取小徑區段的寬度。預期輸出：3）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Width"))
                    {
                        Entity pathEdgeComposition = EntityManager.GetComponentData<Composition>(_path).m_Edge;
                        NetCompositionData pathCompositionData = EntityManager.GetComponentData<NetCompositionData>(pathEdgeComposition);
                        props["Width"] = pathCompositionData.m_Width;
                    }

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Object")) props["Object"] = "Pathway";

                    pathList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetPathwayProperties(); 於 GetPathwayProperties() 發生一個錯誤； {ex}");
                }
            }

            return pathList;
        }

        /// <summary>
        /// Searching for properties of all existing roads.
        /// （搜尋現有所有道路的屬性。）
        /// </summary>
        public List<CartoObject> GetRoadsProperties()
        { 
            List<CartoObject> roadList = new List<CartoObject>();
            RoundaboutAttachments.Clear();
            RoundaboutCenterVertices.Clear();
            RoundaboutInnerVertices.Clear();
            RoundaboutOuterLeftVertices.Clear();
            RoundaboutOuterRightVertices.Clear();
            RoundaboutTramTracks.Clear();
            RoundaboutWidth.Clear();

            foreach (Entity _roundaboutAsset in _roundaboutAssetQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    RoundaboutAttachments[EntityManager.GetComponentData<Attached>(_roundaboutAsset).m_Parent] = _roundaboutAsset;
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetRoadsProperties(); 於 GetRoadsProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _roundabout in _roundaboutQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    float maxWidth = 0f;
                    float roundaboutRadius = EntityManager.GetComponentData<Roundabout>(_roundabout).m_Radius;
                    float3 roundaboutLocation = EntityManager.GetComponentData<Node>(_roundabout).m_Position;
                    Circle3 roundaboutCircle = new Circle3(roundaboutRadius, roundaboutLocation, new quaternion(0, 0, 0, 0));
                    Dictionary<Entity, float> roundaboutAzimuths = new Dictionary<Entity, float>();
                    Dictionary<Entity, bool> connectToStart = new Dictionary<Entity, bool>();
                    RoundaboutInnerVertices[_roundabout] = new Dictionary<Entity, List<float3>>();
                    RoundaboutOuterLeftVertices[_roundabout] = new Dictionary<Entity, List<float3>>();
                    RoundaboutOuterRightVertices[_roundabout] = new Dictionary<Entity, List<float3>>();

                    foreach (ConnectedEdge roundaboutEdge in EntityManager.GetBuffer<ConnectedEdge>(_roundabout))
                    {
                        if (!EntityManager.HasComponent<Road>(roundaboutEdge.m_Edge) && !EntityManager.HasComponent<TrainTrack>(roundaboutEdge.m_Edge) && !EntityManager.HasComponent<SubwayTrack>(roundaboutEdge.m_Edge) && !EntityManager.HasComponent<TramTrack>(roundaboutEdge.m_Edge)) continue;
                        Entity roundaboutEdgeComposition = EntityManager.GetComponentData<Composition>(roundaboutEdge.m_Edge).m_Edge;
                        float roundaboutEdgeWidth = EntityManager.GetComponentData<NetCompositionData>(roundaboutEdgeComposition).m_Width;
                        if (roundaboutEdgeWidth / 2 > maxWidth) maxWidth = roundaboutEdgeWidth / 2;

                        if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Edge"))
                        {
                            Entity roundaboutEdgeStartNode = EntityManager.GetComponentData<Edge>(roundaboutEdge.m_Edge).m_Start;
                            connectToStart[roundaboutEdge.m_Edge] = roundaboutEdgeStartNode.Equals(_roundabout);
                            Bezier4x3 roundaboutEdgeBezier = EntityManager.GetComponentData<Curve>(roundaboutEdge.m_Edge).m_Bezier;
                            Bezier4x2 roundaboutEdgeBezier2D = roundaboutEdgeBezier.xz;
                            float2 roundaboutEdgeTangent = connectToStart[roundaboutEdge.m_Edge] ? MathUtils.StartTangent(roundaboutEdgeBezier2D) : -MathUtils.EndTangent(roundaboutEdgeBezier2D);
                            roundaboutAzimuths[roundaboutEdge.m_Edge] = GeometryUtils.Azimuth(roundaboutEdgeTangent);
                        }
                    }

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Edge"))
                    {
                        Entity[] roundaboutEdgesSorted = roundaboutAzimuths.OrderBy(kvp => kvp.Value).Select(kvp => kvp.Key).ToArray();

                        for (int i = 0; i < roundaboutEdgesSorted.Length; i++)
                        {
                            Entity edge = roundaboutEdgesSorted[i];
                            EdgeGeometry edgeGeometry = EntityManager.GetComponentData<EdgeGeometry>(edge);
                            Bezier4x3 leftEdgeEnd = edgeGeometry.m_End.m_Left;
                            Bezier4x3 leftEdgeStart = edgeGeometry.m_Start.m_Left;
                            Bezier4x3 rightEdgeEnd = edgeGeometry.m_End.m_Right;
                            Bezier4x3 rightEdgeStart = edgeGeometry.m_Start.m_Right;
                            float centerAzimuth = roundaboutAzimuths[edge];
                            float deviation = math.PI / 6;              // The value of maximum acceptable deviation between `left/rightFixAzimuth` and `left/rightIntersectionAzimuth`.（`left/rightFixAzimuth`與`left/rightIntersectionAzimuth`之間最大可容許差值。） 
                            float leftAzimuth = 0;                      // The azimuth angle of the leftest boundary.（最左側邊界的方位角。）
                            float leftConnectionAzimuth = 0;            // The azimuth angle of `leftConnection`.（`leftConnection`的方位角。）
                            float leftIntersectionAzimuth = 0;          // The azimuth angle of `leftIntersection`.（`leftIntersection`的方位角。）
                            float leftTangentAzimuth = 0;               // The azimuth angle of `leftTangent`.（`leftTangent`的方位角。）
                            float rightAzimuth = 0;                     // The azimuth angle of the rightest boundary.（最右側邊界的方位角。）
                            float rightConnectionAzimuth = 0;           // The azimuth angle of `rightConnection`.（`rightConnection`的方位角。）
                            float rightIntersectionAzimuth = 0;         // The azimuth angle of `rightIntersection`.（`rightIntersection`的方位角。）
                            float rightTangentAzimuth = 0;              // The azimuth angle of `rightTangent`.（`rightTangent`的方位角。）
                            float2 leftControlPoint2D = default;        // The position of `leftCurve`'s control point on a plane.（`leftCurve`在平面上的控制點位置。）
                            float2 rightControlPoint2D = default;       // The position of `rightCurve`'s control point on a plane.（`rightCurve`在平面上的控制點位置。）
                            float3 leftConnection = default;            // The position where `leftArc` intersects `leftCurve`.（`leftArc`與`leftCurve`相交的位置。）
                            float3 leftControlPoint = default;          // The position of `leftCurve`'s control point.（`leftCurve`的控制點位置。）
                            float3 leftEdgeEndpoint = default;          // The position where `edge`'s left boundary intersects `leftCurve`.（`edge`的左側邊界與`leftCurve`相交的位置。）
                            float3 leftIntersection = default;          // The position where `leftConnectionTangent` intersects `leftEdgeExtension`.（`leftConnectionTangent`與`leftEdgeExtension`相交的位置。）
                            float3 rightConnection = default;           // The position where `rightArc` intersects `rightCurve`.（`rightArc`與`rightCurve`相交的位置。）
                            float3 rightControlPoint = default;         // The position of `rightCurve`'s control point.（`rightCurve`的控制點位置。）
                            float3 rightEdgeEndpoint = default;         // The position where `edge`'s right boundary intersects `rightCurve`.（`edge`的右側邊界與`rightCurve`相交的位置。）
                            float3 rightIntersection = default;         // The position where `rightConnectionTangent` intersects `rightEdgeExtension`.（`rightConnectionTangent`與`rightEdgeExtension`相交的位置。）
                            float3[] leftTangentCollection = default;   // The two tangent points of the roundabout from `leftEdgeEndpoint`.（圓環上由`leftEdgeEndpoint`產生的兩個切點。）
                            float3[] rightTangentCollection = default;  // The two tangent points of the roundabout from `rightEdgeEndpoint`.（圓環上由`rightEdgeEndpoint`產生的兩個切點。）
                            Line2 leftConnectionTangent = default;      // The tangent line of the roundabout at `leftConnection`.（圓環上由`leftConnection`產生的切線。）
                            Line2 leftEdgeExtension = default;          // The extension line of `edge`'s left boundary from `leftEdgeEndpoint`.（`edge`的左側邊界從`leftEdgeEndpoint`開始的延伸線。）
                            Line2 rightConnectionTangent = default;     // The tangent line of the roundabout at `rightConnection`.（圓環上由`rightConnection`產生的切線。）
                            Line2 rightEdgeExtension = default;         // The extension line of `edge`'s right boundary from `rightEdgeEndpoint`.（`edge`的右側邊界從`rightEdgeEndpoint`開始的延伸線。）
                            List<float3> innerArc = default;            // The inner arc of the roundabout.（圓環內側的圓弧。）
                            List<float3> leftArc = default;             // The left outer arc of the roundabout.（圓環左外側的圓弧。）
                            List<float3> leftCurve = default;           // The transition curve from `leftArc` to `edge`'s left boundary.（`leftArc`至`edge`的左側邊界的過渡曲線。）
                            List<float3> leftFinalCurve = new List<float3>();   // The complete curve of the left part of the road intersection.（道路相交處左側的完整曲線。） 
                            List<float3> rightArc = default;            // The right outer arc of the roundabout.（圓環右外側的圓弧。）
                            List<float3> rightCurve = default;          // The transition curve from `rightArc` to `edge`'s right boundary.（`rightArc`至`edge`的右側邊界的過渡曲線。）
                            List<float3> rightFinalCurve = new List<float3>();  // The complete curve of the right part of the road intersection.（道路相交處右側的完整曲線。） 

                            // Find out road intersection's left and right boundaries in azimuth angle.（找出路口以方位角表示的左右側邊界。）
                            if (roundaboutEdgesSorted.Length > 1)
                            {
                                if (i == 0)
                                {
                                    leftAzimuth = (2 * math.PI + centerAzimuth + roundaboutAzimuths[roundaboutEdgesSorted[roundaboutEdgesSorted.Length - 1]]) / 2;
                                    rightAzimuth = (roundaboutAzimuths[roundaboutEdgesSorted[1]] + centerAzimuth) / 2;
                                    if (leftAzimuth >= 2 * math.PI) leftAzimuth -= 2 * math.PI;
                                }
                                else if (i == roundaboutEdgesSorted.Length - 1)
                                {
                                    leftAzimuth = (centerAzimuth + roundaboutAzimuths[roundaboutEdgesSorted[roundaboutEdgesSorted.Length - 2]]) / 2;
                                    rightAzimuth = (2 * math.PI + roundaboutAzimuths[roundaboutEdgesSorted[0]] + centerAzimuth) / 2;
                                    if (rightAzimuth >= 2 * math.PI) rightAzimuth -= 2 * math.PI;
                                }
                                else
                                {
                                    leftAzimuth = (centerAzimuth + roundaboutAzimuths[roundaboutEdgesSorted[i - 1]]) / 2;
                                    rightAzimuth = (roundaboutAzimuths[roundaboutEdgesSorted[i + 1]] + centerAzimuth) / 2;
                                }
                            }
                            else
                            {
                                leftAzimuth  = GeometryUtils.AddAngle(centerAzimuth, -math.PI / 2);
                                rightAzimuth = GeometryUtils.AddAngle(centerAzimuth, math.PI / 2);
                            }

                            // Find out the endpoints of `edge`'s left and right boundaries.（找出`edge`的左側與右側邊界端點。）
                            if (connectToStart[edge])
                            {
                                leftEdgeEndpoint = leftEdgeStart.a;
                                rightEdgeEndpoint = rightEdgeStart.a;
                            }
                            else
                            {
                                leftEdgeEndpoint = rightEdgeEnd.d;
                                rightEdgeEndpoint = leftEdgeEnd.d;
                            }

                            // Find out the intersections between the extenstion line of `edge`'s left and right boundaries and the roundabout and their azimuth angles.（找出`edge`左側與右側邊界延伸線與圓環的相交處以及它們的方位角。）
                            leftEdgeExtension = new Line2(leftEdgeEndpoint.xz, GeometryUtils.Location(new Circle3(100, leftEdgeEndpoint, new quaternion(0, 0, 0, 0)), centerAzimuth).xz);
                            rightEdgeExtension = new Line2(rightEdgeEndpoint.xz, GeometryUtils.Location(new Circle3(100, rightEdgeEndpoint, new quaternion(0, 0, 0, 0)), centerAzimuth).xz);
                            bool isLeftIntersect = GeometryUtils.Intersect(roundaboutCircle, leftEdgeExtension, out float3[] leftIntersectionCollection);
                            bool isRightIntersect =  GeometryUtils.Intersect(roundaboutCircle, rightEdgeExtension, out float3[] rightIntersectionCollection);
                            leftIntersection = math.distance(leftEdgeEndpoint, leftIntersectionCollection[0]) > math.distance(leftEdgeEndpoint, leftIntersectionCollection[1]) ? leftIntersectionCollection[1] : leftIntersectionCollection[0];
                            rightIntersection = math.distance(rightEdgeEndpoint, rightIntersectionCollection[0]) > math.distance(rightEdgeEndpoint, rightIntersectionCollection[1]) ? rightIntersectionCollection[1] : rightIntersectionCollection[0];
                            leftIntersectionAzimuth = GeometryUtils.Azimuth(roundaboutCircle.position, leftIntersection);
                            rightIntersectionAzimuth = GeometryUtils.Azimuth(roundaboutCircle.position, rightIntersection);

                            // Find out the tangent points between `edge`'s left and right endpoints and the roundabout and their azimuth angles.（找出`edge`左側與右側端點與圓環的切點以及它們的方位角。）
                            leftTangentCollection = GeometryUtils.TangentPoint(roundaboutCircle, leftEdgeEndpoint);
                            rightTangentCollection = GeometryUtils.TangentPoint(roundaboutCircle, rightEdgeEndpoint);
                            leftTangentAzimuth = GeometryUtils.Leftest(leftTangentCollection.Select(x => GeometryUtils.Azimuth(roundaboutCircle.position, x)).ToArray(), leftIntersectionAzimuth);
                            rightTangentAzimuth = GeometryUtils.Rightest(rightTangentCollection.Select(x => GeometryUtils.Azimuth(roundaboutCircle.position, x)).ToArray(), rightIntersectionAzimuth);

                            // Find out the transition point between left/right arcs and curves and their azimuths.（找出左側與右側圓弧與曲線的過渡點與它們的方位角。）
                            _ = GeometryUtils.Righter(leftTangentAzimuth, leftAzimuth, out float leftFixAzimuth);
                            _ = GeometryUtils.Lefter(rightTangentAzimuth, rightAzimuth, out float rightFixAzimuth);
                            leftConnectionAzimuth = GeometryUtils.Lefter(leftIntersectionAzimuth, leftFixAzimuth) ? leftFixAzimuth : ((GeometryUtils.RotationAngle(leftIntersectionAzimuth, leftFixAzimuth) >= deviation) ? GeometryUtils.AddAngle(leftIntersectionAzimuth, -deviation / 2) : GeometryUtils.MeanAngle(leftIntersectionAzimuth, leftFixAzimuth));
                            rightConnectionAzimuth = GeometryUtils.Righter(rightIntersectionAzimuth, rightFixAzimuth) ? rightFixAzimuth : ((GeometryUtils.RotationAngle(rightIntersectionAzimuth, rightFixAzimuth) >= deviation) ? GeometryUtils.AddAngle(rightIntersectionAzimuth, deviation / 2) : GeometryUtils.MeanAngle(rightIntersectionAzimuth, rightFixAzimuth));
                            leftConnection = GeometryUtils.Location(roundaboutCircle, leftConnectionAzimuth);
                            rightConnection = GeometryUtils.Location(roundaboutCircle, rightConnectionAzimuth);

                            // Find out the control point of the curve.（找出曲線的控制點位置。）
                            leftConnectionTangent = new Line2(leftConnection.xz, GeometryUtils.Location(new Circle3(1, leftConnection, new quaternion(0, 0, 0, 0)), leftConnectionAzimuth + math.PI / 2).xz);
                            rightConnectionTangent = new Line2(rightConnection.xz, GeometryUtils.Location(new Circle3(1, rightConnection, new quaternion(0, 0, 0, 0)), rightConnectionAzimuth - math.PI / 2).xz);
                            _ = MathUtils.Intersect(leftEdgeExtension, leftConnectionTangent, out float2 leftT);
                            _ = MathUtils.Intersect(rightEdgeExtension, rightConnectionTangent, out float2 rightT);
                            leftControlPoint2D = MathUtils.Position(leftEdgeExtension, leftT.x);
                            rightControlPoint2D = MathUtils.Position(rightEdgeExtension, rightT.x);
                            leftControlPoint = new float3(leftControlPoint2D.x, (leftEdgeEndpoint.y + roundaboutCircle.position.y) / 2, leftControlPoint2D.y);
                            rightControlPoint = new float3(rightControlPoint2D.x, (rightEdgeEndpoint.y + roundaboutCircle.position.y) / 2, rightControlPoint2D.y);

                            // Interpolate the arcs and the curves.（內插圓弧與曲線。）
                            leftCurve = GeometryUtils.Interpolate(new Bezier4x3(leftConnection, leftConnection, leftControlPoint, leftEdgeEndpoint), 0.5f, 0.2f);
                            rightCurve = GeometryUtils.Interpolate(new Bezier4x3(rightEdgeEndpoint, rightControlPoint, rightConnection, rightConnection), 0.5f, 0.2f);
                            leftArc = GeometryUtils.Arc(roundaboutCircle, leftAzimuth, leftConnectionAzimuth);
                            rightArc = GeometryUtils.Arc(roundaboutCircle, rightConnectionAzimuth, rightAzimuth);

                            if (roundaboutEdgesSorted.Length > 1)
                            {
                                innerArc = GeometryUtils.Arc(new Circle3(roundaboutRadius - maxWidth, roundaboutLocation, new quaternion(0, 0, 0, 0)), leftAzimuth, rightAzimuth);
                                innerArc.Reverse();
                            }
                            else
                            {
                                innerArc = GeometryUtils.Arc(roundaboutCircle, rightAzimuth, leftAzimuth);
                                innerArc = innerArc.GetRange(1, innerArc.Count - 2);
                            }

                            // Join the interpolated line segments.（將內插後的線段結合。）
                            leftFinalCurve.AddRange(leftArc);
                            leftFinalCurve.AddRange(leftCurve.GetRange(1, leftCurve.Count - 1));
                            rightFinalCurve.AddRange(rightCurve);
                            rightFinalCurve.AddRange(rightArc.GetRange(1, rightArc.Count - 1));

                            if (connectToStart[edge])
                            {
                                RoundaboutOuterLeftVertices[_roundabout][edge] = leftFinalCurve;
                                RoundaboutOuterRightVertices[_roundabout][edge] = rightFinalCurve;
                            }
                            else
                            {
                                RoundaboutOuterLeftVertices[_roundabout][edge] = rightFinalCurve;
                                RoundaboutOuterRightVertices[_roundabout][edge] = leftFinalCurve;
                            }

                            RoundaboutInnerVertices[_roundabout][edge] = innerArc;
                        }
                    }

                    if (!RoundaboutAttachments.TryGetValue(_roundabout, out _)) RoundaboutAttachments[_roundabout] = Entity.Null;
                    RoundaboutCenterVertices[_roundabout] = GeometryUtils.Interpolate(new Circle3(roundaboutRadius - maxWidth / 2, roundaboutLocation, new quaternion(0, 0, 0, 0)));
                    RoundaboutWidth[_roundabout] = maxWidth;
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetRoadsProperties(); 於 GetRoadsProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Entity _road in _roadQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, string>();

                    // Retrieve the custom name for the road. Expected output: "Oak Street"
                    // （獲取道路的自訂名稱。預期輸出："橡木街"）
                    Entity roadSection = EntityManager.GetComponentData<Aggregated>(_road).m_Aggregate;
                    string roadName = m_Name.GetRenderedLabelName(roadSection);
                    props["Name"] = roadName;

                    // Retrieve the assset name for the road. Expected output: "Three-Lane One-Way Highway"
                    // （獲取道路的資產名稱。預期輸出："單向三線公路"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Asset")) props["Asset"] = m_Name.GetRenderedLabelName(_road);

                    // Retrieve the category of the road. Expected output: "Highway"
                    // （獲取道路的分類。預期輸出："Highway"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Category"))
                    {
                        Entity roadPrefab = EntityManager.GetComponentData<PrefabRef>(_road).m_Prefab;
                        Entity roadUIGroup = EntityManager.GetComponentData<UIObjectData>(roadPrefab).m_Group;
                        string roadUIGroupPrefabName = m_Prefab.GetPrefabName(roadUIGroup);
                        string roadCategory = Category.TryGetValue(roadUIGroupPrefabName, out string v) ? v : roadUIGroupPrefabName;

                        // Handle the street-running tracks.
                        // （處理混合路權的軌道。）
                        if (EntityManager.HasComponent<TrainTrack>(_road)) roadCategory = roadCategory + ";" + Category["TrainTracks"];
                        if (EntityManager.HasComponent<SubwayTrack>(_road)) roadCategory = roadCategory + ";" + Category["SubwayTracks"];
                        if (EntityManager.HasComponent<TramTrack>(_road)) roadCategory = roadCategory + ";" + Category["TramTracks"];

                        props["Category"] = roadCategory;
                    }

                    // Retrieve the nodes forming the centerline of the road segment. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成道路中心線的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "CenterLine"))
                    {
                        edges["CenterLine"] = GetNetCenterLine(_road, EntityManager, 1, 1);
                        type["CenterLine"] = CartoObject.L;
                    }

                    // Retrieve the vehicle driving direction on the road. Expected output: "Forward"
                    // （獲取載具在道路上的行駛方向。預期輸出："Forward"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Direction"))
                    {
                        Entity roadEdgeComposition = EntityManager.GetComponentData<Composition>(_road).m_Edge;
                        RoadComposition roadComposition= EntityManager.GetComponentData<RoadComposition>(roadEdgeComposition);
                        Game.Prefabs.RoadFlags roadDirectionFlag = roadComposition.m_Flags;
                        string roadDirection = "Both";
                        if (roadDirectionFlag.HasFlag(Game.Prefabs.RoadFlags.DefaultIsForward)) roadDirection = "Forward";
                        if (roadDirectionFlag.HasFlag(Game.Prefabs.RoadFlags.DefaultIsBackward)) roadDirection = "Backward";
                        props["Direction"] = roadDirection;
                    }

                    // Retrieve the edge of the road. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成道路邊緣的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Edge"))
                    {
                        edges["Edge"] = GetNetEdge(_road, EntityManager, 3, 1);
                        type["Edge"] = CartoObject.P;
                    }

                    // Retrieve the form of the road. Expected output: "Normal"
                    // （獲取道路的形式。預期輸出："Normal"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Form"))
                    {
                        Entity roadEdgeComposition = EntityManager.GetComponentData<Composition>(_road).m_Edge;
                        NetCompositionData roadCompositionData = EntityManager.GetComponentData<NetCompositionData>(roadEdgeComposition);
                        CompositionFlags.General roadFormFlag = roadCompositionData.m_Flags.m_General;
                        string roadForm = "Normal";
                        if (roadFormFlag.HasFlag(CompositionFlags.General.Elevated)) roadForm = "Elevated";
                        if (roadFormFlag.HasFlag(CompositionFlags.General.Tunnel)) roadForm = "Tunnel";
                        props["Form"] = roadForm;
                    }

                    // Retrieve the mean height of the road segment. Expected output: 145.881424
                    // （獲取道路區段的平均高度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Height"))
                    {
                        Bounds1 roadHeightBounds = EntityManager.GetComponentData<EdgeGeometry>(_road).m_Bounds.y;
                        props["Height"] = (roadHeightBounds.max + roadHeightBounds.min) / 2;
                    }

                    // Retrieve the length of the road segment. Expected output: 145.881424
                    // （獲取道路區段的長度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Length")) props["Length"] = EntityManager.GetComponentData<Curve>(_road).m_Length;

                    // Retrieve the width of the road segment. Expected output: 17
                    // （獲取道路區段的寬度。預期輸出：17）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Width"))
                    {
                        Entity roadEdgeComposition = EntityManager.GetComponentData<Composition>(_road).m_Edge;
                        NetCompositionData roadCompositionData = EntityManager.GetComponentData<NetCompositionData>(roadEdgeComposition);
                        props["Width"] = roadCompositionData.m_Width;
                    }

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Object")) props["Object"] = "Road";

                    roadList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetRoadsProperties(); 於 GetRoadsProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (KeyValuePair<Entity, List<float3>> _roundaboutData in RoundaboutCenterVertices)
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, string>();

                    // Retrieve the custom name for the roundabout. Expected output: "Small roundabout"
                    // （獲取圓環的自訂名稱。預期輸出："小型圓環島"）
                    Entity roundaboutAttached = RoundaboutAttachments[_roundaboutData.Key];
                    props["Name"] = (roundaboutAttached != Entity.Null) ? m_Name.GetRenderedLabelName(roundaboutAttached) : string.Empty;

                    // Retrieve the assset name for the road. Expected output: "Small roundabout"
                    // （獲取道路的資產名稱。預期輸出："小型圓環島"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Asset"))
                    {
                        if (roundaboutAttached != Entity.Null)
                        {
                            Entity roundaboutPrefab = EntityManager.GetComponentData<PrefabRef>(roundaboutAttached).m_Prefab;
                            props["Asset"] = LocaleUtils.Translate($"Assets.NAME[{m_Prefab.GetPrefabName(roundaboutPrefab)}]");
                        }
                        else
                        {
                            props["Asset"] = m_Name.GetRenderedLabelName(_roundaboutData.Key);
                        }
                    }

                    // Retrieve the category of the road. Expected output: "Small"
                    // （獲取道路的分類。預期輸出："Small"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Category"))
                    {
                        float roundaboutWidth = RoundaboutWidth[_roundaboutData.Key];
                        string roundaboutCategory = "Small";
                        if (roundaboutWidth >= 10)
                        {
                            if (roundaboutWidth >= 14)
                            {
                                roundaboutCategory = "Large";
                            }
                            else
                            {
                                roundaboutCategory = "Medium";
                            }
                        }
                        props["Category"] = roundaboutCategory;
                    }

                    // Retrieve the nodes forming the centerline of the road segment. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成道路中心線的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    List<float3> roundaboutCenterline = _roundaboutData.Value;
                    roundaboutCenterline.Add(roundaboutCenterline[0]);

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "CenterLine"))
                    {
                        edges["CenterLine"] = roundaboutCenterline;
                        type["CenterLine"] = CartoObject.L;
                    }

                    // Retrieve the vehicle driving direction on the road. Expected output: "Forward"
                    // （獲取載具在道路上的行駛方向。預期輸出："Forward"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Direction"))
                    {
                        if (Instance.Traffic == "Left") props["Direction"] = "Forward";
                        else props["Direction"] = "Backward";
                    }

                    // Retrieve the form of the road. Expected output: "Normal"
                    // （獲取道路的形式。預期輸出："Normal"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Form")) props["Form"] = "Normal";

                    // Retrieve the mean height of the road segment. Expected output: 145.881424
                    // （獲取道路區段的平均高度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Height")) props["Height"] = _roundaboutData.Value[0].y;

                    // Retrieve the length of the road segment. Expected output: 145.881424
                    // （獲取道路區段的長度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Length")) props["Length"] = GeometryUtils.Length(roundaboutCenterline);

                    // Retrieve the width of the road segment. Expected output: 8
                    // （獲取道路區段的寬度。預期輸出：8）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Width")) props["Width"] = RoundaboutWidth[_roundaboutData.Key];

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Object")) props["Object"] = "Road";

                    roadList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetRoadsProperties(); 於 GetRoadsProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Dictionary<Entity, List<float3>> _trackData in RoundaboutTramTracks)
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, string>();

                    // Retrieve the custom name for the road. Expected output: "Oak Street"
                    // （獲取道路的自訂名稱。預期輸出："橡木街"）
                    Entity roadSection = EntityManager.GetComponentData<Aggregated>(_trackData.Keys.First()).m_Aggregate;
                    props["Name"] = m_Name.GetRenderedLabelName(roadSection);

                    // Retrieve the assset name for the road. Expected output: "Three-Lane One-Way Highway"
                    // （獲取道路的資產名稱。預期輸出："單向三線公路"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Asset")) props["Asset"] = m_Name.GetRenderedLabelName(_trackData.Keys.First());

                    // Retrieve the category of the road. Expected output: "Small"
                    // （獲取道路的分類。預期輸出："Small"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Category")) props["Category"] = Category["TramTracks"];

                    // Retrieve the nodes forming the centerline of the road segment. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成道路中心線的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "CenterLine"))
                    {
                        edges["CenterLine"] = _trackData.Values.First();
                        type["CenterLine"] = CartoObject.L;
                    }

                    // Retrieve the vehicle driving direction on the road. Expected output: "Forward"
                    // （獲取載具在道路上的行駛方向。預期輸出："Forward"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Direction"))
                    {
                        CompositionFlags trackSideCompositionFlags = EntityManager.GetComponentData<Upgraded>(_trackData.Keys.First()).m_Flags;
                        bool leftSideTrack  = trackSideCompositionFlags.m_Left.HasFlag(CompositionFlags.Side.PrimaryTrack);
                        bool rightSideTrack = trackSideCompositionFlags.m_Right.HasFlag(CompositionFlags.Side.PrimaryTrack);

                        if (leftSideTrack && rightSideTrack)
                        {
                            props["Direction"] = "Both";
                        }
                        else if (Instance.Traffic == "Left")
                        {
                            props["Direction"] = leftSideTrack ? "Forward" : "Backward";
                        }
                        else
                        {
                            props["Direction"] = leftSideTrack ? "Backward" : "Forward";
                        }
                    }

                    // Retrieve the form of the road. Expected output: "Normal"
                    // （獲取道路的形式。預期輸出："Normal"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Form")) props["Form"] = "Normal";

                    // Retrieve the mean height of the road segment. Expected output: 145.881424
                    // （獲取道路區段的平均高度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Height")) props["Height"] = _trackData.Values.First()[0].y;

                    // Retrieve the length of the road segment. Expected output: 145.881424
                    // （獲取道路區段的長度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Length")) props["Length"] = GeometryUtils.Length(_trackData.Values.First());

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Object")) props["Object"] = "Track";

                    roadList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetRoadsProperties(); 於 GetRoadsProperties() 發生一個錯誤； {ex}");
                }
            }

            return roadList;
        }

        /// <summary>
        /// Searching for properties of all existing tracks.
        /// （搜尋現有所有軌道的屬性。）
        /// </summary>
        public List<CartoObject> GetTracksProperties(bool ignoreRoads = true)
        {
            List<CartoObject> trackList = new List<CartoObject>();

            foreach (Entity _track in _trackQuery.ToEntityArray(Allocator.Temp))
            {
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, string>();

                    // Road properties are already handled in GetRoadsProperties().
                    // （道路的屬性已經在 GetRoadsProperties() 處理過了。）
                    if ((EntityManager.HasComponent<Road>(_track)) & ignoreRoads) continue;

                    // Retrieve the custom name for the track. Expected output: "Tram Track"
                    // （獲取軌道的自訂名稱。預期輸出："電車軌道"）
                    string trackName = string.Empty;

                    if (EntityManager.HasComponent<Aggregated>(_track))
                    {
                        trackName = m_Name.GetRenderedLabelName(EntityManager.GetComponentData<Aggregated>(_track).m_Aggregate);
                        if (LocaleUtils.TryTranslate(trackName, out string translated)) trackName = translated;
                    }

                    props["Name"] = trackName;

                    // Retrieve the assset name for the track. Expected output: "Double Train Track"
                    // （獲取軌道的資產名稱。預期輸出："雙軌鐵路"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Asset")) props["Asset"] = m_Name.GetRenderedLabelName(_track);

                    // Retrieve the category of the track. Expected output: "Train"
                    // （獲取軌道的分類。預期輸出："Train"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Category"))
                    {
                        List<string> trackCategory = new List<string>();
                        if (EntityManager.HasComponent<TrainTrack>(_track)) trackCategory.Add(Category["TrainTracks"]);
                        if (EntityManager.HasComponent<SubwayTrack>(_track)) trackCategory.Add(Category["SubwayTracks"]);
                        if (EntityManager.HasComponent<TramTrack>(_track)) trackCategory.Add(Category["TramTracks"]);

                        props["Category"] = string.Join(";", trackCategory);
                    }

                    // Retrieve the nodes forming the centerline of the track segment. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成軌道中心線的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "CenterLine"))
                    {
                        edges["CenterLine"] = GetNetCenterLine(_track, EntityManager, 1, 1);
                        type["CenterLine"] = CartoObject.L;
                    }

                    // Retrieve the vehicle driving direction on the track. Expected output: "Forward"
                    // （獲取載具在軌道上的行駛方向。預期輸出："Forward"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Direction"))
                    {
                        Entity trackEdgeComposition = EntityManager.GetComponentData<Composition>(_track).m_Edge;
                        RoadComposition trackComposition = EntityManager.GetComponentData<RoadComposition>(trackEdgeComposition);
                        Game.Prefabs.RoadFlags trackDirectionFlag = trackComposition.m_Flags;
                        string trackDirection = "Both";
                        if (trackDirectionFlag.HasFlag(Game.Prefabs.RoadFlags.DefaultIsForward)) trackDirection = "Forward";
                        if (trackDirectionFlag.HasFlag(Game.Prefabs.RoadFlags.DefaultIsBackward)) trackDirection = "Backward";
                        props["Direction"] = trackDirection;
                    }

                    // Retrieve the edge of the track. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成軌道邊緣的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Edge"))
                    {
                        edges["Edge"] = GetNetEdge(_track, EntityManager, 3, 1);
                        type["Edge"] = CartoObject.P;
                    }

                    // Retrieve the form of the track. Expected output: "Normal"
                    // （獲取軌道的形式。預期輸出："Normal"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Form"))
                    {
                        Entity trackEdgeComposition = EntityManager.GetComponentData<Composition>(_track).m_Edge;
                        NetCompositionData trackCompositionData = EntityManager.GetComponentData<NetCompositionData>(trackEdgeComposition);
                        CompositionFlags.General trackFormFlag = trackCompositionData.m_Flags.m_General;
                        string trackForm = "Normal";
                        if (trackFormFlag.HasFlag(CompositionFlags.General.Elevated)) trackForm = "Elevated";
                        if (trackFormFlag.HasFlag(CompositionFlags.General.Tunnel)) trackForm = "Tunnel";
                        props["Form"] = trackForm;
                    }

                    // Retrieve the mean height of the track segment. Expected output: 145.881424
                    // （獲取軌道區段的平均高度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Height"))
                    {
                        Bounds1 trackHeightBounds = EntityManager.GetComponentData<EdgeGeometry>(_track).m_Bounds.y;
                        props["Height"] = (trackHeightBounds.max + trackHeightBounds.min) / 2;
                    }

                    // Retrieve the length of the track segment. Expected output: 145.881424
                    // （獲取軌道區段的長度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Length")) props["Length"] = EntityManager.GetComponentData<Curve>(_track).m_Length;

                    // Retrieve the width of the track segment. Expected output: 11
                    // （獲取軌道區段的寬度。預期輸出：11）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Width"))
                    {
                        Entity trackEdgeComposition = EntityManager.GetComponentData<Composition>(_track).m_Edge;
                        NetCompositionData trackCompositionData = EntityManager.GetComponentData<NetCompositionData>(trackEdgeComposition);
                        props["Width"] = trackCompositionData.m_Width;
                    }

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Object")) props["Object"] = "Track";

                    trackList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetTracksProperties(); 於 GetTracksProperties() 發生一個錯誤； {ex}");
                }
            }

            foreach (Dictionary<Entity, List<float3>> _trackData in RoundaboutTramTracks)
            {
                if (EntityManager.HasComponent<Road>(_trackData.Keys.First())) continue;
                
                try
                {
                    var edges = new Dictionary<string, List<float3>>();
                    var props = new Dictionary<string, object>();
                    var type = new Dictionary<string, string>();

                    // Retrieve the custom name for the track. Expected output: "Tram Track"
                    // （獲取軌道的自訂名稱。預期輸出："電車軌道"）
                    string trackName = string.Empty;

                    if (EntityManager.HasComponent<Aggregated>(_trackData.Keys.First()))
                    {
                        trackName = m_Name.GetRenderedLabelName(EntityManager.GetComponentData<Aggregated>(_trackData.Keys.First()).m_Aggregate);
                        if (LocaleUtils.TryTranslate(trackName, out string translated)) trackName = translated;
                    }

                    props["Name"] = trackName;

                    // Retrieve the assset name for the track. Expected output: "Double Train Track"
                    // （獲取軌道的資產名稱。預期輸出："雙軌鐵路"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Asset")) props["Asset"] = m_Name.GetRenderedLabelName(_trackData.Keys.First());

                    // Retrieve the category of the track. Expected output: "Tram"
                    // （獲取軌道的分類。預期輸出："Tram"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Category")) props["Category"] = Category["TramTracks"];

                    // Retrieve the nodes forming the centerline of the track segment. Expected output (per node): float3(-79.33802f, 548.8162f, 397.9146f)
                    // （獲取組成軌道中心線的節點。預期輸出（每個節點）：float3(-79.33802f, 548.8162f, 397.9146f)）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "CenterLine"))
                    {
                        edges["CenterLine"] = _trackData.Values.First();
                        type["CenterLine"] = CartoObject.L;
                    }

                    // Retrieve the vehicle driving direction on the track. Expected output: "Forward"
                    // （獲取載具在軌道上的行駛方向。預期輸出："Forward"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Direction"))
                    {
                        Entity trackEdgeComposition = EntityManager.GetComponentData<Composition>(_trackData.Keys.First()).m_Edge;
                        RoadComposition trackComposition = EntityManager.GetComponentData<RoadComposition>(trackEdgeComposition);
                        Game.Prefabs.RoadFlags trackDirectionFlag = trackComposition.m_Flags;
                        string trackDirection = "Both";
                        if (trackDirectionFlag.HasFlag(Game.Prefabs.RoadFlags.DefaultIsForward)) trackDirection = "Forward";
                        if (trackDirectionFlag.HasFlag(Game.Prefabs.RoadFlags.DefaultIsBackward)) trackDirection = "Backward";
                        props["Direction"] = trackDirection;
                    }

                    // Retrieve the form of the track. Expected output: "Normal"
                    // （獲取軌道的形式。預期輸出："Normal"）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Form")) props["Form"] = "Normal";

                    // Retrieve the mean height of the track segment. Expected output: 145.881424
                    // （獲取軌道區段的平均高度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Height")) props["Height"] = _trackData.Values.First()[0].y;

                    // Retrieve the length of the track segment. Expected output: 145.881424
                    // （獲取軌道區段的長度。預期輸出：145.881424）
                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Length")) props["Length"] = GeometryUtils.Length(_trackData.Values.First());

                    if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Net, "Object")) props["Object"] = "Track";

                    trackList.Add(new CartoObject(edges, props, type));
                }
                catch (Exception ex)
                {
                    m_Log.Error($"An error occured at GetRoadsProperties(); 於 GetTracksProperties() 發生一個錯誤； {ex}");
                }
            }

            return trackList;
        }
    }
}