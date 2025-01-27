using System;
using System.Collections.Generic;
using System.Linq;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides the utility functions of basic C# operations.
    /// （提供常見 C# 操作功能的類別。）
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// Fomrat <see cref="TimeSpan"/> into predefined minute:second:millisecond format.<br/>
        /// 將 <see cref="TimeSpan"/> 格式化為預先定義的「分鐘:秒:毫秒」格式。
        /// </summary>
        /// <param name="timeSpan">The object representing the duration.（表示時長的物件。）</param>
        /// <returns>Formatted string.（格式化的字串。）</returns>
        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalMinutes} min {timeSpan.Seconds}.{timeSpan.Milliseconds:000} seconds";
        }

        /// <summary>
        /// Retrieve the first match between two enumeration collections.
        /// （從兩個枚舉集合中獲得首次出現的枚舉值。）
        /// </summary>
        /// <typeparam name="T">The type of the enum.（枚舉的型別。）</typeparam>
        /// <param name="input">The input flagged enumeration.（輸入的旗標枚舉。）</param>
        /// <param name="reference">The reference collection.（參考集合。）</param>
        /// <returns>The first match of two collections.（兩個集合中的首個配對。）</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T GetFirstMatch<T>(T input, T[] reference) where T : Enum
        {
            return GetFirstMatch(GetFlagComponents(input), reference);
        }

        /// <summary>
        /// Retrieve the first match between two enumeration collections.
        /// （從兩個枚舉集合中獲得首次出現的枚舉值。）
        /// </summary>
        /// <typeparam name="T">The type of the enum.（枚舉的型別。）</typeparam>
        /// <param name="input">The input collection.（輸入的集合。）</param>
        /// <param name="reference">The reference collection.（參考集合。）</param>
        /// <returns>The first match of two collections.（兩個集合中的首個配對。）</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T GetFirstMatch<T>(T[] input, T[] reference) where T : Enum
        {
            HashSet<T> inputCollection = new(input);
            for (int i = 0; i < reference.Length; i++)
            {
                if (inputCollection.Contains(reference[i])) return reference[i];
            }
            throw new InvalidOperationException("There's no match enumeration terms. 沒有相符的枚舉。");
        }

        /// <summary>
        /// Retrieve the flag components from the combined enumeration.
        /// （由混合的枚舉中獲取其組成的旗標。）
        /// </summary>
        /// <typeparam name="T">The type of the enum.（枚舉的型別。）</typeparam>
        /// <param name="input">The combined enum.（混合的枚舉。）</param>
        /// <returns>The array of flag components.（旗標部件陣列。）</returns>
        public static T[] GetFlagComponents<T>(T input) where T : Enum
        {
            IEnumerable<T> values = Enum.GetValues(typeof(T)).Cast<T>();
            if (values.Contains(input)) return new T[] { input };
            return values.Where(v => input.HasFlag(v) && !v.Equals(default(T))).ToArray();
        }

        /// <summary>
        /// Reset an array.
        /// （重置一個陣列。）
        /// </summary>
        /// <typeparam name="T">The type of the items.（陣列內物件的型別。）</typeparam>
        /// <param name="array">The input array.（輸入的陣列。）</param>
        /// <param name="capacity">The capacity used to initialize the array.（用於初始化陣列的容量。）</param>
        public static void Reset<T>(T[] array, int capacity = 16)
        {
            if (array == null)
            {
                array = new T[capacity];
            }
            else
            {
                Array.Clear(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Reset a collection.
        /// （重置一個集合。）
        /// </summary>
        /// <typeparam name="T">The type of the collection items.（集合內物件的型別。）</typeparam>
        /// <param name="collection">The input collection.（輸入的集合。）</param>
        public static void Reset<T>(ICollection<T> collection)
        {
            collection?.Clear();
            return;
        }

        /// <summary>
        /// Reset a dictionary.
        /// （重置一個字典。）
        /// </summary>
        /// <typeparam name="TKey">The type of the dictionary keys.（字典鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of the dictionary values.（字典值的型別。）</typeparam>
        /// <param name="dictionary">The input dictionary.（輸入的字典。）</param>
        public static void Reset<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            dictionary?.Clear();
            return;
        }
    }
}