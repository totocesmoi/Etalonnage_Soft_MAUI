using VMService;

namespace SoftEtalonnageMultiPlateforme.Views
{
    public partial class UserSection : ContentPage
    {
        private readonly UserServiceVM _userServiceVM;

        // Constructeur avec injection de d�pendance
        public UserSection(UserServiceVM userServiceVM)
        {
            _userServiceVM = userServiceVM;
            InitializeComponent();
            BindingContext = _userServiceVM;
        }
    }
}
