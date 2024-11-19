using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class LoginPage : ContentPage
{

    public LoginPage(LoginServiceVM loginServiceVM)
    {
        InitializeComponent();

        BindingContext = loginServiceVM;
    }
}
