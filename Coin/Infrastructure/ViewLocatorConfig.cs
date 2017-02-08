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
                    new { Regex = new Regex("ViewModel$", RegexOptions.Compiled), Replacement = "" },
                    new { Regex = new Regex("Screen$", RegexOptions.Compiled), Replacement = "" },
                    new { Regex = new Regex("Workspace$", RegexOptions.Compiled), Replacement = "Workspace" }
                };

                var viewModelTypeName = modelType.FullName;
                string viewTypeName = "";

                foreach (var modifier in modifiers)
                {
                    var match = modifier.Regex.Match(viewModelTypeName);

                    if (match.Success)
                    {
                        viewTypeName = modifier.Regex.Replace(viewModelTypeName, modifier.Replacement);

                        if (context != null)
                        {
                            viewTypeName += "." + context;
                        }
                        else
                        {
                            viewTypeName += "View";
                        }
                    }
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