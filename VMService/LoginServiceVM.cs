using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Shared;
using System.Diagnostics;
using VMWrapper;

namespace VMService
{
    /// <summary>
    /// Permet de gérer ma logique de connexion à l'application
    /// </summary>
    public partial class LoginServiceVM : ObservableObject, IServiceVM
    {
        private readonly Manager _service;
        private readonly CurrentUserServiceVM _currentUserServiceVM;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string login;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string password;

        public LoginServiceVM(Manager service, CurrentUserServiceVM currentUserServiceVM)
        {
            _service = service;
            _currentUserServiceVM = currentUserServiceVM;

            CreateCommands();
        }

        public void CreateCommands()
        {
            LoginCommand = new AsyncRelayCommand<string>(OnLoginAsync, CanLogin);
            LogoutCommand = new AsyncRelayCommand(OnLogoutAsync, CanLogout);
            ResetLogin = new AsyncRelayCommand(ResetLoginAsync);
        }

        /// <summary>
        /// Commande pour se connecter
        /// </summary>
        public IAsyncRelayCommand LoginCommand { get; private set; }
        private async Task OnLoginAsync(string page)
        {
            try
            {
                if (_service.Login(Login, Password))
                {
                    await _currentUserServiceVM.GetCurrentAsync();
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("Erreur", "Login ou mot de passe incorrect !", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Condition pour savoir si on peut se connecter
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private bool CanLogin(string page)
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password)
                && _service.CurrentUser == null
                && page != null || page == String.Empty;
        }

        /// <summary>
        /// Commande pour se déconnecter
        /// </summary>
        public IAsyncRelayCommand LogoutCommand { get; private set; }
        private async Task OnLogoutAsync()
        {
            try
            {
                _service.Logout();
                await _currentUserServiceVM.ResetCurrentUserAsync();
                Shell.Current.FlyoutIsPresented = false;
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Condition pour savoir si on peut se déconnecter
        /// </summary>
        /// <returns></returns>
        private bool CanLogout()
        {
            return _service.CurrentUser != null;
        }

        /// <summary>
        /// Méthode pour réinitialiser les champs de connexion
        /// </summary>
        public IAsyncRelayCommand ResetLogin { get; private set; }
        private async Task ResetLoginAsync()
        {
            await Task.FromResult(Login = Password = string.Empty);
        }
    }
}
