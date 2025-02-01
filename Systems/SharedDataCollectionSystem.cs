using Carto.Domain;
using Colossal.Logging;
using Game;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Zones;
using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Carto.Systems
{
    /// <summary>
    /// The system that collects shared data across various systems.
    /// （收集多種系統所需之共享資料的系統。）
    /// </summary>
    public partial class SharedDataCollectionSystem : GameSystemBase
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The query to collect all building entities.
        /// （收集所有建築實體的查詢。）
        /// </summary>
        static EntityQuery _buildingQuery;

        /// <summary>
        /// The query to collect all citizen prefabs.
        /// （收集所有市民預製模板的查詢。）
        /// </summary>
        static EntityQuery _citizenPrefabQuery;

        /// <summary>
        /// The query to collect all spawnable building prefabs.
        /// （收集所有自長建築預製模板的查詢。）
        /// </summary>
        static EntityQuery _spawnableBuildingPrefabQuery;

        /// <summary>
        /// The query to find time settings.
        /// （尋找時間設定的查詢。）
        /// </summary>
        static EntityQuery _timeDataQuery;

        /// <summary>
        /// The query to collect all zoning type prefabs.
        /// （收集所有分區類別預製模板的查詢。）
        /// </summary>
        static EntityQuery _zoningPrefabQuery;

        /// <summary>
        /// The list of in-game zoning types' information.
        /// （遊戲內分區類型資訊的列表。）
        /// </summary>
        public NativeList<ZoningType> ZoningTypes { get; private set; } = default;

        /// <summary>
        /// The map between zoning prefab references and their index in <see cref="ZoningTypes"/> and <see cref="ZoningTypesNames"/>.<br/>
        /// （分區預製模板參考與其在 <see cref="ZoningTypes"/> 與 <see cref="ZoningTypesNames"/> 索引值的映射表。）
        /// </summary>
        public NativeParallelHashMap<Entity, int> ZoningTypesEntityMap { get; private set; } = default;

        /// <summary>
        /// The map between zoning ids and their index in <see cref="ZoningTypes"/> and <see cref="ZoningTypesNames"/>.<br/>
        /// （分區識別碼與其在 <see cref="ZoningTypes"/> 與 <see cref="ZoningTypesNames"/> 索引值的映射表。）
        /// </summary>
        public NativeParallelHashMap<ushort, int> ZoningTypesIdMap { get; private set; } = default;

        /// <summary>
        /// The list of in-game zoning types' prefab name.
        /// （遊戲內分區類型名稱的列表。）
        /// </summary>
        public NativeList<NativeText> ZoningTypesNames { get; private set; } = default;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _buildingQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Building>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Placeholder>(),

                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            _citizenPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<CitizenData>()
                }
            });

            _spawnableBuildingPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ObjectData>(),
                    ComponentType.ReadOnly<SpawnableBuildingData>(),
                }
            });

            _timeDataQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<TimeData>()
                }
            });

            _zoningPrefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ZoneData>()
                }
            });

            base.OnCreate();
            _log.Debug("SharedDataCollectionSystem instance created. 共享資料收集系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy()
        {
            Utils.CommonUtils.Dispose(ZoningTypes);
            Utils.CommonUtils.Dispose(ZoningTypesEntityMap);
            Utils.CommonUtils.Dispose(ZoningTypesIdMap);
            Utils.CommonUtils.Dispose(ZoningTypesNames);
            base.OnDestroy();
        }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Retrieve building entities' statistical data.
        /// （獲取建築實體的統計資料。）
        /// </summary>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        /// <returns>The queue with each building's statistics.（包含各建築統計資料的佇列。）</returns>
        public NativeQueue<BuildingStat> GetBuildingStats(Allocator allocator)
        {
            NativeQueue<BuildingStat> queue = new(allocator);
            NativeParallelHashMap<Entity, bool> citizenSex = new(2, allocator);
            TimeData timeData = default;

            // Collect pre-requirements for calculating building statistics.
            // （收集計算建築統計的事前必備項目。）
            CollectCitizenSexJob collectSexJob = new()
            {
                entityType = GetEntityTypeHandle(),
                citizenDataType = GetComponentTypeHandle<CitizenData>(),
                hashmap = citizenSex.AsParallelWriter()
            };
            JobHandle collectSexHandle = collectSexJob.ScheduleParallel(_citizenPrefabQuery, default);
            collectSexHandle.Complete();

            if (_timeDataQuery.TryGetSingleton(out TimeData singleton))
            {
                timeData = singleton;
            }

            CollectBuildingStatsJob collectStatsJob = new()
            {
                entityType = GetEntityTypeHandle(),
                citizenBufferLookup = GetBufferLookup<HouseholdCitizen>(true),
                employeeBufferLookup = GetBufferLookup<Employee>(true),
                employeeBufferType = GetBufferTypeHandle<Employee>(true),
                renterBufferType = GetBufferTypeHandle<Renter>(true),
                citizenLookup = GetComponentLookup<Citizen>(true),
                companyDataLookup = GetComponentLookup<CompanyData>(true),
                healthProblemLookup = GetComponentLookup<HealthProblem>(true),
                householdLookup = GetComponentLookup<Household>(true),
                prefabRefLookup = GetComponentLookup<PrefabRef>(true),
                processLookup = GetComponentLookup<IndustrialProcessData>(true),
                travelPurposeLookup = GetComponentLookup<TravelPurpose>(true),
                currentFrameIndex = Instance.Simulation.frameIndex,
                initialTime = timeData,
                sexHashMap = citizenSex,
                queue = queue.AsParallelWriter()
            };
            JobHandle collectStatsHandle = collectStatsJob.ScheduleParallel(_buildingQuery, default);
            collectStatsHandle.Complete();

            citizenSex.Dispose();
            return queue;
        }

        /// <summary>
        /// The job to collect building statistics.
        /// （收集建築統計資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectBuildingStatsJob : IJobChunk
        {
            [ReadOnly]
            public EntityTypeHandle entityType;

            [ReadOnly]
            public BufferLookup<HouseholdCitizen> citizenBufferLookup;

            [ReadOnly]
            public BufferLookup<Employee> employeeBufferLookup;

            [ReadOnly]
            public BufferTypeHandle<Employee> employeeBufferType;

            [ReadOnly]
            public BufferTypeHandle<Renter> renterBufferType;

            [ReadOnly]
            public ComponentLookup<Citizen> citizenLookup;

            [ReadOnly]
            public ComponentLookup<CompanyData> companyDataLookup;

            [ReadOnly]
            public ComponentLookup<HealthProblem> healthProblemLookup;

            [ReadOnly]
            public ComponentLookup<Household> householdLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> prefabRefLookup;

            [ReadOnly]
            public ComponentLookup<IndustrialProcessData> processLookup;

            [ReadOnly]
            public ComponentLookup<TravelPurpose> travelPurposeLookup;

            [ReadOnly]
            public uint currentFrameIndex;

            [ReadOnly]
            public TimeData initialTime;

            [ReadOnly]
            public NativeParallelHashMap<Entity, bool> sexHashMap;

            [WriteOnly]
            public NativeQueue<BuildingStat>.ParallelWriter queue;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                NativeArray<Entity> entities = chunk.GetNativeArray(entityType);
                BufferAccessor<Employee> employeeBuffers = chunk.GetBufferAccessor(ref employeeBufferType);
                BufferAccessor<Renter> renterBuffers = chunk.GetBufferAccessor(ref renterBufferType);

                // Whether the first shop is recorded or not.（第一間商店是否被記錄了？）
                bool isFirstShop = true;

                for (int i = 0; i < chunk.Count; i++)
                {
                    Entity building = entities[i];
                    BuildingStat stat = new()
                    {
                        entity = building,
                        age = 0f,
                        brand = -1,
                        company = 0,
                        employee = 0,
                        household = 0,
                        level = 0,
                        product = Resource.NoResource,
                        residentFemale = 0,
                        residentMale = 0,
                        zoning = -1
                    };

                    // Check whether the Employee buffer exist in the entity.
                    // （確認 Employee 緩衝區是否存在於實體當中。）
                    if (employeeBuffers.Length > i)
                    {
                        DynamicBuffer<Employee> employeeBuffer = employeeBuffers[i];
                        stat.employee += employeeBuffer.Length;
                    }

                    // Check whether the Renter buffer exist in the entity.
                    // （確認 Renter 緩衝區是否存在於實體當中。）
                    if (renterBuffers.Length > i)
                    {
                        DynamicBuffer<Renter> renterBuffer = renterBuffers[i];
                        for (int j = 0; j < renterBuffer.Length; j++)
                        {
                            Entity renter = renterBuffer[j].m_Renter;

                            if (companyDataLookup.HasComponent(renter))
                            {
                                stat.company++;

                                if (isFirstShop)
                                {
                                    Entity facilityPrefab = prefabRefLookup[renter].m_Prefab;
                                    stat.product = processLookup[facilityPrefab].m_Output.m_Resource;
                                    isFirstShop = false;
                                }
                            }

                            if (employeeBufferLookup.TryGetBuffer(renter, out DynamicBuffer<Employee> employeeBuffer))
                            {
                                stat.employee += employeeBuffer.Length;
                            }

                            if (householdLookup.HasComponent(renter))
                            {
                                stat.household++;
                            }

                            if (citizenBufferLookup.TryGetBuffer(renter, out DynamicBuffer<HouseholdCitizen> citizenBuffer))
                            {
                                for (int k = 0; k < citizenBuffer.Length; k++)
                                {
                                    Entity citizen = citizenBuffer[k].m_Citizen;
                                    if (IsCitizenAlive(citizen))
                                    {
                                        if (!sexHashMap.TryGetValue(prefabRefLookup[citizen].m_Prefab, out bool isMale))
                                        {
                                            continue;
                                        }

                                        if (!citizenLookup.TryGetComponent(citizen, out Citizen citizenComponent))
                                        {
                                            continue;
                                        }

                                        stat.age += citizenComponent.GetAgeInDays(currentFrameIndex, initialTime);

                                        if (isMale)
                                        {
                                            stat.residentMale++;
                                        }
                                        else
                                        {
                                            stat.residentFemale++;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    queue.Enqueue(stat);
                }
            }

            private bool IsCitizenAlive(Entity citizen)
            {
                if (healthProblemLookup.HasComponent(citizen))
                {
                    if ((healthProblemLookup[citizen].m_Flags & HealthProblemFlags.Dead) != 0)
                    {
                        return false;
                    }
                }

                if (travelPurposeLookup.HasComponent(citizen))
                {
                    Purpose purpose = travelPurposeLookup[citizen].m_Purpose;
                    return (purpose != Purpose.Deathcare) & (purpose != Purpose.InDeathcare);
                }

                return true;
            }
        }

        /// <summary>
        /// The job to collect citizen prefab sexes.
        /// （收集市民生理性別資訊的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectCitizenSexJob : IJobChunk
        {
            [ReadOnly]
            public EntityTypeHandle entityType;

            [ReadOnly]
            public ComponentTypeHandle<CitizenData> citizenDataType;

            [WriteOnly]
            public NativeParallelHashMap<Entity, bool>.ParallelWriter hashmap;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                NativeArray<Entity> entities = chunk.GetNativeArray(entityType);
                NativeArray<CitizenData> citizenData = chunk.GetNativeArray(ref citizenDataType);

                for (int i = 0; i < chunk.Count; i++)
                {
                    hashmap.TryAdd(entities[i], citizenData[i].m_Male);
                }
            }
        }

        /// <summary>
        /// Retrieve zoning types' information.
        /// （獲取分區類別的資訊。）
        /// </summary>
        public void GetZoningTypes()
        {
            // Create local copy of properties.（創造屬性的區域副本。）
            NativeParallelHashMap<Entity, int> entityMap = ZoningTypesEntityMap;
            NativeParallelHashMap<ushort, int> idMap = ZoningTypesIdMap;
            NativeList<NativeText> names = ZoningTypesNames;
            NativeList<ZoningType> types = ZoningTypes;

            // Initialize native containers.（初始化原生容器。）
            int zoningTypeCount = _zoningPrefabQuery.CalculateEntityCount();
            NativeParallelHashSet<Entity> zoningTypePool = new(zoningTypeCount, Allocator.TempJob);

            // Reset output containers.（重置輸出容器。）
            Utils.CommonUtils.Reset(ref entityMap, zoningTypeCount);
            Utils.CommonUtils.Reset(ref idMap, zoningTypeCount);
            Utils.CommonUtils.Reset(ref names, zoningTypeCount);
            Utils.CommonUtils.Reset(ref types, zoningTypeCount);

            try
            {
                CollectZoningTypesJob collectJob = new()
                {
                    prefabDataLookup = GetComponentLookup<PrefabData>(),
                    zoneDataLookup = GetComponentLookup<ZoneData>(),
                    list = types.AsParallelWriter(),
                    zoningTypePool = zoningTypePool.AsParallelWriter(),
                    commercialIndex = TypeManager.GetTypeIndex<CommercialProperty>(),
                    industrialIndex = TypeManager.GetTypeIndex<IndustrialProperty>(),
                    officeIndex = TypeManager.GetTypeIndex<OfficeProperty>(),
                    residentialIndex = TypeManager.GetTypeIndex<ResidentialProperty>()
                };
                JobHandle collectHandle = collectJob.ScheduleParallel(_spawnableBuildingPrefabQuery, default);
                collectHandle.Complete();

                VerifyZoningTypesJob verifyJob = new()
                {
                    list = types.AsParallelWriter(),
                    zoningTypePool = zoningTypePool
                };
                JobHandle verifyHandle = verifyJob.ScheduleParallel(_zoningPrefabQuery, default);
                verifyHandle.Complete();

                for (int index = 0; index < zoningTypeCount; index++)
                {
                    ref ZoningType zoningType = ref types.ElementAt(index);
                    ZonePrefab zonePrefabData = Instance.Prefab.GetPrefab<ZonePrefab>(zoningType.prefabData);
                    zoningType.color = zonePrefabData.m_Color;
                    entityMap.TryAdd(zoningType.entity, index);
                    idMap.TryAdd(zoningType.id, index);
                    names.Add(new NativeText(Instance.Prefab.GetPrefabName(zoningType.entity), Allocator.Persistent));
                }

                // Assign properties.（指派屬性。）
                ZoningTypes = types;
                ZoningTypesEntityMap = entityMap;
                ZoningTypesIdMap = idMap;
                ZoningTypesNames = names;
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                if (zoningTypePool.IsCreated)
                {
                    zoningTypePool.Dispose();
                }
            }
        }

        [BurstCompile]
        public partial struct CollectThemesJob : IJobChunk
        {
            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {

            }
        }

        /// <summary>
        /// The job to collect zoning types.
        /// （收集分區類別的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectZoningTypesJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<PrefabData> prefabDataLookup;

            [ReadOnly]
            public ComponentLookup<ZoneData> zoneDataLookup;

            [WriteOnly]
            public NativeList<ZoningType>.ParallelWriter list;

            [WriteOnly]
            public NativeParallelHashSet<Entity>.ParallelWriter zoningTypePool;

            [ReadOnly]
            public TypeIndex commercialIndex;

            [ReadOnly]
            public TypeIndex industrialIndex;

            [ReadOnly]
            public TypeIndex officeIndex;

            [ReadOnly]
            public TypeIndex residentialIndex;

            public void Execute(in ObjectData objectData, in SpawnableBuildingData spawnableData)
            {
                NativeArray<ComponentType> archetypeComponents = default;
                try
                {
                    // Check whether the prefab is already recorded.（確認預製模板是否已被記錄過。）
                    Entity zoningPrefab = spawnableData.m_ZonePrefab;
                    if (!zoningTypePool.Add(zoningPrefab))
                    {
                        return;
                    }

                    // Find out categories.（找出分類。）
                    ZoningCategory categories = ZoningCategory.None;
                    archetypeComponents = objectData.m_Archetype.GetComponentTypes(Allocator.Temp);
                    for (int j = 0; j < archetypeComponents.Length; j++)
                    {
                        TypeIndex componentIndex = archetypeComponents[j].TypeIndex;
                        if (componentIndex == commercialIndex) categories |= ZoningCategory.Commercial;
                        if (componentIndex == industrialIndex) categories |= ZoningCategory.Industrial;
                        if (componentIndex == officeIndex) categories |= ZoningCategory.Office;
                        if (componentIndex == residentialIndex) categories |= ZoningCategory.Residential;
                    }

                    // Remove the industrial zoning from the office zoning.（從辦公分區中移除工業分區。）
                    if ((categories & ZoningCategory.Office) != 0)
                    {
                        categories &= ~ZoningCategory.Industrial;
                    }

                    // Find out density.（找出發展強度。）
                    ZoningDensity density = GetZoningDensity(categories, zoneDataLookup[zoningPrefab]);

                    // Find out id.（找出識別碼。）
                    ushort id = zoneDataLookup[zoningPrefab].m_ZoneType.m_Index;

                    // Insert data.（插入資料。）
                    ZoningType data = new()
                    {
                        entity = zoningPrefab,
                        category = categories,
                        color = new(),
                        density = density,
                        id = id,
                        prefabData = prefabDataLookup[zoningPrefab],
                        theme = -1
                    };
                    list.AddNoResize(data);
                }
                finally
                {
                    if (archetypeComponents.IsCreated)
                    {
                        archetypeComponents.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// The job to verify zoning types integrity.
        /// （驗證分區類別完整性的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct VerifyZoningTypesJob : IJobEntity
        {
            [WriteOnly]
            public NativeList<ZoningType>.ParallelWriter list;

            [ReadOnly]
            public NativeParallelHashSet<Entity> zoningTypePool;

            public void Execute(in PrefabData prefabData, in ZoneData zoneData, Entity zoningPrefab)
            {
                if (!zoningTypePool.Contains(zoningPrefab))
                {
                    // Find out categories.（找出分類。）
                    ZoningCategory categories = zoneData.m_AreaType switch
                    {
                        AreaType.Residential => ZoningCategory.Residential,
                        AreaType.Commercial => ZoningCategory.Commercial,
                        AreaType.Industrial => ZoningCategory.Industrial,
                        _ => ZoningCategory.None,
                    };

                    // Remove the industrial zoning from the office zoning.（從辦公分區中移除工業分區。）
                    if ((zoneData.m_ZoneFlags & ZoneFlags.Office) != 0)
                    {
                        categories |= ZoningCategory.Office;
                        categories &= ~ZoningCategory.Industrial;
                    }

                    // Find out density.（找出發展強度。）
                    ZoningDensity density = GetZoningDensity(categories, zoneData);

                    // Find out id.（找出識別碼。）
                    ushort id = zoneData.m_ZoneType.m_Index;

                    // Insert data.（插入資料。）
                    ZoningType data = new()
                    {
                        entity = zoningPrefab,
                        category = categories,
                        color = new(),
                        density = density,
                        id = id,
                        prefabData = prefabData,
                        theme = -1
                    };
                    list.AddNoResize(data);
                }
            }
        }

        /// <summary>
        /// Get zoning density from its categories and ZoneData component.
        /// （由分區分類與 ZoneData 部件獲得其發展強度。）
        /// </summary>
        /// <param name="categories">The flag indicating zoning categories.（顯示分區分類的旗標。）</param>
        /// <param name="zoneData">The ZoneData component.（ZoneData 部件。）</param>
        /// <returns>A flag indicating zoning density.（顯示分區發展強度的旗標。）</returns>
        private static ZoningDensity GetZoningDensity(ZoningCategory categories, ZoneData zoneData)
        {
            ZoningDensity density = ZoningDensity.Generic;
            ushort heightLimit = zoneData.m_MaxHeight;

            if ((categories & ZoningCategory.Residential) != 0)
            {
                density = heightLimit switch
                {
                    < 12 => ZoningDensity.Low,
                    < 60 => ZoningDensity.Medium,
                    _ => ZoningDensity.High
                };
            }
            else if ((categories & ZoningCategory.Commercial) != 0 | (categories & ZoningCategory.Office) != 0)
            {
                density = heightLimit < 20 ? ZoningDensity.Low : ZoningDensity.High;
            }

            return density;
        }
    }
}