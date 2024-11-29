using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Stub
{
    public class StubbedCustomer : ICRUDService<Customer>
    {
        public static CollectionManager<Customer> CustomerCollection { get; set; }
        private static string CustomerPath;

        static StubbedCustomer()
        {
            string directoryPath;
            string filePathCustomers;

#if WINDOWS
                directoryPath = Path.Combine("C:", "Soft_Etalonnage", "Configuration");
                filePathCustomers = Path.Combine(directoryPath, "customers.xml");
#else
            directoryPath = FileSystem.AppDataDirectory;
            filePathCustomers = Path.Combine(directoryPath, "customers.xml");
#endif

            CustomerCollection = new CollectionManager<Customer>();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                CustomerCollection.SaveToFile(filePathCustomers);
            }
            else
            {
                if (!File.Exists(filePathCustomers))
                {
                    CustomerCollection.SaveToFile(filePathCustomers);
                }
                else
                {
                    CustomerCollection.LoadFromFile(filePathCustomers);
                    if (CustomerCollection.Items.Count == 0)
                    {
                        CustomerCollection.SaveToFile(filePathCustomers);
                    }
                }
            }

            CustomerPath = filePathCustomers;
        }

        // ---- CUSTOMER IMPLEMENTATIONS ----

        // Méthodes sans paramètre customerName
        public async Task<Pagination<Customer>> GetAsyncAllObject(int index, int count)
        {
            return await GetAsyncAllObject(index, count, null);
        }

        public async Task<Customer> GetObjectAsyncById(string id)
        {
            return await GetObjectAsyncById(id, null);
        }

        public async Task<bool> CreateObject(Customer genericObject)
        {
            return await CreateObject(genericObject, null);
        }

        public async Task<Customer> UpdateObject(Customer genericObject)
        {
            return await UpdateObject(genericObject, null);
        }

        public async Task<bool> DeleteObject(string id)
        {
            return await DeleteObject(id, null);
        }

        // Méthodes avec paramètre customerName
        public async Task<Pagination<Customer>> GetAsyncAllObject(int index, int count, string? customerName)
        {
            var customers = CustomerCollection.Items.Skip(index * count).Take(count).ToList();
            var pagination = new Pagination<Customer>
            {
                TotalCount = CustomerCollection.Items.Count,
                Index = index,
                Cout = count,
                Items = customers
            };
            return await Task.FromResult(pagination);
        }

        public async Task<Customer> GetObjectAsyncById(string id, string? customerName)
        {
            var customer = CustomerCollection.Items.FirstOrDefault(u => u.Name == id);
            return await Task.FromResult(customer ?? null!);
        }

        public async Task<bool> CreateObject(Customer genericObject, string? customerName)
        {
            if (CustomerCollection.Items.Any(u => u.Name == genericObject.Name))
            {
                return await Task.FromResult(false);
            }

            CustomerCollection.Items.Add(genericObject);
            CustomerCollection.SaveToFile(CustomerPath);
            return await Task.FromResult(true);
        }

        public async Task<Customer> UpdateObject(Customer genericObject, string? customerName)
        {
            var existingCustomer = CustomerCollection.Items.FirstOrDefault(u => u.Name == genericObject.Name);
            if (existingCustomer != null)
            {
                existingCustomer.Name = genericObject.Name;
                existingCustomer.Address = genericObject.Address;
                existingCustomer.Email = genericObject.Email;
                existingCustomer.Contacts = genericObject.Contacts;
                existingCustomer.Machines = genericObject.Machines;

                CustomerCollection.SaveToFile(CustomerPath);
                return await Task.FromResult(existingCustomer);
            }
            return await Task.FromResult<Customer>(null!);
        }

        public async Task<bool> DeleteObject(string id, string? customerName)
        {
            var existingCustomer = CustomerCollection.Items.FirstOrDefault(u => u.Name == id);
            if (existingCustomer != null)
            {
                CustomerCollection.Items.Remove(existingCustomer);
                CustomerCollection.SaveToFile(CustomerPath);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
