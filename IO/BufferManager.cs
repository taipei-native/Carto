using Carto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carto.IO
{
    /// <summary>
    /// The temporary storage for the continuous data.
    /// （連續資料的臨時儲存處。）
    /// </summary>
    /// <typeparam name="T">The type of the items.（物件的型別。）</typeparam>
    internal class BufferManager<T>
    {
        /// <summary>
        /// The byte representation of the buffer contents.
        /// （緩衝區內容的位元組表達形式。）
        /// </summary>
        private readonly List<byte[]> _buffer;

        /// <summary>
        /// The byte offset of each buffer item.
        /// （各緩衝區物件的位元組偏移。）
        /// </summary>
        private readonly List<int> _offset;

        /// <summary>
        /// The suffix of the written buffer.
        /// （寫出緩衝區的後綴。）
        /// </summary>
        private readonly byte[] _suffix;

        /// <summary>
        /// The buffer contents.
        /// （緩衝區的內容。）
        /// </summary>
        public readonly List<T> items;

        public BufferManager ()
        {
            _buffer = new();
            _offset = new();
            _suffix = new byte[0];
            items = new();
        }

        public BufferManager (byte[] suffix)
        {
            _buffer = new List<byte[]>();
            _offset = new List<int>();
            _suffix = suffix;
            items = new();
        }

        /// <summary>
        /// Add a item to the buffer.
        /// （將一個物件添加至緩衝區。）
        /// </summary>
        /// <param name="item">The new item.（新物件。）</param>
        public void Add(T item)
        {
            byte[] itemInBytes = IOUtils.GetBytes(item);

            if (CommonUtils.TryGetLast(_buffer, out byte[] lastItem))
            {
                _ = CommonUtils.TryGetLast(_offset, out int lastOffset);
                _offset.Add(lastOffset + lastItem.Length);
            }
            else
            {
                _offset.Add(0);
            }

            _buffer.Add(itemInBytes);
            items.Add(item);
        }

        /// <summary>
        /// Add a item to the buffer and retrieve its length and offset.
        /// （將一個物件添加至緩衝區，並且獲得其長度與偏移量。）
        /// </summary>
        /// <param name="item">The new item.（新物件。）</param>
        /// <param name="includeSuffix">Whether to include the suffix length in the result length.（是否要將後綴的長度納入最終長度中？）</param>
        /// <param name="offset">The offset of the new item in bytes.（以位元組計的偏移量。）</param>
        /// <param name="length">The length of the new item in bytes.（以位元組計的長度。）</param>
        public void Add(T item, bool includeSuffix, out int offset, out int length)
        {
            Add(item);
            offset = GetLastOffset();
            length = GetLastLength(includeSuffix);
        }

        /// <summary>
        /// Add a list of items to the buffer.
        /// （將一系列物件添加至緩衝區。）
        /// </summary>
        /// <param name="items">The list of new items.（一系列的新物件。）</param>
        public void Add(IList<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Add(items[i]);
            }
        }

        /// <summary>
        /// Add a list of items and retrieve the overall offset and length.
        /// （將一系列物件添加至緩衝區，並且獲得整體長度與偏移量。）
        /// </summary>
        /// <param name="items">The list of new items.（一系列的新物件。）</param>
        /// <param name="offset">The offset of the first item in bytes.（首物件以位元組計的偏移量。）</param>
        /// <param name="length">The count of the items.（物件的計數。）</param>
        public void Add(IList<T> items, out int offset, out int length)
        {
            offset = 0;
            if (CommonUtils.TryGetLast(_buffer, out _))
            {
                offset += GetLastOffset() + GetLastLength(false);
            }

            length = 0;
            for (int i = 0; i < items.Count; i++)
            {
                Add(items[i]);
                length++;
            }
        }

        /// <summary>
        /// The number of items in the buffer.
        /// （緩衝區內含的物件數量。）
        /// </summary>
        /// <returns>The number of items in the buffer.（緩衝區內含的物件數量。）</returns>
        public int Count()
        {
            return items.Count;
        }

        /// <summary>
        /// Write the buffer contents and reset the memory.
        /// （將緩衝區寫出並且重置記憶體。）
        /// </summary>
        /// <param name="writer">The binary stream writer.（二進位資料流寫出者。）</param>
        /// <param name="forceLittleEndian">
        /// Whether to force writing in little endian or not. If false, the buffer will be written in the current device's endianness.<br/>
        /// （是否要強制以小端序寫出？若為假，緩衝區將依照目前裝置的端序寫出。）
        /// </param>
        public void Flush(BinaryWriter writer, bool forceLittleEndian = false)
        {
            if (!forceLittleEndian || BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < _buffer.Count; i++)
                {
                    writer.Write(_buffer[i]);
                }

                if (_suffix.Length > 0)
                {
                    writer.Write(_suffix);
                }
            }
            else
            {
                for (int i = 0; i < _buffer.Count; i++)
                {
                    writer.Write(_buffer[i].Reverse().ToArray());
                }

                if (_suffix.Length > 0)
                {
                    writer.Write(_suffix.Reverse().ToArray());
                }
            }

            _buffer.Clear();
            _offset.Clear();
            items.Clear();
        }

        /// <summary>
        /// Retrieve the buffer length in bytes.
        /// （獲得緩衝區以位元組計的長度。）
        /// </summary>
        /// <returns>The length of the buffer.（緩衝區的長度。）</returns>
        public int GetFullLength()
        {
            if (CommonUtils.TryGetLast(_buffer, out byte[] lastItem))
            {
                _ = CommonUtils.TryGetLast(_offset, out int lastOffset);
                return lastOffset + lastItem.Length + _suffix.Length;
            }

            return 0;
        }

        /// <summary>
        /// Retrieve the length of the last item of the buffer in bytes.
        /// （獲得緩衝區最後一個物件以位元組計的長度。）
        /// </summary>
        /// <param name="includeSuffix">Whether to include the suffix length in the result length.（是否要將後綴的長度納入最終長度中？）</param>
        /// <returns>The length in bytes.（以位元組計的長度。）</returns>
        public int GetLastLength(bool includeSuffix)
        {
            int length = GetLength(_buffer.Count - 1, includeSuffix);
            return length;
        }

        /// <summary>
        /// Retrieve the offset of the last item of the buffer.
        /// （獲得緩衝區最後一個物件的偏移量。）
        /// </summary>
        /// <returns>The byte offset.（位元組偏移量。）</returns>
        public int GetLastOffset()
        {
            return GetOffset(_buffer.Count - 1);
        }

        /// <summary>
        /// Retrieve the length of the designated item of the buffer in bytes.
        /// （獲得緩衝區中指定物件以位元組計的長度。）
        /// </summary>
        /// <param name="index">The index of the item.（物件的索引值。）</param>
        /// <param name="includeSuffix">Whether to include the suffix length in the result length.（是否要將後綴的長度納入最終長度中？）</param>
        /// <returns>The length in bytes.（以位元組計的長度。）</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public int GetLength(int index, bool includeSuffix)
        {
            if (index < 0 || index >= _buffer.Count)
            {
                throw new IndexOutOfRangeException($"The index should be non negative and smaller than {_buffer.Count}. index 應為非負且小於 {_buffer.Count}。");
            }

            return index == _buffer.Count - 1 ? _buffer[index].Length + (includeSuffix ? _suffix.Length : 0) : _buffer[index].Length;
        }

        /// <summary>
        /// Retrieve the offset of the designated item of the buffer.
        /// （獲得緩衝區中指定物件的偏移量。）
        /// </summary>
        /// <param name="index">The index of the item.（物件的索引值。）</param>
        /// <returns>The byte offset.（位元組偏移量。）</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public int GetOffset(int index)
        {
            if (index < 0 || index >= _offset.Count)
            {
                throw new IndexOutOfRangeException($"The index should be non negative and smaller than {_offset.Count}. index 應為非負且小於 {_offset.Count}。");
            }

            return _offset[index];
        }

        public T this[int index] => items[index];
    }
}