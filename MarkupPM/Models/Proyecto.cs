namespace MarkupPM.Models;

public class Proyecto
{
    public string Nombre { get; set; } = string.Empty;
    public List<Fase> Fases { get; set; } = [];

    public Proyecto Clone() => new()
    {
        Nombre = Nombre,
        Fases = Fases.Select(f => f.Clone()).ToList()
    };

    public override bool Equals(object? obj) =>
        obj is Proyecto p &&
        Nombre == p.Nombre &&
        Fases.SequenceEqual(p.Fases);

    public override int GetHashCode() => Nombre.GetHashCode();
}
