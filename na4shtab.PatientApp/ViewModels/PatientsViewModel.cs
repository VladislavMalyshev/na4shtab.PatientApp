using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;

namespace na4shtab.PatientApp.ViewModels
{
    public class PatientListViewModel : ViewModelBase
    {
        private readonly IPatientService _patientService;

        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
                LoadPatientsCommand.Execute().Subscribe();
            }
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set => this.RaiseAndSetIfChanged(ref _selectedPatient, value);
        }

        public ReactiveCommand<Unit, Unit> LoadPatientsCommand  { get; }
        public ReactiveCommand<Unit, Unit> AddPatientCommand    { get; }
        public ReactiveCommand<Unit, Unit> EditPatientCommand   { get; }
        public ReactiveCommand<Unit, Unit> DeletePatientCommand { get; }

        public PatientListViewModel()
        {
            _patientService = Locator.Current.GetService<IPatientService>();

            LoadPatientsCommand = ReactiveCommand.CreateFromTask(LoadPatientsAsync);
            AddPatientCommand   = ReactiveCommand.CreateFromTask(AddPatientAsync);
            EditPatientCommand  = ReactiveCommand.CreateFromTask(EditPatientAsync,
                                        this.WhenAnyValue(x => x.SelectedPatient).Select(p => p != null));
            DeletePatientCommand = ReactiveCommand.CreateFromTask(DeletePatientAsync,
                                        this.WhenAnyValue(x => x.SelectedPatient).Select(p => p != null));

            LoadPatientsCommand.Execute().Subscribe();
        }

        private async Task LoadPatientsAsync()
        {
            var list = await _patientService.GetAllAsync(SearchText);
            Patients.Clear();
            foreach (var p in list)
                Patients.Add(p);
        }

        private async Task AddPatientAsync()
        {
            var vm = new PatientEditViewModel();
            await ShowDialogAsync(vm);
            await LoadPatientsAsync();
        }

        private async Task EditPatientAsync()
        {
            if (SelectedPatient == null)
                return;

            var vm = new PatientEditViewModel(SelectedPatient);
            await ShowDialogAsync(vm);
            await LoadPatientsAsync();
        }

        private async Task ShowDialogAsync(PatientEditViewModel vm)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var window = new Views.PatientEditWindow { DataContext = vm };
                await window.ShowDialog(desktop.MainWindow);
            }
        }

        private async Task DeletePatientAsync()
        {
            if (SelectedPatient != null)
            {
                await _patientService.DeleteAsync(SelectedPatient.Id);
                await LoadPatientsAsync();
            }
        }
    }
}
