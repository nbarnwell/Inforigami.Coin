using System;
using Caliburn.Micro;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Shell
{
    public class ShellViewModel : Conductor<IScreen>, IShell
    {
        private readonly IEventAggregator _events;
        private HeaderViewModel _header;
        private NavbarViewModel _navbar;
        private WorkspaceHostViewModel _workspaceHost;

        public HeaderViewModel Header
        {
            get { return _header; }
            private set
            {
                if (Equals(value, _header)) return;
                _header = value;
                NotifyOfPropertyChange(() => Header);
            }
        }

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

        public ShellViewModel(HeaderViewModel header, NavbarViewModel navbar, WorkspaceHostViewModel workspaceHost, IEventAggregator events)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));
            if (navbar == null) throw new ArgumentNullException(nameof(navbar));
            if (workspaceHost == null) throw new ArgumentNullException(nameof(workspaceHost));
            if (events == null) throw new ArgumentNullException(nameof(events));

            _events = events;

            Header = header;
            Navbar = navbar;
            WorkspaceHost = workspaceHost;
        }

        protected override void OnActivate()
        {
            ActivateItem(WorkspaceHost);
        }

        public void Refresh()
        {
            _events.PublishOnUIThread(new RefreshRequested());
        }
    }
}