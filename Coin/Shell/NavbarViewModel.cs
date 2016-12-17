using System;
using Caliburn.Micro;
using Coin.Banking;
using Coin.Infrastructure;

namespace Coin.Shell
{
    public class NavbarViewModel : PropertyChangedBase
    {
        private readonly WorkspaceHostViewModel _workspaceHost;
        private readonly IViewModelFactory _viewModelFactory;

        public NavbarViewModel(WorkspaceHostViewModel workspaceHost, IViewModelFactory viewModelFactory)
        {
            if (workspaceHost == null) throw new ArgumentNullException(nameof(workspaceHost));
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));

            _workspaceHost = workspaceHost;
            _viewModelFactory = viewModelFactory;
        }

        public void ShowBankList()
        {
            var viewModel = _viewModelFactory.Create<BankWorkspaceViewModel>();
            _workspaceHost.ActivateItem(viewModel);
        }
    }
}
