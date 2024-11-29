namespace RayTracerGUI
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarReflectiveness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.focalPlaneDistanceControl)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnRender
            // 
            this.btnRender.Location = new System.Drawing.Point(840, 12);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(100, 30);
            this.btnRender.TabIndex = 1;
            this.btnRender.Text = "Render";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // trackBarReflectiveness
            // 
            this.trackBarReflectiveness.Location = new System.Drawing.Point(840, 80);
            this.trackBarReflectiveness.Maximum = 100;
            this.trackBarReflectiveness.Name = "trackBarReflectiveness";
            this.trackBarReflectiveness.Size = new System.Drawing.Size(150, 45);
            this.trackBarReflectiveness.TabIndex = 2;
            this.trackBarReflectiveness.Value = 50;
            this.trackBarReflectiveness.Scroll += new System.EventHandler(this.trackBarReflection_Scroll);
            // 
            // labelReflectiveness
            // 
            this.labelReflectiveness.AutoSize = true;
            this.labelReflectiveness.Location = new System.Drawing.Point(840, 120);
            this.labelReflectiveness.Name = "labelReflectiveness";
            this.labelReflectiveness.Size = new System.Drawing.Size(81, 13);
            this.labelReflectiveness.TabIndex = 3;
            this.labelReflectiveness.Text = "Reflection: 50%";
            // 
            // labelReflectionText
            // 
            this.labelReflectionText.AutoSize = true;
            this.labelReflectionText.Location = new System.Drawing.Point(840, 60);
            this.labelReflectionText.Name = "labelReflectionText";
            this.labelReflectionText.Size = new System.Drawing.Size(80, 13);
            this.labelReflectionText.TabIndex = 4;
            this.labelReflectionText.Text = "Reflectiveness:";
            // 
            // radioButtonPhong
            // 
            this.radioButtonPhong.AutoSize = true;
            this.radioButtonPhong.Checked = true;
            this.radioButtonPhong.Location = new System.Drawing.Point(838, 317);
            this.radioButtonPhong.Name = "radioButtonPhong";
            this.radioButtonPhong.Size = new System.Drawing.Size(56, 17);
            this.radioButtonPhong.TabIndex = 5;
            this.radioButtonPhong.TabStop = true;
            this.radioButtonPhong.Text = "Phong";
            this.radioButtonPhong.UseVisualStyleBackColor = true;
            // 
            // radioButtonFresnel
            // 
            this.radioButtonFresnel.AutoSize = true;
            this.radioButtonFresnel.Location = new System.Drawing.Point(838, 340);
            this.radioButtonFresnel.Name = "radioButtonFresnel";
            this.radioButtonFresnel.Size = new System.Drawing.Size(59, 17);
            this.radioButtonFresnel.TabIndex = 6;
            this.radioButtonFresnel.TabStop = true;
            this.radioButtonFresnel.Text = "Fresnel";
            this.radioButtonFresnel.UseVisualStyleBackColor = true;
            // 
            // SceneChooser
            // 
            this.SceneChooser.Location = new System.Drawing.Point(828, 373);
            this.SceneChooser.Name = "SceneChooser";
            this.SceneChooser.Size = new System.Drawing.Size(160, 21);
            this.SceneChooser.TabIndex = 7;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(820, 589);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(170, 23);
            this.progressBar.TabIndex = 8;
            // 
            // trackIntensity
            // 
            this.trackIntensity.Location = new System.Drawing.Point(838, 185);
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
            this.label1.Location = new System.Drawing.Point(838, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Intensity:";
            // 
            // labelIntensity
            // 
            this.labelIntensity.AutoSize = true;
            this.labelIntensity.Location = new System.Drawing.Point(838, 217);
            this.labelIntensity.Name = "labelIntensity";
            this.labelIntensity.Size = new System.Drawing.Size(72, 13);
            this.labelIntensity.TabIndex = 12;
            this.labelIntensity.Text = "Intensity: 20%";
            // 
            // btnChangeColor
            // 
            this.btnChangeColor.Location = new System.Drawing.Point(843, 137);
            this.btnChangeColor.Name = "btnChangeColor";
            this.btnChangeColor.Size = new System.Drawing.Size(145, 23);
            this.btnChangeColor.TabIndex = 13;
            this.btnChangeColor.Text = "Change Color";
            this.btnChangeColor.UseVisualStyleBackColor = true;
            // 
            // objectListView
            // 
            this.objectListView.HideSelection = false;
            this.objectListView.Location = new System.Drawing.Point(828, 417);
            this.objectListView.Name = "objectListView";
            this.objectListView.Size = new System.Drawing.Size(160, 150);
            this.objectListView.TabIndex = 14;
            this.objectListView.UseCompatibleStateImageBehavior = false;
            // 
            // depthOfFieldCheckbox
            // 
            this.depthOfFieldCheckbox.AutoSize = true;
            this.depthOfFieldCheckbox.Location = new System.Drawing.Point(838, 249);
            this.depthOfFieldCheckbox.Name = "depthOfFieldCheckbox";
            this.depthOfFieldCheckbox.Size = new System.Drawing.Size(123, 17);
            this.depthOfFieldCheckbox.TabIndex = 15;
            this.depthOfFieldCheckbox.Text = "Enable depth of field";
            this.depthOfFieldCheckbox.UseVisualStyleBackColor = true;
            // 
            // focalPlaneLabel
            // 
            this.focalPlaneLabel.AutoSize = true;
            this.focalPlaneLabel.Location = new System.Drawing.Point(837, 269);
            this.focalPlaneLabel.Name = "focalPlaneLabel";
            this.focalPlaneLabel.Size = new System.Drawing.Size(111, 13);
            this.focalPlaneLabel.TabIndex = 16;
            this.focalPlaneLabel.Text = "Focal plane distance: ";
            // 
            // focalPlaneDistanceControl
            // 
            this.focalPlaneDistanceControl.DecimalPlaces = 1;
            this.focalPlaneDistanceControl.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.focalPlaneDistanceControl.Location = new System.Drawing.Point(840, 285);
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
            this.chkAntiAliasing.Location = new System.Drawing.Point(1008, 137);
            this.chkAntiAliasing.Name = "chkAntiAliasing";
            this.chkAntiAliasing.Size = new System.Drawing.Size(82, 17);
            this.chkAntiAliasing.TabIndex = 18;
            this.chkAntiAliasing.Text = "Anti-aliasing";
            this.chkAntiAliasing.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 625);
            this.Controls.Add(this.chkAntiAliasing);
            this.Controls.Add(this.focalPlaneDistanceControl);
            this.Controls.Add(this.focalPlaneLabel);
            this.Controls.Add(this.depthOfFieldCheckbox);
            this.Controls.Add(this.objectListView);
            this.Controls.Add(this.btnChangeColor);
            this.Controls.Add(this.labelIntensity);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackIntensity);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.SceneChooser);
            this.Controls.Add(this.radioButtonFresnel);
            this.Controls.Add(this.radioButtonPhong);
            this.Controls.Add(this.labelReflectionText);
            this.Controls.Add(this.labelReflectiveness);
            this.Controls.Add(this.trackBarReflectiveness);
            this.Controls.Add(this.btnRender);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Ray Tracing Demo";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarReflectiveness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.focalPlaneDistanceControl)).EndInit();
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
    }
}
