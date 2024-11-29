using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ICRUDService<T>
    {
        // Méthode pour récupérer l'information sans le customer courant
        public Task<Pagination<T>> GetAsyncAllObject(int index, int count) => GetAsyncAllObject(index, count, null);
        public Task<T> GetObjectAsyncById(string id) => GetObjectAsyncById(id, null);
        public Task<bool> CreateObject(T genericObject) => CreateObject(genericObject, null);
        public Task<T> UpdateObject(T genericObject) => UpdateObject(genericObject, null);
        public Task<bool> DeleteObject(string id) => DeleteObject(id, null);

        // Méthode pour récupérer l'information avec le customer courant
        public Task<Pagination<T>> GetAsyncAllObject(int index, int count, string? customerName = null);
        public Task<T> GetObjectAsyncById(string id, string? customerName = null);
        public Task<bool> CreateObject(T genericObject, string? customerName = null);
        public Task<T> UpdateObject(T genericObject, string? customerName = null);
        public Task<bool> DeleteObject(string id, string? customerName = null);
    }
}
