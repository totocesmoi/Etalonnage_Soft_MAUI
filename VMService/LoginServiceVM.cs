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
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        public LoginServiceVM(Manager service, INavigationService navigationService)
        {
            _service = service;
            _navigationService = navigationService;
            LoginCommand = new AsyncRelayCommand(OnLoginAsync, CanLogin);
        }

        public IAsyncRelayCommand LoginCommand { get; }

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

        private bool CanLogin() => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password) && _service.CurrentUser == null;
    }
}
