using Model;
using Shared;

namespace DAL.Stub
{
    public class StubbedData : IDataService<User>
    {
        private User ? currentUser = null;

        public IUserService<User> UserService { get; set; }

        public StubbedData()
        {
            UserService = new StubbedUser();
        }

        public void login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void logout()
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<User>> GetAsyncAllUser(int index, int count)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetAsyncCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsyncUserByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsyncUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsyncUser(User user, string login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsyncUser(string login)
        {
            throw new NotImplementedException();
        }
    }
}
