using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public interface IContactService<T>
    {
        public Task<Pagination<T>> GetAsyncAllContact(int index, int count, string name);
        public Task<T> GetAsyncByCustomerName(string name);
        public Task<bool> CreateContact(T contact);
        public Task<T> UpdateContact(T contact);
        public Task<bool> DeleteContact(string name);
    }
}
