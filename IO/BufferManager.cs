using Carto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carto.IO
{
    internal class BufferManager<T>
    {
        private readonly List<byte[]> _buffer;

        private readonly List<int> _offset;

        private readonly byte[] _suffix;

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

        public void Add(T item, bool includeSuffix, out int offset, out int length)
        {
            Add(item);
            offset = GetLastOffset();
            length = GetLastLength(includeSuffix);
        }

        public void Add(IList<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Add(items[i]);
            }
        }

        public void Add(IList<T> items, out int offset, out int length)
        {
            offset = GetLastOffset() + GetLastLength(false);
            length = 0;
            for (int i = 0; i < items.Count; i++)
            {
                Add(items[i]);
                length++;
            }
        }

        public int Count()
        {
            return items.Count;
        }

        public void Flush(BinaryWriter writer, bool considerEndianess = true)
        {
            if (!considerEndianess || BitConverter.IsLittleEndian)
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

        public int GetFullLength()
        {
            if (CommonUtils.TryGetLast(_buffer, out byte[] lastItem))
            {
                _ = CommonUtils.TryGetLast(_offset, out int lastOffset);
                return lastOffset + lastItem.Length + _suffix.Length;
            }

            return 0;
        }

        public int GetLastLength(bool includeSuffix)
        {
            int length = GetLength(_buffer.Count - 1, includeSuffix);
            return length;
        }

        public int GetLastOffset()
        {
            return GetOffset(_buffer.Count - 1);
        }

        public int GetLength(int index, bool includeSuffix)
        {
            if (index < 0 || index >= _buffer.Count)
            {
                throw new IndexOutOfRangeException($"The index should be non negative and smaller than {_buffer.Count}. index 應為非負且小於 {_buffer.Count}。");
            }

            return index == _buffer.Count - 1 ? _buffer[index].Length + (includeSuffix ? _suffix.Length : 0) : _buffer[index].Length;
        }

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