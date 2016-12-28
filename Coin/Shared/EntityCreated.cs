namespace Coin.Shared
{
    public class EntityCreated<TEntity>
    {
        public TEntity Entity { get; private set; }

        public EntityCreated(TEntity entity)
        {
            Entity = entity;
        }
    }
}