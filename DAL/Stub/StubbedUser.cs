using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DAL.Stub
{
    public class StubbedUser : ICRUDService<User>
    {
        public static CollectionManager<User> UserCollection { get; set; }

        readonly static string userPath;

        static StubbedUser()
        {
            string directoryPath;
            string filePathUsers;

#if WINDOWS
            directoryPath = Path.Combine("C:", "Soft_Etalonnage", "Configuration");
            filePathUsers = Path.Combine(directoryPath, "users.xml");
#else
            directoryPath = FileSystem.AppDataDirectory;
            filePathUsers = Path.Combine(directoryPath, "users.xml");
#endif

            UserCollection = new CollectionManager<User>();

            Console.WriteLine($"Checking directory: {directoryPath}");
            Console.WriteLine($"Checking file: {filePathUsers}");

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                Directory.CreateDirectory(directoryPath);
                InitializeUsers();
                UserCollection.SaveToFile(filePathUsers);
            }
            else
            {
                Console.WriteLine($"Directory exists: {directoryPath}");
                if (!File.Exists(filePathUsers))
                {
                    Console.WriteLine($"File not found: {filePathUsers}");
                    InitializeUsers();
                    UserCollection.SaveToFile(filePathUsers);
                }
                else
                {
                    UserCollection.LoadFromFile(filePathUsers);
                    if (UserCollection.Items.Count == 0)
                    {
                        Console.WriteLine("No user found, creating default users");
                        InitializeUsers();
                        UserCollection.SaveToFile(filePathUsers);
                    }
                }
            }
            // On sauvearde le Path de manière globale a l'application ici, puisqu'on ne peut pas l'utiliser avant le build de l'applcation
            userPath = filePathUsers;
        }

        /// <summary>
        /// Utilisateurs par défaut pour ne pas bloqué la connexion durant les tests
        /// </summary>
        private static void InitializeUsers()
        {
            User newUser = new User(
                nom: "muzard",
                prenom: "thomas",
                mdp: "azerty",
                role: Role.Administrator,
                picture: new Picture(),
                signatureName: "Toto l'artiste"
            );

            newUser.SetPasswd(newUser.Password);

            UserCollection.Items.Add(newUser);
            Console.WriteLine("User created: " + newUser.Login);
        }

        /// <summary>
        /// Permet de récupérer tout les utilsateurs
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<Pagination<User>> GetAsyncAllObject(int index, int count)
        {
            var users = UserCollection.Items.Skip(index * count).Take(count).ToList();
            var pagination = new Pagination<User>
            {
                TotalCount = UserCollection.Items.Count,
                Index = index,
                Cout = count,
                Items = users
            };
            return await Task.FromResult(pagination);
        }

        /// <summary>
        /// Permet de récupérer un utilisateur par son login
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetObjectAsyncById(string id)
        {
            var user = UserCollection.Items.FirstOrDefault(u => u.Login == id);
            return await Task.FromResult(user) ?? null!;
        }

        /// <summary>
        /// Permet de créer un utilisateur
        /// </summary>
        /// <param name="genericObject"></param>
        /// <returns></returns>
        public async Task<bool> CreateObject(User genericObject)
        {
            genericObject.Login = genericObject.GenerateLogin(genericObject.Surname, genericObject.Name);
            if (UserCollection.Items.Any(u => u.Login == genericObject.Login))
            {
                return await Task.FromResult(false);
            }

            genericObject.SetPasswd(genericObject.Password);
            UserCollection.Items.Add(genericObject);
            UserCollection.SaveToFile(userPath);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Permet de mettre à jour un utilisateur
        /// </summary>
        /// <param name="genericObject"></param>
        /// <returns> l'utilsateur modifier</returns>
        public async Task<User> UpdateObject(User genericObject)
        {

            var existingUser = UserCollection.Items.FirstOrDefault(u => u.Login == genericObject.Login);
            if (existingUser != null)
            {
                existingUser = genericObject;

                UserCollection.SaveToFile(userPath);
                return await Task.FromResult(existingUser);
            }
            return await Task.FromResult<User>(null!);
        }

        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true ou false</returns>
        public async Task<bool> DeleteObject(string id)
        {
            var existingUser = UserCollection.Items.FirstOrDefault(u => u.Login == id);
            if (existingUser != null)
            {
                UserCollection.Items.Remove(existingUser);
                UserCollection.SaveToFile(userPath);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
