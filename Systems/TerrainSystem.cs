namespace Carto.Systems
{
    using Carto.Domain;
    using Carto.Utils;
    using Colossal.Logging;
    using Colossal.Mathematics;
    using Game;
    using Game.Simulation;
    using GeometryType = Utils.ExportUtils.GeometryType;
    using Purpose = Colossal.Serialization.Entities.Purpose;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Unity.Collections;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Rendering;

    /// <summary>
    /// The system instance that manages the terrain properties, inheriting from GameSystemBase.
    /// （管理地形屬性的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class TerrainSystem : GameSystemBase
    {
        private static readonly ILog m_Log = Instance.Log;
        private static readonly Game.Simulation.TerrainSystem m_Terrain = Instance.GameTerrain;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Log.Debug("TerrainSystem instance created. 地形系統實例創造完成。");
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
        protected override void OnUpdate() {}

        /// <summary>
        /// This event triggers when the system is destroyed.
        /// （這是當系統實例被銷毀時，會被觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// Searching for properties of existing terrian.
        /// （搜尋現有地形的屬性。）
        /// </summary>
        public static CartoObject GetTerrainProperties()
        {
            var values = new Dictionary<string, float[,]>();
            var props = new Dictionary<string, object>();
            var type = new Dictionary<string, GeometryType>();

            try
            {
                TerrainHeightData _terrain = m_Terrain.GetHeightData();
                Bounds3 terrBounds = TerrainUtils.GetBounds(ref _terrain);

                if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Terrain, "Elevation"))
                {
                    // Retrive the raster properties.
                    //（獲取網格屬性。）
                    Dictionary<string, object> terrMatrix = new Dictionary<string, object>
                    {
                        { "bounds", terrBounds },
                        { "offset", _terrain.offset },
                        { "resolution", _terrain.resolution },
                        { "scale", _terrain.scale }
                    };
                    props["ElevationMatrix"] = terrMatrix;

                    // Retrieve the height matrices.
                    // （獲取高度矩陣。）
                    var terrHeightMap = _terrain.heights.ToArray();
                    var terrElevation = new float[_terrain.resolution.x, _terrain.resolution.z];

                    Task[] tasks = new Task[_terrain.resolution.z];

                    if (Instance.Settings.TIFFFormat != ExportUtils.GeoTIFFFormat.Norm16)
                    {
                        for (int i = 0; i < _terrain.resolution.z; i++)
                        {
                            int z = i;
                            tasks[z] = Task.Run(() =>
                            {
                                for (int j = 0; j < _terrain.resolution.x; j++)
                                {
                                    terrElevation[z, j] = (float)Math.Round(terrHeightMap[j + (_terrain.resolution.z - 1 - z) * _terrain.resolution.x] * (terrBounds.y.max - terrBounds.y.min) / 65535, 4);
                                }
                            });
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _terrain.resolution.z; i++)
                        {
                            int z = i;
                            tasks[z] = Task.Run(() =>
                            {
                                for (int j = 0; j < _terrain.resolution.x; j++)
                                {
                                    terrElevation[z, j] = terrHeightMap[j + (_terrain.resolution.z - 1 - z) * _terrain.resolution.x];
                                }
                            });
                        }
                    }

                    Task.WaitAll(tasks);
                    values["Elevation"] = terrElevation;
                    type["Elevation"] = GeometryType.Raster;
                }

                if (ExportUtils.GetFieldStatus(ExportUtils.FeatureType.Terrain, "WorldElevation") && (m_Terrain.worldHeightmap != null))
                {
                    float2 worldOffset = m_Terrain.worldOffset;
                    float2 worldSize = m_Terrain.worldSize;
                    Texture worldMap = m_Terrain.worldHeightmap;

                    // Retrive the raster properties.
                    //（獲取網格屬性。）
                    Dictionary<string, object> worldMatrix = new Dictionary<string, object>
                    {
                        { "offset", new float3(math.abs(worldOffset.x), 0, math.abs(worldOffset.y)) },
                        { "resolution", new int3(worldMap.width, 65536, worldMap.height) },
                        { "scale", new float3(worldMap.width / worldSize.x, 16, worldMap.height / worldSize.y) }
                    };
                    props["WorldElevationMatrix"] = worldMatrix;

                    // Retrieve the height matrices.
                    // （獲取高度矩陣。）
                    NativeArray<ushort> worldHeightmap = new NativeArray<ushort>(worldMap.width * worldMap.height, Allocator.Persistent);
                    AsyncGPUReadback.RequestIntoNativeArray(ref worldHeightmap, worldMap).WaitForCompletion();
                    float[,] worldElevation = new float[worldMap.width, worldMap.height];

                    Task[] tasks = new Task[worldMap.height];

                    if (Instance.Settings.TIFFFormat != ExportUtils.GeoTIFFFormat.Norm16)
                    {
                        for (int i = 0; i < worldMap.height; i++)
                        {
                            int z = i;
                            tasks[z] = Task.Run(() =>
                            {
                                for (int j = 0; j < worldMap.width; j++)
                                {
                                    worldElevation[z, j] = (float)Math.Round((double)(worldHeightmap[j + (worldMap.height - 1 - z) * worldMap.width] * (terrBounds.y.max - terrBounds.y.min) / 65535), 4);
                                }
                            });
                        }
                    }
                    else
                    {
                        for (int i = 0; i < worldMap.height; i++)
                        {
                            int z = i;
                            tasks[z] = Task.Run(() =>
                            {
                                for (int j = 0; j < worldMap.width; j++)
                                {
                                    worldElevation[z, j] = worldHeightmap[j + (worldMap.height - 1 - z) * worldMap.width];
                                }
                            });
                        }
                    }

                    Task.WaitAll(tasks);
                    worldHeightmap.Dispose();
                    values["WorldElevation"] = worldElevation;
                    type["WorldElevation"] = GeometryType.Raster;
                }

            }
            catch (Exception ex)
            {
                m_Log.Error($"An error occured at GetTerrainProperties(); 於 GetTerrainProperties() 發生一個錯誤； {ex}");
            }

            return new CartoObject(values, props, type);
        }
    }
}