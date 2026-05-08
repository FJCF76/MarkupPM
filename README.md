# MarkupPM

MarkupPM es una aplicación de escritorio en `WPF` para planificar proyectos por fases y tareas usando un archivo `Markdown` como formato de almacenamiento.

## Estado del proyecto

Versión inicial: `0.1.0`.

## Características

- Gestión de proyectos por fases.
- Alta, edición y eliminación de tareas.
- Panel de detalle de tarea.
- Guardado y carga en formato `*.md`.
- Arrastrar y soltar tareas entre fases.
- Soporte de tema visual con `Material Design`.
- Indicador de cambios sin guardar.
- Diálogos de confirmación para acciones destructivas.

## Tecnologías

- `.NET 8`
- `WPF`
- `CommunityToolkit.Mvvm`
- `MaterialDesignThemes`
- `gong-wpf-dragdrop`
- `xUnit` para pruebas

## Requisitos

- `Windows 10/11`
- `SDK de .NET 8`
- `Visual Studio 2022` (recomendado, con carga de trabajo de escritorio .NET)

## Ejecución local

1. Restaurar dependencias:
   - `dotnet restore`
2. Compilar:
   - `dotnet build`
3. Ejecutar tests:
   - `dotnet test`

## Estructura principal

- `MarkupPM/`: aplicación WPF.
- `MarkupPM.Tests/`: pruebas unitarias y de regresión.
- `.github/workflows/`: CI/CD y empaquetado de release.
- `docs/`: documentación auxiliar.

## Releases

El pipeline de release se dispara con tags en formato `v*.*.*` (por ejemplo `v0.1.0`).

## Licencia

Pendiente de definir.
