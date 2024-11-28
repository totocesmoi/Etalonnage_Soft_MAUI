using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model
{
    public class PostTraitement
    {
        public string TypeOfTest { get; set; }

        public Machine Machine { get; set; }

        public Customer Customer { get; set; }

        public Laboratory Laboratory { get; set; }

        public User User { get; set; }

        public double? Temperature { get; set; }

        public string SensorType { get; set; }

        public string CertificatReference { get; set; }

        public PostTraitement()
        {
            Machine = new Machine();
            Customer = new Customer();
            Laboratory = new Laboratory();
            User = new User();
        }
    }
}
