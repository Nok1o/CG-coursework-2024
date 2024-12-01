using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Numerics;

using RayTracer.Objects;

using Camera = RayTracer.Objects.Ray;

namespace RayTracer 
{
    public partial class Form1 : Form
    {
        private double reflectionFactor = 0.5;
        private double intensity = 0.2;
        private string selectedScene = "Default";
        private ColorDialog colorDialog;


        ObjectScene sphereScene = new ObjectScene();
        ObjectScene chessScene = new ObjectScene();
        ObjectScene knightScene = new ObjectScene();

        ObjectScene currentScene = null;

        (object Object, int index, string type) selectedObject = (null, -1, null);

        Vector3 lightPos;
        Camera camera = new Camera();

        private Dictionary<string, List<(Camera camera, Vector3 lightPos)>> cameraPositions =
            new Dictionary<string, List<(Camera, Vector3)>>();

        private void InitializeCameraPositions()
        {
            cameraPositions["Sphere Scene"] = new List<(Camera, Vector3)>
            {
                (new Camera(new Vector3(0, 0, 2.5), new Vector3(0, 0, -1)), new Vector3(0, 4.7, -8)), // Default
                (new Camera(new Vector3(0, 2, 5), new Vector3(0, -1, -1).Normalize()), new Vector3(0, 5, -5)),
                (new Camera(new Vector3(-3, 0, 2), new Vector3(1, 0, -1).Normalize()), new Vector3(-2, 4, -6)),
            };

            cameraPositions["Chess Scene"] = new List<(Camera, Vector3)>
            {
                (new Camera(new Vector3(0, 2, 8), new Vector3(0, 0, -1)), new Vector3(0, 6, 0)), // Default
                (new Camera(new Vector3(3, 5, 10), new Vector3(-1, -1, -1).Normalize()), new Vector3(2, 7, 2)),
                (new Camera(new Vector3(-5, 2, 12), new Vector3(1, -0.5, -1).Normalize()), new Vector3(-3, 8, 4)),
            };

            cameraPositions["Knight Scene"] = new List<(Camera, Vector3)>
            {
                (new Camera(new Vector3(2.5, 2, 3.5), new Vector3(-1.5, 0, -2).Normalize()), new Vector3(2, 4, 0)), // Default
                (new Camera(new Vector3(0, 3, 6), new Vector3(0, -1, -2).Normalize()), new Vector3(0, 6, -1)),
                (new Camera(new Vector3(-3, 1, 4), new Vector3(1, 0.5, -1).Normalize()), new Vector3(-2, 4, -2)),
            };
        }

        private void PopulateCameraComboBox()
        {
            cameraComboBox.Items.Clear();
            if (cameraPositions.ContainsKey(selectedScene))
            {
                for (int i = 0; i < cameraPositions[selectedScene].Count; i++)
                {
                    cameraComboBox.Items.Add($"Camera {i + 1}");
                }
                cameraComboBox.SelectedIndex = 0; // Default to the first camera position
            }
        }

