using Shared;
using System.Xml.Linq;

namespace Model
{
    public class Manager
    {
        // Gestion des services + constructeur
        public IDataService<User,Customer> Service { get; set; }

        public INavigationService Navigation { get; set; }

        public Manager(IDataService<User,Customer> service, INavigationService navigation)
        {
            Service = service;
            Navigation = navigation;
        }

        // Propriété pour l'utilisateur courant
        public User? CurrentUser { get => Service.GetAsyncCurrentUser().Result; }

        // Gestion de l'utilisateur courant
        public Task<User?> GetCurrentUser() => Service.GetAsyncCurrentUser();

        // Gestion des utilisateurs
        public Task<Pagination<User>> GetAllUser(int index, int count) => Service.GetAsyncAllUser(index, count);
        public Task<User> GetUserByLogin(string login) => Service.GetAsyncUserByLogin(login);
        public Task<bool> CreateUser(User user) => Service.CreateAsyncUser(user);
        public Task<User> UpdateUser(User user) => Service.UpdateAsyncUser(user);
        public Task<bool> DeleteUser(string login) => Service.DeleteAsyncUser(login);

        // Gestion de la connexion / déconnexion
        public bool Login(string login, string password) => Service.login(login, password);
        public void Logout() => Service.logout();

        // Gestion des clients
        public Task<Pagination<Customer>> GetAllCustomer(int index, int count) => Service.GetAsyncAllCustomer(index, count);
        public Task<Customer> GetUserByNme(string name) => Service.GetAsyncByName(name);
        public Task<bool> CreateCustomer(Customer customer) => Service.CreateAsyncCustomer(customer);
        public Task<Customer> UpdateCustomer(Customer customer) => Service.UpdateAsyncCustomer(customer);
        public Task<bool> DeleteCustomer(string name) => Service.DeleteAsyncCustomer(name);


    }
}
