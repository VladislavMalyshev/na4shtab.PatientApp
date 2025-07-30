using ReactiveUI;
using Splat;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;

namespace na4shtab.PatientApp.ViewModels
{
    public class PatientEditViewModel : ViewModelBase
    {
        private readonly IPatientService _patientService;
        private readonly bool _isNew;

        public int Id { get; }

        private string _fullName;
        [Required]
        public string FullName
        {
            get => _fullName;
            set => this.RaiseAndSetIfChanged(ref _fullName, value);
        }

        private DateTime? _birthDate;
        public DateTime? BirthDate
        {
            get => _birthDate;
            set => this.RaiseAndSetIfChanged(ref _birthDate, value);
        }

        private string _contactInfo;
        public string ContactInfo
        {
            get => _contactInfo;
            set => this.RaiseAndSetIfChanged(ref _contactInfo, value);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand   { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public PatientEditViewModel(Patient patient = null)
        {
            _patientService = Locator.Current.GetService<IPatientService>();

            if (patient == null)
            {
                _isNew = true;
                Id = 0;
                FullName = "";
            }
            else
            {
                _isNew = false;
                Id = patient.Id;
                FullName   = patient.FullName;
                BirthDate  = patient.BirthDate;
                ContactInfo= patient.ContactInfo;
            }

            var canSave = this.WhenAnyValue(
                x => x.FullName,
                name => !string.IsNullOrWhiteSpace(name));

            SaveCommand   = ReactiveCommand.CreateFromTask(SaveAsync, canSave);
            CancelCommand = ReactiveCommand.Create(() => {});
        }

        private async Task SaveAsync()
        {
            var model = new Patient
            {
                Id          = this.Id,
                FullName    = this.FullName,
                BirthDate   = this.BirthDate,
                ContactInfo = this.ContactInfo
            };

            if (_isNew)
                await _patientService.AddAsync(model);
            else
                await _patientService.UpdateAsync(model);
            
        }
    }
}