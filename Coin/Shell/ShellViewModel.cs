using System;
using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.Shell
{
    public class ShellViewModel : Conductor<IScreen>, IShell
    {
        public HeaderViewModel        Header        { get; private set; }
        public ToolbarViewModel       Toolbar       { get; private set; }
        public WorkspaceHostViewModel WorkspaceHost { get; private set; }

        public ShellViewModel(HeaderViewModel header, ToolbarViewModel toolbar, WorkspaceHostViewModel workspaceHost)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));
            if (toolbar == null) throw new ArgumentNullException(nameof(toolbar));
            if (workspaceHost == null) throw new ArgumentNullException(nameof(workspaceHost));

            Header = header;
            Toolbar = toolbar;
            WorkspaceHost = workspaceHost;
        }

        protected override void OnActivate()
        {
            ActivateItem(WorkspaceHost);
        }
    }
}