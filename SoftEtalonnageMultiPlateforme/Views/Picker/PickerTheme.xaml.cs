using SoftEtalonnageMultiPlateforme.Resources.Theme;

namespace SoftEtalonnageMultiPlateforme.Views.Picker;

public partial class PickerTheme : ContentView
{
	public PickerTheme()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public string ThemeDefault { get; set; } = Preferences.Default.Get("Theme", "chosenTheme");

    void pickerColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string? chosenThemeString = pickerTheme.SelectedItem as string;


        ResourceDictionary chosenTheme = chosenThemeString switch
        {
            "Dufournier" => new DufournierTheme(),
            "Dark" => new DarkTheme(),
            "Light" => new LightTheme(),
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
        Preferences.Default.Set("Theme", chosenThemeString);
    }
}