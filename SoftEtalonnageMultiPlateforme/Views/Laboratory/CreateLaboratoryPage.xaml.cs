using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.Laboratory;

public partial class CreateLaboratoryPage : ContentPage
{
	public CreateLaboratoryPage(LaboratoryServiceVM laboratoryServiceVM)
    {
        InitializeComponent();

        BindingContext = laboratoryServiceVM;
    }
}