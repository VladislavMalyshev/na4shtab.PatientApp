using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData.Binding;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;

namespace na4shtab.PatientApp.ViewModels
{
    public class VisitEditViewModel : ViewModelBase
    {
        private readonly IVisitService _visitService;
        private readonly IProcedureService _procedureService;
        private readonly bool _isNew;

        public int Id { get; }
        public int PatientId { get; }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                this.RaiseAndSetIfChanged(ref _date, value);
            }
        }

        public ObservableCollection<Procedure> AvailableProcedures { get; } = new ObservableCollection<Procedure>();
        public ObservableCollection<Procedure> SelectedProcedures { get; } = new ObservableCollection<Procedure>();

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            set => this.RaiseAndSetIfChanged(ref _totalCost, value);
        }

        public ReactiveCommand<Procedure, Unit> ToggleProcedureCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand           { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand         { get; }

        public VisitEditViewModel(Visit visit = null, int patientId = 0)
        {
            _visitService     = Locator.Current.GetService<IVisitService>();
            _procedureService = Locator.Current.GetService<IProcedureService>();

            if (visit == null)
            {
                _isNew     = true;
                Id         = 0;
                PatientId  = patientId;
                Date       = DateTime.Now;
                TotalCost  = 0;
            }
            else
            {
                _isNew     = false;
                Id         = visit.Id;
                PatientId  = visit.PatientId;
                Date       = visit.Date;
                TotalCost  = visit.TotalCost;
                foreach (var p in visit.SelectedProcedures)
                    SelectedProcedures.Add(p);
            }

            ToggleProcedureCommand = ReactiveCommand.Create<Procedure>(ToggleProcedure);
            SaveCommand            = ReactiveCommand.CreateFromTask(SaveAsync, 
                                        this.WhenAnyValue(vm => vm.Date).Select(_ => true));
            CancelCommand          = ReactiveCommand.Create(() => { /* TODO: закрыть */ });

            LoadAvailableProcedures();
            SelectedProcedures
                .ToObservableChangeSet()
                .Subscribe(_ => RecalculateCost());
        }

        private async void LoadAvailableProcedures()
        {
            var list = await _procedureService.GetAllAsync();
            foreach (var p in list)
                AvailableProcedures.Add(p);
        }

        private void ToggleProcedure(Procedure proc)
        {
            if (SelectedProcedures.Contains(proc))
                SelectedProcedures.Remove(proc);
            else
                SelectedProcedures.Add(proc);
        }

        private void RecalculateCost()
        {
            if (_isNew || TotalCost <= 0)
                TotalCost = SelectedProcedures.Sum(p => p.Cost);
        }

        private async Task SaveAsync()
        {
            var model = new Visit
            {
                Id                  = this.Id,
                PatientId           = this.PatientId,
                Date                = this.Date,
                SelectedProcedures  = SelectedProcedures.ToList(),
                TotalCost           = this.TotalCost
            };

            if (_isNew)
                await _visitService.AddAsync(model);
            else
                await _visitService.UpdateAsync(model);
            
        }
    }
}