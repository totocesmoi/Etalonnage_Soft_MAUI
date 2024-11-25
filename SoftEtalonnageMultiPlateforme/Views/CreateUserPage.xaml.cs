using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class CreateUserPage : ContentPage
{
	public CreateUserPage(UserServiceVM userServiceVM)
	{
		InitializeComponent();
        BindingContext = userServiceVM;
    }
}