using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class VehicleType
    {
        public VehicleType()
        {
            Vehicle = new HashSet<Vehicle>();
            VehiclePart = new HashSet<VehiclePart>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Vehicle> Vehicle { get; set; }
        public ICollection<VehiclePart> VehiclePart { get; set; }
    }
}
