using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace VMWrapper
{
    public partial class UserVM : ObservableObject, IEquatable<UserVM>
    {
        private User userModel;
        public User UserModel 
        {
            get => userModel;
            set => SetProperty(ref userModel, value);
        }

        public UserVM(User userModel)
        {
            UserModel = userModel ?? throw new ArgumentNullException(nameof(userModel));
        }

        public UserVM()
        {
            UserModel = new User();
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string surname;

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private Role userRole;

        [ObservableProperty]
        private string signatureName;

        [ObservableProperty]
        private Picture signature;

        public bool Equals(UserVM? other)
        {
            return login.Equals(other.login);
        }
    }
}
