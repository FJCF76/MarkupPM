using System.Text;
using MarkupPM.Models;

namespace MarkupPM.Services;

/// <summary>
/// Serializes a Proyecto to Markdown. Format is stable and round-trip safe with MdParser.
/// </summary>
/// <remarks>
/// Format:
///   # {ProjectName}
///
///   ## {PhaseName}
///   <!-- phase-id: {id} -->
///
///   - [ ] {TaskName}  <!-- id:{id} prio:{prio} resp:{resp} fecha:{fecha} dep:{id1,id2} -->
///     {notes line 1}
///     {notes line 2}
/// </remarks>
public class MdSerializer : IMdSerializer
{
    private static readonly string[] EstadoToken = ["[ ]", "[~]", "[x]"];
    private static readonly string[] PrioridadToken = ["baja", "media", "alta"];

    public string Serialize(Proyecto proyecto)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# {EscapeInline(proyecto.Nombre)}");

        foreach (var fase in proyecto.Fases)
        {
            sb.AppendLine();
            sb.AppendLine($"## {EscapeInline(fase.Nombre)}");
            sb.AppendLine($"<!-- phase-id: {fase.Id} -->");

            foreach (var tarea in fase.Tareas)
            {
                var estado = EstadoToken[(int)tarea.Estado];
                var prio = PrioridadToken[(int)tarea.Prioridad];
                var resp = EscapeAttr(tarea.Responsable);
                var fecha = tarea.Fecha?.ToString("yyyy-MM-dd") ?? "";
                var deps = string.Join(",", tarea.Dependencias);

                sb.AppendLine();
                sb.AppendLine($"- {estado} {EscapeInline(tarea.Nombre)}  <!-- id:{tarea.Id} prio:{prio} resp:{resp} fecha:{fecha} dep:{deps} -->");

                if (!string.IsNullOrWhiteSpace(tarea.Notas))
                {
                    foreach (var line in tarea.Notas.Split('\n'))
                        sb.AppendLine($"  {line.TrimEnd()}");
                }
            }
        }

        return sb.ToString();
    }

    private static string EscapeInline(string s) => s.Replace("<!--", "&#x3C;!--");
    private static string EscapeAttr(string s) => s.Replace(" ", "_").Replace(">", "").Replace("<", "");
}
