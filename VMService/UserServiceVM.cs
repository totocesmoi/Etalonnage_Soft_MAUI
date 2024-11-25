using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMWrapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Shared;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Converters;
using System.Diagnostics;
using System.Xml.Linq;

namespace VMService
{
    /// <summary>
    /// Permet de gérer l'ensemble des utilisateurs de l'application
    /// </summary>
    /// <remarks>
    /// Ici lorsque je vais faire une modification, ajout ou suppression d'un utilisateur, je vais passer par SelectedUser pour notifier la vue.
    /// Quand dans la view je vais cliquer sur un utilisateurs par exemple, les informations de l'utilisateur vont être copié dans SelectedUser.
    /// </remarks>
    public class UserServiceVM : ObservableObject, IServiceVM
    {
        /// <summary>
        /// Permet de manager l'ensemble de mes commandes utilisables dans l'application
        /// Ceci ajoute une couche d'abstraction pour éviter une écriture d'accés trop longue.
        /// </summary>
        private Manager service;

        /// <summary>
        /// Permet d'avoir ma propriété observable qui va notifier le modèle en cas de 
        /// </summary>
        /// <remarks>
        /// Attention, il faut absolument que se soit une ObservableCollection pour que la vue soit notifié
        /// </remarks>
        private ObservableCollection<UserVM> users = new ObservableCollection<UserVM>();
        public ReadOnlyObservableCollection<UserVM> Users => new ReadOnlyObservableCollection<UserVM>(users);

        /// <summary>
        /// UserVm manipuler en cas de de modification, ajout ou suppression pour notifier la vue
        /// </summary>
        private UserVM selectedUser;
        public UserVM SelectedUser
        {
            get => selectedUser;
            set => SetProperty(ref selectedUser, value);
        }

        // Ensemble de variable pour la pagination
        private int pageSize = 10; // Nombre d'éléments par page
        private int currentPageIndex = 0; // Index actuel de la page
        private bool hasMoreItems = true;

        /// <summary>
        /// Constructeur 
        /// </summary>
        /// <param name="service"></param>
#pragma warning disable CS8618 
        public UserServiceVM(Manager manager)
#pragma warning restore CS8618
        {
            service = manager;
            CreateCommands();
            
        }

        /// <summary>
        /// Me permet de rassemble tout les commandes disponible par mes UserVM
        /// </summary>
        public void CreateCommands()
        {
            LoadUsers = new AsyncRelayCommand(LoadMoreUsersAsync, CanLoadMoreUsers);
            GetAnUser = new AsyncRelayCommand<OptionCommand<object>>(GetUserAsync!, CanGetUser!);
            CreateUser = new AsyncRelayCommand<string>(CreateAnUserAsync!, CanCreateUser!);
            InsertUser = new AsyncRelayCommand(InsertAnUserAsync, CanInsertUser);
            UpdateUser = new AsyncRelayCommand<UserVM>(UpdateAnUserAsync!, CanUpdateUser!);
            DeleteUser = new AsyncRelayCommand(DeleteAnUserAsync, CanDeleteUser);
        }


        // Gestion de la commande chargement des utilisateurs
        public IAsyncRelayCommand LoadUsers { get; private set; }
        /// <summary>
        /// Permet de charger les utilisateurs en fonction de la pagination
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// 
        /// <returns> Une partie ou l'entière liste des utilisateurs </returns>
        private async Task<IEnumerable<UserVM>> GetUsersAsync(int index, int count)
        {
            Pagination<User> paginationResult = await service.GetAllUser(index, count);
            if (paginationResult.Items.Count() <= Users.Count)
            {
                return Users;
            }

            foreach (var user in paginationResult.Items)
            {
                if (user.Login == service.CurrentUser!.Login)
                    continue;
                users.Add(new UserVM(user));
            }

            return Users;
        }

        /// <summary>
        /// Permet de savoir si on peut charger plus d'utilisateurs
        /// L'objectif ici est de ne pas chargé tout les utilisateurs d'un coup en cas de très grand nombre
        /// </summary>
        /// <returns></returns>
        private async Task LoadMoreUsersAsync()
        {
            await GetUsersAsync(currentPageIndex, pageSize);
            currentPageIndex += pageSize;
        }

        /// <summary>
        /// Condition pour savoir si on peut charger plus d'utilisateurs
        /// </summary>
        /// <returns> bool </returns>
        private bool CanLoadMoreUsers() => service != null && hasMoreItems && service.CurrentUser != null;


