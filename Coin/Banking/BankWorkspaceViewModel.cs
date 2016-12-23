using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Banking
{
    public class BankWorkspaceViewModel : WorkspaceViewModelBase<BankListViewModel>
    {
        public override string DisplayName => "Banks";

        public BankWorkspaceViewModel(IViewModelFactory viewModelFactory) 
            : base(viewModelFactory)
        {
        }
    }
}
