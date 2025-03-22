using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Carto.Utils;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Areas;
using Game.Common;
using Game.Tools;
using Game.UI;
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
    /// The system that searches area.
    /// （搜尋區域的系統。）
    /// </summary>
    public partial class AreaSystem : GameSystemBase
    {
        /// <summary>
        /// The exclusion filters in the entity query.
        /// （實體查詢中排除的篩選條件。）
        /// </summary>
        static readonly List<ComponentType> _filters = new()
        {
            ComponentType.ReadOnly<Deleted>(),
            ComponentType.ReadOnly<Extractor>(),
            ComponentType.ReadOnly<Navigation>(),
            ComponentType.ReadOnly<Space>(),
            ComponentType.ReadOnly<Storage>(),
            ComponentType.ReadOnly<Surface>(),
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
        /// The query for existing map tiles.（現有地圖區塊的查詢。）
        /// </summary>
        static EntityQuery _mapTileQuery;

        /// <summary>
        /// The query for existing areas.（現有區域的查詢。）
        /// </summary>
        static EntityQueryDesc _queryDesc;

        /// <summary>
        /// The system collecting shared data.（收集共享資料的系統。）<br/>
        /// See <see cref="Instance.Shared"/> for more information.
        /// </summary>
        static readonly SharedDataCollectionSystem _shared = Instance.Shared;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _mapTileQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>(),
                    ComponentType.ReadOnly<MapTile>()
                },
                None = _filters.ToArray()
            });
            
            _queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>()
                }
            };

            base.OnCreate();
            _log.Debug("AreaSystem instance created. 區域系統實例創造完成。");
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
        /// Write boundary features (geometries and properties) to the designated file.
        /// （寫出邊界圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public void WriteBoundaryFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            Feature featureFlag = options.Features;
            if (!featureFlag.HasFlag(Feature.District)) _filters.Add(ComponentType.ReadOnly<District>());
            if (!featureFlag.HasFlag(Feature.MapTile)) _filters.Add(ComponentType.ReadOnly<MapTile>());
            _queryDesc.None = _filters.ToArray();
            EntityQuery query = GetEntityQuery(_queryDesc);

            bool hasName = options.Contains(Property.Name, IO.System.Area);
            bool hasAge = options.Contains(Property.Age, IO.System.Area);
            bool hasArea = options.Contains(Property.Area, IO.System.Area);
            bool hasCompany = options.Contains(Property.Company, IO.System.Area);
            bool hasEmployee = options.Contains(Property.Employee, IO.System.Area);
            bool hasHousehold = options.Contains(Property.Household, IO.System.Area);
            bool hasLabor = options.Contains(Property.Labor, IO.System.Area);
            bool hasObject = options.Contains(Property.Object, IO.System.Area);
            bool hasProfit = options.Contains(Property.Profit, IO.System.Area);
            bool hasResident = options.Contains(Property.Resident, IO.System.Area);
            bool hasSexRatio = options.Contains(Property.SexRatio, IO.System.Area);
            bool hasUnlocked = options.Contains(Property.Unlocked, IO.System.Area);
            bool hasWage = options.Contains(Property.Wage, IO.System.Area);
            bool hasStatistics = hasAge | hasCompany | hasEmployee | hasHousehold | hasLabor | hasProfit | hasResident | hasSexRatio | hasWage;

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<BuildingStat> buildingStats = ref _shared.BuildingStats;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref buildingStats, true);

            // Initialize native containers.（初始化原生容器。）
            int areaCount = query.CalculateEntityCount();
            NativeArray<float3> locations = new(buildingStats.Length, Allocator.Persistent);
            NativeList<AreaStat> areaStats = new(areaCount, Allocator.Persistent);
            NativeList<NativeText> areaNames = new(areaCount, Allocator.Persistent);
            NativeList<BVHUtils.Triangle> triangles = new(_mapTileQuery.CalculateEntityCount() * 4, Allocator.Persistent);
            NativeParallelHashMap<Entity, NativeArray<float3>> nodeEntityMap = new(areaCount, Allocator.Persistent);
            NativeParallelMultiHashMap<Entity, int> areaEntityMap = new(buildingStats.Length * 2, Allocator.Persistent);

            try
            {
                // Only execute this part when any of these fields are required: age, company, employee, household, labor, profit, resident, sex ratio, and wage.
                //（僅在需要下列任何欄位時執行：年齡、公司、員工、家庭、勞工、利潤、居民、性別比、薪資）
                if (hasStatistics)
                {
                    // Map each building to districts.（將各棟建築映射至行政區。）
                    MapBuildingsToDistrictsJob mapDistrictsJob = new()
                    {
                        currentDistrictLookup = GetComponentLookup<CurrentDistrict>(true),
                        buildingStats = buildingStats,
                        hashmap = areaEntityMap.AsParallelWriter()
                    };
                    JobHandle mapDistrictsHandle = mapDistrictsJob.Schedule(buildingStats.Length, 8);
                    mapDistrictsHandle.Complete();

                    // Only execute this part when map tile requires statistical fields.
                    //（僅在地圖區塊需要統計欄位時執行。）
                    if (options.StatisticsMapTile)
                    {
                        // Extract building centroids.（萃取建築中點。）
                        GetBuildingLocationsJob getLocationsJob = new()
                        {
                            transformLookup = GetComponentLookup<Game.Objects.Transform>(true),
                            buildingStats = buildingStats,
                            locations = locations
                        };
                        JobHandle getLocationHandle = getLocationsJob.Schedule(buildingStats.Length, 8);
                        getLocationHandle.Complete();

                        // Extract map tile triangles.（萃取地圖區塊三角形。）
                        // Can't apply parallel operation on this job since the length of `triangles` is unknown.（無法在這個工作上執行平行處理，因為 `triangle` 的長度未知。）
                        RetrieveTrianglesJob retrieveJob = new()
                        {
                            triangleList = triangles
                        };
                        JobHandle retrieveHandle = retrieveJob.Schedule(_mapTileQuery, default);
                        retrieveHandle.Complete();

                        // Map each building to map tiles.（將各建築映射至地圖區塊。）
                        BVHUtils.GetIntersectMap(ref triangles, ref locations, ref areaEntityMap);
                    }
                }

                // Collect the statistics of each area.（收集各個區域的統計資料。）
                CollectAreaStatsJob collectStatsJob = new()
                {
                    districtLookup = GetComponentLookup<District>(true),
                    mapTileLookup = GetComponentLookup<MapTile>(true),
                    nativeLookup = GetComponentLookup<Native>(true),
                    areaEntityMap = areaEntityMap,
                    buildingStats = buildingStats,
                    statslist = areaStats.AsParallelWriter()
                };
                JobHandle collectStatsHandle = collectStatsJob.ScheduleParallel(query, default);
                collectStatsHandle.Complete();

                // Collect the boundary of each area.（收集各個區域的邊界。）
                CollectBoundariesJob collectBoundariesJob = new()
                {
                    nodeEntityMap = nodeEntityMap.AsParallelWriter()
                };
                JobHandle collectBoundariesHandle = collectBoundariesJob.ScheduleParallel(query, default);
                collectBoundariesHandle.Complete();

                // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
                if (hasName)
                {
                    for (int i = 0; i < areaStats.Length; i++)
                    {
                        AreaStat stat = areaStats[i];
                        areaNames[i] = new(stat.objectType == Feature.District ? _name.GetRenderedLabelName(stat.entity) : _name.GetDebugName(stat.entity), Allocator.Persistent);
                    }
                }

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    Coord referenceCoord = options.GetTMCoord();
                    CRS referenceProjection = options.GetTMProjection();
                    ProjectionDefinition referenceProjectionDefinition = options.GetTMProjectionDefinition();
                    
                    for (int i = 0; i < areaStats.Length; i++)
                    {
                        AreaStat stat = areaStats[i];
                        if (!nodeEntityMap.TryGetValue(stat.entity, out NativeArray<float3> areaNodes)) continue;
                        float3[] transformedAreaNodes = new float3[areaNodes.Length];

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");
                        
                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        for (int j = 0; j < areaNodes.Length; j++)
                        {
                            Coord coord = new(referenceCoord.Double3 + areaNodes[j], referenceCoord);
                            transformedAreaNodes[j] = Transform.Apply(coord, referenceProjection, CRS.WGS84, referenceProjectionDefinition, new ProjectionDefinition()).Float3;
                        }

                        GeoJson.WriteGeometry(writer, new Geodata.Geometry(new float3[1][] { transformedAreaNodes }), Shape.Polygon, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();
                        if (hasName)
                        {
                            GeoJson.WriteProperty(writer, Property.Name, areaNames[i].ToString());
                        }
                        if (hasAge)
                        {
                            float age = (stat.residentFemale + stat.residentMale <= 0) ? 0f : (float)Math.Round(stat.age / (stat.residentFemale + stat.residentMale), 1);
                            GeoJson.WriteProperty(writer, Property.Age, age);
                        }
                        if (hasArea)
                        {
                            GeoJson.WriteProperty(writer, Property.Area, stat.area);
                        }
                        if (hasCompany)
                        {
                            GeoJson.WriteProperty(writer, Property.Company, stat.company);
                        }
                        if (hasEmployee)
                        {
                            GeoJson.WriteProperty(writer, Property.Employee, stat.employee);
                        }
                        if (hasHousehold)
                        {
                            GeoJson.WriteProperty(writer, Property.Household, stat.household);
                        }
                        if (hasLabor)
                        {
                            GeoJson.WriteProperty(writer, Property.Labor, stat.labor);
                        }
                        if (hasObject)
                        {
                            Feature displayType = options.Display[(Property.Object, IO.System.Unknown)] ? stat.objectType : Utils.CommonUtils.GetFirstMatch(stat.objectType, IO.IO.FeatureDisplayOrder);
                            GeoJson.WriteProperty(writer, Property.Object, displayType.ToString("G"));
                        }
                        if (hasProfit)
                        {
                            float profit = stat.company <= 0 ? 0f : (float)Math.Round((double)stat.profit / stat.company, 2);
                            GeoJson.WriteProperty(writer, Property.Profit, profit);
                        }
                        if (hasResident)
                        {
                            if (options.SeparateResident)
                            {
                                GeoJson.WriteProperty(writer, Property.Resident, new int[2] { stat.residentFemale, stat.residentMale }, options);
                            }
                            else
                            {
                                GeoJson.WriteProperty(writer, Property.Resident, stat.residentFemale + stat.residentMale);
                            }
                        }
                        if (hasSexRatio)
                        {
                            float sexRatio = stat.residentFemale <= 0 ? 0f : (float)Math.Round((double)stat.residentMale / stat.residentFemale * 100, 4);
                            GeoJson.WriteProperty(writer, Property.SexRatio, sexRatio);
                        }
                        if (hasUnlocked)
                        {
                            GeoJson.WriteProperty(writer, Property.Unlocked, stat.unlocked);
                        }
                        if (hasWage)
                        {
                            float wage = stat.labor <= 0 ? 0f : (float)Math.Round((double)stat.wage / stat.labor, 2);
                            GeoJson.WriteProperty(writer, Property.Wage, wage);
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
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref areaEntityMap);
                Utils.CommonUtils.Dispose(ref areaNames);
                Utils.CommonUtils.Dispose(ref areaStats);
                Utils.CommonUtils.Dispose(ref locations);
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                Instance.Shared.Dispose(DisposePhase.AfterAreaSystem);
            }
        }

        /// <summary>
        /// Write boundary attributes to the designated file.
        /// （寫出邊界屬性至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="fieldLengthMap">The map between the property and the field lengths.（屬性與欄位長度的映射表。）</param>
        public void WriteBoundaryDBF(BinaryWriter writer, Options options, HashSet<Property> validatedFields, out Dictionary<Property, List<Shapefile.FieldLength>> fieldLengthMap)
        {
            fieldLengthMap = new();
        }

        /// <summary>
        /// Write boundary geometries to the designated file.
        /// （寫出邊界幾何至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        public void WriteBoundarySHP(BinaryWriter writer, Options options, out List<Shapefile.IndexPair> indexPairs, out Bounds3 bounds)
        {
            Feature featureFlag = options.Features;
            if (!featureFlag.HasFlag(Feature.District)) _filters.Add(ComponentType.ReadOnly<District>());
            if (!featureFlag.HasFlag(Feature.MapTile)) _filters.Add(ComponentType.ReadOnly<MapTile>());
            _queryDesc.None = _filters.ToArray();
            EntityQuery query = GetEntityQuery(_queryDesc);

            // Initialize native containers.（初始化原生容器。）
            int areaCount = query.CalculateEntityCount();
            NativeParallelHashMap<Entity, NativeArray<float3>> nodeEntityMap = new(areaCount, Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                CollectBoundariesJob collectBoundariesJob = new()
                {
                    nodeEntityMap = nodeEntityMap.AsParallelWriter()
                };
                JobHandle collectBoundariesHandle = collectBoundariesJob.ScheduleParallel(query, default);
                collectBoundariesHandle.Complete();
                
                Task writerThread = Task.Run(() =>
                {
                    Coord referenceCoord = options.GetTMCoord();
                    CRS referenceProjection = options.GetTMProjection();
                    int enumeratorIndex = 0;
                    int shapeId = Shapefile.GetShapeType(VectorKind.Boundary, options.Elevation);
                    NativeParallelHashMap<Entity, NativeArray<float3>>.Enumerator enumerator = nodeEntityMap.GetEnumerator();
                    ProjectionDefinition referenceProjectionDefinition = options.GetTMProjectionDefinition();

                    if (BitConverter.IsLittleEndian)
                    {
                        while (enumerator.MoveNext())
                        {
                            enumeratorIndex++;
                            KeyValue<Entity, NativeArray<float3>> feature = enumerator.Current;
                            float3[] transformedAreaNodes = new float3[feature.Value.Length];

                            for (int i = 0; i < feature.Value.Length; i++)
                            {
                                Coord coord = new(referenceCoord.Double3 + feature.Value[i], referenceCoord);
                                transformedAreaNodes[i] = Transform.Apply(coord, referenceProjection, options.TargetProjection, referenceProjectionDefinition, options.TargetProjectionDefinition).Float3;
                            }

                            Shapefile.WriteGeometryLE(writer, enumeratorIndex, shapeId, new(new float3[1][] { transformedAreaNodes }), out Shapefile.IndexPair indexPair, out Bounds3 featureBounds);
                            _bounds |= featureBounds;
                            _indexPairs.Add(indexPair);
                        }
                    }

                    // TODO: Implement Shapefile.WriteGeometryBE()
                });
                writerThread.Wait();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                Utils.CommonUtils.Dispose(ref nodeEntityMap);
                bounds = _bounds;
                indexPairs = _indexPairs;
            }
        }

        /// <summary>
        /// The job to collect and aggregate area statistics.
        /// （收集並聚合區域統計的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectAreaStatsJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<District> districtLookup;

            [ReadOnly]
            public ComponentLookup<MapTile> mapTileLookup;

            [ReadOnly]
            public ComponentLookup<Native> nativeLookup;

            [ReadOnly]
            public NativeParallelMultiHashMap<Entity, int> areaEntityMap;

            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [WriteOnly]
            public NativeList<AreaStat>.ParallelWriter statslist;

            public void Execute(in Game.Areas.Geometry geometry, Entity area)
            {
                AreaStat stat = new()
                {
                    entity = area,
                    age = 0f,
                    area = geometry.m_SurfaceArea,
                    company = 0,
                    employee = 0,
                    household = 0,
                    labor = 0,
                    objectType = Feature.None,
                    profit = 0,
                    residentFemale = 0,
                    residentMale = 0,
                    wage = 0,
                    unlocked = false
                };

                if (areaEntityMap.TryGetFirstValue(area, out int buildingIndex, out NativeParallelMultiHashMapIterator<Entity> iterator))
                {
                    do
                    {
                        if ((buildingIndex >= 0) & (buildingIndex < buildingStats.Length))
                        {
                            ref BuildingStat building = ref buildingStats.ElementAt(buildingIndex);
                            stat.age += building.age;
                            stat.company += building.company;
                            stat.employee += building.employee;
                            stat.household += building.household;
                            stat.labor += building.labor;
                            stat.profit += building.profit;
                            stat.residentFemale += building.residentFemale;
                            stat.residentMale += building.residentMale;
                            stat.wage += building.wage;
                        }
                    }
                    while (areaEntityMap.TryGetNextValue(out buildingIndex, ref iterator));
                }

                if (districtLookup.TryGetComponent(area, out _)) stat.objectType |= Feature.District;
                if (mapTileLookup.TryGetComponent(area, out _)) stat.objectType |= Feature.MapTile;

                if (mapTileLookup.TryGetComponent(area, out _) && !nativeLookup.TryGetComponent(area, out _))
                {
                    stat.unlocked = true;
                }

                statslist.AddNoResize(stat);
            }
        }

        /// <summary>
        /// The job to collect area's boundaries.
        /// （收集區域邊界的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectBoundariesJob : IJobEntity
        {
            [WriteOnly]
            public NativeParallelHashMap<Entity, NativeArray<float3>>.ParallelWriter nodeEntityMap;

            public void Execute(in Area areaComponent, in DynamicBuffer<Node> nodes, Entity area)
            {
                NativeArray<float3> nodesArray = new(nodes.Length, Allocator.Persistent);
                if ((areaComponent.m_Flags & AreaFlags.CounterClockwise) != 0)
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodesArray[i] = nodes[i].m_Position.xzy;
                    }
                }
                else
                {
                    for (int i = nodes.Length - 1; i > -1; i--)
                    {
                        nodesArray[i] = nodes[i].m_Position.xzy;
                    }
                }

                nodeEntityMap.TryAdd(area, nodesArray);
            }
        }

        /// <summary>
        /// The job to extract building centroid positions.
        /// （萃取建築中點位置的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct GetBuildingLocationsJob : IJobParallelFor
        {
            [ReadOnly]
            public ComponentLookup<Game.Objects.Transform> transformLookup; 
            
            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [WriteOnly]
            public NativeArray<float3> locations;

            public void Execute(int index)
            {
                Entity building = buildingStats[index].entity;
                if (building != Entity.Null)
                {
                    if (transformLookup.TryGetComponent(building, out Game.Objects.Transform transform))
                    {
                        locations[index] = transform.m_Position;
                        return;
                    }
                }

                // No match. Assign the max value so it won't be mapped.（沒有符合的項目。填入極大值以避免被錯誤的輸出。）
                locations[index] = new(float.MaxValue);
            }
        }

        /// <summary>
        /// The job to map each building to districts.
        /// （映射每棟建築至行政區的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct MapBuildingsToDistrictsJob : IJobParallelFor
        {
            [ReadOnly]
            public ComponentLookup<CurrentDistrict> currentDistrictLookup;
            
            [ReadOnly]
            public NativeList<BuildingStat> buildingStats;

            [WriteOnly]
            public NativeParallelMultiHashMap<Entity, int>.ParallelWriter hashmap;

            public void Execute(int index)
            {
                Entity building = buildingStats[index].entity;
                if (currentDistrictLookup.TryGetComponent(building, out CurrentDistrict currentDistrict))
                {
                    if (currentDistrict.m_District != Entity.Null)
                    {
                        hashmap.Add(currentDistrict.m_District, index);
                    }
                }
            }
        }

        /// <summary>
        /// The job to retrieve map tiles' triangles.
        /// （獲得組成地圖區塊三角形的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct RetrieveTrianglesJob : IJobEntity
        {
            [WriteOnly]
            public NativeList<BVHUtils.Triangle> triangleList;

            public void Execute(in DynamicBuffer<Node> nodes, in DynamicBuffer<Triangle> triangles, Entity area)
            {
                for (int i = 0; i < triangles.Length; i++)
                {
                    BVHUtils.Triangle triangle = new(AreaUtils.GetTriangle2(nodes, triangles[i]), area);
                    triangleList.Add(triangle);
                }
            }
        }
    }
}