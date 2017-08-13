using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;
using Inforigami.Regalo.Core;

namespace Coin.CRUD.Vehicles
{
    public class VehicleListScreen : Screen, IHandle<RefreshRequested>, IHandle<EntityCreated<Vehicle>>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ICommandProcessor _commandProcessor;
        private readonly IEventAggregator _eventAggregator;

        public BindableCollection<VehicleListItemViewModel> Vehicles { get; private set; }
        public override string DisplayName => "Vehicles";

        public VehicleListScreen(IViewModelFactory viewModelFactory, ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            if (commandProcessor == null) throw new ArgumentNullException(nameof(commandProcessor));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));

            _viewModelFactory = viewModelFactory;
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            Vehicles = new BindableCollection<VehicleListItemViewModel>();
        }

        protected override void OnInitialize()
        {
            _eventAggregator.Subscribe(this);

            RefreshData();
        }

        public override void TryClose(bool? dialogResult = null)
        {
            _eventAggregator.Unsubscribe(this);
        }

        public IEnumerable<IResult> Edit(VehicleListItemViewModel vehicle)
        {
            var vm = _viewModelFactory.Create<VehicleEditScreen>().For(vehicle.Id);
            var showDialog = new ShowDialog(vm);
            yield return showDialog;

            if (showDialog.Result == true)
            {
                _commandProcessor.Process(
                    new UpdateVehicle(
                        vm.Id,
                        vm.Name,
                        vm.Make,
                        vm.Model,
                        vm.Registration,
                        vm.SelectedVehicleType.Id));
            }
        }



        public void Handle(RefreshRequested message)
        {
            RefreshData();
        }

        public void Handle(EntityCreated<Vehicle> message)
        {
            using (var db = new Database())
            {
                var vehicleType = db.VehicleTypes.Single(x => x.Id == message.Entity.VehicleTypeId);
                var vm = VehicleListItemViewModel.CreateFrom(message.Entity, vehicleType);

                Vehicles.InsertWhere(
                    x => string.Compare(x.Registration, vm.Registration, StringComparison.InvariantCultureIgnoreCase) > 0,
                    vm);
            }
        }

        public IEnumerable<IResult> AddVehicle()
        {
            var vm = _viewModelFactory.Create<VehicleEditScreen>();
            var dialog = new ShowDialog(vm);
            yield return dialog;

            if (dialog.Result == true)
            {
                _commandProcessor.Process(new CreateVehicle(vm.Name, vm.Make, vm.Model, vm.Registration, vm.SelectedVehicleType.Id));
            }
        }

        private void RefreshData()
        {
            Vehicles.Clear();
            
            using (var db = new Database())
            {
                db.Vehicles
                  .OrderBy(x => x.Registration)
                  .ThenBy(x => x.Name)
                  .Select(x => new { v = x, vt = x.VehicleType })
                  .ToList()
                  .ForEach(x => Vehicles.Add(VehicleListItemViewModel.CreateFrom(x.v, x.vt)));
            }
        }
    }
}