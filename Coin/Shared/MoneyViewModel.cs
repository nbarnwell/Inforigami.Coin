using Caliburn.Micro;

namespace Coin.Shared
{
    public class MoneyViewModel : PropertyChangedBase
    {
        private decimal? _amount;
        private CurrencyViewModel _currency;

        public decimal? Amount
        {
            get { return _amount; }
            set
            {
                if (value == _amount) return;
                _amount = value;
                NotifyOfPropertyChange(() => Amount);
            }
        }

        public CurrencyViewModel Currency
        {
            get { return _currency; }
            set
            {
                if (Equals(value, _currency)) return;
                _currency = value;
                NotifyOfPropertyChange(() => Currency);
            }
        }

        public static MoneyViewModel CreateFrom(decimal amount, CurrencyViewModel currency)
        {
            var vm = new MoneyViewModel {Amount = amount, Currency = currency};
            return vm;
        }

        public Money AsMoney()
        {
            return new Money((Amount ?? (decimal?)0).Value, Currency.Code);
        }
    }
}
