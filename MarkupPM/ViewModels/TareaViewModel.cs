using CommunityToolkit.Mvvm.ComponentModel;
using MarkupPM.Models;

namespace MarkupPM.ViewModels;

public partial class TareaViewModel : ObservableObject
{
    private readonly Tarea _model;
    public Tarea Model => _model;

    [ObservableProperty] private string _nombre;
    [ObservableProperty] private EstadoTarea _estado;
    [ObservableProperty] private PrioridadTarea _prioridad;
    [ObservableProperty] private string _responsable;
    [ObservableProperty] private DateOnly? _fecha;
    [ObservableProperty] private string _notas;
    [ObservableProperty] private List<string> _dependencias;
    [ObservableProperty] private bool _isSelected;

    public string Id => _model.Id;

    public string Iniciales => string.IsNullOrWhiteSpace(Responsable)
        ? "—"
        : string.Concat(Responsable.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(2).Select(w => char.ToUpperInvariant(w[0])));

    public bool FechaVencida => Fecha.HasValue && Fecha.Value < DateOnly.FromDateTime(DateTime.Today);

    /// <summary>Bridge property for WPF DatePicker which works with DateTime?.</summary>
    public DateTime? FechaAsDateTime
    {
        get => Fecha.HasValue ? Fecha.Value.ToDateTime(TimeOnly.MinValue) : null;
        set
        {
            Fecha = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
            OnPropertyChanged(nameof(FechaAsDateTime));
            OnPropertyChanged(nameof(FechaDisplay));
            OnPropertyChanged(nameof(FechaVencida));
        }
    }

    public string FechaDisplay => Fecha?.ToString("d MMM") ?? "—";

    public TareaViewModel(Tarea model)
    {
        _model = model;
        _nombre = model.Nombre;
        _estado = model.Estado;
        _prioridad = model.Prioridad;
        _responsable = model.Responsable;
        _fecha = model.Fecha;
        _notas = model.Notas;
        _dependencias = [.. model.Dependencias];
    }

    public void CommitToModel()
    {
        _model.Nombre = Nombre;
        _model.Estado = Estado;
        _model.Prioridad = Prioridad;
        _model.Responsable = Responsable;
        _model.Fecha = Fecha;
        _model.Notas = Notas;
        _model.Dependencias = [.. Dependencias];
    }
}
