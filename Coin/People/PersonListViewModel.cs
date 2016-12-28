using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.People
{
    public class PersonListViewModel : Screen, IHandle<RefreshRequested>, IHandle<EntityCreated<Data.Person>>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IEventAggregator _events;

        public BindableCollection<PersonViewModel> People { get; }

        public override string DisplayName => "People";

        public PersonListViewModel(IViewModelFactory viewModelFactory, IEventAggregator events)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            if (events == null) throw new ArgumentNullException(nameof(events));

            _viewModelFactory = viewModelFactory;
            _events = events;

            People = new BindableCollection<PersonViewModel>();
            _events.Subscribe(this);
        }

        protected override void OnActivate()
        {
            RefreshData();
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                _events.Unsubscribe(this);
            }
        }

        public IEnumerable<IResult> AddPerson()
        {
            var personViewModel = _viewModelFactory.Create<PersonViewModel>();
            var showDialog = new ShowDialog(personViewModel);
            yield return showDialog;

            if (showDialog.Result == true)
            {
                using (var db = new Database())
                {
                    var entity =
                        new Data.Person
                        {
                            Name = personViewModel.PersonName
                        };

                    db.People.Add(entity);

                    db.SaveChanges();

                    _events.PublishOnUIThread(new EntityCreated<Data.Person>(entity));
                }
            }
        }

        public void Handle(RefreshRequested message)
        {
            RefreshData();
        }

        public void Handle(EntityCreated<Data.Person> message)
        {
            var newItem = PersonViewModel.CreateFrom(message.Entity);

            People.InsertWhere(
                x =>
                    string.Compare(
                        x.PersonName,
                        newItem.PersonName,
                        StringComparison.InvariantCultureIgnoreCase) > 0,
                newItem);
        }

        public void RefreshData()
        {
            People.Clear();

            using (var db = new Database())
            {
                People.AddRange(
                    db.People
                      .OrderBy(x => x.Name)
                      .Select(PersonViewModel.CreateFrom));
            }
        }
    }
}