using Avalonia.Controls;
using na4shtab.PatientApp.ViewModels;
using System;

namespace na4shtab.PatientApp.Views;

public partial class PatientEditWindow : Window
{
    public PatientEditWindow()
    {
        InitializeComponent();
        this.DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is PatientEditViewModel vm)
        {
            vm.SaveCommand.Subscribe(_ => Close(true));
            vm.CancelCommand.Subscribe(_ => Close(false));
        }
    }
}
