using System;
using Caliburn.Micro;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Banking
{
    public class BankWorkspaceViewModel : WorkspaceViewModelBase
    {
        private readonly IViewModelFactory _viewModelFactory;

        public override string DisplayName => "Banks";

        public BankWorkspaceViewModel(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;
        }

        protected override IScreen GetDefaultViewModel()
        {
            return _viewModelFactory.Create<BankListViewModel>();
        }
    }
}
