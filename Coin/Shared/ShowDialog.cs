using System;
using Caliburn.Micro;

namespace Coin.Shared
{
    public class ShowDialog : IResult<bool?>
    {
        private readonly DialogViewModel _dialogViewModel;

        public bool? Result { get; private set; }

        public event EventHandler<ResultCompletionEventArgs> Completed;

        public ShowDialog(IScreen viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            _dialogViewModel = Dialog.WithContent(viewModel);
        }

        public void Execute(CoroutineExecutionContext context)
        {
            var dialogConductor = GetDialogConductor(context.Target);
            _dialogViewModel.Deactivated += DialogViewModelDeactivated;
            dialogConductor.ActivateItem(_dialogViewModel);
        }

        private void DialogViewModelDeactivated(object sender, DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                _dialogViewModel.Deactivated -= DialogViewModelDeactivated;
                var completed = Completed;
                var wasCancelled = _dialogViewModel.DialogResult == null;
                Result = !wasCancelled;

                completed?.Invoke(
                    this,
                    new ResultCompletionEventArgs
                    {
                        WasCancelled = wasCancelled
                    });
            }
        }

        private Conductor<IScreen> GetDialogConductor(object viewModel)
        {
            object current = viewModel;
            while (current is IChild)
            {
                var child = (IChild)current;
                var parent = child.Parent;
                var workspace = parent as WorkspaceViewModel;

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