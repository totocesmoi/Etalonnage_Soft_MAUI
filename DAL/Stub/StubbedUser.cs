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


        private static string userPath;

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

            SetPasswd(newUser, newUser.Password);

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
            if (UserCollection.UsersList.Any(u => u.Login == user.Login))
            {
                return await Task.FromResult(false);
            }

            SetPasswd(user, user.Password);
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
                existingUser.Name = user.Name;
                existingUser.Surname = user.Surname;
                existingUser.UserRole = user.UserRole;
                existingUser.Signature = user.Signature;
                existingUser.SignatureName = user.SignatureName;

                if (!string.IsNullOrEmpty(user.Password))
                {
                    SetPasswd(existingUser, user.Password);
                }

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

        /// <summary>
        /// Vérifie le mot de passe avec le hachage et le sel stockés
        /// </summary>
        /// <param name="user"></param>
        /// <param name="motDePasse"></param>
        /// <returns></returns>
        public static async Task<bool> VerifyMotDePasse(User user, string motDePasse)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: motDePasse,
                salt: user.Sel,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return await Task.FromResult(hashed == user.Password);
        }

        /// <summary>
        /// Setteur du mot de passe avec génération de clé de hachage
        /// </summary>
        /// <param name="motDePasse"></param>
        public static void SetPasswd(User user, string motDePasse)
        {
            // Générer un sel aléatoire
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hacher le mot de passe
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: motDePasse,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            user.Sel = salt;
            user.Password = hashed;
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
