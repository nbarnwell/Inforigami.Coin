using Caliburn.Micro;
using Coin.Data;

namespace Coin.Accounting
{
    public class AccountTransactionTypeViewModel : PropertyChangedBase
    {
        private int _id;
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

        public static AccountTransactionTypeViewModel CreateFrom(AccountTransactionType arg)
        {
            var vm = new AccountTransactionTypeViewModel {Id = arg.Id, Name = arg.Name};
            return vm;
        }
    }
}
