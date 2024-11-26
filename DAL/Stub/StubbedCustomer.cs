using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Stub
{
    public class StubbedCustomer : ICustomerService<Customer>, IContactService<Model.Contacts>, IMachineService<Machine>
    {
        public static CustomerCollection CustomerCollection { get; set; }
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

            CustomerCollection = new CustomerCollection();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                CustomerCollection.SaveCustomerFile(filePathCustomers);
            }
            else
            {
                if (!File.Exists(filePathCustomers))
                {
                    CustomerCollection.SaveCustomerFile(filePathCustomers);
                }
                else
                {
                    CustomerCollection.LoadCustomerFile(filePathCustomers);
                    if (CustomerCollection.CustomersList.Count == 0)
                    {
                        CustomerCollection.SaveCustomerFile(filePathCustomers);
                    }
                }
            }

            CustomerPath = filePathCustomers;
        }

        // ---- CUSTOMER IMPLEMENTATIONS ----

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

        public async Task<Customer> GetAsyncByName(string name)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault(u => u.Name == name);
            return await Task.FromResult(customer ?? null!);
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
                existingCustomer.Contacts = customer.Contacts;
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

        // ---- CONTACT IMPLEMENTATIONS ----

        public async Task<Pagination<Model.Contacts>> GetAsyncAllContact(int index, int count, string name)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault(c => c.Name == name);

            if (customer == null)
            {
                return await Task.FromResult(new Pagination<Model.Contacts>
                {
                    TotalCount = 0,
                    Index = index,
                    Cout = count,
                    Items = new List<Model.Contacts>()
                });
            }

            var contacts = customer.Contacts
                .Skip(index * count)
                .Take(count)
                .ToList();

            var pagination = new Pagination<Model.Contacts>
            {
                TotalCount = customer.Contacts.Count,
                Index = index,
                Cout = count,
                Items = contacts
            };

            return await Task.FromResult(pagination);
        }

        public async Task<Model.Contacts> GetAsyncByCustomerName(string name)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault(c => c.Name == name);
            return await Task.FromResult(customer?.Contacts.FirstOrDefault() ?? null!);
        }

        public async Task<bool> CreateContact(Model.Contacts contact)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault();
            if (customer == null || customer.Contacts.Any(c => c.Name == contact.Name))
            {
                return await Task.FromResult(false);
            }

            customer.Contacts.Add(contact);
            CustomerCollection.SaveCustomerFile(CustomerPath);
            return await Task.FromResult(true);
        }

        public async Task<Model.Contacts> UpdateContact(Model.Contacts contact)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault(c => c.Contacts.Any(ct => ct.Name == contact.Name));
            var existingContact = customer?.Contacts.FirstOrDefault(ct => ct.Name == contact.Name);

            if (existingContact != null)
            {
                existingContact.Name = contact.Name;
                existingContact.PhoneNumber= contact.PhoneNumber;
                existingContact.Surname = contact.Surname;

                CustomerCollection.SaveCustomerFile(CustomerPath);
                return await Task.FromResult(existingContact);
            }

            return await Task.FromResult<Model.Contacts>(null!);
        }

        public async Task<bool> DeleteContact(string name)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault(c => c.Contacts.Any(ct => ct.Name == name));
            var contact = customer?.Contacts.FirstOrDefault(ct => ct.Name == name);

            if (contact != null)
            {
                customer.Contacts.Remove(contact);
                CustomerCollection.SaveCustomerFile(CustomerPath);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        // ---- MACHINE IMPLEMENTATIONS ----

        public async Task<Pagination<Machine>> GetAsyncAllMachines(int index, int count)
        {
            var machines = CustomerCollection.CustomersList
                .SelectMany(c => c.Machines)
                .Skip(index * count)
                .Take(count)
                .ToList();

            var pagination = new Pagination<Machine>
            {
                TotalCount = CustomerCollection.CustomersList.SelectMany(c => c.Machines).Count(),
                Index = index,
                Cout = count,
                Items = machines
            };

            return await Task.FromResult(pagination);
        }

        public async Task<Machine> GetAsyncByReference(string reference)
        {
            var machine = CustomerCollection.CustomersList
                .SelectMany(c => c.Machines)
                .FirstOrDefault(m => m.Reference == reference);

            return await Task.FromResult(machine ?? null!);
        }

        public async Task<bool> CreateMachine(Machine machine)
        {
            var existingMachine = CustomerCollection.CustomersList
                .SelectMany(c => c.Machines)
                .FirstOrDefault(m => m.Reference == machine.Reference);

            if (existingMachine != null)
            {
                return await Task.FromResult(false);
            }

            var defaultCustomer = CustomerCollection.CustomersList.FirstOrDefault();
            if (defaultCustomer == null)
            {
                return await Task.FromResult(false);
            }

            defaultCustomer.Machines.Add(machine);
            CustomerCollection.SaveCustomerFile(CustomerPath);
            return await Task.FromResult(true);
        }

        public async Task<Machine> UpdateMachine(Machine machine)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault(c => c.Machines.Any(m => m.Reference == machine.Reference));
            var existingMachine = customer?.Machines.FirstOrDefault(m => m.Reference == machine.Reference);

            if (existingMachine != null)
            {
                existingMachine.Reference = machine.Reference;
                existingMachine.Configuration = machine.Configuration;
                existingMachine.ReceptionDate = machine.ReceptionDate;
                existingMachine.Model = machine.Model;
                existingMachine.SerialNumber = machine.SerialNumber;
                existingMachine.Comment = machine.Comment;

                CustomerCollection.SaveCustomerFile(CustomerPath);
                return await Task.FromResult(existingMachine);
            }

            return await Task.FromResult<Machine>(null!);
        }

        public async Task<bool> DeleteMachine(string reference)
        {
            var customer = CustomerCollection.CustomersList.FirstOrDefault(c => c.Machines.Any(m => m.Reference == reference));
            var machine = customer?.Machines.FirstOrDefault(m => m.Reference == reference);

            if (machine != null)
            {
                customer.Machines.Remove(machine);
                CustomerCollection.SaveCustomerFile(CustomerPath);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
