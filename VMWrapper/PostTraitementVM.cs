using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMWrapper
{
    public partial class PostTraitementVM : ObservableObject, IEquatable<PostTraitementVM>
    {
        [ObservableProperty]
        private PostTraitement postTraitementModel;

        public PostTraitementVM(PostTraitement postTraitementModel)
        {
            PostTraitementModel = postTraitementModel ?? throw new ArgumentNullException(nameof(postTraitementModel));

            TypeOfTest = postTraitementModel.TypeOfTest;
            Machine = new MachineVM(postTraitementModel.Machine);
            Customer = new CustomerVM(postTraitementModel.Customer);
            Laboratoire = new LaboratoryVM(postTraitementModel.Laboratory);
            User = new UserVM(postTraitementModel.User);
            Temperature = postTraitementModel.Temperature;
            SensorType = postTraitementModel.SensorType;
            CertificatReference = postTraitementModel.CertificatReference;
        }

        public PostTraitementVM()
        {
            TypeOfTest = string.Empty;
            PostTraitementModel = new PostTraitement();
            Machine = new MachineVM();
            Customer = new CustomerVM();
            Laboratoire = new LaboratoryVM();
            User = new UserVM();
            Temperature = 0;
            SensorType = string.Empty;
            CertificatReference = string.Empty;
        }

        [ObservableProperty]
        private string typeOfTest;
        partial void OnTypeOfTestChanged(string value)
        {
            PostTraitementModel.TypeOfTest = value;
        }

        [ObservableProperty]
        private MachineVM machine;
        partial void OnMachineChanged(MachineVM value)
        {
            PostTraitementModel.Machine = value.MachineModel;
        }

        [ObservableProperty]
        private CustomerVM customer;
        partial void OnCustomerChanged(CustomerVM value)
        {
            PostTraitementModel.Customer = value.CustomerModel;
        }

        [ObservableProperty]
        private LaboratoryVM laboratoire;
        partial void OnLaboratoireChanged(LaboratoryVM value)
        {
            PostTraitementModel.Laboratory = value.LaboratoryModel;
        }

        [ObservableProperty]
        private UserVM user;
        partial void OnUserChanged(UserVM value)
        {
            PostTraitementModel.User = value.UserModel;
        }

        [ObservableProperty]
        private double? temperature;
        partial void OnTemperatureChanged(double? value)
        {
            PostTraitementModel.Temperature = value;
        }

        [ObservableProperty]
        private string sensorType;
        partial void OnSensorTypeChanged(string value)
        {
            PostTraitementModel.SensorType = value;
        }

        [ObservableProperty]
        private string certificatReference;
        partial void OnCertificatReferenceChanged(string value)
        {
            PostTraitementModel.CertificatReference = value;
        }

        public bool Equals(PostTraitementVM? other)
        {
            return string.IsNullOrEmpty(other?.TypeOfTest) ? false : other.TypeOfTest.Equals(PostTraitementModel.TypeOfTest);
        }
    }
}
