﻿namespace Shared
{
    public interface IDataService<U>
        where U : class /* Pour User */
    {
        // Gestion des utilisateurs
        void login(string login, string password);
        void logout();
        Task<U?> GetAsyncCurrentUser();
        Task<Pagination<U>> GetAsyncAllUser(int index, int count);
        Task<U> GetAsyncUserByLogin(string login);
        Task<bool> CreateAsyncUser(U user);
        Task<U> UpdateAsyncUser(U user, string login);
        Task<bool> DeleteAsyncUser(string login);




    }
    
}