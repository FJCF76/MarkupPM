# Directiva de Privacidad — MarkupPM

**Última actualización:** junio de 2025
**Desarrollador:** Fernando Covecino

---

## 1. Información general

MarkupPM es una aplicación de escritorio para Windows que permite gestionar proyectos mediante archivos Markdown (`.md`). Esta directiva describe cómo se trata la información en el contexto del uso de la aplicación.

## 2. Datos que la aplicación NO recopila

MarkupPM **no recopila, no almacena en servidores remotos, no transmite ni comparte** ningún tipo de dato personal o de uso. Específicamente:

- No se recopilan datos de identificación personal (nombre, correo electrónico, etc.).
- No se envía información de uso, telemetría ni diagnósticos a ningún servidor.
- No se realizan conexiones a Internet de ningún tipo.
- No se comparte ninguna información con terceros.

## 3. Datos almacenados localmente

La aplicación únicamente almacena en el dispositivo del usuario:

- **Archivos de proyecto (`.md`):** creados y guardados por el usuario en la ubicación que él mismo elija.
- **Lista de archivos recientes:** guardada en `%APPDATA%\MarkupPM\recent.json`, exclusivamente en el dispositivo local, para facilitar el acceso rápido a proyectos previos. Este archivo nunca abandona el dispositivo.

Estos datos permanecen en todo momento bajo el control exclusivo del usuario y pueden eliminarse borrando los archivos correspondientes.

## 4. Permisos de la aplicación

MarkupPM requiere el permiso `runFullTrust` al estar empaquetada como MSIX. Este permiso es estándar para aplicaciones de escritorio WPF y se utiliza únicamente para leer y escribir los archivos de proyecto en las rutas seleccionadas por el usuario mediante los diálogos estándar del sistema operativo.

## 5. Aplicaciones de terceros

MarkupPM no integra SDKs de terceros con capacidad de recopilación de datos (publicidad, analítica, redes sociales, etc.).

## 6. Cambios en esta directiva

Cualquier cambio futuro en esta directiva se publicará en este mismo repositorio. Se notificará en las notas de la versión correspondiente si el cambio es relevante para la privacidad del usuario.

## 7. Contacto

Si tienes preguntas sobre esta directiva de privacidad, puedes abrir un issue en el repositorio de GitHub:
[https://github.com/FJCF76/MarkupPM/issues](https://github.com/FJCF76/MarkupPM/issues)
