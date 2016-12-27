using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Banking
{
    public class BankListViewModel : Conductor<PropertyChangedBase>, IHandle<RefreshRequested>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IEventAggregator _events;

        public BindableCollection<BankViewModel> Banks { get; }

        public override string DisplayName => "Banks";

        public BankListViewModel(IViewModelFactory viewModelFactory, IEventAggregator events)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            if (events == null) throw new ArgumentNullException(nameof(events));

            _viewModelFactory = viewModelFactory;
            _events = events;

            Banks = new BindableCollection<BankViewModel>();
            _events.Subscribe(this);
        }

        protected override void OnActivate()
        {
            Refresh();
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                _events.Unsubscribe(this);
            }
        }

        public IEnumerable<IResult> AddBank()
        {
            var bankViewModel = _viewModelFactory.Create<BankViewModel>();
            var showDialog = new ShowDialog(bankViewModel);
            yield return showDialog;

            if (showDialog.Result == true)
            {
                using (var db = new Database())
                {
                    db.Banks.Add(
                        new Data.Bank
                        {
                            Name = bankViewModel.BankName
                        });

                    db.SaveChanges();
                }
            }
        }

        public void Handle(RefreshRequested message)
        {
            Refresh();
        }

        private void Refresh()
        {
            Banks.Clear();

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
    }
}
