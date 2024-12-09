using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace RayTracer 
{
    public partial class Form1 : Form
    {
        private ColorDialog colorDialog;
        private RayTracer tracer = new RayTracer();
        (int index, string type) selectedObject = (-1, null);
        string selectedScene;
        int selectedSceneIndex;


        private void PopulateCameraComboBox()
        {
            cameraComboBox.Items.Clear();
            for (int i = 0; i < 3; i++)
            {
                cameraComboBox.Items.Add($"Камера {i + 1}");
            }
            cameraComboBox.SelectedIndex = 0; // дефолтная камера
        }

        private void cameraComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var res = tracer.selectedCameraChanged((uint) cameraComboBox.SelectedIndex, true);
            if (!(sender is null) || !(e is null))
                FieldOfViewEntry.Value = res;
        }


        public Form1()
        {
            InitializeComponent();
            
            colorDialog = new ColorDialog();

            trackBarReflectiveness.ValueChanged += TrackBarReflectiveness_ValueChanged;
            pictureBox1.MouseClick += PictureBox1_MouseClick;
            cameraComboBox.SelectedIndexChanged += cameraComboBox_SelectedIndexChanged;

            InitializeListView();
            InitializeComboBox();
            PopulateCameraComboBox();
        }

        private void InitializeListView()
        {
            objectListView.View = View.Details;
            objectListView.FullRowSelect = true;
            objectListView.Columns.Add("Тип объекта", -2, HorizontalAlignment.Left);
            objectListView.Columns.Add("Имя объекта", -2, HorizontalAlignment.Left);

            objectListView.SelectedIndexChanged += ObjectListView_SelectedIndexChanged;
        }

        private void ObjectListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objectListView.SelectedItems.Count == 0)
                return;

            ListViewItem item = objectListView.SelectedItems[0];
            string type = item.SubItems[0].Text;
            //int index = int.Parse(item.SubItems[2].Text);
            var r = item.Index; // TODO: may work incorrect
            selectedObject = (r, type);

            tracer.selectedObjectChanged(type, r);
        }

        private void SelectListViewItem(string type, int index)
        {
            objectListView.SelectedItems.Clear();
            foreach (ListViewItem item in objectListView.Items)
            {
                if (item.SubItems[0].Text == type && item.Index == index)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                }
            }
            objectListView.Select();
        }

        private void TrackBarReflectiveness_ValueChanged(object sender, EventArgs e)
        {
            if (selectedObject.index != -1)
            {
                double reflectiveness = trackBarReflectiveness.Value / 100.0;
                tracer.UpdateObjectReflectiveness((selectedObject.index, selectedObject.type), reflectiveness);
                labelReflectiveness.Text = $"Зеркальность: {trackBarReflectiveness.Value}%";
            }
        }

        private void InitializeComboBox()
        {
            SceneChooser.Items.Add("Сцена со сферами");
            SceneChooser.Items.Add("Сцена с шахматными фигурами");
            SceneChooser.Items.Add("Сцена с конем и сферой");

            SceneChooser.SelectedIndexChanged += SceneComboBox_SelectedIndexChanged;
            SceneChooser.SelectedIndex = 0;  // Default scene
        }

        private void SceneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            selectedScene = comboBox.SelectedItem.ToString();
            selectedSceneIndex = comboBox.SelectedIndex;
            tracer.selectedSceneChanged(selectedSceneIndex);
        }

        // Update reflection factor when the slider is adjusted
        private void trackBarReflection_Scroll(object sender, EventArgs e)
        {
            labelReflectiveness.Text = $"Зеркальность: {trackBarReflectiveness.Value}%";
        }

        private void trackIntensity_Scroll(object sender, EventArgs e)
        {
            tracer.intensity = trackIntensity.Value / 100.0;
            labelIntensity.Text = $"Интенсивность: {trackIntensity.Value}%";
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                // Screen-space coordinates normalized to [-1, 1]
                double aspectRatio = (double)pictureBox1.Width / pictureBox1.Height;

                double ndcX = (2 * ((e.X + 0.5) / pictureBox1.Width) - 1) * aspectRatio;
                double ndcY = 1 - 2 * ((e.Y + 0.5) / pictureBox1.Height);


                // Trace ray to find the object clicked on
                selectedObject = tracer.getClickedObject(ndcX, ndcY);
                SelectListViewItem(selectedObject.type, selectedObject.index);
            }
        }

        private void UpdateObjectColor((int index, string type) selection, Color newColor)
        {
            tracer.UpdateObjectColor(selection, newColor);
        }

        private void UpdateObjectReflectiveness((int index, string type) selection, double reflectiveness)
        {
            tracer.UpdateObjectReflectiveness(selection, reflectiveness);
        }

        private void fillListView()
        {
            objectListView.Items.Clear();

            var objects = tracer.getObjectList();
            foreach (var lst in  objects)
                objectListView.Items.Add(new ListViewItem(lst.ToArray()));
        }

        private async void btnRender_Click(object sender, EventArgs e)
        {
            int width = 800;
            int height = 600;

            tracer.selectedSceneChanged(selectedSceneIndex);
            cameraComboBox_SelectedIndexChanged(null, null);
            fillListView();

            await Task.Run(() =>
            {
                var bitmap = tracer.RenderSceneInterface(width, height, chkAntiAliasing.Checked, depthOfFieldCheckbox.Checked, (double) focalPlaneDistanceControl.Value, (int) NumRaysEntry.Value, progressBar);
                pictureBox1.Image = bitmap;
            });

        }

        private void FieldOfViewEntry_ValueChanged(object sender, EventArgs e)
        {
            tracer.fov = (double)FieldOfViewEntry.Value / 180 * Math.PI;
        }

        private void btnChangeColor_Click(object sender, EventArgs e)
        {

            if (selectedObject.type != null && colorDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateObjectColor(selectedObject, colorDialog.Color);
                btnRender.PerformClick();
            }
        }

        private void radioButtonPhong_CheckedChanged(object sender, EventArgs e)
        {
            tracer.shading = radioButtonPhong.Checked ? RayTracer.Shading.Phong : RayTracer.Shading.Gourand;
        }

        private void maxRecRefl_ValueChanged(object sender, EventArgs e)
        {
            tracer.UpdateMaxRecursionDepthReflection((uint) maxRecRefl.Value);
        }
    }
}