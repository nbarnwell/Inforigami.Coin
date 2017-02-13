using Caliburn.Micro;

namespace Coin.Shared
{
    public class WorkspaceViewModel : Conductor<IScreen>
    {
        private IScreen _viewModel;

        public IConductor DialogConductor { get; private set; }

        public override string DisplayName => _viewModel.DisplayName;

        public WorkspaceViewModel()
        {
            var conductor = new Collection.OneActive();
            conductor.ConductWith(this);
            DialogConductor = conductor;
        }

        public WorkspaceViewModel WithViewModel(IScreen screen)
        {
            _viewModel = screen;
            return this;
        }

        protected override void OnActivate()
        {
            ActivateItem(_viewModel);

            base.OnActivate();
        }
    }
}
