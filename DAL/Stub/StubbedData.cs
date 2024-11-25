using Model;
using Shared;

namespace DAL.Stub
{
    public class StubbedData : IDataService<User, Customer, Model.Contacts>
    {
        private User? currentUser = null;

        public IUserService<User> UserService { get; set; }

        public ICustomerService<Customer> CustomerService { get; set; }


        public StubbedData()
        {
            UserService = new StubbedUser();
            CustomerService = new StubbedCustomer();
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

        public Task<Pagination<Model.Contacts>> GetAsyncAllContact(int index, int count, string name)
        {
            throw new NotImplementedException();
        }

        public Task<Model.Contacts> GetContactsByCustomer(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsyncContact(Model.Contacts contact)
        {
            throw new NotImplementedException();
        }

        public Task<Model.Contacts> UpdateAsyncContact(Model.Contacts contact)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsyncContact(string name)
        {
            throw new NotImplementedException();
        }
    }
}
