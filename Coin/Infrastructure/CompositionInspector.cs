using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Coin.Shared;

namespace Coin.Infrastructure
{
    public static class CompositionInspector
    {
        public static WorkspaceViewModel FindTopLevelWorkspace(object viewModel)
        {
            var workspace = GetViewModelHierarchy(viewModel).FirstOrDefault(x => x is WorkspaceViewModel);

            if (workspace == null)
            {
                throw new InvalidOperationException("No workspace could be found.");
            }

            return (WorkspaceViewModel)workspace;
        }

        public static IConductor FindLocalConductor(object viewModel)
        {
            var conductor = GetViewModelHierarchy(viewModel).FirstOrDefault(x => x is IConductor);

            if (conductor == null)
            {
                throw new InvalidOperationException("No conductor could be found.");
            }

            return (IConductor)conductor;
        }

        public static IEnumerable<object> GetViewModelHierarchy(object viewModel)
        {
            do
            {
                yield return viewModel;
            } while ((viewModel = (viewModel as IChild)?.Parent) != null);
        }
    }
}
