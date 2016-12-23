using System;
using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.Shared
{
    public abstract class WorkspaceViewModelBase<TDefaultViewModel> : Conductor<IScreen>
        where TDefaultViewModel : IScreen
    {
        private readonly IViewModelFactory _viewModelFactory;

        public Conductor<IScreen> DialogConductor { get; private set; }

        public WorkspaceViewModelBase(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;

            DialogConductor = new Conductor<IScreen>();
        }

        protected override void OnActivate()
        {
            ActivateItem(_viewModelFactory.Create<TDefaultViewModel>());

            base.OnActivate();
        }
    }
}
