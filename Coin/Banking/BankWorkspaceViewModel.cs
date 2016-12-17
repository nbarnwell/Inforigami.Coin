using System;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Banking
{
    public class BankWorkspaceViewModel : Conductor<IScreen>
    {
        private readonly IViewModelFactory _viewModelFactory;

        public BankWorkspaceViewModel(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;
        }

        protected override void OnActivate()
        {
            ActivateItem(_viewModelFactory.Create<BankListViewModel>());
        }
    }
}
