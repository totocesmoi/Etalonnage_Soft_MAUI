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
            NameLaboratory = "";
            AdressLaboratory = "";
            LaboritoryLocation = "";
            CachetEntreprise = new Picture();
        }

        public LaboratoryVM(Laboratory laboratoryModel)
        {
            LaboratoryModel = laboratoryModel ?? throw new ArgumentNullException(nameof(laboratoryModel));
            NameLaboratory = laboratoryModel.Name;
            AdressLaboratory = laboratoryModel.Address;
            LaboritoryLocation = laboratoryModel.LaboritoryLocation;
            CachetEntreprise = laboratoryModel.CachetEntreprise ?? new Picture();
        }

        [ObservableProperty]
        private string nameLaboratory;
        partial void OnNameLaboratoryChanged(string value)
        {
            LaboratoryModel.Name = value;
        }

        [ObservableProperty]
        private string adressLaboratory;
        partial void OnAdressLaboratoryChanged(string value)
        {
            LaboratoryModel.Address = value;
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
            return string.IsNullOrEmpty(other?.NameLaboratory) ? false : other.NameLaboratory.Equals(LaboratoryModel.Name);        
        }
    }
}
