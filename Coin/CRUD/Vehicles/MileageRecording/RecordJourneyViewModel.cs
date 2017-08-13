using System;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;

namespace Coin.CRUD.Vehicles.MileageRecording
{
    public class RecordJourneyViewModel : Screen
    {
        private Vehicle _selectedVehicle;
        private DateTimeOffset _tripDateTime;
        private int _startMileage;
        private int _endMileage;
        private VehicleTravelPurposeType _selectedTravelPurposeType;
        private string _purpose;
        private string _from;
        private string _to;

        public BindableCollection<Vehicle> Vehicles { get; set; }

        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                if (Equals(value, _selectedVehicle)) return;
                _selectedVehicle = value;
                NotifyOfPropertyChange(() => SelectedVehicle);
            }
        }

        public DateTimeOffset TripDateTime
        {
            get { return _tripDateTime; }
            set
            {
                if (value.Equals(_tripDateTime)) return;
                _tripDateTime = value;
                NotifyOfPropertyChange(() => TripDateTime);
            }
        }

        public int StartMileage
        {
            get { return _startMileage; }
            set
            {
                if (value == _startMileage) return;
                _startMileage = value;
                NotifyOfPropertyChange(() => StartMileage);
            }
        }

        public int EndMileage
        {
            get { return _endMileage; }
            set
            {
                if (value == _endMileage) return;
                _endMileage = value;
                NotifyOfPropertyChange(() => EndMileage);
            }
        }

        public BindableCollection<VehicleTravelPurposeType> VehicleTravelPurposeTypes { get; set; }

        public VehicleTravelPurposeType SelectedTravelPurposeType
        {
            get { return _selectedTravelPurposeType; }
            set
            {
                if (Equals(value, _selectedTravelPurposeType)) return;
                _selectedTravelPurposeType = value;
                NotifyOfPropertyChange(() => SelectedTravelPurposeType);
            }
        }

        public string Purpose
        {
            get { return _purpose; }
            set
            {
                if (value == _purpose) return;
                _purpose = value;
                NotifyOfPropertyChange(() => Purpose);
            }
        }

        public string From
        {
            get { return _from; }
            set
            {
                if (value == _from) return;
                _from = value;
                NotifyOfPropertyChange(() => From);
            }
        }

        public string To
        {
            get { return _to; }
            set
            {
                if (value == _to) return;
                _to = value;
                NotifyOfPropertyChange(() => To);
            }
        }

        public RecordJourneyViewModel()
        {
            Vehicles = new BindableCollection<Vehicle>();
            VehicleTravelPurposeTypes = new BindableCollection<VehicleTravelPurposeType>();
        }

        protected override void OnActivate()
        {
            using (var db = new Database())
            {
                Vehicles.AddRange(db.Vehicles.ToList());
                VehicleTravelPurposeTypes.AddRange(db.VehicleTravelPurposeTypes.ToList());
            }
        }
    }
}