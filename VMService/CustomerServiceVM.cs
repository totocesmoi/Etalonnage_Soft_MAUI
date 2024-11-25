using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using Shared;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VMWrapper;

namespace VMService
{
    public partial class CustomerServiceVM : ObservableObject, IServiceVM
    {
        /// <summary>
        /// Permet de manager l'ensemble de mes commandes utilisables dans l'application
        /// </summary>
        private readonly Manager service;

        // Ensemble de variable pour la pagination
        private int pageSize = 10; // Nombre d'éléments par page
        private int currentPageIndex = 0; // Index actuel de la page
        private bool hasMoreItems = true;


        private ObservableCollection<CustomerVM> customers = new ObservableCollection<CustomerVM>();
        public ReadOnlyObservableCollection<CustomerVM> Customers => new ReadOnlyObservableCollection<CustomerVM>(customers);


        private CustomerVM selectedCustomer;
        public CustomerVM SelectedCustomer
        {
            get => selectedCustomer;
            set => SetProperty(ref selectedCustomer, value);
        }



        public CustomerServiceVM(Manager service)
        {
            this.service = service;

            CreateCommands();
        }

        public void CreateCommands()
        {
            LoadCustomer = new AsyncRelayCommand(LoadCustomerAsync, CanLoadCustomer);
            CreateCustomer = new AsyncRelayCommand<string>(CreateCustomerAsync, CanCreateCustomer);
            InsertCustomer = new AsyncRelayCommand(InsertCustomerAsync, CanInsertCustomer);
            UpdateCustomer = new AsyncRelayCommand<CustomerVM>(UpdateCustomerAsync, CanUpdateCustomer);
            DeleteCustomer = new AsyncRelayCommand(DeleteCustomerAsync, CanDeleteCustomer);
        }


        public IAsyncRelayCommand LoadCustomer { get; private set; }
        private async Task LoadCustomerAsync()
        {
            await GetCustomerAsync(currentPageIndex, pageSize);
            currentPageIndex += pageSize;
        }

        private async Task<IEnumerable<CustomerVM>> GetCustomerAsync(int index, int count)
        {
            Pagination<Customer> paginationResult = await service.GetAllCustomer(index, count);
            if (paginationResult.Items.Count() <= Customers.Count)
            {
                return Customers;
            }

            foreach (var customer in paginationResult.Items)
            {
                customers.Add(new CustomerVM(customer));
            }

            return Customers;
        }

        private bool CanLoadCustomer() => service != null && hasMoreItems && service.CurrentUser != null;
        public IAsyncRelayCommand<string> CreateCustomer { get; private set; }

        /// <summary>
        /// Méthode qui permet de créer un client
        /// </summary>
        /// <returns></returns>
        private async Task CreateCustomerAsync(string page)
        {
            SelectedCustomer = new CustomerVM();
            if (!string.IsNullOrEmpty(page))
            {
                await service.Navigation.NavigateToAsync(page);
            }
        }
        private bool CanCreateCustomer(string page)
        {
            return service != null && !string.IsNullOrEmpty(page) && service.CurrentUser.UserRole == Role.Administrator;
        }

        public IAsyncRelayCommand InsertCustomer { get; private set; }

        private async Task<CustomerVM> InsertCustomerAsync()
        {
            if (await service.CreateCustomer(selectedCustomer.CustomerModel))
            {
                // Pour être sur que le SelectedUser contient bien les informations auto généré.
                selectedCustomer = new CustomerVM(await service.GetUserByNme(selectedCustomer.CustomerModel.Name));

                customers.Add(selectedCustomer);
                await service.Navigation.GoBackAsync();
                return selectedCustomer;
            }
            else
                throw new Exception("An error occured during the User creation");
        }

        private bool CanInsertCustomer() => selectedCustomer != null && !string.IsNullOrEmpty(selectedCustomer.Name);

        // Gestion de la commande pour mettre à jour un utilisateur
        public IAsyncRelayCommand UpdateCustomer { get; private set; }

        /// <summary>
        /// Méthode qui permet de charger les utilisateurs après une mise à jour
        /// </summary>
        /// <returns></returns>
        private async Task LoadAfterUpdateAsync()
        {

            await GetCustomerAsync(currentPageIndex, pageSize);
        }
        /// <summary>
        /// Permet de mettre à jour un utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"> Problème lors de la mise à jour de l'utilisateur </exception>
        private async Task UpdateCustomerAsync(CustomerVM customer)
        {
            try
            {
                if (await service.UpdateCustomer(customer.CustomerModel) != null)
                {
                    await LoadAfterUpdateAsync();
                    await service.Navigation.GoBackAsync();
                }
                else
                    throw new Exception("An error occured during the User update");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the user update : {ex.Message}");
            }

        }

        /// <summary>
        /// Condition pour savoir si on peut mettre à jour un utilisateur
        /// </summary>
        /// <param name="user"> SelectedUser dans notre cas </param>
        /// <returns> bool </returns>
        private bool CanUpdateCustomer(CustomerVM customer) => customer != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;

        // Gestion de la commande pour supprimer un utilisateur
        public IAsyncRelayCommand DeleteCustomer { get; private set; }
        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <returns> Vrai si l'utilisateur est supprimé sinon Faux </returns>
        /// <exception cref="Exception"> Problème lors de la suppression de l'utilisateur </exception>
        private async Task<bool> DeleteCustomerAsync()
        {
            try
            {
                if (await service.DeleteUser(SelectedCustomer.CustomerModel.Name))
                {
                    customers.Remove(SelectedCustomer);
                    SelectedCustomer = null!;
                    await service.Navigation.GoBackAsync();
                    return true;
                }

                await Application.Current.MainPage.DisplayAlert("Erreur", "An error occured during the user deletion !", "OK");
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
        private bool CanDeleteCustomer() => SelectedCustomer != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;
    }

}

