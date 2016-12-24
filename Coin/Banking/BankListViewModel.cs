using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Banking
{
    public class BankListViewModel : Conductor<PropertyChangedBase>
    {
        private readonly IViewModelFactory _viewModelFactory;
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

        public BankListViewModel(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;
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

        public IEnumerable<IResult> AddBank()
        {
            var bankViewModel = _viewModelFactory.Create<BankViewModel>();
            yield return new ShowDialog(bankViewModel);
            //(Parent as BankWorkspaceViewModel).DialogConductor.ActivateItem(new BankViewModel());
        }
    }
}
