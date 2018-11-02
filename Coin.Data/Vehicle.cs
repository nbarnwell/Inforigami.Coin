using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            VehicleMaintenanceLog = new HashSet<VehicleMaintenanceLog>();
            VehicleMileageLog = new HashSet<VehicleMileageLog>();
            VehicleRefuelLog = new HashSet<VehicleRefuelLog>();
        }

        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Registration { get; set; }

        public VehicleType VehicleType { get; set; }
        public ICollection<VehicleMaintenanceLog> VehicleMaintenanceLog { get; set; }
        public ICollection<VehicleMileageLog> VehicleMileageLog { get; set; }
        public ICollection<VehicleRefuelLog> VehicleRefuelLog { get; set; }
    }
}
