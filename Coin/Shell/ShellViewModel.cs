using System;
using Caliburn.Micro;
using Coin.Shared;

namespace Coin.Shell
{
    public class ShellViewModel : Conductor<IScreen>, IShell
    {
        public AppBarViewModel AppBar { get; set; }
        public SideNavViewModel SideNav { get; set; }
        public WorkspaceHostViewModel WorkspaceHost { get; set; }

        public override string DisplayName => "Coin";

        public ShellViewModel(
            AppBarViewModel appBar,
            SideNavViewModel sideNav,
            WorkspaceHostViewModel workspaceHost)
        {
            if (appBar == null) throw new ArgumentNullException(nameof(appBar));
            if (sideNav == null) throw new ArgumentNullException(nameof(sideNav));
            if (workspaceHost == null) throw new ArgumentNullException(nameof(workspaceHost));

            AppBar = appBar;
            SideNav = sideNav;
            WorkspaceHost = workspaceHost;
        }

        protected override void OnActivate()
        {
            ActivateItem(WorkspaceHost);
        }
    }
}