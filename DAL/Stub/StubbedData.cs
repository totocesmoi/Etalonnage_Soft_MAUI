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

        public bool login(string login, string password)
        {
            var user = UserService.GetAsyncUserByLogin(login).Result;
            password = UserService.VerifyPassword(user, password).Result;
            if (user != null && user.Password == password)
            {
                currentUser = user;
                return true;
            }
            return false;
        }

        public void logout()
        {
            currentUser = null!;
        }

        public Task<Pagination<User>> GetAsyncAllUser(int index, int count)
        {
            return UserService.GetAsyncAllUser(index, count);
        }

        public Task<User?> GetAsyncCurrentUser()
        {
            return Task.FromResult(currentUser) ?? null!;
        }

        public Task<User> GetAsyncUserByLogin(string login)
        {
            return UserService.GetAsyncUserByLogin(login);
        }

        public Task<bool> CreateAsyncUser(User user)
        {
            return UserService.CreateUser(user);
        }

        public Task<User> UpdateAsyncUser(User user)
        {
            return UserService.UpdateUser(user);
        }

        public Task<bool> DeleteAsyncUser(string login)
        {
            return UserService.DeleteUser(login);
        }
    }
}
