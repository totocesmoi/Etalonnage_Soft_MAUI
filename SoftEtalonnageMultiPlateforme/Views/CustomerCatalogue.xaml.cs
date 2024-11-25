using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class CustomerCatalogue : ContentPage
{
    public CustomerServiceVM CustomerServiceVM { get; }
    public ContactServiceVM ContactServiceVM { get; }

    public CustomerCatalogue(CustomerServiceVM customerServiceVM, ContactServiceVM contactServiceVM)
	{
		CustomerServiceVM = customerServiceVM;
        ContactServiceVM = contactServiceVM;

		InitializeComponent();
        BindingContext = this;
    }
}