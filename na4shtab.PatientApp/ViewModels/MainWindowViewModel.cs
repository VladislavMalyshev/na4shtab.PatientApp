using ReactiveUI;
using System.Reactive;

namespace na4shtab.PatientApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _current = new PatientListViewModel();
    public ViewModelBase Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }

    public ReactiveCommand<Unit, Unit> ShowPatientsCmd { get; }
    public ReactiveCommand<Unit, Unit> ShowProceduresCmd { get; }
    public ReactiveCommand<Unit, Unit> ShowVisitsCmd { get; }

    public MainWindowViewModel()
    {
        ShowPatientsCmd = ReactiveCommand.Create(() => { Current = new PatientListViewModel(); });
        ShowProceduresCmd = ReactiveCommand.Create(() => { Current = new ProcedureListViewModel(); });
        ShowVisitsCmd = ReactiveCommand.Create(() => { Current = new VisitListViewModel(); });
    }
}

