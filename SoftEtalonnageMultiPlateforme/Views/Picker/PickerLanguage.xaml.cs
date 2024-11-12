using System.Globalization;
using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.Picker;

public partial class PickerLanguage : ContentView
{
    public static readonly BindableProperty LanguageVMProperty =
        BindableProperty.Create(nameof(LanguageVM), typeof(LanguageVM), typeof(PickerLanguage), null);

    public LanguageVM LanguageVM
    {
        get => (LanguageVM)GetValue(LanguageVMProperty);
        set => SetValue(LanguageVMProperty, value);
    }

    public PickerLanguage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public PickerLanguage(LanguageVM languageVM)
    {
        LanguageVM = languageVM;
        InitializeComponent();
        
        BindingContext = this;
    }
}