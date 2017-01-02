﻿using System;
using Caliburn.Micro;
using Coin.Accounts;
using Coin.Banking;
using Coin.Infrastructure;
using Coin.People;
using Coin.Shared;

namespace Coin.Shell
{
    public class NavbarViewModel : PropertyChangedBase
    {
        private readonly WorkspaceHostViewModel _workspaceHost;
        private readonly IViewModelFactory _viewModelFactory;

        private IScreen _selectedWorkspace;

        public IScreen SelectedWorkspace
        {
            get { return _selectedWorkspace; }
            set
            {
                if (Equals(value, _selectedWorkspace)) return;
                _selectedWorkspace = value;
                _workspaceHost.ActivateItem(value);
                NotifyOfPropertyChange(() => SelectedWorkspace);
            }
        }

        public BindableCollection<IScreen> Workspaces { get; }

        public NavbarViewModel(WorkspaceHostViewModel workspaceHost, IViewModelFactory viewModelFactory)
        {
            if (workspaceHost == null) throw new ArgumentNullException(nameof(workspaceHost));
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));

            _workspaceHost = workspaceHost;
            _viewModelFactory = viewModelFactory;

            Workspaces = new BindableCollection<IScreen>
            {
                _viewModelFactory.Create<WorkspaceViewModel>()
                                 .WithViewModel(
                                     _viewModelFactory.Create<AccountListViewModel>()),
                _viewModelFactory.Create<WorkspaceViewModel>()
                                 .WithViewModel(
                                     _viewModelFactory.Create<BankListViewModel>()),
                _viewModelFactory.Create<WorkspaceViewModel>()
                                 .WithViewModel(
                                     _viewModelFactory.Create<PersonListViewModel>())
            };

        }
    }
}
