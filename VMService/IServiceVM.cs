using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMService
{
    /// <summary>
    /// Interface avec la méthode CreateCommands afin d'avoir une méthode de création des commandes pour tout mes services
    /// </summary>
    public interface IServiceVM
    {
        void CreateCommands();
    }
}
