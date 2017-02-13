using Caliburn.Micro;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountTransactionCategorySplitViewModel : PropertyChangedBase
    {
        private decimal _splitAmount;
        private ListItemViewModel _category;

        public decimal SplitAmount
        {
            get { return _splitAmount; }
            set
            {
                if (value == _splitAmount) return;
                _splitAmount = value;
                NotifyOfPropertyChange(() => SplitAmount);
            }
        }

        public ListItemViewModel Category
        {
            get { return _category; }
            set
            {
                if (Equals(value, _category)) return;
                _category = value;
                NotifyOfPropertyChange(() => Category);
            }
        }

        public AccountTransactionCategorySplitViewModel(decimal splitAmount, ListItemViewModel category)
        {
            SplitAmount = splitAmount;
            Category = category;
        }
    }
}