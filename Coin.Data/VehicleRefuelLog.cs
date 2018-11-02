using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class VehicleRefuelLog
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public decimal FuelLitres { get; set; }
        public int PencePerLitre { get; set; }
        public int Mileage { get; set; }

        public Vehicle Vehicle { get; set; }
    }
}
