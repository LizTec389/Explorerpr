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
// ¡NUEVAS LIBRERÍAS PARA LEER EXCEL!
using ExcelDataReader;


namespace Explorerpr
{
    public partial class Form1 : Form
    {
        // ==========================================
        // LIBRERÍAS Y VARIABLES PARA LA CÁMARA WEB Y AUDIO NATIVO
        // ==========================================
        [System.Runtime.InteropServices.DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        private static extern IntPtr capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hwndParent, int nID);

        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SendMessage")]
        private static extern int SendMessageString(IntPtr hWnd, uint Msg, int wParam, string lParam);

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnString, int returnLength, IntPtr hwndCallback);

        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CAP_START = 0x0400;
        private const int WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10;
        private const int WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11;
        private const int WM_CAP_EDIT_COPY = WM_CAP_START + 30;
        private const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
        private const int WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52;
        private const int WM_CAP_SET_SCALE = WM_CAP_START + 53;
        private const int WM_CAP_FILE_SET_CAPTURE_FILEA = WM_CAP_START + 20;
        private const int WM_CAP_SEQUENCE = WM_CAP_START + 62;

        private Stack<string> backStack = new Stack<string>();
        private Stack<string> forwardStack = new Stack<string>();
        private string currentPath = @"C:\Users\liz\";
        private double currentLat;
        private double currentLon;
        string connectionString = @"Server=LIZETH; Database=ExplorerDB; Integrated Security=True; TrustServerCertificate=True;";

        bool isDragging = false;
        private List<string> playlist = new List<string>();
        private int indicePlaylist = 0;

