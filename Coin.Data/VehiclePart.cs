using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class VehiclePart
    {
        public VehiclePart()
        {
            VehiclePartsReplacementLog = new HashSet<VehiclePartsReplacementLog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int VehicleTypeId { get; set; }

        public VehicleType VehicleType { get; set; }
        public ICollection<VehiclePartsReplacementLog> VehiclePartsReplacementLog { get; set; }
    }
}
