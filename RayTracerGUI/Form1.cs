using RayTracerGUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Assimp;
using System.Threading.Tasks;
using System.Linq.Expressions;



namespace RayTracerGUI 
{
    public partial class Form1 : Form
    {
        private double reflectionFactor = 0.5; // Default reflection value
        private double intensity = 0.2;
        private string selectedScene = "Default";
        private ColorDialog colorDialog;


        ObjectScene sphereScene = new ObjectScene();
        ObjectScene chessScene = new ObjectScene();
        ObjectScene knightScene = new ObjectScene();

        ObjectScene currentScene = null;

        (object Object, int index, string type) selectedObject = (null, -1, null);

        Vector3 cameraPos, lightPos;
        Vector3 cameraDirection = new Vector3(0, 0, -1);


        public Form1()
        {
            InitializeComponent();
            InitializeComboBox();
            colorDialog = new ColorDialog();

            // Initialize the reflectiveness slider
            trackBarReflectiveness.Minimum = 0;
            trackBarReflectiveness.Maximum = 100;
            trackBarReflectiveness.ValueChanged += TrackBarReflectiveness_ValueChanged;

            // Color button event
            btnChangeColor.Click += BtnChangeColor_Click;

            pictureBox1.MouseClick += PictureBox1_MouseClick;

            InitializeListView();
        }

        private void InitializeListView()
        {
            objectListView.View = View.Details;
            objectListView.FullRowSelect = true;
            objectListView.Columns.Add("Object Type", -2, HorizontalAlignment.Left);
            objectListView.Columns.Add("Index", -2, HorizontalAlignment.Left);

            //PopulateListView();

            objectListView.SelectedIndexChanged += ObjectListView_SelectedIndexChanged;
        }

        private void PopulateListView()
        {
            objectListView.Items.Clear();

            for (int i = 0; i < currentScene.spheres.Length; i++)
                objectListView.Items.Add(new ListViewItem(new[] { "Sphere", i.ToString() }));

            for (int i = 0; i < currentScene.walls.Length; i++)
                objectListView.Items.Add(new ListViewItem(new[] { "Wall", i.ToString() }));

            for (int i = 0; i < currentScene.chessPieces.Length; i++)
                objectListView.Items.Add(new ListViewItem(new[] { "ChessPiece", i.ToString() }));
        }

        private void ObjectListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objectListView.SelectedItems.Count == 0)
                return;

            ListViewItem item = objectListView.SelectedItems[0];
            string type = item.SubItems[0].Text;
            int index = int.Parse(item.SubItems[1].Text);

