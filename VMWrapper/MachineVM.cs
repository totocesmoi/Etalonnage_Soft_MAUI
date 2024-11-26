using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace VMWrapper
{
    public partial class MachineVM: ObservableObject, IEquatable<MachineVM>
    {
        [ObservableProperty]
        private Machine machineModel;

        public MachineVM(Machine machineModel)
        {
            MachineModel = machineModel ?? throw new ArgumentNullException(nameof(machineModel));

            Reference = machineModel.Reference;
            Configuration = machineModel.Configuration;
            ReceptionDate = machineModel.ReceptionDate;
            Model = machineModel.Model;
            SerialNumber = machineModel.SerialNumber;
            Comment = machineModel.Comment;
        }

        public MachineVM()
        {
            MachineModel = new Machine();
            Reference = "";
            Configuration = "";
            ReceptionDate = DateTime.Now;
            Model = "";
            SerialNumber = "";
            Comment = "";
        }

        [ObservableProperty]
        private string reference;

        partial void OnReferenceChanged(string value)
        {
            MachineModel.Reference = value;
        }

        [ObservableProperty]
        private string configuration;

        partial void OnConfigurationChanged(string value)
        {
            MachineModel.Configuration = value;
        }

        [ObservableProperty]
        private DateTime receptionDate;

        partial void OnReceptionDateChanged(DateTime value)
        {
            MachineModel.ReceptionDate = value;
        }

        [ObservableProperty]
        private string model;

        partial void OnModelChanged(string value) 
        {
            MachineModel.Model = value;
        }

        [ObservableProperty]
        private string serialNumber;

        partial void OnSerialNumberChanged(string value)
        {
            MachineModel.SerialNumber = value;
        }

        [ObservableProperty]
        private string comment;

        partial void OnCommentChanged(string value)
        {
            MachineModel.Comment = value;
        }

        public bool Equals(MachineVM? other)
        {
            return string.IsNullOrEmpty(other?.Reference) ? false : other.Reference.Equals(MachineModel.Reference);
        }
    }
}
