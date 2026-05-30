using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Explorerpr.Forms
{


    public partial class FrmImageEditor : Form
    {
        // ==========================================
        private string rutaImagenActual;
        private Image imagenEnMemoria;
        private long tamanoOriginalImagen;
        private int porcentajeZoom = 100;

        // Define los modos posibles para las herramientas
        private enum TipoHerramienta { Mano, Linea, Rectangulo, Elipse, Texto, Pincel, Borrador, Relleno, Zoom, Recortar }
        private TipoHerramienta herramientaActiva = TipoHerramienta.Mano;

        // Variables para controlar el mouse
        private bool mousePresionado = false;
        private Point puntoInicio;
        private Point puntoFin;
        private string textoFlotante = "";
        private Point posTextoImagen;
        private bool moviendoTexto = false;
        // Pila para Deshacer / Rehacer
        private Stack<Bitmap> undoStack = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();

        // Configuración del pincel/dibujo
        private Color colorActual = Color.Black;
        private float grosorPincel = 5f;

        public FrmImageEditor(string rutaArchivo)
        {
            InitializeComponent();
            this.rutaImagenActual = rutaArchivo;
            this.Load += FrmImageEditor_Load;

            tkbZoom.Minimum = 10;
            tkbZoom.Maximum = 400;
            tkbZoom.TickFrequency = 10;
            tkbZoom.Value = 100;

        }

        private void FrmImageEditor_Load(object sender, EventArgs e)
        {
            CargarImagenEnEditor(rutaImagenActual);
        }
        private void CambiarColor()
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = colorActual;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    colorActual = cd.Color;
                    lblColor.Text = $"Color: {colorActual.Name}";
                }
            }
        }
        private void lblColor_Click(object sender, EventArgs e) { CambiarColor(); }

        private void CargarImagenEnEditor(string ruta)
        {
            try
            {
                if (File.Exists(ruta))
                {
                    using (var tempImg = Image.FromFile(ruta))
                    {
                        // Aseguramos que la imagen abarque sus dimensiones reales al copiarse a la memoria
                        imagenEnMemoria = new Bitmap(tempImg.Width, tempImg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(imagenEnMemoria))
                        {
                            g.DrawImage(tempImg, new Rectangle(0, 0, tempImg.Width, tempImg.Height));
                        }
                    }

                    FileInfo fileInfo = new FileInfo(ruta);
                    tamanoOriginalImagen = fileInfo.Length;

                    // Restablecemos el lienzo para que ocupe todo el espacio
                    picCanvas.Dock = DockStyle.Fill;
                    picCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                    picCanvas.Image = new Bitmap(imagenEnMemoria);

                    // Forzamos el zoom inicial al 100% y lo reflejamos en la interfaz
                    porcentajeZoom = 100;
                    if (tkbZoom.Minimum <= 100 && tkbZoom.Maximum >= 100) tkbZoom.Value = 100;

                    undoStack.Clear();
                    redoStack.Clear();

                    LeerMetadatosExif(ruta);
                    PintarInterfazDatos();
                }
                else { MessageBox.Show("No se encontró el archivo de imagen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }



        private void LeerMetadatosExif(string ruta)
        {
            lvMetadatosGeneral.Items.Clear();
            lvMetadatosGeneral.View = View.Details;

            // Propiedades Base
            lvMetadatosGeneral.Items.Add(new ListViewItem(new[] { "Nombre", Path.GetFileName(ruta) }));
            lvMetadatosGeneral.Items.Add(new ListViewItem(new[] { "Extensión", Path.GetExtension(ruta).ToUpper() }));
            lvMetadatosGeneral.Items.Add(new ListViewItem(new[] { "Dimensiones", $"{imagenEnMemoria.Width} x {imagenEnMemoria.Height} px" }));
            lvMetadatosGeneral.Items.Add(new ListViewItem(new[] { "Peso", CalcularTamañoString(tamanoOriginalImagen) }));

            // Propiedades Avanzadas EXIF
            try
            {
                foreach (var prop in imagenEnMemoria.PropertyItems)
                {
                    string nombreMetadato = ObtenerNombreExif(prop.Id);
                    if (nombreMetadato != "Desconocido")
                    {
                        string valor = FormatearValorExif(prop);
                        lvMetadatosGeneral.Items.Add(new ListViewItem(new[] { nombreMetadato, valor }));
                    }
                }
            }
            catch { /* Algunas imágenes no tienen metadatos válidos */ }
        }

        private string ObtenerNombreExif(int id)
        {
            switch (id)
            {
                case 0x010F: return "Fabricante de Cámara";
                case 0x0110: return "Modelo de Cámara";
                case 0x8827: return "ISO";
                case 0x9003: return "Fecha de Captura";
                case 0x829A: return "Tiempo de Exposición";
                case 0x829D: return "Número F (Apertura)";
                case 0x920A: return "Longitud Focal";
                case 0x0112: return "Orientación";
                case 0x0131: return "Software / Editor";
                default: return "Desconocido"; // Omitimos los miles de tags técnicos raros
            }
        }

        private string FormatearValorExif(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 2 && prop.Value != null) // Texto
                return System.Text.Encoding.ASCII.GetString(prop.Value).Trim('\0');
            if (prop.Type == 3 && prop.Value.Length == 2) // Entero corto
                return BitConverter.ToUInt16(prop.Value, 0).ToString();
            if (prop.Type == 5 && prop.Value.Length == 8) // Racional (Fracción)
            {
                uint num = BitConverter.ToUInt32(prop.Value, 0);
                uint den = BitConverter.ToUInt32(prop.Value, 4);
                if (den != 0) return $"{num}/{den} ({(double)num / den:F2})";
            }
            return "Datos de cámara";
        }

        private void PintarInterfazDatos()
        {
            lblResolucion.Text = $"Resolución: {picCanvas.Image.Width} x {picCanvas.Image.Height}";
            lblTamaño.Text = $"Tamaño: {CalcularTamañoString(tamanoOriginalImagen)}";
            lblZoom.Text = $"Zoom: {porcentajeZoom}%";
            lblColor.Text = "Color: RGB";
        }

        private string CalcularTamañoString(long bytes)
        {
            string[] sufijos = { "Bytes", "KB", "MB", "GB" };
            double doubleBytes = bytes;
            int i = 0;
            while (doubleBytes >= 1024 && i < sufijos.Length - 1)
            {
                doubleBytes /= 1024;
                i++;
            }
            return $"{doubleBytes:0.00} {sufijos[i]}";
        }

        // =========================================================================
        // ACCIONES DE BOTONES (GUARDAR, EXPORTAR, DESHACER, ETC)
        // =========================================================================

        private void GuardarEstadoParaDeshacer()
        {
            if (picCanvas.Image != null)
            {
                undoStack.Push(new Bitmap(picCanvas.Image));
                redoStack.Clear(); // Si haces algo nuevo, pierdes el rehacer
            }
        }

        private void Deshacer_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new Bitmap(picCanvas.Image)); // Guardar estado actual en rehacer
                picCanvas.Image = undoStack.Pop();
                picCanvas.Refresh();
            }
        }

        private void Rehacer_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new Bitmap(picCanvas.Image)); // Guardar estado actual en deshacer
                picCanvas.Image = redoStack.Pop();
                picCanvas.Refresh();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (picCanvas.Image == null) return;
            try
            {
                // 1. Guardamos los cambios en un archivo temporal
                string tempPath = rutaImagenActual + ".temp";
                picCanvas.Image.Save(tempPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                // 2. Liberamos (desbloqueamos) la imagen original de la memoria RAM
                imagenEnMemoria?.Dispose();
                picCanvas.Image.Dispose();

                // 3. Borramos la original vieja y renombramos la nueva
                System.IO.File.Delete(rutaImagenActual);
                System.IO.File.Move(tempPath, rutaImagenActual);

                // 4. Volvemos a cargarla para que puedas seguir editando
                imagenEnMemoria = Image.FromFile(rutaImagenActual);
                picCanvas.Image = (Image)imagenEnMemoria.Clone();

                MessageBox.Show("Imagen guardada y reemplazada exitosamente.", "Éxito");
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }
        }

        private void GuardarComo_Click(object sender, EventArgs e)
        {
            if (picCanvas.Image == null) return;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Imagen JPEG|*.jpg|Imagen PNG|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    picCanvas.Image.Save(sfd.FileName);
                    MessageBox.Show("Copia guardada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Exportar_Click(object sender, EventArgs e)
        {
            if (picCanvas.Image == null) return;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Exportar Imagen";
                sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Imaging.ImageFormat formato = System.Drawing.Imaging.ImageFormat.Png;
                    string ext = Path.GetExtension(sfd.FileName).ToLower();
                    if (ext == ".jpg" || ext == ".jpeg") formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                    else if (ext == ".bmp") formato = System.Drawing.Imaging.ImageFormat.Bmp;
                    else if (ext == ".gif") formato = System.Drawing.Imaging.ImageFormat.Gif;

                    picCanvas.Image.Save(sfd.FileName, formato);
                    MessageBox.Show("Exportado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RotarIzq_Click(object sender, EventArgs e)
        {
            GuardarEstadoParaDeshacer();
            picCanvas.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            picCanvas.Refresh();
        }

        private void RotarDer_Click(object sender, EventArgs e)
        {
            GuardarEstadoParaDeshacer();
            picCanvas.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            picCanvas.Refresh();
        }

        private void Voltear_Click(object sender, EventArgs e)
        {
            GuardarEstadoParaDeshacer();
            picCanvas.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            picCanvas.Refresh();
        }

        private void Abrir_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.rutaImagenActual = ofd.FileName;
                    CargarImagenEnEditor(this.rutaImagenActual);
                }
            }
        }

        // =========================================================================
        // SELECCIÓN DE HERRAMIENTAS LATERALES
        // =========================================================================

        private void Mano_Click(object sender, EventArgs e) { herramientaActiva = TipoHerramienta.Mano; picCanvas.Cursor = Cursors.Hand; }
        private void Linea_Click(object sender, EventArgs e) { herramientaActiva = TipoHerramienta.Linea; picCanvas.Cursor = Cursors.Cross; }
        private void Rectangulo_Click(object sender, EventArgs e) { herramientaActiva = TipoHerramienta.Rectangulo; picCanvas.Cursor = Cursors.Cross; }
        private void Elipse_Click(object sender, EventArgs e) { herramientaActiva = TipoHerramienta.Elipse; picCanvas.Cursor = Cursors.Cross; }
        private void TextoLateral_Click(object sender, EventArgs e)
        {
            herramientaActiva = TipoHerramienta.Texto;
            picCanvas.Cursor = Cursors.IBeam;
            CambiarColor(); // Te deja elegir el color al seleccionar la herramienta
        }
        private void Texto_Click(object sender, EventArgs e) { TextoLateral_Click(sender, e); } // Para el botón superior
        private void Pincel_Click(object sender, EventArgs e)
        {
            herramientaActiva = TipoHerramienta.Pincel;
            picCanvas.Cursor = Cursors.Cross;
            CambiarColor(); // ¡Abre la paleta!
        }
        private void Borrador_Click(object sender, EventArgs e) { herramientaActiva = TipoHerramienta.Borrador; picCanvas.Cursor = Cursors.Cross; }
        private void Relleno_Click(object sender, EventArgs e)
        {
            herramientaActiva = TipoHerramienta.Relleno;
            picCanvas.Cursor = Cursors.Hand;
            CambiarColor(); // ¡Abre la paleta!
        }
        private void Recortar_Click(object sender, EventArgs e) { herramientaActiva = TipoHerramienta.Recortar; picCanvas.Cursor = Cursors.Cross; }
        private void Dibujar_Click(object sender, EventArgs e) { Pincel_Click(sender, e); } // Para el botón superior
        private void Zoom_Click(object sender, EventArgs e) { herramientaActiva = TipoHerramienta.Zoom; picCanvas.Cursor = Cursors.SizeAll; }

        // =========================================================================
        // LÓGICA DE DIBUJO Y MOUSE (CANVAS)
        // =========================================================================

        private Point inicioPan; // Para la herramienta Mano

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (picCanvas.Image == null) return;

            if (herramientaActiva == TipoHerramienta.Texto)
            {
                // Al dar clic, pedimos el texto
                textoFlotante = Microsoft.VisualBasic.Interaction.InputBox("Introduce el texto a dibujar:", "Herramienta Texto", "");
                if (!string.IsNullOrEmpty(textoFlotante))
                {
                    posTextoImagen = ObtenerCoordenadasRealesDeImagen(e.Location);
                    moviendoTexto = true; // Empieza a arrastrar inmediatamente
                    picCanvas.Refresh();
                }
                return;
            }

            if (herramientaActiva != TipoHerramienta.Mano && herramientaActiva != TipoHerramienta.Zoom)
                GuardarEstadoParaDeshacer();

            mousePresionado = true;
            puntoInicio = ObtenerCoordenadasRealesDeImagen(e.Location);

            if (herramientaActiva == TipoHerramienta.Mano) inicioPan = e.Location;
            else if (herramientaActiva == TipoHerramienta.Relleno)
            {
                Bitmap bmp = (Bitmap)picCanvas.Image;
                Color colorObjetivo = bmp.GetPixel(puntoInicio.X, puntoInicio.Y);
                RellenarArea(bmp, puntoInicio, colorObjetivo, colorActual);
                picCanvas.Refresh();
                mousePresionado = false;
            }
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (picCanvas.Image == null) return;

            if (moviendoTexto)
            {
                posTextoImagen = ObtenerCoordenadasRealesDeImagen(e.Location);
                picCanvas.Refresh(); // Esto dispara que se vea como flota el texto
                return;
            }

            if (!mousePresionado) return;

            if (herramientaActiva == TipoHerramienta.Mano)
            {
                if (picCanvas.Dock == DockStyle.Fill) picCanvas.Dock = DockStyle.None;
                picCanvas.Left += e.X - inicioPan.X;
                picCanvas.Top += e.Y - inicioPan.Y;
                return;
            }

            Point puntoActualImagen = ObtenerCoordenadasRealesDeImagen(e.Location);

            if (herramientaActiva == TipoHerramienta.Pincel || herramientaActiva == TipoHerramienta.Borrador)
            {
                using (Graphics g = Graphics.FromImage(picCanvas.Image))
                {
                    if (herramientaActiva == TipoHerramienta.Borrador)
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                    Color colorTrazo = (herramientaActiva == TipoHerramienta.Borrador) ? Color.Transparent : colorActual;

                    using (Pen lapiz = new Pen(colorTrazo, grosorPincel))
                    {
                        lapiz.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        lapiz.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLine(lapiz, puntoInicio, puntoActualImagen);
                    }
                }
                puntoInicio = puntoActualImagen;
                picCanvas.Refresh();
            }
            puntoFin = puntoActualImagen;
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mousePresionado || picCanvas.Image == null) return;
            mousePresionado = false;

            if (moviendoTexto)
            {
                moviendoTexto = false;
                // El usuario soltó el mouse, ¡imprimimos el texto permanentemente en la imagen!
                GuardarEstadoParaDeshacer();
                using (Graphics g = Graphics.FromImage(picCanvas.Image))
                {
                    using (Font fuente = new Font("Arial", 40, FontStyle.Bold))
                    using (Brush brocha = new SolidBrush(colorActual))
                    {
                        g.DrawString(textoFlotante, fuente, brocha, posTextoImagen);
                    }
                }
                textoFlotante = ""; // Limpiamos
                picCanvas.Refresh();
                return;
            }

            // Herramientas geométricas y Recorte
            using (Graphics g = Graphics.FromImage(picCanvas.Image))
            using (Pen lapiz = new Pen(colorActual, grosorPincel))
            {
                int ancho = Math.Abs(puntoFin.X - puntoInicio.X);
                int alto = Math.Abs(puntoFin.Y - puntoInicio.Y);
                int x = Math.Min(puntoInicio.X, puntoFin.X);
                int y = Math.Min(puntoInicio.Y, puntoFin.Y);

                if (herramientaActiva == TipoHerramienta.Linea)
                {
                    g.DrawLine(lapiz, puntoInicio, puntoFin);
                }
                else if (herramientaActiva == TipoHerramienta.Rectangulo)
                {
                    g.DrawRectangle(lapiz, x, y, ancho, alto);
                }
                else if (herramientaActiva == TipoHerramienta.Elipse)
                {
                    g.DrawEllipse(lapiz, x, y, ancho, alto);
                }
                else if (herramientaActiva == TipoHerramienta.Recortar && ancho > 0 && alto > 0)
                {
                    // Crear nueva imagen recortada
                    Rectangle rectRecorte = new Rectangle(x, y, ancho, alto);
                    Bitmap bmpRecortado = new Bitmap(ancho, alto);
                    using (Graphics gRecorte = Graphics.FromImage(bmpRecortado))
                    {
                        gRecorte.DrawImage(picCanvas.Image, new Rectangle(0, 0, ancho, alto), rectRecorte, GraphicsUnit.Pixel);
                    }
                    picCanvas.Image = bmpRecortado;
                }
            }
            picCanvas.Refresh();
            PintarInterfazDatos();
        }

        // =========================================================================
        // MÉTODOS AUXILIARES (TRADUCCIÓN DE COORDENADAS Y ALGORITMOS)
        // =========================================================================

        // IMPORTANTE: Como usamos SizeMode.Zoom, el ratón no concuerda directamente con los píxeles reales.
        private Point ObtenerCoordenadasRealesDeImagen(Point mouseP)
        {
            if (picCanvas.SizeMode == PictureBoxSizeMode.Normal || picCanvas.SizeMode == PictureBoxSizeMode.AutoSize)
                return mouseP; // 1 a 1

            // Cálculo para SizeMode.Zoom
            PropertyInfo imageRectangleProperty = typeof(PictureBox).GetProperty("ImageRectangle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (imageRectangleProperty != null)
            {
                Rectangle rect = (Rectangle)imageRectangleProperty.GetValue(picCanvas, null);
                if (rect.Contains(mouseP))
                {
                    int realX = (int)((mouseP.X - rect.X) * (double)picCanvas.Image.Width / rect.Width);
                    int realY = (int)((mouseP.Y - rect.Y) * (double)picCanvas.Image.Height / rect.Height);

                    // Limitar dentro de los bordes
                    realX = Math.Max(0, Math.Min(realX, picCanvas.Image.Width - 1));
                    realY = Math.Max(0, Math.Min(realY, picCanvas.Image.Height - 1));

                    return new Point(realX, realY);
                }
            }
            return mouseP;
        }

        // Algoritmo para la herramienta Bote de Pintura (Flood Fill básico)
        private void RellenarArea(Bitmap bmp, Point pt, Color colorObjetivo, Color colorRelleno)
        {
            if (colorObjetivo.ToArgb() == colorRelleno.ToArgb()) return;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int[] pixels = new int[bmp.Width * bmp.Height];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);

            int targetR = colorObjetivo.R, targetG = colorObjetivo.G, targetB = colorObjetivo.B;
            int replacementArgb = colorRelleno.ToArgb();

            Stack<Point> stack = new Stack<Point>();
            stack.Push(pt);

            // Tolerancia para rellenar fondos de fotos o dibujos no perfectos (puedes subirla a 60 o bajarla a 20)
            int tolerancia = 40;

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int index = p.Y * bmp.Width + p.X;

                // Si este píxel ya fue rellenado, lo saltamos
                if (pixels[index] == replacementArgb) continue;

                int pixelR = (pixels[index] >> 16) & 0xFF;
                int pixelG = (pixels[index] >> 8) & 0xFF;
                int pixelB = pixels[index] & 0xFF;

                // Comparamos si el color es parecido al color que hicimos clic
                if (Math.Abs(pixelR - targetR) <= tolerancia &&
                    Math.Abs(pixelG - targetG) <= tolerancia &&
                    Math.Abs(pixelB - targetB) <= tolerancia)
                {
                    pixels[index] = replacementArgb;
                    if (p.X > 0) stack.Push(new Point(p.X - 1, p.Y));
                    if (p.X < bmp.Width - 1) stack.Push(new Point(p.X + 1, p.Y));
                    if (p.Y > 0) stack.Push(new Point(p.X, p.Y - 1));
                    if (p.Y < bmp.Height - 1) stack.Push(new Point(p.X, p.Y + 1));
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bmp.UnlockBits(data);
        }

        // =========================================================================
        // LÓGICA DE CONTROL DE ZOOM (BARRA INFERIOR)
        // =========================================================================

        private void AplicarZoom()
        {
            if (picCanvas.Image == null || imagenEnMemoria == null) return;

            if (porcentajeZoom < 10) porcentajeZoom = 10;
            if (porcentajeZoom > 400) porcentajeZoom = 400;

            picCanvas.SizeMode = PictureBoxSizeMode.Zoom;

            // Simular zoom modificando el tamaño del PictureBox (requiere un Panel contenedor con AutoScroll)
            int nuevoAncho = (int)(imagenEnMemoria.Width * (porcentajeZoom / 100.0f));
            int nuevoAlto = (int)(imagenEnMemoria.Height * (porcentajeZoom / 100.0f));

            picCanvas.Width = nuevoAncho;
            picCanvas.Height = nuevoAlto;

            lblZoom.Text = $"Zoom: {porcentajeZoom}%";

            if (tkbZoom.Value != porcentajeZoom && porcentajeZoom >= tkbZoom.Minimum && porcentajeZoom <= tkbZoom.Maximum)
                tkbZoom.Value = porcentajeZoom;
        }

        private void tkbZoom_Scroll(object sender, EventArgs e)
        {
            porcentajeZoom = tkbZoom.Value;
            AplicarZoom();
        }

        private void btnZoomMenos_Click(object sender, EventArgs e)
        {
            porcentajeZoom -= 10;
            AplicarZoom();
        }

        private void btnZoomMas_Click(object sender, EventArgs e)
        {
            porcentajeZoom += 10;
            AplicarZoom();
        }

        private void btnAjustarVentana_Click(object sender, EventArgs e)
        {
            if (picCanvas.Image == null) return;

            picCanvas.Dock = DockStyle.Fill;
            picCanvas.SizeMode = PictureBoxSizeMode.Zoom;

            porcentajeZoom = 100;
            lblZoom.Text = "Zoom: Ajustado";
            tkbZoom.Value = 100;
        }

        // Mantengo tus eventos vacíos generados por el diseñador por compatibilidad
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (moviendoTexto && !string.IsNullOrEmpty(textoFlotante))
            {
                // Convierte las coordenadas de la imagen a las coordenadas actuales de tu pantalla (para el zoom)
                Point posicionPantalla = picCanvas.PointToClient(MousePosition);
                e.Graphics.DrawString(textoFlotante, new Font("Arial", 40, FontStyle.Bold), new SolidBrush(colorActual), posicionPantalla);
            }
        }
    }
}





