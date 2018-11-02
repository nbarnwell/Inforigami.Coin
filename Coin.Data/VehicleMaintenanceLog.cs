using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class VehicleMaintenanceLog
    {
        public VehicleMaintenanceLog()
        {
            VehiclePartsReplacementLog = new HashSet<VehiclePartsReplacementLog>();
        }

        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTimeOffset MaintenanceDateTime { get; set; }
        public int Mileage { get; set; }

        public Vehicle Vehicle { get; set; }
        public ICollection<VehiclePartsReplacementLog> VehiclePartsReplacementLog { get; set; }
    }
}
