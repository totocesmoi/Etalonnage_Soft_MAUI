using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VMWrapper
{
    public partial class HeaderUserVM : ObservableObject
    {
        private readonly Manager _manager; 

        public HeaderUserVM(Manager manager)
        {
            _manager = manager;


            UpdateUserInfo();
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string surname;

        [ObservableProperty]
        private Role userRole;

        private void UpdateUserInfo()
        {
            var currentUser = _manager.GetCurrentUser().Result; 
            if (currentUser != null)
            {
                Name = currentUser.Name;
                Surname = currentUser.Surname;
                UserRole = currentUser.UserRole;
            }
        }
    }
}
