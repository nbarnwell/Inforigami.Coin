using Caliburn.Micro;

namespace Coin.Shared
{
    public abstract class WorkspaceViewModelBase : Conductor<IScreen>
    {
        public Conductor<IScreen> DialogConductor { get; private set; }

        public WorkspaceViewModelBase()
        {
            DialogConductor = new Conductor<IScreen>();
            DialogConductor.ConductWith(this);
        }

        protected override void OnActivate()
        {
            ActivateItem(GetDefaultViewModel());

            base.OnActivate();
        }

        protected abstract IScreen GetDefaultViewModel();
    }
}
