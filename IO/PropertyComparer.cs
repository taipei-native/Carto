using System.Collections.Generic;

namespace Carto.IO
{
    /// <summary>
    /// The comparer to sort <see cref="Property"/>.
    /// （用於排列 <see cref="Property"/> 的比較器。）
    /// </summary>
    public class PropertyComparer : IComparer<Property>
    {
        public int Compare(Property left, Property right)
        {
            // `Name` is always at the first place.（`Name` 總是在第一個。） 
            if (left == Property.Name) return -1;
            if (right == Property.Name) return 1;

            // `Unknown` is always at the last place.（`Unknown` 總是在最後一個。）
            if (left == Property.Unknown) return 1;
            if (right == Property.Unknown) return -1;

            return ((int)left).CompareTo((int)right);
        }
    }
}