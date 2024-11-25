using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class LoginPage : ContentPage, IDisposable
{

    public LoginPage(LoginServiceVM loginServiceVM)
    {
        InitializeComponent();

        BindingContext = loginServiceVM;
    }

    public void Dispose()
    {
        Dispose();
    }
}
