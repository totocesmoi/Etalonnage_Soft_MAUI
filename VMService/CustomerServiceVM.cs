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
            GetACustomer = new AsyncRelayCommand<OptionCommand<object>>(GetCustomerAsync!, CanGetCustomer!);
            CreateCustomer = new AsyncRelayCommand<string>(CreateCustomerAsync, CanCreateCustomer);
            InsertCustomer = new AsyncRelayCommand(InsertCustomerAsync, CanInsertCustomer);
            UpdateCustomer = new AsyncRelayCommand<CustomerVM>(UpdateCustomerAsync, CanUpdateCustomer);
            DeleteCustomer = new AsyncRelayCommand(DeleteCustomerAsync, CanDeleteCustomer);
        }

        public IAsyncRelayCommand<OptionCommand<object>> GetACustomer { get; private set; }
        private async Task GetCustomerAsync(OptionCommand<object> options)
        {
            try
            {
                var name = options[0] as string;
                var navigate = (bool)options[1];
                if (navigate)
                {
                    var pageName = options[2] as string;

                    if (name == null || pageName == null)
                        throw new ArgumentNullException("Missing required parameters");

                    var customer = await service.GetUserByName(name);
                    SelectedCustomer = new CustomerVM(customer);

                    await service.Navigation.NavigateToAsync(pageName);
                }
                else
                {
                    if (name == null)
                        throw new ArgumentNullException("Missing required parameters");

                    var customer = await service.GetUserByName(name);
                    SelectedCustomer = new CustomerVM(customer);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the user retrieval : {ex.Message}");
            }
        }

        private bool CanGetCustomer(OptionCommand<object> options)
        {
            var name = options[0] as string;
            var canExecute = !string.IsNullOrEmpty(name) && service.CurrentUser != null;
            Debug.WriteLine($"CanGetCustomer: {canExecute}");
            return canExecute;
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
            return service != null && !string.IsNullOrEmpty(page) && service.CurrentUser!.UserRole == Role.Administrator;
        }

        public IAsyncRelayCommand InsertCustomer { get; private set; }

        private async Task<CustomerVM> InsertCustomerAsync()
        {
            if (await service.CreateCustomer(selectedCustomer.CustomerModel))
            {
                // Pour être sur que le SelectedUser contient bien les informations auto généré.
                selectedCustomer = new CustomerVM(await service.GetUserByName(selectedCustomer.CustomerModel.Name));

                customers.Add(selectedCustomer);
                try
                {
                    await service.Navigation.GoBackAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Navigation failed: {ex.Message}");
                }
                return selectedCustomer;
            }
            else
                throw new Exception("An error occured during the Customer creation");
        }

        private bool CanInsertCustomer() => selectedCustomer != null && !string.IsNullOrEmpty(selectedCustomer.Name);

        // Gestion de la commande pour mettre à jour un client
        public IAsyncRelayCommand UpdateCustomer { get; private set; }

        /// <summary>
        /// Méthode qui permet de charger les clients après une mise à jour
        /// </summary>
        /// <returns></returns>
        private async Task LoadAfterUpdateAsync()
        {
            await GetCustomerAsync(currentPageIndex, pageSize);
        }
        /// <summary>
        /// Permet de mettre à jour un client
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"> Problème lors de la mise à jour du client </exception>
        private async Task UpdateCustomerAsync(CustomerVM customer)
        {
            try
            {
                var updatedCustomer = await service.UpdateCustomer(customer.CustomerModel);
                if (updatedCustomer != null)
                {
                    // Remplace l'objet dans la collection
                    var index = customers.IndexOf(customer);
                    if (index >= 0)
                    {
                        customers[index] = new CustomerVM(updatedCustomer);
                    }
                    // Recharge les données si nécessaire
                    await service.Navigation.GoBackAsync();
                }
                else
                {
                    throw new Exception("An error occurred during the Customer update");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during the customer update: {ex.Message}");
            }
        }




        /// <summary>
        /// Condition pour savoir si on peut mettre à jour un client
        /// </summary>
        /// <param name="user"> SelectedUser dans notre cas </param>
        /// <returns> bool </returns>
        private bool CanUpdateCustomer(CustomerVM customer) => customer != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;

        // Gestion de la commande pour supprimer un client
        public IAsyncRelayCommand DeleteCustomer { get; private set; }
        /// <summary>
        /// Permet de supprimer un client
        /// </summary>
        /// <returns> Vrai si l'client est supprimé sinon Faux </returns>
        /// <exception cref="Exception"> Problème lors de la suppression du client </exception>
        private async Task<bool> DeleteCustomerAsync()
        {
            try
            {
                if (await service.DeleteCustomer(SelectedCustomer.CustomerModel.Name))
                {
                    customers.Remove(SelectedCustomer);
                    SelectedCustomer = null!;
                    await service.Navigation.GoBackAsync();
                    return true;
                }

                await Application.Current.MainPage.DisplayAlert("Erreur", "An error occured during the customer deletion !", "OK");
                await service.Navigation.GoBackAsync();
                return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the customer deletion : {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// Condition pour savoir si on peut supprimer un client
        /// </summary>
        /// <returns> bool </returns>
        private bool CanDeleteCustomer() => SelectedCustomer != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;
    }
}

