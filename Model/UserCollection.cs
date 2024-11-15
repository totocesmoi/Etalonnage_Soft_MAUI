using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model
{
    /// <summary>
    /// Class stockant tout ls utilisateurs enregistrer dans le soft.
    /// </summary>
    [Serializable]
    public class UserCollection
    {
        
        public List<User> UsersList => usersList;
        /// <summary>
        /// Liste regroupant tout les utilisateurs
        /// </summary>
        private List<User> usersList;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public UserCollection()
        {
            usersList = new List<User>();
        }

        /// <summary>
        /// Vérifie si repertoire exist, si il n'existe pas, alors il le créé
        /// </summary>
        /// <param name="path"></param>
        public void VerifDirectoryExist(string path)
        {
            string directory = Path.GetDirectoryName(path)!;
            if (!Directory.Exists(directory) || !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Sauvegarde dans un fichier xml, les informations de l'utilisateurs
        /// </summary>
        /// <param name="path"></param>
        public void SaveUserFile(string path)
        {
            try
            {
                VerifDirectoryExist(path);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                    serializer.Serialize(fileStream, usersList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while saving users : {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Permet de charger les données du fichier xml dans la liste
        /// </summary>
        /// <param name="path"></param>
        public void LoadUserFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    using (FileStream fileStream = new FileStream(path, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                        usersList = (List<User>)serializer.Deserialize(fileStream)!;
                    }

                    usersList = usersList ?? new List<User>();
                }
                else
                {
                    throw new Exception("User file doesn't exist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while loading users : {ex.Message}", "Error");
            }
        }

    }
}
