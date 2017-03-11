using Caliburn.Micro;
using Coin.Data;

namespace Coin.CRUD.Vehicles
{
    public class VehicleListItemViewModel : PropertyChangedBase
    {
        private int _vehicleTypeId;
        private string _vehicleTypeName;
        private string _name;
        private string _make;
        private string _model;
        private string _registration;

        public int VehicleTypeId
        {
            get { return _vehicleTypeId; }
            set
            {
                if (value == _vehicleTypeId) return;
                _vehicleTypeId = value;
                NotifyOfPropertyChange(() => VehicleTypeId);
            }
        }

        public string VehicleTypeName
        {
            get { return _vehicleTypeName; }
            set
            {
                if (value == _vehicleTypeName) return;
                _vehicleTypeName = value;
                NotifyOfPropertyChange(() => VehicleTypeName);
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

        public static VehicleListItemViewModel CreateFrom(Vehicle vehicle, VehicleType vehicleType)
        {
            return
                new VehicleListItemViewModel
                {
                    Name = vehicle.Name,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    Registration = vehicle.Registration,
                    VehicleTypeId = vehicle.VehicleTypeId,
                    VehicleTypeName = vehicleType.Name
                };
        }
    }
}