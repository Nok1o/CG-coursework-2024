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
            this.trackBarReflection = new System.Windows.Forms.TrackBar();
            this.labelReflection = new System.Windows.Forms.Label();
            this.labelReflectionText = new System.Windows.Forms.Label();
            this.radioButtonPhong = new System.Windows.Forms.RadioButton();
            this.radioButtonFresnel = new System.Windows.Forms.RadioButton();
            this.SceneChooser = new System.Windows.Forms.ComboBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.trackIntensity = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.labelIntensity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarReflection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackIntensity)).BeginInit();
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
            // trackBarReflection
            // 
            this.trackBarReflection.Location = new System.Drawing.Point(840, 80);
            this.trackBarReflection.Maximum = 100;
            this.trackBarReflection.Name = "trackBarReflection";
            this.trackBarReflection.Size = new System.Drawing.Size(150, 45);
            this.trackBarReflection.TabIndex = 2;
            this.trackBarReflection.Value = 50;
            this.trackBarReflection.Scroll += new System.EventHandler(this.trackBarReflection_Scroll);
            // 
            // labelReflection
            // 
            this.labelReflection.AutoSize = true;
            this.labelReflection.Location = new System.Drawing.Point(840, 120);
            this.labelReflection.Name = "labelReflection";
            this.labelReflection.Size = new System.Drawing.Size(81, 13);
            this.labelReflection.TabIndex = 3;
            this.labelReflection.Text = "Reflection: 50%";
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
            this.radioButtonPhong.Location = new System.Drawing.Point(846, 285);
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
            this.radioButtonFresnel.Location = new System.Drawing.Point(843, 308);
            this.radioButtonFresnel.Name = "radioButtonFresnel";
            this.radioButtonFresnel.Size = new System.Drawing.Size(59, 17);
            this.radioButtonFresnel.TabIndex = 6;
            this.radioButtonFresnel.TabStop = true;
            this.radioButtonFresnel.Text = "Fresnel";
            this.radioButtonFresnel.UseVisualStyleBackColor = true;
            // 
            // SceneChooser
            // 
            this.SceneChooser.Location = new System.Drawing.Point(828, 366);
            this.SceneChooser.Name = "SceneChooser";
            this.SceneChooser.Size = new System.Drawing.Size(134, 21);
            this.SceneChooser.TabIndex = 7;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(820, 589);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(170, 23);
            this.progressBar.TabIndex = 8;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(80, 17);
            this.hScrollBar1.TabIndex = 9;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 625);
            this.Controls.Add(this.labelIntensity);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackIntensity);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.SceneChooser);
            this.Controls.Add(this.radioButtonFresnel);
            this.Controls.Add(this.radioButtonPhong);
            this.Controls.Add(this.labelReflectionText);
            this.Controls.Add(this.labelReflection);
            this.Controls.Add(this.trackBarReflection);
            this.Controls.Add(this.btnRender);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Ray Tracing Demo";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarReflection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackIntensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRender;
        private System.Windows.Forms.TrackBar trackBarReflection;
        private System.Windows.Forms.Label labelReflection;
        private System.Windows.Forms.Label labelReflectionText;
        private System.Windows.Forms.RadioButton radioButtonPhong;
        private System.Windows.Forms.RadioButton radioButtonFresnel;
        private System.Windows.Forms.ComboBox SceneChooser;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.TrackBar trackIntensity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelIntensity;
    }
}
