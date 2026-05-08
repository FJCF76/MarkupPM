using System.Windows;
using MarkupPM.Services;
using MarkupPM.ViewModels;

namespace MarkupPM;

public partial class App : Application
{
    public static IRecentFilesService RecentFiles { get; } = new RecentFilesService();

    private static string? _pendingFilePath;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var args = Environment.GetCommandLineArgs();
        if (args.Length > 1 && System.IO.File.Exists(args[1]))
        {
            // MainWindow hasn't been created yet (StartupUri runs after OnStartup),
            // so store the path and open it once the window is loaded.
            _pendingFilePath = args[1];
        }
    }

    public static void OpenPendingFile()
    {
        if (_pendingFilePath is null) return;
        var path = _pendingFilePath;
        _pendingFilePath = null;
        AbrirArchivoDirecto(path);
    }

    public static void AbrirArchivoDirecto(string filePath)
    {
        if (Current.MainWindow?.DataContext is MainViewModel vm)
        {
            vm.AbrirArchivo(filePath);
        }
    }
}
