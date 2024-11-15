using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class LoginPage : ContentPage
{
	private readonly LoginServiceVM _loginServiceVM;

    public LoginPage(LoginServiceVM loginServiceVM)
	{
		_loginServiceVM = loginServiceVM;
        InitializeComponent();

		BindingContext = this;
	}
}