using Carto.Domain;
using Colossal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides the utility functions of basic C# operations.
    /// （提供常見 C# 操作功能的類別。）
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// Add the elements from a <see cref="NativeList{T}"/> to a managed list.
        /// （將一個 <see cref="NativeList{T}"/> 的元素加入至列表中。）
        /// </summary>
        /// <typeparam name="T">The type of list's items.（列表內物件的型別。）</typeparam>
        /// <param name="destination">The managed list.（控管列表。）</param>
        /// <param name="source">The unmanaged list.（未控管列表。）</param>
        public static void AddTo<T>(List<T> destination, ref NativeList<T> source) where T : unmanaged
        {
            if (!source.IsCreated || source.Length == 0) return;
            destination.Capacity += source.Length;
            for (int i = 0; i < source.Length; i++)
            {
                destination.Add(source[i]);
            }
        }

        /// <summary>
        /// Add the elements from a <see cref="NativeList{T}"/> to another native list.
        /// （將一個 <see cref="NativeList{T}"/> 的元素加入至另一個原生列表中。）
        /// </summary>
        /// <typeparam name="T">The type of list's items.（列表內物件的型別。）</typeparam>
        /// <param name="destination">The target list.（目標列表。）</param>
        /// <param name="source">The source native list.（來源原生列表。）</param>
        public static void AddTo<T>(ref NativeList<T> destination, ref NativeList<T> source) where T : unmanaged
        {
            if (!destination.IsCreated || !source.IsCreated ||
                (source.Length == 0) || ((destination.Capacity - destination.Length) < source.Length)) return;

            AddToJob<T> addJob = new()
            {
                source = source,
                destination = destination.AsParallelWriter(),
            };
            JobHandle addHandle = addJob.Schedule(source.Length, 64, default);
            addHandle.Complete();
        }

        /// <summary>
        /// Copy a <see cref="NativeParallelHashSet{T}"/> to a managed array.
        /// （將一個 <see cref="NativeParallelHashSet{T}"/> 複製為受控管陣列。）
        /// </summary>
        /// <typeparam name="T">The type of array's enums.（陣列內枚舉的型別。）</typeparam>
        /// <param name="hashSet">The input hash set.（輸入的集合。）</param>
        /// <returns>The copied array.（複製的陣列。）</returns>
        public static T[] Copy<T>(ref NativeParallelHashSet<EnumWrapper<T>> hashSet) where T : unmanaged, Enum
        {
            int index = 0;
            int size = hashSet.Count();
            T[] enumArray = new T[size];
            foreach (EnumWrapper<T> enumWrapper in hashSet)
            {
                enumArray[index] = enumWrapper.value;
                index++;
            }
            Array.Sort(enumArray);
            return enumArray;
        }

        /// <summary>
        /// Copy a <see cref="NativeList{T}"/> to a managed list.
        /// （將一個 <see cref="NativeList{T}"/> 複製為列表。）
        /// </summary>
        /// <typeparam name="T">The type of list's items.（列表內物件的型別。）</typeparam>
        /// <param name="list">The input list.（輸入的列表。）</param>
        /// <returns>The copied list.（複製的列表。）</returns>
        public static List<T> Copy<T>(ref NativeList<T> list) where T : unmanaged
        {
            List<T> mList = new();
            AddTo(mList, ref list);
            return mList;
        }

        /// <summary>
        /// Copy a <see cref="NativeList{T}"/> of <see cref="NativeText"/>s to an array.
        /// （將一個由 <see cref="NativeText"/> 組成的 <see cref="NativeList{T}"/> 複製為陣列。）
        /// </summary>
        /// <param name="list">The input list.（輸入的列表。）</param>
        /// <returns>The copied array.（複製的陣列。）</returns>
        public static string[] Copy(ref NativeList<NativeText> list)
        {
            string[] array = new string[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                array[i] = list[i].ToString();
            }
            return array;
        }
        
        /// <summary>
        /// Try disposing of an object that implements <see cref="IDisposable"/>.
        /// （嘗試丟棄一個實作 <see cref="IDisposable"/> 介面的物件。）
        /// </summary>
        /// <typeparam name="T">The type of the item.（物件的型別。）</typeparam>
        /// <param name="item">The input item.（輸入的物件。）</param>
        public static void Dispose<T>(ref T item) where T : struct, IDisposable
        {
            item.Dispose();
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeArray{T}"/>.
        /// （嘗試丟棄一個 <see cref="NativeArray{T}"/>。）
        /// </summary>
        /// <typeparam name="T">The type of array's items.（陣列內物件的型別。）</typeparam>
        /// <param name="array">The input array.（輸入的陣列。）</param>
        /// <param name="disposeContentsOnly">Only dispose array contents.（僅丟棄陣列內容物。）</param>
        public static void Dispose<T>(ref NativeArray<T> array, bool disposeContentsOnly = false) where T : struct
        {
            if (array == null) return;
            if (array.IsCreated)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    unsafe
                    {
                        DisposeHelper(ref UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafePtr(), i));
                    }
                }
                if (!disposeContentsOnly)
                {
                    array.Dispose();
                }
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeCounter"/>.
        /// （嘗試丟棄一個 <see cref="NativeCounter"/>。）
        /// </summary>
        /// <param name="counter">The input counter.（輸入的計數器。）</param>
        public static void Dispose(ref NativeCounter counter)
        {
            if (counter.IsCreated)
            {
                counter.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeHashMap{TKey, TValue}" />.
        /// （嘗試丟棄一個 <see cref="NativeHashMap{TKey, TValue}" />。）
        /// </summary>
        /// <typeparam name="TKey">The type of hashmap's keys.（映射表鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of hashmap's values.（映射表值的型別。）</typeparam>
        /// <param name="hashmap">The input hashmap.（輸入的映射表。）</param>
        public static void Dispose<TKey, TValue>(ref NativeHashMap<TKey, TValue> hashmap)
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            if (hashmap.IsCreated)
            {
                NativeArray<TValue> values = hashmap.GetValueArray(Allocator.Temp);
                Dispose(ref values, true);
                hashmap.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeHashSet{T}" />.
        /// （嘗試丟棄一個 <see cref="NativeHashSet{T}" />。）
        /// </summary>
        /// <typeparam name="T">The type of hashset's items.（集合內物件的型別。）</typeparam>
        /// <param name="hashset">The input hashset.（輸入的集合。）</param>
        public static void Dispose<T>(ref NativeHashSet<T> hashset) where T : unmanaged, IEquatable<T>
        {
            if (hashset.IsCreated)
            {
                NativeArray<T> items = hashset.ToNativeArray(Allocator.Temp);
                Dispose(ref items, true);
                hashset.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeList{T}"/>.
        /// （嘗試丟棄一個 <see cref="NativeList{T}"/>。）
        /// </summary>
        /// <typeparam name="T">The type of list's items.（列表內物件的型別。）</typeparam>
        /// <param name="list">The input list.（輸入的列表。）</param>
        public static void Dispose<T>(ref NativeList<T> list) where T : unmanaged
        {
            if (list.IsCreated)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    DisposeHelper(ref list.ElementAt(i));
                }
                list.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeParallelHashMap{TKey, TValue}" />.
        /// （嘗試丟棄一個 <see cref="NativeParallelHashMap{TKey, TValue}" />。）
        /// </summary>
        /// <typeparam name="TKey">The type of hashmap's keys.（映射表鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of hashmap's values.（映射表值的型別。）</typeparam>
        /// <param name="hashmap">The input hashmap.（輸入的映射表。）</param>
        public static void Dispose<TKey, TValue>(ref NativeParallelHashMap<TKey, TValue> hashmap)
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            if (hashmap.IsCreated)
            {
                NativeArray<TValue> values = hashmap.GetValueArray(Allocator.Temp);
                Dispose(ref values, true);
                hashmap.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeParallelHashSet{T}" />.
        /// （嘗試丟棄一個 <see cref="NativeParallelHashSet{T}" />。）
        /// </summary>
        /// <typeparam name="T">The type of hashset's items.（集合內物件的型別。）</typeparam>
        /// <param name="hashset">The input hashset.（輸入的集合。）</param>
        public static void Dispose<T>(ref NativeParallelHashSet<T> hashset) where T : unmanaged, IEquatable<T>
        {
            if (hashset.IsCreated)
            {
                NativeArray<T> items = hashset.ToNativeArray(Allocator.Temp);
                Dispose(ref items, true);
                hashset.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeParallelMultiHashMap{TKey, TValue}"/>.
        /// （嘗試丟棄一個 <see cref="NativeParallelMultiHashMap{TKey, TValue}"/>。）
        /// </summary>
        /// <typeparam name="TKey">The type of hashmap's keys.（映射表鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of hashmap's values.（映射表值的型別。）</typeparam>
        /// <param name="hashmap">The input hashmap.（輸入的映射表。）</param>
        public static void Dispose<TKey, TValue>(ref NativeParallelMultiHashMap<TKey, TValue> hashmap)
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            if (hashmap.IsCreated)
            {
                NativeArray<TValue> values = hashmap.GetValueArray(Allocator.Temp);
                Dispose(ref values, true);
                hashmap.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeQueue{T}"/>.
        /// （嘗試丟棄一個 <see cref="NativeQueue{T}"/>。）
        /// </summary>
        /// <typeparam name="T">The type of queue's items.（佇列內物件的型別。）</typeparam>
        /// <param name="queue">The input queue.（輸入的佇列。）</param>
        public static void Dispose<T>(ref NativeQueue<T> queue) where T : unmanaged
        {
            if (queue.IsCreated)
            {
                while (queue.Count > 0)
                {
                    T item = queue.Dequeue();
                    DisposeHelper(ref item);
                }

                queue.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeReference{T}"/>.
        /// （嘗試丟棄一個 <see cref="NativeReference{T}"/>。）
        /// </summary>
        /// <typeparam name="T">The type of reference's value.（參考值的型別。。）</typeparam>
        /// <param name="reference">The input reference.（輸入的參考。）</param>
        public static void Dispose<T>(ref NativeReference<T> reference) where T : unmanaged
        {
            if (reference.IsCreated)
            {
                reference.Dispose();
            }
        }

        /// <summary>
        /// Try disposing of a <see cref="NativeText" />.
        /// （嘗試丟棄一個 <see cref="NativeText" />。）
        /// </summary>
        /// <param name="text">The input text.（輸入的文字。）</param>
        public static void Dispose(ref NativeText text)
        {
            if (text.IsCreated)
            {
                text.Dispose();
            }
        }

        /// <summary>
        /// The helper function to try disposing of an object.
        /// （用於嘗試丟棄一個物件的輔助函數。）
        /// </summary>
        /// <typeparam name="T">The type of the item.（物件的型別。）</typeparam>
        /// <param name="item">The item waiting to be examined.（等待被檢驗的物件。）</param>
        private static void DisposeHelper<T>(ref T item) where T : struct
        {
            if (item is IDisposable disposable)
            {                
                disposable.Dispose();
            }
        }
        
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
        /// Retrieve the named flags' names from the enum type.
        /// （由枚舉型別獲得命名旗標的名稱。）
        /// </summary>
        /// <returns>A dictionary between named flags and their names.（包含命名旗標與其名稱的字典。）</returns>
        public static Dictionary<T, string> GetNamedFlags<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(value => value, value => value.ToString());
        }

        /// <summary>
        /// Checks whether the target enum is a subset of another enum.
        /// （確認目標枚舉為另一個枚舉的子集。）
        /// </summary>
        /// <typeparam name="T">The type of the enum.（枚舉的型別。）</typeparam>
        /// <param name="target">The input enum value.（輸入的枚舉值。）</param>
        /// <param name="rule">The allowed enum flags.（允許的枚舉值。）</param>
        /// <returns>If true, the target enum is a subset of <paramref name="rule"/>.（若為真，目標枚舉為 <paramref name="rule"/> 的子集。）</returns>
        public static bool IsSubSetOf<T>(T target, T rule) where T : struct, Enum
        {
            int targetValue = UnsafeUtility.EnumToInt(target);
            return (targetValue & UnsafeUtility.EnumToInt(rule)) == targetValue;
        }

        /// <summary>
        /// Replace the tokens into pre-defined texts.
        /// （將代號轉換為預先定義的文字。）
        /// </summary>
        /// <param name="text">The input string.（輸入的字串。）</param>
        /// <param name="regex">The matching pattern.（比對的模式。）</param>
        /// <param name="tokens">The dictionary containing the replacements.（包含替代物的字典。）</param>
        /// <returns>Replaced string.（已被替換的字串。）</returns>
        public static string ReplaceTokens(string text, string regex, Dictionary<string, string> tokens)
        {
            return Regex.Replace(text, regex, matched =>
            {
                string key = matched.Groups[1].Value;
                return tokens.TryGetValue(key, out string val) ? val : matched.Value;
            });
        }

        /// <summary>
        /// Reset an array.
        /// （重置一個陣列。）
        /// </summary>
        /// <typeparam name="T">The type of the items.（陣列內物件的型別。）</typeparam>
        /// <param name="array">The input array.（輸入的陣列。）</param>
        /// <param name="capacity">The capacity used to initialize the array.（用於初始化陣列的容量。）</param>
        public static void Reset<T>(ref T[] array, int capacity = 16)
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
        /// <typeparam name="TCollection">The type of the collection.（集合的型別。）</typeparam>
        /// <typeparam name="TItem">The type of the collection items.（集合內物件的型別。）</typeparam>
        /// <param name="collection">The input collection.（輸入的集合。）</param>
        public static void Reset<TCollection, TItem>(ref TCollection collection) where TCollection : ICollection<TItem>, new()
        {
            collection ??= new();
            collection.Clear();
            return;
        }

        /// <summary>
        /// Reset a dictionary.
        /// （重置一個字典。）
        /// </summary>
        /// <typeparam name="TDictionary">The type of the dictionary.（字典的型別。）</typeparam>
        /// <typeparam name="TKey">The type of the dictionary keys.（字典鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of the dictionary values.（字典值的型別。）</typeparam>
        /// <param name="dictionary">The input dictionary.（輸入的字典。）</param>
        public static void Reset<TDictionary, TKey, TValue>(ref TDictionary dictionary) where TDictionary : IDictionary<TKey, TValue>, new()
        {
            dictionary ??= new();
            dictionary.Clear();
            return;
        }

        /// <summary>
        /// Reset a native array.
        /// （重置一個原生陣列。）
        /// </summary>
        /// <typeparam name="T">The type of the items.（陣列內物件的型別。）</typeparam>
        /// <param name="array">The input array.（輸入的陣列。）</param>
        /// <param name="capacity">The capacity used to initialize the array.（用於初始化陣列的容量。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset<T>(ref NativeArray<T> array, int capacity = 16, Allocator allocator = Allocator.Persistent) where T : struct
        {
            Dispose(ref array);
            array = new(capacity, allocator);
        }

        /// <summary>
        /// Reset a native counter.
        /// （重置一個原生計數器。）
        /// </summary>
        /// <param name="counter">The input counter.（輸入的計數器。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset(ref NativeCounter counter, Allocator allocator = Allocator.Persistent)
        {
            Dispose(ref counter);
            counter = new(allocator);
        }

        /// <summary>
        /// Reset a native hashmap.
        /// （重置一個原生映射表。）
        /// </summary>
        /// <typeparam name="TKey">The type of the hashmap's keys.（映射表鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of the hashmap's values.（映射表值的型別。）</typeparam>
        /// <param name="hashmap">The input hashmap.（輸入的映射表。）</param>
        /// <param name="capacity">The capacity used to initialize the hashmap.（用於初始化映射表的容量。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset<TKey, TValue>(ref NativeHashMap<TKey, TValue> hashmap, int capacity = 16, Allocator allocator = Allocator.Persistent)
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            Dispose(ref hashmap);
            hashmap = new(capacity, allocator);
        }

        /// <summary>
        /// Reset a native hashset.
        /// （重置一個原生集合。）
        /// </summary>
        /// <typeparam name="T">The type of the hashset's items.（集合內物件的型別。）</typeparam>
        /// <param name="hashset">The input hashset.（輸入的集合。）</param>
        /// <param name="capacity">The capacity used to initialize the hashset.（用於初始化集合的容量。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset<T>(ref NativeHashSet<T> hashset, int capacity = 16, Allocator allocator = Allocator.Persistent) where T : unmanaged, IEquatable<T>
        {
            Dispose(ref hashset);
            hashset = new(capacity, allocator);
        }

        /// <summary>
        /// Reset a native list.
        /// （重置一個原生列表。）
        /// </summary>
        /// <typeparam name="T">The type of the list's items.（列表內物件的型別。）</typeparam>
        /// <param name="list">The input list.（輸入的列表。）</param>
        /// <param name="capacity">The capacity used to initialize the list.（用於初始化列表的容量。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset<T>(ref NativeList<T> list, int capacity = 16, Allocator allocator = Allocator.Persistent) where T : unmanaged
        {
            Dispose(ref list);
            list = new(capacity, allocator);
        }

        /// <summary>
        /// Reset a native hashmap (parallel variant).
        /// （重置一個原生映射表（平行運算變種）。）
        /// </summary>
        /// <typeparam name="TKey">The type of the hashmap's keys.（映射表鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of the hashmap's values.（映射表值的型別。）</typeparam>
        /// <param name="hashmap">The input hashmap.（輸入的映射表。）</param>
        /// <param name="capacity">The capacity used to initialize the hashmap.（用於初始化映射表的容量。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset<TKey, TValue>(ref NativeParallelHashMap<TKey, TValue> hashmap, int capacity = 16, Allocator allocator = Allocator.Persistent)
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            Dispose(ref hashmap);
            hashmap = new(capacity, allocator);
        }

        /// <summary>
        /// Reset a native hashset (parallel variant).
        /// （重置一個原生集合（平行運算變種）。）
        /// </summary>
        /// <typeparam name="T">The type of the hashset's items.（集合內物件的型別。）</typeparam>
        /// <param name="hashset">The input hashset.（輸入的集合。）</param>
        /// <param name="capacity">The capacity used to initialize the hashset.（用於初始化集合的容量。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset<T>(ref NativeParallelHashSet<T> hashset, int capacity = 16, Allocator allocator = Allocator.Persistent) where T : unmanaged, IEquatable<T>
        {
            Dispose(ref hashset);
            hashset = new(capacity, allocator);
        }

        /// <summary>
        /// Reset a native hashmap (parallel, multi variant).
        /// （重置一個原生映射表（平行運算、重複變種）。）
        /// </summary>
        /// <typeparam name="TKey">The type of the hashmap's keys.（映射表鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of the hashmap's values.（映射表值的型別。）</typeparam>
        /// <param name="hashmap">The input hashmap.（輸入的映射表。）</param>
        /// <param name="capacity">The capacity used to initialize the hashmap.（用於初始化映射表的容量。）</param>
        /// <param name="allocator">The memory allocator.（記憶體分配器。）</param>
        public static void Reset<TKey, TValue>(ref NativeParallelMultiHashMap<TKey, TValue> hashmap, int capacity = 16, Allocator allocator = Allocator.Persistent)
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            Dispose(ref hashmap);
            hashmap = new(capacity, allocator);
        }

        /// <summary>
        /// Try to retrieve the last item in the collection.
        /// （嘗試獲得集合中的最後一個物件。）
        /// </summary>
        /// <typeparam name="T">The type of the collection's items.（集合內物件的型別。）</typeparam>
        /// <param name="collection">The input collection.（輸入的集合。）</param>
        /// <param name="lastItem">The last item in the collection.（集合內的最後一個物件。）</param>
        /// <returns>Whether the last item is retrievable.（最後一個物件是否可被取得。）</returns>
        public static bool TryGetLast<T>(IList<T> collection, out T lastItem)
        {
            lastItem = default;
            if ((collection == null) || collection.Count == 0) return false;
            lastItem = collection[collection.Count - 1];
            return true;
        }

        /// <summary>
        /// Sum up the numbers in the queue.
        /// （將佇列內的數字加總。）
        /// </summary>
        /// <param name="queue">The input queue.（輸入的佇列。）</param>
        /// <returns>The sum of the numbers in the queue.（佇列內的數字總和。）</returns>
        public static int Sum(ref NativeQueue<int> queue)
        {
            int count = 0;
            if (!queue.IsCreated) return count;
            NativeReference<int> counter = new(Allocator.TempJob);
            SumQueueContentsJob sumJob = new()
            {
                queue = queue,
                result = counter
            };
            JobHandle sumHandle = sumJob.Schedule();
            sumHandle.Complete();

            count = counter.Value;
            Dispose(ref counter);
            return count;
        }

        /// <summary>
        /// Copy a managed hashset to a unmanaged one.
        /// （將一個受控管集合複製至未控管集合。）
        /// </summary>
        /// <typeparam name="T">The type of the hashset items.（集合物件的型別。）</typeparam>
        /// <param name="source">The managed hashset.（受控管的集合。）</param>
        /// <param name="target">The target unmanaged hashset.（目標未控管集合。）</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void UnmanagedCopy<T>(HashSet<T> source, ref NativeParallelHashSet<T> target) where T : unmanaged, IEquatable<T>
        {
            if (!target.IsCreated)
            {
                throw new InvalidOperationException("The target hashset is not initialized. 集合未初始化。");
            }

            if (source.Count > target.Capacity)
            {
                throw new InvalidOperationException("The capacity of the target hashset is smaller than the managed hashset. 目標集合的容量較受控管集合小。");
            }

            foreach (T item in source)
            {
                target.Add(item);
            }
        }

        /// <summary>
        /// Copy a managed hashset to a unmanaged one.
        /// （將一個受控管集合複製至未控管集合。）
        /// </summary>
        /// <typeparam name="T">The type of the hashset items.（集合物件的型別。）</typeparam>
        /// <param name="source">The managed hashset.（受控管的集合。）</param>
        /// <param name="target">The target unmanaged hashset.（目標未控管集合。）</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void UnmanagedCopy<T>(HashSet<T> source, ref NativeParallelHashSet<EnumWrapper<T>> target) where T : unmanaged, Enum
        {
            if (!target.IsCreated)
            {
                throw new InvalidOperationException("The target hashset is not initialized. 集合未初始化。");
            }

            if (source.Count > target.Capacity)
            {
                throw new InvalidOperationException("The capacity of the target hashset is smaller than the managed hashset. 目標集合的容量較受控管集合小。");
            }

            foreach (T item in source)
            {
                target.Add(item);
            }
        }

        /// <summary>
        /// Validate the integrity of a native array.
        /// （驗證原生陣列的完整性。）
        /// </summary>
        /// <typeparam name="T">The type of the array items.（陣列內物件的型別。）</typeparam>
        /// <param name="array">The input array.（輸入的陣列。）</param>
        /// <param name="omitLength">Omit the length check.（省略長度檢查。）</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ValidateIntegrity<T>(ref NativeArray<T> array, bool omitLength = false) where T : struct
        {
            if (array == null || !array.IsCreated)
            {
                throw new InvalidOperationException("The array is not initialized. 陣列未初始化。");
            }
            
            if (!omitLength && array.Length == 0)
            {
                throw new InvalidOperationException("The array is empty. 陣列為空。");
            }
        }

        /// <summary>
        /// Validate the integrity of a native list.
        /// （驗證原生列表的完整性。）
        /// </summary>
        /// <typeparam name="T">The type of the list's items.（列表內物件的型別。）</typeparam>
        /// <param name="list">The input list.（輸入的列表。）</param>
        /// <param name="omitLength">Omit the length check.（省略長度檢查。）</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ValidateIntegrity<T>(ref NativeList<T> list, bool omitLength = false) where T : unmanaged
        {
            if (list.Equals(null) || !list.IsCreated)
            {
                throw new InvalidOperationException("The list is not initialized. 列表未初始化。");
            }

            if (!omitLength && list.Length == 0)
            {
                throw new InvalidOperationException("The list is empty. 列表為空。");
            }
        }

        /// <summary>
        /// Validate the integrity of a native hashmap (parallel variant).
        /// （驗證原生映射表（平行運算變種）的完整性。）
        /// </summary>
        /// <typeparam name="TKey">The type of the hashmap's keys.（映射表鍵的型別。）</typeparam>
        /// <typeparam name="TValue">The type of the hashmap's values.（映射表值的型別。）</typeparam>
        /// <param name="hashmap">The input hashmap.（輸入的映射表。）</param>
        /// <param name="omitLength">Omit the length check.（省略長度檢查。）</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ValidateIntegrity<TKey, TValue>(ref NativeParallelHashMap<TKey, TValue> hashmap, bool omitLength = false)
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            if (hashmap.Equals(null) || !hashmap.IsCreated)
            {
                throw new InvalidOperationException("The hashmap is not initialized. 映射表未初始化。");
            }

            if (!omitLength && hashmap.Count() == 0)
            {
                throw new InvalidOperationException("The hashmap is empty. 映射表為空。");
            }
        }

        /// <summary>
        /// The job to add a <see cref="NativeList{T}"/>'s contents to another list.
        /// （將一個 <see cref="NativeList{T}"/> 內容加入至另一個列表的工作。）
        /// </summary>
        [BurstCompile]
        private partial struct AddToJob<T> : IJobParallelFor
            where T : unmanaged
        {
            [ReadOnly]
            public NativeList<T> source;

            [WriteOnly]
            public NativeList<T>.ParallelWriter destination;

            public void Execute(int index)
            {
                destination.AddNoResize(source[index]);
            }
        }

        /// <summary>
        /// The job to sum up the numbers in the queue.
        /// （將佇列內數字相加的工作。 ） 
        /// </summary>
        [BurstCompile]
        private partial struct SumQueueContentsJob : IJob
        {
            [ReadOnly]
            public NativeQueue<int> queue;

            [WriteOnly]
            public NativeReference<int> result;

            public void Execute()
            {
                int count = 0;
                while (queue.TryDequeue(out int individualCount))
                {
                    count += individualCount;
                }
                result.Value = count;
            }
        }
    }
}