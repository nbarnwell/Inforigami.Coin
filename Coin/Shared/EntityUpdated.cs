namespace Coin.Shared
{
    public class EntityUpdated<TEntity>
    {
        public TEntity Entity { get; private set; }

        public EntityUpdated(TEntity entity)
        {
            Entity = entity;
        }
    }
}