namespace MarkupPM.Models;

/// <summary>Static lists of enum values for ComboBox ItemsSource binding in XAML.</summary>
public static class EstadoTareaValues
{
    public static EstadoTarea[] All { get; } = [EstadoTarea.Pendiente, EstadoTarea.EnCurso, EstadoTarea.Hecha];
}

public static class PrioridadTareaValues
{
    public static PrioridadTarea[] All { get; } = [PrioridadTarea.Baja, PrioridadTarea.Media, PrioridadTarea.Alta];
}
