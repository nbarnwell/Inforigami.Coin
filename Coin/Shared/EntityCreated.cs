using Inforigami.Regalo.Interfaces;

namespace Coin.Shared
{
    public class EntityCreated<TEntity> : Event
    {
        public TEntity Entity { get; private set; }

        public EntityCreated(TEntity entity)
        {
            Entity = entity;
        }
    }

    public static class EntityCreated
    {
        public static EntityCreated<T> For<T>(T entity)
        {
            return new EntityCreated<T>(entity);
        }
    }
}