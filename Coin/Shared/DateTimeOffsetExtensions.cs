using System;

namespace Coin.Shared
{
    public static class DateTimeOffsetExtensions
    {
        public static bool IsBetween(this DateTimeOffset target, DateTimeOffset start, DateTimeOffset end)
        {
            return target >= start && target < end;
        }
    }
}