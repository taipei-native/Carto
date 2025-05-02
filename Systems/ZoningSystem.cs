using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Colossal.Logging;
using Game;
using Game.Common;
using Game.Tools;
using Game.Zones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Carto.Systems
{
    /// <summary>
    /// The system that searches zoning blocks.
    /// （搜尋分區的系統。）
    /// </summary>
    public partial class ZoningSystem : GameSystemBase
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The system collecting shared data.（收集共享資料的系統。）<br/>
        /// See <see cref="Instance.Shared"/> for more information.
        /// </summary>
        static readonly SharedDataCollectionSystem _shared = Instance.Shared;

        /// <summary>
        /// The query for existing zoning blocks.（現有分區的查詢。）
        /// </summary>
        static EntityQuery _zoningBlockQuery;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _zoningBlockQuery = GetEntityQuery(new EntityQueryDesc{
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Block>()
                },
                None = new ComponentType[]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            
            base.OnCreate();
            _log.Debug("ZoningSystem instance created. 分區系統實例創造完成。");
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
        /// Retrieve the number of zonable zoning cells.
        /// （獲得可劃設分區的單元數量。）
        /// </summary>
        /// <returns>The number of cells.（單元的數量。）</returns>
        private int GetZonableCellsCount()
        {
            int count = 0;

            // Initialize native containers.（初始化原生容器。）
            NativeQueue<int> validCellCounts = new(Allocator.Persistent);
            NativeReference<int> cellCount = new(0, Allocator.Persistent);

            try
            {
                CountZonableCellsJob countJob = new() {
                    validCellCounts = validCellCounts.AsParallelWriter(),
                };
                JobHandle countHandle = countJob.ScheduleParallel(_zoningBlockQuery, default);
                countHandle.Complete();

                SumQueueContentsJob sumJob = new() {
                    validCellCounts = validCellCounts,
                    cellCount = cellCount
                };
                JobHandle sumHandle = sumJob.Schedule();
                sumHandle.Complete();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
                count = cellCount.Value;
                Utils.CommonUtils.Dispose(ref cellCount);
                Utils.CommonUtils.Dispose(ref validCellCounts);
            }

            return count;
        }

        /// <summary>
        /// Check whether a zoning cell is zonable.
        /// （確認分區單元是否可用於分區。）
        /// </summary>
        /// <param name="cell">The input cell component.（輸入的單元組件。）</param>
        /// <returns>True if the cell is zonable.（當分區單元可使用時，回傳真值。）</returns>
        private static bool IsZonableCell(Cell cell)
        {
            CellFlags cellStatus = cell.m_State;
            return ((cellStatus & CellFlags.Blocked) == 0) && ((cellStatus & CellFlags.Shared) == 0);
        }

        /// <summary>
        /// Write boundary features (geometries and properties) to the designated file.
        /// （寫出邊界圖徵（幾何與屬性）至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public void WriteBoundaryFeatures(JsonTextWriter writer, Options options, Action<string, int> onReportMethod)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Zoning);
            bool hasColor = options.Contains(Property.Color, IO.System.Zoning);
            bool hasDensity = options.Contains(Property.Density, IO.System.Zoning);
            bool hasObject = options.Contains(Property.Object, IO.System.Zoning);
            bool hasTheme = options.Contains(Property.Theme, IO.System.Zoning);
            bool hasZoning = options.Contains(Property.Zoning, IO.System.Zoning);

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<ZoningType> zoningTypes = ref _shared.ZoningTypes;
            ref NativeParallelHashMap<ushort, int> zoningTypesIdMap = ref _shared.ZoningTypesIdMap;
            ref NativeList<NativeText> zoningTypesNames = ref _shared.ZoningTypesNames;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypes);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypesIdMap);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypesNames);

            // Initialize native containers.（初始化原生容器。）
            int zoningCellsMaxCount = GetZonableCellsCount();
            NativeList<ZoningCell> zoningCells = new(zoningCellsMaxCount, Allocator.Persistent);

            // Initialize managed containers.（初始化控管容器。）
            List<Theme> themes = _shared.Themes;
            ZoningType[] zoningTypesManaged = Utils.CommonUtils.Copy(ref zoningTypes);
            string[] zoningTypesNamesManaged = Utils.CommonUtils.Copy(ref zoningTypesNames);

            try
            {
                CollectZoningCellsJob collectJob = new()
                {
                    useUnzoned = options.Unzoned,
                    center = options.GetTMCoord(),
                    sourceCRS = options.GetTMProjection(),
                    targetCRS = Geodata.CRS.WGS84,
                    sourceProjection = options.GetTMProjectionDefinition(),
                    targetProjection = default,
                    zoningTypes = zoningTypes,
                    zoningTypesIdMap = zoningTypesIdMap,
                    zoningCells = zoningCells.AsParallelWriter(),
                };
                JobHandle collectHandle = collectJob.ScheduleParallel(_zoningBlockQuery, default);
                collectHandle.Complete();

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    for (int i = 0; i < zoningCells.Length; i++)
                    {
                        ZoningCell cell = zoningCells[i];
                        int typeIndex = cell.zoningTypeIndex;
                        if ((typeIndex < 0) || (typeIndex >= zoningTypesManaged.Length)) continue;

                        // Write feature header.（寫出圖徵檔頭。）
                        writer.WriteStartObject();
                        GeoJson.WritePropertyPair(writer, "type", "Feature");

                        // Write feature geometry.（寫出圖徵幾何圖形。）
                        writer.WritePropertyName("geometry");
                        double3[] cellNodes = new double3[4] { cell.a, cell.b, cell.c, cell.d };
                        GeoJson.WriteGeometry(writer, new Geometry(new double3[1][] { cellNodes }), Shape.Polygon, options.Elevation);

                        // Write feature properties.（寫出圖徵）
                        writer.WritePropertyName("properties");
                        writer.WriteStartObject();

                        ZoningType zoningType = zoningTypesManaged[typeIndex];

                        if (hasName)
                        {
                            GeoJson.WriteProperty(writer, Property.Name, zoningTypesNamesManaged[typeIndex]);
                        }
                        if (hasColor)
                        {
                            GeoJson.WriteProperty(writer, Property.Color, $"#{UnityEngine.ColorUtility.ToHtmlStringRGB(zoningType.color)}");
                        }
                        if (hasDensity)
                        {
                            GeoJson.WriteProperty(writer, Property.Density, zoningType.density.ToString("G"));
                        }
                        if (hasObject)
                        {
                            GeoJson.WriteProperty(writer, Property.Object, Feature.Zoning.ToString("G"));
                        }
                        if (hasTheme)
                        {
                            GeoJson.WriteProperty(writer, Property.Theme, themes[zoningType.theme].name);
                        }
                        if (hasZoning)
                        {
                            ZoningCategory category = options.Display[(Property.Zoning, IO.System.Unknown)] ? zoningType.category : Utils.CommonUtils.GetFirstMatch(zoningType.category, IO.IO.ZoningDisplayOrder);
                            GeoJson.WriteProperty(writer, Property.Zoning, category.ToString("G"));
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
                Utils.CommonUtils.Dispose(ref zoningCells);
                Instance.Shared.Dispose(DisposePhase.AfterZoningSystem);
            }
        }

        /// <summary>
        /// The job to collect all zoning cells.
        /// （收集分區單元的工作。）
        /// </summary>
        [BurstCompile]
        public partial struct CollectZoningCellsJob : IJobEntity
        {
            [ReadOnly]
            public bool useUnzoned;

            [ReadOnly]
            public Coord center;

            [ReadOnly]
            public Geodata.CRS sourceCRS;

            [ReadOnly]
            public Geodata.CRS targetCRS;

            [ReadOnly]
            public ProjectionDefinition sourceProjection;

            [ReadOnly]
            public ProjectionDefinition targetProjection;

            [ReadOnly]
            public NativeList<ZoningType> zoningTypes;

            [ReadOnly]
            public NativeParallelHashMap<ushort, int> zoningTypesIdMap;
            
            [WriteOnly]
            public NativeList<ZoningCell>.ParallelWriter zoningCells;

            public void Execute(in Block blockComponent, in DynamicBuffer<Cell> cells, Entity zoningBlock)
            {
                float xDirection = blockComponent.m_Direction.x;
                float yDirection = blockComponent.m_Direction.y;
                float directionLength = math.sqrt(xDirection * xDirection + yDirection * yDirection);
                float3 xUnitVector = new float3(-yDirection / directionLength, 0, xDirection / directionLength) * 8;
                float3 yUnitVector = new float3( xDirection / directionLength, 0, yDirection / directionLength) * 8;
                int xSize = blockComponent.m_Size.x;
                int ySize = blockComponent.m_Size.y;
                float3 cornerPoint = blockComponent.m_Position - xSize * xUnitVector / 2 - ySize * yUnitVector / 2;

                if (cells.Length != xSize * ySize) return;

                for (int i = 0; i < ySize; i++)
                {
                    for (int j = 0; j < xSize; j++)
                    {
                        int cellIndex = j + xSize * (ySize - i - 1);
                        Cell cell = cells[cellIndex];

                        // Omit the cells with the following flags:（忽略擁有下列旗標的單元：）
                        // - Blocked, which indicates that the cell is inzonable due to overlapped entities (e.g., networks, ploppable buildings)
                        //            （分區單元因重疊的實體（如網路、可放置建築）而無法劃設分區。）
                        // - Shared, which indicates that the cell overlaps with one or more cells at the same location.
                        //            （分區單元與其他位於相同位置的單元重疊。）
                        if (IsZonableCell(cell))
                        {
                            if (!zoningTypesIdMap.TryGetValue(cell.m_Zone.m_Index, out int zoningTypeIndex)) return;
                            if ((zoningTypeIndex < 0) || (zoningTypeIndex >= zoningTypes.Length)) return;
                            if (!useUnzoned && (zoningTypes[zoningTypeIndex].category == ZoningCategory.None)) return;

                            float3 point1 = cornerPoint + j * xUnitVector + i * yUnitVector;
                            float3 point2 = cornerPoint + j * xUnitVector + (i + 1) * yUnitVector;
                            float3 point3 = cornerPoint + (j + 1) * xUnitVector + (i + 1) * yUnitVector;
                            float3 point4 = cornerPoint + (j + 1) * xUnitVector + i * yUnitVector;

                            zoningCells.AddNoResize(new ZoningCell
                            {
                                a = Transform.Apply(center.Shift(point1.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                                b = Transform.Apply(center.Shift(point2.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                                c = Transform.Apply(center.Shift(point3.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                                cellIndex = cellIndex,
                                d = Transform.Apply(center.Shift(point4.xzy), sourceCRS, targetCRS, sourceProjection, targetProjection).Round().ToDouble3(),
                                entity = zoningBlock,
                                zoningTypeIndex = zoningTypeIndex,
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The job to count the number of zonable cells in each zoning block.
        /// （計算每個分區塊中可劃設分區單元的數量。）
        /// </summary>
        [BurstCompile]
        public partial struct CountZonableCellsJob : IJobEntity
        {   
            [WriteOnly]
            public NativeQueue<int>.ParallelWriter validCellCounts;

            public void Execute(in Block blockComponent, in DynamicBuffer<Cell> cells)
            {
                int count = 0;
                int xSize = blockComponent.m_Size.x;
                int ySize = blockComponent.m_Size.y;
                if (cells.Length != xSize * ySize) return;

                for (int i = 0; i < ySize; i++)
                {
                    for (int j = 0; j < xSize; j++)
                    {
                        if (IsZonableCell(cells[j + xSize * (ySize - i - 1)])) count++;
                    }
                }

                validCellCounts.Enqueue(count);
            }
        }

        /// <summary>
        /// The job to sum up the count calculated in <see cref="CountZonableCellsJob"/>.
        /// （將 <see cref="CountZonableCellsJob"/> 計算的數量相加的工作。 ） 
        /// </summary>
        [BurstCompile]
        public partial struct SumQueueContentsJob : IJob
        {
            [ReadOnly]
            public NativeQueue<int> validCellCounts;

            [WriteOnly]
            public NativeReference<int> cellCount;

            public void Execute()
            {
                int count = 0;
                while (validCellCounts.TryDequeue(out int individualCount))
                {
                    count += individualCount;
                }
                cellCount.Value = count;
            }
        }
    }
}