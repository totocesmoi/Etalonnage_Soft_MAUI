using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IPostTraitementService<T>
    {
        Task<bool> CreateAsyncPostTraitement(T postTraitement);
        Task<T> TestExcutionAsync(T postTraitement);
        Task<bool> StopExcutionAsync(T postTraitement);
    }
}
