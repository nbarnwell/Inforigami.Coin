using System;
using Coin.Data;
using Coin.Shared;
using Inforigami.Regalo.Core;

namespace Coin.CRUD.Vehicles
{
    public class CreateVehicleHandler : ICommandHandler<CreateVehicle>
    {
        private readonly IEventBus _eventBus;

        public CreateVehicleHandler(IEventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            _eventBus = eventBus;
        }

        public void Handle(CreateVehicle command)
        {
            var vehicle =
                new Vehicle
                {
                    Name = command.Name,
                    Make = command.Make,
                    Model = command.Model,
                    Registration = command.Registration,
                    VehicleTypeId = command.VehicleTypeId
                };

            using (var db = new Database())
            {
                db.Vehicles.Add(vehicle);

                db.SaveChanges();
            }

            _eventBus.Publish(new EntityCreated<Vehicle>(vehicle));
        }
    }
}