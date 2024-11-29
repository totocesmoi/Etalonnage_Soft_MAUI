using Microsoft.Maui.ApplicationModel.Communication;
﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Model;
using Shared;

namespace DAL.Stub
{
    public class StubbedData : IDataService<User, Customer, Model.Contacts, Machine, PostTraitement, Laboratory>
    {
        private User? currentUser = null;

        public ICRUDService<User> UserService { get; set; }

        public ICRUDService<Laboratory> LaboratoryService { get; set; }

        public ICRUDService<Customer> CustomerService { get; set; }

        public ICRUDService<Model.Contacts> ContactService { get; set; }

        public ICRUDService<Machine> MachineService { get; set; }

        public IPostTraitementService<PostTraitement> PostTraitementService { get; set; }

        public StubbedData()
        {
            UserService = new StubbedUser();
            // Ici je suis obligé de créer un StubbedCustomer pour pouvoir instancier un StubbedMachine et un StubbedContact
            var stubbedCustomer = new StubbedCustomer();
            CustomerService = stubbedCustomer;

            PostTraitementService = new StubbedPostTraitement();
            LaboratoryService = new StubbedLaboratory();

            MachineService = new StubbedMachine(stubbedCustomer);
            ContactService = new StubbedContact(stubbedCustomer);
        }

        public bool login(string login, string password)
        {
            var user = UserService.GetObjectAsyncById(login).Result;
            password = VerifyPassword(user, password).Result;
            if (user != null && user.Password == password)
            {
                currentUser = user;
                return true;
            }
            return false;
        }

        public void logout()
        {
            currentUser = null!;
        }

        public async Task<string> VerifyPassword(User user, string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: user.Sel,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return await Task.FromResult(hashed == user.Password ? hashed : String.Empty);
        }

        public Task<User?> GetAsyncCurrentUser()
        {
            return Task.FromResult(currentUser) ?? null!;
        }

        public Task<Pagination<User>> GetAsyncAllUser(int index, int count)
        {
            return UserService.GetAsyncAllObject(index, count);
        }

        public Task<User> GetAsyncUserByLogin(string login)
        {
            return UserService.GetObjectAsyncById(login);
        }

        public Task<bool> CreateAsyncUser(User user)
        {
            return UserService.CreateObject(user);
        }

        public Task<User> UpdateAsyncUser(User user)
        {
            return UserService.UpdateObject(user);
        }

        public Task<bool> DeleteAsyncUser(string login)
        {
            return UserService.DeleteObject(login);
        }

        public Task<Pagination<Customer>> GetAsyncAllCustomer(int index, int count)
        {
            return CustomerService.GetAsyncAllObject(index, count);
        }

        public Task<Customer> GetAsyncByName(string name)
        {
            return CustomerService.GetObjectAsyncById(name);
        }

        public Task<bool> CreateAsyncCustomer(Customer customer)
        {
            return CustomerService.CreateObject(customer);
        }

        public Task<Customer> UpdateAsyncCustomer(Customer customer)
        {
            return CustomerService.UpdateObject(customer);
        }

        public Task<bool> DeleteAsyncCustomer(string name)
        {
            return CustomerService.DeleteObject(name);
        }

        // Gestion des contacts
        public Task<Pagination<Model.Contacts>> GetAsyncAllContact(int index, int count, string name)
        {
            return ContactService.GetAsyncAllObject(index, count, name);
        }

        public Task<Model.Contacts> GetContactsByCustomer(string name)
        {
            return ContactService.GetObjectAsyncById(name);
        }

        public Task<bool> CreateAsyncContact(Model.Contacts contact)
        {
            return ContactService.CreateObject(contact);
        }

        public Task<Model.Contacts> UpdateAsyncContact(Model.Contacts contact)
        {
            return ContactService.UpdateObject(contact);
        }

        public Task<bool> DeleteAsyncContact(string name)
        {
            return ContactService.DeleteObject(name);
        }

        // Gestion des machines
        public Task<Pagination<Machine>> GetAsyncAllMachines(int index, int count)
        {
            return MachineService.GetAsyncAllObject(index, count);
        }

        public Task<Machine> GetAsyncByReference(string reference)
        {
            return MachineService.GetObjectAsyncById(reference);
        }

        public Task<bool> CreateAsyncMachine(Machine machine)
        {
            return MachineService.CreateObject(machine);
        }

        public Task<Machine> UpdateAsyncMachine(Machine machine)
        {
            return MachineService.UpdateObject(machine);
        }

        public Task<bool> DeleteAsyncMachine(string reference)
        {
            return MachineService.DeleteObject(reference);
        }

        public Task<bool> CreateAsyncPostTraitement(PostTraitement postTraitement)
        {
            return PostTraitementService.CreateAsyncPostTraitement(postTraitement);
        }

        public Task<PostTraitement> TestExcutionAsync(PostTraitement postTraitement)
        {
            return PostTraitementService.TestExcutionAsync(postTraitement);
        }

        public Task<bool> StopExcutionAsync(PostTraitement postTraitement)
        {
            return PostTraitementService.StopExcutionAsync(postTraitement);
        }

        public Task<Pagination<Laboratory>> GetAsyncAllLaboratory(int index, int count)
        {
            return LaboratoryService.GetAsyncAllObject(index, count);
        }

        public Task<Laboratory> GetLaboratoryAsyncByName(string name)
        {
            return LaboratoryService.GetObjectAsyncById(name);
        }

        public Task<bool> CreateAsyncLaboratory(Laboratory laboratory)
        {
            return LaboratoryService.CreateObject(laboratory);
        }

        public Task<Laboratory> UpdateAsyncLaboratory(Laboratory laboratory)
        {
            return LaboratoryService.UpdateObject(laboratory);
        }

        public Task<bool> DeleteAsyncLaboratory(string name)
        {
            return LaboratoryService.DeleteObject(name);
        }
    }
}