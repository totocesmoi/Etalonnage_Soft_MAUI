using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class CustomerCatalogue : ContentPage
{
   
    public CustomerCatalogue(CustomerServiceVM customerServiceVM)
	{
		InitializeComponent();
        BindingContext = customerServiceVM;
    }
}