using System;
using Coin.Data;
using Coin.Shared;
using Inforigami.Regalo.Core;

namespace Coin.CRUD.Vehicles
{
    public class UpdateVehicleHandler : ICommandHandler<UpdateVehicle>
    {
        private readonly IEventBus _eventBus;

        public UpdateVehicleHandler(IEventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            _eventBus = eventBus;
        }

        public void Handle(UpdateVehicle command)
        {
            Vehicle v;
            using (var db = new Database())
            {
                v = db.Vehicles.Find(command.Id);

                v.Name = command.Name;
                v.Make = command.Make;
                v.Model = command.Model;
                v.Registration = command.Registration;
                v.VehicleTypeId = command.VehicleTypeId;

                db.SaveChanges();
            }

            _eventBus.Publish(new EntityUpdated<Vehicle>(v));
        }
    }
}