using System.Collections.Generic;
using System;
using Caliburn.Micro;

namespace Coin.CRUD
{
    public abstract class ListItemViewModel<TEntity> : PropertyChangedBase
    {
        public string Title { get; set; }
        public IEnumerable<string> SubTitleItems { get; set; }
        public Func<IResult> Action { get; set; }
        public TEntity Item { get; set; }

        public ListItemViewModel<TEntity> For(TEntity item)
        {
            Title = GetTitle(item);
            SubTitleItems = GetSubTitleItems(item);
            Item = item;

            return this;
        }

        protected abstract IEnumerable<string> GetSubTitleItems(TEntity item);

        protected abstract string GetTitle(TEntity item);
    }
}