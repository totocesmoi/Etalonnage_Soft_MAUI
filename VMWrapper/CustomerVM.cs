using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using System.Reflection.PortableExecutable;


namespace VMWrapper
{
    public partial class CustomerVM : ObservableObject
    {
        [ObservableProperty]
        private Customer customerModel;

        
        public CustomerVM(Customer customerModel)
        {
            customerModel = customerModel ?? throw new ArgumentNullException(nameof(customerModel));

            Name = customerModel.Name;
            Address = customerModel.Address;
            PhNum = customerModel.PhoneNumber;
            Contact = customerModel.Contact;
        }

        public CustomerVM()
        {
            customerModel = new Customer();
        }

        [ObservableProperty]
        private string name;

        partial void OnNameChanged(string value)
        {
            customerModel.Name = value;
        }

        [ObservableProperty]
        private string address;

        partial void OnAddressChanged(string value)
        {
            customerModel.Address = value;
        }

        [ObservableProperty]
        private string phNum;

        partial void OnPhNumChanged(string value)
        {
            customerModel.PhoneNumber = value;
        }

        [ObservableProperty]
        private string email;

        partial void OnEmailChanged(string value)
        {
            customerModel.Email = value;
        }


        [ObservableProperty]
        private Model.Contact contact;

        partial void OnContactChanged(Model.Contact value)
        {
            customerModel.Contact = value;
        }


    }
}
