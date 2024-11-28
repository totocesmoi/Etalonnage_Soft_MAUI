using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.UserPage;

public partial class UserCatalogue : ContentPage
{
	public UserCatalogue(UserServiceVM userServiceVM)
	{
		InitializeComponent();

		BindingContext = userServiceVM;
	}
}