# Changelog

Todos los cambios notables de este proyecto se documentan en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es/1.0.0/),
y este proyecto sigue [Semantic Versioning](https://semver.org/lang/es/).

---

## [0.3.0] - 2026-05-13

### Añadido

- Sidebar lateral redimensionable mediante `GridSplitter`.
- Política de privacidad en español (`privacy-policy.md`).

### Cambiado

- Sistema de diseño visual modernizado para cumplir los estándares de Microsoft Store (paleta de colores, tipografía, espaciados).
- Panel de detalle de tarea rediseñado con inputs con fondo, etiquetas explícitas y sin solapamiento de hints.
- Nombres de tareas y fases ahora pueden expandirse a múltiples líneas.

---

## [0.2.0] - 2026-05-13

### Añadido

- Filtrado de tareas por fase desde el sidebar lateral.
- Instrucciones de IA mejoradas en el markup de proyectos (`AiHeaderLines`).

### Corregido

- Truncamiento silencioso de contenido por conversión LF→CRLF al guardar archivos.

### Documentación

- README reescrito con enfoque más claro en propuesta de valor y adopción.
- Añadida sección "¿Por qué MarkupPM?" con beneficios diferenciadores.
- Archivo `LICENSE` (MIT) añadido al repositorio.

---

## [0.1.1] - 2026-05-08

### Corregido

- Normalización de la declaración XML del manifiesto antes de ejecutar `makeappx`.
- Codificación del manifiesto generado y reemplazos dirigidos en el release workflow.
- Empaquetado y firma del MSIX de forma manual en el workflow de release.
- Generación correcta de paquetes MSIX durante la compilación de release.
- Ruta de salida del MSIX y subida del asset al release.
- Permisos de escritura para publicar el release en GitHub.

---

## [0.1.0] - 2026-05-08

### Añadido

- Base de aplicación `MarkupPM` en WPF sobre `.NET 8`.
- Gestión de proyecto con fases y tareas.
- Edición de nombre de proyecto y fases desde la UI.
- Panel lateral de detalle de tarea.
- Persistencia en formato Markdown (parser y serializer).
- Soporte de archivos recientes.
- Arrastrar y soltar tareas (`gong-wpf-dragdrop`).
- Pruebas unitarias para parser/serializer y regresión básica.
- Workflows de CI y release con empaquetado MSIX.
- Iconos de toolbar y botones con MaterialDesign (`PackIcon`).
- Auto-foco en TextBoxes de renombrado al iniciar edición.

### Corregido

- `QA-001` — apertura de archivos desde línea de comandos fallaba al iniciar.
- `QA-002` — la lista de recientes en WelcomeView no se actualizaba tras abrir/guardar.
- `QA-003` — las iniciales del responsable no se actualizaban al cambiar el nombre.
- `QA-004` — fuga de suscripción a eventos en `RemoveFase`.
- `QA-006` — la app crasheaba al iniciar por recurso de icono faltante.
- Implementación de `IDropTarget` y corrección de fuga en `PropertyChanged`.

### Notas

- Esta es la primera publicación funcional del repositorio.

---

[0.3.0]: https://github.com/FJCF76/MarkupPM/compare/v0.2.0...v0.3.0
[0.2.0]: https://github.com/FJCF76/MarkupPM/compare/v0.1.1...v0.2.0
[0.1.1]: https://github.com/FJCF76/MarkupPM/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/FJCF76/MarkupPM/releases/tag/v0.1.0
