using Caliburn.Micro;

namespace Coin.CRUD.Accounts
{
    public class SortCodeViewModel : PropertyChangedBase
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value) return;
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        public SortCode GetSortCode()
        {
            return new SortCode(Value);
        }
    }
}