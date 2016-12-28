using System;
using System.Collections.Generic;

namespace Coin.Infrastructure
{
    public static class ListExtensions
    {
        public static void InsertWhere<T>(this IList<T> list, Func<T, bool> predicate, T item)
        {
            int i = 0;
            foreach (var person in list)
            {
                if (predicate(person))
                {
                    break;
                }

                i++;
            }

            list.Insert(i, item);
        }
    }
}