using MarkupPM.Models;
using MarkupPM.Services;
using MarkupPM.ViewModels;
using Xunit;

namespace MarkupPM.Tests;

public class StubMdParser : IMdParser
{
    public Proyecto Parse(string markdown) => new() { Nombre = "Test" };
}

public class StubMdSerializer : IMdSerializer
{
    public string Serialize(Proyecto proyecto) => "";
}

public class StubRecentFiles : IRecentFilesService
{
    public IReadOnlyList<string> GetRecent() => [];
    public void AddRecent(string filePath) { }
}

public class TareaViewModelTests
{
    [Fact]
    public void SelectTarea_SetsIsSelected_True()
    {
        var tarea = new TareaViewModel(new Tarea { Nombre = "Test" });

        tarea.IsSelected = true;

        Assert.True(tarea.IsSelected);
    }
}

public class MainViewModelSelectionTests
{
    private static MainViewModel CreateVm() =>
        new(new StubMdParser(), new StubMdSerializer(), new StubRecentFiles());

    [Fact]
    public void SelectTarea_SetsIsSelected()
    {
        var vm = CreateVm();
        vm.NuevoProyecto();
        vm.AddFase();
        var fase = vm.Fases[0];
        var tarea = fase.AddTarea("A");

        vm.SelectedTarea = tarea;

        Assert.True(tarea.IsSelected);
    }

    [Fact]
    public void SelectDifferentTarea_UnsetsOldIsSelected()
    {
        var vm = CreateVm();
        vm.NuevoProyecto();
        vm.AddFase();
        var fase = vm.Fases[0];
        var a = fase.AddTarea("A");
        var b = fase.AddTarea("B");

        vm.SelectedTarea = a;
        vm.SelectedTarea = b;

        Assert.False(a.IsSelected);
        Assert.True(b.IsSelected);
    }

    [Fact]
    public void DeselectTarea_SetsIsSelected_False()
    {
        var vm = CreateVm();
        vm.NuevoProyecto();
        vm.AddFase();
        var tarea = vm.Fases[0].AddTarea("A");

        vm.SelectedTarea = tarea;
        vm.SelectedTarea = null;

        Assert.False(tarea.IsSelected);
    }

    [Fact]
    public void TotalTareas_ComputedCorrectly()
    {
        var vm = CreateVm();
        vm.NuevoProyecto();
        vm.AddFase();
        vm.AddFase();
        vm.Fases[0].AddTarea("A");
        vm.Fases[0].AddTarea("B");
        vm.Fases[1].AddTarea("C");

        Assert.Equal(3, vm.TotalTareas);
    }

    [Fact]
    public void TotalTareas_UpdatesOnTaskAdded()
    {
        var vm = CreateVm();
        vm.NuevoProyecto();
        vm.AddFase();

        var changed = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(vm.TotalTareas)) changed = true;
        };

        vm.Fases[0].AddTarea("A");

        Assert.True(changed);
        Assert.Equal(1, vm.TotalTareas);
    }

    [Fact]
    public void TotalTareas_UpdatesOnTaskRemoved()
    {
        var vm = CreateVm();
        vm.NuevoProyecto();
        vm.AddFase();
        var tarea = vm.Fases[0].AddTarea("A");

        var changed = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(vm.TotalTareas)) changed = true;
        };

        vm.Fases[0].RemoveTarea(tarea);

        Assert.True(changed);
        Assert.Equal(0, vm.TotalTareas);
    }
}