        public Form1()
        {
            InitializeComponent();
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
                if (term.StartsWith("."))
                {
                    try
                    {
                        listView1.Clear();
                        listView1.View = View.Details;
                        listView1.Columns.Add("Archivo Encontrado", 400);
                        listView1.Columns.Add("Tamaño", 100);
                        listView1.Columns.Add("Tipo", 80);
                        BuscarArchivosSeguro(new DirectoryInfo(currentPath), term);
                        if (listView1.Items.Count == 0)
                            MessageBox.Show($"No se encontraron archivos con extensión {term}");
                        else
                            MessageBox.Show($"Se encontraron {listView1.Items.Count} archivos con extensión {term}");
                    }
                    catch (Exception ex) { MessageBox.Show("Error general: " + ex.Message); }
                }
                else
                {
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

        private void BuscarArchivosSeguro(DirectoryInfo carpeta, string extension)
        {
            try
            {
                foreach (var file in carpeta.GetFiles("*" + extension))
                {
                    ListViewItem item = new ListViewItem(file.FullName);
                    item.SubItems.Add((file.Length / 1024).ToString() + " KB");
                    item.SubItems.Add(file.Extension.ToUpper().Replace(".", ""));
                    listView1.Items.Add(item);
                }
                foreach (var subDir in carpeta.GetDirectories())
                {
                    try { BuscarArchivosSeguro(subDir, extension); }
                    catch { }
                }
            }
            catch { }
        }

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
                    var fileTag = TagLib.File.Create(fullPath);
                    info += $"🎵 Detalles Multimedia:\n" +
                            $"Título: {fileTag.Tag.Title}\n" +
                            $"Álbum: {fileTag.Tag.Album}\n" +
                            $"Duración: {fileTag.Properties.Duration:mm\\:ss}";
                }
                catch { }

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
                smtp.Credentials = new NetworkCredential("lizethcarrizales42@gmail.com", "shjcpwvfujwszenb");
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

            // LOGICA NUEVA: Decifrar texto de Excel si es necesario
            string textoParaMostrar = "";
            if (ruta.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) || ruta.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = File.Open(ruta, FileMode.Open, FileAccess.Read))
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var tabla = reader.AsDataSet().Tables[0];
                        foreach (DataRow row in tabla.Rows)
                        {
                            textoParaMostrar += string.Join(" | ", row.ItemArray) + Environment.NewLine;
                        }
                    }
                }
                catch (Exception ex) { textoParaMostrar = "Error al leer Excel: " + ex.Message; }
            }
            else
            {
                textoParaMostrar = File.ReadAllText(ruta);
            }

            TextBox txtContenido = new TextBox { Multiline = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Both, Text = textoParaMostrar, Font = new Font("Consolas", 11) };

            Panel panelAbajo = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            Button btnGuardar = new Button { Text = "Guardar Cambios", Dock = DockStyle.Right, Width = 150 };
            Button btnGuardarComo = new Button { Text = "Guardar Como...", Dock = DockStyle.Right, Width = 150 };

            btnGuardar.Click += (s, ev) =>
            {
                // Evitamos dañar los archivos de Excel si se intentan guardar como texto plano directo
                if (ruta.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) || ruta.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("No se pueden sobrescribir archivos Excel nativos desde aquí. Usa 'Guardar Como...' para exportarlo como CSV o Texto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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
                        LoadFilesAndDirectories(currentPath);
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

                // LOGICA NUEVA: Descifrar Excel para la tabla
                if (ext == ".xlsx" || ext == ".xls")
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = File.Open(ruta, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var tablaExcel = result.Tables[0]; // Tomamos la Hoja 1

                            // Creamos las columnas en el ListView
                            for (int i = 0; i < tablaExcel.Columns.Count; i++)
                            {
                                listView1.Columns.Add("Columna " + (i + 1), 120);
                            }

                            // Llenamos los datos fila por fila
                            for (int r = 0; r < tablaExcel.Rows.Count; r++)
                            {
                                var fila = tablaExcel.Rows[r];
                                ListViewItem item = new ListViewItem(fila[0]?.ToString() ?? "");
                                for (int c = 1; c < tablaExcel.Columns.Count; c++)
                                {
                                    item.SubItems.Add(fila[c]?.ToString() ?? "");
                                }
                                listView1.Items.Add(item);
                            }
                        }
                    }
                }
                else if (ext == ".csv" || ext == ".txt")
                {
                    string contenido = File.ReadAllText(ruta);
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
                else if (ext == ".json" || ext == ".xml")
                {
                    string contenido = File.ReadAllText(ruta);
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
                // LOGICA NUEVA: Agregamos Excel a la condición
                else if (extension == ".txt" || extension == ".csv" || extension == ".json" || extension == ".xml" || extension == ".xlsx" || extension == ".xls")
                {
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

        // ==========================================
        // 5. SELECCIÓN, CARGA Y NUEVO REPRODUCTOR (SIN WMP)
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
                    ReproducirMedia(fullPath);
                }
                else if (extension == "jpg" || extension == "jpeg" || extension == "png")
                {
                    // Apagar música nativa
                    mciSendString("close ReproductorPropio", null, 0, IntPtr.Zero);
                    timerMusic.Stop();
                    picAlbumArt.Image = Image.FromFile(fullPath);
                    CargarMetadatosFoto(fullPath);
                    lblMediaTitle.Text = sel.Text;
                }
                else
                {
                    mciSendString("close ReproductorPropio", null, 0, IntPtr.Zero);
                    timerMusic.Stop();
                }
            }
        }

        private async void ReproducirMedia(string rutaArchivo)
        {
            // Cerramos cualquier archivo de audio previo
            mciSendString("close ReproductorPropio", null, 0, IntPtr.Zero);

            // Abrimos el nuevo MP3 y le damos play
            mciSendString($"open \"{rutaArchivo}\" type mpegvideo alias ReproductorPropio", null, 0, IntPtr.Zero);
            mciSendString("play ReproductorPropio", null, 0, IntPtr.Zero);

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

            timerMusic.Start();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            mciSendString("play ReproductorPropio", null, 0, IntPtr.Zero);
            timerMusic.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            mciSendString("pause ReproductorPropio", null, 0, IntPtr.Zero);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mciSendString("close ReproductorPropio", null, 0, IntPtr.Zero);
            timerMusic.Stop();
            trackBarProgress.Value = 0;
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            int volumenReal = trackBarVolume.Value * 10;
            mciSendString($"setaudio ReproductorPropio volume to {volumenReal}", null, 0, IntPtr.Zero);
        }

        private void timerMusic_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                StringBuilder sbLongitud = new StringBuilder(128);
                mciSendString("status ReproductorPropio length", sbLongitud, 128, IntPtr.Zero);

                StringBuilder sbPosicion = new StringBuilder(128);
                mciSendString("status ReproductorPropio position", sbPosicion, 128, IntPtr.Zero);

                int longitud = 0, posicion = 0;
                int.TryParse(sbLongitud.ToString(), out longitud);
                int.TryParse(sbPosicion.ToString(), out posicion);

                if (longitud > 0)
                {
                    trackBarProgress.Maximum = longitud;
                    if (posicion <= longitud) trackBarProgress.Value = posicion;

                    TimeSpan t = TimeSpan.FromMilliseconds(posicion);
                    lblTime.Text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

                    // LOGICA DE PLAYLIST: Si la canción terminó (posicion >= longitud), avanza
                    if (posicion >= longitud && longitud > 0)
                    {
                        timerMusic.Stop();
                        btnNextTrack_Click(null, null);
                    }
                }
            }
        }

        private void trackBarProgress_MouseDown(object sender, MouseEventArgs e) { isDragging = true; }
        private void trackBarProgress_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            mciSendString($"seek ReproductorPropio to {trackBarProgress.Value}", null, 0, IntPtr.Zero);
            mciSendString("play ReproductorPropio", null, 0, IntPtr.Zero);
        }

        // ==========================================
        // GRABADORA DE VIDEO (REEMPLAZÓ A LA DE VOZ)
        // ==========================================
        private void btnGrabarAudio_Click(object sender, EventArgs e)
        {
            Form frmVideo = new Form { Text = "🔴 Grabando Video...", Size = new Size(640, 520), StartPosition = FormStartPosition.CenterParent };
            PictureBox picVideo = new PictureBox { Dock = DockStyle.Fill };
            Button btnDetener = new Button { Text = "⏹ DETENER Y GUARDAR VIDEO", Dock = DockStyle.Bottom, Height = 50, BackColor = Color.Red, ForeColor = Color.White, Font = new Font("Arial", 12, FontStyle.Bold) };

            frmVideo.Controls.Add(picVideo);
            frmVideo.Controls.Add(btnDetener);

            IntPtr hWndC = capCreateCaptureWindowA("Video", WS_CHILD | WS_VISIBLE, 0, 0, 640, 480, picVideo.Handle, 0);

            if (SendMessage(hWndC, WM_CAP_DRIVER_CONNECT, 0, 0) != 0)
            {
                SendMessage(hWndC, WM_CAP_SET_SCALE, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEWRATE, 66, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEW, 1, 0);

                string rutaPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                string rutaVideo = System.IO.Path.Combine(rutaPictures, "Video_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".avi");

                SendMessageString(hWndC, WM_CAP_FILE_SET_CAPTURE_FILEA, 0, rutaVideo);
                SendMessage(hWndC, WM_CAP_SEQUENCE, 0, 0);

                btnDetener.Click += (s, ev) => { frmVideo.Close(); };

                frmVideo.FormClosing += (s, ev) =>
                {
                    SendMessage(hWndC, WM_CAP_DRIVER_DISCONNECT, 0, 0);
                    MessageBox.Show("¡Video guardado con éxito en tu carpeta de Imágenes!\n\nRuta: " + rutaVideo, "Video Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (currentPath == rutaPictures) { LoadFilesAndDirectories(currentPath); }
                };

                frmVideo.ShowDialog();
            }
            else
            {
                MessageBox.Show("No se detectó cámara web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Funciones de Playlist
        private void btnCargarPlaylist_Click(object sender, EventArgs e)
        {
            playlist.Clear();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    playlist.Add(Path.Combine(currentPath, item.Text));
                    if (playlist.Count >= 5) break;
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
                if (indicePlaylist >= playlist.Count) indicePlaylist = 0;
                ReproducirMedia(playlist[indicePlaylist]);
            }
        }

        private void btnPrevTrack_Click(object sender, EventArgs e)
        {
            if (playlist.Count > 0)
            {
                indicePlaylist--;
                if (indicePlaylist < 0) indicePlaylist = playlist.Count - 1;
                ReproducirMedia(playlist[indicePlaylist]);
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
        // SQL Y OTRAS FUNCIONES SIN CAMBIOS
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
                        MessageBox.Show("¡Archivo subido exitosamente!");
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
                                sw.WriteLine("ID, Nombre, Extension, Tamano, Ruta");
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

        private void Navegar(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists) return;

                currentPath = path;
                cmbPath.Text = path;

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
                case "Playlist":
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

        private void AbrirCamaraWeb()
        {
            Form frmCamara = new Form { Text = "Cámara Web - Captura", Size = new Size(640, 520), StartPosition = FormStartPosition.CenterParent };
            PictureBox picVideo = new PictureBox { Dock = DockStyle.Fill };
            Button btnTomarFoto = new Button { Text = "📸 Tomar Foto y Guardar", Dock = DockStyle.Bottom, Height = 50, Font = new Font("Arial", 12, FontStyle.Bold) };

            frmCamara.Controls.Add(picVideo);
            frmCamara.Controls.Add(btnTomarFoto);

            IntPtr hWndC = capCreateCaptureWindowA("Video", WS_CHILD | WS_VISIBLE, 0, 0, 640, 480, picVideo.Handle, 0);

            if (SendMessage(hWndC, WM_CAP_DRIVER_CONNECT, 0, 0) != 0)
            {
                SendMessage(hWndC, WM_CAP_SET_SCALE, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEWRATE, 66, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEW, 1, 0);

                btnTomarFoto.Click += (s, ev) =>
                {
                    SendMessage(hWndC, WM_CAP_EDIT_COPY, 0, 0);

                    if (Clipboard.ContainsImage())
                    {
                        Image foto = Clipboard.GetImage();
                        string rutaPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        string nombreArchivo = "Captura_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";
                        string rutaFinal = System.IO.Path.Combine(rutaPictures, nombreArchivo);

                        foto.Save(rutaFinal, System.Drawing.Imaging.ImageFormat.Jpeg);
                        MessageBox.Show("¡Foto capturada y guardada en tu carpeta de Imágenes!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (currentPath == rutaPictures) { LoadFilesAndDirectories(currentPath); }
                    }
                };

                frmCamara.FormClosing += (s, ev) => { SendMessage(hWndC, WM_CAP_DRIVER_DISCONNECT, 0, 0); };
                frmCamara.ShowDialog();
            }
            else { MessageBox.Show("No se detectó ninguna cámara web conectada o está siendo usada por otro programa.", "Error de Cámara", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // Resto de eventos
        private void btnOptions_Click(object sender, EventArgs e) { panelProperties.Visible = !panelProperties.Visible; }
        private void btnEdit_Click(object sender, EventArgs e) { if (listView1.SelectedItems.Count > 0) listView1.SelectedItems[0].BeginEdit(); }
        private void toolStripTextBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void webMapa_Click(object sender, EventArgs e) { }
        private void btnViewCSV_Click(object sender, EventArgs e) { }
        private void lblCoordenadas_Click(object sender, EventArgs e) { }
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e) { AbrirCamaraWeb(); }
    }
}