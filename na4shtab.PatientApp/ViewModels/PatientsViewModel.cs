using System;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

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
            var win = new Views.PatientEditWindow { DataContext = vm };
            await win.ShowDialog(GetMainWindow());
            await LoadPatientsAsync();
        }

        private async Task EditPatientAsync()
        {
            if (SelectedPatient == null)
                return;

            var vm = new PatientEditViewModel(SelectedPatient);
            var win = new Views.PatientEditWindow { DataContext = vm };
            await win.ShowDialog(GetMainWindow());
            await LoadPatientsAsync();
        }

        private static Avalonia.Controls.Window? GetMainWindow()
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
                return desktop.MainWindow;
            return null;
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
