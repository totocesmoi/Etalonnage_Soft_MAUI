using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMWrapper;

namespace VMService
{
    public partial class LaboratoryServiceVM : ObservableObject, IServiceVM
    {
        private readonly Manager service;

        private int pageSize = 10; 
        private int currentPageIndex = 0;
        private bool hasMoreItems = true;


        private ObservableCollection<LaboratoryVM> laboratories = new ObservableCollection<LaboratoryVM>();
        public ReadOnlyObservableCollection<LaboratoryVM> Laboratories => new ReadOnlyObservableCollection<LaboratoryVM>(laboratories);


        private LaboratoryVM selectedLaboratory;
        public LaboratoryVM SelectedLaboratory
        {
            get => selectedLaboratory;
            set => SetProperty(ref selectedLaboratory, value);
        }



        public LaboratoryServiceVM(Manager service)
        {
            this.service = service;

            CreateCommands();
        }

        public void CreateCommands()
        {
            LoadLaboratories = new AsyncRelayCommand(LoadLaboratoriesAsync, CanLoadLaboratories);
            GetLaboratory = new AsyncRelayCommand<OptionCommand<object>>(GetLaboratoryAsync, CanGetLaboratory);
            CreateLaboratory = new AsyncRelayCommand<string>(CreateLaboratoryAsync, CanCreateCustomer);
            InsertLaboratory = new AsyncRelayCommand(InsertLaboratoryAsync, CanInsertLaboratory);
            UpdateLaboratory = new AsyncRelayCommand<LaboratoryVM>(UpdateLaboratoryAsync, CanUpdateLaboratory);
            DeleteLaboratory = new AsyncRelayCommand(DeleteLaboratoryAsync, CanDeleteLaboratory);
        }

        /// <summary>
        /// Commande pour charger les laboratoires
        /// </summary>
        public IAsyncRelayCommand LoadLaboratories { get; private set; }
        private async Task LoadLaboratoriesAsync()
        {
            await GetLaboratoriesAsync(currentPageIndex, pageSize);
            currentPageIndex += pageSize;
        }

        private async Task<IEnumerable<LaboratoryVM>> GetLaboratoriesAsync(int index, int count)
        {
            Pagination<Laboratory> paginationResult = await service.GetAllLaboratories(index, count);
            if (paginationResult.Items.Count() <= Laboratories.Count)
            {
                return Laboratories;
            }

            foreach (var laboratory in paginationResult.Items)
            {
                laboratories.Add(new LaboratoryVM(laboratory));
            }

            return Laboratories;
        }

        private bool CanLoadLaboratories() => service != null && hasMoreItems && service.CurrentUser != null;

        /// <summary>
        /// Commande pour récupérer un laboratoire
        /// </summary>
        public IAsyncRelayCommand GetLaboratory { get; private set; }
        private async Task GetLaboratoryAsync(OptionCommand<object> options)
        {
            try
            {
                var name = options[0] as string;
                var navigate = (bool)options[1];
                if (navigate)
                {
                    var pageName = options[2] as string;

                    if (name == null || pageName == null)
                        throw new ArgumentNullException("Missing required parameters");

                    var laboratory = await service.GetLaboratoryByName(name);
                    SelectedLaboratory = new LaboratoryVM(laboratory);

                    await service.Navigation.NavigateToAsync(pageName);
                }
                else
                {
                    if (name == null)
                        throw new ArgumentNullException("Missing required parameters");

                    var laboratory = await service.GetLaboratoryByName(name);
                    SelectedLaboratory = new LaboratoryVM(laboratory);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the user retrieval : {ex.Message}");
            }
        }

        private bool CanGetLaboratory(OptionCommand<object> options) => options != null && options.Parameters.Count().Equals(3);

        /// <summary>
        /// Commande pour créer un laboratoire vide et naviguer vers la page de création
        /// </summary>
        public IAsyncRelayCommand<string> CreateLaboratory { get; private set; }
        private async Task CreateLaboratoryAsync(string page)
        {
            SelectedLaboratory = new LaboratoryVM();
            if (!string.IsNullOrEmpty(page))
            {
                await service.Navigation.NavigateToAsync(page);
            }
        }

        private bool CanCreateCustomer(string page) => service != null && !string.IsNullOrEmpty(page) && service.CurrentUser!.UserRole == Role.Administrator;

        /// <summary>
        /// Commande pour insérer un laboratoire
        /// </summary>
        public IAsyncRelayCommand InsertLaboratory { get; private set; }
        private async Task<LaboratoryVM> InsertLaboratoryAsync()
        {
            if (await service.CreateLaboratory(selectedLaboratory.LaboratoryModel))
            {
                // Pour être sur que le SelectedUser contient bien les informations auto généré.
                selectedLaboratory = new LaboratoryVM(await service.GetLaboratoryByName(SelectedLaboratory.LaboratoryModel.Name));

                laboratories.Add(selectedLaboratory);
                await service.Navigation.GoBackAsync();
                return selectedLaboratory;
            }
            else
                throw new Exception("An error occured during the User creation");
        }

        private bool CanInsertLaboratory() => selectedLaboratory != null && !string.IsNullOrEmpty(selectedLaboratory.NameLaboratory);

        /// <summary>
        /// Commande pour mettre à jour un laboratoire
        /// </summary>
        public IAsyncRelayCommand UpdateLaboratory { get; private set; }
        private async Task UpdateLaboratoryAsync(LaboratoryVM laboratory)
        {
            try
            {
                var updatedLaboratory = await service.UpdateLaboratory(laboratory.LaboratoryModel);
                if (updatedLaboratory != null)
                {
                    // Remplace l'objet dans la collection
                    var index = laboratories.IndexOf(laboratory);
                    if (index >= 0)
                    {
                        laboratories[index] = new LaboratoryVM(updatedLaboratory);
                    }

                    // Recharge les données si nécessaire
                    await service.Navigation.GoBackAsync();
                }
                else
                {
                    throw new Exception("An error occurred during the Laboratory update");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during the laboratory update: {ex.Message}");
            }

        }

        private bool CanUpdateLaboratory(LaboratoryVM customer) => customer != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;

        /// <summary>
        /// Commande pour supprimer un utilisateur
        /// </summary>
        public IAsyncRelayCommand DeleteLaboratory { get; private set; }
        private async Task<bool> DeleteLaboratoryAsync()
        {
            try
            {
                if (await service.DeleteUser(SelectedLaboratory.LaboratoryModel.Name))
                {
                    laboratories.Remove(SelectedLaboratory);
                    SelectedLaboratory = null!;
                    await service.Navigation.GoBackAsync();
                    return true;
                }

                await Application.Current!.MainPage!.DisplayAlert("Erreur", "An error occured during the user deletion !", "OK");
                await service.Navigation.GoBackAsync();
                return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occured during the user deletion : {ex.Message}");
                return false;
            }

        }

        private bool CanDeleteLaboratory() => SelectedLaboratory != null && service.CurrentUser != null && service.CurrentUser.UserRole == Role.Administrator;
    }
}
