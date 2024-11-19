using VMService;

namespace SoftEtalonnageMultiPlateforme.Views
{
    public partial class PersonalInfo : ContentPage
    {
        private readonly UserServiceVM _userServiceVM;
        public PersonalInfo(UserServiceVM userServiceVM)
        {
            _userServiceVM = userServiceVM;
            InitializeComponent();
            BindingContext = _userServiceVM;
        }

    }
}


