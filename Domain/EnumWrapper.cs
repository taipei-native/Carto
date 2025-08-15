using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Carto.Domain
{
    /// <summary>
    /// The wrapper struct of enums that implements IComparable &amp; IEquatable.<br/>
    /// （包裝枚舉並實作 IComparable 及 IEquatable 的結構。）
    /// </summary>
    /// <typeparam name="T">The enum's type.（枚舉的型別。）</typeparam>
    public struct EnumWrapper<T> : IComparable, IComparable<EnumWrapper<T>>, IEquatable<EnumWrapper<T>>
        where T : unmanaged, Enum
    {
        /// <summary>
        /// The enum value.
        /// （枚舉的數值。）
        /// </summary>
        public T value;

        public EnumWrapper(T property)
        {
            value = property;
        }

        public readonly int CompareTo(object obj)
        {
            if (obj is EnumWrapper<T> other)
            {
                return UnsafeUtility.EnumToInt(value).CompareTo(UnsafeUtility.EnumToInt(other.value));
            }
            throw new ArgumentException($"Object is not an {typeof(EnumWrapper<T>)}");
        }

        public readonly int CompareTo(EnumWrapper<T> other) => UnsafeUtility.EnumToInt(value).CompareTo(UnsafeUtility.EnumToInt(other.value));

        public override readonly bool Equals(object obj) => obj is EnumWrapper<T> other && Equals(other);

        public readonly bool Equals(EnumWrapper<T> other) => UnsafeUtility.EnumToInt(value) == UnsafeUtility.EnumToInt(other.value);

        public override readonly int GetHashCode() => UnsafeUtility.EnumToInt(value);

        public override readonly string ToString() => $"EnumWrapper<{nameof(T)}> ({value})";

        public static bool operator ==(EnumWrapper<T> left, EnumWrapper<T> right) => left.Equals(right);
        public static bool operator !=(EnumWrapper<T> left, EnumWrapper<T> right) => !left.Equals(right);
        public static bool operator >(EnumWrapper<T> left, EnumWrapper<T> right) => left.CompareTo(right) > 0;
        public static bool operator <(EnumWrapper<T> left, EnumWrapper<T> right) => left.CompareTo(right) < 0;
        public static bool operator >=(EnumWrapper<T> left, EnumWrapper<T> right) => left.CompareTo(right) >= 0;
        public static bool operator <=(EnumWrapper<T> left, EnumWrapper<T> right) => left.CompareTo(right) <= 0;

        public static implicit operator EnumWrapper<T>(T enumValue) => new(enumValue);
        public static implicit operator T(EnumWrapper<T> wrapper) => wrapper.value;
    }
}