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
    public class StubbedUser : IUserService<User>
    {
        public static UserCollection UserCollection { get; set; }


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

            UserCollection = new UserCollection();

            Console.WriteLine($"Checking directory: {directoryPath}");
            Console.WriteLine($"Checking file: {filePathUsers}");

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                Directory.CreateDirectory(directoryPath);
                InitializeUsers();
                UserCollection.SaveUserFile(filePathUsers);
            }
            else
            {
                Console.WriteLine($"Directory exists: {directoryPath}");
                if (!File.Exists(filePathUsers))
                {
                    Console.WriteLine($"File not found: {filePathUsers}");
                    InitializeUsers();
                    UserCollection.SaveUserFile(filePathUsers);
                }
                else
                {
                    UserCollection.LoadUserFile(filePathUsers);
                    if (UserCollection.UsersList.Count == 0)
                    {
                        Console.WriteLine("No user found, creating default users");
                        InitializeUsers();
                        UserCollection.SaveUserFile(filePathUsers);
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

            UserCollection.UsersList.Add(newUser);
            Console.WriteLine("User created: " + newUser.Login);
        }

        /// <summary>
        /// Permet de récupérer tout les utilsateurs
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<Pagination<User>> GetAsyncAllUser(int index, int count)
        {
            var users = UserCollection.UsersList.Skip(index * count).Take(count).ToList();
            var pagination = new Pagination<User>
            {
                TotalCount = UserCollection.UsersList.Count,
                Index = index,
                Cout = count,
                Items = users
            };
            return await Task.FromResult(pagination);
        }

        /// <summary>
        /// Permet de récupérer un utilisateur par son login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<User> GetAsyncUserByLogin(string login)
        {
            var user = UserCollection.UsersList.FirstOrDefault(u => u.Login == login);
            return await Task.FromResult(user) ?? null!;
        }

        /// <summary>
        /// Permet de créer un utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(User user)
        {
            user.Login = user.GenerateLogin(user.Surname, user.Name);
            if (UserCollection.UsersList.Any(u => u.Login == user.Login))
            {
                return await Task.FromResult(false);
            }

            user.SetPasswd(user.Password);
            UserCollection.UsersList.Add(user);
            UserCollection.SaveUserFile(userPath);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Permet de mettre à jour un utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <returns> l'utilsateur modifier</returns>
        public async Task<User> UpdateUser(User user)
        {

            var existingUser = UserCollection.UsersList.FirstOrDefault(u => u.Login == user.Login);
            if (existingUser != null)
            {
                existingUser = user;

                UserCollection.SaveUserFile(userPath);
                return await Task.FromResult(existingUser);
            }
            return await Task.FromResult<User>(null!);
        }

        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <param name="login"></param>
        /// <returns>true ou false</returns>
        public async Task<bool> DeleteUser(string login)
        {
            var existingUser = UserCollection.UsersList.FirstOrDefault(u => u.Login == login);
            if (existingUser != null)
            {
                UserCollection.UsersList.Remove(existingUser);
                UserCollection.SaveUserFile(userPath);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        

        public async Task<string> VerifyPassword(User user, string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: user.Sel,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return await Task.FromResult(hashed == user.Password ? hashed : String.Empty);
        }
    }
}
