namespace Explorerpr.Forms
{
    partial class FrmImageEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImageEditor));
            toolEditor = new ToolStrip();
            btnAbrir = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnGuardar = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnGuardarComo = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            btnRotarIzq = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            btnRotarDer = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            btnVoltear = new ToolStripButton();
            toolStripSeparator6 = new ToolStripSeparator();
            btnRecortar = new ToolStripButton();
            toolStripSeparator7 = new ToolStripSeparator();
            btnDibujar = new ToolStripButton();
            toolStripSeparator8 = new ToolStripSeparator();
            btnTexto = new ToolStripButton();
            toolStripSeparator9 = new ToolStripSeparator();
            btnExportar = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            btnDeshacer = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            btnRehacer = new ToolStripButton();
            toolStripSeparator12 = new ToolStripSeparator();
            toolStripButton13 = new ToolStripButton();
            toolStrip1 = new ToolStrip();
            toolStripSeparator13 = new ToolStripSeparator();
            btnMano = new ToolStripButton();
            toolStripSeparator14 = new ToolStripSeparator();
            btnLinea = new ToolStripButton();
            toolStripSeparator15 = new ToolStripSeparator();
            btnRectangulo = new ToolStripButton();
            toolStripSeparator16 = new ToolStripSeparator();
            btnElipse = new ToolStripButton();
            toolStripSeparator17 = new ToolStripSeparator();
            btnTextoLateral = new ToolStripButton();
            toolStripSeparator18 = new ToolStripSeparator();
            btnPincel = new ToolStripButton();
            toolStripSeparator19 = new ToolStripSeparator();
            btnBorrador = new ToolStripButton();
            toolStripSeparator20 = new ToolStripSeparator();
            btnRelleno = new ToolStripButton();
            toolStripSeparator21 = new ToolStripSeparator();
            btnZoom = new ToolStripButton();
            tabControl1 = new TabControl();
            tab = new TabPage();
            lvMetadatosGeneral = new ListView();
            Propiedad = new ColumnHeader();
            Valor = new ColumnHeader();
            picCanvas = new PictureBox();
            panel2 = new Panel();
            btnAjustarVentana = new Button();
            button2 = new Button();
            tkbZoom = new TrackBar();
            button1 = new Button();
            lblZoomPorcentaje = new Label();
            statusStrip1 = new StatusStrip();
            lblResolucion = new ToolStripStatusLabel();
            lblTamaño = new ToolStripStatusLabel();
            lblZoom = new ToolStripStatusLabel();
            lblColor = new ToolStripStatusLabel();
            toolEditor.SuspendLayout();
            toolStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tkbZoom).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolEditor
            // 
            toolEditor.GripStyle = ToolStripGripStyle.Hidden;
            toolEditor.Items.AddRange(new ToolStripItem[] { btnAbrir, toolStripSeparator1, btnGuardar, toolStripSeparator2, btnGuardarComo, toolStripSeparator3, btnRotarIzq, toolStripSeparator4, btnRotarDer, toolStripSeparator5, btnVoltear, toolStripSeparator6, btnRecortar, toolStripSeparator7, btnDibujar, toolStripSeparator8, btnTexto, toolStripSeparator9, btnExportar, toolStripSeparator10, btnDeshacer, toolStripSeparator11, btnRehacer, toolStripSeparator12, toolStripButton13 });
            toolEditor.Location = new Point(0, 0);
            toolEditor.Name = "toolEditor";
            toolEditor.Size = new Size(800, 38);
            toolEditor.TabIndex = 0;
            toolEditor.Text = "toolStrip1";
            // 
            // btnAbrir
            // 
            btnAbrir.ForeColor = Color.Black;
            btnAbrir.Image = (Image)resources.GetObject("btnAbrir.Image");
            btnAbrir.ImageTransparentColor = Color.Magenta;
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(37, 35);
            btnAbrir.Text = "Abrir";
            btnAbrir.TextImageRelation = TextImageRelation.ImageAboveText;
            btnAbrir.Click += Abrir_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 38);
            // 
            // btnGuardar
            // 
            btnGuardar.ForeColor = Color.Black;
            btnGuardar.Image = (Image)resources.GetObject("btnGuardar.Image");
            btnGuardar.ImageTransparentColor = Color.Magenta;
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(53, 35);
            btnGuardar.Text = "Guardar";
            btnGuardar.TextImageRelation = TextImageRelation.ImageAboveText;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 38);
            // 
            // btnGuardarComo
            // 
            btnGuardarComo.ForeColor = Color.Black;
            btnGuardarComo.Image = (Image)resources.GetObject("btnGuardarComo.Image");
            btnGuardarComo.ImageTransparentColor = Color.Magenta;
            btnGuardarComo.Name = "btnGuardarComo";
            btnGuardarComo.Size = new Size(86, 35);
            btnGuardarComo.Text = "GuardarComo";
            btnGuardarComo.TextImageRelation = TextImageRelation.ImageAboveText;
            btnGuardarComo.Click += GuardarComo_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 38);
            // 
            // btnRotarIzq
            // 
            btnRotarIzq.ForeColor = Color.Black;
            btnRotarIzq.Image = (Image)resources.GetObject("btnRotarIzq.Image");
            btnRotarIzq.ImageTransparentColor = Color.Magenta;
            btnRotarIzq.Name = "btnRotarIzq";
            btnRotarIzq.Size = new Size(57, 35);
            btnRotarIzq.Text = "Rotar Izq";
            btnRotarIzq.TextImageRelation = TextImageRelation.ImageAboveText;
            btnRotarIzq.Click += RotarIzq_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 38);
            // 
            // btnRotarDer
            // 
            btnRotarDer.ForeColor = Color.Black;
            btnRotarDer.Image = (Image)resources.GetObject("btnRotarDer.Image");
            btnRotarDer.ImageTransparentColor = Color.Magenta;
            btnRotarDer.Name = "btnRotarDer";
            btnRotarDer.Size = new Size(60, 35);
            btnRotarDer.Text = "Rotar Der";
            btnRotarDer.TextImageRelation = TextImageRelation.ImageAboveText;
            btnRotarDer.Click += RotarDer_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 38);
            // 
            // btnVoltear
            // 
            btnVoltear.ForeColor = Color.Black;
            btnVoltear.Image = (Image)resources.GetObject("btnVoltear.Image");
            btnVoltear.ImageTransparentColor = Color.Magenta;
            btnVoltear.Name = "btnVoltear";
            btnVoltear.Size = new Size(47, 35);
            btnVoltear.Text = "Voltear";
            btnVoltear.TextImageRelation = TextImageRelation.ImageAboveText;
            btnVoltear.Click += Voltear_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 38);
            // 
            // btnRecortar
            // 
            btnRecortar.ForeColor = Color.Black;
            btnRecortar.Image = (Image)resources.GetObject("btnRecortar.Image");
            btnRecortar.ImageTransparentColor = Color.Magenta;
            btnRecortar.Name = "btnRecortar";
            btnRecortar.Size = new Size(55, 35);
            btnRecortar.Text = "Recortar";
            btnRecortar.TextImageRelation = TextImageRelation.ImageAboveText;
            btnRecortar.Click += Recortar_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 38);
            // 
            // btnDibujar
            // 
            btnDibujar.ForeColor = Color.Black;
            btnDibujar.Image = (Image)resources.GetObject("btnDibujar.Image");
            btnDibujar.ImageTransparentColor = Color.Magenta;
            btnDibujar.Name = "btnDibujar";
            btnDibujar.Size = new Size(49, 35);
            btnDibujar.Text = "Dibujar";
            btnDibujar.TextImageRelation = TextImageRelation.ImageAboveText;
            btnDibujar.Click += Dibujar_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(6, 38);
            // 
            // btnTexto
            // 
            btnTexto.ForeColor = Color.Black;
            btnTexto.Image = (Image)resources.GetObject("btnTexto.Image");
            btnTexto.ImageTransparentColor = Color.Magenta;
            btnTexto.Name = "btnTexto";
            btnTexto.Size = new Size(39, 35);
            btnTexto.Text = "Texto";
            btnTexto.TextImageRelation = TextImageRelation.ImageAboveText;
            btnTexto.Click += Texto_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 38);
            // 
            // btnExportar
            // 
            btnExportar.ForeColor = Color.Black;
            btnExportar.Image = (Image)resources.GetObject("btnExportar.Image");
            btnExportar.ImageTransparentColor = Color.Magenta;
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(54, 35);
            btnExportar.Text = "Exportar";
            btnExportar.TextImageRelation = TextImageRelation.ImageAboveText;
            btnExportar.Click += Exportar_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 38);
            // 
            // btnDeshacer
            // 
            btnDeshacer.ForeColor = Color.Black;
            btnDeshacer.Image = (Image)resources.GetObject("btnDeshacer.Image");
            btnDeshacer.ImageTransparentColor = Color.Magenta;
            btnDeshacer.Name = "btnDeshacer";
            btnDeshacer.Size = new Size(59, 35);
            btnDeshacer.Text = "Deshacer";
            btnDeshacer.TextImageRelation = TextImageRelation.ImageAboveText;
            btnDeshacer.Click += Deshacer_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 38);
            // 
            // btnRehacer
            // 
            btnRehacer.ForeColor = Color.Black;
            btnRehacer.Image = (Image)resources.GetObject("btnRehacer.Image");
            btnRehacer.ImageTransparentColor = Color.Magenta;
            btnRehacer.Name = "btnRehacer";
            btnRehacer.Size = new Size(53, 35);
            btnRehacer.Text = "Rehacer";
            btnRehacer.TextImageRelation = TextImageRelation.ImageAboveText;
            btnRehacer.Click += Rehacer_Click;
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(6, 38);
            // 
            // toolStripButton13
            // 
            toolStripButton13.Image = (Image)resources.GetObject("toolStripButton13.Image");
            toolStripButton13.ImageTransparentColor = Color.Magenta;
            toolStripButton13.Name = "toolStripButton13";
            toolStripButton13.Size = new Size(23, 35);
            toolStripButton13.TextImageRelation = TextImageRelation.ImageAboveText;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.Left;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripSeparator13, btnMano, toolStripSeparator14, btnLinea, toolStripSeparator15, btnRectangulo, toolStripSeparator16, btnElipse, toolStripSeparator17, btnTextoLateral, toolStripSeparator18, btnPincel, toolStripSeparator19, btnBorrador, toolStripSeparator20, btnRelleno, toolStripSeparator21, btnZoom });
            toolStrip1.Location = new Point(0, 38);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = ToolStripRenderMode.Professional;
            toolStrip1.Size = new Size(88, 472);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new Size(85, 6);
            // 
            // btnMano
            // 
            btnMano.Image = (Image)resources.GetObject("btnMano.Image");
            btnMano.ImageTransparentColor = Color.Magenta;
            btnMano.Name = "btnMano";
            btnMano.Size = new Size(85, 20);
            btnMano.Text = "Mano";
            btnMano.Click += Mano_Click;
            // 
            // toolStripSeparator14
            // 
            toolStripSeparator14.Name = "toolStripSeparator14";
            toolStripSeparator14.Size = new Size(85, 6);
            // 
            // btnLinea
            // 
            btnLinea.Image = (Image)resources.GetObject("btnLinea.Image");
            btnLinea.ImageTransparentColor = Color.Magenta;
            btnLinea.Name = "btnLinea";
            btnLinea.Size = new Size(85, 20);
            btnLinea.Text = "Linea";
            btnLinea.Click += Linea_Click;
            // 
            // toolStripSeparator15
            // 
            toolStripSeparator15.Name = "toolStripSeparator15";
            toolStripSeparator15.Size = new Size(85, 6);
            // 
            // btnRectangulo
            // 
            btnRectangulo.Image = (Image)resources.GetObject("btnRectangulo.Image");
            btnRectangulo.ImageTransparentColor = Color.Magenta;
            btnRectangulo.Name = "btnRectangulo";
            btnRectangulo.Size = new Size(85, 20);
            btnRectangulo.Text = "Rectangulo";
            btnRectangulo.Click += Rectangulo_Click;
            // 
            // toolStripSeparator16
            // 
            toolStripSeparator16.Name = "toolStripSeparator16";
            toolStripSeparator16.Size = new Size(85, 6);
            // 
            // btnElipse
            // 
            btnElipse.Image = (Image)resources.GetObject("btnElipse.Image");
            btnElipse.ImageTransparentColor = Color.Magenta;
            btnElipse.Name = "btnElipse";
            btnElipse.Size = new Size(85, 20);
            btnElipse.Text = "Elipse";
            btnElipse.Click += Elipse_Click;
            // 
            // toolStripSeparator17
            // 
            toolStripSeparator17.Name = "toolStripSeparator17";
            toolStripSeparator17.Size = new Size(85, 6);
            // 
            // btnTextoLateral
            // 
            btnTextoLateral.Image = (Image)resources.GetObject("btnTextoLateral.Image");
            btnTextoLateral.ImageTransparentColor = Color.Magenta;
            btnTextoLateral.Name = "btnTextoLateral";
            btnTextoLateral.Size = new Size(85, 20);
            btnTextoLateral.Text = "Texto";
            btnTextoLateral.Click += TextoLateral_Click;
            // 
            // toolStripSeparator18
            // 
            toolStripSeparator18.Name = "toolStripSeparator18";
            toolStripSeparator18.Size = new Size(85, 6);
            // 
            // btnPincel
            // 
            btnPincel.Image = (Image)resources.GetObject("btnPincel.Image");
            btnPincel.ImageTransparentColor = Color.Magenta;
            btnPincel.Name = "btnPincel";
            btnPincel.Size = new Size(85, 20);
            btnPincel.Text = "Pincel";
            btnPincel.Click += Pincel_Click;
            // 
            // toolStripSeparator19
            // 
            toolStripSeparator19.Name = "toolStripSeparator19";
            toolStripSeparator19.Size = new Size(85, 6);
            // 
            // btnBorrador
            // 
            btnBorrador.Image = (Image)resources.GetObject("btnBorrador.Image");
            btnBorrador.ImageTransparentColor = Color.Magenta;
            btnBorrador.Name = "btnBorrador";
            btnBorrador.Size = new Size(85, 20);
            btnBorrador.Text = "Borrador";
            btnBorrador.Click += Borrador_Click;
            // 
            // toolStripSeparator20
            // 
            toolStripSeparator20.Name = "toolStripSeparator20";
            toolStripSeparator20.Size = new Size(85, 6);
            // 
            // btnRelleno
            // 
            btnRelleno.Image = (Image)resources.GetObject("btnRelleno.Image");
            btnRelleno.ImageTransparentColor = Color.Magenta;
            btnRelleno.Name = "btnRelleno";
            btnRelleno.Size = new Size(85, 20);
            btnRelleno.Text = "Relleno";
            btnRelleno.Click += Relleno_Click;
            // 
            // toolStripSeparator21
            // 
            toolStripSeparator21.Name = "toolStripSeparator21";
            toolStripSeparator21.Size = new Size(85, 6);
            // 
            // btnZoom
            // 
            btnZoom.Image = (Image)resources.GetObject("btnZoom.Image");
            btnZoom.ImageTransparentColor = Color.Magenta;
            btnZoom.Name = "btnZoom";
            btnZoom.Size = new Size(85, 20);
            btnZoom.Text = "Zoom";
            btnZoom.Click += Zoom_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tab);
            tabControl1.Dock = DockStyle.Right;
            tabControl1.Location = new Point(600, 38);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(200, 472);
            tabControl1.TabIndex = 2;
            // 
            // tab
            // 
            tab.Controls.Add(lvMetadatosGeneral);
            tab.Location = new Point(4, 26);
            tab.Name = "tab";
            tab.Padding = new Padding(3);
            tab.Size = new Size(192, 442);
            tab.TabIndex = 1;
            tab.Text = "General";
            tab.UseVisualStyleBackColor = true;
            // 
            // lvMetadatosGeneral
            // 
            lvMetadatosGeneral.Columns.AddRange(new ColumnHeader[] { Propiedad, Valor });
            lvMetadatosGeneral.FullRowSelect = true;
            lvMetadatosGeneral.GridLines = true;
            lvMetadatosGeneral.Location = new Point(0, 0);
            lvMetadatosGeneral.Name = "lvMetadatosGeneral";
            lvMetadatosGeneral.Size = new Size(192, 442);
            lvMetadatosGeneral.TabIndex = 0;
            lvMetadatosGeneral.UseCompatibleStateImageBehavior = false;
            lvMetadatosGeneral.View = View.Details;
            // 
            // Propiedad
            // 
            Propiedad.Text = "Propiedad";
            Propiedad.Width = 111;
            // 
            // Valor
            // 
            Valor.Text = "Valor";
            Valor.Width = 111;
            // 
            // picCanvas
            // 
            picCanvas.Location = new Point(91, 41);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(507, 337);
            picCanvas.TabIndex = 3;
            picCanvas.TabStop = false;
            picCanvas.Click += pictureBox1_Click;
            picCanvas.Paint += picCanvas_Paint;
            picCanvas.MouseDown += picCanvas_MouseDown;
            picCanvas.MouseMove += picCanvas_MouseMove;
            picCanvas.MouseUp += picCanvas_MouseUp;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnAjustarVentana);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(tkbZoom);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(lblZoomPorcentaje);
            panel2.Location = new Point(91, 384);
            panel2.Name = "panel2";
            panel2.Size = new Size(504, 50);
            panel2.TabIndex = 4;
            panel2.Paint += panel2_Paint;
            // 
            // btnAjustarVentana
            // 
            btnAjustarVentana.BackColor = Color.Silver;
            btnAjustarVentana.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAjustarVentana.Location = new Point(396, 10);
            btnAjustarVentana.Name = "btnAjustarVentana";
            btnAjustarVentana.Size = new Size(99, 31);
            btnAjustarVentana.TabIndex = 7;
            btnAjustarVentana.Text = "Ajustar Ventana";
            btnAjustarVentana.UseVisualStyleBackColor = false;
            btnAjustarVentana.Click += btnAjustarVentana_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Silver;
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(372, 10);
            button2.Name = "button2";
            button2.Size = new Size(18, 26);
            button2.TabIndex = 8;
            button2.Text = "➕";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btnZoomMas_Click;
            // 
            // tkbZoom
            // 
            tkbZoom.Location = new Point(110, 10);
            tkbZoom.Name = "tkbZoom";
            tkbZoom.Size = new Size(256, 45);
            tkbZoom.TabIndex = 7;
            tkbZoom.Scroll += tkbZoom_Scroll;
            // 
            // button1
            // 
            button1.BackColor = Color.Silver;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(58, 8);
            button1.Name = "button1";
            button1.Size = new Size(20, 30);
            button1.TabIndex = 6;
            button1.Text = "➖";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnZoomMenos_Click;
            // 
            // lblZoomPorcentaje
            // 
            lblZoomPorcentaje.AutoSize = true;
            lblZoomPorcentaje.Location = new Point(13, 15);
            lblZoomPorcentaje.Name = "lblZoomPorcentaje";
            lblZoomPorcentaje.Size = new Size(21, 19);
            lblZoomPorcentaje.TabIndex = 5;
            lblZoomPorcentaje.Text = "--";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblResolucion, lblTamaño, lblZoom, lblColor });
            statusStrip1.Location = new Point(88, 488);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(512, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblResolucion
            // 
            lblResolucion.Name = "lblResolucion";
            lblResolucion.Size = new Size(91, 17);
            lblResolucion.Text = "Resolucion:  0x0";
            lblResolucion.Click += toolStripStatusLabel1_Click;
            // 
            // lblTamaño
            // 
            lblTamaño.Name = "lblTamaño";
            lblTamaño.Size = new Size(101, 17);
            lblTamaño.Text = "Tamaño:  0.00 MB";
            // 
            // lblZoom
            // 
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(73, 17);
            lblZoom.Text = "Zoom: 100%";
            // 
            // lblColor
            // 
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(64, 17);
            lblColor.Text = "Color: RGB";
            lblColor.Click += lblColor_Click;
            // 
            // FrmImageEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(800, 510);
            Controls.Add(statusStrip1);
            Controls.Add(panel2);
            Controls.Add(picCanvas);
            Controls.Add(tabControl1);
            Controls.Add(toolStrip1);
            Controls.Add(toolEditor);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.Black;
            Name = "FrmImageEditor";
            Text = "Editor de Imagenes";
            Load += FrmImageEditor_Load;
            toolEditor.ResumeLayout(false);
            toolEditor.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tkbZoom).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolEditor;
        private ToolStripButton btnAbrir;
        private ToolStripButton btnGuardar;
        private ToolStripButton btnGuardarComo;
        private ToolStripButton btnRotarIzq;
        private ToolStripButton btnRotarDer;
        private ToolStripButton btnVoltear;
        private ToolStripButton btnRecortar;
        private ToolStripButton btnDibujar;
        private ToolStripButton btnTexto;
        private ToolStripButton btnExportar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripButton btnDeshacer;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton btnRehacer;
        private ToolStripButton toolStripButton13;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStrip toolStrip1;
        private ToolStripButton btnMano;
        private ToolStripButton btnLinea;
        private ToolStripButton btnRectangulo;
        private ToolStripButton btnElipse;
        private ToolStripButton btnTextoLateral;
        private ToolStripButton btnPincel;
        private ToolStripButton btnBorrador;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripSeparator toolStripSeparator16;
        private ToolStripSeparator toolStripSeparator17;
        private ToolStripSeparator toolStripSeparator18;
        private ToolStripSeparator toolStripSeparator19;
        private ToolStripSeparator toolStripSeparator20;
        private ToolStripButton btnRelleno;
        private ToolStripSeparator toolStripSeparator21;
        private ToolStripButton btnZoom;
        private TabControl tabControl1;
        private TabPage tab;
        private ListView lvMetadatosGeneral;
        private ColumnHeader Propiedad;
        private ColumnHeader Valor;
        private PictureBox picCanvas;
        private Panel panel2;
        private Button button2;
        private TrackBar tkbZoom;
        private Button button1;
        private Label lblZoomPorcentaje;
        private Button btnAjustarVentana;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblResolucion;
        private ToolStripStatusLabel lblTamaño;
        private ToolStripStatusLabel lblZoom;
        private ToolStripStatusLabel lblColor;
    }
}