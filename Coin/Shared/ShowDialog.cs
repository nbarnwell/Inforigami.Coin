using System;
using Caliburn.Micro;

namespace Coin.Shared
{
    public class ShowDialog : IResult
    {
        private readonly IScreen _viewModel;

        public event EventHandler<ResultCompletionEventArgs> Completed;

        public ShowDialog(IScreen viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            _viewModel = viewModel;
        }

        public void Execute(CoroutineExecutionContext context)
        {
            var dialogConductor = GetDialogConductor(context.Target);
            _viewModel.Deactivated += viewModelDeactivated;
            dialogConductor.ActivateItem(_viewModel);
        }

        private void viewModelDeactivated(object sender, DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                var completed = Completed;
                completed?.Invoke(this, new ResultCompletionEventArgs());
            }
        }

        private Conductor<IScreen> GetDialogConductor(object viewModel)
        {
            object current = viewModel;
            while (current is IChild)
            {
                var child = (IChild)current;
                var parent = child.Parent;
                var workspace = parent as WorkspaceViewModelBase;

                if (workspace != null)
                {
                    return workspace.DialogConductor;
                }

                current = child.Parent;
            }

            throw new InvalidOperationException("No dialog conductor could be found.");
        }
    }
}