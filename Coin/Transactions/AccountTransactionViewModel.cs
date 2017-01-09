using System;
using Caliburn.Micro;
using Coin.Data;

namespace Coin.Transactions
{
    public class AccountTransactionViewModel : Screen
    {
        private int _id;
        private int _accountStatementId;
        private DateTimeOffset? _transactionTime;
        private DateTime _recordedDate;
        private int _accountTransactionStatusId;
        private string _accountTransactionStatusName;
        private decimal _amount;
        private string _payee;
        private string _description;
        private int _accountTransactionTypeId;
        private string _accountTransactionTypeName;

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

        public int AccountStatementId
        {
            get { return _accountStatementId; }
            set
            {
                if (value == _accountStatementId) return;
                _accountStatementId = value;
                NotifyOfPropertyChange(() => AccountStatementId);
            }
        }

        public System.DateTimeOffset? TransactionTime
        {
            get { return _transactionTime; }
            set
            {
                if (value.Equals(_transactionTime)) return;
                _transactionTime = value;
                NotifyOfPropertyChange(() => TransactionTime);
            }
        }

        public System.DateTime RecordedDate
        {
            get { return _recordedDate; }
            set
            {
                if (value.Equals(_recordedDate)) return;
                _recordedDate = value;
                NotifyOfPropertyChange(() => RecordedDate);
            }
        }

        public int AccountTransactionStatusId
        {
            get { return _accountTransactionStatusId; }
            set
            {
                if (value == _accountTransactionStatusId) return;
                _accountTransactionStatusId = value;
                NotifyOfPropertyChange(() => AccountTransactionStatusId);
            }
        }

        public string AccountTransactionStatusName
        {
            get { return _accountTransactionStatusName; }
            set
            {
                if (value == _accountTransactionStatusName) return;
                _accountTransactionStatusName = value;
                NotifyOfPropertyChange(() => AccountTransactionStatusName);
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (value == _amount) return;
                _amount = value;
                NotifyOfPropertyChange(() => Amount);
            }
        }

        public string Payee
        {
            get { return _payee; }
            set
            {
                if (value == _payee) return;
                _payee = value;
                NotifyOfPropertyChange(() => Payee);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        public int AccountTransactionTypeId
        {
            get { return _accountTransactionTypeId; }
            set
            {
                if (value == _accountTransactionTypeId) return;
                _accountTransactionTypeId = value;
                NotifyOfPropertyChange(() => AccountTransactionTypeId);
            }
        }

        public string AccountTransactionTypeName
        {
            get { return _accountTransactionTypeName; }
            set
            {
                if (value == _accountTransactionTypeName) return;
                _accountTransactionTypeName = value;
                NotifyOfPropertyChange(() => AccountTransactionTypeName);
            }
        }

        public static AccountTransactionViewModel CreateFrom(AccountTransaction arg)
        {
            var vm = new AccountTransactionViewModel
            {
                Id                           = arg.Id,
                AccountStatementId           = (int) arg.AccountStatementId,
                Amount                       = arg.Amount,
                AccountTransactionStatusId   = arg.AccountTransactionStatusId,
                AccountTransactionStatusName = arg.AccountTransactionStatu.Name,
                AccountTransactionTypeId     = arg.AccountTransactionTypeId,
                AccountTransactionTypeName   = arg.AccountTransactionType.Name,
                Description                  = arg.Description,
                Payee                        = arg.Payee,
                RecordedDate                 = arg.RecordedDate,
                TransactionTime              = arg.TransactionTime
            };

            return vm;
        }
    }
}