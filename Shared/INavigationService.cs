using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface INavigationService
    {
        Task NavigateToAsync(string page);
        Task GoBackAsync();
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToAsync(string page)
        {
            await Shell.Current.GoToAsync($"//{page}");
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task NavigateToUpdateUserAsync()
        {
            await Shell.Current.GoToAsync("//UserUpdate");
        }
    }
}
