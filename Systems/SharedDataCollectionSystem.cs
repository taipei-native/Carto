using Colossal.Logging;
using Game;
using Game.Buildings;
using Game.Companies;
using Game.Objects;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

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
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
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

        public struct CollectBuildingStatsJob : IJobChunk
        {
            [ReadOnly]
            public ComponentTypeHandle<Building> buildingType;

            [ReadOnly]
            public EntityTypeHandle entityType;

            [ReadOnly]
            public ComponentTypeHandle<ResidentialProperty> residentialPropertyType;

            [ReadOnly]
            public ComponentLookup<CompanyData> companyDataLookup;

            [ReadOnly]
            public BufferLookup<Employee> employeeLookup;

            [ReadOnly]
            public ComponentLookup<Placeholder> placeholderLookup;

            [ReadOnly]
            public BufferLookup<Renter> renterLookup;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {

            }
        }
    }
}