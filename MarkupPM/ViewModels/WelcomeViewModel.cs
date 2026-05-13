using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarkupPM.Services;

namespace MarkupPM.ViewModels;

public partial class WelcomeViewModel : ObservableObject
{
    private readonly IRecentFilesService _recentFiles;
    public Action<string>? OnOpenRecent { get; set; }
    public Action? OnNuevoProyecto { get; set; }
    public Action? OnAbrirArchivo { get; set; }

    public ObservableCollection<string> Recientes { get; } = [];

    public bool HasRecientes => Recientes.Count > 0;

    public WelcomeViewModel(IRecentFilesService recentFiles)
    {
        _recentFiles = recentFiles;
        RefreshRecientes();
    }

    public void RefreshRecientes()
    {
        Recientes.Clear();
        foreach (var f in _recentFiles.GetRecent())
            Recientes.Add(f);
        OnPropertyChanged(nameof(HasRecientes));
    }

    [RelayCommand]
    private void NuevoProyecto() => OnNuevoProyecto?.Invoke();

    [RelayCommand]
    private void AbrirArchivo() => OnAbrirArchivo?.Invoke();

    [RelayCommand]
    private void AbrirReciente(string path) => OnOpenRecent?.Invoke(path);
}
