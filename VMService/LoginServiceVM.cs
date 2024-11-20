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
        /// <summary>
        /// Permet de manager l'ensemble de mes commandes utilisables dans l'application
        /// </summary>
        private readonly Manager _service;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string login;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string password;

        public LoginServiceVM(Manager service)
        {
            _service = service;

            CreateCommands(); 
        }

        public void CreateCommands()
        {
            LoginCommand = new AsyncRelayCommand<string>(OnLoginAsync, CanLogin);
            LogoutCommand = new AsyncRelayCommand(OnLogoutAsync, CanLogout);
        }

        public IAsyncRelayCommand LoginCommand { get; private set; }
        private async Task OnLoginAsync(string page)
        {
            try
            {
                if (_service.Login(Login, Password))
                {
                    await _service.Navigation.NavigateToAsync("MainPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Login ou mot de passe incorrect !", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        private bool CanLogin(string page)
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password) 
                && _service.CurrentUser == null
                && page != null || page == String.Empty;
        }

        public IAsyncRelayCommand LogoutCommand { get; private set; }
        private async Task OnLogoutAsync()
        {
            try
            {
                _service.Logout();
                await _service.Navigation.NavigateToAsync("Login");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        private bool CanLogout()
        {
            return _service.CurrentUser != null;
        }
    }
}
