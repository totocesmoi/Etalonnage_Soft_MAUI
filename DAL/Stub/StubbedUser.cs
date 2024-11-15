using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Stub
{
    public class StubbedUser : IUserService<User>
    {
        public static UserCollection UserCollection { get; set; } = new UserCollection();

        static StubbedUser()
        {
            InitializeUsers();
        }

        private static void InitializeUsers()
        {
            // Création d'une image de signature (facultatif)
            byte[] imageData = File.ReadAllBytes("dotnet_bot.png");
            Picture signaturePicture = new Picture(imageData);

            // Création d'un utilisateur
            User newUser = new User(
                nom: "muzard",
                prenom: "thomas",
                mdp: "azerty",
                role: Role.Administrator,
                picture: signaturePicture,
                signatureName: "Jean Dupont"
            );

            // Ajout de l'utilisateur à la collection
            UserCollection.UsersList.Add(newUser);

            Console.WriteLine("User created: " + newUser.Login);
        }

        public Task<Pagination<User>> GetAsyncAllUser(int index, int count)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsyncUserByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
