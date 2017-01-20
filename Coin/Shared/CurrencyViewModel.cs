using Caliburn.Micro;
using Coin.Data;

namespace Coin.Shared
{
    public class CurrencyViewModel : PropertyChangedBase
    {
        private int _id;
        private string _code;
        private string _name;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (value == _code) return;
                _code = value;
                NotifyOfPropertyChange(() => Code);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public static CurrencyViewModel CreateFrom(Currency currency)
        {
            var vm = new CurrencyViewModel {Id = currency.Id, Code = currency.Code, Name = currency.Name};
            return vm;
        }
    }
}