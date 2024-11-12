using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Globalization;

namespace VMService
{
    public partial class LanguageVM : ObservableObject
    {
        public List<string> AvailableLanguages { get; } = new List<string> { "en", "fr" };

        [ObservableProperty]
        private string selectedLanguage;

        partial void OnSelectedLanguageChanged(string value)
        {
            Preferences.Default.Set("Language", value);
            CultureInfo newCulture = new CultureInfo(value);
            CultureInfo.CurrentUICulture = newCulture;
            CultureInfo.CurrentCulture = newCulture;
        }
    }
}

