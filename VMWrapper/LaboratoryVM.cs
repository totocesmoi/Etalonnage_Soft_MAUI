using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMWrapper
{
    public partial class LaboratoryVM : ObservableObject, IEquatable<LaboratoryVM>
    {
        [ObservableProperty]
        private Laboratory laboratoryModel;

        public LaboratoryVM()
        {
            LaboratoryModel = new Laboratory();
            Name = "";
            Adress = "";
            LaboritoryLocation = "";
            CachetEntreprise = new Picture();
        }

        public LaboratoryVM(Laboratory laboratoryModel)
        {
            LaboratoryModel = laboratoryModel ?? throw new ArgumentNullException(nameof(laboratoryModel));
            Name = laboratoryModel.Name;
            Adress = laboratoryModel.Adress;
            LaboritoryLocation = laboratoryModel.LaboritoryLocation;
            CachetEntreprise = laboratoryModel.CachetEntreprise;
        }

        [ObservableProperty]
        private string name;

        partial void OnNameChanged(string value)
        {
            LaboratoryModel.Name = value;
        }

        [ObservableProperty]
        private string adress;

        partial void OnAdressChanged(string value)
        {
            LaboratoryModel.Adress = value;
        }

        [ObservableProperty]
        private string laboritoryLocation;

        partial void OnLaboritoryLocationChanged(string value)
        {
            LaboratoryModel.LaboritoryLocation = value;
        }

        [ObservableProperty]
        private Picture cachetEntreprise;

        partial void OnCachetEntrepriseChanged(Picture value)
        {
            LaboratoryModel.CachetEntreprise = value;
        }

        public bool Equals(LaboratoryVM? other)
        {
            return string.IsNullOrEmpty(other?.Name) ? false : other.Name.Equals(LaboratoryModel.Name);        
        }
    }
}
