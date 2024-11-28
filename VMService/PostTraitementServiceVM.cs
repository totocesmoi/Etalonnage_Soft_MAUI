using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMWrapper;

namespace VMService
{
    public partial class PostTraitementServiceVM : ObservableObject ,IServiceVM
    {
        private readonly Manager _service;
        private readonly CurrentUserServiceVM _currentUserServiceVM;
        private readonly CustomerServiceVM _customerServiceVM;
        private readonly MachineServiceVM _machineServiceVM;



        public void CreateCommands()
        {
            throw new NotImplementedException();
        }
    }
}
