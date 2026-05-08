using MarkupPM.Models;
using MarkupPM.Services;
using Xunit;

namespace MarkupPM.Tests;

public class MdSerializerTests
{
    private readonly MdSerializer _sut = new();

    [Fact]
    public void Serialize_EmptyProject_OnlyTitle()
    {
        var p = new Proyecto { Nombre = "Mi Proyecto" };
        var md = _sut.Serialize(p);
        Assert.Contains("MarkupPM AI instructions:", md);
        Assert.Contains("# Mi Proyecto", md);
        Assert.DoesNotContain("##", md);
    }

    [Fact]
    public void Serialize_Task_ContainsAllFields()
    {
        var tarea = new Tarea
        {
            Id = "abc123",
            Nombre = "Hacer algo",
            Estado = EstadoTarea.EnCurso,
            Prioridad = PrioridadTarea.Alta,
            Responsable = "Juan Carlos",
            Fecha = new DateOnly(2026, 5, 20),
            Notas = "Nota de prueba",
            Dependencias = ["dep001"]
        };
        var p = new Proyecto { Nombre = "P", Fases = [new Fase { Nombre = "F1", Tareas = [tarea] }] };
        var md = _sut.Serialize(p);

        Assert.Contains("[~]", md);
        Assert.Contains("Hacer algo", md);
        Assert.Contains("id:abc123", md);
        Assert.Contains("prio:alta", md);
        Assert.Contains("resp:Juan_Carlos", md);
        Assert.Contains("fecha:2026-05-20", md);
        Assert.Contains("dep:dep001", md);
        Assert.Contains("Nota de prueba", md);
    }

    [Fact]
    public void Serialize_TaskWithNoDate_EmptyFechaField()
    {
        var tarea = new Tarea { Nombre = "Sin fecha", Fecha = null };
        var p = new Proyecto { Nombre = "P", Fases = [new Fase { Nombre = "F", Tareas = [tarea] }] };
        var md = _sut.Serialize(p);
        Assert.Contains("fecha: ", md);
    }

    [Fact]
    public void Serialize_PhaseContainsPhaseId()
    {
        var fase = new Fase { Id = "fase01", Nombre = "Planificación" };
        var p = new Proyecto { Nombre = "P", Fases = [fase] };
        var md = _sut.Serialize(p);
        Assert.Contains("phase-id: fase01", md);
    }

    [Fact]
    public void Serialize_SpecialCharsInName_EscapedSafely()
    {
        var p = new Proyecto { Nombre = "Proyecto <!-- hack -->" };
        var md = _sut.Serialize(p);
        Assert.DoesNotContain("# Proyecto <!--", md);
    }
}
