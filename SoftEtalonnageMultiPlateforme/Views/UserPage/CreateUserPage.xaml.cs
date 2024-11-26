using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.UserPage;

public partial class CreateUserPage : ContentPage
{
	public CreateUserPage(UserServiceVM userServiceVM)
	{
		InitializeComponent();
        BindingContext = userServiceVM;
    }
}