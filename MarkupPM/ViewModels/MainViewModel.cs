using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using MarkupPM.Models;
using MarkupPM.Services;
using Microsoft.Win32;

namespace MarkupPM.ViewModels;

public partial class MainViewModel : ObservableObject, IDropTarget
{
    private readonly IMdParser _parser;
    private readonly IMdSerializer _serializer;
    private readonly IRecentFilesService _recentFiles;

    [ObservableProperty] private Proyecto? _proyecto;
    [ObservableProperty] private TareaViewModel? _selectedTarea;
    [ObservableProperty] private bool _isDirty;
    [ObservableProperty] private string? _filePath;
    [ObservableProperty] private bool _hasProyecto;
    [ObservableProperty] private bool _isEditingProjectName;

    public WelcomeViewModel WelcomeVm { get; }

    public ObservableCollection<FaseViewModel> Fases { get; } = [];

    public bool HasFases => Fases.Count > 0;

    public int TotalTareas => Fases.Sum(f => f.Tareas.Count);

    public string TituloVentana => HasProyecto
        ? $"{(IsDirty ? "* " : "")}{Proyecto?.Nombre ?? "Sin título"} — MarkupPM"
        : "MarkupPM";

    public string FileNameDisplay => FilePath is not null
        ? Path.GetFileName(FilePath)
        : "sin guardar";

