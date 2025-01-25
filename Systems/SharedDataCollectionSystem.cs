using Carto.Domain;
using Colossal.Logging;
using Game;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
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
        static EntityQuery _buildingQuery;
        
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

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
            base.OnCreate();
            _log.Debug("SharedDataCollectionSystem instance created. 共享資料收集系統實例創造完成。");
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
        /// Retrieve building entities' statistical data.
        /// （獲取建築實體的統計資料。）
        /// </summary>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        /// <returns>The queue with each building's statistics.（包含各建築統計資料的佇列。）</returns>
        public NativeQueue<BuildingStat> GetBuildingStats(Allocator allocator)
        {
            NativeQueue<BuildingStat> queue = new(allocator);
            CollectBuildingStatsJob job = new()
            {
                entityType = GetEntityTypeHandle(),
                citizenBufferLookup = GetBufferLookup<HouseholdCitizen>(true),
                employeeBufferType = GetBufferTypeHandle<Employee>(true),
                renterBufferType = GetBufferTypeHandle<Renter>(true),
                companyDataLookup = GetComponentLookup<CompanyData>(true),
                healthProblemLookup = GetComponentLookup<HealthProblem>(true),
                householdLookup = GetComponentLookup<Household>(true),
                prefabRefLookup = GetComponentLookup<PrefabRef>(true),
                processLookup = GetComponentLookup<IndustrialProcessData>(true),
                travelPurposeLookup = GetComponentLookup<TravelPurpose>(true),
                queue = queue.AsParallelWriter()
            };
            JobHandle handle = job.ScheduleParallel(_buildingQuery, default);
            handle.Complete();
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
            public BufferTypeHandle<Employee> employeeBufferType;

            [ReadOnly]
            public BufferTypeHandle<Renter> renterBufferType;

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

            [WriteOnly]
            public NativeQueue<BuildingStat>.ParallelWriter queue;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                NativeArray<Entity> entities = chunk.GetNativeArray(entityType);
                BufferAccessor<Employee> employeeBuffers = chunk.GetBufferAccessor(ref employeeBufferType);
                BufferAccessor<Renter> renterBuffers = chunk.GetBufferAccessor(ref renterBufferType);

                for (int i = 0; i < chunk.Count; i++)
                {
                    Entity building = entities[i];
                    BuildingStat stat = new()
                    {
                        entity = building,
                        brand = -1,
                        company = 0,
                        employee = 0,
                        household = 0,
                        product = 0ul,
                        resident = 0
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
                            }

                            if (householdLookup.HasComponent(renter))
                            {
                                stat.household++;
                            }

                            if (citizenBufferLookup.HasBuffer(renter))
                            {
                                DynamicBuffer<HouseholdCitizen> citizenBuffer = citizenBufferLookup[renter];
                                for (int k = 0; k < citizenBuffer.Length; k++)
                                {
                                    if (IsCitizenAlive(citizenBuffer[k].m_Citizen))
                                    {
                                        stat.resident++;
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
    }
}