using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface INavigationService
    {
        Task NavigateToLoginAsync();
        Task NavigateToMainPageAsync();
        Task NavigateToUpdateUserAsync();
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToLoginAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public async Task NavigateToMainPageAsync()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        public async Task NavigateToUpdateUserAsync()
        {
            await Shell.Current.GoToAsync("//UserUpdate");
        }
    }
}
