using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.UserPage;

public partial class UserUpdate : ContentPage
{
    public UserUpdate(UserServiceVM userServiceVM)
    {
        InitializeComponent();
        BindingContext = userServiceVM;
    }
}