using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class CustomerUpdate : ContentPage
{
    public CustomerServiceVM CustomerServiceVM { get; }
    public ContactServiceVM ContactServiceVM { get; }
    public CustomerUpdate(CustomerServiceVM customerServiceVM, ContactServiceVM contactServiceVM)
	{
        CustomerServiceVM = customerServiceVM;
        ContactServiceVM = contactServiceVM;

		InitializeComponent();
        BindingContext = this;

        // Vous pouvez maintenant injecter les services dans le ContactCatalogue
        var contactCatalogue = new ContactCatalogue(CustomerServiceVM, ContactServiceVM);
        ContactCatalogueLayout.Children.Add(contactCatalogue);
    }
}