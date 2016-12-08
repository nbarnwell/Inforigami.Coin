using System;
using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.Shell
{
    public class ShellViewModel : Conductor<IScreen>, IShell
    {
        public HeaderViewModel        Header        { get; private set; }
        public NavbarViewModel       Navbar       { get; private set; }
        public WorkspaceHostViewModel WorkspaceHost { get; private set; }

        public ShellViewModel(HeaderViewModel header, NavbarViewModel navbar, WorkspaceHostViewModel workspaceHost)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));
            if (navbar == null) throw new ArgumentNullException(nameof(navbar));
            if (workspaceHost == null) throw new ArgumentNullException(nameof(workspaceHost));

            Header = header;
            Navbar = navbar;
            WorkspaceHost = workspaceHost;
        }

        protected override void OnActivate()
        {
            ActivateItem(WorkspaceHost);
        }
    }
}