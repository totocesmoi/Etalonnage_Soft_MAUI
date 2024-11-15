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

namespace VMService
{
    /// <summary>
    /// Permet de gérer l'ensemble des utilisateurs de l'application
    /// </summary>
    /// <remarks>
    /// Ici lorsque je vais faire une modification, ajout ou suppression d'un utilisateur, je vais passer par SelectedUser pour notifier la vue.
    /// Quand dans la view je vais cliquer sur un utilisateurs par exemple, les informations de l'utilisateur vont être copié dans SelectedUser.
    /// </remarks>
    public class UserServiceVM : ObservableObject
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
        public UserServiceVM(Manager manager)
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
            GetAnUser = new AsyncRelayCommand<string>(GetUserAsync, CanGetUser);
            GetCurrentUser = new AsyncRelayCommand(GetCurrentAsync, CanGetCurrentUser);
            CreateUser = new RelayCommand(CreateAnUserAsync, CanCreateUser);
            InsertUser = new AsyncRelayCommand(InsertAnUserAsync, CanInsertUser);
            UpdateUser = new AsyncRelayCommand<UserVM>(UpdateAnUserAsync, CanUpdateUser);
            DeleteUser = new AsyncRelayCommand(DeleteAnUserAsync, CanDeleteUser);
        }


        // Gestion de la commande chargement des utilisateurs
        public IAsyncRelayCommand LoadUsers { get; private set; }
        /// <summary>
        /// Permet de charger les utilisateurs en fonction de la pagination
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
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
        /// Méthode qui permet de charger les utilisateurs après une mise à jour
        /// </summary>
        /// <returns></returns>
        private async Task LoadEquipementAfterUpdateAsync()
        {
            await GetUsersAsync(currentPageIndex, pageSize);
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
        private async Task<UserVM> GetUserAsync(string login)
        {
            var user = await service.GetUserByLogin(login);
            SelectedUser = new UserVM(user);
            return SelectedUser;
        }

        /// <summary>
        /// Condition pour savoir si on peut récupérer un utilisateur
        /// </summary>
        /// <param name="login"></param>
        /// <returns> bool </returns>
        private bool CanGetUser(string login) => service != null && !string.IsNullOrEmpty(login) && service.CurrentUser != null;


        // Gestion de la commande pour récupérer l'utilisateur courant
        public IAsyncRelayCommand GetCurrentUser { get; private set; }
        /// <summary>
        /// Permet de récupérer l'utilisateur courant, c'est à dire l'utilisateur connecté
        /// </summary>
        /// <returns> L'utilisateur connecté </returns>
        public async Task<UserVM> GetCurrentAsync()
        {
            var user = await service.GetCurrentUser();
            SelectedUser = new UserVM(user);
            return SelectedUser;
        }

        /// <summary>
        /// Condition pour savoir si on peut récupérer l'utilisateur courant
        /// </summary>
        /// <returns> bool </returns>
        private bool CanGetCurrentUser() => service != null && service.CurrentUser != null;

        // Gestion de la commande pour créer un utilisateur
        public RelayCommand CreateUser { get; private set; }
        /// <summary>
        /// Méthode qui permet de créer un utilisateur
        /// </summary>
        /// <returns></returns>
        private void CreateAnUserAsync()
        {
            SelectedUser = new UserVM();
        }

        /// <summary>
        /// Condition pour savoir si on peut créer un utilisateur
        /// </summary>
        /// <returns> bool </returns>
        private bool CanCreateUser() => service != null;

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
                users.Add(SelectedUser);
                return SelectedUser;
            }
            else
                throw new Exception("An error occured during the User creation");
        }

        /// <summary>
        /// Condition pour savoir si on peut ajouter un utilisateur
        /// </summary>
        /// <returns> bool </returns>
        private bool CanInsertUser() => SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.Login) && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;

        // Gestion de la commande pour mettre à jour un utilisateur
        public IAsyncRelayCommand UpdateUser { get; private set; }
        /// <summary>
        /// Permet de mettre à jour un utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"> Problème lors de la mise à jour de l'utilisateur </exception>
        private async Task UpdateAnUserAsync(UserVM user)
        {
            if (await service.UpdateUser(user.UserModel, user.UserModel.Login) != null)
            {
                await LoadEquipementAfterUpdateAsync();
            }
            else
                throw new Exception("An error occured during the User update");
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
            if (await service.DeleteUser(SelectedUser.UserModel.Login))
            {
                users.Remove(SelectedUser);
                SelectedUser = null!;
                return true;
            }
            else
                throw new Exception("An error occured during the User deletion");
        }

        /// <summary>
        /// Condition pour savoir si on peut supprimer un utilisateur
        /// </summary>
        /// <returns> bool </returns>
        private bool CanDeleteUser() => SelectedUser != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;
    }
}
