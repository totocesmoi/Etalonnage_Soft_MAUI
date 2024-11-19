using System.Globalization;
using SoftEtalonnageMultiPlateforme.Resources.Theme;

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
