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

        public Form1()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            SceneChooser.Items.Add("Default");
            SceneChooser.Items.Add("Chess Scene");
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
            reflectionFactor = trackBarReflection.Value / 100.0; // Convert from 0-100 to 0-1
            labelReflection.Text = $"Reflection: {trackBarReflection.Value}%";
        }

        private void trackIntensity_Scroll(object sender, EventArgs e)
        {
            intensity = trackIntensity.Value / 100.0;
            labelIntensity.Text = $"Intensity: {trackIntensity.Value}%";
        }

        private Sphere[] GetDefaultSpheres()
        {
            return new Sphere[]
            {
                new Sphere(new Vector3(-2, 0, -6), 2, Color.Red, reflectionFactor),
                new Sphere(new Vector3(2, 0, -8), 2, Color.Green, reflectionFactor),
            };
        }

        private Plane[] GetDefaultWalls()
        {
            return new Plane[]
            {

                new Plane(new Vector3(-5, 0, 0), new Vector3(1, 0, 0), Color.Red),   // Left wall
                new Plane(new Vector3(5, 0, 0), new Vector3(-1, 0, 0), Color.Green), // Right wall
                new Plane(new Vector3(0, 0, -10), new Vector3(0, 0, 1), Color.Blue), // Back wall
                new Plane(new Vector3(0, 5, 0), new Vector3(0, -1, 0), Color.White), // Ceiling
                new Plane(new Vector3(0, -2, 0), new Vector3(0, 1, 0), Color.Yellow), // Floor
                new Plane(new Vector3(0, 0, 1), new Vector3(0, 0, -1), Color.SandyBrown)
            };
        }

        private Plane[] GetChessWalls()
        {
            return new Plane[]
            {

                new Plane(new Vector3(-6, 0, 0), new Vector3(1, 0, 0), Color.Red),   // Left wall
                new Plane(new Vector3(6, 0, 0), new Vector3(-1, 0, 0), Color.Green), // Right wall
                new Plane(new Vector3(0, 0, -12), new Vector3(0, 0, 1), Color.Blue), // Back wall
                new Plane(new Vector3(0, 10, 0), new Vector3(0, -1, 0), Color.White), // Ceiling
                new Plane(new Vector3(0, 0, 0), new Vector3(0, 1, 0), Color.Yellow), // Floor
                new Plane(new Vector3(0, 0, 12), new Vector3(0, 0, -1), Color.SandyBrown)
            };
        }

        private async void btnRender_Click(object sender, EventArgs e)
        {
            int width = 800;
            int height = 600;
            Bitmap bitmap = new Bitmap(width, height);
            Color backgroundColor = Color.Gray;
            Vector3 cameraPos = null, lightPos = null;

            Sphere[] spheres = Array.Empty<Sphere>();
            Plane[] walls = Array.Empty<Plane>();
            ChessPiece[] chessPieces = Array.Empty<ChessPiece>();

            if (selectedScene == "Default")
            {
                cameraPos = new Vector3(0, 0, 0);
                lightPos = new Vector3(0, 4.7, -8);
                spheres = GetDefaultSpheres();
                walls = GetDefaultWalls();
            }
            else if (selectedScene == "Chess Scene")
            {
                cameraPos = new Vector3(0, 2, 8);
                lightPos = new Vector3(0, 6, 0);
                chessPieces = LoadChessPieces();
                walls = GetChessWalls();
            }

            // Run the render on a background thread
            await Task.Run(() =>
            {
                RenderScene(bitmap, width, height, cameraPos, lightPos, backgroundColor, spheres, walls, chessPieces);
            });

            pictureBox1.Image = bitmap;
        }

        private void RenderScene(Bitmap bitmap, int width, int height, Vector3 cameraPos, Vector3 lightPos, Color backgroundColor, Sphere[] spheres, Plane[] walls, ChessPiece[] chessPieces)
        {
            int totalPixels = width * height;
            int processedPixels = 0;
            object lockObj = new object();

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    double i = (2 * (x + 0.5) / width - 1) * width / height;
                    double j = -(2 * (y + 0.5) / height - 1);
                    Vector3 rayDirection = new Vector3(i, j, -1).Normalize();

                    Color pixelColor = TraceRay(cameraPos, rayDirection, spheres, walls, chessPieces, lightPos, backgroundColor, 5);

                    lock (lockObj)
                    {
                        bitmap.SetPixel(x, y, pixelColor);
                    }

                    // Update the progress bar asynchronously
                    lock (lockObj)
                    {
                        processedPixels++;
                        if (processedPixels % (totalPixels / 100) == 0)
                        {
                            int progress = (processedPixels * 100) / totalPixels;
                            progressBar.Invoke(new Action(() => progressBar.Value = progress));
                        }
                    }
                }
            });

            // Ensure progress bar completes
            progressBar.Invoke(new Action(() => progressBar.Value = 100));
        }

        private ChessPiece[] LoadChessPieces()
        {
            var importer = new AssimpContext();
            var scene = importer.ImportFile("../../chess.obj", PostProcessSteps.Triangulate);
            var chessPieces = new List<ChessPiece>();

            int totalMeshes = scene.Meshes.Count;
            int progressIncrement = totalMeshes > 0 ? 100 / totalMeshes : 100;
            //progressBar.Value = 0; // Reset progress bar

            foreach (var mesh in scene.Meshes)
            {
                //if (i++ > 2)
                //    break;
                //var mesh = scene.Meshes[0];
                var triangles = new List<Triangle>();

                foreach (var face in mesh.Faces)
                {
                    if (face.Indices.Count == 3)
                    {
                        var v0 = mesh.Vertices[face.Indices[0]];
                        var v1 = mesh.Vertices[face.Indices[1]];
                        var v2 = mesh.Vertices[face.Indices[2]];
                        var normal = (mesh.Normals[face.Indices[0]] + mesh.Normals[face.Indices[1]] + mesh.Normals[face.Indices[2]]) / 3;

                        triangles.Add(new Triangle(
                            new Vector3(v0.X, v0.Y, v0.Z),
                            new Vector3(v1.X, v1.Y, v1.Z),
                            new Vector3(v2.X, v2.Y, v2.Z),
                            new Vector3(normal.X, normal.Y, normal.Z)
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

        // Recursive ray tracing function
        private Color TraceRay(Vector3 origin, Vector3 direction, Sphere[] spheres, Plane[] walls, ChessPiece[] chessPieces, Vector3 lightPos, Color backgroundColor, int depth)
        {
            if (depth <= 0)
                return backgroundColor;

            double nearestT = double.MaxValue;
            Sphere nearestSphere = null;
            Plane nearestPlane = null;
            ChessPiece nearestChessPiece = null;
            Vector3 closest_normal = null;
            bool hitObject = false;

            // Check intersection with spheres
            foreach (Sphere sphere in spheres)
            {
                if (sphere.Intersect(origin, direction, out double t) && t < nearestT)
                {
                    nearestT = t;
                    nearestSphere = sphere;
                    nearestPlane = null;
                    nearestChessPiece = null;
                    hitObject = true;
                }
            }

            // Check intersection with walls
            foreach (Plane wall in walls)
            {
                if (wall.Intersect(origin, direction, out double t) && t < nearestT)
                {
                    nearestT = t;
                    nearestPlane = wall;
                    nearestChessPiece = null;
                    nearestSphere = null;
                    hitObject = true;
                }
            }

            // Check intersection with chess pieces
            foreach (ChessPiece chessPiece in chessPieces)
            {
                if (chessPiece.IntersectRay(origin, direction, out double t, out Vector3 potential_normal) && t < nearestT)
                {
                    nearestT = t;
                    nearestChessPiece = chessPiece;
                    nearestSphere = null;
                    nearestPlane = null;
                    hitObject = true;
                    closest_normal = potential_normal;
                }
            }

            if (!hitObject)
                return backgroundColor; // No hit, return background color

            Vector3 hitPoint = origin + direction * nearestT;
            Vector3 normal;
            Color objectColor;

            if (nearestSphere != null)
            {
                // Sphere hit
                normal = (hitPoint - nearestSphere.Center).Normalize();
                objectColor = nearestSphere.SurfaceColor;

                // Calculate lighting with shadow check
                Color lightingColor = CalculateLighting(hitPoint, normal, lightPos, objectColor, spheres, walls, chessPieces);

                // Reflection
                if (nearestSphere.Reflection > 0)
                {
                    Vector3 reflectionDir = Reflect(direction, normal);
                    Color reflectionColor = TraceRay(hitPoint, reflectionDir, spheres, walls, chessPieces, lightPos, backgroundColor, depth - 1);
                    lightingColor = MixColors(lightingColor, reflectionColor, nearestSphere.Reflection);
                }

                return lightingColor;
            }
            else if (nearestPlane != null)
            {
                // Wall hit
                normal = nearestPlane.Normal;
                objectColor = nearestPlane.SurfaceColor;

                // Calculate lighting with shadow check
                return CalculateLighting(hitPoint, normal, lightPos, objectColor, spheres, walls, chessPieces);
            }
            else if (nearestChessPiece != null)
            {
                // Chess piece hit
                objectColor = nearestChessPiece.Color;

                Color lightingColor = CalculateLighting(hitPoint, closest_normal, lightPos, objectColor, spheres, walls, chessPieces);

                // Reflection
                if (nearestChessPiece.Reflection > 0)
                {
                    Vector3 reflectionDir = Reflect(direction, closest_normal);
                    Color reflectionColor = TraceRay(hitPoint, reflectionDir, spheres, walls, chessPieces, lightPos, backgroundColor, depth - 1);
                    lightingColor = MixColors(lightingColor, reflectionColor, nearestChessPiece.Reflection);
                }

                return lightingColor;
            }


            return backgroundColor;
        }

            private Color MultiplyColor(Color color, float intensity)
        {
            int r = Math.Min(255, (int)(color.R * intensity));
            int g = Math.Min(255, (int)(color.G * intensity));
            int b = Math.Min(255, (int)(color.B * intensity));
            return Color.FromArgb(r, g, b);
        }

        // Updated lighting calculation to include shadows
        // Updated lighting calculation to include both Phong and Fresnel shading
        private Color CalculateLighting(Vector3 hitPoint, Vector3 normal, Vector3 lightPos, Color objectColor, Sphere[] spheres, Plane[] walls, ChessPiece[] chessPieces)
        {
            // Ambient light factor (constant across the entire scene)
            double ambientIntensity = 0.5;
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
            bool inShadow = IsInShadow(hitPoint, lightDir, lightPos, spheres, walls, chessPieces);
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

        private Color CalculatePhongSpecular(Vector3 hitPoint, Vector3 normal, Vector3 lightPos)
        {
            // Phong specular intensity parameters
            double specularIntensity = intensity;
            int shininess = 32;

            // Calculate reflection direction
            Vector3 viewDir = (new Vector3(0, 0, 0) - hitPoint).Normalize(); // Assume camera at origin
            Vector3 lightDir = (lightPos - hitPoint).Normalize();
            Vector3 reflectDir = (normal * (2.0 * normal.Dot(lightDir)) - lightDir).Normalize();

            // Calculate specular component
            double specFactor = Math.Pow(Math.Max(viewDir.Dot(reflectDir), 0), shininess) * specularIntensity;

            // Return specular component (white highlight)
            return Color.FromArgb(
                (int)(255 * specFactor),
                (int)(255 * specFactor),
                (int)(255 * specFactor)
            );
        }

        private Color CalculateFresnelSpecular(Vector3 hitPoint, Vector3 normal, Vector3 lightPos)
        {
            // Fresnel reflection using Schlick's approximation
            Vector3 viewDir = (new Vector3(0, 0, 0) - hitPoint).Normalize(); // Assume camera at origin
            double cosTheta = Math.Max(0, viewDir.Dot(normal));
            double fresnelFactor = Math.Pow(1 - cosTheta, 10) * 0.9 + 0.1; // Adjust reflection strength

            // Return Fresnel reflection (white for simplicity)
            return Color.FromArgb(
                (int)(255 * fresnelFactor),
                (int)(255 * fresnelFactor),
                (int)(255 * fresnelFactor)
            );
        }

        private Color CombineLighting(Color objectColor, Color ambientLight, double diffuseIntensity, Color specularLight)
        {
            // Combine ambient and diffuse lighting
            int r = Math.Min(255, (int)(objectColor.R * diffuseIntensity + ambientLight.R + specularLight.R));
            int g = Math.Min(255, (int)(objectColor.G * diffuseIntensity + ambientLight.G + specularLight.G));
            int b = Math.Min(255, (int)(objectColor.B * diffuseIntensity + ambientLight.B + specularLight.B));

            return Color.FromArgb(r, g, b);
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
        private bool IsInShadow(Vector3 hitPoint, Vector3 lightDir, Vector3 lightPos, Sphere[] spheres, Plane[] walls, ChessPiece[] chessPieces)
        {
            Vector3 shadowOrigin = hitPoint + lightDir * 1e-4; // Small offset to avoid self-intersection
            double lightDistance = (lightPos - hitPoint).Dot(lightPos - hitPoint); // Square of distance to the light

            // Check for intersections with spheres
            foreach (Sphere sphere in spheres)
            {
                if (sphere.Intersect(shadowOrigin, lightDir, out double t))
                {
                    if (t * t < lightDistance) // If intersection occurs before reaching the light
                        return true;
                }
            }

            // Check for intersections with walls
            foreach (Plane wall in walls)
            {
                if (wall.Intersect(shadowOrigin, lightDir, out double t))
                {
                    if (t * t < lightDistance)
                        return true;
                }
            }

            // Check for intersections with chess pieces
            foreach (ChessPiece chessPiece in chessPieces)
            {
                if (chessPiece.IntersectRay(shadowOrigin, lightDir, out double t, out Vector3 normal))
                {
                    if (t * t < lightDistance)
                        return true;
                }
            }

            return false; // No obstruction found, not in shadow
        }

        // Phong shading model
        private Color PhongShading(Color objectColor, double intensity)
        {
            return Color.FromArgb(
                (int)(objectColor.R * intensity),
                (int)(objectColor.G * intensity),
                (int)(objectColor.B * intensity));
        }

        // Fresnel reflection model using Schlick's approximation
        private Color FresnelShading(Color objectColor, Vector3 hitPoint, Vector3 normal, Vector3 lightPos)
        {
            // Calculate reflection angle (Fresnel effect)
            Vector3 viewDir = (hitPoint - new Vector3(0, 0, 0)).Normalize(); // Assuming camera at origin
            double cosTheta = Math.Max(0, viewDir.Dot(normal));
            double fresnelFactor = Math.Pow(1 - cosTheta, 3) * 0.9 + 0.1; // Adjust reflection strength

            // Mix object color with reflection color (here, we'll use white for simplicity)
            Color reflectionColor = Color.White;

            return Color.FromArgb(
                (int)(objectColor.R * (1 - fresnelFactor) + reflectionColor.R * fresnelFactor),
                (int)(objectColor.G * (1 - fresnelFactor) + reflectionColor.G * fresnelFactor),
                (int)(objectColor.B * (1 - fresnelFactor) + reflectionColor.B * fresnelFactor));
        }


    }
}
