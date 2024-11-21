using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class UserCatalogue : ContentPage
{
	public UserCatalogue(UserServiceVM userServiceVM)
	{
		InitializeComponent();

		BindingContext = userServiceVM;
	}
}