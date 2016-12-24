using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Coin.Banking
{
    public class BankViewModel : Screen
    {
        private string _bankName;
        private int _bankId;

        public int BankId
        {
            get { return _bankId; }
            set
            {
                if (value == _bankId) return;
                _bankId = value;
                NotifyOfPropertyChange(() => BankId);
            }
        }

        public string BankName
        {
            get { return _bankName; }
            set
            {
                if (value == _bankName) return;
                _bankName = value;
                NotifyOfPropertyChange(() => BankName);
            }
        }

        public void Cancel()
        {
            TryClose(null);
        }
    }
}