            selectedObject = (GetObjectByTypeAndIndex(type, index), index, type);
            //HighlightSelectedObject();
        }

        private object GetObjectByTypeAndIndex(string type, int index)
        {
            switch (type)
            {
                case "Sphere":
                    return currentScene.spheres[index];
                case "ChessPiece":
                    return currentScene.chessPieces[index];
                case "Wall":
                    return currentScene.walls[index];
                default:
                    return null;
            };
        }

        private void SelectListViewItem(string type, int index)
        {
            objectListView.SelectedItems.Clear();
            foreach (ListViewItem item in objectListView.Items)
            {
                if (item.SubItems[0].Text == type && item.SubItems[1].Text == index.ToString())
                {
                    item.Selected = true;
                    item.EnsureVisible();
                }
            }
            objectListView.Select();
        }


        private void BtnChangeColor_Click(object sender, EventArgs e)
        {
            if (selectedObject.Object != null && colorDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateObjectColor(selectedObject, colorDialog.Color);
                btnRender.PerformClick();
            }
        }

        private void TrackBarReflectiveness_ValueChanged(object sender, EventArgs e)
        {
            if (selectedObject.Object != null)
            {
                double reflectiveness = trackBarReflectiveness.Value / 100.0;
                UpdateObjectReflectiveness(selectedObject, reflectiveness);
                labelReflectiveness.Text = $"Reflectiveness: {trackBarReflectiveness.Value}%";
                //btnRender.PerformClick();
            }
        }

        private void InitializeComboBox()
        {
            SceneChooser.Items.Add("Sphere Scene");
            SceneChooser.Items.Add("Chess Scene");
            SceneChooser.Items.Add("Knight Scene");

            SceneChooser.SelectedIndexChanged += SceneComboBox_SelectedIndexChanged;
            SceneChooser.SelectedIndex = 0;  // Default scene
        }

        private void SceneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            selectedScene = comboBox.SelectedItem.ToString();
        }

        // Update reflection factor when the slider is adjusted
        private void trackBarReflection_Scroll(object sender, EventArgs e)
        {
            reflectionFactor = trackBarReflectiveness.Value / 100.0; // Convert from 0-100 to 0-1
            labelReflectiveness.Text = $"Reflection: {trackBarReflectiveness.Value}%";
        }

        private void trackIntensity_Scroll(object sender, EventArgs e)
        {
            intensity = trackIntensity.Value / 100.0;
            labelIntensity.Text = $"Intensity: {trackIntensity.Value}%";
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Vector3 cameraPos = new Vector3(0, 0, 0);
                Vector3 cameraDirection = new Vector3(0, 0, -1);

                // Convert screen coordinates to ray direction
                double i = (2 * (e.X + 0.5) / pictureBox1.Width - 1) * pictureBox1.Width / pictureBox1.Height;
                double j = -(2 * (e.Y + 0.5) / pictureBox1.Height - 1);
                Vector3 rayDirection = (new Vector3(i, j, -1) + cameraDirection).Normalize();

                // Trace ray to find the object clicked on
                selectedObject = FindObjectUnderMouse(new Ray(cameraPos, rayDirection));
                SelectListViewItem(selectedObject.type, selectedObject.index);

                //if (selection.selectedObject != null && colorDialog.ShowDialog() == DialogResult.OK)
                //{
                //    UpdateObjectColor(selection, colorDialog.Color);
                //    btnRender.PerformClick(); // Re-render the scene to apply color change
                //}
            }
        }


        private (object selectedObject, int index, string type) FindObjectUnderMouse(Ray ray)
        {
            double closestDistance = double.MaxValue;
            object clickedObject = null;
            int clickedIndex = -1;
            string clickedType = "";

            for (int i = 0; i < currentScene.spheres.Length; i++)
            {
                if (currentScene.spheres[i].Intersect(ray, out double distance) && distance < closestDistance)
                {
                    closestDistance = distance;
                    clickedObject = currentScene.spheres[i];
                    clickedIndex = i;
                    clickedType = "Sphere";
                }
            }

            for (int i = 0; i < currentScene.walls.Length; i++)
            {
                if (currentScene.walls[i].Intersect(ray, out double distance) && distance < closestDistance)
                {
                    closestDistance = distance;
                    clickedObject = currentScene.walls[i];
                    clickedIndex = i;
                    clickedType = "Wall";
                }
            }

            for (int i = 0; i < currentScene.chessPieces.Length; i++)
            {
                if (currentScene.chessPieces[i].IntersectRay(ray, out double distance, out _) && distance < closestDistance)
                {
                    closestDistance = distance;
                    clickedObject = currentScene.chessPieces[i];
                    clickedIndex = i;
                    clickedType = "ChessPiece";
                }
            }

            selectedObject = (clickedObject, clickedIndex, clickedType);
            //HighlightSelectedObject();
            return selectedObject;
        }

        //private void HighlightSelectedObject()
        //{
        //    if (selectedObject.Object is Sphere sphere)
        //    {
        //        originalColor = sphere.SurfaceColor;
        //        sphere.SurfaceColor = Color.Yellow; // Highlight color
        //    }
        //    else if (selectedObject.Object is Wall wall)
        //    {
        //        originalColor = wall.SurfaceColor;
        //        wall.SurfaceColor = Color.Yellow;
        //    }
        //    else if (selectedObject.Object is ChessPiece chessPiece)
        //    {
        //        originalColor = chessPiece.Color;
        //        chessPiece.Color = Color.Yellow;
        //    }
        //    //btnRender.PerformClick(); // Update rendering to show the highlight
        //}

        // Update object color
        private void UpdateObjectColor((object obj, int index, string type) selection, Color newColor)
        {
            switch (selection.type)
            {
                case "Sphere":
                    currentScene.spheres[selection.index].SurfaceColor = newColor;
                    break;
                case "Wall":
                    currentScene.walls[selection.index].SurfaceColor = newColor;
                    break;
                case "ChessPiece":
                    currentScene.chessPieces[selection.index].Color = newColor;
                    break;
            }
        }

        private void UpdateObjectReflectiveness((object obj, int index, string type) selection, double reflectiveness)
        {
            switch (selection.type)
            {
                case "Sphere":
                    currentScene.spheres[selection.index].Reflection = reflectiveness;
                    break;
                case "Wall":
                    currentScene.walls[selection.index].Reflection = reflectiveness;
                    break;
                case "ChessPiece":
                    currentScene.chessPieces[selection.index].Reflection = reflectiveness;
                    break;
            }
        }

        private void fillListView()
        {
            objectListView.Items.Clear();
            for (int i = 0; i < currentScene.spheres.Length; i++)
            {
                objectListView.Items.Add(new ListViewItem(new string[] { "Sphere", i.ToString() }));
            }
            for (int i = 0; i < currentScene.walls.Length; i++)
            {
                objectListView.Items.Add(new ListViewItem(new string[] { "Wall", i.ToString() }));
            }
            for (int i = 0; i < currentScene.chessPieces.Length; i++)
            {
                objectListView.Items.Add(new ListViewItem(new string[] { "ChessPiece", i.ToString() }));
            }
        }

        private async void btnRender_Click(object sender, EventArgs e)
        {
            int width = 800;
            int height = 600;
            Bitmap bitmap = new Bitmap(width, height);
            Color backgroundColor = Color.Gray;


            if (selectedScene == "Sphere Scene")
            {
                if (sphereScene.spheres.Length == 0)
                {
                    sphereScene = setupSphereScene();
                }
                setupSphereCamera();
                currentScene = sphereScene;
            }
            else if (selectedScene == "Chess Scene")
            {
                if (chessScene.chessPieces.Length == 0)
                {
                    chessScene = setupChessScene();
                }
                setupChessCamera();
                currentScene = chessScene;
            }
            else if (selectedScene == "Knight Scene")
            {
                if (knightScene.chessPieces.Length == 0)
                {
                    knightScene = setupKnightScene();
                }
                setupKnightCamera();
                currentScene = knightScene;
            }

            fillListView();

            // Run the render on a background thread
            await Task.Run(() =>
            {
                RenderScene(bitmap, width, height, cameraPos, cameraDirection, lightPos, backgroundColor, currentScene);
            });

            pictureBox1.Image = bitmap;
        }

        [ThreadStatic]
        private static Random random;

        private double GetRandomOffset()
        {
            if (random == null)
                random = new Random();
            return random.NextDouble() - 0.5; // Offset in the range [-0.5, 0.5]
        }

        private void RenderScene(Bitmap bitmap, int width, int height, Vector3 cameraPos, Vector3 cameraDirection, Vector3 lightPos, Color backgroundColor, ObjectScene scene)
        {
            Vector3 cameraRight = cameraDirection.Cross(new Vector3(0, 1, 0)).Normalize(); // X-axis in camera space
            Vector3 cameraUp = cameraRight.Cross(cameraDirection).Normalize();            // Y-axis in camera space

            double aspectRatio = (double)width / height;
            double fov = Math.PI / 3.0; // 60 degrees field of view
            double scale = Math.Tan(fov / 2);

            int samplesPerPixel = (chkAntiAliasing.Checked || depthOfFieldCheckbox.Checked) ? 16 : 1; // Enable anti-aliasing if checked
            double apertureSize = depthOfFieldCheckbox.Checked ? 0.01 : 0; // Enable DOF if checked
            double focalPlaneDistance = (double)focalPlaneDistanceControl.Value;

            int totalPixels = width * height;
            int processedPixels = 0;

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    double rSum = 0, gSum = 0, bSum = 0;

                    for (int s = 0; s < samplesPerPixel; s++)
                    {
                        // Random jitter for anti-aliasing
                        double pixelWidth = 2.0 / width;
                        double pixelHeight = 2.0 / height;


                        double jitterX = 0;
                        double jitterY = 0;

                        if (chkAntiAliasing.Checked)
                        {
                            jitterX = GetRandomOffset() * pixelWidth;
                            jitterY = GetRandomOffset() * pixelHeight;
                        }

                        // Pixel position with jitter
                        double ndcX = (2 * ((x + 0.5) / width) - 1) * aspectRatio + jitterX;
                        double ndcY = (1 - 2 * ((y + 0.5) / height)) + jitterY;

                        // Base ray direction
                        Vector3 rayDir = (cameraRight * (ndcX * scale) + cameraUp * (ndcY * scale) + cameraDirection).Normalize();

                        // Apply depth of field (DOF) if enabled
                        Vector3 rayOrigin = cameraPos;
                        if (apertureSize > 0)
                        {
                            Vector3 focalPoint = rayOrigin + rayDir * focalPlaneDistance;

                            var jitter = RandomInUnitCircle() * apertureSize;
                            Vector3 apertureOffset = jitter.X * cameraRight + jitter.Y * cameraUp;
                            rayOrigin += apertureOffset;

                            rayDir = (focalPoint - rayOrigin).Normalize();
                        }

                        Ray primaryRay = new Ray(rayOrigin, rayDir);
                        Color sampleColor = TraceRay(primaryRay, scene, lightPos, backgroundColor, 5);

                        rSum += sampleColor.R;
                        gSum += sampleColor.G;
                        bSum += sampleColor.B;
                    }

                    int r = (int)(rSum / samplesPerPixel);
                    int g = (int)(gSum / samplesPerPixel);
                    int b = (int)(bSum / samplesPerPixel);

                    Color finalColor = Color.FromArgb(
                        Clamp(r, 0, 255),
                        Clamp(g, 0, 255),
                        Clamp(b, 0, 255)
                    );

                    lock (bitmap)
                    {
                        bitmap.SetPixel(x, y, finalColor);

                        processedPixels++;
                        if (processedPixels % (totalPixels / 100) == 0)
                        {
                            int progress = (processedPixels * 100) / totalPixels;
                            progressBar.Invoke(new Action(() => progressBar.Value = progress));
                        }
                    }
                }
            });

            progressBar.Invoke(new Action(() => progressBar.Value = 100));
        }







        private ChessPiece[] LoadChessPieces(int mode = 1)
        {
            var importer = new AssimpContext();
            Scene scene;
            if (mode == 1)
            {
                scene = importer.ImportFile("../../chess.obj", PostProcessSteps.Triangulate);
            }
            else
            {
                scene = importer.ImportFile("../../knight.obj", PostProcessSteps.Triangulate);
            }
            var chessPieces = new List<ChessPiece>();

            int totalMeshes = scene.Meshes.Count;
            int progressIncrement = totalMeshes > 0 ? 100 / totalMeshes : 100;
            //progressBar.Value = 0; // Reset progress bar

            foreach (var mesh in scene.Meshes)
            {
                var triangles = new List<Triangle>();

                foreach (var face in mesh.Faces)
                {
                    if (face.Indices.Count == 3)
                    {
                        var v0 = mesh.Vertices[face.Indices[0]];
                        var v1 = mesh.Vertices[face.Indices[1]];
                        var v2 = mesh.Vertices[face.Indices[2]];
                        var n0 = mesh.Normals[face.Indices[0]];
                        var n1 = mesh.Normals[face.Indices[1]];
                        var n2 = mesh.Normals[face.Indices[2]];

                        triangles.Add(new Triangle(
                            new Vector3(v0.X, v0.Y, v0.Z),
                            new Vector3(v1.X, v1.Y, v1.Z),
                            new Vector3(v2.X, v2.Y, v2.Z),
                            new Vector3(n0.X, n0.Y, n0.Z),
                            new Vector3(n1.X, n1.Y, n1.Z),
                            new Vector3(n2.X, n2.Y, n2.Z)
                        ));
                    }
                }

                chessPieces.Add(new ChessPiece(new Vector3(0, 0, 0), Color.White, triangles.ToArray(), 0));

                // Update progress
                //progressBar.Value = Math.Min(progressBar.Value + progressIncrement, 100);
                //Application.DoEvents(); // Refresh UI
            }

            //progressBar.Value = 100; // Ensure it completes
            return chessPieces.ToArray();
        }        

        private Color MultiplyColor(Color color, float intensity)
        {
            int r = Math.Min(255, (int)(color.R * intensity));
            int g = Math.Min(255, (int)(color.G * intensity));
            int b = Math.Min(255, (int)(color.B * intensity));
            return Color.FromArgb(r, g, b);
        }


        private Color CalculateLighting(Vector3 hitPoint, Vector3 normal, Vector3 lightPos, Color objectColor, ObjectScene scene)
        {
            // Ambient light factor (constant across the entire scene)
            double ambientIntensity = intensity;
            Color ambientLight = ApplyAmbientLight(objectColor, ambientIntensity);

            // Diffuse lighting (based on angle of incidence)
            Vector3 lightDir = (lightPos - hitPoint).Normalize();
            double diffuseIntensity = Math.Max(0, normal.Dot(lightDir)); // Diffuse component

            // Specular lighting (Phong or Fresnel, depends on user selection)
            Color specularLight = Color.Black;
            if (radioButtonPhong.Checked)
            {
                specularLight = CalculatePhongSpecular(hitPoint, normal, lightPos);
            }
            else if (radioButtonFresnel.Checked)
            {
                specularLight = CalculateFresnelSpecular(hitPoint, normal, lightPos);
            }

            // Combine the ambient, diffuse, and specular components
            Color finalColor = CombineLighting(objectColor, ambientLight, diffuseIntensity, specularLight);

            // Shadow checking
            bool inShadow = IsInShadow(hitPoint, lightDir, lightPos, scene);
            if (inShadow)
            {
                finalColor = ApplyShadow(finalColor);
            }

            return finalColor;
        }

        private Color ApplyAmbientLight(Color objectColor, double ambientIntensity)
        {
            return Color.FromArgb(
                (int)(objectColor.R * ambientIntensity),
                (int)(objectColor.G * ambientIntensity),
                (int)(objectColor.B * ambientIntensity)
            );
        }

        private Color ApplyShadow(Color finalColor)
        {
            // Reduce brightness of the color for shadow areas
            return Color.FromArgb(
                (int)(finalColor.R * 0.5),
                (int)(finalColor.G * 0.5),
                (int)(finalColor.B * 0.5)
            );
        }



        // Reflect vector around normal
        private Vector3 Reflect(Vector3 rayDirection, Vector3 normal)
        {
            return rayDirection - normal * 2 * rayDirection.Dot(normal);
        }

        // Helper function to mix two colors based on a reflection factor
        private Color MixColors(Color color1, Color color2, double factor)
        {
            return Color.FromArgb(
                (int)(color1.R * (1 - factor) + color2.R * factor),
                (int)(color1.G * (1 - factor) + color2.G * factor),
                (int)(color1.B * (1 - factor) + color2.B * factor));
        }


        // Check if the point is in shadow by casting a ray towards the light
        private bool IsInShadow(Vector3 hitPoint, Vector3 lightDir, Vector3 lightPos, ObjectScene scene)
        {
            Vector3 shadowOrigin = hitPoint + lightDir * 1e-4; // Small offset to avoid self-intersection
            double lightDistance = (lightPos - hitPoint).Dot(lightPos - hitPoint); // Square of distance to the light

            // Check for intersections with spheres
            foreach (Sphere sphere in scene.spheres)
            {
                if (sphere.Intersect(new Ray(shadowOrigin, lightDir), out double t))
                {
                    if (t * t < lightDistance) // If intersection occurs before reaching the light
                        return true;
                }
            }

            // Check for intersections with walls
            foreach (Wall wall in scene.walls)
            {
                if (wall.Intersect(new Ray(shadowOrigin, lightDir), out double t))
                {
                    if (t * t < lightDistance)
                        return true;
                }
            }

            // Check for intersections with chess pieces
            foreach (ChessPiece chessPiece in scene.chessPieces)
            {
                if (chessPiece.IntersectRay(new Ray(shadowOrigin, lightDir), out double t, out Vector3 normal))
                {
                    if (t * t < lightDistance)
                        return true;
                }
            }

            return false; // No obstruction found, not in shadow
        }
    }
}