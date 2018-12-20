using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coin.Web.Services
{
    public static class EnumerableExtensions
    {
        public static bool Between<T>(this T value, T lower, T upper)
            where T : IComparable
        {
            return value.CompareTo(lower) < 0 && value.CompareTo(upper) >= 0;
        }
    }
}
