using System;
using Caliburn.Micro;
using Coin.Shared;

namespace Coin.Shell
{
    public class ShellViewModel : Conductor<IScreen>, IShell
    {
        private readonly IEventAggregator _events;
        private NavbarViewModel _navbar;
        private WorkspaceHostViewModel _workspaceHost;

        public IConductor DialogConductor { get; private set; }

        public NavbarViewModel Navbar
        {
            get { return _navbar; }
            private set
            {
                if (Equals(value, _navbar)) return;
                _navbar = value;
                NotifyOfPropertyChange(() => Navbar);
            }
        }

        public WorkspaceHostViewModel WorkspaceHost
        {
            get { return _workspaceHost; }
            private set
            {
                if (Equals(value, _workspaceHost)) return;
                _workspaceHost = value;
                NotifyOfPropertyChange(() => WorkspaceHost);
            }
        }

        public override string DisplayName => "Coin";

        public ShellViewModel(NavbarViewModel navbar, WorkspaceHostViewModel workspaceHost, IEventAggregator events)
        {
            if (navbar == null) throw new ArgumentNullException(nameof(navbar));
            if (workspaceHost == null) throw new ArgumentNullException(nameof(workspaceHost));
            if (events == null) throw new ArgumentNullException(nameof(events));

            _events = events;

            Navbar = navbar;
            WorkspaceHost = workspaceHost;
        }

        protected override void OnActivate()
        {
            ActivateItem(WorkspaceHost);
        }

        public void RefreshData()
        {
            _events.PublishOnUIThread(new RefreshRequested());
        }
    }
}