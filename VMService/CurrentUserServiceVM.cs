using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using VMWrapper;

namespace VMService
{
    public class CurrentUserServiceVM : ObservableObject, IServiceVM
    {
        private readonly Manager service;

        private UserVM currentUser;
        public UserVM CurrentUser
        {
            get => currentUser;
            set => SetProperty(ref currentUser, value);
        }

#pragma warning disable CS8618
        public CurrentUserServiceVM(Manager manager)
#pragma warning restore CS8618
        {
            service = manager;
            CreateCommands();
        }

        public void CreateCommands()
        {
            GetCurrentUser = new AsyncRelayCommand(GetCurrentAsync, CanGetCurrentUser);
            UpdateCurrentUser = new AsyncRelayCommand<UserVM>(UpdateCurrentUserAsync!, CanUpdateCurrentUser!);
            SelectSignatureCommand = new AsyncRelayCommand(SelectSignatureAsync);
            ResetCurrentUser = new AsyncRelayCommand(ResetCurrentUserAsync);
        }

        // Gestion de la commande pour récupérer l'utilisateur courant
        public IAsyncRelayCommand GetCurrentUser { get; private set; }
        /// <summary>
        /// Permet de récupérer l'utilisateur courant, c'est à dire l'utilisateur connecté
        /// </summary>
        /// <returns> L'utilisateur connecté </returns>
        public async Task<UserVM> GetCurrentAsync()
        {
            var user = await service.GetCurrentUser();
            CurrentUser = new UserVM(user!);
            return CurrentUser;
        }

        /// <summary>
        /// Condition pour savoir si on peut récupérer l'utilisateur courant
        /// </summary>
        /// <returns> bool </returns>
        private bool CanGetCurrentUser() => service != null && service.CurrentUser != null;

        // Gestion de la commande pour modifier l'utilisateur courant
        public IAsyncRelayCommand UpdateCurrentUser { get; private set; }
        /// <summary>
        /// Méthode qui permet de mettre à jour l'utilisateur courant
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateCurrentUserAsync(UserVM user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var updatedUser = await service.UpdateUser(user.UserModel);
            if (updatedUser != null)
            {
                CurrentUser = new UserVM(updatedUser);
            }
        }

        /// <summary>
        /// Condition pour savoir si on peut mettre à jour l'utilisateur courant
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool CanUpdateCurrentUser(UserVM user)
        {
            return user != null && !string.IsNullOrEmpty(user.Login);
        }

        /// <summary>
        /// Commande pour sélectionner une image de signature
        /// </summary>
        public IAsyncRelayCommand SelectSignatureCommand { get; private set; }
        private async Task SelectSignatureAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select a signature image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    CurrentUser.Signature = new Picture(memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured during the selection of the signature : {ex.Message}");
            }
        }

        /// <summary>
        /// Méthode pour réinitialiser les données de l'utilisateur courant
        /// </summary>
        public IAsyncRelayCommand ResetCurrentUser { get; private set; }
        public async Task ResetCurrentUserAsync()
        {
            await Task.FromResult(CurrentUser = null!);
        }
    }
}
