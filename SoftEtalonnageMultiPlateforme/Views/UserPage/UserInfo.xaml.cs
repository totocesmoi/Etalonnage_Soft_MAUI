using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.UserPage;

public partial class UserInfo : ContentPage
{
	public UserInfo(CurrentUserServiceVM currentUserServiceVM)
	{
		InitializeComponent();
		BindingContext = currentUserServiceVM;
    }
}