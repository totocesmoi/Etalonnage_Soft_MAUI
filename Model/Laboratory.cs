using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Laboratory
    {
        /// <summary>
        /// Nom du laboratoire
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Adresse du laboratoire
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Lieu du laboratoire, pouvant se trouver à la même adresse
        /// </summary>
        public string LaboritoryLocation { get; set; }

        public Picture CachetEntreprise { get; set; }

        public Laboratory()
        {
            CachetEntreprise = new Picture();
        }

        public Laboratory(string name, string adress, string laboritoryLocation, Picture cachetEntreprise = null)
        {
            Name = name;
            Address = adress;
            LaboritoryLocation = laboritoryLocation;
            CachetEntreprise = cachetEntreprise ?? new Picture();
        }
    }
}
