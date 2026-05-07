using System.Text.RegularExpressions;
using MarkupPM.Models;

namespace MarkupPM.Services;

public class MdParser : IMdParser
{
    // <!-- phase-id: abc123 -->
    private static readonly Regex PhaseIdRx = new(@"<!--\s*phase-id:\s*(\S+)\s*-->", RegexOptions.Compiled);
    // - [ ] Task name  <!-- id:abc prio:media resp:JC fecha:2026-05-01 dep:x1,x2 -->
    private static readonly Regex TaskRx = new(
        @"^- (\[[ ~x]\]) (.+?)  <!--\s*id:(\S+)\s+prio:(\S+)\s+resp:(\S*)\s+fecha:(\S*)\s+dep:(\S*)\s*-->",
        RegexOptions.Compiled);

    public Proyecto Parse(string markdown)
    {
        var proyecto = new Proyecto();
        Fase? currentFase = null;
        Tarea? currentTarea = null;
        var notesLines = new List<string>();

        foreach (var rawLine in markdown.Split('\n'))
        {
            var line = rawLine.TrimEnd('\r');

            // Project title
            if (line.StartsWith("# ") && proyecto.Nombre == string.Empty)
            {
                proyecto.Nombre = line[2..].Trim();
                continue;
            }

            // Phase header
            if (line.StartsWith("## "))
            {
                FlushNotes(currentTarea, notesLines);
                currentTarea = null;
                currentFase = new Fase { Nombre = line[3..].Trim() };
                proyecto.Fases.Add(currentFase);
                continue;
            }

            // Phase id comment
            var phaseMatch = PhaseIdRx.Match(line);
            if (phaseMatch.Success && currentFase is not null)
            {
                currentFase.Id = phaseMatch.Groups[1].Value;
                continue;
            }

            // Task line
            var taskMatch = TaskRx.Match(line);
            if (taskMatch.Success && currentFase is not null)
            {
                FlushNotes(currentTarea, notesLines);

                currentTarea = new Tarea
                {
                    Estado = taskMatch.Groups[1].Value switch
                    {
                        "[~]" => EstadoTarea.EnCurso,
                        "[x]" => EstadoTarea.Hecha,
                        _ => EstadoTarea.Pendiente
                    },
                    Nombre = taskMatch.Groups[2].Value.Trim(),
                    Id = taskMatch.Groups[3].Value,
                    Prioridad = taskMatch.Groups[4].Value switch
                    {
                        "alta" => PrioridadTarea.Alta,
                        "baja" => PrioridadTarea.Baja,
                        _ => PrioridadTarea.Media
                    },
                    Responsable = taskMatch.Groups[5].Value.Replace("_", " ").Trim(),
                    Fecha = ParseDate(taskMatch.Groups[6].Value),
                    Dependencias = ParseDeps(taskMatch.Groups[7].Value)
                };
                currentFase.Tareas.Add(currentTarea);
                notesLines.Clear();
                continue;
            }

            // Notes indented under a task
            if (currentTarea is not null && (line.StartsWith("  ") || line == string.Empty))
            {
                notesLines.Add(line.Length >= 2 ? line[2..] : string.Empty);
            }
        }

        FlushNotes(currentTarea, notesLines);
        return proyecto;
    }

    private static void FlushNotes(Tarea? tarea, List<string> lines)
    {
        if (tarea is null || lines.Count == 0) return;
        tarea.Notas = string.Join("\n", lines).Trim();
        lines.Clear();
    }

    private static DateOnly? ParseDate(string s)
    {
        if (string.IsNullOrEmpty(s)) return null;
        return DateOnly.TryParse(s, out var d) ? d : null;
    }

    private static List<string> ParseDeps(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return [];
        return [.. s.Split(',', StringSplitOptions.RemoveEmptyEntries)];
    }
}
