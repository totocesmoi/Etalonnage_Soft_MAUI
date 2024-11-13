using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IUserService<T>
        where T : class
    {
        public Task<Pagination<T>> GetAsyncAllUser(int index, int count);
        public Task<T> GetAsyncUserByLogin(string login);
        public Task<bool> CreateUser(T user);
        public Task<T> UpdateUser(T user);
        public Task<bool> DeleteUser(T user);
    }
}
