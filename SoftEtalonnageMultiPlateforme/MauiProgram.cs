using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Model;
using DAL.Stub;
using VMService;
using SoftEtalonnageMultiPlateforme.Views;

namespace SoftEtalonnageMultiPlateforme
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                // Initialize the .NET MAUI Community Toolkit by adding the below line of code
                .UseMauiCommunityToolkit()
                // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            // Gestion des injections de dépendances

            // Ajout des services de données
            builder.Services.AddSingleton<IDataService<User>, StubbedData>();
            // Ajout de la navigation
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            // Ajout des commandes utilisables dans l'application
            builder.Services.AddSingleton<Manager>();
            // Ajout des commandes de l'utilisateurs
            builder.Services.AddSingleton<UserServiceVM>();
            // Ajout des commandes de connexion
            builder.Services.AddSingleton<LoginServiceVM>();


            // Gestion des injection de views 
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            


#endif

            return builder.Build();
        }
    }
}
