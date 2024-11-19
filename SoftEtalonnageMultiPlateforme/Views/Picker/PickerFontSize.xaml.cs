namespace SoftEtalonnageMultiPlateforme.Views.Picker;

public partial class PickerFontSize : ContentView
{
	public PickerFontSize()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public string SizeDefault { get; set; } = TextPickerFontSize();

    private static string TextPickerFontSize()
    {
        var fontSize = Application.Current.Resources["GlobalFontSize"];

        var size = (double)fontSize;
        return size switch
        {
            12 => "Small",
            16 => "Medium",
            20 => "Large",
            24 => "X-Large",
            28 => "Mikael",
            _ => "Moyen"
        };
    }

    private void pickerFontSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedFontSize = pickerFontSize.SelectedItem.ToString();
        double fontSize = selectedFontSize switch
        {
            "Small" => 12,
            "Medium" => 16,
            "Large" => 20,
            "X-Large" => 24,
            "Mikael" => 28,
            _ => 16
        };

        double titleFontSize = fontSize switch
        {
            12 => 24,
            16 => 32,
            20 => 40,
            24 => 44,
            28 => 48,
            _ => 32
        };

        Application.Current.Resources["GlobalFontSize"] = fontSize;
        Application.Current.Resources["TitleFontSize"] = titleFontSize;
        Preferences.Set("GlobalFontSize", fontSize);
        Preferences.Set("TitleFontSize", titleFontSize);
    }
}