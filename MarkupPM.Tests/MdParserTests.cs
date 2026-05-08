using MarkupPM.Models;
using MarkupPM.Services;
using Xunit;

namespace MarkupPM.Tests;

public class MdParserTests
{
    private readonly MdParser _sut = new();

    [Fact]
    public void Parse_ProjectTitle_Extracted()
    {
        var p = _sut.Parse("# Mi Proyecto\n");
        Assert.Equal("Mi Proyecto", p.Nombre);
    }

    [Fact]
    public void Parse_WithAiHeader_ProjectTitleStillExtracted()
    {
        var md = """
            <!--
            MarkupPM AI instructions:
            - Keep format stable.
            -->

            # Mi Proyecto
            """;

        var p = _sut.Parse(md);
        Assert.Equal("Mi Proyecto", p.Nombre);
    }

    [Fact]
    public void Parse_Phase_NameAndIdExtracted()
    {
        var md = """
            # P

            ## Planificación
            <!-- phase-id: f001 -->
            """;
        var p = _sut.Parse(md);
        Assert.Single(p.Fases);
        Assert.Equal("Planificación", p.Fases[0].Nombre);
        Assert.Equal("f001", p.Fases[0].Id);
    }

    [Fact]
    public void Parse_Task_AllFieldsExtracted()
    {
        var md = """
            # P

            ## F
            <!-- phase-id: f1 -->

            - [~] Revisar código  <!-- id:t001 prio:alta resp:Juan_Carlos fecha:2026-05-20 dep:t000 -->
              Nota línea 1
              Nota línea 2
            """;
        var p = _sut.Parse(md);
        var t = p.Fases[0].Tareas[0];
        Assert.Equal("Revisar código", t.Nombre);
        Assert.Equal(EstadoTarea.EnCurso, t.Estado);
        Assert.Equal(PrioridadTarea.Alta, t.Prioridad);
        Assert.Equal("Juan Carlos", t.Responsable);
        Assert.Equal(new DateOnly(2026, 5, 20), t.Fecha);
        Assert.Equal("t001", t.Id);
        Assert.Equal("t000", t.Dependencias[0]);
        Assert.Contains("Nota línea 1", t.Notas);
    }

    [Fact]
    public void Parse_TaskWithNoDate_NullFecha()
    {
        var md = "# P\n\n## F\n<!-- phase-id: f1 -->\n\n- [ ] Sin fecha  <!-- id:t1 prio:media resp: fecha: dep: -->\n";
        var t = _sut.Parse(md).Fases[0].Tareas[0];
        Assert.Null(t.Fecha);
    }

    [Fact]
    public void Parse_DoneTask_EstadoHecha()
    {
        var md = "# P\n\n## F\n<!-- phase-id: f1 -->\n\n- [x] Completada  <!-- id:t1 prio:baja resp: fecha: dep: -->\n";
        var t = _sut.Parse(md).Fases[0].Tareas[0];
        Assert.Equal(EstadoTarea.Hecha, t.Estado);
    }

    [Fact]
    public void Parse_InvalidPriority_DefaultsToMedia()
    {
        var md = "# P\n\n## F\n<!-- phase-id: f1 -->\n\n- [ ] T  <!-- id:t1 prio:desconocido resp: fecha: dep: -->\n";
        var t = _sut.Parse(md).Fases[0].Tareas[0];
        Assert.Equal(PrioridadTarea.Media, t.Prioridad);
    }

    [Fact]
    public void Parse_MultipleFases_AllExtracted()
    {
        var md = """
            # P

            ## F1
            <!-- phase-id: f1 -->

            ## F2
            <!-- phase-id: f2 -->
            """;
        var p = _sut.Parse(md);
        Assert.Equal(2, p.Fases.Count);
        Assert.Equal("F1", p.Fases[0].Nombre);
        Assert.Equal("F2", p.Fases[1].Nombre);
    }
}
