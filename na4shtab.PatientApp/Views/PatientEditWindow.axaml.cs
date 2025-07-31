using System;
using Avalonia.Controls;
using ReactiveUI;
using na4shtab.PatientApp.ViewModels;

namespace na4shtab.PatientApp.Views;

public partial class PatientEditWindow : Window
{
    public PatientEditWindow()
    {
        InitializeComponent();
        this.DataContextChanged += (_, __) => RegisterHandlers();
    }

    private void RegisterHandlers()
    {
        if (DataContext is PatientEditViewModel vm)
        {
            vm.SaveCommand.Subscribe(_ => Close());
            vm.CancelCommand.Subscribe(_ => Close());
        }
    }
}
