using Carto.Domain;
using Carto.Geodata;
using Carto.IO;
using Colossal.Logging;
using Colossal.Mathematics;
using Game;
using Game.Common;
using Game.Tools;
using Game.Zones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            try
            {
                CountZonableCellsJob countJob = new()
                {
                    validCellCounts = validCellCounts.AsParallelWriter(),
                };
                JobHandle countHandle = countJob.ScheduleParallel(_zoningBlockQuery, default);
                countHandle.Complete();

                count = Utils.CommonUtils.Sum(ref validCellCounts);
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
            }
            finally
            {
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
        /// Write boundary attributes to the designated file.
        /// （寫出邊界屬性至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="validatedFields">The actually written fields.（實際寫入的欄位。）</param>
        /// <param name="cellSyncList">The list of zoning cells, which is the reference of synchronization.（分區單元的列表，作為同步的參考。）</param>
        /// <param name="fieldMap">The map between the property and the fields.（屬性與欄位的映射表。）</param>
        public void WriteBoundaryDBF(BinaryWriter writer, Options options, HashSet<Property> validatedFields, List<ZoningCell> cellSyncList, out Dictionary<Property, FieldInfo> fieldMap)
        {
            bool hasName = options.Contains(Property.Name, IO.System.Zoning) && validatedFields.Contains(Property.Name);
            bool hasColor = options.Contains(Property.Color, IO.System.Zoning) && validatedFields.Contains(Property.Color);
            bool hasDensity = options.Contains(Property.Density, IO.System.Zoning) && validatedFields.Contains(Property.Density);
            bool hasObject = options.Contains(Property.Object, IO.System.Zoning) && validatedFields.Contains(Property.Object);
            bool hasTheme = options.Contains(Property.Theme, IO.System.Zoning) && validatedFields.Contains(Property.Theme);
            bool hasZoning = options.Contains(Property.Zoning, IO.System.Zoning) && validatedFields.Contains(Property.Zoning);

            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<ZoningType> zoningTypes = ref _shared.ZoningTypes;
            ref NativeList<NativeText> zoningTypesNames = ref _shared.ZoningTypesNames;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypes);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypesNames);

            // Initialize managed containers.（初始化控管容器。）
            List<Theme> themes = _shared.Themes;
            List<ZoningType> zoningTypesManaged = Utils.CommonUtils.Copy(ref zoningTypes);
            string[] zoningTypesNamesManaged = Utils.CommonUtils.Copy(ref zoningTypesNames);
            Dictionary<Property, FieldInfo> _fieldMap = new();

            // Initiate static field info.（初始化靜態欄位資訊。）
            FieldInfo colorField = new("#ZZZZZZ");
            FieldInfo objectField = new("Zoning");

            try
            {
                // Prepare data that can only be retrieved in the main thread.（準備只能在主執行緒獲得的資料。）
                if (hasName)
                {
                    FieldInfo nameField = new(0, 0, false, FieldType.String);

                    for (int i = 0; i < zoningTypesNamesManaged.Length; i++)
                    {
                        nameField += new FieldInfo(zoningTypesNamesManaged[i]);
                    }

                    _fieldMap.Add(Property.Name, nameField);
                }

                if (hasColor)
                {
                    _fieldMap.Add(Property.Color, colorField);
                }

                if (hasObject)
                {
                    _fieldMap.Add(Property.Object, objectField);
                }

                if (hasDensity || hasTheme || hasZoning)
                {
                    FieldInfo densityField = new(0, 0, false, FieldType.String);
                    FieldInfo themeField = new(0, 0, false, FieldType.String);
                    FieldInfo zoningField = new(0, 0, false, FieldType.String);

                    for (int i = 0; i < cellSyncList.Count; i++)
                    {
                        ZoningType zoningType = zoningTypesManaged[cellSyncList[i].zoningTypeIndex];
                        densityField += new FieldInfo(zoningType.density.ToString("G"));
                        themeField += new FieldInfo(themes[zoningType.theme].name);

                        ZoningCategory category = options.Display[(Property.Zoning, IO.System.Unknown)] ? zoningType.category : Utils.CommonUtils.GetFirstMatch(zoningType.category, IO.IO.ZoningDisplayOrder);
                        zoningField += new FieldInfo(category.ToString("G"));
                    }

                    if (hasDensity) _fieldMap.Add(Property.Density, densityField);
                    if (hasTheme) _fieldMap.Add(Property.Theme, themeField);
                    if (hasZoning) _fieldMap.Add(Property.Zoning, zoningField);
                }

                // Initialize the writer thread.（初始化負責寫出的執行緒。）
                Task writerThread = Task.Run(() =>
                {
                    for (int i = 0; i < cellSyncList.Count; i++)
                    {
                        ZoningCell cell = cellSyncList[i];
                        int typeIndex = cell.zoningTypeIndex;
                        if ((typeIndex < 0) || (typeIndex >= zoningTypesManaged.Count)) continue;

                        ZoningType zoningType = zoningTypesManaged[typeIndex];
                        writer.Write((byte)32);

                        if (hasName && _fieldMap.TryGetValue(Property.Name, out FieldInfo nameField))
                        {
                            Shapefile.WriteRecord(writer, nameField, zoningTypesNamesManaged[typeIndex]);
                        }
                        if (hasColor)
                        {
                            Shapefile.WriteRecord(writer, colorField, $"#{UnityEngine.ColorUtility.ToHtmlStringRGB(zoningType.color)}");
                        }
                        if (hasDensity && _fieldMap.TryGetValue(Property.Density, out FieldInfo densityField))
                        {
                            Shapefile.WriteRecord(writer, densityField, zoningType.density.ToString("G"));
                        }
                        if (hasObject)
                        {
                            Shapefile.WriteRecord(writer, objectField, Feature.Zoning.ToString("G"));
                        }
                        if (hasTheme && _fieldMap.TryGetValue(Property.Theme, out FieldInfo themeField))
                        {
                            Shapefile.WriteRecord(writer, themeField, themes[zoningType.theme].name);
                        }
                        if (hasZoning && _fieldMap.TryGetValue(Property.Zoning, out FieldInfo zoningField))
                        {
                            ZoningCategory category = options.Display[(Property.Zoning, IO.System.Unknown)] ? zoningType.category : Utils.CommonUtils.GetFirstMatch(zoningType.category, IO.IO.ZoningDisplayOrder);
                            Shapefile.WriteRecord(writer, zoningField, category.ToString("G"));
                        }
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
                fieldMap = _fieldMap;
                Instance.Shared.Dispose(DisposePhase.AfterZoningSystem);
            }
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
            List<ZoningType> zoningTypesManaged = Utils.CommonUtils.Copy(ref zoningTypes);
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
                        if ((typeIndex < 0) || (typeIndex >= zoningTypesManaged.Count)) continue;

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
        /// Write boundary geometries to the designated file.
        /// （寫出邊界幾何至指定的檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        /// <param name="cellSyncList">The list of zoning cells, which is the reference of synchronization.（分區單元的列表，作為同步的參考。）</param>
        public void WriteBoundarySHP(BinaryWriter writer, Options options, out List<Shapefile.IndexPair> indexPairs, out Bounds3 bounds, out List<ZoningCell> cellSyncList)
        {
            // Create alias for fields.（創造欄位的別名。）
            ref NativeList<ZoningType> zoningTypes = ref _shared.ZoningTypes;
            ref NativeParallelHashMap<ushort, int> zoningTypesIdMap = ref _shared.ZoningTypesIdMap;

            // Validate native containers integrity.（驗證原生容器的完整性。）
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypes);
            Utils.CommonUtils.ValidateIntegrity(ref zoningTypesIdMap);

            // Initialize native containers.（初始化原生容器。）
            int zoningCellsMaxCount = GetZonableCellsCount();
            NativeList<ZoningCell> zoningCells = new(zoningCellsMaxCount, Allocator.Persistent);

            // Initialize out parameters.（初始化回傳參數。）
            Bounds3 _bounds = new();
            _bounds.Reset();
            List<ZoningCell> _cellSyncList = new();
            List<Shapefile.IndexPair> _indexPairs = new();

            try
            {
                CollectZoningCellsJob collectJob = new()
                {
                    useUnzoned = options.Unzoned,
                    center = options.GetTMCoord(),
                    sourceCRS = options.GetTMProjection(),
                    targetCRS = options.TargetProjection,
                    sourceProjection = options.GetTMProjectionDefinition(),
                    targetProjection = options.TargetProjectionDefinition,
                    zoningTypes = zoningTypes,
                    zoningTypesIdMap = zoningTypesIdMap,
                    zoningCells = zoningCells.AsParallelWriter()
                };
                JobHandle collectHandle = collectJob.ScheduleParallel(_zoningBlockQuery, default);
                collectHandle.Complete();

                // Copy cell data to managed list.（複製單元資料至受控管的陣列。）
                _cellSyncList = Utils.CommonUtils.Copy(ref zoningCells);

                Task writerThread = Task.Run(() =>
                {
                    int shapeId = Shapefile.GetShapeType(VectorKind.Boundary, options.Elevation);

                    if (BitConverter.IsLittleEndian)
                    {
                        for (int i = 0; i < _cellSyncList.Count; i++)
                        {
                            ZoningCell cell = _cellSyncList[i];
                            double3[] cellNodes = new double3[4] { cell.a, cell.b, cell.c, cell.d };
                            Shapefile.WriteGeometryLE(writer, i + 1, shapeId, new(new double3[1][] { cellNodes }), out Shapefile.IndexPair indexPair, out Bounds3 bounds);
                            _bounds |= bounds;
                            _indexPairs.Add(indexPair);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _cellSyncList.Count; i++)
                        {
                            ZoningCell cell = _cellSyncList[i];
                            double3[] cellNodes = new double3[4] { cell.a, cell.b, cell.c, cell.d };
                            Shapefile.WriteGeometryBE(writer, i + 1, shapeId, new(new double3[1][] { cellNodes }), out Shapefile.IndexPair indexPair, out Bounds3 bounds);
                            _bounds |= bounds;
                            _indexPairs.Add(indexPair);
                        }
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
                indexPairs = _indexPairs;
                bounds = _bounds;
                cellSyncList = _cellSyncList;
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
                            if (!zoningTypesIdMap.TryGetValue(cell.m_Zone.m_Index, out int zoningTypeIndex)) continue;
                            if ((zoningTypeIndex < 0) || (zoningTypeIndex >= zoningTypes.Length)) continue;
                            if (!useUnzoned && (zoningTypes[zoningTypeIndex].category == ZoningCategory.None)) continue;

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
    }
}