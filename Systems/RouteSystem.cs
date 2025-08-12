using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Creatures;
using Game.Economy;
using Game.Net;
using Game.Pathfind;
using Game.Routes;
using Game.Tools;
using Game.UI;
using Game.Vehicles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// The system that searches transportation routes.
    /// （搜尋運輸服務路線的系統。）
    /// </summary>
    public partial class RouteSystem : GameSystemBase
    {
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
        /// The system managing prefabricated data.（管理預製模板資料的系統。）<br/>
        /// See <see cref="Instance.Prefab"/> for more information.
        /// </summary>
        static readonly Game.Prefabs.PrefabSystem _prefab = Instance.Prefab;

        /// <summary>
        /// The assembly of Extended Transport Manager mod.（Extended Transport Manager 模組組件。）<br/>
        /// See <see cref="Instance.Xtm"/> for more information.
        /// </summary>
        static readonly ExtendedTransportManager _xtm = Instance.Xtm;

        /// <summary>
        /// The query to collect transportation route instances.
        /// （收集運輸服務路線實例的查詢。）
        /// </summary>
        static EntityQuery _routeQuery;

        /// <summary>
        /// The query to collect transportation route prefabs.
        /// （收集運輸服務路線預製模板的查詢。）
        /// </summary>
        static EntityQuery _routePrefabQuery;

        /// <summary>
        /// The list of <see cref="RouteStat"/>s stored in the local system.
        /// （儲存於本地系統的 <see cref="RouteStat"/> 列表。）
        /// </summary>
        private NativeList<RouteStat> _localRouteStats;

        /// <summary>
        /// The type of transportation routes that can be exported.
        /// （可被輸出的運輸路線種類。）
        /// </summary>
        public static HashSet<Game.Prefabs.TransportType> ExportableTransportTypes = new()
        {
            Game.Prefabs.TransportType.Bus,
            Game.Prefabs.TransportType.Train,
            Game.Prefabs.TransportType.Tram,
            Game.Prefabs.TransportType.Ship,
            Game.Prefabs.TransportType.Subway
        };

        protected override void OnCreate()
        { 
            _routeQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Color>(),
                    ComponentType.ReadOnly<Game.Prefabs.PrefabRef>(),
                    ComponentType.ReadOnly<Route>(),
                    ComponentType.ReadOnly<RouteNumber>(),
                    ComponentType.ReadOnly<RouteSegment>(),
                    ComponentType.ReadOnly<RouteVehicle>(),
                    ComponentType.ReadOnly<RouteWaypoint>(),
                    ComponentType.ReadOnly<TransportLine>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _routePrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Prefabs.RouteData>(),
                    ComponentType.ReadOnly<Game.Prefabs.TransportLineData>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            base.OnCreate();
            _log.Debug("RouteSystem instance created. 路線系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy()
        {
            _xtm.Dispose();
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
            // CommonUtils.Dispose(ref _curveEntityMap);
            CommonUtils.Dispose(ref _localRouteStats);
        }

        /// <summary>
        /// Retrieve the centerline of the routes.
        /// （獲得運輸服務路線的中心線。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="nodeEntityMap">The map between centerline nodes and the routes.（運輸服務路線與中心線節點的映射表。）</param>
        private void GetCenterlines(Options options, ref NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap)
        {
            NativeParallelHashMap<Entity, int> nodeCountEntityMap = new(_routeQuery.CalculateEntityCount(), Allocator.Persistent);
            IOUtils.GetTargetProjections(options, out Geodata.CRS targetCRS, out ProjectionDefinition targetProjection);

            CountCenterlineNodesJob countNodesJob = new()
            {
                pathElementBufferLookup = GetBufferLookup<PathElement>(true),
                routeSegmentBufferLookup = GetBufferLookup<RouteSegment>(true),
                curveLookup = GetComponentLookup<Curve>(true),
                routeStats = _localRouteStats,
                nodeCountEntityMap = nodeCountEntityMap.AsParallelWriter()
            };
            JobHandle countNodesHandle = countNodesJob.Schedule(_localRouteStats.Length, 16, default);
            countNodesHandle.Complete();

            CollectCenterlinesJob collectCenterlinesJob = new()
            {
                pathElementBufferLookup = GetBufferLookup<PathElement>(true),
                routeSegmentBufferLookup = GetBufferLookup<RouteSegment>(true),
                curveLookup = GetComponentLookup<Curve>(true),
                outsideConnectionLookup = GetComponentLookup<OutsideConnection>(true),
                center = options.GetTMCoord(),
                sourceCRS = options.GetTMProjection(),
                targetCRS = targetCRS,
                routeStats = _localRouteStats,
                nodeCountEntityMap = nodeCountEntityMap,
                sourceProjection = options.GetTMProjectionDefinition(),
                targetProjection = targetProjection,
                nodeEntityMap = nodeEntityMap.AsParallelWriter()
            };
            JobHandle collectCenterlinesHandle = collectCenterlinesJob.Schedule(_localRouteStats.Length, 16, default);
            collectCenterlinesHandle.Complete();

            CommonUtils.Dispose(ref nodeCountEntityMap);
        }

        /// <summary>
        /// Retrieve the acronym of routes from the Extended Transport Mnager mod.
        /// （由 Extended Transport Mnager 模組獲得路線的縮寫。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <returns>The map between route entities and their acronyms.（路線實體與縮寫的映射表。）</returns>
        private Dictionary<Entity, string> GetRouteAcronyms(Options options)
        {
            Dictionary<Entity, string> acronymEntityMap = new();
            
            if (options.XtmAcronym && _xtm.TryGet(false) && _xtm.TryGetXtmRouteExtraData())
            {
                NativeArray<Entity> routes = _routeQuery.ToEntityArray(Allocator.Temp);
                for (int i = 0; i < routes.Length; i++)
                {
                    Entity route = routes[i];
                    if (_xtm.TryGetRouteAcronym(EntityManager, route, out string acronym))
                    {
                        acronymEntityMap.Add(route, acronym);
                    }
                }
            }

            return acronymEntityMap;
        }

        /// <summary>
        /// Retrieve the length of the route.
        /// （獲得運輸服務路線的長度。）
        /// </summary>
        /// <param name="routeSegments">The buffer of route segments.（路線片段的緩衝區。）</param>
        /// <param name="pathInformationLookup">The lookup that searches for <see cref="PathInformation"/>.（搜尋 <see cref="PathInformation"/> 的查詢。）</param>
        /// <returns>The length of the route in meters (m).（以公尺計算的路線長度。）</returns>
        private static float GetRouteLength(DynamicBuffer<RouteSegment> routeSegments, ref ComponentLookup<PathInformation> pathInformationLookup)
        {
            ///  This is the burst-compatible version of <see cref="Game.UI.InGame.TransportUIUtils.GetRouteLength(EntityManager, Entity)"/>.
            /// （這是 <see cref="Game.UI.InGame.TransportUIUtils.GetRouteLength(EntityManager, Entity)"/> 的可 Burst 編譯版本。）。

            float length = 0f;

            for (int i = 0; i < routeSegments.Length; i++)
            {
                if (pathInformationLookup.TryGetComponent(routeSegments[i].m_Segment, out PathInformation pathInformationComponent))
                {
                    length += pathInformationComponent.m_Distance;
                }
            }

            return length;
        }

        /// <summary>
        /// Retrieve route entities' statistical data.
        /// （獲取運輸服務路線實體的統計資料。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        private void GetRouteStats(Options options)
        {
            bool countPets = options.PetPassenger;
            bool includeInactive = options.InactiveRoute;
            NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData> validRoutePrefabsDataMap = new(_routePrefabQuery.CalculateEntityCount(), Allocator.Persistent);
            NativeParallelHashSet<EnumWrapper<Game.Prefabs.TransportType>> exportableTransportTypes = new(ExportableTransportTypes.Count, Allocator.Persistent);
            CommonUtils.UnmanagedCopy(ExportableTransportTypes, ref exportableTransportTypes);

            CollectRoutePrefabsJob collectPrefabsJob = new()
            {
                acceptedRouteTypes = options.Features,
                exportableTransportTypes = exportableTransportTypes,
                validRoutePrefabsDataMap = validRoutePrefabsDataMap.AsParallelWriter()
            };
            JobHandle collectRoutesHandle = collectPrefabsJob.ScheduleParallel(_routePrefabQuery, default);
            collectRoutesHandle.Complete();

            CollectRouteStatsJob collectStatsJob = new()
            {
                countPets = countPets,
                includeInactive = includeInactive,
                layoutElementBufferLookup = GetBufferLookup<LayoutElement>(true),
                passengerBufferLookup = GetBufferLookup<Passenger>(true),
                resourcesBufferLookup = GetBufferLookup<Resources>(true),
                cargoTransportLookup = GetComponentLookup<CargoTransport>(true),
                cargoTransportVehicleDataLookup = GetComponentLookup<Game.Prefabs.CargoTransportVehicleData>(true),
                connectedLookup = GetComponentLookup<Connected>(true),
                currentRouteLookup = GetComponentLookup<CurrentRoute>(true),
                customNameLookup = GetComponentLookup<CustomName>(true),
                pathInformationLookup = GetComponentLookup<PathInformation>(true),
                petLookup = GetComponentLookup<Pet>(true),
                prefabRefLookup = GetComponentLookup<Game.Prefabs.PrefabRef>(true),
                publicTransportLookup = GetComponentLookup<PublicTransport>(true),
                publicTransportVehicleDataLookup = GetComponentLookup<Game.Prefabs.PublicTransportVehicleData>(true),
                taxiStandLookup = GetComponentLookup<TaxiStand>(true),
                transportStopLookup = GetComponentLookup<TransportStop>(true),
                vehicleModelLookup = GetComponentLookup<VehicleModel>(true),
                validRoutePrefabsDataMap = validRoutePrefabsDataMap,
                routeStats = _localRouteStats.AsParallelWriter()
            };
            JobHandle collectStatsHandle = collectStatsJob.ScheduleParallel(_routeQuery, default);
            collectStatsHandle.Complete();

            CommonUtils.Dispose(ref exportableTransportTypes);
            CommonUtils.Dispose(ref validRoutePrefabsDataMap);
        }

        /// <summary>
        /// Retrieve the number of stops on the route.
        /// （獲得運輸服務路線上的站點數量。）
        /// </summary>
        /// <param name="routeWaypoints">The buffer of route waypoints.（路線路徑點的緩衝區。）</param>
        /// <param name="connectedLookup">The lookup that searches for <see cref="Connected"/>.（搜尋 <see cref="Connected"/> 的查詢。）</param>
        /// <param name="taxiStandLookup">The lookup that searches for <see cref="TaxiStand"/>.（搜尋 <see cref="TaxiStand"/> 的查詢。）</param>
        /// <param name="transportStopLookup">The lookup that searches for <see cref="TransportStop"/>.（搜尋 <see cref="TransportStop"/> 的查詢。）</param>
        /// <returns>The number of stops on the route.（路線上的站點數量。）</returns>
        private static int GetStopCount(DynamicBuffer<RouteWaypoint> routeWaypoints, ref ComponentLookup<Connected> connectedLookup,
                                        ref ComponentLookup<TaxiStand> taxiStandLookup, ref ComponentLookup<TransportStop> transportStopLookup)
        {
            ///  This is the burst-compatible version of <see cref="Game.UI.InGame.TransportUIUtils.GetStopCount(EntityManager, Entity)"/>.
            /// （這是 <see cref="Game.UI.InGame.TransportUIUtils.GetStopCount(EntityManager, Entity)"/> 的可 Burst 編譯版本。）。
            
            int count = 0;

            for (int i = 0; i < routeWaypoints.Length; i++)
            {
                if (connectedLookup.TryGetComponent(routeWaypoints[i].m_Waypoint, out Connected connectedComponent))
                {
                    Entity connected = connectedComponent.m_Connected;
                    if (transportStopLookup.HasComponent(connected) && !taxiStandLookup.HasComponent(connected))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Retrieve the amount of cargo being transported by the vehicle.
        /// （獲得車輛輸送的貨物數量。）
        /// </summary>
        /// <param name="vehicle">The vehicle entity.（車輛實體。）</param>
        /// <param name="resourcesBufferLookup">The lookup that searches for <see cref="Resources"/>.（搜尋 <see cref="Resources"/> 的查詢。）</param>
        /// <param name="cargoTransportVehicleLookup">The lookup that searches for <see cref="Game.Prefabs.CargoTransportVehicleData"/>.（搜尋 <see cref="Game.Prefabs.CargoTransportVehicleData"/> 的查詢。）</param>
        /// <param name="prefabRefLookup">The lookup that searches for <see cref="Game.Prefabs.PrefabRef"/>.（搜尋 <see cref="Game.Prefabs.PrefabRef"/> 的查詢。）</param>
        /// <param name="capacity">The capacity of the vehicle.（車輛的容量。）</param>
        /// <returns>The amount of cargo transported by the vehicle.（車輛輸送的貨物數量。）</returns>
        private static int GetVehicleCargoAmount(Entity vehicle, ref BufferLookup<Resources> resourcesBufferLookup,
                                                 ref ComponentLookup<Game.Prefabs.CargoTransportVehicleData> cargoTransportVehicleLookup, ref ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup,
                                                 out int capacity)
        {
            int amount = 0;
            capacity = 0;
            
            if (!prefabRefLookup.TryGetComponent(vehicle, out Game.Prefabs.PrefabRef prefabRef) ||
                !cargoTransportVehicleLookup.TryGetComponent(prefabRef.m_Prefab, out Game.Prefabs.CargoTransportVehicleData vehicleData) ||
                !resourcesBufferLookup.TryGetBuffer(vehicle, out DynamicBuffer<Resources> resources)) return amount;

            capacity = vehicleData.m_CargoCapacity;

            for (int i = 0; i < resources.Length; i++)
            {
                amount += resources[i].m_Amount;
            }

            return amount;
        }

        /// <summary>
        /// Retrieve the number of passengers being transported by the vehicle.
        /// （獲得車輛輸送的乘客數量。）
        /// </summary>
        /// <param name="vehicle">The vehicle entity.（車輛實體。）</param>
        /// <param name="countPets">Whether to consider pets as passengers or not.（是否將寵物視為乘客？）</param>
        /// <param name="passengers">The buffer of vehicle passengers.（載具乘客的緩衝區。）</param>
        /// <param name="petLookup">The lookup that searches for <see cref="Pet"/>.（搜尋 <see cref="Pet"/> 的查詢。）</param>
        /// <param name="prefabRefLookup">The lookup that searches for <see cref="Game.Prefabs.PrefabRef"/>.（搜尋 <see cref="Game.Prefabs.PrefabRef"/> 的查詢。）</param>
        /// <param name="publicTransportVehicleDataLookup">The lookup that searches for <see cref="Game.Prefabs.PublicTransportVehicleData"/>.（搜尋 <see cref="Game.Prefabs.PublicTransportVehicleData"/> 的查詢。）</param>
        /// <param name="vehicleCapacity">The capacity of the vehicle.（車輛的容量。）</param>
        /// <returns>The number of passengers being transported by the vehicle.（車輛輸送的乘客數量。）</returns>
        private static int GetVehiclePassengerCount(Entity vehicle, bool countPets, DynamicBuffer<Passenger> passengers,
                                                    ref ComponentLookup<Pet> petLookup, ref ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup,
                                                    ref ComponentLookup<Game.Prefabs.PublicTransportVehicleData> publicTransportVehicleDataLookup,
                                                    out int vehicleCapacity)
        {
            int count = 0;
            vehicleCapacity = 0;

            if (!prefabRefLookup.TryGetComponent(vehicle, out Game.Prefabs.PrefabRef prefabRef) ||
                !publicTransportVehicleDataLookup.TryGetComponent(prefabRef.m_Prefab, out Game.Prefabs.PublicTransportVehicleData vehicleData)) return count;

            vehicleCapacity = vehicleData.m_PassengerCapacity;

            if (countPets)
            {
                count += passengers.Length;
            }
            else
            {
                for (int i = 0; i < passengers.Length; i++)
                {
                    if (!petLookup.HasComponent(passengers[i].m_Passenger))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Retrieve the statistics of vehicles runing on the route.
        /// （獲得運輸服務路線上行駛的車輛統計資訊。）
        /// </summary>
        /// <param name="route">The route entity.（運輸服務路線實體。）</param>
        /// <param name="routeVehicles">The buffer of route vehicles.（路線車輛的緩衝區。）</param>
        /// <param name="countPets">Whether to consider pets as passengers or not.（是否將寵物視為乘客？）</param>
        /// <param name="layoutElementBufferLookup">The lookup that searches for the buffer of <see cref="LayoutElement"/>.（搜尋 <see cref="LayoutElement"/> 緩衝區的查詢。）</param>
        /// <param name="passengerBufferLookup">The lookup that searches for the buffer of <see cref="Passenger"/>.（搜尋 <see cref="Passenger"/> 緩衝區的查詢。）</param>
        /// <param name="resourcesBufferLookup">The lookup that searches for the buffer of <see cref="Resources"/>.（搜尋 <see cref="Resources"/> 緩衝區的查詢。）</param>
        /// <param name="cargoTransportLookup">The lookup that searches for <see cref="CargoTransport"/>.（搜尋 <see cref="CargoTransport"/> 的查詢。）</param>
        /// <param name="cargoTransportVehicleDataLookup">The lookup that searches for <see cref="Game.Prefabs.CargoTransportVehicleData"/>.（搜尋 <see cref="Game.Prefabs.CargoTransportVehicleData"/> 的查詢。）</param>
        /// <param name="currentRouteLookup">The lookup that searches for <see cref="CurrentRoute"/>.（搜尋 <see cref="CurrentRoute"/> 的查詢。）</param>
        /// <param name="petLookup">The lookup that searches for <see cref="Pet"/>.（搜尋 <see cref="Pet"/> 的查詢。）</param>
        /// <param name="prefabRefLookup">The lookup that searches for <see cref="Game.Prefabs.PrefabRef"/>.（搜尋 <see cref="Game.Prefabs.PrefabRef"/> 的查詢。）</param>
        /// <param name="publicTransportLookup">The lookup that searches for <see cref="PublicTransport"/>.（搜尋 <see cref="PublicTransport"/> 的查詢。）</param>
        /// <param name="publicTransportVehicleLookup">The lookup that searches for <see cref="Game.Prefabs.PublicTransportVehicleData"/>.（搜尋 <see cref="Game.Prefabs.PublicTransportVehicleData"/> 的查詢。）</param>
        /// <param name="capacity">The total capacity of the vehicle.（車輛的總容量。）</param>
        /// <param name="cargoAmount">The total amount of cargo transported by the vehicles that belongs to the route.（路線車輛正在載運的貨物數量。）</param>
        /// <param name="passengerCount">The total number of passengers being transported by the vehicles that belong to the route.（路線車輛正在載運的乘客數量。）</param>
        /// <param name="vehicleCount">The total number of vehciles serving the route.（服務該路線的車輛數量。）</param>
        private static void GetVehicleStatistics(Entity route, DynamicBuffer<RouteVehicle> routeVehicles,
                                                 bool countPets, ref BufferLookup<LayoutElement> layoutElementBufferLookup,
                                                 ref BufferLookup<Passenger> passengerBufferLookup, ref BufferLookup<Resources> resourcesBufferLookup,
                                                 ref ComponentLookup<CargoTransport> cargoTransportLookup, ref ComponentLookup<Game.Prefabs.CargoTransportVehicleData> cargoTransportVehicleDataLookup,
                                                 ref ComponentLookup<CurrentRoute> currentRouteLookup, ref ComponentLookup<Pet> petLookup,
                                                 ref ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup, ref ComponentLookup<PublicTransport> publicTransportLookup,
                                                 ref ComponentLookup<Game.Prefabs.PublicTransportVehicleData> publicTransportVehicleLookup,
                                                 out int capacity, out int cargoAmount, out int passengerCount, out int vehicleCount)
        {
            capacity = 0;
            cargoAmount = 0;
            passengerCount = 0;
            vehicleCount = 0;

            for (int i = 0; i < routeVehicles.Length; i++)
            {
                Entity routeVehicle = routeVehicles[i].m_Vehicle;
                if (IsValidRouteVehicle(route, routeVehicle, ref cargoTransportLookup,
                                        ref currentRouteLookup, ref publicTransportLookup))
                {
                    vehicleCount++;

                    // Handle the vehicles with multiple cabins.（處理有多個車廂的車輛。）
                    if (layoutElementBufferLookup.TryGetBuffer(routeVehicle, out DynamicBuffer<LayoutElement> layoutElements))
                    {
                        for (int j = 0; j < layoutElements.Length; j++)
                        {
                            Entity routeVehicleCabin = layoutElements[j].m_Vehicle;
                            if (passengerBufferLookup.TryGetBuffer(routeVehicleCabin, out DynamicBuffer<Passenger> cabinPassengers))
                            {
                                passengerCount += GetVehiclePassengerCount(routeVehicleCabin, countPets, cabinPassengers,
                                                                           ref petLookup, ref prefabRefLookup,
                                                                           ref publicTransportVehicleLookup, out int cabinPassengerCapacity);
                                capacity += cabinPassengerCapacity;
                            }

                            cargoAmount += GetVehicleCargoAmount(routeVehicleCabin, ref resourcesBufferLookup,
                                                                 ref cargoTransportVehicleDataLookup, ref prefabRefLookup, out int cabinCargoCapacity);
                            capacity += cabinCargoCapacity;
                        }
                    }
                    else if (passengerBufferLookup.TryGetBuffer(routeVehicle, out DynamicBuffer<Passenger> vehiclePassengers))
                    {
                        cargoAmount += GetVehicleCargoAmount(routeVehicle, ref resourcesBufferLookup,
                                                             ref cargoTransportVehicleDataLookup, ref prefabRefLookup, out int vehicleCargoCapacity);
                        passengerCount += GetVehiclePassengerCount(routeVehicle, countPets, vehiclePassengers,
                                                                   ref petLookup, ref prefabRefLookup,
                                                                   ref publicTransportVehicleLookup, out int vehiclePassengerCapacity);
                        capacity += vehicleCargoCapacity + vehiclePassengerCapacity;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether a route is valid.
        /// （確認一條運輸服務路線是否有效。）
        /// </summary>
        /// <param name="includeInactive">Whether to consider the inactive routes.（是否考慮未啟用路線。）</param>
        /// <param name="prefabRef">The route prefab reference.（運輸服務路線的預製模板範本。）</param>
        /// <param name="route">The route component.（運輸服務路線組件。）</param>
        /// <param name="validRoutePrefabsDataMap">The map between valid transport routes and their types.（有效的運輸服務路線與其類別的映射表。）</param>
        /// <returns>If true, the route is a valid route.（若為真，則該路線為有效的運輸服務路線。）</returns>
        private static bool IsValidRoute(bool includeInactive, Game.Prefabs.PrefabRef prefabRef, Route route, ref NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData> validRoutePrefabsDataMap, out Game.Prefabs.TransportLineData routeData)
        {
            routeData = default;
            bool invalidity = ((route.m_Flags & RouteFlags.Complete) == 0) ||                                   // Is the route a complete loop?（運輸服務路線是完整的環嗎？）
                              (!includeInactive && RouteUtils.CheckOption(route, RouteOption.Inactive)) ||      // Is the route active?（運輸服務路線啟用了嗎？）
                              !validRoutePrefabsDataMap.TryGetValue(prefabRef.m_Prefab, out routeData);         // Is this route an exportable route?（這是一個可被輸出的運輸服務路線嗎？）
            return !invalidity;
        }

        /// <summary>
        /// Checks whether a vehicle is valid for a specific route.
        /// （確認一臺車輛是否有效，且屬於特定的路線。）
        /// </summary>
        /// <param name="route">The route entity.（路線實體。）</param>
        /// <param name="routeVehicle">The route vehicle entity.（路線車輛實體。）</param>
        /// <param name="cargoTransportLookup">The lookup that searches for <see cref="CargoTransport"/>.（搜尋 <see cref="CargoTransport"/> 的查詢。）</param>
        /// <param name="currentRouteLookup">The lookup that searches for <see cref="CurrentRoute"/>.（搜尋 <see cref="CurrentRoute"/> 的查詢。）</param>
        /// <param name="publicTransportLookup">The lookup that searches for <see cref="PublicTransport"/>.（搜尋 <see cref="PublicTransport"/> 的查詢。）</param>
        /// <returns>If true, the vehicle is a valid vehicle for the provided route.（若為真，則該車輛為指定路線的有效車輛。）</returns>
        private static bool IsValidRouteVehicle(Entity route, Entity routeVehicle,
                                                ref ComponentLookup<CargoTransport> cargoTransportLookup, ref ComponentLookup<CurrentRoute> currentRouteLookup,
                                                ref ComponentLookup<PublicTransport> publicTransportLookup)
        {
            return currentRouteLookup.TryGetComponent(routeVehicle, out CurrentRoute currentRouteComponent) &&          // Does the vehicle has a route?（這臺車輛擁有路線嗎？）
                   currentRouteComponent.m_Route.Equals(route) &&                                                       // Is the route the same as the provided one?（路線與提供的路線相同嗎？）

                   // Is the vehicle a public transport vehicle?（這臺車輛是大眾運輸車輛嗎？） 
                   ((publicTransportLookup.TryGetComponent(routeVehicle, out PublicTransport publicTransportComponent) &&
                     ((publicTransportComponent.m_State & PublicTransportFlags.EnRoute) != 0)) ||

                   // ...or a cargo transport vehicle?（或貨運車輛嗎？）
                    (cargoTransportLookup.TryGetComponent(routeVehicle, out CargoTransport cargoTransportComponent) &&
                    (cargoTransportComponent.m_State & CargoTransportFlags.EnRoute) != 0));
        }

        /// <summary>
        /// Convert a <see cref="Game.Prefabs.TransportType"/> to a <see cref="TransportCategory"/> enum.<br/>
        /// （將一個 <see cref="Game.Prefabs.TransportType"/> 轉換為 <see cref="TransportCategory"/> 枚舉。）
        /// </summary>
        /// <param name="transportType">The input vanilla enum.（輸入的遊戲原版枚舉。）</param>
        /// <returns>Carto's enum.（Carto 的枚舉。）</returns>
        public static TransportCategory ToTransportCategory(Game.Prefabs.TransportType transportType)
        {
            return transportType switch
            {
                // TODO: Ensure the consistency of the vanilla enum after each update.（確保每次更新後和原版枚舉的一致性。）
                Game.Prefabs.TransportType.Airplane => TransportCategory.Airplane,
                Game.Prefabs.TransportType.Bus => TransportCategory.Bus,
                Game.Prefabs.TransportType.Helicopter => TransportCategory.Helicopter,
                Game.Prefabs.TransportType.Ship => TransportCategory.Ship,
                Game.Prefabs.TransportType.Subway => TransportCategory.Subway,
                Game.Prefabs.TransportType.Taxi => TransportCategory.Taxi,
                Game.Prefabs.TransportType.Train => TransportCategory.Train,
                Game.Prefabs.TransportType.Tram => TransportCategory.Tram,
                Game.Prefabs.TransportType.None => TransportCategory.None,
                _ => TransportCategory.None,
            };
        }

        /// <summary>
        /// Write centerline attributes to the designated file.
        /// （寫出中心線屬性至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="entitySyncList">The list of entities, which is the reference of synchronization.（實體的列表，作為同步的參考。）</param>
        /// <param name="fieldMap">The map between the property and the fields.（屬性與欄位的映射表。）</param>
        public void WriteCenterlineDBF(BinaryWriter writer, Options options, HashSet<Property> validatedFields, List<Entity> entitySyncList, out Dictionary<Property, FieldInfo> fieldMap)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Route) && validatedFields.Contains(Property.Name);
            bool hasColor = options.Contains(Property.Color, IO.System.Route) && validatedFields.Contains(Property.Color);
            bool hasLength = options.Contains(Property.Length, IO.System.Route) && validatedFields.Contains(Property.Length);
            bool hasModel = options.Contains(Property.Model, IO.System.Route) && validatedFields.Contains(Property.Model);
            bool hasObject = options.Contains(Property.Object, IO.System.Route) && validatedFields.Contains(Property.Object);
            bool hasPassenger = options.Contains(Property.Passenger, IO.System.Route) && validatedFields.Contains(Property.Passenger);
            bool hasRoute = options.Contains(Property.Route, IO.System.Route) && validatedFields.Contains(Property.Route);
            bool hasStop = options.Contains(Property.Stop, IO.System.Route) && validatedFields.Contains(Property.Stop);
            bool hasTransport = options.Contains(Property.Transport, IO.System.Route) && validatedFields.Contains(Property.Transport);
            bool hasUsage = options.Contains(Property.Usage, IO.System.Route) && validatedFields.Contains(Property.Usage);
            bool hasVehicle = options.Contains(Property.Vehicle, IO.System.Route) && validatedFields.Contains(Property.Vehicle);
            bool hasWeight = options.Contains(Property.Weight, IO.System.Route) && validatedFields.Contains(Property.Weight);

            // Validate native containers integrity.（驗證原生容器的完整性。）
            CommonUtils.ValidateIntegrity(ref _localRouteStats, true);

            // Initialize native containers.（初始化原生容器。）
            NativeParallelHashMap<Entity, int> syncMap = new(_localRouteStats.Length, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<string> routeModels = new();
            List<string> routeNames = new();
            List<string> routeRoutes = new();
            Dictionary<Property, FieldInfo> _fieldMap = new();
            Dictionary<Entity, string> routeAcronymMap = GetRouteAcronyms(options);

            try
            {
                FieldInfo colorField = new("#ZZZZZZ");
                FieldInfo lengthField = new(0f);
                FieldInfo modelField = new(0, 0, false, FieldType.String);
                FieldInfo nameField = new(0, 0, false, FieldType.String);
                FieldInfo objectField = new(0, 0, false, FieldType.String);
                FieldInfo passengerField = new(0);
                FieldInfo routeField = new(0, 0, false, FieldType.String);
                FieldInfo stopField = new(0);
                FieldInfo transportField = new(0, 0, false, FieldType.String);
                FieldInfo usageField = new(0.0001f);
                FieldInfo vehicleField = new(0);
                FieldInfo weightField = new(0);

                for (int i = 0; i < _localRouteStats.Length; i++)
                {
                    RouteStat routeStat = _localRouteStats[i];
                    if (hasName)
                    {
                        string renderedName = _name.GetRenderedLabelName(routeStat.entity);

                        if (!routeStat.hasCustomName)
                        {
                            renderedName = LocaleUtils.Translate($"Assets.ROUTE_NAME[{_prefab.GetPrefabName(routeStat.prefab)}]").Replace("{NUMBER}", routeStat.number.ToString());
                        }

                        routeNames.Add(renderedName);
                        nameField += new FieldInfo(renderedName);
                    }
                    if (hasLength)
                    {
                        lengthField += new FieldInfo(routeStat.length);
                    }
                    if (hasModel)
                    {
                        routeModels.Add(LocaleUtils.TryTranslate($"Assets.NAME[{_prefab.GetPrefabName(routeStat.model)}]", out string translated) ? translated : _prefab.GetPrefabName(routeStat.model));
                        modelField += new FieldInfo(routeModels[^1]);
                    }
                    if (hasObject)
                    {
                        Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? routeStat.Object : CommonUtils.GetFirstMatch(routeStat.Object, IO.IO.FeatureDisplayOrder);
                        objectField += new FieldInfo(displayType.ToString("G"));
                    }
                    if (hasPassenger)
                    {
                        passengerField += new FieldInfo(routeStat.passenger);
                    }
                    if (hasRoute)
                    {
                        string routeNumbering = routeStat.number.ToString("G");
                        if (options.XtmAcronym && routeAcronymMap.TryGetValue(routeStat.entity, out string acronym) && !string.IsNullOrEmpty(acronym)) routeNumbering = acronym;
                        routeRoutes.Add(routeNumbering);
                        routeField += new FieldInfo(routeNumbering);
                    }
                    if (hasStop)
                    {
                        stopField += new FieldInfo(routeStat.stop);
                    }
                    if (hasTransport)
                    {
                        transportField += new FieldInfo(routeStat.transport.ToString("G"));
                    }
                    if (hasVehicle)
                    {
                        vehicleField += new FieldInfo(routeStat.vehicle);
                    }
                    if (hasWeight)
                    {
                        weightField += new FieldInfo(routeStat.weight);
                    }
                }

                if (hasName) _fieldMap.Add(Property.Name, nameField);
                if (hasColor) _fieldMap.Add(Property.Color, colorField);
                if (hasLength)
                {
                    lengthField = Shapefile.ApplyFieldInfoConstraint(Property.Length, lengthField);
                    _fieldMap.Add(Property.Length, lengthField);
                }
                if (hasModel) _fieldMap.Add(Property.Model, modelField);
                if (hasObject) _fieldMap.Add(Property.Object, objectField);
                if (hasPassenger)
                {
                    passengerField = Shapefile.ApplyFieldInfoConstraint(Property.Passenger, passengerField);
                    _fieldMap.Add(Property.Passenger, passengerField);
                }
                if (hasRoute) _fieldMap.Add(Property.Route, routeField);
                if (hasStop)
                {
                    stopField = Shapefile.ApplyFieldInfoConstraint(Property.Stop, stopField);
                    _fieldMap.Add(Property.Stop, stopField);
                }
                if (hasTransport) _fieldMap.Add(Property.Transport, transportField);
                if (hasUsage)
                {
                    usageField = Shapefile.ApplyFieldInfoConstraint(Property.Usage, usageField);
                    _fieldMap.Add(Property.Usage, usageField);
                }
                if (hasVehicle)
                {
                    vehicleField = Shapefile.ApplyFieldInfoConstraint(Property.Vehicle, vehicleField);
                    _fieldMap.Add(Property.Vehicle, vehicleField);
                }
                if (hasWeight)
                {
                    weightField = Shapefile.ApplyFieldInfoConstraint(Property.Weight, weightField);
                    _fieldMap.Add(Property.Weight, weightField);
                }

                // Sync the entity order with that of the .shp file.（與 .shp 檔案的實體順序同步。）
                Shapefile.SyncStatsToIndex(entitySyncList, ref _localRouteStats, ref syncMap);

                Task writerThread = Task.Run(() =>
                {
                    for (int index = 0; index < entitySyncList.Count; index++)
                    {
                        if (!syncMap.TryGetValue(entitySyncList[index], out int i))
                        {
                            _log.Error($"Couldn't find the statistical object of {entitySyncList[index]} at index {index}. 無法找到位於索引值 {index} 的實體 {entitySyncList[index]} 之統計物件。");
                        }

                        RouteStat routeStat = _localRouteStats[i];
                        Entity route = routeStat.entity;
                        writer.Write((byte)32);

                        if (hasName)
                        {
                            Shapefile.WriteRecord(writer, nameField, routeNames[i]);
                        }
                        if (hasColor)
                        {
                            Shapefile.WriteRecord(writer, colorField, $"#{UnityEngine.ColorUtility.ToHtmlStringRGB(routeStat.color)}");
                        }
                        if (hasLength)
                        {
                            Shapefile.WriteRecord(writer, lengthField, routeStat.length);
                        }
                        if (hasModel)
                        {
                            Shapefile.WriteRecord(writer, modelField, routeModels[i]);
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? routeStat.Object : CommonUtils.GetFirstMatch(routeStat.Object, IO.IO.FeatureDisplayOrder);
                            Shapefile.WriteRecord(writer, objectField, displayType.ToString("G"));
                        }
                        if (hasPassenger)
                        {
                            Shapefile.WriteRecord(writer, passengerField, routeStat.passenger);
                        }
                        if (hasRoute)
                        {
                            Shapefile.WriteRecord(writer, routeField, routeRoutes[i]);
                        }
                        if (hasStop)
                        {
                            Shapefile.WriteRecord(writer, stopField, routeStat.stop);
                        }
                        if (hasTransport)
                        {
                            Shapefile.WriteRecord(writer, transportField, routeStat.transport.ToString("G"));
                        }
                        if (hasUsage)
                        {
                            Shapefile.WriteRecord(writer, usageField, routeStat.Usage);
                        }
                        if (hasVehicle)
                        {
                            Shapefile.WriteRecord(writer, vehicleField, routeStat.vehicle);
                        }
                        if (hasWeight)
                        {
                            Shapefile.WriteRecord(writer, weightField, routeStat.weight);
                        }
                    }
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                CommonUtils.Dispose(ref syncMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                fieldMap = _fieldMap;
                CommonUtils.Dispose(ref syncMap);
                Dispose();
            }
        }

        /// <summary>
        /// Write centerline features (geometries and properties) to the designated file.
        /// （寫出中線圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public void WriteCenterlineFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Route);
            bool hasColor = options.Contains(Property.Color, IO.System.Route);
            bool hasLength = options.Contains(Property.Length, IO.System.Route);
            bool hasModel = options.Contains(Property.Model, IO.System.Route);
            bool hasObject = options.Contains(Property.Object, IO.System.Route);
            bool hasPassenger = options.Contains(Property.Passenger, IO.System.Route);
            bool hasRoute = options.Contains(Property.Route, IO.System.Route);
            bool hasStop = options.Contains(Property.Stop, IO.System.Route);
            bool hasTransport = options.Contains(Property.Transport, IO.System.Route);
            bool hasUsage = options.Contains(Property.Usage, IO.System.Route);
            bool hasVehicle = options.Contains(Property.Vehicle, IO.System.Route);
            bool hasWeight = options.Contains(Property.Weight, IO.System.Route);

            // Initialize native containers.（初始化原生容器。）
            int routesCount = _routeQuery.CalculateEntityCount();
            NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap = new(routesCount, Allocator.Persistent);
            CommonUtils.Reset(ref _localRouteStats, routesCount, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<string> routeModels = new();
            List<string> routeNames = new();
            List<string> routeRoutes = new();

            try
            {
                GetRouteStats(options);
                GetCenterlines(options, ref nodeEntityMap);
                Dictionary<Entity, string> routeAcronymMap = GetRouteAcronyms(options);

                // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
                if (hasName || hasModel || hasRoute)
                {
                    for (int i = 0; _localRouteStats.IsCreated && (i < _localRouteStats.Length); i++)
                    {
                        RouteStat routeStat = _localRouteStats[i];
                        string renderedName = _name.GetRenderedLabelName(routeStat.entity);

                        if (!routeStat.hasCustomName)
                        {
                            renderedName = LocaleUtils.Translate($"Assets.ROUTE_NAME[{_prefab.GetPrefabName(routeStat.prefab)}]").Replace("{NUMBER}", routeStat.number.ToString());
                        }

                        routeModels.Add(LocaleUtils.TryTranslate($"Assets.NAME[{_prefab.GetPrefabName(routeStat.model)}]", out string translated) ? translated : _prefab.GetPrefabName(routeStat.model));
                        routeNames.Add(renderedName);

                        string routeNumbering = routeStat.number.ToString("G");
                        if (options.XtmAcronym && routeAcronymMap.TryGetValue(routeStat.entity, out string acronym) && !string.IsNullOrEmpty(acronym)) routeNumbering = acronym;
                        routeRoutes.Add(routeNumbering);
                    }
                }

                Task writerThread = Task.Run(() =>
                {
                    for (int i = 0; i < _localRouteStats.Length; i++)
                    {
                        RouteStat routeStat = _localRouteStats[i];
                        Entity route = routeStat.entity;
                        if (!nodeEntityMap.TryGetValue(route, out NativeList<double3> nodes)) continue;

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");

                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        GeoJson.WriteGeometry(writer, new Geometry(ref nodes), Shape.LineString, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();

                        if (hasName)
                        {
                            GeoJson.WriteProperty(writer, Property.Name, routeNames[i]);
                        }
                        if (hasColor)
                        {
                            GeoJson.WriteProperty(writer, Property.Color, $"#{UnityEngine.ColorUtility.ToHtmlStringRGB(routeStat.color)}");
                        }
                        if (hasLength)
                        {
                            GeoJson.WriteProperty(writer, Property.Length, routeStat.length);
                        }
                        if (hasModel)
                        {
                            GeoJson.WriteProperty(writer, Property.Model, routeModels[i]);
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? routeStat.Object : CommonUtils.GetFirstMatch(routeStat.Object, IO.IO.FeatureDisplayOrder);
                            GeoJson.WriteProperty(writer, Property.Object, displayType.ToString("G"));
                        }
                        if (hasPassenger)
                        {
                            GeoJson.WriteProperty(writer, Property.Passenger, routeStat.passenger);
                        }
                        if (hasRoute)
                        {
                            GeoJson.WriteProperty(writer, Property.Route, routeRoutes[i]);
                        }
                        if (hasStop)
                        {
                            GeoJson.WriteProperty(writer, Property.Stop, routeStat.stop);
                        }
                        if (hasTransport)
                        {
                            GeoJson.WriteProperty(writer, Property.Transport, routeStat.transport.ToString("G"));
                        }
                        if (hasUsage)
                        {
                            GeoJson.WriteProperty(writer, Property.Usage, routeStat.Usage);
                        }
                        if (hasVehicle)
                        {
                            GeoJson.WriteProperty(writer, Property.Vehicle, routeStat.vehicle);
                        }
                        if (hasWeight)
                        {
                            GeoJson.WriteProperty(writer, Property.Weight, routeStat.weight);
                        }

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
                CommonUtils.Dispose(ref nodeEntityMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                CommonUtils.Dispose(ref nodeEntityMap);
                Dispose();
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
        public void WriteCenterlineSHP(BinaryWriter writer, Options options, out List<Shapefile.IndexPair> indexPairs, out Bounds3 bounds, out List<Entity> entitySyncList)
        {
            // Initialize native containers.（初始化原生容器。）
            int routesCount = _routeQuery.CalculateEntityCount();
            NativeParallelHashMap<Entity, NativeList<double3>> nodeEntityMap = new(routesCount, Allocator.Persistent);
            CommonUtils.Reset(ref _localRouteStats, routesCount, Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<Entity> _entitySyncList = new();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                GetRouteStats(options);
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
                CommonUtils.Dispose(ref nodeEntityMap);
                IO.IO.DisposeAll();
            }
            finally
            {
                // Don't dispose `_localRouteStats`, it is required in `WriteCenterlineDBF()`.
                // （不要拋棄 `_localRouteStats` ，它仍會被 `WriteCenterlineDBF()` 呼叫。）
                CommonUtils.Dispose(ref nodeEntityMap);
                bounds = _bounds;
                entitySyncList = _entitySyncList;
                indexPairs = _indexPairs;
            }
        }

        /// <summary>
        /// The job to collect route's centerlines.
        /// （收集運輸服務路線中心線的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectCenterlinesJob : IJobParallelFor
        {
            [ReadOnly]
            public BufferLookup<PathElement> pathElementBufferLookup;
            
            [ReadOnly]
            public BufferLookup<RouteSegment> routeSegmentBufferLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public ComponentLookup<OutsideConnection> outsideConnectionLookup;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public NativeList<RouteStat> routeStats;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> nodeCountEntityMap;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeList<double3>>.ParallelWriter nodeEntityMap;

            public void Execute(int index)
            {
                Entity route = routeStats[index].entity;
                if (!nodeCountEntityMap.TryGetValue(route, out int pointCount)) return;

                NativeList<double3> nodes = new(pointCount, Allocator.Persistent);
                if (routeSegmentBufferLookup.TryGetBuffer(route, out DynamicBuffer<RouteSegment> routeSegments))
                {
                    float3 firstPoint = new(float.MaxValue);
                    float3 previousPoint = new(float.MaxValue);
                    double3 previousTransformedPoint = new(double.MaxValue);

                    for (int i = 0; i < routeSegments.Length; i++)
                    {
                        if (pathElementBufferLookup.TryGetBuffer(routeSegments[i].m_Segment, out DynamicBuffer<PathElement> pathElements))
                        {
                            for (int j = 0; j < pathElements.Length; j++)
                            {
                                PathElement path = pathElements[j];
                                if (!curveLookup.TryGetComponent(path.m_Target, out Curve curveComponent) || outsideConnectionLookup.HasComponent(path.m_Target)) continue;
                                
                                Bezier4x3 curve = Utils.MathUtils.Trim(curveComponent.m_Bezier, path.m_TargetDelta.x, path.m_TargetDelta.y);

                                if (firstPoint.Equals(new float3(float.MaxValue))) firstPoint = curve.a;

                                if (!previousPoint.Equals(new float3(float.MaxValue)) && !previousPoint.Equals(curve.a))
                                {
                                    nodes.AddNoResize(previousTransformedPoint);
                                }

                                Utils.MathUtils.Interpolate(curve, ref nodes, 2f, 4f, false, out previousTransformedPoint,
                                                            center, sourceCRS, targetCRS, sourceProjection, targetProjection);
                                previousPoint = curve.d;
                            }
                        }
                    }

                    if (!previousPoint.Equals(firstPoint)) nodes.AddNoResize(previousTransformedPoint);
                }

                nodeEntityMap.TryAdd(route, nodes);
            }
        }

        /// <summary>
        /// The job to collect the exportable transportation route prefabs.
        /// （收集可輸出的運輸服務路線預製模板。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectRoutePrefabsJob : IJobEntity
        {
            [ReadOnly]
            public Feature acceptedRouteTypes;

            [ReadOnly]
            public NativeParallelHashSet<EnumWrapper<Game.Prefabs.TransportType>> exportableTransportTypes;

            [WriteOnly]
            public NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData>.ParallelWriter validRoutePrefabsDataMap;

            public void Execute(in Game.Prefabs.TransportLineData transportLineData, Entity routePrefab)
            {
                if (!exportableTransportTypes.Contains(new(transportLineData.m_TransportType))) return;

                byte routeFlag = 0;
                if (transportLineData.m_CargoTransport) routeFlag += 1;
                if (transportLineData.m_PassengerTransport) routeFlag += 2;

                if (routeFlag == 0) return;

                byte allowFlag = 0;
                if ((acceptedRouteTypes & Feature.RouteCargo) != 0) allowFlag += 1;
                if ((acceptedRouteTypes & Feature.RoutePassenger) != 0) allowFlag += 2;

                if ((routeFlag & allowFlag) != 0) validRoutePrefabsDataMap.TryAdd(routePrefab, transportLineData);
            }
        }

        /// <summary>
        /// The job to collect route statistics.
        /// （收集運輸服務路線統計資料的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectRouteStatsJob : IJobEntity
        {
            [ReadOnly]
            public bool countPets;
            
            [ReadOnly]
            public bool includeInactive;

            [ReadOnly]
            public BufferLookup<LayoutElement> layoutElementBufferLookup;

            [ReadOnly]
            public BufferLookup<Passenger> passengerBufferLookup;

            [ReadOnly]
            public BufferLookup<Resources> resourcesBufferLookup;

            [ReadOnly]
            public ComponentLookup<CargoTransport> cargoTransportLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.CargoTransportVehicleData> cargoTransportVehicleDataLookup;

            [ReadOnly]
            public ComponentLookup<Connected> connectedLookup;

            [ReadOnly]
            public ComponentLookup<CurrentRoute> currentRouteLookup;

            [ReadOnly]
            public ComponentLookup<CustomName> customNameLookup;

            [ReadOnly]
            public ComponentLookup<PathInformation> pathInformationLookup;

            [ReadOnly]
            public ComponentLookup<Pet> petLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<PublicTransport> publicTransportLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.PublicTransportVehicleData> publicTransportVehicleDataLookup;

            [ReadOnly]
            public ComponentLookup<TaxiStand> taxiStandLookup;

            [ReadOnly]
            public ComponentLookup<TransportStop> transportStopLookup;

            [ReadOnly]
            public ComponentLookup<VehicleModel> vehicleModelLookup;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData> validRoutePrefabsDataMap;

            [WriteOnly]
            public NativeList<RouteStat>.ParallelWriter routeStats;

            public void Execute(in Game.Prefabs.PrefabRef prefabRef, in Route routeComponent, in Color routeColor, in RouteNumber routeNumber, Entity route,
                                in DynamicBuffer<RouteSegment> routeSegments, in DynamicBuffer<RouteVehicle> routeVehicles,
                                in DynamicBuffer<RouteWaypoint> routeWaypoints)
            {
                if (!IsValidRoute(includeInactive, prefabRef, routeComponent, ref validRoutePrefabsDataMap, out Game.Prefabs.TransportLineData routeData)) return;

                GetVehicleStatistics(route, routeVehicles, countPets,
                                     ref layoutElementBufferLookup, ref passengerBufferLookup,
                                     ref resourcesBufferLookup, ref cargoTransportLookup,
                                     ref cargoTransportVehicleDataLookup, ref currentRouteLookup,
                                     ref petLookup, ref prefabRefLookup,
                                     ref publicTransportLookup, ref publicTransportVehicleDataLookup,
                                     out int capacity, out int cargoAmount, out int passengerCount, out int vehicleCount);

                RouteStat stat = new()
                {
                    entity = route,
                    capacity = capacity,
                    color = routeColor.m_Color,
                    hasCustomName = customNameLookup.HasComponent(route),
                    isCargo = routeData.m_CargoTransport,
                    isPassenger = routeData.m_PassengerTransport,
                    length = GetRouteLength(routeSegments, ref pathInformationLookup),
                    model = vehicleModelLookup.TryGetComponent(route, out VehicleModel modelComponent) ? modelComponent.m_PrimaryPrefab : default,
                    number = routeNumber.m_Number,
                    passenger = passengerCount,
                    prefab = prefabRef.m_Prefab,
                    stop = GetStopCount(routeWaypoints, ref connectedLookup, ref taxiStandLookup, ref transportStopLookup),
                    transport = ToTransportCategory(routeData.m_TransportType),
                    vehicle = vehicleCount,
                    weight = cargoAmount
                };

                routeStats.AddNoResize(stat);
            }
        }

        /// <summary>
        /// The job to count the number of points required to sketch the routes.
        /// （計算描繪運輸服務路線所需點數的工作。）
        /// </summary>
        public partial struct CountCenterlineNodesJob : IJobParallelFor
        {
            [ReadOnly]
            public BufferLookup<PathElement> pathElementBufferLookup;

            [ReadOnly]
            public BufferLookup<RouteSegment> routeSegmentBufferLookup;

            [ReadOnly]
            public ComponentLookup<Curve> curveLookup;

            [ReadOnly]
            public NativeList<RouteStat> routeStats;

            [WriteOnly]
            public NativeParallelHashMap<Entity, int>.ParallelWriter nodeCountEntityMap;

            public void Execute(int index)
            {
                Entity route = routeStats[index].entity;
                int count = 0;

                if (routeSegmentBufferLookup.TryGetBuffer(route, out DynamicBuffer<RouteSegment> routeSegments))
                {
                    for (int i = 0; i < routeSegments.Length; i++)
                    {
                        if (pathElementBufferLookup.TryGetBuffer(routeSegments[i].m_Segment, out DynamicBuffer<PathElement> pathElements))
                        {
                            for (int j = 0; j < pathElements.Length; j++)
                            {
                                PathElement path = pathElements[j];
                                if (!curveLookup.TryGetComponent(path.m_Target, out Curve curveComponent)) continue;
                                count += Utils.MathUtils.CountInterpolationPoints(Utils.MathUtils.Trim(curveComponent.m_Bezier, path.m_TargetDelta.x, path.m_TargetDelta.y), 2f, 4f) + 1;
                            }
                        }
                    }
                }

                nodeCountEntityMap.TryAdd(route, count);
            }
        }
    }
}