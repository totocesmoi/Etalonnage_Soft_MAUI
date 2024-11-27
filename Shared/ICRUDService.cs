using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ICRUDService<T>
    {
        public Task<Pagination<T>> GetAsyncAllObject(int index, int count);
        public Task<T> GetObjectAsyncById(string id);
        public Task<bool> CreateObject(T genericObject);
        public Task<T> UpdateObject(T genericObject);
        public Task<bool> DeleteObject(string id);
    }
}
