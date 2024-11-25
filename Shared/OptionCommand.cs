using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class OptionCommand<T>
    where T : class
    {
        public IEnumerable<T> Parameters { get; }

        public OptionCommand(IEnumerable<T> parameters)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentException("At least one parameter is required.", nameof(parameters));

            Parameters = parameters;
        }

        /// <summary>
        /// Indexeur pour accéder directement aux paramètres par leur index
        /// </summary>
        public T this[int index] => Parameters.ElementAt(index);

        /// <summary>
        /// Méthode pour récupérer un paramètre spécifique si nécessaire
        /// </summary>
        public T? GetParameter(int index) => Parameters.ElementAtOrDefault(index);
    }

}
