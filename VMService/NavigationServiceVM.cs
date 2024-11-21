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
    public partial class NavigationServiceVM : IServiceVM
    {
        private readonly Manager service;

        public NavigationServiceVM(Manager service) 
        {
            this.service = service;
            CreateCommands();
        }

        public void CreateCommands()
        {
            NavigateTo = new AsyncRelayCommand<string>(NavigateToPage, CanNavigateTo);
            NavigateToBack = new AsyncRelayCommand(NavigateToBackAsync, CanNavigateToBack);
        }

        // Gestion de navigation d'une page à une autre
        public IAsyncRelayCommand NavigateTo { get; private set; }
        private async Task NavigateToPage(string page)
        {
            try
            {
                await service.Navigation.NavigateToAsync(page);
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Erreur", $"An error occured during navigation : {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Condition pour naviguer vers la page correspondante
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private bool CanNavigateTo(string page) => page != null || page != String.Empty;

        // Gestions de navigation vers la page principale
        public IAsyncRelayCommand NavigateToBack { get; private set; }
        private async Task NavigateToBackAsync()
        {
            try
            {
                await service.Navigation.GoBackAsync();
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Erreur", $"An error occured during navigation : {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Condition pour naviguer vers la page principale
        /// </summary>
        /// <returns> bool </returns>
        private bool CanNavigateToBack() => service != null;
    }
}
