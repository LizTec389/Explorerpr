using Explorerpr.Forms;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Runtime.InteropServices;

namespace Explorerpr
{
    public partial class Form1 : Form
    {
        private Stack<string> backStack = new Stack<string>();
        private Stack<string> forwardStack = new Stack<string>();
        private string currentPath = @"C:\Users\liz\Media";
        private double currentLat;
        private double currentLon;
        string connectionString = @"Server=LIZETH; Database=ExplorerDB; Integrated Security=True; TrustServerCertificate=True;";

        WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
        bool isDragging = false;

        private List<string> playlist = new List<string>();
        private int indicePlaylist = 0;
        private bool estaGrabandoAudio = false;

        // Librería nativa de Windows para grabar audio del micrófono
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnString, int returnLength, IntPtr hwndCallback);

        public Form1()
        {
            InitializeComponent();

            player.PlayStateChange += Player_PlayStateChange;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
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
                LoadFilesAndDirectories(currentPath);
                lblSelectionInfo.Text = "Lista actualizada: " + DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex) { MessageBox.Show("No se pudo actualizar: " + ex.Message); }
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            if (forwardStack.Count > 0)
            {
                backStack.Push(currentPath);
                Navegar(forwardStack.Pop());
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string term = txtSearch.Text.ToLower().Trim();

                // Si el usuario escribe una extensión (empieza con punto)
                if (term.StartsWith("."))
                {
                    try
                    {
                        listView1.Clear();
                        listView1.View = View.Details;
                        listView1.Columns.Add("Resultados de Búsqueda", 400);
                        listView1.Columns.Add("Tipo", 100);

                        DirectoryInfo di = new DirectoryInfo(currentPath);

                        // Busca en la carpeta actual y en todas las carpetas de adentro (AllDirectories)
                        foreach (var file in di.GetFiles("*" + term, SearchOption.AllDirectories))
                        {
                            ListViewItem item = new ListViewItem(file.FullName, GetIconIndex(file.Extension));
                            item.SubItems.Add(file.Extension.ToUpper());
                            listView1.Items.Add(item);
                        }

                        MessageBox.Show($"Búsqueda completada para la extensión: {term}", "Búsqueda");
                    }
                    catch (Exception ex) { MessageBox.Show("Error en la búsqueda: " + ex.Message); }
                }
                else
                {
                    // Búsqueda normal por nombre que ya tenías
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Text.ToLower().Contains(term))
                        {
                            item.Selected = true;
                            item.EnsureVisible();
                            break;
                        }
                    }
                }
            }
        }

        // ==========================================
        // 1. CARGA DE METADATOS Y CORREO ELÉCTRONICO
        // ==========================================
        private void btnMediaDetails_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string fullPath = Path.Combine(currentPath, listView1.SelectedItems[0].Text);
                FileInfo fi = new FileInfo(fullPath);

                string info = $"📄 Metadatos del archivo:\n\n" +
                              $"Nombre: {fi.Name}\n" +
                              $"Tamaño: {fi.Length / 1024} KB\n" +
                              $"Creación: {fi.CreationTime}\n" +
                              $"Modificación: {fi.LastWriteTime}\n" +
                              $"Atributos: {fi.Attributes}\n\n";

                try
                {
                    // Si es multimedia intentamos sacar más datos
                    var fileTag = TagLib.File.Create(fullPath);
                    info += $"🎵 Detalles Multimedia:\n" +
                            $"Título: {fileTag.Tag.Title}\n" +
                            $"Álbum: {fileTag.Tag.Album}\n" +
                            $"Duración: {fileTag.Properties.Duration:mm\\:ss}";
                }
                catch { /* No es archivo multimedia soportado por TagLib */ }

                MessageBox.Show(info, "Metadatos Completos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

      
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) { MessageBox.Show("Selecciona un archivo primero."); return; }

            string fullPath = Path.Combine(currentPath, listView1.SelectedItems[0].Text);

            string destino = Microsoft.VisualBasic.Interaction.InputBox("Ingresa el correo del destinatario:", "Enviar Archivo", "");

            if (string.IsNullOrEmpty(destino) || !destino.Contains("@") || !destino.Contains("."))
            {
                MessageBox.Show("Debes ingresar un correo electrónico válido (ejemplo@gmail.com).", "Correo Inválido");
                return;
            }
            string asunto = Microsoft.VisualBasic.Interaction.InputBox("Ingresa el asunto del correo:", "Asunto", "Te comparto este archivo");

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("lizethcarrizales42@gmail.com");
                mail.To.Add(destino);
                mail.Subject = asunto;
                mail.Body = "Hola, te envío este archivo adjunto desde mi explorador personalizado.";
                mail.Attachments.Add(new Attachment(fullPath));

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("lizethcarrizales@gmail.com", "shjcpwvfujwszenb");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                MessageBox.Show("¡Correo enviado exitosamente con el archivo adjunto!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo (Revisa tus credenciales): " + ex.Message, "Error SMTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            string newPath = Path.Combine(currentPath, "New Folder");
            Directory.CreateDirectory(newPath);
            Navegar(currentPath);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string fullPath = Path.Combine(currentPath, item.Text);
                    if (item.SubItems[2].Text == "Folder") Directory.Delete(fullPath, true);
                    else File.Delete(fullPath);
                }
                Navegar(currentPath);
            }
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Crear Nuevo Archivo";
                sfd.Filter = "Documento de Texto|*.txt|Archivo JSON|*.json|Archivo CSV|*.csv|Archivo XML|*.xml|Archivo PDF (Texto plano)|*.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Se crea vacío dependiendo del formato
                        string contenidoInicial = "";
                        if (sfd.FileName.EndsWith(".json")) contenidoInicial = "{\n\n}";
                        else if (sfd.FileName.EndsWith(".xml")) contenidoInicial = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<root>\n</root>";

                        File.WriteAllText(sfd.FileName, contenidoInicial);
                        LoadFilesAndDirectories(currentPath);
                        MessageBox.Show("Archivo creado correctamente.");
                    }
                    catch (Exception ex) { MessageBox.Show("Error al crear archivo: " + ex.Message); }
                }
            }
        }

        private void AbrirEditorDeArchivos(string ruta)
        {
            Form editorForm = new Form { Text = "Editor Avanzado - " + Path.GetFileName(ruta), Size = new Size(700, 500) };
            TextBox txtContenido = new TextBox { Multiline = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Both, Text = File.ReadAllText(ruta), Font = new Font("Consolas", 11) };

            Panel panelAbajo = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            Button btnGuardar = new Button { Text = "Guardar Cambios", Dock = DockStyle.Right, Width = 150 };
            Button btnGuardarComo = new Button { Text = "Guardar Como...", Dock = DockStyle.Right, Width = 150 };

            btnGuardar.Click += (s, ev) =>
            {
                File.WriteAllText(ruta, txtContenido.Text);
                MessageBox.Show("Cambios guardados en el archivo original.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnGuardarComo.Click += (s, ev) =>
            {
                using (SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Texto|*.txt|JSON|*.json|CSV|*.csv|XML|*.xml|Documento PDF|*.pdf|Documento Word|*.docx|Hoja Excel|*.xlsx|Todos|*.*"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(sfd.FileName, txtContenido.Text);
                        MessageBox.Show("Guardado exitosamente como " + Path.GetExtension(sfd.FileName));
                        LoadFilesAndDirectories(currentPath); // Actualizamos explorador
                    }
                }
            };

            panelAbajo.Controls.Add(btnGuardarComo);
            panelAbajo.Controls.Add(btnGuardar);
            editorForm.Controls.Add(txtContenido);
            editorForm.Controls.Add(panelAbajo);
            editorForm.ShowDialog();
        }

        private void CargarDatosEnTabla(string ruta)
        {
            try
            {
                listView1.Clear();
                listView1.View = View.Details;

                string ext = Path.GetExtension(ruta).ToLower();
                string contenido = File.ReadAllText(ruta);

                if (ext == ".csv" || ext == ".txt")
                {
                    string[] lineas = contenido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (lineas.Length > 0)
                    {
                        char separador = ext == ".csv" ? ',' : '\t';
                        string[] cabeceras = lineas[0].Split(separador);

                        foreach (string cab in cabeceras) listView1.Columns.Add(cab, 150);

                        for (int i = 1; i < lineas.Length; i++)
                        {
                            string[] datos = lineas[i].Split(separador);
                            ListViewItem item = new ListViewItem(datos[0]);
                            for (int j = 1; j < datos.Length; j++) item.SubItems.Add(datos[j]);
                            listView1.Items.Add(item);
                        }
                    }
                }
                else if (ext == ".json" || ext == ".xml") // ¡Agregamos soporte para XML!
                {
                    listView1.Columns.Add("Propiedad Original", 300);
                    listView1.Columns.Add("Contenido", 400);
                    ListViewItem item = new ListViewItem("Código Crudo");
                    item.SubItems.Add(contenido.Substring(0, Math.Min(contenido.Length, 150)) + "...");
                    listView1.Items.Add(item);
                }

                MessageBox.Show("Datos cargados. Usa el botón 'Back' o navega a otra carpeta para regresar a la vista normal.");
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar tabla: " + ex.Message); }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string fullPath = Path.Combine(currentPath, item.Text);
                string extension = Path.GetExtension(item.Text).ToLower();

                if (item.SubItems[2].Text == "Folder")
                {
                    backStack.Push(currentPath);
                    forwardStack.Clear();
                    Navegar(fullPath);
                }
                else if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    if (File.Exists(fullPath)) { new FrmImageEditor(fullPath).Show(); }
                }
                else if (extension == ".txt" || extension == ".csv" || extension == ".json" || extension == ".xml")
                {
                    // PREGUNTAMOS CÓMO LO QUIERE ABRIR
                    DialogResult res = MessageBox.Show("¿Deseas editar el archivo en el Editor de Texto (SÍ) o visualizarlo en la Tabla/Lista (NO)?", "Opciones de Apertura", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes) AbrirEditorDeArchivos(fullPath);
                    else if (res == DialogResult.No) CargarDatosEnTabla(fullPath);
                }
                else if (File.Exists(fullPath))
                {
                    try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fullPath) { UseShellExecute = true }); }
                    catch (Exception ex) { MessageBox.Show("No se pudo abrir el archivo: " + ex.Message); }
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e) { player.controls.play(); timerMusic.Start(); }
        private void btnPause_Click(object sender, EventArgs e) { player.controls.pause(); }
        private void btnStop_Click(object sender, EventArgs e) { player.controls.stop(); timerMusic.Stop(); trackBarProgress.Value = 0; }
        private void trackBarVolume_Scroll(object sender, EventArgs e) { player.settings.volume = trackBarVolume.Value; }

        private void timerMusic_Tick(object sender, EventArgs e)
        {
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying && !isDragging)
            {
                trackBarProgress.Maximum = (int)player.currentMedia.duration;
                trackBarProgress.Value = (int)player.controls.currentPosition;
                lblTime.Text = player.controls.currentPositionString;
            }
        }

        private void trackBarProgress_MouseDown(object sender, MouseEventArgs e) { isDragging = true; }
        private void trackBarProgress_MouseUp(object sender, MouseEventArgs e) { isDragging = false; player.controls.currentPosition = trackBarProgress.Value; }

        private void Player_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                btnNextTrack_Click(null, null);
            }
        }

        private void btnCargarPlaylist_Click(object sender, EventArgs e)
        {
            playlist.Clear();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    playlist.Add(Path.Combine(currentPath, item.Text));
                    if (playlist.Count >= 5) break; // Límite de 5 canciones
                }
            }

            if (playlist.Count > 0)
            {
                indicePlaylist = 0;
                ReproducirMedia(playlist[indicePlaylist]);
                MessageBox.Show($"Se cargaron {playlist.Count} canciones en la Playlist.");
            }
            else { MessageBox.Show("No hay archivos mp3 en esta carpeta para la playlist."); }
        }

        private void btnNextTrack_Click(object sender, EventArgs e)
        {
            if (playlist.Count > 0)
            {
                indicePlaylist++;
                if (indicePlaylist >= playlist.Count) indicePlaylist = 0; // Vuelve al inicio
                ReproducirMedia(playlist[indicePlaylist]);
            }
        }

        private void btnPrevTrack_Click(object sender, EventArgs e)
        {
            if (playlist.Count > 0)
            {
                indicePlaylist--;
                if (indicePlaylist < 0) indicePlaylist = playlist.Count - 1; // Va al final
                ReproducirMedia(playlist[indicePlaylist]);
            }
        }

        private async void ReproducirMedia(string rutaArchivo)
        {
            player.URL = rutaArchivo;
            lblMediaTitle.Text = Path.GetFileName(rutaArchivo);

            try
            {
                string nombreLimpio = Path.GetFileNameWithoutExtension(rutaArchivo);
                string urlCaratula = await BuscarCaratulaOnline(nombreLimpio);
                if (!string.IsNullOrEmpty(urlCaratula))
                    picAlbumArt.LoadAsync(urlCaratula);
                else
                    picAlbumArt.Image = Properties.Resources.sound;
            }
            catch { }

            player.controls.play();
            timerMusic.Start();
        }

        private void btnGrabarAudio_Click(object sender, EventArgs e)
        {
            if (!estaGrabandoAudio)
            {
                mciSendString("open new Type waveaudio Alias recsound", null, 0, IntPtr.Zero);
                mciSendString("record recsound", null, 0, IntPtr.Zero);
                estaGrabandoAudio = true;
                MessageBox.Show("🔴 GRABANDO... Habla por el micrófono de tu laptop. \nPresiona este mismo botón para DETENER y guardar el archivo.", "Grabadora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                mciSendString("stop recsound", null, 0, IntPtr.Zero);
                using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Archivo de Audio WAV|*.wav", Title = "Guardar Grabación" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        mciSendString($"save recsound \"{sfd.FileName}\"", null, 0, IntPtr.Zero);
                        MessageBox.Show("Grabación guardada con éxito.", "Guardado");
                        LoadFilesAndDirectories(currentPath);
                    }
                }
                mciSendString("close recsound", null, 0, IntPtr.Zero);
                estaGrabandoAudio = false;
            }
        }

        // ==========================================
        // 5. SELECCIÓN, CARGA Y API DE IMÁGENES
        // ==========================================
        private async void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem sel = listView1.SelectedItems[0];
                string extension = Path.GetExtension(sel.Text).ToLower().Replace(".", "");
                string fullPath = Path.Combine(currentPath, sel.Text);

                lblSelectedName.Text = sel.Text;
                lblSelectedType.Text = sel.SubItems[2].Text;

                if (sel.SubItems.Count > 3) lblModifiedValue.Text = sel.SubItems[3].Text;

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

                if (extension == "mp3")
                {
                    player.URL = fullPath;
                    lblMediaTitle.Text = sel.Text;
                    string nombreLimpio = sel.Text.Replace(".mp3", "");
                    string urlCaratula = await BuscarCaratulaOnline(nombreLimpio);

                    if (!string.IsNullOrEmpty(urlCaratula)) picAlbumArt.LoadAsync(urlCaratula);

                    player.controls.play();
                    timerMusic.Start();
                }
                else if (extension == "jpg" || extension == "jpeg" || extension == "png")
                {
                    player.controls.stop();
                    timerMusic.Stop();
                    picAlbumArt.Image = Image.FromFile(fullPath);
                    CargarMetadatosFoto(fullPath);
                    lblMediaTitle.Text = sel.Text;
                }
                else
                {
                    player.controls.stop();
                    timerMusic.Stop();
                }
            }
        }

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
                            return root[0].GetProperty("artworkUrl100").GetString().Replace("100x100", "600x600");
                    }
                }
            }
            catch { }
            return null;
        }

        // ==========================================
        // 6. SQL SERVER - SUBIR Y DESCARGAR ARCHIVOS
        // ==========================================
        private void btnSqlUp_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem sel = listView1.SelectedItems[0];
                string nombre = sel.Text;
                string extension = Path.GetExtension(nombre);
                string ruta = Path.Combine(currentPath, nombre);
                double tamano = 0;
                double.TryParse(sel.SubItems[1].Text.Replace(" KB", ""), out tamano);

                string query = "INSERT INTO ArchivosSubidos (NombreArchivo, Extension, TamanoKB, RutaLocal, FechaModificacion) VALUES (@nom, @ext, @tam, @ruta, @fecha)";

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
                        MessageBox.Show("¡Archivo subido a SQL Server exitosamente!");
                    }
                    catch (Exception ex) { MessageBox.Show("Error en SQL Server: " + ex.Message); }
                }
            }
            else { MessageBox.Show("Selecciona un archivo primero."); }
        }

        private void btnDescargarDB_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Archivo CSV|*.csv|Archivo de Texto|*.txt", Title = "Exportar Base de Datos" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("SELECT * FROM ArchivosSubidos", conn);
                            SqlDataReader reader = cmd.ExecuteReader();

                            using (StreamWriter sw = new StreamWriter(sfd.FileName))
                            {
                                sw.WriteLine("ID, Nombre, Extension, Tamano, Ruta"); // Encabezados
                                while (reader.Read())
                                {
                                    sw.WriteLine($"{reader["ID"]}, {reader["NombreArchivo"]}, {reader["Extension"]}, {reader["TamanoKB"]}, {reader["RutaLocal"]}");
                                }
                            }
                            MessageBox.Show("Base de datos descargada y guardada en el archivo exitosamente.");
                        }
                        catch (Exception ex) { MessageBox.Show("Error al descargar DB: " + ex.Message); }
                    }
                }
            }
        }

        // ==========================================
        // 7. FUNCIONES DEL NAVEGADOR
        // ==========================================
        private void Navegar(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists) return;

                currentPath = path;
                cmbPath.Text = path;

                // ¡ESTO ARREGLA LA VISTA RARA! Reiniciamos las columnas del explorador
                listView1.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("Name", 200);
                listView1.Columns.Add("Size", 100);
                listView1.Columns.Add("Type", 100);
                listView1.Columns.Add("Date", 150);
                listView1.Columns.Add("Status", 80);

                foreach (var dir in di.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(dir.Name, 0);
                    item.SubItems.Add("");
                    item.SubItems.Add("Folder");
                    item.SubItems.Add(dir.LastWriteTime.ToString("yyyy-MM-dd"));
                    item.SubItems.Add("OK");
                    listView1.Items.Add(item);
                }

                foreach (var file in di.GetFiles())
                {
                    int iconIndex = GetIconIndex(file.Extension);
                    ListViewItem item = new ListViewItem(file.Name, iconIndex);
                    item.SubItems.Add((file.Length / 1024).ToString() + " KB");
                    item.SubItems.Add(file.Extension.ToUpper().Replace(".", ""));
                    item.SubItems.Add(file.LastWriteTime.ToString("yyyy-MM-dd"));
                    item.SubItems.Add("OK");
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex) { lblSelectionInfo.Text = "Error: " + ex.Message; }
        }

        private void LoadFilesAndDirectories(string path) { Navegar(path); }

        private int GetIconIndex(string ext)
        {
            switch (ext.ToLower())
            {
                case ".mp3": return 1;
                case ".mp4": return 2;
                case ".csv": case ".xlsx": return 3;
                default: return 4;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string carpetaDestino = e.Node.Text;
            string nuevaRuta = "";
            switch (carpetaDestino)
            {
                case "Documents": nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); break;
                case "Music": nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); break;
                case "Pictures": nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); break;
                case "Video":
                case "Videos": nuevaRuta = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos); break;
                case "Playlist": // ¡NUEVA LÓGICA PARA PLAYLIST!
                    CargarPlaylistAutomatica();
                    return;
                default: return;
            }
            if (!string.IsNullOrEmpty(nuevaRuta))
            {
                currentPath = nuevaRuta;
                txtSearch.Text = currentPath;
                LoadFilesAndDirectories(currentPath);
            }
        }

        private void CargarPlaylistAutomatica()
        {
            try
            {
                string rutaMusica = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                DirectoryInfo di = new DirectoryInfo(rutaMusica);

                playlist.Clear();
                listView1.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("Canciones de Mi Playlist", 300);
                // Buscamos 5 canciones
                foreach (var file in di.GetFiles("*.mp3"))
                {
                    playlist.Add(file.FullName);
                    listView1.Items.Add(new ListViewItem(file.Name, 1));
                    if (playlist.Count >= 5) break;
                }
                if (playlist.Count > 0)
                {
                    indicePlaylist = 0;
                    ReproducirMedia(playlist[indicePlaylist]);
                    MessageBox.Show("¡Playlist de 5 canciones cargada y lista para probar los botones Next/Prev!");
                }
                else { MessageBox.Show("No se encontraron archivos MP3 en tu carpeta de Música para crear la playlist."); }
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar playlist: " + ex.Message); }
        }

        private void CargarMetadatosFoto(string rutaArchivo)
        {
            try
            {
                using (Image img = Image.FromFile(rutaArchivo))
                {
                    var coordenadas = ObtenerCoordenadas(img);
                    if (coordenadas != null)
                    {
                        lblCoordenadas.Text = $"{coordenadas.Value.lat:F4}, {coordenadas.Value.lon:F4}";
                        MostrarMapa(coordenadas.Value.lat, coordenadas.Value.lon);
                    }
                    else { lblCoordenadas.Text = "Sin datos GPS"; }
                }
            }
            catch { }
        }

        private (double lat, double lon)? ObtenerCoordenadas(Image img)
        {
            try
            {
                if (!img.PropertyIdList.Contains(0x0002) || !img.PropertyIdList.Contains(0x0004)) return null;

                var latRef = System.Text.Encoding.ASCII.GetString(img.GetPropertyItem(0x0001).Value).Trim('\0');
                var lonRef = System.Text.Encoding.ASCII.GetString(img.GetPropertyItem(0x0003).Value).Trim('\0');

                double latitude = ConvertToDegrees(img.GetPropertyItem(0x0002).Value);
                double longitude = ConvertToDegrees(img.GetPropertyItem(0x0004).Value);

                if (latRef == "S") latitude = -latitude;
                if (lonRef == "W") longitude = -longitude;

                return (latitude, longitude);
            }
            catch { return null; }
        }

        private double ConvertToDegrees(byte[] value)
        {
            uint numG = BitConverter.ToUInt32(value, 0); uint denG = BitConverter.ToUInt32(value, 4);
            uint numM = BitConverter.ToUInt32(value, 8); uint denM = BitConverter.ToUInt32(value, 12);
            uint numS = BitConverter.ToUInt32(value, 16); uint denS = BitConverter.ToUInt32(value, 20);
            return (denG == 0 ? 0 : (double)numG / denG) + ((denM == 0 ? 0 : (double)numM / denM) / 60.0) + ((denS == 0 ? 0 : (double)numS / denS) / 3600.0);
        }

        private async void MostrarMapa(double lat, double lon)
        {
            await webMapa.EnsureCoreWebView2Async();
            string html = $@"<html><body style='margin:0'><iframe width='100%' height='100%' frameborder='0' style='border:0' src='https://maps.google.com/maps?q={lat.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lon.ToString(System.Globalization.CultureInfo.InvariantCulture)}&z=14&output=embed'></iframe></body></html>";
            webMapa.NavigateToString(html);
        }

        // Resto de eventos vacíos del diseñador para compatibilidad
        private void btnOptions_Click(object sender, EventArgs e) { panelProperties.Visible = !panelProperties.Visible; }
        private void btnEdit_Click(object sender, EventArgs e) { if (listView1.SelectedItems.Count > 0) listView1.SelectedItems[0].BeginEdit(); }
        private void toolStripTextBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void webMapa_Click(object sender, EventArgs e) { }
        private void btnViewCSV_Click(object sender, EventArgs e) { /* Usa la base que ya tenías */ }
    }


   
 }

    



