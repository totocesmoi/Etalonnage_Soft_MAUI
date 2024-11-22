namespace Shared
{
    public interface IDataService<U,C>
        where U : class /* Pour User */
        where C : class /* Pour Customer */
    {
        // Gestion des utilisateurs
        bool login(string login, string password);
        void logout();
        Task<U?> GetAsyncCurrentUser();
        Task<Pagination<U>> GetAsyncAllUser(int index, int count);
        Task<U> GetAsyncUserByLogin(string login);
        Task<bool> CreateAsyncUser(U user);
        Task<U> UpdateAsyncUser(U user);
        Task<bool> DeleteAsyncUser(string login);

        Task<Pagination<C>> GetAsyncAllCustomer(int index, int count);
        Task<C> GetAsyncByName(string name);
        Task<bool> CreateAsyncCustomer(C customer);
        Task<C> UpdateAsyncCustomer(C customer);
        Task<bool> DeleteAsyncCustomer(string name);



    }
    
}
