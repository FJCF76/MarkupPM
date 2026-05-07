using MarkupPM.Models;
using MarkupPM.Services;
using Xunit;

namespace MarkupPM.Tests;

public class RoundTripTests
{
    private readonly MdSerializer _serializer = new();
    private readonly MdParser _parser = new();

    private Proyecto RoundTrip(Proyecto p) => _parser.Parse(_serializer.Serialize(p));

    [Fact]
    public void RoundTrip_EmptyProject_Equal()
    {
        var p = new Proyecto { Nombre = "Vacío" };
        Assert.Equal(p, RoundTrip(p));
    }

    [Fact]
    public void RoundTrip_ProjectWithEmptyPhase_Equal()
    {
        var p = new Proyecto
        {
            Nombre = "Test",
            Fases = [new Fase { Id = "f1", Nombre = "Fase vacía" }]
        };
        Assert.Equal(p, RoundTrip(p));
    }

    [Fact]
    public void RoundTrip_FullTask_Equal()
    {
        var tarea = new Tarea
        {
            Id = "t001",
            Nombre = "Tarea completa",
            Estado = EstadoTarea.EnCurso,
            Prioridad = PrioridadTarea.Alta,
            Responsable = "Ana López",
            Fecha = new DateOnly(2026, 6, 15),
            Notas = "Línea uno\nLínea dos",
            Dependencias = ["t000", "t002"]
        };
        var p = new Proyecto
        {
            Nombre = "Proyecto Full",
            Fases = [new Fase { Id = "f1", Nombre = "F1", Tareas = [tarea] }]
        };
        Assert.Equal(p, RoundTrip(p));
    }

    [Fact]
    public void RoundTrip_MultiplePhasesTasks_Equal()
    {
        var p = new Proyecto
        {
            Nombre = "Multi",
            Fases =
            [
                new Fase
                {
                    Id = "fa",
                    Nombre = "Alpha",
                    Tareas =
                    [
                        new Tarea { Id = "t1", Nombre = "T1", Estado = EstadoTarea.Pendiente, Prioridad = PrioridadTarea.Baja },
                        new Tarea { Id = "t2", Nombre = "T2", Estado = EstadoTarea.Hecha, Prioridad = PrioridadTarea.Media }
                    ]
                },
                new Fase
                {
                    Id = "fb",
                    Nombre = "Beta",
                    Tareas =
                    [
                        new Tarea { Id = "t3", Nombre = "T3", Prioridad = PrioridadTarea.Alta, Dependencias = ["t1"] }
                    ]
                }
            ]
        };
        Assert.Equal(p, RoundTrip(p));
    }

    [Fact]
    public void RoundTrip_TaskWithNullDate_Equal()
    {
        var tarea = new Tarea { Id = "t1", Nombre = "Sin fecha", Fecha = null };
        var p = new Proyecto { Nombre = "P", Fases = [new Fase { Id = "f1", Nombre = "F", Tareas = [tarea] }] };
        Assert.Equal(p, RoundTrip(p));
    }

    [Fact]
    public void RoundTrip_TaskWithEmptyResponsable_Equal()
    {
        var tarea = new Tarea { Id = "t1", Nombre = "Sin resp", Responsable = "" };
        var p = new Proyecto { Nombre = "P", Fases = [new Fase { Id = "f1", Nombre = "F", Tareas = [tarea] }] };
        Assert.Equal(p, RoundTrip(p));
    }
}
