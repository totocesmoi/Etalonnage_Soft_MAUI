﻿using System.Globalization;
using SoftEtalonnageMultiPlateforme.Resources.Theme;

namespace SoftEtalonnageMultiPlateforme
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void PreferenceLoading(object sender, EventArgs e)
        {
            if (Preferences.Default.ContainsKey("Theme"))
            {
                string? chosenThemeString = Preferences.Default.Get("Theme", "chosenTheme");

                ResourceDictionary chosenTheme = chosenThemeString switch
                {
                    "Dark" => new DarkTheme(),
                    "Light" => new LightTheme(),
                    "Dufournier" => new DufournierTheme(),
                    _ => new DufournierTheme()
                };

                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    foreach (var dico in mergedDictionaries.Where(d => d is IThemeManager).ToList())
                    {
                        mergedDictionaries.Remove(dico);
                    }
                    mergedDictionaries.Add(chosenTheme);
                }
            }

            if (Preferences.Default.ContainsKey("Language"))
            {
                string? chosenLanguage = Preferences.Default.Get("Language", "en");
                CultureInfo newCulture = new CultureInfo(chosenLanguage);
                CultureInfo.CurrentUICulture = newCulture;
                CultureInfo.CurrentCulture = newCulture;
            }
        }
    }
}