using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

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

    private async Task AddProcedureAsync()
    {
        var vm = new ProcedureEditViewModel();
        var win = new Views.ProcedureEditWindow { DataContext = vm };
        await win.ShowDialog(GetMainWindow());
        await LoadProceduresAsync();
    }

    private async Task EditProcedureAsync()
    {
        if (SelectedProcedure == null)
            return;

        var vm = new ProcedureEditViewModel(SelectedProcedure);
        var win = new Views.ProcedureEditWindow { DataContext = vm };
        await win.ShowDialog(GetMainWindow());
        await LoadProceduresAsync();
    }

    private static Avalonia.Controls.Window? GetMainWindow()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow;
        return null;
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
