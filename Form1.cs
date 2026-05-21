using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Explorerpr
{
    public partial class Form1 : Form
    {
        // Pilas para el historial (Atrás/Adelante)
        private Stack<string> backStack = new Stack<string>();
        private Stack<string> forwardStack = new Stack<string>();
        private string currentPath = @"C:\Users\liz\Media"; // Ruta de ejemplo de la captura
        private double currentLat;
        private double currentLon;
        string connectionString = @"Server=LIZETH; Database=ExplorerDB; Integrated Security=True; TrustServerCertificate=True;";

        WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
        bool isDragging = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (backStack.Count > 0)
            {
                forwardStack.Push(currentPath);
                Navegar(backStack.Pop());
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            DirectoryInfo parent = Directory.GetParent(currentPath);
            if (parent != null)
            {
                backStack.Push(currentPath);
                Navegar(parent.FullName);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamamos a la función que llena el ListView pasándole la ruta que tenemos abierta
                LoadFilesAndDirectories(currentPath);

                // Opcional: Mandar un mensajito rápido en la barra de estado
                lblStatusPlaylist.Text = "Lista actualizada: " + DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actualizar la lista: " + ex.Message);
            }
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            if (forwardStack.Count > 0)
            {
                // Guardamos la actual en el historial de atrás antes de avanzar
                backStack.Push(currentPath);
                string nextPath = forwardStack.Pop();
                Navegar(nextPath);
            }
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string term = txtSearch.Text.ToLower();
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Text.ToLower().Contains(term))
                    {
                        item.Selected = true;
                        item.EnsureVisible(); // Hace scroll automático hasta el archivo
                        break;
                    }
                }
            }
        }


        private void btnMediaDetails_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string fullPath = Path.Combine(currentPath, listView1.SelectedItems[0].Text);
                var fileTag = TagLib.File.Create(fullPath);

                string info = $"🎵 Detalles de la pista:\n\n" +
                              $"Título: {fileTag.Tag.Title}\n" +
                              $"Álbum: {fileTag.Tag.Album}\n" +
                              $"Año: {fileTag.Tag.Year}\n" +
                              $"Duración: {fileTag.Properties.Duration:mm\\:ss}\n" +
                              $"Calidad: {fileTag.Properties.AudioBitrate} kbps";

                MessageBox.Show(info, "Propiedades del Archivo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            string newPath = Path.Combine(currentPath, "New Folder");
            Directory.CreateDirectory(newPath);
            Navegar(currentPath); // Refresca la vista
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.SelectedItems[0].BeginEdit();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string fullPath = Path.Combine(currentPath, item.Text);
                    if (item.SubItems[2].Text == "Folder")
                        Directory.Delete(fullPath, true);
                    else
                        File.Delete(fullPath);
                }
                Navegar(currentPath);
            }
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            panelProperties.Visible = !panelProperties.Visible;
            lblSelectionInfo.Text = "Options panel toggled";
        }


        private void btnPlay_Click(object sender, EventArgs e)
        {
            player.controls.play();
            timerMusic.Start(); // Activamos el movimiento de la barra
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            player.controls.pause();
            lblSelectionInfo.Text = "Paused";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            player.controls.stop();
            timerMusic.Stop(); // Detenemos el proceso para ahorrar recursos
            trackBarProgress.Value = 0;
        }
        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            player.settings.volume = trackBarVolume.Value;
        }

        private void timerMusic_Tick(object sender, EventArgs e)
        {
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying && !isDragging)
            {
                // Actualiza el Slider con la posición actual del track 
                trackBarProgress.Maximum = (int)player.currentMedia.duration;
                trackBarProgress.Value = (int)player.controls.currentPosition;

                lblTime.Text = player.controls.currentPositionString;
            }
        }

        private void trackBarProgress_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
        }

        // 2. Cuando suelta el clic, cambiamos la posición de la música
        private void trackBarProgress_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            player.controls.currentPosition = trackBarProgress.Value;
        }

        // 3. (Opcional) Si quieres que cambie mientras arrastra
        private void trackBarProgress_Scroll(object sender, EventArgs e)
        {
            if (isDragging)
            {
                lblTime.Text = TimeSpan.FromSeconds(trackBarProgress.Value).ToString(@"mm\:ss");
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string fullPath = Path.Combine(currentPath, item.Text);

                // 1. Si es CARPETA: Navegamos dentro
                if (item.SubItems[2].Text == "Folder")
                {
                    backStack.Push(currentPath);
                    forwardStack.Clear();
                    Navegar(fullPath); // Usa tu función de navegación
                }
                // 2. Si es ARCHIVO: Lo abrimos con el programa predeterminado del PC
                else if (File.Exists(fullPath))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fullPath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se pudo abrir el archivo: " + ex.Message);
                    }
                }
            }
        }

        // SELECCIÓN: Para actualizar el panel de propiedades y la barra de estado
        private async void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem sel = listView1.SelectedItems[0];
                string extension = Path.GetExtension(sel.Text).ToLower().Replace(".", "");
                string fullPath = Path.Combine(currentPath, sel.Text);

                // --- ACTUALIZACIÓN DE PANEL DERECHO (File Properties) ---
                lblSelectedName.Text = sel.Text;
                lblSelectedType.Text = sel.SubItems[2].Text;
                lblModifiedValue.Text = sel.SubItems[3].Text;

                // --- LÓGICA DE ICONO DINÁMICO ---
                if (sel.SubItems[2].Text == "Folder")
                {
                    picSelectedIcon.Image = Properties.Resources.folder;
                    lblTypeValue.Text = "System Folder";
                }
                else
                {
                    picSelectedIcon.Image = Properties.Resources.file;
                    lblTypeValue.Text = extension.ToUpper() + " File";
                }

                // --- LÓGICA MULTIMEDIA ---
                if (extension == "mp3")
                {
                    player.URL = fullPath;
                    lblMediaTitle.Text = sel.Text;

                    // 1. Intentar obtener carátula de la API (Spotify/iTunes)
                    // Quitamos la extensión para que la búsqueda sea más limpia
                    string nombreLimpio = sel.Text.Replace(".mp3", "");
                    string urlCaratula = await BuscarCaratulaOnline(nombreLimpio);

                    if (!string.IsNullOrEmpty(urlCaratula))
                    {
                        picAlbumArt.LoadAsync(urlCaratula); // Carga la imagen de la API
                    }
                    else
                    {
                        picAlbumArt.Image = Properties.Resources.music_default;
                    }

                    // 2. Metadatos básicos
                    string artista = player.currentMedia.getItemInfo("Artist");
                    lblMediaArtist.Text = !string.IsNullOrEmpty(artista) ? artista : "Artista Desconocido";

                    player.controls.play();
                    timerMusic.Start();
                }
                else if (extension == "jpg" || extension == "jpeg" || extension == "png")
                {
                    player.controls.stop();
                    timerMusic.Stop();

                    // Limpiamos el picturebox de música para mostrar la foto o dejarlo listo
                    picAlbumArt.Image = Image.FromFile(fullPath);

                    CargarMetadatosFoto(fullPath);
                    lblMediaTitle.Text = sel.Text;
                    lblMediaArtist.Text = "Image Preview";
                }
                else if (extension == "mp4")
                {
                    player.URL = fullPath;
                    lblMediaTitle.Text = sel.Text;
                    lblMediaArtist.Text = "Video File";

                    // En video usualmente no ponemos carátula de álbum
                    picAlbumArt.Image = null;

                    player.controls.play();
                    timerMusic.Start();
                }
                else
                {
                    lblMediaTitle.Text = "No media selected";
                    lblMediaArtist.Text = "--";
                    picAlbumArt.Image = null;
                    player.controls.stop();
                    timerMusic.Stop();
                }
            }
        }
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }



        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
    string rutaArchivo = Path.Combine(currentPath, "NuevoDocumento.txt");
    
    try {
        File.Create(rutaArchivo).Dispose(); // Creamos el archivo y cerramos el proceso
        LoadFilesAndDirectories(currentPath);
    }
    catch (Exception ex) {
        MessageBox.Show("Error al crear archivo: " + ex.Message);
    }
}
        

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        //METODOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOSs


        private async Task<string> BuscarCaratulaOnline(string nombreCancion)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://itunes.apple.com/search?term={Uri.EscapeDataString(nombreCancion)}&limit=1&entity=song";
                    var response = await client.GetStringAsync(url);

                    using (JsonDocument doc = JsonDocument.Parse(response))
                    {
                        var root = doc.RootElement.GetProperty("results");
                        if (root.GetArrayLength() > 0)
                        {
                            return root[0].GetProperty("artworkUrl100").GetString().Replace("100x100", "600x600");
                        }
                    }
                }
            }
            catch { /* Si no hay internet o falla, no se detiene el programa */ }
            return null;
        }








        private void Navegar(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists) return;

                currentPath = path;
                cmbPath.Text = path; // El ComboBox o TextBox de la barra de direcciones

                listView1.Items.Clear();

                // Cargar Carpetas
                foreach (var dir in di.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(dir.Name, 0); // 0 = Icono Carpeta en imageList
                    item.SubItems.Add(""); // Size
                    item.SubItems.Add("Folder"); // Type
                    item.SubItems.Add(dir.LastWriteTime.ToString("yyyy-MM-dd")); // Date
                    item.SubItems.Add("OK"); // Status
                    listView1.Items.Add(item);
                }

                // Cargar Archivos
                foreach (var file in di.GetFiles())
                {
                    int iconIndex = GetIconIndex(file.Extension); // Función para elegir icono
                    ListViewItem item = new ListViewItem(file.Name, iconIndex);
                    item.SubItems.Add((file.Length / 1024).ToString() + " KB");
                    item.SubItems.Add(file.Extension.ToUpper().Replace(".", ""));
                    item.SubItems.Add(file.LastWriteTime.ToString("yyyy-MM-dd"));
                    item.SubItems.Add("OK");
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Actualizamos la barra de estado inferior (StatusStrip)
                lblSelectionInfo.Text = "Error: " + ex.Message;
            }
        }

        private int GetIconIndex(string ext)
        {
            switch (ext.ToLower())
            {
                case ".mp3": return 1; // Icono música
                case ".mp4": return 2; // Icono video
                case ".csv": case ".xlsx": return 3; // Icono Excel
                default: return 4; // Icono archivo genérico
            }
        }

        private void ReproducirMedia(string rutaArchivo)
        {
            // 1. Asignar ruta al reproductor
            player.URL = rutaArchivo;

            // 2. Actualizar Labels del panel superior (image_ce19da.png / Captura 180411)
            lblMediaTitle.Text = Path.GetFileName(rutaArchivo);
            lblMediaArtist.Text = "Cargando metadatos..."; // Aquí podrías usar una librería como TagLib#

            // 3. Iniciar reproducción
            player.controls.play();
        }

        // --- 1. LAS FUNCIONES QUE TE DIO EL MAESTRO (Indispensables para el mapa) ---

        private (double lat, double lon)? ObtenerCoordenadas(Image img)
        {
            try
            {
                // IDs EXIF para GPS
                const int GPSLatitudeRef = 0x0001;
                const int GPSLatitude = 0x0002;
                const int GPSLongitudeRef = 0x0003;
                const int GPSLongitude = 0x0004;

                if (!img.PropertyIdList.Contains(GPSLatitude) || !img.PropertyIdList.Contains(GPSLongitude))
                    return null;

                var latRef = GetString(img.GetPropertyItem(GPSLatitudeRef));
                var lonRef = GetString(img.GetPropertyItem(GPSLongitudeRef));

                double latitude = ConvertToDegrees(img.GetPropertyItem(GPSLatitude).Value);
                double longitude = ConvertToDegrees(img.GetPropertyItem(GPSLongitude).Value);

                if (latRef == "S") latitude = -latitude;
                if (lonRef == "W") longitude = -longitude;

                return (latitude, longitude);
            }
            catch { return null; }
        }

        private async void MostrarMapa(double lat, double lon)
        {
            // Asegúrate que tu control WebView2 en el diseño se llame "webMapa"
            await webMapa.EnsureCoreWebView2Async();
            string html = $@"
    <html>
    <body style='margin:0'>
        <iframe width='100%' height='100%' frameborder='0' style='border:0' 
            src='https://maps.google.com/maps?q={lat},{lon}&z=14&output=embed'>
        </iframe>
    </body>
    </html>";
            webMapa.NavigateToString(html);
        }

        // Funciones auxiliares del maestro
        private string GetString(System.Drawing.Imaging.PropertyItem prop)
            => System.Text.Encoding.ASCII.GetString(prop.Value).Trim('\0');

        private double ConvertToDegrees(byte[] value)
        {
            double grados = ToRational(value, 0);
            double minutos = ToRational(value, 8);
            double segundos = ToRational(value, 16);
            return grados + (minutos / 60.0) + (segundos / 3600.0);
        }

        private double ToRational(byte[] bytes, int index)
        {
            uint num = BitConverter.ToUInt32(bytes, index);
            uint den = BitConverter.ToUInt32(bytes, index + 4);
            return den == 0 ? 0 : (double)num / den;
        }

        // --- 2. EL MÉTODO PARA CARGAR LA FOTO (El que une todo) ---

        private void CargarMetadatosFoto(string rutaArchivo)
        {
            try
            {
                using (Image img = Image.FromFile(rutaArchivo))
                {
                    var coordenadas = ObtenerCoordenadas(img);
                    if (coordenadas != null)
                    {
                        // Actualiza el label de coordenadas en tu panel (image_26e115.png)
                        lblCoordenadas.Text = $"{coordenadas.Value.lat:F4}, {coordenadas.Value.lon:F4}";
                        MostrarMapa(coordenadas.Value.lat, coordenadas.Value.lon);
                    }
                    else
                    {
                        lblCoordenadas.Text = "Sin datos GPS";
                    }
                }
            }
            catch { /* Manejo de error */ }
        }




        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Obtenemos el nombre del nodo seleccionado (ej. "Music")
            string carpetaDestino = e.Node.Text;
            string nuevaRuta = "";

            // Mapeamos los nombres del árbol a las rutas reales del sistema
            switch (carpetaDestino)
            {
                case "Documents":
                    nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    break;
                case "Music":
                    nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                    break;
                case "Pictures":
                    nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    break;
                case "Video":
                case "Videos":
                    nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                    break;
                case "This PC":

                    return;
                default:
                    return;
            }

            // Si encontramos una ruta válida, mandamos llamar a tu función de cargar archivos
            if (!string.IsNullOrEmpty(nuevaRuta))
            {
                currentPath = nuevaRuta;
                txtSearch.Text = currentPath; // Actualiza la barra de direcciones (Captura 083248)
                LoadFilesAndDirectories(currentPath); // Esta es la función que ya tienes para llenar el ListView
            }
        }


        private void LoadFilesAndDirectories(string path)
        {
            try
            {
                listView1.Items.Clear(); // Limpiamos la lista actual
                DirectoryInfo di = new DirectoryInfo(path);

                // 1. Cargar Carpetas
                foreach (var dir in di.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(dir.Name, 0); // Icono 0 suele ser folder
                    item.SubItems.Add(""); // Tamaño
                    item.SubItems.Add("Folder");
                    item.SubItems.Add(dir.LastWriteTime.ToString());
                    listView1.Items.Add(item);
                }

                // 2. Cargar Archivos
                foreach (var file in di.GetFiles())
                {
                    ListViewItem item = new ListViewItem(file.Name, 1); // Icono 1 suele ser archivo
                    item.SubItems.Add((file.Length / 1024).ToString() + " KB");
                    item.SubItems.Add(file.Extension.ToUpper());
                    item.SubItems.Add(file.LastWriteTime.ToString());
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo acceder a la carpeta: " + ex.Message);
            }
        }

        private void btnViewCSV_Click(object sender, EventArgs e)
        {
            try
            {
                string rutaCSV = Path.Combine(Application.StartupPath, "ListaArchivos.csv");
                using (StreamWriter sw = new StreamWriter(rutaCSV))
                {
                    // Escribimos los encabezados
                    sw.WriteLine("Nombre,Tamaño,Tipo,Fecha");

                    foreach (ListViewItem item in listView1.Items)
                    {
                        string linea = $"{item.Text},{item.SubItems[1].Text},{item.SubItems[2].Text},{item.SubItems[3].Text}";
                        sw.WriteLine(linea);
                    }
                }
                // Abrimos el archivo automáticamente con Excel o Bloc de notas
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(rutaCSV) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el CSV: " + ex.Message);
            }
        }



        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nombreCarpeta = "Nueva Carpeta";
            string rutaCompleta = Path.Combine(currentPath, nombreCarpeta);

            // Si ya existe, le agregamos un número para no borrar nada
            int i = 1;
            while (Directory.Exists(rutaCompleta))
            {
                rutaCompleta = Path.Combine(currentPath, nombreCarpeta + " (" + i + ")");
                i++;
            }

            try
            {
                Directory.CreateDirectory(rutaCompleta);
                LoadFilesAndDirectories(currentPath); // Refresca la lista para que aparezca
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear carpeta: " + ex.Message);
            }
        }


        private void btnSqlUp_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem sel = listView1.SelectedItems[0];
                string nombre = sel.Text;
                string extension = Path.GetExtension(nombre);
                string ruta = Path.Combine(currentPath, nombre);

                // Convertimos el tamaño de string (ej: "3750 KB") a número
                double tamano = 0;
                double.TryParse(sel.SubItems[1].Text.Replace(" KB", ""), out tamano);

                string query = "INSERT INTO ArchivosSubidos (NombreArchivo, Extension, TamanoKB, RutaLocal, FechaModificacion) " +
                               "VALUES (@nom, @ext, @tam, @ruta, @fecha)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@nom", nombre);
                        cmd.Parameters.AddWithValue("@ext", extension);
                        cmd.Parameters.AddWithValue("@tam", tamano);
                        cmd.Parameters.AddWithValue("@ruta", ruta);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.Parse(sel.SubItems[3].Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("¡Archivo subido a SQL Server exitosamente!", "SQL Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al conectar con SQL Server: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un archivo primero.");
            }
        }










    }
}






