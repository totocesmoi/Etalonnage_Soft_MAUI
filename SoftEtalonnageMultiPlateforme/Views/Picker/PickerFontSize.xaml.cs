namespace SoftEtalonnageMultiPlateforme.Views.Picker;

public partial class PickerFontSize : ContentView
{
	public PickerFontSize()
	{
		InitializeComponent();
	}

    private void pickerPolice_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedFontSize = pickerFontSize.SelectedItem.ToString();
        double fontSize = selectedFontSize switch
        {
            "Petit" => 12,
            "Moyen" => 16,
            "Grand" => 20,
            "Papi" => 24,
            "Mikael" => 28,
            _ => 16
        };

        double titleFontSize = fontSize switch
        {
            12 => 24,
            16 => 32,
            20 => 40,
            24 => 48,
            28 => 52,
            _ => 32
        };

        Application.Current.Resources["GlobalFontSize"] = fontSize;
        Application.Current.Resources["TitleFontSize"] = titleFontSize;
        Preferences.Set("GlobalFontSize", fontSize);
        Preferences.Set("TitleFontSize", titleFontSize);
    }
}