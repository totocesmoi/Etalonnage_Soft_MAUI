using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
    }
}
