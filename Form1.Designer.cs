namespace Explorerpr
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            TreeNode treeNode1 = new TreeNode("Music", 5, 5);
            TreeNode treeNode2 = new TreeNode("Project", 4, 4);
            TreeNode treeNode3 = new TreeNode("Documents", 1, 1, new TreeNode[] { treeNode1, treeNode2 });
            TreeNode treeNode4 = new TreeNode("Video", 2, 2);
            TreeNode treeNode5 = new TreeNode("Liz", 4, 4);
            TreeNode treeNode6 = new TreeNode("Pictures", new TreeNode[] { treeNode5 });
            TreeNode treeNode7 = new TreeNode("My Jams", 6, 6);
            TreeNode treeNode8 = new TreeNode("Playlist", -2, -2, new TreeNode[] { treeNode7 });
            TreeNode treeNode9 = new TreeNode("This PC", 0, 0, new TreeNode[] { treeNode3, treeNode4, treeNode6, treeNode8 });
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            toolStripButton4 = new ToolStripButton();
            toolStripButton5 = new ToolStripButton();
            cmbPath = new ToolStripComboBox();
            txtSearch = new ToolStripTextBox();
            toolStripLabel1 = new ToolStripLabel();
            toolStripSeparator5 = new ToolStripSeparator();
            newFolder = new ToolStripDropDownButton();
            newFileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripButton6 = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripButton7 = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripButton8 = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripButton9 = new ToolStripButton();
            imageList1 = new ImageList(components);
            treeView1 = new TreeView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox1 = new GroupBox();
            btnPrevTrack = new Button();
            btnNextTrack = new Button();
            lblTime = new Label();
            trackBarVolume = new TrackBar();
            btnMediaDetails = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            trackBarProgress = new TrackBar();
            lblMediaTitle = new Label();
            lblMainTitle = new Label();
            picAlbumArt = new PictureBox();
            panelProperties = new GroupBox();
            panel1 = new Panel();
            lblDate = new Label();
            lblCoordenadas = new Label();
            lblModifiedValue = new Label();
            lblTypeValue = new Label();
            webMapa = new Microsoft.Web.WebView2.WinForms.WebView2();
            label8 = new Label();
            label7 = new Label();
            label1 = new Label();
            label6 = new Label();
            label5 = new Label();
            lblSelectedType = new Label();
            lblSelectedName = new Label();
            picSelectedIcon = new PictureBox();
            listView1 = new ListView();
            chName = new ColumnHeader();
            chSize = new ColumnHeader();
            chType = new ColumnHeader();
            chDate = new ColumnHeader();
            chStatus = new ColumnHeader();
            statusStrip1 = new StatusStrip();
            lblSelectionInfo = new ToolStripStatusLabel();
            toolStripSplitButton1 = new ToolStripSplitButton();
            BtnGrabarAudio = new ToolStripDropDownButton();
            btnDescargarDB = new ToolStripDropDownButton();
            btnEnviarCorreo = new ToolStripDropDownButton();
            timerMusic = new System.Windows.Forms.Timer(components);
            toolStrip1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarProgress).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAlbumArt).BeginInit();
            panelProperties.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webMapa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSelectedIcon).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2, toolStripButton3, toolStripButton4, toolStripButton5, cmbPath, txtSearch, toolStripLabel1, toolStripSeparator5, newFolder, toolStripSeparator4, toolStripButton6, toolStripSeparator3, toolStripButton7, toolStripSeparator1, toolStripButton8, toolStripSeparator2, toolStripButton9 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = ToolStripRenderMode.System;
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.Image = Properties.Resources._5690080;
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(52, 22);
            toolStripButton1.Text = "Back";
            toolStripButton1.Click += btnBack_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.Image = Properties.Resources.left_arrow;
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(70, 22);
            toolStripButton2.Text = "Forward";
            toolStripButton2.Click += btnForward_Click;
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = Properties.Resources.arrow;
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(23, 22);
            toolStripButton3.Text = "Up";
            toolStripButton3.Click += btnUp_Click;
            // 
            // toolStripButton4
            // 
            toolStripButton4.Image = Properties.Resources.left_arrow1;
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new Size(66, 22);
            toolStripButton4.Text = "Refresh";
            // 
            // toolStripButton5
            // 
            toolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton5.Image = Properties.Resources.home;
            toolStripButton5.ImageTransparentColor = Color.Magenta;
            toolStripButton5.Name = "toolStripButton5";
            toolStripButton5.Size = new Size(23, 22);
            toolStripButton5.Text = "Home";
            // 
            // cmbPath
            // 
            cmbPath.Name = "cmbPath";
            cmbPath.Size = new Size(121, 25);
            // 
            // txtSearch
            // 
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(100, 25);
            txtSearch.Text = "Search  ⌕";
            txtSearch.KeyDown += txtSearch_KeyDown;
            txtSearch.Click += toolStripTextBox1_Click;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(0, 22);
            toolStripLabel1.Text = "toolStripLabel1";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 25);
            // 
            // newFolder
            // 
            newFolder.DropDownItems.AddRange(new ToolStripItem[] { newFileToolStripMenuItem, newToolStripMenuItem });
            newFolder.Image = Properties.Resources.new_folder;
            newFolder.ImageTransparentColor = Color.Magenta;
            newFolder.Name = "newFolder";
            newFolder.Size = new Size(60, 22);
            newFolder.Text = "New";
            newFolder.Click += btnNewFolder_Click;
            // 
            // newFileToolStripMenuItem
            // 
            newFileToolStripMenuItem.Image = Properties.Resources.file;
            newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            newFileToolStripMenuItem.Size = new Size(134, 22);
            newFileToolStripMenuItem.Text = "New File";
            newFileToolStripMenuItem.Click += newFileToolStripMenuItem_Click;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = Properties.Resources.folder;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(134, 22);
            newToolStripMenuItem.Text = "New Folder";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 25);
            // 
            // toolStripButton6
            // 
            toolStripButton6.Image = Properties.Resources.edit;
            toolStripButton6.ImageTransparentColor = Color.Magenta;
            toolStripButton6.Name = "toolStripButton6";
            toolStripButton6.Size = new Size(47, 22);
            toolStripButton6.Text = "Edit";
            toolStripButton6.Click += btnEdit_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            // 
            // toolStripButton7
            // 
            toolStripButton7.Image = Properties.Resources.bin;
            toolStripButton7.ImageTransparentColor = Color.Magenta;
            toolStripButton7.Name = "toolStripButton7";
            toolStripButton7.Size = new Size(60, 22);
            toolStripButton7.Text = "Delete";
            toolStripButton7.Click += btnDelete_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripButton8
            // 
            toolStripButton8.Image = Properties.Resources.settings;
            toolStripButton8.ImageTransparentColor = Color.Magenta;
            toolStripButton8.Name = "toolStripButton8";
            toolStripButton8.Size = new Size(69, 22);
            toolStripButton8.Text = "Options";
            toolStripButton8.Click += btnOptions_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // toolStripButton9
            // 
            toolStripButton9.Image = Properties.Resources.upload;
            toolStripButton9.ImageTransparentColor = Color.Magenta;
            toolStripButton9.Name = "toolStripButton9";
            toolStripButton9.Size = new Size(61, 22);
            toolStripButton9.Text = "Sql Up";
            toolStripButton9.Click += btnSqlUp_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "This PC");
            imageList1.Images.SetKeyName(1, "Documents");
            imageList1.Images.SetKeyName(2, "Video");
            imageList1.Images.SetKeyName(3, "Pictures");
            imageList1.Images.SetKeyName(4, "Projects");
            imageList1.Images.SetKeyName(5, "music");
            imageList1.Images.SetKeyName(6, "player");
            imageList1.Images.SetKeyName(7, "file.png");
            // 
            // treeView1
            // 
            treeView1.HotTracking = true;
            treeView1.ImageIndex = 0;
            treeView1.ImageList = imageList1;
            treeView1.Location = new Point(0, 28);
            treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 5;
            treeNode1.Name = "Nodo2";
            treeNode1.SelectedImageIndex = 5;
            treeNode1.Text = "Music";
            treeNode2.ImageIndex = 4;
            treeNode2.Name = "Project";
            treeNode2.SelectedImageIndex = 4;
            treeNode2.Text = "Project";
            treeNode3.ImageIndex = 1;
            treeNode3.Name = "Nodo1";
            treeNode3.SelectedImageIndex = 1;
            treeNode3.Text = "Documents";
            treeNode4.ImageIndex = 2;
            treeNode4.Name = "Nodo4";
            treeNode4.SelectedImageIndex = 2;
            treeNode4.Text = "Video";
            treeNode5.ImageIndex = 4;
            treeNode5.Name = "Nodo3";
            treeNode5.SelectedImageIndex = 4;
            treeNode5.Text = "Liz";
            treeNode6.ImageKey = "Pictures";
            treeNode6.Name = "Nodo5";
            treeNode6.SelectedImageIndex = 3;
            treeNode6.Text = "Pictures";
            treeNode7.ImageIndex = 6;
            treeNode7.Name = "Nodo7";
            treeNode7.SelectedImageIndex = 6;
            treeNode7.Text = "My Jams";
            treeNode8.ImageIndex = -2;
            treeNode8.Name = "Nodo6";
            treeNode8.SelectedImageIndex = -2;
            treeNode8.Text = "Playlist";
            treeNode9.ImageIndex = 0;
            treeNode9.Name = "Nodo0";
            treeNode9.SelectedImageIndex = 0;
            treeNode9.Text = "This PC";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode9 });
            treeView1.SelectedImageIndex = 0;
            treeView1.Size = new Size(136, 395);
            treeView1.TabIndex = 1;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(groupBox1);
            flowLayoutPanel1.Controls.Add(panelProperties);
            flowLayoutPanel1.Dock = DockStyle.Right;
            flowLayoutPanel1.Location = new Point(600, 25);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(200, 425);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnPrevTrack);
            groupBox1.Controls.Add(btnNextTrack);
            groupBox1.Controls.Add(lblTime);
            groupBox1.Controls.Add(trackBarVolume);
            groupBox1.Controls.Add(btnMediaDetails);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(trackBarProgress);
            groupBox1.Controls.Add(lblMediaTitle);
            groupBox1.Controls.Add(lblMainTitle);
            groupBox1.Controls.Add(picAlbumArt);
            groupBox1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 152);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Dynamic Media";
            // 
            // btnPrevTrack
            // 
            btnPrevTrack.FlatStyle = FlatStyle.Flat;
            btnPrevTrack.ImageAlign = ContentAlignment.MiddleLeft;
            btnPrevTrack.Location = new Point(7, 90);
            btnPrevTrack.Name = "btnPrevTrack";
            btnPrevTrack.Size = new Size(17, 23);
            btnPrevTrack.TabIndex = 12;
            btnPrevTrack.Text = "⏮";
            btnPrevTrack.UseVisualStyleBackColor = true;
            btnPrevTrack.Click += btnPrevTrack_Click;
            // 
            // btnNextTrack
            // 
            btnNextTrack.FlatStyle = FlatStyle.Flat;
            btnNextTrack.ImageAlign = ContentAlignment.MiddleLeft;
            btnNextTrack.Location = new Point(177, 90);
            btnNextTrack.Name = "btnNextTrack";
            btnNextTrack.Size = new Size(17, 23);
            btnNextTrack.TabIndex = 11;
            btnNextTrack.Text = "⏭";
            btnNextTrack.UseVisualStyleBackColor = true;
            btnNextTrack.Click += btnNextTrack_Click;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Location = new Point(86, 105);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(17, 15);
            lblTime.TabIndex = 10;
            lblTime.Text = "--";
            // 
            // trackBarVolume
            // 
            trackBarVolume.AutoSize = false;
            trackBarVolume.LargeChange = 10;
            trackBarVolume.Location = new Point(86, 123);
            trackBarVolume.Maximum = 100;
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Size = new Size(77, 23);
            trackBarVolume.TabIndex = 8;
            trackBarVolume.TickStyle = TickStyle.None;
            trackBarVolume.Value = 50;
            trackBarVolume.Scroll += trackBarVolume_Scroll;
            // 
            // btnMediaDetails
            // 
            btnMediaDetails.Location = new Point(165, 123);
            btnMediaDetails.Name = "btnMediaDetails";
            btnMediaDetails.Size = new Size(35, 23);
            btnMediaDetails.TabIndex = 7;
            btnMediaDetails.Text = "📑";
            btnMediaDetails.UseVisualStyleBackColor = true;
            btnMediaDetails.Click += btnMediaDetails_Click;
            // 
            // button3
            // 
            button3.Location = new Point(60, 123);
            button3.Name = "button3";
            button3.Size = new Size(28, 23);
            button3.TabIndex = 6;
            button3.Text = "⏹️";
            button3.UseVisualStyleBackColor = true;
            button3.Click += btnStop_Click;
            // 
            // button2
            // 
            button2.Location = new Point(29, 123);
            button2.Name = "button2";
            button2.Size = new Size(25, 23);
            button2.TabIndex = 5;
            button2.Text = "⏸️";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnPause_Click;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(6, 123);
            button1.Name = "button1";
            button1.Size = new Size(17, 23);
            button1.TabIndex = 4;
            button1.Text = "▶️";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnPlay_Click;
            // 
            // trackBarProgress
            // 
            trackBarProgress.AutoSize = false;
            trackBarProgress.Location = new Point(29, 87);
            trackBarProgress.Name = "trackBarProgress";
            trackBarProgress.Size = new Size(145, 15);
            trackBarProgress.TabIndex = 3;
            trackBarProgress.TickStyle = TickStyle.None;
            trackBarProgress.MouseDown += trackBarProgress_MouseDown;
            trackBarProgress.MouseUp += trackBarProgress_MouseUp;
            // 
            // lblMediaTitle
            // 
            lblMediaTitle.AutoSize = true;
            lblMediaTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMediaTitle.Location = new Point(94, 57);
            lblMediaTitle.Name = "lblMediaTitle";
            lblMediaTitle.Size = new Size(40, 15);
            lblMediaTitle.TabIndex = 2;
            lblMediaTitle.Text = "label2";
            // 
            // lblMainTitle
            // 
            lblMainTitle.AutoSize = true;
            lblMainTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMainTitle.Location = new Point(94, 19);
            lblMainTitle.Name = "lblMainTitle";
            lblMainTitle.Size = new Size(92, 15);
            lblMainTitle.TabIndex = 1;
            lblMainTitle.Text = "-----------------";
            // 
            // picAlbumArt
            // 
            picAlbumArt.Location = new Point(6, 22);
            picAlbumArt.Name = "picAlbumArt";
            picAlbumArt.Size = new Size(82, 62);
            picAlbumArt.SizeMode = PictureBoxSizeMode.Zoom;
            picAlbumArt.TabIndex = 0;
            picAlbumArt.TabStop = false;
            // 
            // panelProperties
            // 
            panelProperties.Controls.Add(panel1);
            panelProperties.Controls.Add(lblSelectedType);
            panelProperties.Controls.Add(lblSelectedName);
            panelProperties.Controls.Add(picSelectedIcon);
            panelProperties.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            panelProperties.Location = new Point(3, 161);
            panelProperties.Name = "panelProperties";
            panelProperties.Size = new Size(200, 264);
            panelProperties.TabIndex = 4;
            panelProperties.TabStop = false;
            panelProperties.Text = "File Properties";
            // 
            // panel1
            // 
            panel1.Controls.Add(lblDate);
            panel1.Controls.Add(lblCoordenadas);
            panel1.Controls.Add(lblModifiedValue);
            panel1.Controls.Add(lblTypeValue);
            panel1.Controls.Add(webMapa);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Location = new Point(0, 76);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 188);
            panel1.TabIndex = 4;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(6, 30);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(36, 15);
            lblDate.TabIndex = 10;
            lblDate.Text = "Date:";
            // 
            // lblCoordenadas
            // 
            lblCoordenadas.AutoSize = true;
            lblCoordenadas.Location = new Point(97, 54);
            lblCoordenadas.Name = "lblCoordenadas";
            lblCoordenadas.Size = new Size(13, 15);
            lblCoordenadas.TabIndex = 9;
            lblCoordenadas.Text = "..";
            lblCoordenadas.Click += lblCoordenadas_Click;
            // 
            // lblModifiedValue
            // 
            lblModifiedValue.AutoSize = true;
            lblModifiedValue.Location = new Point(60, 30);
            lblModifiedValue.Name = "lblModifiedValue";
            lblModifiedValue.Size = new Size(13, 15);
            lblModifiedValue.TabIndex = 7;
            lblModifiedValue.Text = "..";
            lblModifiedValue.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblTypeValue
            // 
            lblTypeValue.AutoSize = true;
            lblTypeValue.Location = new Point(71, 0);
            lblTypeValue.Name = "lblTypeValue";
            lblTypeValue.Size = new Size(13, 15);
            lblTypeValue.TabIndex = 6;
            lblTypeValue.Text = "..";
            // 
            // webMapa
            // 
            webMapa.AllowExternalDrop = true;
            webMapa.CreationProperties = null;
            webMapa.DefaultBackgroundColor = Color.White;
            webMapa.Location = new Point(6, 87);
            webMapa.Name = "webMapa";
            webMapa.Size = new Size(187, 95);
            webMapa.TabIndex = 5;
            webMapa.ZoomFactor = 1D;
            webMapa.Click += webMapa_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(0, 69);
            label8.Name = "label8";
            label8.Size = new Size(82, 15);
            label8.TabIndex = 4;
            label8.Text = "Geo-Location:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(97, 0);
            label7.Name = "label7";
            label7.Size = new Size(89, 15);
            label7.TabIndex = 3;
            label7.Text = "Full Methadata";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 54);
            label1.Name = "label1";
            label1.Size = new Size(83, 15);
            label1.TabIndex = 2;
            label1.Text = "Coordenadas:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 15);
            label6.Name = "label6";
            label6.Size = new Size(0, 15);
            label6.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 0);
            label5.Name = "label5";
            label5.Size = new Size(36, 15);
            label5.TabIndex = 0;
            label5.Text = "Type:";
            // 
            // lblSelectedType
            // 
            lblSelectedType.AutoSize = true;
            lblSelectedType.Location = new Point(86, 46);
            lblSelectedType.Name = "lblSelectedType";
            lblSelectedType.Size = new Size(17, 15);
            lblSelectedType.TabIndex = 3;
            lblSelectedType.Text = "--";
            // 
            // lblSelectedName
            // 
            lblSelectedName.AutoSize = true;
            lblSelectedName.Location = new Point(86, 22);
            lblSelectedName.Name = "lblSelectedName";
            lblSelectedName.Size = new Size(17, 15);
            lblSelectedName.TabIndex = 2;
            lblSelectedName.Text = "--";
            // 
            // picSelectedIcon
            // 
            picSelectedIcon.Image = Properties.Resources.folder;
            picSelectedIcon.Location = new Point(6, 22);
            picSelectedIcon.Name = "picSelectedIcon";
            picSelectedIcon.Size = new Size(74, 48);
            picSelectedIcon.SizeMode = PictureBoxSizeMode.Zoom;
            picSelectedIcon.TabIndex = 1;
            picSelectedIcon.TabStop = false;
            picSelectedIcon.Click += pictureBox2_Click;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { chName, chSize, chType, chDate, chStatus });
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.LabelEdit = true;
            listView1.Location = new Point(145, 28);
            listView1.Name = "listView1";
            listView1.Size = new Size(452, 395);
            listView1.TabIndex = 3;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.Click += listView1_SelectedIndexChanged;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            // 
            // chName
            // 
            chName.Text = "Name";
            chName.Width = 120;
            // 
            // chSize
            // 
            chSize.Text = "Size";
            chSize.Width = 80;
            // 
            // chType
            // 
            chType.Text = "Type";
            chType.Width = 100;
            // 
            // chDate
            // 
            chDate.Text = "Date";
            chDate.Width = 95;
            // 
            // chStatus
            // 
            chStatus.Text = "Status";
            chStatus.Width = 44;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblSelectionInfo, toolStripSplitButton1, BtnGrabarAudio, btnDescargarDB, btnEnviarCorreo });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(600, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblSelectionInfo
            // 
            lblSelectionInfo.Name = "lblSelectionInfo";
            lblSelectionInfo.Size = new Size(32, 17);
            lblSelectionInfo.Text = "-----";
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.Image = (Image)resources.GetObject("toolStripSplitButton1.Image");
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new Size(32, 20);
            toolStripSplitButton1.Text = "toolStripSplitButton1";
            toolStripSplitButton1.ButtonClick += toolStripSplitButton1_ButtonClick;
            // 
            // BtnGrabarAudio
            // 
            BtnGrabarAudio.Image = (Image)resources.GetObject("BtnGrabarAudio.Image");
            BtnGrabarAudio.Name = "BtnGrabarAudio";
            BtnGrabarAudio.Size = new Size(29, 20);
            BtnGrabarAudio.Click += btnGrabarAudio_Click;
            // 
            // btnDescargarDB
            // 
            btnDescargarDB.Image = Properties.Resources.download;
            btnDescargarDB.Name = "btnDescargarDB";
            btnDescargarDB.Size = new Size(29, 20);
            btnDescargarDB.Click += btnDescargarDB_Click;
            // 
            // btnEnviarCorreo
            // 
            btnEnviarCorreo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnEnviarCorreo.Image = Properties.Resources.gmail;
            btnEnviarCorreo.ImageTransparentColor = Color.Magenta;
            btnEnviarCorreo.Name = "btnEnviarCorreo";
            btnEnviarCorreo.Size = new Size(29, 20);
            btnEnviarCorreo.Click += btnEnviarCorreo_Click;
            // 
            // timerMusic
            // 
            timerMusic.Interval = 1000;
            timerMusic.Tick += timerMusic_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(listView1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(treeView1);
            Controls.Add(toolStrip1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load_1;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarProgress).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAlbumArt).EndInit();
            panelProperties.ResumeLayout(false);
            panelProperties.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webMapa).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSelectedIcon).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private ToolStripComboBox cmbPath;
        private ToolStripTextBox txtSearch;
        private ToolStripLabel toolStripLabel1;
        private ToolStripDropDownButton newFolder;
        private ToolStripMenuItem newFileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripButton toolStripButton6;
        private ToolStripButton toolStripButton7;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButton8;
        private ToolStripButton toolStripButton9;
        private ImageList imageList1;
        private TreeView treeView1;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox panelProperties;
        private Button button1;
        private TrackBar trackBarProgress;
        private Label lblMediaTitle;
        private Label lblMainTitle;
        private PictureBox picAlbumArt;
        private Button button2;
        private TrackBar trackBarVolume;
        private Button btnMediaDetails;
        private Button button3;
        private PictureBox picSelectedIcon;
        private Label lblSelectedType;
        private Label lblSelectedName;
        private Panel panel1;
        private Label label6;
        private Label label5;
        private Label label8;
        private Label label7;
        private Label label1;
        private Label lblCoordenadas;
        private Label label10;
        private Label lblTypeValue;
        private Microsoft.Web.WebView2.WinForms.WebView2 webMapa;
        private ListView listView1;
        private ColumnHeader chName;
        private ColumnHeader chSize;
        private ColumnHeader chType;
        private ColumnHeader chDate;
        private ColumnHeader chStatus;
        private StatusStrip statusStrip1;
        private Label lblTime;
        private System.Windows.Forms.Timer timerMusic;
        private Label lblModifiedValue;
        private Label lblDate;
        private ToolStripStatusLabel lblSelectionInfo;
        private ToolStripDropDownButton BtnGrabarAudio;
        private Button btnPrevTrack;
        private Button btnNextTrack;
        private ToolStripDropDownButton btnDescargarDB;
        private ToolStripDropDownButton btnEnviarCorreo;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSplitButton toolStripSplitButton1;
    }
}
