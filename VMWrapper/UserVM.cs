using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace VMWrapper
{
    public class UserVM : ObservableObject, IEquatable<UserVM>
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

        public string Name
        {
            get => UserModel.Name;
            set => SetProperty (Name, value, newValue => { UserModel.Name = newValue; });
        }

        public string Surname
        {
            get => UserModel.Surname;
            set => SetProperty(Surname, value, newValue => { UserModel.Surname = newValue; });
        }

        public string Login
        {
            get => UserModel.Login;
            set => SetProperty(Login, value, newValue => { UserModel.Login = newValue; });
        }

        public string Password
        {
            get => UserModel.Password;
            set => SetProperty(Password, value, newValue => { UserModel.Password = newValue; });
        }

        public Role UserRole
        {
            get => UserModel.UserRole;
            set => SetProperty(UserRole, value, newValue => { UserModel.UserRole = newValue; });
        }

        public string SignatureName
        {
            get => UserModel.SignatureName;
            set => SetProperty(SignatureName, value, newValue => { UserModel.SignatureName = newValue; });
        }

        public Picture Signature
        {
            get => UserModel.Signature;
            set => SetProperty(Signature, value, newValue => { UserModel.Signature = newValue; });
        }

        public bool Equals(UserVM? other)
        {
            return Login.Equals(other.Login);
        }
    }
}
