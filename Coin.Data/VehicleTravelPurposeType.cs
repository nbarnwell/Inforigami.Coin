using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class VehicleTravelPurposeType
    {
        public VehicleTravelPurposeType()
        {
            VehicleMileageLog = new HashSet<VehicleMileageLog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<VehicleMileageLog> VehicleMileageLog { get; set; }
    }
}
