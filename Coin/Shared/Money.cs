using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Shared
{
    public class Money
    {
        public decimal Amount { get; }
        public string CurrencyCode { get; }

        public Money(decimal amount, string currencyCode)
        {
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.CurrencyCode.Equals(b.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return new Money(a.Amount + b.Amount, a.CurrencyCode);
            }

            throw new InvalidOperationException("Cannot add two currency values with different currency codes");
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.CurrencyCode.Equals(b.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return new Money(a.Amount - b.Amount, a.CurrencyCode);
            }

            throw new InvalidOperationException("Cannot subtract two currency values with different currency codes");
        }

        public static Money operator *(Money a, decimal multiplier)
        {
            return new Money(a.Amount * multiplier, a.CurrencyCode);
        }

        public static Money operator /(Money a, decimal divisor)
        {
            return new Money(a.Amount / divisor, a.CurrencyCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Money) obj);
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

        protected bool Equals(Money other)
        {
            return Amount == other.Amount && string.Equals(CurrencyCode, other.CurrencyCode);
        }
    }
}
