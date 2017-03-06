using System;
using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.Transactions
{
    public class AccountingWorkspace : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IViewModelFactory _viewModelFactory;

        public override string DisplayName => "Accounting";

        public AccountingWorkspace(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;
        }

        protected override void OnActivate()
        {
            if (ActiveItem == null)
            {
                ActivateItem(_viewModelFactory.Create<AccountListScreen>());
            }
        }
    }
}