using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model
{
    public class CustomerCollection
    {

        private List<Customer> customersList;
        public List<Customer> CustomersList => customersList;

        public CustomerCollection()
        {
            customersList = new List<Customer>();
        }

        /// <summary>
        /// Vérifie si repertoire exist, si il n'existe pas, alors il le créé
        /// </summary>
        /// <param name="path"></param>
        public void VerifDirectoryExist(string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory) || !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Sauvegarde dans un fichier xml, les informations de l'utilisateurs
        /// </summary>
        /// <param name="path"></param>
        public void SaveCustomerFile(string path)
        {
            try
            {
                VerifDirectoryExist(path);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>));
                    serializer.Serialize(fileStream, CustomersList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while loading users : {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Permet de charger les données du fichier xml dans la liste
        /// </summary>
        /// <param name="path"></param>
        public void LoadCustomerFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    using (FileStream fileStream = new FileStream(path, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>));
                        customersList = (List<Customer>)serializer.Deserialize(fileStream);
                    }

                    customersList = CustomersList ?? new List<Customer>();
                }
                else
                {
                    throw new Exception("Customers file doesn't exist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while loading users : {ex.Message}", "Error");
            }
        }

    }
}
