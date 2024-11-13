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

namespace VMService
{
    public class UserServiceVM : ObservableObject
    {
        /// <summary>
        /// Permet de manager l'ensemble de mes commandes de mes Users.
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
        public ReadOnlyObservableCollection<UserVM> Users 
        {
            get => new ReadOnlyObservableCollection<UserVM>(users);
        }

        /// <summary>
        /// UserVm manipuler en cas de de modification, ajout ou suppression pour notifier la vue
        /// </summary>
        private UserVM selectedUser;
        public UserVM SelectedUser
        {
            get => selectedUser;
            set
            {
                SetProperty(ref selectedUser, value);
            }
        }

        // Ensemble de variable pour la pagination
        private int pageSize = 10; // Nombre d'éléments par page
        private int currentPageIndex = 0; // Index actuel de la page
        private bool hasMoreItems = true;

        /// <summary>
        /// Constructeur 
        /// </summary>
        /// <param name="service"></param>
        public UserServiceVM(IDataService<User> service)
        {
            this.service = new Manager(service);
            CreateCommands();
        }

        /// <summary>
        /// Me permet de rassemble tout les commandes disponible par mes UserVM
        /// </summary>
        public void CreateCommands()
        {
            LoadUsers = new RelayCommand(
                async () => await LoadMoreUsers(),
                () => service != null
            );

            GetAnUser = new RelayCommand<string>(
                async (login) => await GetUser(login),
                (login) => service != null && login != null && service.CurrentUser != null
            );

            GetCurrentUser = new RelayCommand(
                async () => await GetCurrent(),
                () => service != null
            );

            CreateUser = new RelayCommand(
                async () => await CreateAnUser(),
                () => service != null && service.CurrentUser != null
            );

            InsertUser = new RelayCommand(
                async () => await InsertAnUser(),
                () => service != null && SelectedUser != null && service.CurrentUser != null
            );

            UpdateUser = new RelayCommand<UserVM>(
                async (user) => await UpdateAnUser(user),
                (user) => service != null && user != null && service.CurrentUser != null
            );

            DeleteUser = new RelayCommand(
                async () => await DeleteAnUser(),
                () => service != null && SelectedUser != null && service.CurrentUser != null
            );
        }


        // Gestion de la commande chargement des utilisateurs
        public ICommand LoadUsers { get; private set; }
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

        private async Task LoadMoreUsers()
        {
            await GetUsersAsync(currentPageIndex, pageSize);
            currentPageIndex += pageSize;
        }

        private async Task LoadEquipementAfterUpdate()
        {
            await GetUsersAsync(currentPageIndex, pageSize);
        }

        // Gestion de la commande récupération d'un utilisateur
        public ICommand GetAnUser { get; private set; }
        private async Task<UserVM> GetUser(string login)
        {
            var user = await service.GetUserByLogin(login);
            SelectedUser = new UserVM(user);
            return SelectedUser;
        }

        // Gestion de la commande pour récupérer l'utilisateur courant
        public ICommand GetCurrentUser { get; private set; }
        public async Task<UserVM> GetCurrent()
        {
            var user = await service.GetCurrentUser();
            SelectedUser = new UserVM(user);
            return SelectedUser;
        }

        // Gestion de la commande pour créer un utilisateur
        public ICommand CreateUser { get; private set; }
        private Task CreateAnUser()
        {
            return Task.FromResult<UserVM>(SelectedUser = new UserVM());
        }

        public ICommand InsertUser { get; private set; }
        private async Task<UserVM> InsertAnUser()
        {
            if( await service.CreateUser(SelectedUser.UserModel))
            {
                users.Add(SelectedUser);
                return SelectedUser;
            }
            else 
                throw new Exception("An error occured during the User creation");
        }

        // Gestion de la commande pour mettre à jour un utilisateur
        public ICommand UpdateUser { get; private set; }
        private async Task UpdateAnUser(UserVM user)
        {
            if (await service.UpdateUser(user.UserModel, user.UserModel.Login) != null)
            {
                await LoadEquipementAfterUpdate();
            }
            else
                throw new Exception("An error occured during the User update");
        }

        // Gestion de la commande pour supprimer un utilisateur
        public ICommand DeleteUser { get; private set; }
        private async Task<bool> DeleteAnUser()
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
}
