using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Utility
{
    public static class CollectionExtensions
    {
        public static int FindIndex<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            int index = 0;
            foreach (var item in items)
            {
                if (predicate(item)) break;
                index++;
            }
            return index;
        }

        public static bool IsLastItem<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            var lastItem = items.LastOrDefault();
            if (lastItem == null)
            {
                return false;
            }

            return predicate(lastItem);
        }
    }
}
