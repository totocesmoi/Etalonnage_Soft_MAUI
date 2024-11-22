using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace VMWrapper
{
    public partial class UserVM : ObservableObject
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
            UserRole = userModel.UserRole;
            Signature = userModel.Signature;
            SignatureName = userModel.SignatureName;
        }

        [ObservableProperty]
        private string name;

        partial void OnNameChanged(string value)
        {
            UserModel.Name = value;
        }

        [ObservableProperty]
        private string surname;

        partial void OnSurnameChanged(string value)
        {
            UserModel.Surname = value;
        }

        [ObservableProperty]
        private string login;

        partial void OnLoginChanged(string value)
        {
            UserModel.Login = value;
        }

        [ObservableProperty]
        private string password;

        partial void OnPasswordChanged(string value)
        {
            UserModel.Password = value;
        }

        [ObservableProperty]
        private Role userRole;

        partial void OnUserRoleChanged(Role value)
        {
            UserModel.UserRole = value;
        }

        [ObservableProperty]
        private string signatureName;

        partial void OnSignatureNameChanged(string value)
        {
            UserModel.SignatureName = value;
        }

        [ObservableProperty]
        private Picture signature;

        partial void OnSignatureChanged(Picture value)
        {
            UserModel.Signature = value;
        }

<<<<<<< HEAD
        public bool Equals(UserVM? other)
        {
            return string.IsNullOrEmpty(other?.Login) ? false : UserModel.Login!.Equals(other.Login);
        }
=======
        
>>>>>>> 443b54e6cce8b999bf7111548510e1a1f1f874a0

        public override string ToString()
        {
            return $"nom : {Name} \nprénom : {Surname} \nlogin : {Login}";
        }
    }
}
