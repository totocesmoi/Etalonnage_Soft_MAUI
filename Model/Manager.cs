using Shared;

namespace Model
{
    public class Manager
    {
        // Gestion des services + constructeur
        public IDataService<User> Service { get; set; }

        public Manager(IDataService<User> service)
        {
            Service = service;
        }

        // Propriété pour l'utilisateur courant
        public User? CurrentUser { get => Service.GetAsyncCurrentUser().Result; }

        // Gestion de l'utilisateur courant
        public Task<User?> GetCurrentUser() => Service.GetAsyncCurrentUser();

        // Gestion des utilisateurs
        public Task<Pagination<User>> GetAllUser(int index, int count) => Service.GetAsyncAllUser(index, count);
        public Task<User> GetUserByLogin(string login) => Service.GetAsyncUserByLogin(login);
        public Task<bool> CreateUser(User user) => Service.CreateAsyncUser(user);
        public Task<User> UpdateUser(User user, string login) => Service.UpdateAsyncUser(user, login);
        public Task<bool> DeleteUser(string login) => Service.DeleteAsyncUser(login);

        // Gestion de la connexion / déconnexion
        public void Login(string login, string password) => Service.login(login, password);
        public void Logout() => Service.logout();



    }
}
