using RayTracer.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera = RayTracer.Objects.Ray;


namespace RayTracer
{
    public partial class RayTracer
    {
        //private double reflectionFactor { get; set; } = 0.5;

        public enum Shading {Phong, Gourand};

        public Shading shading = Shading.Phong;


        public double intensity { get; set; } = 0.2;
        public double fov { get; set; } = Math.PI / 3;
        private int selectedScene { get; set; } = 0;

        ObjectScene sphereScene = new ObjectScene();
        ObjectScene chessScene = new ObjectScene();
        ObjectScene knightScene = new ObjectScene();

        Dictionary<int, ObjectScene> sceneDict = new Dictionary<int, ObjectScene>();

        ObjectScene currentScene = null;

        Vector3 lightPos;
        Camera camera = new Camera();

        private Dictionary<int, List<(Camera camera, Vector3 lightPos, double fov)>> cameraPositions =
            new Dictionary<int, List<(Camera, Vector3, double)>>();
        private AbstractObject selectedObject = null;

        private ColorCalculation colorCalculation = new ColorCalculation();

        public RayTracer()
        {
            InitializeCameraPositions();

            sceneDict[0] = sphereScene;
            sceneDict[1] = chessScene;
            sceneDict[2] = knightScene;
        }

        private void InitializeCameraPositions()
        {
            cameraPositions[0] = new List<(Camera, Vector3, double)>
            {
                (new Camera(new Vector3(0, 0, 2.5), new Vector3(0, 0, -1)), new Vector3(0, 4.7, -8), Math.PI / 3), // Default
                (new Camera(new Vector3(0, 2, 0), new Vector3(0, -0.5, -1).Normalize()), new Vector3(4.5, 4.5, 2.5), Math.PI / 3),
                (new Camera(new Vector3(-4.8, 4.8, -9.8), new Vector3(1, -1, 1).Normalize()), new Vector3(-2, 4, -6), Math.PI / 2.5),
            };

            cameraPositions[1] = new List<(Camera, Vector3, double)>
            {
                (new Camera(new Vector3(0, 2, 8), new Vector3(0, 0, -1)), new Vector3(0, 6, 0), Math.PI / 3), // Default
                (new Camera(new Vector3(-5, 5, 4), new Vector3(1, -1, -1).Normalize()), new Vector3(2, 7, 2), Math.PI / 3),
                (new Camera(new Vector3(-4.8, 1.5, 1.28), new Vector3(1, 0, 0).Normalize()), new Vector3(-3, 8, 4), Math.PI / 3),
            };

            cameraPositions[2] = new List<(Camera, Vector3, double)>
            {
                (new Camera(new Vector3(2.5, 2, 3.5), new Vector3(-1.5, 0, -2).Normalize()), new Vector3(2, 4, 0), Math.PI / 3), // Default
                (new Camera(new Vector3(-2.5, 2, 3.5), new Vector3(1.5, 0, -2).Normalize()), new Vector3(-1.5, 6, 2), Math.PI / 3),
                (new Camera(new Vector3(3.5, 5.5, -3.5), new Vector3(-1.5, -2.5, 1).Normalize()), new Vector3(-2, 4, -2), Math.PI / 3),
            };
        }

        public decimal selectedCameraChanged(uint index, bool updateFOV)
        {
            if (cameraPositions.Count > index && index >= 0)
            {
                var selectedCamera = cameraPositions[selectedScene][(int) index];
                camera.origin = selectedCamera.camera.origin;
                camera.dir = selectedCamera.camera.dir;
                lightPos = selectedCamera.lightPos;
                if (updateFOV)
                    return (decimal)(selectedCamera.fov / Math.PI * 180);
                return 0;
            }
            return -1;
        }

        private AbstractObject getObjectByTypeIndex(string type, int index)
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

        public void selectedObjectChanged(string type, int index)
        {
            selectedObject = getObjectByTypeIndex(type, index);
        }

        public (int index, string type) getClickedObject(double ndcX, double ndcY)
        {
            double scale = Math.Tan(fov / 2);
            Vector3 cameraRight = camera.dir.Cross(new Vector3(0, 1, 0)).Normalize();
            Vector3 cameraUp = cameraRight.Cross(camera.dir).Normalize();

            Vector3 rayDir = (cameraRight * (ndcX * scale) + cameraUp * (ndcY * scale) + camera.dir).Normalize();
            Ray ray = new Ray(camera.origin, rayDir);

            return FindObjectUnderMouse(ray);
        }

        private (int index, string type) FindObjectUnderMouse(Objects.Ray ray)
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

            selectedObject = clickedObject;
            
            return (clickedIndex, clickedType);
        }

