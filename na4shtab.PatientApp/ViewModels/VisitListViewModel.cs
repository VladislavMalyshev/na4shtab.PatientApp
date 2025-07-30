using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;

namespace na4shtab.PatientApp.ViewModels
{
    public class VisitListViewModel : ViewModelBase
    {
        private readonly IVisitService _visitService;

        public ObservableCollection<Visit> Visits { get; } = new ObservableCollection<Visit>();

        private int? _patientFilterId;
        public int? PatientFilterId
        {
            get => _patientFilterId;
            set
            {
                this.RaiseAndSetIfChanged(ref _patientFilterId, value);
                LoadVisitsCommand.Execute().Subscribe();
            }
        }

        private Visit _selectedVisit;
        public Visit SelectedVisit
        {
            get => _selectedVisit;
            set => this.RaiseAndSetIfChanged(ref _selectedVisit, value);
        }

        public ReactiveCommand<Unit, Unit> LoadVisitsCommand  { get; }
        public ReactiveCommand<Unit, Unit> AddVisitCommand    { get; }
        public ReactiveCommand<Unit, Unit> EditVisitCommand   { get; }
        public ReactiveCommand<Unit, Unit> DeleteVisitCommand { get; }

        public VisitListViewModel()
        {
            _visitService = Locator.Current.GetService<IVisitService>();

            LoadVisitsCommand = ReactiveCommand.CreateFromTask(LoadVisitsAsync);
            AddVisitCommand   = ReactiveCommand.CreateFromTask(AddVisitAsync);
            EditVisitCommand  = ReactiveCommand.CreateFromTask(EditVisitAsync,
                                      this.WhenAnyValue(x => x.SelectedVisit).Select(v => v != null));
            DeleteVisitCommand = ReactiveCommand.CreateFromTask(DeleteVisitAsync,
                                      this.WhenAnyValue(x => x.SelectedVisit).Select(v => v != null));

            LoadVisitsCommand.Execute().Subscribe();
        }

        private async Task LoadVisitsAsync()
        {
            var list = await _visitService.GetAllAsync(PatientFilterId);
            Visits.Clear();
            foreach (var v in list)
                Visits.Add(v);
        }

        private Task AddVisitAsync()
        {
            // TODO: открыть диалог VisitEditViewModel(null)
            return Task.CompletedTask;
        }

        private Task EditVisitAsync()
        {
            // TODO: открыть диалог VisitEditViewModel(SelectedVisit)
            return Task.CompletedTask;
        }

        private async Task DeleteVisitAsync()
        {
            if (SelectedVisit != null)
            {
                await _visitService.DeleteAsync(SelectedVisit.Id);
                await LoadVisitsAsync();
            }
        }
    }
}