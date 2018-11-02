using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class VehiclePartsReplacementLog
    {
        public int Id { get; set; }
        public int VehicleMaintenanceLogId { get; set; }
        public int VehiclePartId { get; set; }

        public VehicleMaintenanceLog VehicleMaintenanceLog { get; set; }
        public VehiclePart VehiclePart { get; set; }
    }
}
