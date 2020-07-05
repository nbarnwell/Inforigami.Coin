using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Coin.Web.ScaffoldingHelpers
{
    public static class TextHelper
    {
        public static HtmlString Pluralise(this string input)
        {
            var replacements = new Dictionary<string, string>()
            {
                { "Person", "People"},
                { "Status$", "Statuses" },
                { "is$", "es" },
                { "ion$", "ions" },
                { "on$", "a" },
                { "us$", "uses" },
                { "o$", "oes" },
                { "y$", "ies" },
                { "(s|ss|sh|ch|x|z)$", "es" },
                { ".+", "$`s" }
            };

            foreach (var item in replacements)
            {
                string result;
                if ((result = Regex.Replace(input, item.Key, item.Value)) != input)
                {
                    return new HtmlString(result);
                }
            }

            return new HtmlString(input);
        }
    }
}
