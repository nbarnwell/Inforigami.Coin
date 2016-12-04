namespace Coin
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        public HeaderViewModel Header { get; private set; }
        public ToolbarViewModel Toolbar { get; private set; }

        public ShellViewModel()
        {
            Header = new HeaderViewModel();
            Toolbar = new ToolbarViewModel();
        }
    }
}