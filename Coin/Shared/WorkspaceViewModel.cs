using Caliburn.Micro;

namespace Coin.Shared
{
    public class WorkspaceViewModel : Conductor<IScreen>
    {
        private IScreen _viewModel;

        public Conductor<IScreen> DialogConductor { get; private set; }

        public override string DisplayName => _viewModel.DisplayName;

        public WorkspaceViewModel()
        {
            DialogConductor = new Conductor<IScreen>();
            DialogConductor.ConductWith(this);
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
