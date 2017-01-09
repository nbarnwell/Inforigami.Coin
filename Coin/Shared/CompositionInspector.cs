using System;
using Caliburn.Micro;

namespace Coin.Shared
{
    public static class CompositionInspector
    {
        public static WorkspaceViewModel FindWorkspace(IChild viewModel)
        {
            do
            {
                if (viewModel is WorkspaceViewModel)
                {
                    return viewModel as WorkspaceViewModel;
                }
            } while ((viewModel = viewModel.Parent as IChild) != null);

            throw new InvalidOperationException("No workspace could be found.");
        }
    }
}
