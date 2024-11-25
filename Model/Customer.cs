using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<Model.Contacts> Contacts { get; set; }

        public List<Machine> Machines { get; set; }

        public Customer()
        {
            Name = string.Empty;
            Address = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
            Contacts = new List<Model.Contacts>(); 
            Machines = new List<Machine>();
        }

        public Customer(string nom, string adresse, string telephone, string email)
        {
            Name = nom;
            Address = adresse;
            PhoneNumber = telephone;
            Email = email;
            Contacts = new List<Model.Contacts>(); 
            Machines = new List<Machine>();
        }

        public Customer(string nom, string adresse, string telephone, string email, List<Model.Contacts> contacts)
        {
            Name = nom;
            Address = adresse;
            PhoneNumber = telephone;
            Email = email;
            Contacts = contacts; 
            Machines = new List<Machine>();
        }
    }

}
