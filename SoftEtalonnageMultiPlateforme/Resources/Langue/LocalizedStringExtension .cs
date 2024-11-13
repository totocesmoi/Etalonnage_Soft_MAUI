using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoftEtalonnageMultiPlateforme.Resources.Langue
{
    public class Localization
    {
        public string Culture
        {
            get => culture;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                culture = value;
                LocalizedStringExtension.Culture = culture;
            }
        }
        private string culture = "fr-FR";
    }


    public class LocalizedStringExtension : IMarkupExtension<string>
    {
        public string Key { get; set; }
        public static string Culture
        {
            get => culture;
            set
            {
                if (culture == value) return;
                culture = value;
                AppRes.Culture = new CultureInfo(Culture);
                CultureChanged?.Invoke(null, EventArgs.Empty);
            }
        }
        private static string culture = "fr-FR";

        public static AppResourcesVM AppRes { get; set; } = new AppResourcesVM();

        public static event EventHandler? CultureChanged;

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget? provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            CultureChanged += (src, args) =>
            {
                (provideValueTarget?.TargetObject as BindableObject)?.SetValue((provideValueTarget?.TargetProperty as BindableProperty), GetLocalizedValue());
            };
            return GetLocalizedValue();
        }

        private string GetLocalizedValue()
        {
            var value = typeof(AppRessources).GetProperty(Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as string;
            // Console.WriteLine($"Localized value for key '{Key}': {value}");
            return value ?? "";
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
        }
    }

}
