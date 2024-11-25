using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Contact
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public Contact(string name, string surname, string PhNum = null!)
        {
            Name = name;
            Surname = surname;
            PhoneNumber = PhNum;
        }

        public Contact() { }
   

    }
}