        private void cameraComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cameraPositions.ContainsKey(selectedScene) && cameraComboBox.SelectedIndex >= 0)
            {
                var selectedCamera = cameraPositions[selectedScene][cameraComboBox.SelectedIndex];
                camera.origin = selectedCamera.camera.origin;
                camera.dir = selectedCamera.camera.dir;
                lightPos = selectedCamera.lightPos;
            }
        }


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

            InitializeCameraPositions();
            cameraComboBox.SelectedIndexChanged += cameraComboBox_SelectedIndexChanged;
        }

        private void InitializeListView()
        {
            objectListView.View = View.Details;
            objectListView.FullRowSelect = true;
            objectListView.Columns.Add("Тип объекта", -2, HorizontalAlignment.Left);
            objectListView.Columns.Add("Имя объекта", -2, HorizontalAlignment.Left);
            objectListView.Columns.Add("Индекс", -2, HorizontalAlignment.Left);

            //PopulateListView();

            objectListView.SelectedIndexChanged += ObjectListView_SelectedIndexChanged;
        }

        private void ObjectListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objectListView.SelectedItems.Count == 0)
                return;

            ListViewItem item = objectListView.SelectedItems[0];
            string type = item.SubItems[0].Text;
            int index = int.Parse(item.SubItems[2].Text);

            selectedObject = (GetObjectByTypeAndIndex(type, index), index, type);
            //HighlightSelectedObject();
        }

        private object GetObjectByTypeAndIndex(string type, int index)
        {
            int i = -1;
            int j = 0;
            while (i < index && j < currentScene.objects.Count)
            {
                if (currentScene.objects[j].GetType().Name == type)
                {
                    i++;
                }
                j++;
            }

            return currentScene.objects[i];
        }

        private void SelectListViewItem(string type, int index)
        {
            objectListView.SelectedItems.Clear();
            foreach (ListViewItem item in objectListView.Items)
            {
                if (item.SubItems[0].Text == type && item.SubItems[2].Text == index.ToString())
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
                // Get camera basis vectors
                Vector3 cameraRight = camera.dir.Cross(new Vector3(0, 1, 0)).Normalize();
                Vector3 cameraUp = cameraRight.Cross(camera.dir).Normalize();

                // Screen-space coordinates normalized to [-1, 1]
                double aspectRatio = (double)pictureBox1.Width / pictureBox1.Height;
                double fov = Math.PI / 3.0; // Match the FOV used in RenderScene
                double scale = Math.Tan(fov / 2);

                double ndcX = (2 * ((e.X + 0.5) / pictureBox1.Width) - 1) * aspectRatio;
                double ndcY = 1 - 2 * ((e.Y + 0.5) / pictureBox1.Height);

                // Compute ray direction in world space
                Vector3 rayDir = (cameraRight * (ndcX * scale) + cameraUp * (ndcY * scale) + camera.dir).Normalize();
                Ray ray = new Ray(camera.origin, rayDir);

                // Trace ray to find the object clicked on
                selectedObject = FindObjectUnderMouse(ray);
                SelectListViewItem(selectedObject.type, selectedObject.index);

                // Debugging (optional)
                Console.WriteLine($"Ray Origin: {ray.origin}");
                Console.WriteLine($"Ray Direction: {ray.dir}");
            }
        }


        private (object selectedObject, int index, string type) FindObjectUnderMouse(Objects.Ray ray)
        {
            double closestDistance = double.MaxValue;
            AbstractObject clickedObject = null;
            int clickedIndex = -1;
            string clickedType = "";

            for (int i = 0; i < currentScene.objects.Count; i++)
            {
                if (currentScene.objects[i].IntersectRay(ray, out double distance, out Vector3 _) && distance < closestDistance)
                {
                    closestDistance = distance;
                    clickedObject = currentScene.objects[i];
                    clickedIndex = i;
                    clickedType = currentScene.objects[i].GetType().Name;
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
            currentScene.objects[selection.index].SurfaceColor = newColor;
        }

        private void UpdateObjectReflectiveness((object obj, int index, string type) selection, double reflectiveness)
        {
            currentScene.objects[selection.index].Reflection = reflectiveness;
        }

        private void fillListView()
        {
            objectListView.Items.Clear();

            for (int i = 0; i < currentScene.objects.Count; i++)
            {
                objectListView.Items.Add(new ListViewItem(new string[] { currentScene.objects[i].GetType().Name, currentScene.objects[i].Name, i.ToString() }));
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
                if (sphereScene.objects.Count == 0)
                {
                    sphereScene = setupSphereScene();
                    PopulateCameraComboBox();
                }
                currentScene = sphereScene;
            }
            else if (selectedScene == "Chess Scene")
            {
                if (chessScene.objects.Count == 0)
                {
                    chessScene = setupChessScene();
                    PopulateCameraComboBox();
                }
                currentScene = chessScene;
            }
            else if (selectedScene == "Knight Scene")
            {
                if (knightScene.objects.Count == 0)
                {
                    knightScene = setupKnightScene();
                    PopulateCameraComboBox();
                }
                currentScene = knightScene;
            }

            fillListView();

            // Run the render on a background thread
            await Task.Run(() =>
            {
                RenderScene(bitmap, width, height, camera, lightPos, backgroundColor, currentScene);
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

        private void RenderScene(Bitmap bitmap, int width, int height, Camera camera, Vector3 lightPos, Color backgroundColor, ObjectScene scene)
        {
            Vector3 cameraRight = camera.dir.Cross(new Vector3(0, 1, 0)).Normalize(); // X-axis in camera space
            Vector3 cameraUp = cameraRight.Cross(camera.dir).Normalize();            // Y-axis in camera space

            double aspectRatio = (double)width / height;
            double fov = Math.PI / 3.0; // 60 degrees field of view
            double scale = Math.Tan(fov / 2);

            int samplesPerPixel = (chkAntiAliasing.Checked || depthOfFieldCheckbox.Checked) ? 32 : 1; // Enable anti-aliasing if checked
            double apertureSize = depthOfFieldCheckbox.Checked ? 0.08 : 0; // Enable DOF if checked
            double focalPlaneDistance = (double)focalPlaneDistanceControl.Value;

            int totalPixels = width * height;
            int processedPixels = 0;
            double pixelWidth = 1.0f / width;
            double pixelHeight = 1.0f / height;

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    double rSum = 0, gSum = 0, bSum = 0;

                    for (int s = 0; s < samplesPerPixel; s++)
                    {
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
                        Vector3 rayDir = (cameraRight * (ndcX * scale) + cameraUp * (ndcY * scale) + camera.dir).Normalize();
                        Vector3 localcameraRight = rayDir.Cross(new Vector3(0, 1, 0)).Normalize(); // X-axis in camera space
                        Vector3 localcameraUp = cameraRight.Cross(rayDir).Normalize();

                        // Apply depth of field (DOF) if enabled
                        Vector3 rayOrigin = camera.origin;
                        if (apertureSize > 0)
                        {
                            Vector3 focalPoint = rayOrigin + rayDir * focalPlaneDistance;

                            var jitter = RandomInUnitCircle() * apertureSize;
                            Vector3 apertureOffset = jitter.X * localcameraRight + jitter.Y * localcameraUp;
                            rayOrigin += apertureOffset;

                            rayDir = (focalPoint - rayOrigin).Normalize();
                        }

                        Color sampleColor = TraceRay(new Ray(rayOrigin, rayDir), scene, lightPos, backgroundColor, 3);

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
            var importer = new Assimp.AssimpContext();
            Assimp.Scene scene;
            if (mode == 1)
            {
                scene = importer.ImportFile("models/chess.obj", Assimp.PostProcessSteps.Triangulate);
            }
            else
            {
                scene = importer.ImportFile("models/knight.obj", Assimp.PostProcessSteps.Triangulate);
            }
            var chessPieces = new List<ChessPiece>();

            int totalMeshes = scene.Meshes.Count;
            int progressIncrement = totalMeshes > 0 ? 100 / totalMeshes : 100;
            //progressBar.Value = 0; // Reset progress bar

            foreach (var mesh in scene.Meshes)
            {
                var triangles = new List<ChessPiece.Triangle>();

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

                        triangles.Add(new ChessPiece.Triangle(
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

            foreach (var obj in scene.objects)
            {
                if (obj.IntersectRay(new Ray(shadowOrigin, lightDir), out double t, out Vector3 _))
                {
                    if (t * t < lightDistance)
                        return true;
                }
            }
            
            return false; // No obstruction found, not in shadow
        }
    }
}