        // Gestion de la commande récupération d'un utilisateur
        public IAsyncRelayCommand GetAnUser { get; private set; }
        /// <summary>
        /// Méthode qui permet de récupérer un utilisateur en fonction de son login
        /// </summary>
        /// <param name="login"></param>
        /// <returns> L'utilisateur rechercher </returns>
        private async Task GetUserAsync(OptionCommand<object> options)
        {
            try 
            {
                var login = options[0] as string;
                var navigate = (bool)options[1];
                if (navigate)
                {
                    var pageName = options[2] as string;

                    if (login == null || pageName == null)
                        throw new ArgumentNullException("Missing required parameters");

                    var user = await service.GetUserByLogin(login);
                    SelectedUser = new UserVM(user);

                    await service.Navigation.NavigateToAsync(pageName);
                }
                else
                {
                    if (login == null)
                        throw new ArgumentNullException("Missing required parameters");

                    var user = await service.GetUserByLogin(login);
                    SelectedUser = new UserVM(user);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the user retrieval : {ex.Message}");
            }
        }

        /// <summary>
        /// Condition pour savoir si on peut récupérer un utilisateur
        /// </summary>
        /// <param name="login"></param>
        /// <returns> bool </returns>
        private bool CanGetUser(OptionCommand<object> options)
        {
            var login = options[0] as string;
            var canExecute = !string.IsNullOrEmpty(login) && service.CurrentUser != null;
            Debug.WriteLine($"CanGetUser: {canExecute}");
            return canExecute;
        }

        // Gestion de la commande pour créer un utilisateur
        public IAsyncRelayCommand<string> CreateUser { get; private set; }

        /// <summary>
        /// Méthode qui permet de créer un utilisateur
        /// </summary>
        /// <returns></returns>
        private async Task CreateAnUserAsync(string page)
        {
            SelectedUser = new UserVM();
            if (!string.IsNullOrEmpty(page))
            {
                await service.Navigation.NavigateToAsync(page);
            }
        }

        /// <summary>
        /// Condition pour savoir si on peut créer un utilisateur
        /// </summary>
        /// <returns> bool </returns>
        private bool CanCreateUser(string page)
        {
            return service != null && !string.IsNullOrEmpty(page) && service.CurrentUser!.UserRole == Role.Administrator;
        }


        public IAsyncRelayCommand InsertUser { get; private set; }
        /// <summary>
        /// Permet d'ajouter un utilisateur et de le notifier à la vue
        /// </summary>
        /// <returns> L'utilisateur ajouté </returns>
        /// <exception cref="Exception"> Problème dans l'ajout d'un user </exception>
        private async Task<UserVM> InsertAnUserAsync()
        {
            if (await service.CreateUser(SelectedUser.UserModel))
            {
                // Pour être sur que le SelectedUser contient bien les informations auto généré.
                SelectedUser.SyncWithModel();

                users.Add(SelectedUser);
                await service.Navigation.GoBackAsync();
                return SelectedUser;
            }
            else
                throw new Exception("An error occured during the User creation");
        }

        /// <summary>
        /// Condition pour savoir si on peut ajouter un utilisateur
        /// </summary>
        /// <returns> bool </returns>
        private bool CanInsertUser() => SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.Name) && !string.IsNullOrEmpty(SelectedUser.Surname) && !string.IsNullOrEmpty(SelectedUser.Password) ;

        // Gestion de la commande pour mettre à jour un utilisateur
        public IAsyncRelayCommand UpdateUser { get; private set; }
        private async Task UpdateAnUserAsync(UserVM user)
        {
            try
            {
                var updatedUser = await service.UpdateUser(user.UserModel);
                if (updatedUser != null)
                {
                    // Remplace l'objet dans la collection
                    var index = users.IndexOf(user);
                    if (index >= 0)
                    {
                        users[index] = new UserVM(updatedUser);
                    }

                    // Recharge les données si nécessaire
                    await service.Navigation.GoBackAsync();
                }
                else
                {
                    throw new Exception("An error occurred during the User update");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during the user update: {ex.Message}");
            }
        }

        /// <summary>
        /// Condition pour savoir si on peut mettre à jour un utilisateur
        /// </summary>
        /// <param name="user"> SelectedUser dans notre cas </param>
        /// <returns> bool </returns>
        private bool CanUpdateUser(UserVM user) => user != null && !string.IsNullOrEmpty(user.Login) && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;

        // Gestion de la commande pour supprimer un utilisateur
        public IAsyncRelayCommand DeleteUser { get; private set; }
        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <returns> Vrai si l'utilisateur est supprimé sinon Faux </returns>
        /// <exception cref="Exception"> Problème lors de la suppression de l'utilisateur </exception>
        private async Task<bool> DeleteAnUserAsync()
        {
            try
            {
                if (await service.DeleteUser(SelectedUser.UserModel.Login))
                {
                    users.Remove(SelectedUser);
                    SelectedUser = null!;
                    await service.Navigation.GoBackAsync();
                    return true;
                }

                await Application.Current!.MainPage!.DisplayAlert("Erreur", "An error occured during the user deletion !", "OK");
                await service.Navigation.GoBackAsync();
                return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the user deletion : {ex.Message}");
                return false;
            }
                
        }

        /// <summary>
        /// Condition pour savoir si on peut supprimer un utilisateur
        /// </summary>
        /// <returns> bool </returns>
        private bool CanDeleteUser() => SelectedUser != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;
    }
}
