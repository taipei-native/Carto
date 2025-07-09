using Carto.Domain;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Game;
using Game.Creatures;
using Game.Pathfind;
using Game.Routes;
using Game.Tools;
using Game.UI;
using Game.Vehicles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

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
        /// The query to collect human instances.
        /// （收集人類實例的查詢。）
        /// </summary>
        static EntityQuery _humanQuery;

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
        /// The map between route pieces and the routes.
        /// （運輸路線片段與路線的映射表。）
        /// </summary>
        //private NativeParallelHashMap<Bezier4x3, NativeList<Entity>> _curveEntityMap;

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
            _humanQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Creature>(),
                    ComponentType.ReadOnly<Human>(),
                    ComponentType.ReadOnly<HumanCurrentLane>(),
                    ComponentType.ReadOnly<PathElement>(),
                    ComponentType.ReadOnly<PathOwner>(),
                    ComponentType.ReadOnly<Queue>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Game.Common.Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            
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
            //CommonUtils.Dispose(ref _curveEntityMap);
            CommonUtils.Dispose(ref _localRouteStats);
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
            NativeList<Entity> queueRoutes = new(_humanQuery.CalculateEntityCount(), Allocator.Persistent);
            NativeParallelHashMap<Entity, int> routeStopWaitingPassengersMap = new(_routeQuery.CalculateEntityCount(), Allocator.Persistent);
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

            if (!countPets)
            {
                MapWaitingPassengersToRoutesJob mapJob = new()
                {
                    includeInactive = includeInactive,
                    connectedLookup = GetComponentLookup<Connected>(true),
                    ownerLookup = GetComponentLookup<Game.Common.Owner>(true),
                    prefabRefLookup = GetComponentLookup<Game.Prefabs.PrefabRef>(true),
                    routeLookup = GetComponentLookup<Route>(true),
                    taxiStandLookup = GetComponentLookup<TaxiStand>(true),
                    transportStopLookup = GetComponentLookup<TransportStop>(true),
                    waypointLookup = GetComponentLookup<Waypoint>(true),
                    validRoutePrefabsDataMap = validRoutePrefabsDataMap,
                    queueRoutes = queueRoutes.AsParallelWriter(),
                };
                JobHandle mapHandle = mapJob.ScheduleParallel(_humanQuery, default);
                mapHandle.Complete();

                AggregateRouteWaitingPassengersJob aggregatePassengersJob = new()
                {
                    queueRoutes = queueRoutes,
                    routeStopWaitingPassengersMap = routeStopWaitingPassengersMap
                };
                JobHandle aggregatePassengersHandle = aggregatePassengersJob.Schedule();    // To avoid race-conditions, only use single thread here.（為了避免競合情形，在此僅使用一個執行緒。）
                aggregatePassengersHandle.Complete();
            }

            CollectRouteStatsJob collectStatsJob = new()
            {
                countPets = countPets,
                includeInactive = includeInactive,
                layoutElementBufferLookup = GetBufferLookup<LayoutElement>(true),
                passengerBufferLookup = GetBufferLookup<Passenger>(true),
                cargoTransportLookup = GetComponentLookup<CargoTransport>(true),
                connectedLookup = GetComponentLookup<Connected>(true),
                currentRouteLookup = GetComponentLookup<CurrentRoute>(true),
                customNameLookup = GetComponentLookup<CustomName>(true),
                pathInformationLookup = GetComponentLookup<PathInformation>(true),
                petLookup = GetComponentLookup<Pet>(true),
                publicTransportLookup = GetComponentLookup<PublicTransport>(true),
                taxiStandLookup = GetComponentLookup<TaxiStand>(true),
                transportStopLookup = GetComponentLookup<TransportStop>(true),
                waitingPassengersLookup = GetComponentLookup<WaitingPassengers>(true),
                routeStopWaitingPassengersMap = routeStopWaitingPassengersMap,
                validRoutePrefabsDataMap = validRoutePrefabsDataMap,
                routeStats = _localRouteStats.AsParallelWriter()
            };
            JobHandle collectStatsHandle = collectStatsJob.ScheduleParallel(_routeQuery, default);
            collectStatsHandle.Complete();

            CommonUtils.Dispose(ref exportableTransportTypes);
            CommonUtils.Dispose(ref queueRoutes);
            CommonUtils.Dispose(ref routeStopWaitingPassengersMap);
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
        /// 
        /// </summary>
        /// <param name="routeWaypoints"></param>
        /// <param name="connectedLookup"></param>
        /// <param name="taxiStandLookup"></param>
        /// <param name="transportStopLookup"></param>
        /// <param name="waitingPassengersLookup"></param>
        /// <returns></returns>
        private static int GetStopPassengerCount(DynamicBuffer<RouteWaypoint> routeWaypoints, ref ComponentLookup<Connected> connectedLookup,
                                                 ref ComponentLookup<TaxiStand> taxiStandLookup, ref ComponentLookup<TransportStop> transportStopLookup,
                                                 ref ComponentLookup<WaitingPassengers> waitingPassengersLookup)
        {
            ///  This is the Carto version of <see cref="Game.UI.InGame.LinesSection.LinesJob"/>.
            /// （這是 Carto 版本的 <see cref="Game.UI.InGame.LinesSection.LinesJob"/>。）

            int count = 0;

            for (int i = 0; i < routeWaypoints.Length; i++)
            {
                if (connectedLookup.TryGetComponent(routeWaypoints[i].m_Waypoint, out Connected connectedComponent))
                {
                    Entity connected = connectedComponent.m_Connected;
                    if (transportStopLookup.HasComponent(connected) && !taxiStandLookup.HasComponent(connected) &&
                        waitingPassengersLookup.TryGetComponent(routeWaypoints[i].m_Waypoint, out WaitingPassengers waitingPassengersComponent))
                    {
                        count += waitingPassengersComponent.m_Count;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Retrieve the number of passengers being transported by the vehicle.
        /// （獲得車輛輸送的乘客數量。）
        /// </summary>
        /// <param name="countPets">Whether to consider pets as passengers or not.（是否將寵物視為乘客？）</param>
        /// <param name="passengers">The buffer of vehicle passengers.（載具乘客的緩衝區。）</param>
        /// <param name="petLookup">The lookup that searches for <see cref="Pet"/>.（搜尋 <see cref="Pet"/> 的查詢。）</param>
        /// <returns>The number of passengers being transported by the vehicle.（車輛輸送的乘客數量。）</returns>
        private static int GetVehiclePassengerCount(bool countPets, DynamicBuffer<Passenger> passengers, ref ComponentLookup<Pet> petLookup)
        {
            int count = 0;

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
        /// <param name="cargoTransportLookup">The lookup that searches for <see cref="CargoTransport"/>.（搜尋 <see cref="CargoTransport"/> 的查詢。）</param>
        /// <param name="currentRouteLookup">The lookup that searches for <see cref="CurrentRoute"/>.（搜尋 <see cref="CurrentRoute"/> 的查詢。）</param>
        /// <param name="petLookup">The lookup that searches for <see cref="Pet"/>.（搜尋 <see cref="Pet"/> 的查詢。）</param>
        /// <param name="publicTransportLookup">The lookup that searches for <see cref="PublicTransport"/>.（搜尋 <see cref="PublicTransport"/> 的查詢。）</param>
        /// <param name="passengerCount">The total number of passengers being transported by the vehicles that belong to the route.（路線車輛正在載運的乘客數量。）</param>
        /// <param name="vehicleCount">The total number of vehciles serving the route.（服務該路線的車輛數量。）</param>
        private static void GetVehicleStatistics(Entity route, DynamicBuffer<RouteVehicle> routeVehicles,
                                                 bool countPets, ref BufferLookup<LayoutElement> layoutElementBufferLookup,
                                                 ref BufferLookup<Passenger> passengerBufferLookup, ref ComponentLookup<CargoTransport> cargoTransportLookup,
                                                 ref ComponentLookup<CurrentRoute> currentRouteLookup, ref ComponentLookup<Pet> petLookup,
                                                 ref ComponentLookup<PublicTransport> publicTransportLookup,
                                                 out int passengerCount, out int vehicleCount)
        {
            passengerCount = 0;
            vehicleCount = 0;

            for (int i = 0; i < routeVehicles.Length; i++)
            {
                Entity routeVehicle = routeVehicles[i].m_Vehicle;
                if (IsValidRouteVehicle(route, routeVehicle, ref cargoTransportLookup,
                                        ref currentRouteLookup, ref publicTransportLookup))
                {
                    vehicleCount++;

                    if (passengerBufferLookup.TryGetBuffer(routeVehicle, out DynamicBuffer<Passenger> passengers))
                    {
                        passengerCount += GetVehiclePassengerCount(countPets, passengers, ref petLookup);
                    }

                    // Handle the vehicles with multiple cabins.（處理有多個車廂的車輛。）
                    if (layoutElementBufferLookup.TryGetBuffer(routeVehicle, out DynamicBuffer<LayoutElement> layoutElements))
                    {
                        for (int j = 0; j < layoutElements.Length; j++)
                        {
                            Entity routeVehicleCabin = layoutElements[j].m_Vehicle;
                            if (!routeVehicleCabin.Equals(routeVehicle) &&          // The passenger count of the engine cabin have already calculated.（引擎車廂的乘客數量已經計算過了。）
                                IsValidRouteVehicle(route, routeVehicleCabin,
                                                    ref cargoTransportLookup, ref currentRouteLookup,
                                                    ref publicTransportLookup) &&
                                passengerBufferLookup.TryGetBuffer(routeVehicleCabin, out DynamicBuffer<Passenger> cabinPassengers))
                            {
                                passengerCount += GetVehiclePassengerCount(countPets, cabinPassengers, ref petLookup);
                            }
                        }
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
        /// Checks whether a stop is valid, and returns the route it belongs to.
        /// （確認運輸站點是否有效，並回傳奇屬於的運輸服務路線。）
        /// </summary>
        /// <param name="stop">The transportation waypoint entity.（運輸路徑點實體。）</param>
        /// <param name="includeInactive">Whether to export inactive transportation routes or not.（是否要輸出未啟用的運輸服務路線？）</param>
        /// <param name="connectedLookup">The lookup that searches for <see cref="Connected"/>.（搜尋 <see cref="Connected"/> 的查詢。）</param>
        /// <param name="ownerLookup">The lookup that searches for <see cref="Game.Common.Owner"/>.（搜尋 <see cref="Game.Common.Owner"/> 的查詢。）</param>
        /// <param name="prefabRefLookup">The lookup that searches for <see cref="Game.Prefabs.PrefabRef"/>.（搜尋 <see cref="Game.Prefabs.PrefabRef"/> 的查詢。）</param>
        /// <param name="routeLookup">The lookup that searches for <see cref="Route"/>.（搜尋 <see cref="Route"/> 的查詢。）</param>
        /// <param name="taxiStandLookup">The lookup that searches for <see cref="TaxiStand"/>.（搜尋 <see cref="TaxiStand"/> 的查詢。）</param>
        /// <param name="transportStopLookup">The lookup that searches for <see cref="TransportStop"/>.（搜尋 <see cref="TransportStop"/> 的查詢。）</param>
        /// <param name="waypointLookup">The lookup that searches for <see cref="Waypoint"/>.（搜尋 <see cref="Waypoint"/> 的查詢。）</param>
        /// <param name="validRoutePrefabsDataMap">The exportable transportation route prefabs.（可輸出的運輸服務路線預製模板。）</param>
        /// <param name="route">The route entity.（路線實體。）</param>
        /// <returns>If true, the stop is a valid stop.（若為真，該站點為有效的運輸站點。）</returns>
        private static bool IsValidStop(Entity stop, bool includeInactive,
                                        ref ComponentLookup<Connected> connectedLookup, ref ComponentLookup<Game.Common.Owner> ownerLookup,
                                        ref ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup, ref ComponentLookup<Route> routeLookup,
                                        ref ComponentLookup<TaxiStand> taxiStandLookup, ref ComponentLookup<TransportStop> transportStopLookup,
                                        ref ComponentLookup<Waypoint> waypointLookup, ref NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData> validRoutePrefabsDataMap,
                                        out Entity route)
        {
            route = Entity.Null;

            // Ensure transport stop integrity.（確保運輸場站完整性。）
            if (stop != Entity.Null)
            {
                if (!waypointLookup.HasComponent(stop) ||
                    !connectedLookup.TryGetComponent(stop, out Connected connectedComponent) ||
                    !transportStopLookup.HasComponent(connectedComponent.m_Connected) ||
                    taxiStandLookup.HasComponent(connectedComponent.m_Connected))
                {
                    return false;
                }
            }

            // Ensure route integrity.（確保運輸服務路線完整性。）
            if (ownerLookup.TryGetComponent(stop, out Game.Common.Owner ownerComponent) &&
                routeLookup.TryGetComponent(ownerComponent.m_Owner, out Route routeComponent) &&
                prefabRefLookup.TryGetComponent(ownerComponent.m_Owner, out Game.Prefabs.PrefabRef prefabRef))
            {
                if (IsValidRoute(includeInactive, prefabRef, routeComponent, ref validRoutePrefabsDataMap, out _))
                {
                    route = ownerComponent.m_Owner;
                    return true;
                }
            }

            return false;
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
            bool hasVehicle = options.Contains(Property.Vehicle, IO.System.Route);

            // Initialize native containers.（初始化原生容器。）
            int routesCount = _routeQuery.CalculateEntityCount();
            CommonUtils.Reset(ref _localRouteStats, routesCount, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<string> routeNames = new();

            try
            {
                GetRouteStats(options);

                for (int i = 0; _localRouteStats.IsCreated && (i < _localRouteStats.Length); i++)
                {
                    RouteStat routeStat = _localRouteStats[i];
                    string renderedName = _name.GetRenderedLabelName(routeStat.entity);

                    if (!routeStat.hasCustomName)
                    {
                        renderedName = LocaleUtils.Translate($"Assets.ROUTE_NAME[{_prefab.GetPrefabName(routeStat.prefab)}]").Replace("{NUMBER}", routeStat.number.ToString());
                    }

                    _log.Info($"{renderedName}  -  {routeStat}");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Collect the exportable transportation route prefabs.
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
            public ComponentLookup<CargoTransport> cargoTransportLookup;

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
            public ComponentLookup<PublicTransport> publicTransportLookup;

            [ReadOnly]
            public ComponentLookup<TaxiStand> taxiStandLookup;

            [ReadOnly]
            public ComponentLookup<TransportStop> transportStopLookup;

            [ReadOnly]
            public ComponentLookup<WaitingPassengers> waitingPassengersLookup;

            [ReadOnly]
            public NativeParallelHashMap<Entity, int> routeStopWaitingPassengersMap;

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
                                     ref cargoTransportLookup, ref currentRouteLookup,
                                     ref petLookup, ref publicTransportLookup,
                                     out int passengerCount, out int vehicleCount);

                if (countPets)
                {
                    passengerCount += GetStopPassengerCount(routeWaypoints, ref connectedLookup,
                                                            ref taxiStandLookup, ref transportStopLookup,
                                                            ref waitingPassengersLookup);
                }
                else
                {
                    if (routeStopWaitingPassengersMap.TryGetValue(route, out int waitingHumanPassengerCount))
                    {
                        passengerCount += waitingHumanPassengerCount;
                    }
                }

                RouteStat stat = new()
                {
                    entity = route,
                    color = routeColor.m_Color,
                    hasCustomName = customNameLookup.HasComponent(route),
                    isCargo = routeData.m_CargoTransport,
                    isPassenger = routeData.m_PassengerTransport,
                    length = GetRouteLength(routeSegments, ref pathInformationLookup),
                    number = routeNumber.m_Number,
                    passenger = passengerCount,
                    prefab = prefabRef.m_Prefab,
                    stop = GetStopCount(routeWaypoints, ref connectedLookup, ref taxiStandLookup, ref transportStopLookup),
                    transport = routeData.m_TransportType,
                    vehicle = vehicleCount
                };

                routeStats.AddNoResize(stat);
            }
        }

        /// <summary>
        /// The job to count the number of individual and 'atomic' route pieces.
        /// （計算獨立、不可細分運輸服務路線片段數量的工作。）
        /// </summary>
        //[BurstCompile]
        //public partial struct CountCurveElementsJob : IJobEntity
        //{
        //    [ReadOnly]
        //    public bool includeInactive;

        //    [ReadOnly]
        //    public BufferLookup<CurveElement> curveElementBufferLookup;

        //    [ReadOnly]
        //    public ComponentLookup<Game.Common.Owner> ownerLookup;

        //    [ReadOnly]
        //    public NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData> validRoutePrefabsDataMap;

        //    [WriteOnly]
        //    public NativeQueue<int>.ParallelWriter queue;

        //    public void Execute(in Game.Prefabs.PrefabRef prefabRef, in Route routeComponent, Entity route, in DynamicBuffer<RouteSegment> routeSegments)
        //    {
        //        if (!IsValidRoute(includeInactive, prefabRef, routeComponent, ref validRoutePrefabsDataMap, out _)) return;

        //        int validCurveElementsCount = 0;
        //        for (int i = 0; i < routeSegments.Length; i++)
        //        {
        //            Entity routeSegment = routeSegments[i].m_Segment;
        //            if (!ownerLookup.TryGetComponent(routeSegment, out Game.Common.Owner ownerEntity) || !ownerEntity.m_Owner.Equals(route) || !curveElementBufferLookup.TryGetBuffer(routeSegment, out DynamicBuffer<CurveElement> curveElements))
        //            {
        //                continue;
        //            }

        //            validCurveElementsCount += curveElements.Length;
        //        }

        //        queue.Enqueue(validCurveElementsCount);
        //    }
        //}

        /// <summary>
        /// The job to aggregate the number of waiting passengers at the stops.
        /// （聚合在運輸站點等待的乘客數量的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct AggregateRouteWaitingPassengersJob : IJob
        {
            [ReadOnly]
            public NativeList<Entity> queueRoutes;

            public NativeParallelHashMap<Entity, int> routeStopWaitingPassengersMap;

            public void Execute()
            {
                for (int i = 0; i < queueRoutes.Length; i++)
                {
                    Entity route = queueRoutes[i];
                    if (routeStopWaitingPassengersMap.TryGetValue(route, out int waitingCount))
                    {
                        routeStopWaitingPassengersMap[route] = waitingCount + 1;
                    }
                    else
                    {
                        routeStopWaitingPassengersMap.TryAdd(route, 1);
                    }
                }
            }
        }

        /// <summary>
        /// The job to map each route pieces to the routes.
        /// （將運輸服務路線片段映射至路線的工作。）
        /// </summary>
        //[BurstCompile]
        //public partial struct MapCurveElementsToRoutesJob : IJobEntity
        //{
        //    [ReadOnly]
        //    public bool includeInactive;

        //    [ReadOnly]
        //    public BufferLookup<CurveElement> curveElementBufferLookup;

        //    [ReadOnly]
        //    public ComponentLookup<Game.Common.Owner> ownerLookup;

        //    [ReadOnly]
        //    public NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData> validRoutePrefabsDataMap;

        //    [WriteOnly]
        //    public NativeParallelMultiHashMap<Bezier4x3, Entity>.ParallelWriter curveEntityMultiMap;

        //    public void Execute(in Game.Prefabs.PrefabRef prefabRef, in Route routeComponent, Entity route, in DynamicBuffer<RouteSegment> routeSegments)
        //    {
        //        if (!IsValidRoute(includeInactive, prefabRef, routeComponent, ref validRoutePrefabsDataMap, out _)) return;

        //        for (int i = 0; i < routeSegments.Length; i++)
        //        {
        //            Entity routeSegment = routeSegments[i].m_Segment;
        //            if (!ownerLookup.TryGetComponent(routeSegment, out Game.Common.Owner ownerEntity) || !ownerEntity.m_Owner.Equals(route) || !curveElementBufferLookup.TryGetBuffer(routeSegment, out DynamicBuffer<CurveElement> curveElements))
        //            {
        //                continue;
        //            }

        //            for (int j = 0; j < curveElements.Length; j++)
        //            {
        //                curveEntityMultiMap.Add(curveElements[j].m_Curve, route);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// The job to help mapping waiting passengers to the routes.
        /// （協助將等待的乘客映射至運輸服務路線的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct MapWaitingPassengersToRoutesJob : IJobEntity
        {
            [ReadOnly]
            public bool includeInactive;

            [ReadOnly]
            public ComponentLookup<Connected> connectedLookup;
            
            [ReadOnly]
            public ComponentLookup<Game.Common.Owner> ownerLookup;

            [ReadOnly]
            public ComponentLookup<Game.Prefabs.PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<Route> routeLookup;

            [ReadOnly]
            public ComponentLookup<TaxiStand> taxiStandLookup;

            [ReadOnly]
            public ComponentLookup<TransportStop> transportStopLookup;

            [ReadOnly]
            public ComponentLookup<Waypoint> waypointLookup;

            [ReadOnly]
            public NativeParallelHashMap<Entity, Game.Prefabs.TransportLineData> validRoutePrefabsDataMap;

            [WriteOnly]
            public NativeList<Entity>.ParallelWriter queueRoutes;
            
            public void Execute(in Creature creature, in HumanCurrentLane humanCurrentLane, in PathOwner pathOwner,
                                in DynamicBuffer<PathElement> pathElements, in DynamicBuffer<Queue> queues)
            {
                ///  This is the Carto version of <see cref="Game.Simulation.WaitingPassengersSystem.CountWaitingPassengersJob"/>.
                /// （這是 Carto 版本的 <see cref="Game.Simulation.WaitingPassengersSystem.CountWaitingPassengersJob"/>。）
                 
                if (CreatureUtils.TransportStopReached(humanCurrentLane))
                {
                    // Ensure passenger path integrity.（確保乘客路徑完整性。）
                    int pathIndex = pathOwner.m_ElementIndex;
                    if ((pathElements.Length < pathIndex + 2) || pathElements[pathIndex].m_Target == Entity.Null)
                    {
                        return;
                    }

                    if ((creature.m_QueueEntity != Entity.Null) &&
                        IsValidStop(creature.m_QueueEntity, includeInactive,
                                    ref connectedLookup, ref ownerLookup,
                                    ref prefabRefLookup, ref routeLookup,
                                    ref taxiStandLookup, ref transportStopLookup,
                                    ref waypointLookup, ref validRoutePrefabsDataMap,
                                    out Entity route))
                    {
                        queueRoutes.AddNoResize(route);
                    }

                    return;
                }

                for (int i = 0; i < queues.Length; i++)
                {
                    Queue queue = queues[i];
                    if ((queue.m_TargetArea.radius > 0f) && (queue.m_TargetEntity != Entity.Null) &&
                        IsValidStop(creature.m_QueueEntity, includeInactive,
                                    ref connectedLookup, ref ownerLookup,
                                    ref prefabRefLookup, ref routeLookup,
                                    ref taxiStandLookup, ref transportStopLookup,
                                    ref waypointLookup, ref validRoutePrefabsDataMap,
                                    out Entity route))
                    {
                        queueRoutes.AddNoResize(route);
                        break;
                    }
                }
            }
        }
    }
}