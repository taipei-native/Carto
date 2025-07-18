using System;
using System.Reflection;
using Unity.Entities;

namespace Carto.Domain
{
    /// <summary>
    /// The interface for the accessor of external assemblies (mostly from other mods.)
    /// （存取外部組件（主要來自其他模組）的介面。）
    /// </summary>
    public interface IAssembly
    {
        /// <summary>
        /// Whether accessing the assembly is posible or not.
        /// （是否可以存取組件？）
        /// </summary>
        public bool Accessible { get; }

        /// <summary>
        /// The external assembly object.
        /// （外部組件物件。）
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// The name of the assembly.
        /// （組件的名稱。）
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The assembly version that its compatibility has been verified.<br/>
        /// （已驗證過無衝突的組件版本。）
        /// </summary>
        public string VerifiedVersion { get; }

        /// <summary>
        /// The version of the assembly.
        /// （組件的版本。）
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Try to dispose the assembly object.
        /// （嘗試丟棄組件物件。）
        /// </summary>
        public void Dispose();

        /// <summary>
        /// Try to retrieve the assembly.
        /// （嘗試取得組件。）
        /// </summary>
        /// <param name="verbose">Whether to log messages.（是否要記錄訊息。）</param>
        public bool TryGet(bool verbose = true);
    }

    /// <summary>
    /// The extension methods of <see cref="IAssembly"/> objects.
    /// （<see cref="IAssembly"/> 物件的擴充方法。）
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Try to retrieve the designated type.<br/>
        /// （嘗試取得指定的型別。）
        /// </summary>
        /// <param name="assembly">The object that implements <see cref="IAssembly"/>.（實作 <see cref="IAssembly"/> 的物件。）</param>
        /// <param name="fullName">The full name of the type.（型別的完整名稱。）</param>
        /// <param name="targetType">The type that represents the target.（表示目標的型別）</param>
        /// <returns>Whether the query success or not.（查詢是否成功？）</returns>
        internal static bool TryGetType(this IAssembly assembly, string fullName, out Type targetType)
        {
            targetType = null;
            if (!assembly.Accessible) return false;
            targetType = assembly.Assembly?.GetType(fullName, throwOnError: false, ignoreCase: false);
            return targetType != null;
        }
    }

    /// <summary>
    /// The interface for all struct with `Stat` prefix.
    /// （所有具 `Stat` 後綴結構的介面。）
    /// </summary>
    public interface IStat
    {
        /// <summary>
        /// The entity that represents the statistic object.
        /// （代表統計物件的實體。）
        /// </summary>
        public Entity Entity { get; set; }
    }
}