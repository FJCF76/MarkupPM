using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarkupPM.Models;

namespace MarkupPM.ViewModels;

public partial class FaseViewModel : ObservableObject
{
    private readonly Fase _model;
    public Fase Model => _model;
    public string Id => _model.Id;

    [ObservableProperty] private string _nombre;
    [ObservableProperty] private bool _isExpanded = true;
    [ObservableProperty] private bool _isEditingName;
    [ObservableProperty] private bool _isFilterSelected;

    public ObservableCollection<TareaViewModel> Tareas { get; }

    public string TaskCountLabel => Tareas.Count == 1 ? "1 tarea" : $"{Tareas.Count} tareas";

    public FaseViewModel(Fase model)
    {
        _model = model;
        _nombre = model.Nombre;
        Tareas = new ObservableCollection<TareaViewModel>(
            model.Tareas.Select(t => new TareaViewModel(t)));
        Tareas.CollectionChanged += (_, _) => OnPropertyChanged(nameof(TaskCountLabel));
    }

    [RelayCommand]
    private void BeginRenaming() => IsEditingName = true;

    public void CommitRename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            Nombre = _model.Nombre; // revert
        }
        else
        {
            Nombre = newName.Trim();
            _model.Nombre = Nombre;
        }
        IsEditingName = false;
    }

    public TareaViewModel AddTarea(string nombre = "Nueva tarea")
    {
        var tarea = new Tarea { Nombre = nombre };
        _model.Tareas.Add(tarea);
        var vm = new TareaViewModel(tarea);
        Tareas.Add(vm);
        return vm;
    }

    public void RemoveTarea(TareaViewModel tareaVm)
    {
        _model.Tareas.Remove(tareaVm.Model);
        Tareas.Remove(tareaVm);
    }

    public void InsertTarea(TareaViewModel tareaVm, int index)
    {
        var clamped = Math.Clamp(index, 0, Tareas.Count);
        _model.Tareas.Insert(Math.Clamp(index, 0, _model.Tareas.Count), tareaVm.Model);
        Tareas.Insert(clamped, tareaVm);
    }

    public void SyncToModel()
    {
        _model.Nombre = Nombre;
        _model.Tareas = Tareas.Select(t => { t.CommitToModel(); return t.Model; }).ToList();
    }
}
