using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Coin.Shared;

namespace Coin.Banking
{
    public class BankListViewModel : Screen
    {
        public ScreenHeaderViewModel Header { get; set; }

        public BankListViewModel()
        {
            Header = new ScreenHeaderViewModel {HeaderText = "Banks"};
        }
    }
}
