using System;
using Coin.Data;
using Coin.Shared;
using Inforigami.Regalo.Core;

namespace Coin.CRUD.Vehicles.MileageRecording
{
    public class RecordJourneyHandler : ICommandHandler<RecordJourney>
    {
        private readonly IEventBus _eventBus;

        public RecordJourneyHandler(IEventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            _eventBus = eventBus;
        }

        public void Handle(RecordJourney command)
        {
            var j = new VehicleMileageLog
            {
                VehicleId = command.VehicleId,
                TripDateTime = command.TripDateTime,
                StartMileage = command.StartMileage,
                EndMileage = command.EndMileage,
                VehicleTravelPurposeTypeId = (int)command.TravelPurposeType,
                Purpose = command.Purpose,
                From = command.From,
                To = command.To
            };

            using (var db = new Database())
            {
                db.VehicleMileageLogs.Add(j);
                db.SaveChanges();
            }

            _eventBus.Publish(EntityCreated.For(j));
        }
    }
}