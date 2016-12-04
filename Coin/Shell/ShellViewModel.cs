using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.Shell
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        public HeaderViewModel Header { get; private set; }
        public ToolbarViewModel Toolbar { get; private set; }

        public ShellViewModel(IViewModelFactory viewModelFactory)
        {
            Header = viewModelFactory.Create<HeaderViewModel>();
            Toolbar = viewModelFactory.Create<ToolbarViewModel>();
        }
    }
}