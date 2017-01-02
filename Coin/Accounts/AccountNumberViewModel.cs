using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Coin.Accounts
{
    public class AccountNumberViewModel : PropertyChangedBase
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

        public AccountNumber GetAccountNumber()
        {
            return new AccountNumber(Value);
        }
    }
}
