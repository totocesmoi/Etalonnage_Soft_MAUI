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
    public partial class LoginServiceVM : ObservableObject
    {
        /// <summary>
        /// Permet de manager l'ensemble de mes commandes utilisables dans l'application
        /// </summary>
        private readonly Manager _service;
        /// <summary>
        /// Permet d'accéder a mes propriétés de navigation
        /// </summary>
        private readonly INavigationService _navigationService;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string login;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string password;

        public LoginServiceVM(Manager service, INavigationService navigationService)
        {
            _service = service;
            _navigationService = navigationService;

            CreateCommands(); 
        }

        private void CreateCommands()
        {
            LoginCommand = new AsyncRelayCommand(OnLoginAsync, CanLogin);
            LogoutCommand = new AsyncRelayCommand(OnLogoutAsync, CanLogout);
        }

        public IAsyncRelayCommand LoginCommand { get; private set; }
        private async Task OnLoginAsync()
        {
            if (_service.Login(Login, Password))
            {
                await _navigationService.NavigateToMainPageAsync();
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Login or password incorrect !", "OK");
            }
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password) && _service.CurrentUser == null;
        }

        public IAsyncRelayCommand LogoutCommand { get; private set; }
        private async Task OnLogoutAsync()
        {
            _service.Logout();
            await _navigationService.NavigateToLoginAsync();
        }

        private bool CanLogout()
        {
            return _service.CurrentUser != null;
        }
    }
}
