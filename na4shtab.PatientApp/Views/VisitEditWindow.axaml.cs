using Avalonia.Controls;
using na4shtab.PatientApp.ViewModels;
using System;

namespace na4shtab.PatientApp.Views;

public partial class VisitEditWindow : Window
{
    public VisitEditWindow()
    {
        InitializeComponent();
        this.DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is VisitEditViewModel vm)
        {
            vm.SaveCommand.Subscribe(_ => Close(true));
            vm.CancelCommand.Subscribe(_ => Close(false));
        }
    }
}
