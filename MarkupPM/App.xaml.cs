using System.Windows;
using MarkupPM.Services;
using MarkupPM.ViewModels;

namespace MarkupPM;

public partial class App : Application
{
    public static IRecentFilesService RecentFiles { get; } = new RecentFilesService();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var args = Environment.GetCommandLineArgs();
        if (args.Length > 1 && System.IO.File.Exists(args[1]))
            AbrirArchivoDirecto(args[1]);
    }

    public static void AbrirArchivoDirecto(string filePath)
    {
        // MainViewModel is set as DataContext of MainWindow; retrieve it from the main window
        if (Current.MainWindow?.DataContext is MainViewModel vm)
        {
            vm.AbrirArchivo(filePath);
        }
    }
}
