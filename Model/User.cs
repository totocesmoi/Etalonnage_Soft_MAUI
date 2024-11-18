using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Model
{
    /// <summary>
    /// Liste des différents role existant
    /// </summary>
    public enum Role
    {
        Administrator,
        MeasurementExpert,
        Operator,
    }

    /// <summary>
    /// Liste des différentes actions possible dans le logiciel
    /// </summary>
    public enum Action
    {
        DataPostTraitement,
        UserManagement,
        ClientManagement,
        GenerateCertificat,
    }

    /// <summary>
    /// Classe Utilisateurs pour se loger et gestion des actions du soft Etalonnage
    /// </summary>
    [Serializable]
    public  class User
    {
        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Prénom de l'utilisateurs
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Identifiant de l'utilisateur
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Permet de hacher le mot de passe
        /// </summary>
        public byte[] Sel { get; set; }

        /// <summary>
        /// Role de l'utilisateur
        /// </summary>
        public Role UserRole { get; set; }

        /// <summary>
        /// Nom apparant lors de la signature
        /// </summary>
        public string SignatureName { get; set; }

        /// <summary>
        /// Image pour la signature de l'utilisateur
        /// </summary>
        public Picture Signature { get; set; }

        public List<Action> ActionsList => actionsList;
        /// <summary>
        /// Ensemble des tâches possible par utilisateur
        /// </summary>
        private readonly List<Action> actionsList;

        /// <summary>
        /// Constructeur par défaut pour la sérialisation
        /// </summary>
        public User()
        {
            UserRole = Role.Operator;
            actionsList = new List<Action>();
            Signature = new Picture();
        }

        /// <summary>
        /// Constructeur détaillé
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        public User(string nom, string prenom, string mdp, Role role = Role.Operator, Picture picture = null!, string signatureName = "")
        {
            Name = nom;
            Surname = prenom;
            Login = GenerateLogin(prenom, nom);
            Password = mdp;
            UserRole = role;
            actionsList = new List<Action>();
            Signature = picture ?? new Picture();
            SignatureName = signatureName;

            switch (UserRole)
            {
                case Role.Administrator:
                    actionsList = GetAdminActions();
                    break;
                case Role.MeasurementExpert:
                    actionsList = GetMeasurementExpertActions();
                    break;
                case Role.Operator:
                    actionsList = GetOperatorActions();
                    break;
            }
        }

        /// <summary>
        /// Permet la génération automatique du login
        /// </summary>
        /// <param name="prenom"></param>
        /// <param name="nom"></param>
        /// <returns> login : string </returns>
        public string GenerateLogin(string prenom, string nom)
        {
            string prefix = prenom.Substring(0, 2).ToLower();
            return prefix + nom.ToLower();
        }

        /// <summary>
        /// Obtenir toutes les actions administrateur
        /// </summary>
        /// <returns></returns>
        public List<Action> GetAdminActions()
        {
            return new List<Action>((Action[])Enum.GetValues(typeof(Action)));
        }

        /// <summary>
        /// Obtenir toutes les actions de l'expert mesure
        /// </summary>
        /// <returns></returns>
        public List<Action> GetMeasurementExpertActions()
        {
            return new List<Action> { Action.DataPostTraitement, Action.GenerateCertificat };
        }

        /// <summary>
        /// Obtenir toutes les actions de l'opérateur
        /// </summary>
        /// <returns></returns>
        public List<Action> GetOperatorActions()
        {
            return new List<Action> { Action.DataPostTraitement };
        }

        public override string ToString()
        {
            return $"nom : {Name} \nprénom : {Surname} \nlogin : {Login}";
        }
    }
}
