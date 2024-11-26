using Model;
using Shared;

namespace DAL.Stub
{
    public class StubbedData : IDataService<User, Customer, PostTraitement>
    {
        private User? currentUser = null;

        public IUserService<User> UserService { get; set; }

        public ICustomerService<Customer> CustomerService { get; set; }

        public IPostTraitementService<PostTraitement> PostTraitementService { get; set; }


        public StubbedData()
        {
            UserService = new StubbedUser();
            CustomerService = new StubbedCustomer();
            PostTraitementService = new StubbedPostTraitement();
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

        public Task<Pagination<Customer>> GetAsyncAllCustomer(int index, int count)
        {
            return CustomerService.GetAsyncAllCustomer(index, count);
        }

        public Task<Customer> GetAsyncByName(string name)
        {
            return CustomerService.GetAsyncByName(name);
        }

        public Task<bool> CreateAsyncCustomer(Customer customer)
        {
            return CustomerService.CreateCustomer(customer);
        }

        public Task<Customer> UpdateAsyncCustomer(Customer customer)
        {
            return CustomerService.UpdateCustomer(customer);
        }

        public Task<bool> DeleteAsyncCustomer(string name)
        {
            return CustomerService.DeleteCustomer(name);
        }

        public Task<bool> CreateAsyncPostTraitement(PostTraitement postTraitement)
        {
            return PostTraitementService.CreateAsyncPostTraitement(postTraitement);
        }

        public Task<PostTraitement> TestExcutionAsync(PostTraitement postTraitement)
        {
            return PostTraitementService.TestExcutionAsync(postTraitement);
        }

        public Task<bool> StopExcutionAsync(PostTraitement postTraitement)
        {
            return PostTraitementService.StopExcutionAsync(postTraitement);
        }
    }
}
