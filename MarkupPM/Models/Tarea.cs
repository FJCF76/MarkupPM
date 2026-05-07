namespace MarkupPM.Models;

public enum EstadoTarea { Pendiente, EnCurso, Hecha }
public enum PrioridadTarea { Baja, Media, Alta }

public class Tarea
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Nombre { get; set; } = string.Empty;
    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;
    public PrioridadTarea Prioridad { get; set; } = PrioridadTarea.Media;
    public string Responsable { get; set; } = string.Empty;
    public DateOnly? Fecha { get; set; }
    public string Notas { get; set; } = string.Empty;
    public List<string> Dependencias { get; set; } = [];

    public Tarea Clone() => new()
    {
        Id = Id,
        Nombre = Nombre,
        Estado = Estado,
        Prioridad = Prioridad,
        Responsable = Responsable,
        Fecha = Fecha,
        Notas = Notas,
        Dependencias = [.. Dependencias]
    };

    public override bool Equals(object? obj) =>
        obj is Tarea t &&
        Id == t.Id && Nombre == t.Nombre && Estado == t.Estado &&
        Prioridad == t.Prioridad && Responsable == t.Responsable &&
        Fecha == t.Fecha && Notas == t.Notas &&
        Dependencias.SequenceEqual(t.Dependencias);

    public override int GetHashCode() => Id.GetHashCode();
}
