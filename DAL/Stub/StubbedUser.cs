using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Stub
{
    public class StubbedUser : IUserService<User>
    {
        public static UserCollection UserCollection { get; set; } = new UserCollection();

        public Task<Pagination<User>> GetAsyncAllUser(int index, int count)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsyncUserByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
