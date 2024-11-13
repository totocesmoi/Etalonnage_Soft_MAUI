using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEtalonnageMultiPlateforme.Resources.Langue
{
    public class AppResourcesVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public System.Globalization.CultureInfo Culture
        {
            get => AppRessources.Culture;
            set
            {
                if (AppRessources.Culture == value) return;

                AppRessources.Culture = value;
                OnPropertyChanged("Culture");
            }
        }
    }
}
