using System;
using Caliburn.Micro;
using Coin.Infrastructure;

namespace Coin.Shared
{
    public class ShowScreen : IResult
    {
        private readonly IScreen _viewModelToShow;

        public event EventHandler<ResultCompletionEventArgs> Completed;

        public ShowScreen(IScreen viewModelToShow)
        {
            if (viewModelToShow == null) throw new ArgumentNullException(nameof(viewModelToShow));

            _viewModelToShow = viewModelToShow;
        }

        public void Execute(CoroutineExecutionContext context)
        {
            var contextTarget = context.Target as IChild;

            if (contextTarget == null)
            {
                throw new InvalidOperationException("Unable to search for workspace when invoked from viewmodel that does not implement IChild");
            }

            var conductor = CompositionInspector.FindLocalConductor(contextTarget);
            conductor.ActivateItem(_viewModelToShow);

            Completed?.Invoke(this, new ResultCompletionEventArgs());
        }
    }
}