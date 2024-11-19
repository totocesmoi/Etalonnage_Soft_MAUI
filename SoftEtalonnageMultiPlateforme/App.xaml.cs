using System.Globalization;
using SoftEtalonnageMultiPlateforme.Resources.Langue;

namespace SoftEtalonnageMultiPlateforme
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        
    }
}