        public void UpdateObjectReflectiveness((int index, string type) selection, double reflectionFactor)
        {
            if (selectedObject != null && selection.type != null && selection.index >= 0)
            {
                getObjectByTypeIndex(selection.type, selection.index).Reflection = reflectionFactor;
            }
        }
        public void UpdateObjectColor((int index, string type) selection, Color color)
        {
            if (selectedObject != null && selection.type != null && selection.index >= 0)
            {
                getObjectByTypeIndex(selection.type, selection.index).SurfaceColor = color;
            }
        }

        public void selectedSceneChanged(int sceneIndex)
        {
            currentScene = sceneDict[selectedScene];
            selectedScene = sceneIndex;

            if (sceneIndex == 0)
            {
                if (sphereScene.objects.Count == 0)
                    sphereScene = Settings.setupSphereScene();
                currentScene = sphereScene;
            }
            else if (sceneIndex == 1)
            {
                if (chessScene.objects.Count == 0)
                    chessScene = Settings.setupChessScene();

                currentScene = chessScene;
            }
            else if (sceneIndex == 2)
            {
                if (knightScene.objects.Count == 0)
                    knightScene = Settings.SetupKnightScene();

                currentScene = knightScene;
            }
        }

        public List<List<string>> getObjectList() 
        {
            if (currentScene == null)
                return null;

            List<List<string>> res = new List<List<string>>();

            for (int i = 0; i < currentScene.objects.Count; i++)
            {
                res.Add(new List<string>(new string[] { currentScene.objects[i].GetType().Name, currentScene.objects[i].Name }));
            }

            return res;
        }

        public Bitmap RenderSceneInterface(int width, int height, bool antiAliasing = false, bool DOF = false, double focalPlaneDistance = 0, int numRays = 16, ProgressBar bar = null)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Color backgroundColor = Color.Gray;

            RenderScene(bitmap, width, height, backgroundColor, antiAliasing, DOF, focalPlaneDistance, numRays, bar);

            return bitmap;
        }

        [ThreadStatic]
        private static Random random;

        private double GetRandomOffset()
        {
            if (random == null)
                random = new Random();
            return random.NextDouble() - 0.5; // Offset in the range [-0.5, 0.5]
        }

        private Vector3 RandomInUnitCircle()
        {
            if (random is null)
                random = new Random();
            double angle = random.NextDouble() * 2 * Math.PI;
            double radius = Math.Sqrt(random.NextDouble()); // Uniform sampling
            return new Vector3(Math.Cos(angle) * radius, Math.Sin(angle) * radius, 0);
        }

        private void RenderScene(Bitmap bitmap, int width, int height, Color backgroundColor, bool antiAliasing=false, bool DOF=false, double fPlaneDistance=0, int numRays = 16, ProgressBar bar = null)
        {
            Vector3 cameraRight = camera.dir.Cross(new Vector3(0, 1, 0)).Normalize(); // X-axis in camera space
            Vector3 cameraUp = cameraRight.Cross(camera.dir).Normalize();            // Y-axis in camera space

            double aspectRatio = (double)width / height;
            double scale = Math.Tan(fov / 2);

            int samplesPerPixel = (antiAliasing || DOF) ? numRays : 1; // anti-aliasing = 32
            double apertureSize = DOF ? 0.08 : 0; // Enable DOF if checked
            double focalPlaneDistance = fPlaneDistance;

            int totalPixels = width * height;
            int processedPixels = 0;
            double pixelWidth = 2.0f / width;
            double pixelHeight = 2.0f / height;
            Color[,] pixelBuffer = new Color[width, height];
            bar?.Invoke(new Action(() => bar.Value = 0));

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    double rSum = 0, gSum = 0, bSum = 0;

                    for (int s = 0; s < samplesPerPixel; s++)
                    {
                        double jitterX = 0;
                        double jitterY = 0;

                        if (antiAliasing)
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

                        Color sampleColor = TraceRay(new Ray(rayOrigin, rayDir), currentScene, lightPos, backgroundColor, 3);

                        rSum += sampleColor.R;
                        gSum += sampleColor.G;
                        bSum += sampleColor.B;
                    }

                    int r = (int)(rSum / samplesPerPixel);
                    int g = (int)(gSum / samplesPerPixel);
                    int b = (int)(bSum / samplesPerPixel);

                    Color finalColor = Color.FromArgb(
                        ColorCalculation.Clamp(r, 0, 255),
                        ColorCalculation.Clamp(g, 0, 255),
                        ColorCalculation.Clamp(b, 0, 255)
                    );


                    pixelBuffer[x, y] = finalColor;
                    //lock (bitmap)
                    //{
                    //    bitmap.SetPixel(x, y, finalColor);

                    processedPixels++;
                    if (bar != null && processedPixels % (totalPixels / 100) == 0)
                    {
                        int progress = (processedPixels * 100) / totalPixels;
                        bar.Invoke(new Action(() => bar.Value = progress));
                    }
                    //}
                }
            });

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, pixelBuffer[x, y]);
                }
            }

            bar?.Invoke(new Action(() => bar.Value = 100));
        }

    }
}
