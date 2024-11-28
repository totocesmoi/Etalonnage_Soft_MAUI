using System.Globalization;
using Model;
using SoftEtalonnageMultiPlateforme.Resources.Theme;
using SoftEtalonnageMultiPlateforme.Views;
using SoftEtalonnageMultiPlateforme.Views.UserPage;
using SoftEtalonnageMultiPlateforme.Views.PostTraitement;
using SoftEtalonnageMultiPlateforme.Views.Laboratory;
using VMService;

namespace SoftEtalonnageMultiPlateforme
{
    public partial class AppShell : Shell
    {

        public AppShell(CurrentUserServiceVM currentUserServiceVM, LoginServiceVM loginServiceVM)
        {
            InitializeComponent();

            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("UserUpdate", typeof(UserUpdate));
            Routing.RegisterRoute("CreateUserPage", typeof(CreateUserPage));

            Routing.RegisterRoute("CustomerUpdate", typeof(CustomerUpdate));
            Routing.RegisterRoute("CreateCustomerPage", typeof(CreateCustomerPage));

            Routing.RegisterRoute("CreateLaboratoryPage", typeof(CreateLaboratoryPage));
            Routing.RegisterRoute("UpdateLaboratory", typeof(UpdateLaboratory));

            Routing.RegisterRoute("PostTraitementSection", typeof(PostTraitementSection));

            FlyoutHeader.BindingContext = currentUserServiceVM;
            FlyoutFooter.BindingContext = loginServiceVM;
        }

        private void PreferenceLoading(object sender, EventArgs e)
        {
            if (Preferences.Default.ContainsKey("Theme"))
            {
                string? chosenThemeString = Preferences.Default.Get("Theme", "chosenTheme");

                ResourceDictionary chosenTheme = chosenThemeString switch
                {
                    "Dark" => new DarkTheme(),
                    "Light" => new LightTheme(),
                    "Dufournier" => new DufournierTheme(),
                    _ => new DufournierTheme()
                };

                ICollection<ResourceDictionary> mergedDictionaries = Application.Current!.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    foreach (var dico in mergedDictionaries.Where(d => d is IThemeManager).ToList())
                    {
                        mergedDictionaries.Remove(dico);
                    }
                    mergedDictionaries.Add(chosenTheme);
                }
            }

            if (Preferences.Default.ContainsKey("Language"))
            {
                string? chosenLanguage = Preferences.Default.Get("Language", "fr-FR");
                CultureInfo newCulture = new CultureInfo(chosenLanguage);
                CultureInfo.CurrentUICulture = newCulture;
                CultureInfo.CurrentCulture = newCulture;
            }
        }
    }
}
