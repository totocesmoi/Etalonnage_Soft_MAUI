using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using Shared;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VMWrapper;

namespace VMService
{
    public partial class ContactServiceVM : ObservableObject, IServiceVM
    {
        private readonly Manager service;

        // Variables pour la pagination des contacts
        private int pageSize = 10;
        private int currentPageIndex = 0;
        private bool hasMoreItems = true;

        // Liste des contacts récupérés
        private ObservableCollection<ContactVM> contacts = new ObservableCollection<ContactVM>();
        public ReadOnlyObservableCollection<ContactVM> Contacts => new ReadOnlyObservableCollection<ContactVM>(contacts);

        // Contact sélectionné
        private ContactVM selectedContact;
        public ContactVM SelectedContact
        {
            get => selectedContact;
            set => SetProperty(ref selectedContact, value);
        }

        private readonly CustomerServiceVM customerService;

        // Commandes
        public IAsyncRelayCommand<string> LoadContacts { get; private set; }
        public IAsyncRelayCommand<OptionCommand<object>> GetAContact { get; private set; }
        public IAsyncRelayCommand<string> CreateContact { get; private set; }
        public IAsyncRelayCommand InsertContact { get; private set; }
        public IAsyncRelayCommand<ContactVM> UpdateContact { get; private set; }
        public IAsyncRelayCommand DeleteContact { get; private set; }

        // Constructeur du ViewModel
        public ContactServiceVM(Manager service, CustomerServiceVM customerServiceVM)
        {
            customerService = customerServiceVM;
            this.service = service;
            CreateCommands();
        }

        public void CreateCommands()
        {
            LoadContacts = new AsyncRelayCommand<string>(LoadContactsAsync, CanLoadContacts);
            CreateContact = new AsyncRelayCommand<string>(CreateContactAsync, CanCreateContact);
            InsertContact = new AsyncRelayCommand(InsertContactAsync, CanInsertContact);
            UpdateContact = new AsyncRelayCommand<ContactVM>(UpdateContactAsync, CanUpdateContact);
            DeleteContact = new AsyncRelayCommand(DeleteContactAsync, CanDeleteContact);
        }

        // Charger les contacts associés à un client
        private async Task LoadContactsAsync(string name)
        {
            await GetContactsAsync(currentPageIndex, pageSize, customerService.SelectedCustomer.CustomerModel.Name);
            currentPageIndex += pageSize;
        }

        private async Task<IEnumerable<ContactVM>> GetContactsAsync(int index, int count, string name)
        {
            Pagination<Model.Contacts> paginationResult = await service.GetAllContactByCustomer(index, count, name);
            if (paginationResult.Items.Count() <= Contacts.Count)
            {
                return Contacts;
            }

            foreach (var contact in paginationResult.Items)
            {
                contacts.Add(new ContactVM(contact));
            }

            return Contacts;
        }

        private bool CanLoadContacts(string name) => customerService.SelectedCustomer != null && service.CurrentUser != null && name != null;




        private async Task GetContactAsync(OptionCommand<object> options)
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

                    var contact= await service.GetContactByCustomer(name);
                    SelectedContact = new ContactVM(contact);

                    await service.Navigation.NavigateToAsync(pageName);
                }
                else
                {
                    if (name == null)
                        throw new ArgumentNullException("Missing required parameters");

                    var contact = await service.GetContactByCustomer(name);
                    SelectedContact = new ContactVM(contact);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the contact retrieval : {ex.Message}");
            }
        }
        

      
        private async Task CreateContactAsync(string page)
        {
            
            SelectedContact = new ContactVM();
            if (!string.IsNullOrEmpty(page))
            {
                await service.Navigation.NavigateToAsync(page);
            }
        }

        private bool CanCreateContact(string page)
        {
            return service != null && !string.IsNullOrEmpty(page) && customerService.SelectedCustomer != null;
        }

        private async Task<ContactVM> InsertContactAsync()
        {
            if (customerService.SelectedCustomer != null && SelectedContact != null)
            {
                customerService.SelectedCustomer.CustomerModel.Contacts.Add(SelectedContact.ContactModel);
                await service.UpdateCustomer(customerService.SelectedCustomer.CustomerModel);
                contacts.Add(SelectedContact);
                await service.Navigation.GoBackAsync();
                return SelectedContact;
            }
            throw new Exception("Erreur lors de la création du contact");
        }

        // Condition pour savoir si l'on peut insérer un contact
        private bool CanInsertContact() => SelectedContact != null && !string.IsNullOrEmpty(SelectedContact.NameContact);

        // Mettre à jour un contact
        private async Task UpdateContactAsync(ContactVM contact)
        {
            var updatedContact = await service.UpdateContact(contact.ContactModel);
            if (contact != null && customerService.SelectedCustomer != null)
            {
                await service.UpdateCustomer(customerService.SelectedCustomer.CustomerModel);
                var index = contacts.IndexOf(contact);
                if (index >= 0)
                {
                    contacts[index] = contact;
                }

                await service.Navigation.GoBackAsync();
            }
        }

        private bool CanUpdateContact(ContactVM contact) => contact != null && customerService.SelectedCustomer != null;

        private async Task DeleteContactAsync()
        {
            if (SelectedContact != null && customerService.SelectedCustomer != null)
            {
                customerService.SelectedCustomer.CustomerModel.Contacts.Remove(SelectedContact.ContactModel);
                contacts.Remove(SelectedContact);
                await service.UpdateCustomer(customerService.SelectedCustomer.CustomerModel);
                await service.Navigation.GoBackAsync();
            }
        }

        private bool CanDeleteContact() => SelectedContact != null && customerService.SelectedCustomer != null;
    }
}
