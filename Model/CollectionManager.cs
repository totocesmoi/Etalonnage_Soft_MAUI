using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model
{
    public class CollectionManager<T>
    {
        public List<T> Items { get; private set; } = new List<T>();

        /// <summary>
        /// Vérifie si le répertoire existe, et le crée si nécessaire.
        /// </summary>
        private void EnsureDirectoryExists(string path)
        {
            string directory = Path.GetDirectoryName(path)!;
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Sauvegarde la collection dans un fichier XML.
        /// </summary>
        public void SaveToFile(string path)
        {
            try
            {
                EnsureDirectoryExists(path);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(List<T>));
                    serializer.Serialize(fileStream, Items);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
            }
        }

        /// <summary>
        /// Charge la collection à partir d'un fichier XML.
        /// </summary>
        public void LoadFromFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    using (var fileStream = new FileStream(path, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(List<T>));
                        Items = (List<T>)serializer.Deserialize(fileStream) ?? new List<T>();
                    }
                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas.");
                    Items = new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement : {ex.Message}");
            }
        }
    }
}

