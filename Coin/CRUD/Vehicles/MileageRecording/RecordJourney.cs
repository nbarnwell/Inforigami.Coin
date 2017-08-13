using System;

namespace Coin.CRUD.Vehicles.MileageRecording
{
    public class RecordJourney
    {
        public int VehicleId { get; set; }
        public DateTimeOffset TripDateTime { get; set; }
        public int StartMileage { get; set; }
        public int EndMileage { get; set; }
        public TravelPurposeType TravelPurposeType { get; set; }
        public string Purpose { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}