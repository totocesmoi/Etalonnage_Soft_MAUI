using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class UserUpdate : ContentPage
{
    public UserUpdate(UserServiceVM userServiceVM)
    {
        InitializeComponent();
        BindingContext = userServiceVM;
    }
}