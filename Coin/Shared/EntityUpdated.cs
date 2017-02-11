using Inforigami.Regalo.Interfaces;

namespace Coin.Shared
{
    public class EntityUpdated<TEntity> : Event
    {
        public TEntity Entity { get; private set; }

        public EntityUpdated(TEntity entity)
        {
            Entity = entity;
        }
    }
}