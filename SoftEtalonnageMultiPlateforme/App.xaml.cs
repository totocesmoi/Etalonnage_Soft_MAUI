using System.Globalization;
using SoftEtalonnageMultiPlateforme.Resources.Langue;

namespace SoftEtalonnageMultiPlateforme
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }

        
    }
}
