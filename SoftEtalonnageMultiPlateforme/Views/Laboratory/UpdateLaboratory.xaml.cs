using VMService;

namespace SoftEtalonnageMultiPlateforme.Views.Laboratory;

public partial class UpdateLaboratory : ContentPage
{
	public UpdateLaboratory(LaboratoryServiceVM laboratoryServiceVM)
    {
        InitializeComponent();
        BindingContext = laboratoryServiceVM;
    }
}