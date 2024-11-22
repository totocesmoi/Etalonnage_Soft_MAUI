using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Stub
{
    public class  StubbedCustomer : ICustomerService<Customer>
    {
        public static CustomerCollection CustomerCollection { get; set; }
        private static string CustomerPath;

        static StubbedCustomer() {

            string directoryPath;
            string filePathCustomers;

#if WINDOWS
            directoryPath = Path.Combine("C:", "Soft_Etalonnage", "Configuration");
            filePathUsers = Path.Combine(directoryPath, "customers.xml");
#else
            directoryPath = FileSystem.AppDataDirectory;
            filePathCustomers = Path.Combine(directoryPath, "customers.xml");
#endif

            CustomerCollection = new CustomerCollection();

            Console.WriteLine($"Checking directory: {directoryPath}");
            Console.WriteLine($"Checking file: {filePathCustomers}");

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                Directory.CreateDirectory(directoryPath);
                CustomerCollection.SaveCustomerFile(filePathCustomers);
            }
            else
            {
                Console.WriteLine($"Directory exists: {directoryPath}");
                if (!File.Exists(filePathCustomers))
                {
                    Console.WriteLine($"File not found: {filePathCustomers}");
                    CustomerCollection.SaveCustomerFile(filePathCustomers);
                }
                else
                {
                    CustomerCollection.LoadCustomerFile(filePathCustomers);
                    if (CustomerCollection.CustomersList.Count == 0)
                    {
                        Console.WriteLine("No user found, creating default users");
                        CustomerCollection.SaveCustomerFile(filePathCustomers);
                    }
                }
            }
        
            CustomerPath = filePathCustomers;

        }

        public async Task<Pagination<Customer>> GetAsyncAllCustomer(int index, int count)
        {
            var customers = CustomerCollection.CustomersList.Skip(index * count).Take(count).ToList();
            var pagination = new Pagination<Customer>
            {
                TotalCount = CustomerCollection.CustomersList.Count,
                Index = index,
                Cout = count,
                Items = customers
            };
            return await Task.FromResult(pagination);
        }

        public async Task<bool> CreateCustomer(Customer customer)
        {

            if (CustomerCollection.CustomersList.Any(u => u.Name == customer.Name))
            {
                return await Task.FromResult(false);
            }

            CustomerCollection.CustomersList.Add(customer);
            CustomerCollection.SaveCustomerFile(CustomerPath);
            return await Task.FromResult(true);

        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {

            var existingCustomer = CustomerCollection.CustomersList.FirstOrDefault(u => u.Name == customer.Name);
            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.Address = customer.Address;
                existingCustomer.Email = customer.Email;
                existingCustomer.Contact = customer.Contact;
                existingCustomer.Machines = customer.Machines;

                CustomerCollection.SaveCustomerFile(CustomerPath);
                return await Task.FromResult(existingCustomer);
            }
            return await Task.FromResult<Customer>(null!);

        }

        public async Task<bool> DeleteCustomer(string name)
        {
            var existingCustomer = CustomerCollection.CustomersList.FirstOrDefault(u => u.Name == name);
            if (existingCustomer != null)
            {
                CustomerCollection.CustomersList.Remove(existingCustomer);
                CustomerCollection.SaveCustomerFile(CustomerPath);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }


    }
}