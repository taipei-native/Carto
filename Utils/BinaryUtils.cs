namespace Carto.Utils
{
    using Carto.Geodata;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A class to provide easy interface to manipulate binary data.
    /// （一個提供操作二進位資料的簡易介面的類別。）
    /// </summary>
    public class BinaryUtils
    {
        /// <summary>
        /// Add <paramref name="items"/> into <paramref name="target"/> by converting them into byte array.
        /// （將 <paramref name="items"/> 轉換為位元組並加入到 <paramref name="target"/> 列表中。）
        /// </summary>
        public static void AddBytes(List<byte> target, params object[] items)
        {
            foreach (var item in items)
            {
                switch (item)
                {
                    case byte @byte:
                        target.Add(@byte);
                        break;

                    case double @double:
                        target.AddRange(BitConverter.GetBytes(@double));
                        break;

                    case float @float:
                        target.AddRange(BitConverter.GetBytes(@float));
                        break;

                    case int @int:
                        target.AddRange(BitConverter.GetBytes(@int));
                        break;

                    case long @long:
                        target.AddRange(BitConverter.GetBytes(@long));
                        break;

                    case short @short:
                        target.AddRange(BitConverter.GetBytes(@short));
                        break;

                    case ushort @ushort:
                        target.AddRange(BitConverter.GetBytes(@ushort));
                        break;

                    case string @string:
                        target.AddRange(Encoding.UTF8.GetBytes(@string));
                        break;
                }
            }
        }

        /// <summary>
        /// Add a 8-byte GeoKey into <paramref name="target"/>.
        /// （將一個8位元組的 GeoKey 加入到 <paramref name="target"/> 列表中。）
        /// </summary>
        /// <param name="num">Serial number of the GeoKey. （GeoKey的序號。）</param>
        /// <param name="location">Reference TIFF Tag location (Tag number) or 0. （參照的TIFF標籤序號，或為0。）</param>
        /// <param name="count">Value count. （數值的數量。）</param>
        /// <param name="data">Value offset or value itself, when the data is less than 3 bytes. （數值偏移或是數值本身（當資料小於3位元組時。））</param>
        internal static void AddGeoKey(List<byte> target, int num, int location, int count, int data)
        {
            AddBytes(target, (ushort)num, (ushort)location, (ushort)count, (ushort)data);
        }

        /// <summary>
        /// Add a 12-byte TIFF/EXIF tag into <paramref name="target"/>.
        /// （將一個12位元組的 TIFF/EXIF 標籤加入到 <paramref name="target"/> 列表中。）
        /// </summary>
        /// <param name="num">Serial number of the tag. （標籤的序號。）</param>
        /// <param name="field">Designated field type. （指定的欄位類型。）</param>
        /// <param name="count">Value count. （數值的數量。）</param>
        /// <param name="data">Value offset or value itself, when the data is less than 5 bytes. （數值偏移或是數值本身（當資料小於5位元組時。））</param>
        internal static void AddTag(List<byte> target, int num, Geodata.TField field, int count, object data)
        {
            AddBytes(target, (ushort)num, (short)field, count, data);
        }

        /// <summary>
        /// Flip the endianess. Since C# uses little endian, the flipped number uses big endian.
        /// （翻轉端序。由於 C# 使用小端序，因此翻轉後的數字是大端序。）
        /// </summary>
        public static int FlipEndian(int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Replace bytes of <paramref name="target"/> from <paramref name="startIndex"/> using <paramref name="item"/>.
        /// （以 <paramref name="item"/> 取代從 <paramref name="target"/> 列表中索引值 <paramref name="startIndex"/> 開始的位元組。）
        /// </summary>
        public static void ReplaceBytes(List<byte> target, int startIndex, object item)
        {
            byte[] buffer;

            switch (item)
            {
                case double @double:
                    buffer = BitConverter.GetBytes(@double);
                    break;

                case float @float:
                    buffer = BitConverter.GetBytes(@float);
                    break;

                case int @int:
                    buffer = BitConverter.GetBytes(@int);
                    break;

                case long @long:
                    buffer = BitConverter.GetBytes(@long);
                    break;

                case short @short:
                    buffer = BitConverter.GetBytes(@short);
                    break;

                case string @string:
                    buffer = Encoding.UTF8.GetBytes(@string);
                    break;

                default:
                    buffer = BitConverter.GetBytes(0);
                    break;
            }

            if (startIndex < target.Count - 1 && startIndex + buffer.Length <= target.Count - 1)
            {
                int stepper = startIndex;

                foreach (var by in buffer)
                {
                    target[stepper] = by;
                    stepper++;
                }
            }
        }

        /// <summary>
        /// Add empty bytes into <paramref name="target"/>.
        /// （將空白位元組加入到 <paramref name="target"/> 列表中。）
        /// </summary>
        public static void Skip(List<byte> target, int count, string mode = "int")
        {
            if (mode == "long") { for (int i = 0; i < count; i++) target.AddRange(BitConverter.GetBytes((long)0)); }
            if (mode == "int") { for (int i = 0; i < count; i++) target.AddRange(BitConverter.GetBytes(0)); }
            if (mode == "short") { for (int i = 0; i < count; i++) target.AddRange(BitConverter.GetBytes((short)0)); }
            if (mode == "byte") { for (int i = 0; i < count; i++) target.Add(0); }
        }
    }
}