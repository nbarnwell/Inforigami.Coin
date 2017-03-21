using Inforigami.Regalo.Interfaces;

namespace Coin.CRUD.Vehicles
{
    public class UpdateVehicle : Command
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public string Registration { get; private set; }
        public int VehicleTypeId { get; private set; }

        public UpdateVehicle(int vehicleId, string name, string make, string model, string registration, int vehicleTypeId)
        {
            Id = vehicleId;
            Name = name;
            Make = make;
            Model = model;
            Registration = registration;
            VehicleTypeId = vehicleTypeId;
        }
    }
}