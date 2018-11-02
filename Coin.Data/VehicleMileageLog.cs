using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class VehicleMileageLog
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTimeOffset TripDateTime { get; set; }
        public int StartMileage { get; set; }
        public int EndMileage { get; set; }
        public int VehicleTravelPurposeTypeId { get; set; }
        public string Purpose { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public Vehicle Vehicle { get; set; }
        public VehicleTravelPurposeType VehicleTravelPurposeType { get; set; }
    }
}
