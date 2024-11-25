using VMService;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class CustomerUpdate : ContentPage
{
	public CustomerUpdate(CustomerServiceVM customerServiceVM)
	{
		InitializeComponent();
        BindingContext = customerServiceVM;
    }
}