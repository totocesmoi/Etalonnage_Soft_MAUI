using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class UserUpdate : ContentPage
{
	public UserServiceVM UserServiceVM { get; private set; }
	public NavigationServiceVM NavigationServiceVM { get; private set; }
	public UserUpdate(UserServiceVM userServiceVM, NavigationServiceVM navigationServiceVM)
	{
		UserServiceVM = userServiceVM;
		NavigationServiceVM = navigationServiceVM;
		InitializeComponent();

		BindingContext = this;
	}
}