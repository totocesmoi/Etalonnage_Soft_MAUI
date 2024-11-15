using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMService
{
    /// <summary>
    /// Permet de gérer ma logique de connexion à l'application
    /// </summary>
    public partial class LoginServiceVM : ObservableObject
    {
        /// <summary>
        /// Permet de manager l'ensemble de mes commandes utilisables dans l'application
        /// </summary>
        private Manager service;

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        public LoginServiceVM()
        {
            LoginCommand = new AsyncRelayCommand(OnLoginAsync, CanLogin);
        }

        public IAsyncRelayCommand LoginCommand { get; }

        private async Task OnLoginAsync()
        {
            // Remplacez cette logique par la validation réelle de l'utilisateur
            var user = ValidateUser(Login, Password);
            if (user != null)
            {
                // Naviguer vers la page principale après une connexion réussie
                //await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Nom d'utilisateur ou mot de passe incorrect", "OK");
            }
        }

        private bool CanLogin() => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password) && service.CurrentUser == null;

        private User ValidateUser(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
