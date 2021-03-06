﻿using Caliburn.Micro;

namespace Coin.Accounting
{
    public class AccountTransactionStatusViewModel : PropertyChangedBase
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

        public static AccountTransactionStatusViewModel CreateFrom(Data.AccountTransactionStatus status)
        {
            var vm = new AccountTransactionStatusViewModel {Id = status.Id, Name = status.Name};
            return vm;
        }
    }
}