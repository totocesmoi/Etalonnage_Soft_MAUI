using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class CreateCustomerPage : ContentPage
{
	public CreateCustomerPage(CustomerServiceVM customerService)
	{
		InitializeComponent();

        BindingContext = customerService;
    }
}