# MarkupPM

> Planifica con la potencia de una app de escritorio y la simplicidad de `Markdown`.

MarkupPM es una aplicación de escritorio en `WPF` que te ayuda a organizar proyectos por fases y tareas con una experiencia visual fluida, guardando toda la información en archivos `*.md` legibles por humanos e IA.

## ¿Por qué MarkupPM?

- **Productividad inmediata**: crea y reorganiza tareas en segundos.
- **Formato abierto**: tus datos viven en `Markdown`, sin bloqueo de plataforma.
- **Trabajo claro y trazable**: estructura por fases para tener siempre visibilidad.
- **UX moderna**: interfaz limpia con `Material Design` y arrastrar/soltar.

## Características principales

- Gestión de proyectos por fases.
- Alta, edición y eliminación de tareas.
- Panel de detalle de tarea.
- Guardado y carga en formato `*.md`.
- Arrastrar y soltar tareas entre fases.
- Soporte de tema visual con `Material Design`.
- Indicador de cambios sin guardar.
- Diálogos de confirmación para acciones destructivas.

## Stack tecnológico

- `.NET 8`
- `WPF`
- `CommunityToolkit.Mvvm`
- `MaterialDesignThemes`
- `gong-wpf-dragdrop`
- `xUnit`

## Requisitos

- `Windows 10/11`
- `SDK de .NET 8`
- `Visual Studio 2022` (recomendado)

## Inicio rápido

1. Restaurar dependencias:
   - `dotnet restore`
2. Compilar:
   - `dotnet build`
3. Ejecutar pruebas:
   - `dotnet test`

## Estructura del repositorio

- `MarkupPM/`: aplicación principal `WPF`.
- `MarkupPM.Tests/`: pruebas unitarias y de regresión.
- `.github/workflows/`: automatización CI/CD y release.
- `docs/`: documentación auxiliar.

## Releases

Consulta versiones y novedades en la sección de releases del repositorio:

- `https://github.com/FJCF76/MarkupPM/releases`

El pipeline de release se dispara con tags en formato `v*.*.*` (ejemplo: `v0.1.1`).

## Licencia

Este proyecto está licenciado bajo **MIT License**.

- Ver archivo: `LICENSE`

Puedes usar, modificar y distribuir MarkupPM libremente bajo los términos de la licencia MIT.

## Estado

Versión inicial pública: `0.1.x`.

Si buscas una herramienta ligera para planificar proyectos con datos portables y legibles, **MarkupPM está hecho para ti**.
