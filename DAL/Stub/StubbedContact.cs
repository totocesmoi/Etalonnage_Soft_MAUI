using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Stub
{
    public class StubbedContact : ICRUDService<Model.Contacts>
    {
        public static CollectionManager<Model.Contacts> ContactCollection { get; set; }
        private readonly StubbedCustomer stubbed;

        public StubbedContact(StubbedCustomer stubbedCustomer)
        {
            ContactCollection = new CollectionManager<Model.Contacts>();
            stubbed = stubbedCustomer;
        }

        public async Task<Pagination<Model.Contacts>> GetAsyncAllObject(int index, int count, string? customerName = null)
        {
            var customers = stubbed.GetAsyncAllObject(index, count, customerName).Result;
            var customer = customers.Items.FirstOrDefault(c => c.Name == customerName);

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

        public async Task<Model.Contacts> GetObjectAsyncById(string name, string? customerName = null)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            var contact = customer?.Contacts.FirstOrDefault(c => c.Name == name);
            return await Task.FromResult(contact ?? null!);
        }

        public async Task<bool> CreateObject(Model.Contacts contact, string? customerName = null)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            if (customer == null || customer.Contacts.Any(c => c.Name == contact.Name))
            {
                return await Task.FromResult(false);
            }

            customer.Contacts.Add(contact);
            await stubbed.UpdateObject(customer);
            return await Task.FromResult(true);
        }

        public async Task<Model.Contacts> UpdateObject(Model.Contacts contact, string? customerName = null)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            var existingContact = customer?.Contacts.FirstOrDefault(c => c.Name == contact.Name);

            if (existingContact != null)
            {
                existingContact.Name = contact.Name;
                existingContact.PhoneNumber = contact.PhoneNumber;
                existingContact.Surname = contact.Surname;

                await stubbed.UpdateObject(customer);
                return await Task.FromResult(existingContact);
            }

            return await Task.FromResult<Model.Contacts>(null!);
        }

        public async Task<bool> DeleteObject(string name, string? customerName = null)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            var contact = customer?.Contacts.FirstOrDefault(c => c.Name == name);

            if (contact != null)
            {
                customer.Contacts.Remove(contact);
                await stubbed.UpdateObject(customer);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
