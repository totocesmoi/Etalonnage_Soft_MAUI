using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using Shared;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VMWrapper;

namespace VMService
{
    public partial class MachineServiceVM : ObservableObject, IServiceVM
    {
        private readonly Manager service;

        // Variables pour la pagination des machines
        private int pageSize = 10;
        private int currentPageIndex = 0;
        private bool hasMoreItems = true;

        // Liste des machines récupérées
        private ObservableCollection<MachineVM> machines = new ObservableCollection<MachineVM>();
        public ReadOnlyObservableCollection<MachineVM> Machines => new ReadOnlyObservableCollection<MachineVM>(machines);

        // Machine sélectionnée
        private MachineVM selectedMachine;
        public MachineVM SelectedMachine
        {
            get => selectedMachine;
            set => SetProperty(ref selectedMachine, value);
        }

        // Commandes
        public IAsyncRelayCommand LoadMachines { get; private set; }
        public IAsyncRelayCommand<string> GetAMachine { get; private set; }
        public IAsyncRelayCommand<string> CreateMachine { get; private set; }
        public IAsyncRelayCommand InsertMachine { get; private set; }
        public IAsyncRelayCommand<MachineVM> UpdateMachine { get; private set; }
        public IAsyncRelayCommand DeleteMachine { get; private set; }

        // Constructeur du ViewModel
        public MachineServiceVM(Manager service)
        {
            this.service = service;
            CreateCommands();
        }

        public void CreateCommands()
        {
            LoadMachines = new AsyncRelayCommand(LoadMachinesAsync, CanLoadMachines);
            CreateMachine = new AsyncRelayCommand<string>(CreateMachineAsync, CanCreateMachine);
            InsertMachine = new AsyncRelayCommand(InsertMachineAsync, CanInsertMachine);
            UpdateMachine = new AsyncRelayCommand<MachineVM>(UpdateMachineAsync, CanUpdateMachine);
            DeleteMachine = new AsyncRelayCommand(DeleteMachineAsync, CanDeleteMachine);
        }

        // Charger les machines
        private async Task LoadMachinesAsync()
        {
            await GetMachinesAsync(currentPageIndex, pageSize);
            currentPageIndex += pageSize;
        }

        private async Task<IEnumerable<MachineVM>> GetMachinesAsync(int index, int count)
        {
            Pagination<Machine> paginationResult = await service.GetAllMachines(index, count);
            if (paginationResult.Items.Count() <= Machines.Count)
            {
                hasMoreItems = false;
                return Machines;
            }

            foreach (var machine in paginationResult.Items)
            {
                machines.Add(new MachineVM(machine));
            }

            return Machines;
        }

        private bool CanLoadMachines() => service.CurrentUser != null;

        // Récupérer une machine par référence
        private async Task GetMachineAsync(string reference)
        {
            try
            {
                if (string.IsNullOrEmpty(reference))
                    throw new ArgumentNullException(nameof(reference), "Reference cannot be null or empty.");

                var machine = await service.GetMachineByReference(reference);
                SelectedMachine = new MachineVM(machine);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while retrieving the machine: {ex.Message}");
            }
        }

        // Créer une nouvelle machine
        private async Task CreateMachineAsync(string page)
        {
            SelectedMachine = new MachineVM();
            if (!string.IsNullOrEmpty(page))
            {
                await service.Navigation.NavigateToAsync(page);
            }
        }

        private bool CanCreateMachine(string page)
        {
            return service != null && !string.IsNullOrEmpty(page);
        }

        // Insérer une nouvelle machine
        private async Task<MachineVM> InsertMachineAsync()
        {
            if (SelectedMachine != null)
            {
                await service.CreateMachine(SelectedMachine.MachineModel);
                machines.Add(SelectedMachine);
                await service.Navigation.GoBackAsync();
                return SelectedMachine;
            }
            throw new Exception("Error while creating the machine");
        }

        private bool CanInsertMachine() => SelectedMachine != null && !string.IsNullOrEmpty(SelectedMachine.Reference);

        // Mettre à jour une machine
        private async Task UpdateMachineAsync(MachineVM machine)
        {
            var updatedMachine = await service.UpdateMachine(machine.MachineModel);
            if (updatedMachine != null)
            {
                var index = machines.IndexOf(machine);
                if (index >= 0)
                {
                    machines[index] = new MachineVM(updatedMachine);
                }
                await service.Navigation.GoBackAsync();
            }
        }

        private bool CanUpdateMachine(MachineVM machine) => machine != null;

        // Supprimer une machine
        private async Task DeleteMachineAsync()
        {
            if (SelectedMachine != null)
            {
                await service.DeleteMachine(SelectedMachine.Reference);
                machines.Remove(SelectedMachine);
                await service.Navigation.GoBackAsync();
            }
        }
        private bool CanDeleteMachine() => SelectedMachine != null;
    }
}
