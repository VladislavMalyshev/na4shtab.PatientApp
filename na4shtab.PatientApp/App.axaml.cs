using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Splat;
using na4shtab.PatientApp.Services;

namespace na4shtab.PatientApp
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            
            Locator.CurrentMutable.RegisterLazySingleton<IPatientService>(() => new PatientService());
            Locator.CurrentMutable.RegisterLazySingleton<IProcedureService>(() => new ProcedureService());
            Locator.CurrentMutable.RegisterLazySingleton<IVisitService>(()     => new VisitService());
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new Views.MainWindow
                {
                    DataContext = new ViewModels.MainWindowViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}