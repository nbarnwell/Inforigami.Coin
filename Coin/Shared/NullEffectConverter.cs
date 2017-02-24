using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace Coin.Shared
{
    public class NullEffectConverter : IValueConverter
    {
        public Effect EffectIfNull { get; set; }
        public Effect EffectIfNotNull { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return EffectIfNull;
            }
            else
            {
                return EffectIfNotNull;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}