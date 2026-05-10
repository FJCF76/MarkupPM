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

    private static readonly string[] AiHeaderLines =
    [
        "<!--",
        "MarkupPM AI instructions:",
        "- Purpose: create or update a project plan in Markdown that can be parsed by MarkupPM without data loss.",
        "- Output only valid MarkupPM content. Do not add prose outside this format.",
        "- Required top-level order:",
        "  1) Project title line: # {ProjectName}",
        "  2) One or more phases, each using:",
        "     - Phase header: ## {PhaseName}",
        "     - Phase id comment on next line: <!-- phase-id: {id} -->",
        "     - Zero or more tasks under that phase",
        "- Use a clear, realistic project title and concrete phase names (not placeholders).",
        "- Every phase must have a stable unique id. Keep existing phase ids unchanged when editing.",
        "- Task line format (exact metadata keys and order):",
        "  - [ ] {TaskName}  <!-- id:{id} prio:{baja|media|alta} resp:{Name_With_Underscores} fecha:{yyyy-MM-dd|empty} dep:{id1,id2|empty} -->",
        "- Allowed task state tokens:",
        "  - [ ] pending, [~] in progress, [x] completed",
        "- Task authoring rules:",
        "  - Make task names actionable and specific (verb + outcome).",
        "  - Use responsible person in resp with underscores instead of spaces.",
        "  - Use fecha as yyyy-MM-dd or leave empty after fecha: if unknown.",
        "  - Use dep as comma-separated task ids with no spaces, or empty.",
        "  - Keep each task id unique across the whole file.",
        "  - Preserve existing task ids and dependency ids when editing.",
        "- Notes format:",
        "  - Optional notes go directly below the task line.",
        "  - Each note line must start with exactly two spaces.",
        "  - Keep notes concise and implementation-oriented.",
        "- Planning quality checklist for a full project plan:",
        "  - Include discovery/planning, implementation, testing, and release/closure phases when applicable.",
        "  - Break work into small, testable tasks with clear ownership.",
        "  - Add dependencies so execution order is explicit.",
        "  - Balance priorities: alta only for truly critical tasks.",
        "  - Prefer complete coverage over vague high-level bullets.",
        "- Validation before final output:",
        "  - Exactly one # title line.",
        "  - Every ## phase header is followed by its phase-id comment.",
        "  - Every task line contains id, prio, resp, fecha, dep metadata.",
        "  - No malformed comments and no missing ids.",
        "-->"
    ];

    public string Serialize(Proyecto proyecto)
    {
        var sb = new StringBuilder();

        foreach (var headerLine in AiHeaderLines)
            sb.AppendLine(headerLine);

        sb.AppendLine();
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

    private static string EscapeInline(string s) => s.Replace("<!--", "&#x3C;!--").Replace("-->", "--&#x3E;");
    private static string EscapeAttr(string s) => s.Replace(" ", "_").Replace(">", "").Replace("<", "");
}