    public MainViewModel(IMdParser parser, IMdSerializer serializer, IRecentFilesService recentFiles)
    {
        _parser = parser;
        _serializer = serializer;
        _recentFiles = recentFiles;

        WelcomeVm = new WelcomeViewModel(recentFiles)
        {
            OnNuevoProyecto = NuevoProyecto,
            OnAbrirArchivo = AbrirDialogo,
            OnOpenRecent = AbrirArchivo
        };

        Fases.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(HasFases));
            OnPropertyChanged(nameof(TotalTareas));
        };
    }

    [RelayCommand]
    public void NuevoProyecto()
    {
        if (!ConfirmarDescartarCambios()) return;
        Proyecto = new Proyecto { Nombre = "Nuevo Proyecto" };
        FilePath = null;
        Fases.Clear();
        HasProyecto = true;
        IsDirty = true;
        SelectedTarea = null;
        OnPropertyChanged(nameof(TituloVentana));
        OnPropertyChanged(nameof(FileNameDisplay));
    }

    [RelayCommand]
    public void AbrirDialogo()
    {
        if (!ConfirmarDescartarCambios()) return;
        var dlg = new OpenFileDialog
        {
            Filter = "Proyectos Markdown (*.md)|*.md|Todos los archivos (*.*)|*.*",
            Title = "Abrir proyecto MarkupPM"
        };
        if (dlg.ShowDialog() == true)
            AbrirArchivo(dlg.FileName);
    }

    public void AbrirArchivo(string path)
    {
        try
        {
            var md = File.ReadAllText(path);
            Proyecto = _parser.Parse(md);
            FilePath = path;
            _recentFiles.AddRecent(path);
            WelcomeVm.RefreshRecientes();
            RebuildFases();
            HasProyecto = true;
            IsDirty = false;
            SelectedTarea = null;
            OnPropertyChanged(nameof(TituloVentana));
            OnPropertyChanged(nameof(FileNameDisplay));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo abrir el archivo:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(HasProyecto))]
    public void Guardar()
    {
        if (FilePath is null)
            GuardarComo();
        else
            EscribirArchivo(FilePath);
    }

    [RelayCommand(CanExecute = nameof(HasProyecto))]
    public void GuardarComo()
    {
        var dlg = new SaveFileDialog
        {
            Filter = "Proyectos Markdown (*.md)|*.md",
            FileName = Proyecto?.Nombre ?? "proyecto",
            Title = "Guardar proyecto como"
        };
        if (dlg.ShowDialog() == true)
        {
            if (Proyecto is not null && string.IsNullOrEmpty(Proyecto.Nombre))
                Proyecto.Nombre = Path.GetFileNameWithoutExtension(dlg.FileName);
            EscribirArchivo(dlg.FileName);
        }
    }

    [RelayCommand]
    public void AddFase()
    {
        if (Proyecto is null) return;
        var fase = new Fase { Nombre = "Nueva fase" };
        Proyecto.Fases.Add(fase);
        var vm = new FaseViewModel(fase);
        vm.PropertyChanged += (_, _) => MarkDirty();
        vm.Tareas.CollectionChanged += (_, _) => OnPropertyChanged(nameof(TotalTareas));
        Fases.Add(vm);
        MarkDirty();
    }

    [RelayCommand]
    public void RemoveFase(FaseViewModel faseVm)
    {
        Proyecto?.Fases.Remove(faseVm.Model);
        Fases.Remove(faseVm);
        if (SelectedTarea is not null && faseVm.Tareas.Contains(SelectedTarea))
            SelectedTarea = null;
        MarkDirty();
    }

    [RelayCommand]
    public void SelectTarea(TareaViewModel? tarea) => SelectedTarea = tarea;

    private void OnSelectedTareaPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(TareaViewModel.IsSelected))
            MarkDirty();
    }

    [RelayCommand]
    public void ClosePanel() => SelectedTarea = null;

    [RelayCommand]
    public void BeginRenamingProject() => IsEditingProjectName = true;

    public void CommitProjectRename(string newName)
    {
        if (Proyecto is null) return;
        if (string.IsNullOrWhiteSpace(newName))
            OnPropertyChanged(nameof(Proyecto));
        else
        {
            Proyecto.Nombre = newName.Trim();
            MarkDirty();
        }
        IsEditingProjectName = false;
        OnPropertyChanged(nameof(TituloVentana));
    }

    [RelayCommand]
    public void AddTareaToFase(FaseViewModel faseVm)
    {
        var tareaVm = faseVm.AddTarea();
        faseVm.IsExpanded = true;
        SelectedTarea = tareaVm;
        MarkDirty();
    }

    [RelayCommand]
    public void DeleteSelectedTarea()
    {
        if (SelectedTarea is null) return;
        var fase = Fases.FirstOrDefault(f => f.Tareas.Contains(SelectedTarea));
        if (fase is null) return;

        var result = MessageBox.Show(
            $"¿Eliminar la tarea \"{SelectedTarea.Nombre}\"?",
            "Confirmar eliminación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            fase.RemoveTarea(SelectedTarea);
            SelectedTarea = null;
            MarkDirty();
        }
    }

    public void MarkDirty()
    {
        IsDirty = true;
        OnPropertyChanged(nameof(TituloVentana));
    }

    public bool ConfirmarDescartarCambios()
    {
        if (!IsDirty) return true;
        var r = MessageBox.Show(
            "Hay cambios sin guardar. ¿Descartar y continuar?",
            "Cambios sin guardar",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        return r == MessageBoxResult.Yes;
    }

    private void RebuildFases()
    {
        Fases.Clear();
        foreach (var fase in Proyecto?.Fases ?? [])
        {
            var vm = new FaseViewModel(fase);
            vm.PropertyChanged += (_, _) => MarkDirty();
            vm.Tareas.CollectionChanged += (_, _) => OnPropertyChanged(nameof(TotalTareas));
            Fases.Add(vm);
        }
    }

    private void EscribirArchivo(string path)
    {
        try
        {
            SyncModelFromViewModels();
            var md = _serializer.Serialize(Proyecto!);
            File.WriteAllText(path, md);
            FilePath = path;
            _recentFiles.AddRecent(path);
            WelcomeVm.RefreshRecientes();
            IsDirty = false;
            OnPropertyChanged(nameof(TituloVentana));
            OnPropertyChanged(nameof(FileNameDisplay));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SyncModelFromViewModels()
    {
        if (Proyecto is null) return;
        foreach (var fvm in Fases)
            fvm.SyncToModel();
    }

    partial void OnSelectedTareaChanged(TareaViewModel? oldValue, TareaViewModel? newValue)
    {
        if (oldValue is not null)
        {
            oldValue.PropertyChanged -= OnSelectedTareaPropertyChanged;
            oldValue.IsSelected = false;
        }
        if (newValue is not null)
        {
            newValue.PropertyChanged += OnSelectedTareaPropertyChanged;
            newValue.IsSelected = true;
        }
        OnPropertyChanged(nameof(TituloVentana));
    }

    partial void OnIsDirtyChanged(bool value)
    {
        OnPropertyChanged(nameof(TituloVentana));
        GuardarCommand.NotifyCanExecuteChanged();
        GuardarComoCommand.NotifyCanExecuteChanged();
    }

    public void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is TareaViewModel)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not TareaViewModel tarea) return;

        var sourceFase = Fases.FirstOrDefault(f => f.Tareas.Contains(tarea));
        if (sourceFase is null) return;

        var targetFase = Fases.FirstOrDefault(f =>
            dropInfo.TargetCollection is ObservableCollection<TareaViewModel> col &&
            ReferenceEquals(f.Tareas, col)) ?? sourceFase;

        var insertIndex = Math.Clamp(dropInfo.InsertIndex, 0, targetFase.Tareas.Count);

        if (ReferenceEquals(sourceFase, targetFase))
        {
            var current = sourceFase.Tareas.IndexOf(tarea);
            if (current < 0 || current == insertIndex || current == insertIndex - 1) return;
            if (current < insertIndex) insertIndex--;
            sourceFase.Tareas.Move(current, Math.Clamp(insertIndex, 0, sourceFase.Tareas.Count - 1));
            sourceFase.Model.Tareas = [.. sourceFase.Tareas.Select(t => t.Model)];
        }
        else
        {
            sourceFase.RemoveTarea(tarea);
            targetFase.InsertTarea(tarea, insertIndex);
        }

        MarkDirty();
    }
}
