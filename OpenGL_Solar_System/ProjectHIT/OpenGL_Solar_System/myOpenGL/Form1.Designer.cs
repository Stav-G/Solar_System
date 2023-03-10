namespace myOpenGL
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public int pointY = 50;
        public int pointPlanetY = 500;
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar4 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar5 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar6 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar9 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar8 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar7 = new System.Windows.Forms.HScrollBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.hScrollBar13 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar11 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar12 = new System.Windows.Forms.HScrollBar();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            this.groupBox3.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.radioButton1)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.radioButton2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Location = new System.Drawing.Point(12, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 600);//515, 500
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar1.Maximum = 200;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar1.TabIndex = 7;
            this.hScrollBar1.Value = 110;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
           // System.Console.WriteLine(this.hScrollBar1.Location);
            pointY += 20;
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Location = new System.Drawing.Point(850, pointY); //(550, 37)
            this.hScrollBar2.Maximum = 200;
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar2.TabIndex = 8;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            //System.Console.WriteLine(this.hScrollBar2.Location);
            pointY += 20;
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar3.Maximum = 200;
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar3.TabIndex = 9;
            this.hScrollBar3.Value = 200;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            pointY += 20;
            // 
            // hScrollBar4
            // 
            this.hScrollBar4.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar4.Maximum = 200;
            this.hScrollBar4.Name = "hScrollBar4";
            this.hScrollBar4.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar4.TabIndex = 10;
            this.hScrollBar4.Value = 100;
            this.hScrollBar4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            pointY += 20;
            // 
            // hScrollBar5
            // 
            this.hScrollBar5.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar5.Maximum = 200;
            this.hScrollBar5.Name = "hScrollBar5";
            this.hScrollBar5.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar5.TabIndex = 8;
            this.hScrollBar5.Value = 100;
            this.hScrollBar5.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            pointY += 20;
            // 
            // hScrollBar6
            // 
            this.hScrollBar6.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar6.Maximum = 200;
            this.hScrollBar6.Name = "hScrollBar6";
            this.hScrollBar6.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar6.TabIndex = 11;
            this.hScrollBar6.Value = 100;
            this.hScrollBar6.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            pointY += 20;
            // 
            // hScrollBar9
            // 
            this.hScrollBar9.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar9.Maximum = 200;
            this.hScrollBar9.Name = "hScrollBar9";
            this.hScrollBar9.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar9.TabIndex = 14;
            this.hScrollBar9.Value = 100;
            this.hScrollBar9.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            pointY += 20;
            // 
            // hScrollBar8
            // 
            this.hScrollBar8.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar8.Maximum = 200;
            this.hScrollBar8.Name = "hScrollBar8";
            this.hScrollBar8.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar8.TabIndex = 12;
            this.hScrollBar8.Value = 110;
            this.hScrollBar8.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            pointY += 20;
            // 
            // hScrollBar7
            // 
            this.hScrollBar7.Location = new System.Drawing.Point(850, pointY);
            this.hScrollBar7.Maximum = 200;
            this.hScrollBar7.Name = "hScrollBar7";
            this.hScrollBar7.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar7.TabIndex = 13;
            this.hScrollBar7.Value = 100;
            this.hScrollBar7.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            pointY += 20;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(850, 210);//(549, 245)
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 66);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Translate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "x";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(84, 21);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown3.TabIndex = 2;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(48, 21);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown2.TabIndex = 1;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(10, 21);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDown4);
            this.groupBox2.Controls.Add(this.numericUpDown5);
            this.groupBox2.Controls.Add(this.numericUpDown6);
            this.groupBox2.Location = new System.Drawing.Point(850, 275);//(549, 327)
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(118, 66);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rotate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(88, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "x";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(10, 21);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown4.TabIndex = 2;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Location = new System.Drawing.Point(48, 21);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown5.TabIndex = 1;
            this.numericUpDown5.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Location = new System.Drawing.Point(84, 21);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown6.TabIndex = 0;
            this.numericUpDown6.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = false;// true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(850, 407);//(563, 399);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(40, 5);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "ANIMATION";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            
            // 
            // hScrollBar13
            // 
            this.hScrollBar13.Location = new System.Drawing.Point(850, 476);
            this.hScrollBar13.Maximum = 200;
            this.hScrollBar13.Name = "hScrollBar13";
            this.hScrollBar13.Size = new System.Drawing.Size(143, 17);
            this.hScrollBar13.TabIndex = 24;
            this.hScrollBar13.Value = 130;
            this.hScrollBar13.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar11
            // 
            this.hScrollBar11.Location = new System.Drawing.Point(850, 426);
            this.hScrollBar11.Maximum = 200;
            this.hScrollBar11.Name = "hScrollBar11";
            this.hScrollBar11.Size = new System.Drawing.Size(143, 17);
            this.hScrollBar11.TabIndex = 25;
            this.hScrollBar11.Value = 70;
            this.hScrollBar11.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar12
            // 
            this.hScrollBar12.Location = new System.Drawing.Point(850, 452);
            this.hScrollBar12.Maximum = 200;
            this.hScrollBar12.Name = "hScrollBar12";
            this.hScrollBar12.Size = new System.Drawing.Size(143, 17);
            this.hScrollBar12.TabIndex = 26;
            this.hScrollBar12.Value = 80;
            this.hScrollBar12.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(850, 350);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(86, 17);
            this.checkBox2.TabIndex = 27;
            this.checkBox2.Text = "MIRROR";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // timer2
            // 
            this.timer2.Enabled = false;// true;
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(850, 385);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(39, 16);
            this.radioButton1.TabIndex = 28;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Reflection ON";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(850, 370);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(60, 17);
            this.radioButton2.TabIndex = 29;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Reflection OFF";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(850, pointPlanetY);
            this.radioButton3.Name = "radioButton1";
            this.radioButton3.Size = new System.Drawing.Size(60, 17);
            this.radioButton3.TabIndex = 33;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Earth";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(850, pointPlanetY);
            this.radioButton4.Name = "radioButton2";
            this.radioButton4.Size = new System.Drawing.Size(60, 17);
            this.radioButton4.TabIndex = 34;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Moon";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(850, pointPlanetY);
            this.radioButton5.Name = "radioButton3";
            this.radioButton5.Size = new System.Drawing.Size(60, 17);
            this.radioButton5.TabIndex = 35;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Sun";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton6
            // 
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(850, pointPlanetY);
            this.radioButton6.Name = "radioButton4";
            this.radioButton6.Size = new System.Drawing.Size(60, 17);
            this.radioButton6.TabIndex = 36;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Mercury";
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton7
            // 
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(850, pointPlanetY);
            this.radioButton7.Name = "radioButton5";
            this.radioButton7.Size = new System.Drawing.Size(60, 17);
            this.radioButton7.TabIndex = 37;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "Venus";
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton8
            // 
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(850, pointPlanetY);
            this.radioButton8.Name = "radioButton6";
            this.radioButton8.Size = new System.Drawing.Size(60, 17);
            this.radioButton8.TabIndex = 38;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Mars";
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton9
            // 
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(910, 500);
            this.radioButton9.Name = "radioButton7";
            this.radioButton9.Size = new System.Drawing.Size(60, 17);
            this.radioButton9.TabIndex = 39;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "Jupiter";
            this.radioButton9.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton10
            // 
            this.radioButton10.UseVisualStyleBackColor = true;
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(910, 520);
            this.radioButton10.Name = "radioButton8";
            this.radioButton10.Size = new System.Drawing.Size(60, 17);
            this.radioButton10.TabIndex = 40;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "Uranus";
            this.radioButton10.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            // 
            // radioButton11
            // 
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(910, 540);
            this.radioButton8.Name = "radioButton9";
            this.radioButton8.Size = new System.Drawing.Size(60, 17);
            this.radioButton8.TabIndex = 41;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Saturn";
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            pointPlanetY += 20;
            //
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.radioButton2);
            this.groupBox3.Location = new System.Drawing.Point(850, 340);//(549, 245)
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(118, 66);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Reflection";
            //
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(850, 300);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Y";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(850, 300);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "N";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 850);//(685, 528)
            this.Controls.Add(this.hScrollBar12);
            this.Controls.Add(this.hScrollBar11);
            this.Controls.Add(this.hScrollBar13);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.hScrollBar9);
            this.Controls.Add(this.hScrollBar8);
            this.Controls.Add(this.hScrollBar7);
            this.Controls.Add(this.hScrollBar6);
            this.Controls.Add(this.hScrollBar5);
            this.Controls.Add(this.hScrollBar4);
            this.Controls.Add(this.hScrollBar3);
            this.Controls.Add(this.hScrollBar2);
            this.Controls.Add(this.hScrollBar1);
           // this.Controls.Add(this.checkBox2);
            //this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.radioButton6);
            this.Controls.Add(this.radioButton7);
            this.Controls.Add(this.radioButton8);
            this.Controls.Add(this.radioButton9);
            this.Controls.Add(this.radioButton10);
            this.Controls.Add(this.radioButton11);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";

            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();

            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
           

            //this.groupBox3.ResumeLayout(false);
            //this.groupBox3.PerformLayout();
            //((System.ComponentModel.ISupportInitialize)(this.radioButton1)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.radioButton2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar2;
        private System.Windows.Forms.HScrollBar hScrollBar3;
        private System.Windows.Forms.HScrollBar hScrollBar4;
        private System.Windows.Forms.HScrollBar hScrollBar5;
        private System.Windows.Forms.HScrollBar hScrollBar6;
        private System.Windows.Forms.HScrollBar hScrollBar9;
        private System.Windows.Forms.HScrollBar hScrollBar8;
        private System.Windows.Forms.HScrollBar hScrollBar7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.HScrollBar hScrollBar13;
        private System.Windows.Forms.HScrollBar hScrollBar11;
        private System.Windows.Forms.HScrollBar hScrollBar12;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.RadioButton radioButton11;
    }
}

