using CommunityToolkit.Mvvm.Input;
using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMService
{
    public partial class NavigationServiceVM
    {
        private Manager service;
        private readonly INavigationService _navigationService;

        public NavigationServiceVM(INavigationService navigationService, Manager service) 
        {
            _navigationService = navigationService;
            this.service = service;
            CreateCommands();
        }

        public void CreateCommands()
        {
            NavigateToLoginCommand = new AsyncRelayCommand(NavigateToLoginAsync, CanNavigateToLogin);
            NavigateToMainPageCommand = new AsyncRelayCommand(NavigateToMainPageAsync, CanNavigateToMainPage);
        }

        // Gestion de navigation vers la page de connexion
        public IAsyncRelayCommand NavigateToLoginCommand { get; private set; }

        private async Task NavigateToLoginAsync()
        {
            await _navigationService.NavigateToLoginAsync();
        }

        /// <summary>
        /// Condition pour naviguer vers la page de connexion
        /// </summary>
        /// <returns> bool </returns>
        private bool CanNavigateToLogin() => service != null && service.CurrentUser == null;

        // Gestions de navigation vers la page principale
        public IAsyncRelayCommand NavigateToMainPageCommand { get; private set; }
        private async Task NavigateToMainPageAsync()
        {
            await _navigationService.NavigateToMainPageAsync();
        }

        /// <summary>
        /// Condition pour naviguer vers la page principale
        /// </summary>
        /// <returns> bool </returns>
        private bool CanNavigateToMainPage() => service != null && service.CurrentUser != null;
    }
}
