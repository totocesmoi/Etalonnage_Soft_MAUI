using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace VMWrapper
{
    public partial class UserVM : ObservableObject, IEquatable<UserVM>
    {
        [ObservableProperty]
        private User userModel;

        public UserVM(User userModel)
        {
            UserModel = userModel ?? throw new ArgumentNullException(nameof(userModel));

            Name = userModel.Name;
            Surname = userModel.Surname;
            Login = userModel.Login;
            Password = userModel.Password;
            userRole = userModel.UserRole;
            signature = userModel.Signature;
            signatureName = userModel.SignatureName;
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
        public override string ToString()
        {
            return $"nom : {Name} \nprénom : {Surname} \nlogin : {Login}";
        }
    }
}
