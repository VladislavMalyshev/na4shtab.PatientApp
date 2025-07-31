using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;

namespace na4shtab.PatientApp.ViewModels;

public class ProcedureListViewModel : ViewModelBase
{
    private readonly IProcedureService _procedureService;

    public ObservableCollection<Procedure> Procedures { get; } = new();

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchText, value);
            LoadProceduresCommand.Execute().Subscribe(_ => { });
        }
    }

    private Procedure? _selectedProcedure;
    public Procedure? SelectedProcedure
    {
        get => _selectedProcedure;
        set => this.RaiseAndSetIfChanged(ref _selectedProcedure, value);
    }

    public ReactiveCommand<Unit, Unit> LoadProceduresCommand { get; }
    public ReactiveCommand<Unit, Unit> AddProcedureCommand { get; }
    public ReactiveCommand<Unit, Unit> EditProcedureCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteProcedureCommand { get; }

    public ProcedureListViewModel()
    {
        _procedureService = Locator.Current.GetService<IProcedureService>();

        LoadProceduresCommand = ReactiveCommand.CreateFromTask(LoadProceduresAsync);
        AddProcedureCommand = ReactiveCommand.CreateFromTask(AddProcedureAsync);
        EditProcedureCommand = ReactiveCommand.CreateFromTask(EditProcedureAsync,
            this.WhenAnyValue(x => x.SelectedProcedure).Select(p => p != null));
        DeleteProcedureCommand = ReactiveCommand.CreateFromTask(DeleteProcedureAsync,
            this.WhenAnyValue(x => x.SelectedProcedure).Select(p => p != null));

        LoadProceduresCommand.Execute().Subscribe(_ => { });
    }

    private async Task LoadProceduresAsync()
    {
        var list = await _procedureService.GetAllAsync(SearchText);
        Procedures.Clear();
        foreach (var p in list)
            Procedures.Add(p);
    }

    private Task AddProcedureAsync()
    {
        // TODO: open ProcedureEdit dialog
        return Task.CompletedTask;
    }

    private Task EditProcedureAsync()
    {
        // TODO: open ProcedureEdit dialog
        return Task.CompletedTask;
    }

    private async Task DeleteProcedureAsync()
    {
        if (SelectedProcedure != null)
        {
            await _procedureService.DeleteAsync(SelectedProcedure.Id);
            await LoadProceduresAsync();
        }
    }
}
