using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Coin.Banking
{
    public class BankViewModel : PropertyChangedBase
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
    }
}
