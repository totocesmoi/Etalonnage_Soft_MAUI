using System.Diagnostics;
using VMService;
using VMWrapper;

namespace SoftEtalonnageMultiPlateforme.Views;

public partial class ContactCatalogue : ContentView
{
    public CustomerServiceVM toto { get; }
    public ContactServiceVM tata { get; }
    public ContactCatalogue(CustomerServiceVM customerServiceVM, ContactServiceVM contactServiceVM)
    {
        toto = customerServiceVM;
        tata = contactServiceVM;
        InitializeComponent(); 
        Debug.WriteLine($"CustomerServiceVM: {toto}, ContactServiceVM: {tata}"); 
        BindingContext = this;
    }
}



