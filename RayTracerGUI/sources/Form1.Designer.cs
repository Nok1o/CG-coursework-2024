namespace RayTracer
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRender = new System.Windows.Forms.Button();
            this.trackBarReflectiveness = new System.Windows.Forms.TrackBar();
            this.labelReflectiveness = new System.Windows.Forms.Label();
            this.labelReflectionText = new System.Windows.Forms.Label();
            this.radioButtonPhong = new System.Windows.Forms.RadioButton();
            this.radioButtonFresnel = new System.Windows.Forms.RadioButton();
            this.SceneChooser = new System.Windows.Forms.ComboBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.trackIntensity = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.labelIntensity = new System.Windows.Forms.Label();
            this.btnChangeColor = new System.Windows.Forms.Button();
            this.objectListView = new System.Windows.Forms.ListView();
            this.depthOfFieldCheckbox = new System.Windows.Forms.CheckBox();
            this.focalPlaneLabel = new System.Windows.Forms.Label();
            this.focalPlaneDistanceControl = new System.Windows.Forms.NumericUpDown();
            this.chkAntiAliasing = new System.Windows.Forms.CheckBox();
            this.cameraComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.NumRaysEntry = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FieldOfViewEntry = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.maxRecRefl = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarReflectiveness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.focalPlaneDistanceControl)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumRaysEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldOfViewEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRecRefl)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnRender
            // 
            this.btnRender.Location = new System.Drawing.Point(838, 552);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(283, 31);
            this.btnRender.TabIndex = 1;
            this.btnRender.Text = "Визуализировать";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // trackBarReflectiveness
            // 
            this.trackBarReflectiveness.Location = new System.Drawing.Point(33, 38);
            this.trackBarReflectiveness.Maximum = 100;
            this.trackBarReflectiveness.Name = "trackBarReflectiveness";
            this.trackBarReflectiveness.Size = new System.Drawing.Size(217, 45);
            this.trackBarReflectiveness.TabIndex = 2;
            this.trackBarReflectiveness.Value = 50;
            this.trackBarReflectiveness.Scroll += new System.EventHandler(this.trackBarReflection_Scroll);
            // 
            // labelReflectiveness
            // 
            this.labelReflectiveness.AutoSize = true;
            this.labelReflectiveness.Location = new System.Drawing.Point(40, 70);
            this.labelReflectiveness.Name = "labelReflectiveness";
            this.labelReflectiveness.Size = new System.Drawing.Size(105, 13);
            this.labelReflectiveness.TabIndex = 3;
            this.labelReflectiveness.Text = "Зеркальность: 50%";
            // 
            // labelReflectionText
            // 
            this.labelReflectionText.AutoSize = true;
            this.labelReflectionText.Location = new System.Drawing.Point(105, 22);
            this.labelReflectionText.Name = "labelReflectionText";
            this.labelReflectionText.Size = new System.Drawing.Size(79, 13);
            this.labelReflectionText.TabIndex = 4;
            this.labelReflectionText.Text = "Зеркальность";
            // 
            // radioButtonPhong
            // 
            this.radioButtonPhong.AutoSize = true;
            this.radioButtonPhong.Checked = true;
            this.radioButtonPhong.Location = new System.Drawing.Point(164, 120);
            this.radioButtonPhong.Name = "radioButtonPhong";
            this.radioButtonPhong.Size = new System.Drawing.Size(75, 17);
            this.radioButtonPhong.TabIndex = 5;
            this.radioButtonPhong.TabStop = true;
            this.radioButtonPhong.Text = "По Фонгу";
            this.radioButtonPhong.UseVisualStyleBackColor = true;
            this.radioButtonPhong.CheckedChanged += new System.EventHandler(this.radioButtonPhong_CheckedChanged);
            // 
            // radioButtonFresnel
            // 
            this.radioButtonFresnel.AutoSize = true;
            this.radioButtonFresnel.Location = new System.Drawing.Point(164, 143);
            this.radioButtonFresnel.Name = "radioButtonFresnel";
            this.radioButtonFresnel.Size = new System.Drawing.Size(65, 17);
            this.radioButtonFresnel.TabIndex = 6;
            this.radioButtonFresnel.TabStop = true;
            this.radioButtonFresnel.Text = "По Гуро";
            this.radioButtonFresnel.UseVisualStyleBackColor = true;
            // 
            // SceneChooser
            // 
            this.SceneChooser.Location = new System.Drawing.Point(837, 32);
            this.SceneChooser.Name = "SceneChooser";
            this.SceneChooser.Size = new System.Drawing.Size(160, 21);
            this.SceneChooser.TabIndex = 7;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(838, 589);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(282, 23);
            this.progressBar.TabIndex = 8;
            // 
            // trackIntensity
            // 
            this.trackIntensity.Location = new System.Drawing.Point(9, 42);
            this.trackIntensity.Maximum = 100;
            this.trackIntensity.Name = "trackIntensity";
            this.trackIntensity.Size = new System.Drawing.Size(150, 45);
            this.trackIntensity.TabIndex = 10;
            this.trackIntensity.Value = 20;
            this.trackIntensity.Scroll += new System.EventHandler(this.trackIntensity_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Интенсивность источника света";
            // 
            // labelIntensity
            // 
            this.labelIntensity.AutoSize = true;
            this.labelIntensity.Location = new System.Drawing.Point(6, 74);
            this.labelIntensity.Name = "labelIntensity";
            this.labelIntensity.Size = new System.Drawing.Size(111, 13);
            this.labelIntensity.TabIndex = 12;
            this.labelIntensity.Text = "Интенсивность: 20%";
            // 
            // btnChangeColor
            // 
            this.btnChangeColor.Location = new System.Drawing.Point(65, 89);
            this.btnChangeColor.Name = "btnChangeColor";
            this.btnChangeColor.Size = new System.Drawing.Size(145, 23);
            this.btnChangeColor.TabIndex = 13;
            this.btnChangeColor.Text = "Изменить цвет";
            this.btnChangeColor.UseVisualStyleBackColor = true;
            this.btnChangeColor.Click += new System.EventHandler(this.btnChangeColor_Click);
            // 
            // objectListView
            // 
            this.objectListView.HideSelection = false;
            this.objectListView.Location = new System.Drawing.Point(837, 77);
            this.objectListView.Name = "objectListView";
            this.objectListView.Size = new System.Drawing.Size(286, 150);
            this.objectListView.TabIndex = 14;
            this.objectListView.UseCompatibleStateImageBehavior = false;
            // 
            // depthOfFieldCheckbox
            // 
            this.depthOfFieldCheckbox.AutoSize = true;
            this.depthOfFieldCheckbox.Location = new System.Drawing.Point(188, 42);
            this.depthOfFieldCheckbox.Name = "depthOfFieldCheckbox";
            this.depthOfFieldCheckbox.Size = new System.Drawing.Size(94, 17);
            this.depthOfFieldCheckbox.TabIndex = 15;
            this.depthOfFieldCheckbox.Text = "Глубина поля";
            this.depthOfFieldCheckbox.UseVisualStyleBackColor = true;
            // 
            // focalPlaneLabel
            // 
            this.focalPlaneLabel.AutoSize = true;
            this.focalPlaneLabel.Location = new System.Drawing.Point(7, 104);
            this.focalPlaneLabel.Name = "focalPlaneLabel";
            this.focalPlaneLabel.Size = new System.Drawing.Size(122, 26);
            this.focalPlaneLabel.TabIndex = 16;
            this.focalPlaneLabel.Text = "Расстояние до \r\nфокальной плоскости ";
            // 
            // focalPlaneDistanceControl
            // 
            this.focalPlaneDistanceControl.DecimalPlaces = 1;
            this.focalPlaneDistanceControl.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.focalPlaneDistanceControl.Location = new System.Drawing.Point(9, 133);
            this.focalPlaneDistanceControl.Name = "focalPlaneDistanceControl";
            this.focalPlaneDistanceControl.Size = new System.Drawing.Size(120, 20);
            this.focalPlaneDistanceControl.TabIndex = 17;
            this.focalPlaneDistanceControl.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // chkAntiAliasing
            // 
            this.chkAntiAliasing.AutoSize = true;
            this.chkAntiAliasing.Location = new System.Drawing.Point(188, 26);
            this.chkAntiAliasing.Name = "chkAntiAliasing";
            this.chkAntiAliasing.Size = new System.Drawing.Size(94, 17);
            this.chkAntiAliasing.TabIndex = 18;
            this.chkAntiAliasing.Text = "Сглаживание";
            this.chkAntiAliasing.UseVisualStyleBackColor = true;
            // 
            // cameraComboBox
            // 
            this.cameraComboBox.FormattingEnabled = true;
            this.cameraComboBox.Location = new System.Drawing.Point(1002, 32);
            this.cameraComboBox.Name = "cameraComboBox";
            this.cameraComboBox.Size = new System.Drawing.Size(121, 21);
            this.cameraComboBox.TabIndex = 19;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelReflectiveness);
            this.groupBox1.Controls.Add(this.labelReflectionText);
            this.groupBox1.Controls.Add(this.trackBarReflectiveness);
            this.groupBox1.Controls.Add(this.btnChangeColor);
            this.groupBox1.Location = new System.Drawing.Point(838, 428);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 118);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Свойства объектов сцены";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.maxRecRefl);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.NumRaysEntry);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.labelIntensity);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.trackIntensity);
            this.groupBox2.Controls.Add(this.chkAntiAliasing);
            this.groupBox2.Controls.Add(this.depthOfFieldCheckbox);
            this.groupBox2.Controls.Add(this.focalPlaneDistanceControl);
            this.groupBox2.Controls.Add(this.focalPlaneLabel);
            this.groupBox2.Controls.Add(this.radioButtonPhong);
            this.groupBox2.Controls.Add(this.radioButtonFresnel);
            this.groupBox2.Location = new System.Drawing.Point(838, 233);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(285, 189);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры сцены";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(189, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Число лучей";
            // 
            // NumRaysEntry
            // 
            this.NumRaysEntry.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NumRaysEntry.Location = new System.Drawing.Point(191, 78);
            this.NumRaysEntry.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.NumRaysEntry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumRaysEntry.Name = "NumRaysEntry";
            this.NumRaysEntry.Size = new System.Drawing.Size(88, 20);
            this.NumRaysEntry.TabIndex = 20;
            this.NumRaysEntry.Tag = "";
            this.NumRaysEntry.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(161, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Алгоритм закраски";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(835, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Выбор сцены";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(999, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Выбор камеры";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(835, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Список объектов сцены";
            // 
            // FieldOfViewEntry
            // 
            this.FieldOfViewEntry.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.FieldOfViewEntry.Location = new System.Drawing.Point(1083, 54);
            this.FieldOfViewEntry.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.FieldOfViewEntry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FieldOfViewEntry.Name = "FieldOfViewEntry";
            this.FieldOfViewEntry.Size = new System.Drawing.Size(40, 20);
            this.FieldOfViewEntry.TabIndex = 22;
            this.FieldOfViewEntry.Tag = "";
            this.FieldOfViewEntry.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.FieldOfViewEntry.ValueChanged += new System.EventHandler(this.FieldOfViewEntry_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1005, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Поле зрения";
            // 
            // maxRecRefl
            // 
            this.maxRecRefl.Location = new System.Drawing.Point(184, 166);
            this.maxRecRefl.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.maxRecRefl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxRecRefl.Name = "maxRecRefl";
            this.maxRecRefl.Size = new System.Drawing.Size(51, 20);
            this.maxRecRefl.TabIndex = 22;
            this.maxRecRefl.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.maxRecRefl.ValueChanged += new System.EventHandler(this.maxRecRefl_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(156, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Глубина рекурсии отражения";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 625);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FieldOfViewEntry);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cameraComboBox);
            this.Controls.Add(this.objectListView);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.SceneChooser);
            this.Controls.Add(this.btnRender);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Курсовая работа. Трассировка лучей";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarReflectiveness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.focalPlaneDistanceControl)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumRaysEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldOfViewEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRecRefl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRender;
        private System.Windows.Forms.TrackBar trackBarReflectiveness;
        private System.Windows.Forms.Label labelReflectiveness;
        private System.Windows.Forms.Label labelReflectionText;
        private System.Windows.Forms.RadioButton radioButtonPhong;
        private System.Windows.Forms.RadioButton radioButtonFresnel;
        private System.Windows.Forms.ComboBox SceneChooser;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TrackBar trackIntensity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelIntensity;
        private System.Windows.Forms.Button btnChangeColor;
        private System.Windows.Forms.ListView objectListView;
        private System.Windows.Forms.CheckBox depthOfFieldCheckbox;
        private System.Windows.Forms.Label focalPlaneLabel;
        private System.Windows.Forms.NumericUpDown focalPlaneDistanceControl;
        private System.Windows.Forms.CheckBox chkAntiAliasing;
        private System.Windows.Forms.ComboBox cameraComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NumRaysEntry;
        private System.Windows.Forms.NumericUpDown FieldOfViewEntry;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown maxRecRefl;
    }
}
