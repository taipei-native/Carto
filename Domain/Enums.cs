using System;

namespace Carto.Domain
{
    /// <summary>
    /// The specific phase to dipose native containers.
    /// （丟棄原生容器的特定階段。）
    /// </summary>
    public enum DisposePhase
    {
        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.AreaSystem"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.AreaSystem"/> 操作後不再需要的容器。）
        /// </summary>
        AfterAreaSystem,

        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.BuildingSystem"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.BuildingSystem"/> 操作後不再需要的容器。）
        /// </summary>
        AfterBuildingSystem,

        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.SharedDataCollectionSystem.GetBuildingStats"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.SharedDataCollectionSystem.GetBuildingStats"/> 操作後不再需要的容器。）
        /// </summary>
        AfterBuildingStats,

        /// <summary>
        /// Dispose containers no longer needed after terrain related operations in <see cref="Systems.RasterSystem"/>.<br/>
        /// （丟棄在 <see cref="Systems.RasterSystem"/> 與地形相關操作執行後不再需要的容器。）
        /// </summary>
        AfterTerrainRelated,

        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.ZoningSystem"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.ZoningSystem"/> 操作後不再需要的容器。）
        /// </summary>
        AfterZoningSystem
    }
    
    /// <summary>
    /// The basic zoning types in the game.
    /// （遊戲內的基本分區類別。）
    /// </summary>
    [Flags]
    public enum ZoningCategory
    {
        /// <summary>
        /// The fallback value for unknown or unregistered feature type.（用於未知或未註冊圖徵的後備值。）
        /// </summary>
        None = 0,

        /// <summary>
        /// The zoning type that provides residences.（提供居所的分區類別。）
        /// </summary>
        Residential = 1,

        /// <summary>
        /// The zoning type that provides tertiary sector services and hires people with medium education.（提供第三級服務、雇傭受中等教育的分區類別。）
        /// </summary>
        Commercial = 2,

        /// <summary>
        /// The zoning type that provides primary and secondary services and hires people with poor education.（提供第一級與第二級服務、雇傭受初級教育的分區類別。）
        /// </summary>
        Industrial = 4,

        /// <summary>
        /// The zoning type that provides tertiary sector services and hires people with well education.（提供第三級服務、雇傭受高等教育的分區類別。）
        /// </summary>
        Office = 8
    }

    /// <summary>
    /// The development strength of the zoning.
    /// （分區的發展強度。）
    /// </summary>
    public enum ZoningDensity
    {
        /// <summary>
        /// The density is not specified.
        /// （密度未指定。）
        /// </summary>
        Generic,

        /// <summary>
        /// Low density development.
        /// （低強度開發。）
        /// </summary>
        Low,

        /// <summary>
        /// Medium density development.
        /// （中強度開發。）
        /// </summary>
        Medium,

        /// <summary>
        /// High density development.
        /// （高強度開發。）
        /// </summary>
        High
    }
}