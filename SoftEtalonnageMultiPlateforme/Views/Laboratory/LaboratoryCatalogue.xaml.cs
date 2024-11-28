using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.Laboratory;

public partial class LaboratoryCatalogue : ContentPage
{
	public LaboratoryCatalogue(LaboratoryServiceVM laboratoryServiceVM)
	{
		InitializeComponent();
        BindingContext = laboratoryServiceVM;
    }
}