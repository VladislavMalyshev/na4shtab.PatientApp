using ReactiveUI;
using Splat;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;
using na4shtab.PatientApp.Services;

namespace na4shtab.PatientApp.ViewModels;

public class ProcedureEditViewModel : ViewModelBase
{
    private readonly IProcedureService _procedureService;
    private readonly bool _isNew;

    public int Id { get; }

    private string _name = string.Empty;
    [Required]
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    private decimal _cost;
    public decimal Cost
    {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public ProcedureEditViewModel(Procedure? procedure = null)
    {
        _procedureService = Locator.Current.GetService<IProcedureService>();

        if (procedure == null)
        {
            _isNew = true;
            Id = 0;
        }
        else
        {
            _isNew = false;
            Id = procedure.Id;
            Name = procedure.Name;
            Description = procedure.Description;
            Cost = procedure.Cost;
        }

        var canSave = this.WhenAnyValue(x => x.Name,
                                        name => !string.IsNullOrWhiteSpace(name));

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync, canSave);
        CancelCommand = ReactiveCommand.Create(() => { });
    }

    private async Task SaveAsync()
    {
        var model = new Procedure
        {
            Id = this.Id,
            Name = this.Name,
            Description = this.Description,
            Cost = this.Cost
        };

        if (_isNew)
            await _procedureService.AddAsync(model);
        else
            await _procedureService.UpdateAsync(model);
    }
}
