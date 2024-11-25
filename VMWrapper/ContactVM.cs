using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Model;

namespace VMWrapper
{
    public partial class ContactVM : ObservableObject , IEquatable<ContactVM>
    {
        [ObservableProperty]
        private Model.Contacts contactModel;

        [ObservableProperty]
        private string nameContact;

        partial void OnNameContactChanged(string value)
        {
            ContactModel.Name = value;
        }

        [ObservableProperty]
        private string surnameContact;

        partial void OnSurnameContactChanged(string value)
        {
            ContactModel.Surname = value;
        }

        [ObservableProperty]
        private string phoneNumberContact;

        partial void OnPhoneNumberContactChanged(string value)
        {
            ContactModel.PhoneNumber = value;
        }

        public bool Equals(ContactVM? other)
        {
            return string.IsNullOrEmpty(other?.NameContact) ? false : other.NameContact.Equals(ContactModel.Name);
        }

        public ContactVM(Model.Contacts contact)
        {
            contactModel = contact ?? throw new ArgumentNullException(nameof(contact));

            NameContact = contact.Name;
            SurnameContact = contact.Surname;
            PhoneNumberContact = contact.PhoneNumber;
        }

        public ContactVM()
        {
            contactModel = new Model.Contacts();

            NameContact = string.Empty;
            SurnameContact = string.Empty;
            PhoneNumberContact = string.Empty;
        }

    }
    
}
