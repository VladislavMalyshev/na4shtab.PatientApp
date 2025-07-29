using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Microsoft.EntityFrameworkCore;
using na4shtab.PatientApp.Data;

namespace na4shtab.PatientApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new PatientDbContext())
            {
                db.Database.Migrate();
            }

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}