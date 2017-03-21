using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Shared;

namespace Coin.CRUD.Vehicles
{
    public class VehicleEditScreen : Screen
    {
        private ListItemViewModel _selectedVehicleType;
        private int _id;
        private string _name;
        private string _make;
        private string _model;
        private string _registration;

        public BindableCollection<ListItemViewModel> VehicleTypes { get; private set; }

        public VehicleEditScreen()
        {
            VehicleTypes = new BindableCollection<ListItemViewModel>();
        }

        protected override void OnActivate()
        {
            using (var db = new Database())
            {
                VehicleTypes.AddRange(
                    db.VehicleTypes.Select(
                        x => new ListItemViewModel
                        {
                            Id = x.Id,
                            Name = x.Name
                        }));

                if (Id != default(int))
                {
                    var v = db.Vehicles.Find(Id);

                    Id = v.Id;
                    Name = v.Name;
                    Make = v.Make;
                    Model = v.Model;
                    Registration = v.Registration;

                    SelectedVehicleType =
                        VehicleTypes.SingleOrDefault(x => x.Id == v.VehicleTypeId);
                }
            }
        }

        public ListItemViewModel SelectedVehicleType
        {
            get { return _selectedVehicleType; }
            set
            {
                if (Equals(value, _selectedVehicleType)) return;
                _selectedVehicleType = value;
                NotifyOfPropertyChange(() => SelectedVehicleType);
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Make
        {
            get { return _make; }
            set
            {
                if (value == _make) return;
                _make = value;
                NotifyOfPropertyChange(() => Make);
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (value == _model) return;
                _model = value;
                NotifyOfPropertyChange(() => Model);
            }
        }

        public string Registration
        {
            get { return _registration; }
            set
            {
                if (value == _registration) return;
                _registration = value;
                NotifyOfPropertyChange(() => Registration);
            }
        }

        public VehicleEditScreen For(int vehicleId)
        {
            Id = vehicleId;

            return this;
        }
    }
}