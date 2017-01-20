using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Coin.Infrastructure
{
    public static class ViewLocatorConfig
    {
        public static void ConfigureViewLocator()
        {
            ViewLocator.LocateForModelType = (modelType, displayLocation, context) =>
            {
                var modifiers = new[]
                {
                    new Regex("Model$", RegexOptions.Compiled),
                    new Regex("Screen$", RegexOptions.Compiled),
                    new Regex("Workspace$", RegexOptions.Compiled)
                };

                var viewTypeName = modelType.FullName;

                foreach (var regex in modifiers)
                {
                    viewTypeName = regex.Replace(viewTypeName, "");
                }

                if (context != null)
                {
                    viewTypeName = viewTypeName.Remove(viewTypeName.Length - 4, 4);
                    viewTypeName = viewTypeName + "." + context;
                }

                var viewType = (from assmebly in AssemblySource.Instance
                    from type in assmebly.GetExportedTypes()
                    where type.FullName.Equals(viewTypeName, StringComparison.InvariantCultureIgnoreCase)
                    select type).SingleOrDefault();

                return viewType == null
                    ? new TextBlock { Text = $"{viewTypeName} not found." }
                    : ViewLocator.GetOrCreateViewType(viewType);
            };


        }
    }
}