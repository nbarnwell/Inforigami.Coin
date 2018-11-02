using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.CRUD
{
    public class BasicCrudWorkspace<TEntity, TListViewModel, TListItemViewModel, TEditViewModel, TReviewViewModel> 
        : Conductor<IScreen>.Collection.OneActive
        where TListViewModel : ListViewModel<TEntity, TListItemViewModel>, new() 
        where TListItemViewModel : ListItemViewModel<TEntity>, new() 
        where TEditViewModel : EditViewModel<TEntity>, new()
        where TReviewViewModel : ReviewViewModel<TEntity>, new()
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly TListViewModel _listViewModel;

        public BasicCrudWorkspace(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;

            _listViewModel = new TListViewModel();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();

            throw new NotImplementedException("TODO: Invoke entity-specific impl for loading items");
        }

        public void SelectItem(ListItemViewModel<TEntity> item)
        {
            ActivateItem(
                _viewModelFactory.Create<TReviewViewModel>()
                                 .For(item.Item));
        }
    }
}