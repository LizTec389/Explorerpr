# 📂 Explorador y Gestor de Archivos Avanzado (C# Windows Forms)

Un explorador de archivos profesional desarrollado en C# y Windows Forms. Este proyecto no solo permite navegar por los directorios del sistema operativo, sino que integra herramientas avanzadas de edición de texto, reproducción multimedia nativa, uso de cámara web, geolocalización y conexión a bases de datos.

## 🚀 Características Principales

### 1. 🗂️ Form1 - Explorador Principal
Es el núcleo de la aplicación. Actúa como un reemplazo interactivo del Explorador de Windows nativo.
- **Navegación Intuitiva:** Sistema de historial (Pilas/Stacks) para botones de Adelante, Atrás, Arriba, botón de Refrescar y barra de rutas.
- **Búsqueda Rápida:** Filtra archivos en la carpeta actual y subcarpetas por nombre o por extensión (ej. `.pdf`).
- **Reproductor Multimedia Nativo:** Reproduce archivos `.mp3` interactuando directamente con el hardware usando `winmm.dll` (sin depender de Windows Media Player). Soporta carátulas automáticas descargadas de la API de iTunes, control de volumen interactivo y sistema de *playlist* con pase automático.
- **Grabadora de Video Integrada:** Utiliza `avicap32.dll` para acceder a la cámara web y grabar video (con audio incluido) guardándolo automáticamente en formato `.avi` en la carpeta de Imágenes.
- **Lector de Excel y Tablas:** Utiliza `ExcelDataReader` para descifrar de manera segura archivos binarios `.xlsx` y `.xls`, además de `.csv`, `.json` y `.xml`, dibujándolos en formato de tabla estructurada.
- **Integración SQL Server:** Sube el registro de los archivos seleccionados (nombre, ruta, tamaño) a una base de datos local y permite exportarlos a un archivo.
- **Envío por Correo (SMTP):** Envía cualquier archivo seleccionado por correo electrónico como archivo adjunto de forma instantánea.

### 2. 📝 FrmEditor - Editor de Archivos Avanzado
Un potente editor de texto integrado capaz de modificar y visualizar múltiples formatos sin necesidad de abrir programas externos.
- Soporte para visualización y edición rápida de `.txt`, `.json`, `.csv` y `.xml`.
- Capacidad de **descifrar archivos de Excel** para mostrarlos en texto plano separado por barras `|`, previniendo así la corrupción accidental de los archivos binarios si el usuario intenta guardarlos directamente.
- Funciones de "Guardar Cambios" y "Guardar Como..." para exportar el trabajo.

### 3. 🖼️ FrmImageEditor - Visor y Editor de Imágenes
Una herramienta dedicada a la visualización de fotografías y sus datos ocultos.
- **Extracción EXIF Profunda:** Lee los bytes de metadatos de la fotografía (Fabricante, Modelo de cámara, ISO, Exposición, Punto F, etc.) y los estructura en una tabla detallada, similar a las propiedades avanzadas de Windows.
- **Integración GPS y Google Maps:** Si la fotografía fue tomada con ubicación activada, extrae las coordenadas GPS y renderiza un mapa interactivo de Google Maps utilizando el componente avanzado `WebView2`.
- **Cámara Web (Tomar Fotos):** Permite encender la cámara de la computadora y realizar capturas fotográficas de forma instantánea.

## 💻 Tecnologías y Librerías Utilizadas
- **Lenguaje:** C# (.NET)
- **Interfaz Gráfica:** Windows Forms (WinForms)
- **Bases de Datos:** `Microsoft.Data.SqlClient` (SQL Server)
- **Lector de Excel:** `ExcelDataReader` y `ExcelDataReader.DataSet`
- **Multimedia y Hardware:** Librerías nativas `winmm.dll` (MCI para Audio) y `avicap32.dll` (Video/Webcam)
- **Lectura de Metadatos:** `TagLib` para canciones e `Image.PropertyIdList` para fotografías.
- **Mapas y Navegador Web:** `Microsoft.Web.WebView2`

## ⚙️ Instrucciones de Instalación y Uso
1. Clona o descarga este repositorio en tu computadora.
2. Abre la solución en **Visual Studio**.
3. Restaura los paquetes NuGet. Asegúrate de tener instalados:
   - `ExcelDataReader` y `ExcelDataReader.DataSet`
   - `TagLib`
   - `Microsoft.Web.WebView2`
   - `Microsoft.Data.SqlClient`
4. (Opcional) Configura tu cadena de conexión a SQL Server en las propiedades de `Form1.cs`.
5. Compila y ejecuta la aplicación (Presiona F5).