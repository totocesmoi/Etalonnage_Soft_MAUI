using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{

    public interface ICustomerService<T>
    {
        public Task<Pagination<T>> GetAsyncAllCustomer(int index, int count);
        public Task<bool> CreateCustomer(T user);
        public Task<T> UpdateCustomer(T user);
        public Task<bool> DeleteCustomer(string login);

    }    
    
}


