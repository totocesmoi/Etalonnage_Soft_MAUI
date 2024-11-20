using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Model;
using DAL.Stub;
using VMService;
using SoftEtalonnageMultiPlateforme.Views;
using SoftEtalonnageMultiPlateforme.Resources.Langue;

namespace SoftEtalonnageMultiPlateforme
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
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
            builder.Services.AddTransient<LoginServiceVM>();


            // Gestion des injection de views 
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<UserInfo>();
            builder.Services.AddTransient<UserUpdate>();
            builder.Services.AddTransient<CreateUserPage>();



#endif

            return builder.Build();
        }
    }
}
