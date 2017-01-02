using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Shared
{
    public class Currency
    {
        public decimal Amount { get; private set; }
        public string CurrencyCode { get; private set; }

        public Currency(decimal amount, string currencyCode)
        {
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public static Currency operator +(Currency a, Currency b)
        {
            if (a.CurrencyCode.Equals(b.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return new Currency(a.Amount + b.Amount, a.CurrencyCode);
            }

            throw new InvalidOperationException("Cannot add two currency values with different currency codes");
        }

        public static Currency operator -(Currency a, Currency b)
        {
            if (a.CurrencyCode.Equals(b.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return new Currency(a.Amount - b.Amount, a.CurrencyCode);
            }

            throw new InvalidOperationException("Cannot subtract two currency values with different currency codes");
        }

        public static Currency operator *(Currency a, decimal multiplier)
        {
            return new Currency(a.Amount * multiplier, a.CurrencyCode);
        }

        public static Currency operator /(Currency a, decimal divisor)
        {
            return new Currency(a.Amount / divisor, a.CurrencyCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Currency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Amount.GetHashCode()*397) ^ (CurrencyCode != null ? CurrencyCode.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"{Amount:F2} {CurrencyCode}";
        }

        protected bool Equals(Currency other)
        {
            return Amount == other.Amount && string.Equals(CurrencyCode, other.CurrencyCode);
        }
    }
}
