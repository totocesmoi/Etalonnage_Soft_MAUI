using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEtalonnageMultiPlateforme.Converters
{
    public class CultureInfo2StringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            CultureInfo? cultureInfo = value as CultureInfo;

            if (cultureInfo == null) 
                return string.Empty;

            return cultureInfo.Name;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) 
                return CultureInfo.CurrentUICulture;

            string? chosenCulture = value as string;

            if (string.IsNullOrWhiteSpace(chosenCulture)) 
                return CultureInfo.CurrentUICulture;

            CultureInfo newCulture = new CultureInfo(chosenCulture);

            if (newCulture == null) 
                return CultureInfo.CurrentUICulture;

            return newCulture;
        }
    }
}
