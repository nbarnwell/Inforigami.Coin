using System;
using Caliburn.Micro;

namespace Coin.Shared
{
    public class ShowViewModel : IResult
    {
        private readonly IChild _sender;
        private readonly IScreen _viewModelToShow;

        public event EventHandler<ResultCompletionEventArgs> Completed;

        public ShowViewModel(IChild sender, IScreen viewModelToShow)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (viewModelToShow == null) throw new ArgumentNullException(nameof(viewModelToShow));

            _sender = sender;
            _viewModelToShow = viewModelToShow;
        }

        public void Execute(CoroutineExecutionContext context)
        {
            var workspace = CompositionInspector.FindWorkspace(_sender);
            workspace.ActivateItem(_viewModelToShow);

            Completed?.Invoke(this, new ResultCompletionEventArgs());
        }
    }
}