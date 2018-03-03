using Caliburn.Micro;

namespace Coin.CRUD
{
    public abstract class ReviewViewModel<TEntity> : Screen
    {
        private TEntity _item;

        public ReviewViewModel<TEntity> For(TEntity item)
        {
            _item = item;
            return this;
        }
    }
}