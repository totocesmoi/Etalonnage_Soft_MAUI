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

        public Contact Contact { get; set; }

        public List<Machine> Machines { get; set; }

        public Customer()
        {
            Contact = new Contact();
            Machines = new List<Machine>();
        }

        public Customer(string nom, string adresse, string telephone, string email)
        {
            Name = nom;
            Address = adresse;
            PhoneNumber = telephone;
            Email = email;
            Contact = new Contact();
            Machines = new List<Machine>();
        }

        public Customer(string nom, string adresse, string telephone, string email, Contact contact)
        {
            Name = nom;
            Address = adresse;
            PhoneNumber = telephone;
            Email = email;
            Contact = contact;
            Machines = new List<Machine>();
        }
    }
}
