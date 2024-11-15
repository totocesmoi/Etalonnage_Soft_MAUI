using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToLoginAsync()
        {
            //var loginPage = _serviceProvider.GetService<LoginPage>();
            //await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        public async Task NavigateToMainPageAsync()
        {
            //var mainPage = _serviceProvider.GetService<MainPage>();
            //await Shell.Current.GoToAsync(nameof(MainPage));
        }
    }
}
