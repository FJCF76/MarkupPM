namespace MarkupPM.Models;

public class Fase
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Nombre { get; set; } = string.Empty;
    public List<Tarea> Tareas { get; set; } = [];

    public Fase Clone() => new()
    {
        Id = Id,
        Nombre = Nombre,
        Tareas = Tareas.Select(t => t.Clone()).ToList()
    };

    public override bool Equals(object? obj) =>
        obj is Fase f &&
        Id == f.Id && Nombre == f.Nombre &&
        Tareas.SequenceEqual(f.Tareas);

    public override int GetHashCode() => Id.GetHashCode();
}
