using Caliburn.Micro;

namespace Coin.Shared
{
    public class MoneyViewModel : PropertyChangedBase
    {
        private decimal? _amount;
        private CurrencyViewModel _currency;
        private string _amountInput;

        public string AmountInput
        {
            get { return _amountInput; }
            set
            {
                if (value == _amountInput) return;
                _amountInput = value;
                NotifyOfPropertyChange(() => AmountInput);
            }
        }

        public decimal? Amount
        {
            get
            {
                decimal amount;
                decimal.TryParse(AmountInput, out amount);
                return amount;
            }

            set
            {
                if (value == _amount) return;
                _amount = value;
                AmountInput = _amount.ToString();
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
