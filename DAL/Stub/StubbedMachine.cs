using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Stub
{
    public class StubbedMachine : ICRUDService<Machine>
    {
        public static CollectionManager<Machine> MachineCollection { get; set; }
        private readonly StubbedCustomer stubbed;

        public StubbedMachine(StubbedCustomer stubbedCustomer)
        {
            MachineCollection = new CollectionManager<Machine>();
            stubbed = stubbedCustomer;
        }

        public async Task<Pagination<Machine>> GetAsyncAllObject(int index, int count, string? customerName = null)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            if (customer == null)
            {
                return await Task.FromResult(new Pagination<Machine>
                {
                    TotalCount = 0,
                    Index = index,
                    Cout = count,
                    Items = new List<Machine>()
                });
            }

            var machines = customer.Machines
                .Skip(index * count)
                .Take(count)
                .ToList();

            var pagination = new Pagination<Machine>
            {
                TotalCount = customer.Machines.Count,
                Index = index,
                Cout = count,
                Items = machines
            };

            return await Task.FromResult(pagination);
        }

        public async Task<Machine> GetObjectAsyncById(string reference, string customerName)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            var machine = customer?.Machines.FirstOrDefault(m => m.Reference == reference);
            return await Task.FromResult(machine ?? null!);
        }

        public async Task<bool> CreateObject(Machine machine, string customerName)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            if (customer == null || customer.Machines.Any(m => m.Reference == machine.Reference))
            {
                return await Task.FromResult(false);
            }

            customer.Machines.Add(machine);
            await stubbed.UpdateObject(customer);
            return await Task.FromResult(true);
        }

        public async Task<Machine> UpdateObject(Machine machine, string customerName)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            var existingMachine = customer?.Machines.FirstOrDefault(m => m.Reference == machine.Reference);

            if (existingMachine != null)
            {
                existingMachine.Reference = machine.Reference;
                existingMachine.Configuration = machine.Configuration;
                existingMachine.ReceptionDate = machine.ReceptionDate;
                existingMachine.Model = machine.Model;
                existingMachine.SerialNumber = machine.SerialNumber;
                existingMachine.Comment = machine.Comment;

                await stubbed.UpdateObject(customer);
                return await Task.FromResult(existingMachine);
            }

            return await Task.FromResult<Machine>(null!);
        }

        public async Task<bool> DeleteObject(string reference, string customerName)
        {
            var customer = stubbed.GetObjectAsyncById(customerName).Result;
            var machine = customer?.Machines.FirstOrDefault(m => m.Reference == reference);

            if (machine != null)
            {
                customer.Machines.Remove(machine);
                await stubbed.UpdateObject(customer);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
