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
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string surname;

        [ObservableProperty]
        private Role userRole;

        public HeaderUserVM()
        {
            Name = "";
            Surname = "";
            UserRole = new Role();
        }
    }
}
