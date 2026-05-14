# Changelog

Todos los cambios notables de este proyecto se documentan en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es/1.0.0/),
y este proyecto sigue [Semantic Versioning](https://semver.org/lang/es/).

---

## [1.0.5] - 2026-05-14

### Corregido

- Eliminadas capabilities restringidas del manifiesto MSIX: `documentsLibrary`, `picturesLibrary`, `videosLibrary`, `musicLibrary`.
- La app usa exclusivamente `LocalState` para persistencia, por lo que ninguna de esas capabilities era necesaria.
- `documentsLibrary` requería aprobación especial de Microsoft y bloqueaba la validación en el proceso de Microsoft Store.
- Solo se mantiene `internetClient` (capability estándar, no requiere aprobación).

---

## [1.0.4] - 2026-05-14

### Añadido

- **Diálogos de archivo personalizados** (`OpenFileDialog`, `SaveFileDialog`): reemplazan los diálogos del sistema, incompatibles con AppContainer en entornos con OneDrive redirigido.
- **Protección de sobreescritura**: diálogo de confirmación estilizado (`OverwriteConfirmDialog`) al intentar guardar sobre un archivo existente, en lugar del `MessageBox` genérico de Windows.
- **Archivos recientes persistentes**: los proyectos abiertos y guardados se recuerdan entre sesiones mediante `RecentFilesService` (JSON en `LocalState`).
- **Guardado directo a LocalState**: el primer guardado de un proyecto escribe directamente en `LocalState` sin abrir un diálogo.
- **Búsqueda en diálogo de apertura**: filtro en tiempo real para localizar proyectos cuando hay muchos archivos.
- **Botón "Abrir carpeta"** en la barra de herramientas principal: abre la carpeta de proyectos (`LocalState`) directamente en el Explorador de Windows.
- `WarningBrush` y `WarningLightBrush` añadidos al sistema de diseño (`Styles.xaml`).

### Cambiado

- Los diálogos de apertura y guardado ahora se abren centrados sobre la ventana principal.
- El diálogo de apertura muestra scroll vertical cuando hay muchos proyectos.

### Técnico

- Migración completa a **AppContainer** (`uap10:TrustLevel="appContainer"`, `uap10:RuntimeBehavior="packagedClassicApp"`): la app ya no requiere acceso completo al sistema.
- Reemplazados WinRT pickers e `InitializeWithWindow` por diálogos WPF propios.
- `SanitizeFileName` para limpiar nombres de proyecto antes de escribir al disco.

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

[1.0.5]: https://github.com/FJCF76/MarkupPM/compare/v1.0.4...v1.0.5
[1.0.4]: https://github.com/FJCF76/MarkupPM/compare/v0.3.0...v1.0.4
[0.3.0]: https://github.com/FJCF76/MarkupPM/compare/v0.2.0...v0.3.0
[0.2.0]: https://github.com/FJCF76/MarkupPM/compare/v0.1.1...v0.2.0
[0.1.1]: https://github.com/FJCF76/MarkupPM/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/FJCF76/MarkupPM/releases/tag/v0.1.0


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
