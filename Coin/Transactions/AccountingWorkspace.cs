using System;
using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.Transactions
{
    public class AccountingWorkspace : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IViewModelFactory _viewModelFactory;

        public AccountingWorkspace(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;
        }

        protected override void OnInitialize()
        {
            ActivateItem(_viewModelFactory.Create<AccountListScreen>());
        }
    }
}