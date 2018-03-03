﻿using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace Coin.CRUD
{
    public class ListViewModel<TEntity, TListItemViewModel> : PropertyChangedBase
        where TListItemViewModel : ListItemViewModel<TEntity>, new() 
    {
        public BindableCollection<TEntity> Items { get; private set; }

        public ListViewModel()
        {
            Items = new BindableCollection<TEntity>();
        }

        internal void UpdateFrom(IEnumerable<TEntity> items)
        {
            var list = new List<Tuple<TEntity, TListItemViewModel>>();
            fore
        }
    }
}