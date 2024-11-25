using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using System.Reflection.PortableExecutable;


namespace VMWrapper
{
    public partial class CustomerVM : ObservableObject, IEquatable<CustomerVM>
    {
        [ObservableProperty]
        private Customer customerModel;

        
        public CustomerVM(Customer customerModel)
        {
            this.customerModel = customerModel ?? throw new ArgumentNullException(nameof(customerModel));

            Name = customerModel.Name;
            Address = customerModel.Address;
            PhNum = customerModel.PhoneNumber;

            
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



        public bool Equals(CustomerVM? other)
        {
            return string.IsNullOrEmpty(other?.Name) ? false : other.Name.Equals(CustomerModel.Name);
        }
    }
}
