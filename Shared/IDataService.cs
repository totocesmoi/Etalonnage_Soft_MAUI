using System.Reflection.PortableExecutable;

namespace Shared
{
    public interface IDataService<U,C,Cs,M,T,L>
        where U : class /* Pour User */
        where C : class /* Pour Customer */
        where Cs : class /* Pour Contacts */
        where M : class /* Pour Machines */
        where T : class /* Pour PostTraitement */
        where L : class /* Pour Laboratory */
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

        // Gestion des clients
        Task<Pagination<C>> GetAsyncAllCustomer(int index, int count);
        Task<C> GetAsyncByName(string name);
        Task<bool> CreateAsyncCustomer(C customer);
        Task<C> UpdateAsyncCustomer(C customer);
        Task<bool> DeleteAsyncCustomer(string name);

        // Gestion des contacts
        Task<Pagination<Cs>> GetAsyncAllContact(int index, int count, string name);
        Task<Cs> GetContactsByCustomer(string name);
        Task<bool> CreateAsyncContact(Cs contact);
        Task<Cs> UpdateAsyncContact(Cs contact);
        Task<bool> DeleteAsyncContact(string name);

        // Gestion des machines
        Task<Pagination<M>> GetAsyncAllMachines(int index, int count);
        Task<M> GetAsyncByReference(string reference);
        Task<bool> CreateAsyncMachine(M machine);
        Task<M> UpdateAsyncMachine(M machine);
        Task<bool> DeleteAsyncMachine(string reference);

        // Gestion du post-traitement
        Task<bool> CreateAsyncPostTraitement(T postTraitement);
        Task<T> TestExcutionAsync(T postTraitement);
        Task<bool> StopExcutionAsync(T postTraitement);

        // Gestion des laboratoires
        Task<Pagination<L>> GetAsyncAllLaboratory(int index, int count);
        Task<L> GetLaboratoryAsyncByName(string name);
        Task<bool> CreateAsyncLaboratory(L laboratory);
        Task<L> UpdateAsyncLaboratory(L laboratory);
        Task<bool> DeleteAsyncLaboratory(string name);
    }
}
