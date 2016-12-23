using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Shared;

namespace Coin.Banking
{
    public class BankListViewModel : Conductor<PropertyChangedBase>
    {
        public BindableCollection<BankViewModel> Banks { get; set; }

        private BankViewModel _selectedBank;
        public BankViewModel SelectedBank
        {
            get { return _selectedBank; }
            set
            {
                if (Equals(value, _selectedBank)) return;
                _selectedBank = value;
                NotifyOfPropertyChange();
            }
        }

        public BankListViewModel()
        {
            Banks = new BindableCollection<BankViewModel>();
        }

        protected override void OnActivate()
        {
            using (var db = new Database())
            {
                Banks.AddRange(
                    db.Banks
                      .OrderBy(x => x.Name)
                      .Select(x => new BankViewModel
                      {
                          BankId = x.Id,
                          BankName = x.Name
                      }));
            }
        }

        public void AddBank()
        {
            ActivateItem(new BankViewModel());
        }
    }
}
