using CommunityToolkit.Mvvm.ComponentModel;
using DAL.Stub;
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
            UserRole = userModel.UserRole;
            Signature = userModel.Signature;
            SignatureName = userModel.SignatureName;
            PlainPassword = string.Empty;
        }

        public UserVM()
        {
            UserModel = new User();
            Name = "";
            Surname = "";
            Login = "";
            Password = "";
            UserRole = Role.Operator;
            Signature = new Picture();
            SignatureName = "";
            PlainPassword = string.Empty;
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

        [ObservableProperty]
        private string plainPassword;

        partial void OnPlainPasswordChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                UserModel.SetPasswd(value);
            }
        }

        /// <summary>
        /// Synchrone les données de la VM avec le modèle
        /// Utiliser uniquement après un ajout, pour avoir les données générés par le modèle
        /// </summary>
        public void SyncWithModel()
        {
            Name = UserModel.Name;
            Surname = UserModel.Surname;
            // Pour le login plus précisément
            Login = UserModel.Login;
            Password = UserModel.Password;
            UserRole = UserModel.UserRole;
            Signature = UserModel.Signature;
            SignatureName = UserModel.SignatureName;
            PlainPassword = string.Empty;
        }

        /// <summary>
        /// Protocole d'égalité entre mes UserVM nécéssaire pour la notification de changement dans la views
        /// </summary>
        /// <param name="other"></param>
        /// <returns> true or false </returns>
        public bool Equals(UserVM? other)
        {
            return string.IsNullOrEmpty(other?.Login) ? false : other.Login.Equals(UserModel.Login);
        }

        public override string ToString()
        {
            return $"nom : {Name} \nprénom : {Surname} \nlogin : {Login}";
        }
    }
}
