using System.Reflection.PortableExecutable;

namespace Shared
{
    public interface IDataService<U,C,Cs,M>
        where U : class /* Pour User */
        where C : class /* Pour Customer */
        where Cs : class /* Pour Contacts */
        where M : class /* Pour Machines */
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


        Task<Pagination<Cs>> GetAsyncAllContact(int index, int count, string name);
        Task<Cs> GetContactsByCustomer(string name);
        Task<bool> CreateAsyncContact(Cs contact);
        Task<Cs> UpdateAsyncContact(Cs contact);
        Task<bool> DeleteAsyncContact(string name);


        Task<Pagination<M>> GetAsyncAllMachines(int index, int count);
        Task<M> GetAsyncByReference(string reference);
        Task<bool> CreateAsyncMachine(M machine);
        Task<M> UpdateAsyncMachine(M machine);
        Task<bool> DeleteAsyncMachine(string reference);

    }

}
