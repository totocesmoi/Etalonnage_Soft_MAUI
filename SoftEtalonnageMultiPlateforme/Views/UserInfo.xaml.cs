using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class UserInfo : ContentPage
{
	public UserInfo(CurrentUserServiceVM currentUserServiceVM)
	{
		InitializeComponent();
		BindingContext = currentUserServiceVM;
    }
}