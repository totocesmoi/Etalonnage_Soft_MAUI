using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Machine
    {
        public string Reference { get; set; }

        public string Configuration { get; set; }

        public DateTime ReceptionDate { get; set; }

        public string Model { get; set; }

        public string SerialNumber { get; set; }

        public string Comment { get; set; }

        public Machine() { }

        public Machine(string reference, string configuration, DateTime date, string model, string serialNumber, string comment = "")
        {
            Reference = reference;
            Configuration = configuration;
            ReceptionDate = date;
            Model = model;
            SerialNumber = serialNumber;
            Comment = comment;
        }

    }
}
