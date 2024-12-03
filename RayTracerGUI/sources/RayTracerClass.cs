using Assimp;
using RayTracer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Camera = RayTracer.Objects.Ray;


namespace RayTracer
{
    class RayTracer
    {
        private double reflectionFactor { get; set; } = 0.5;
        private double intensity { get; set; } = 0.2;
        private string selectedScene { get; set; } = "Default";
        private double fov { get; set; } = Math.PI / 3;

        ObjectScene sphereScene = new ObjectScene();
        ObjectScene chessScene = new ObjectScene();
        ObjectScene knightScene = new ObjectScene();

        ObjectScene currentScene = null;

        Vector3 lightPos;
        Camera camera = new Camera();

        private Dictionary<string, List<(Camera camera, Vector3 lightPos, double fov)>> cameraPositions =
            new Dictionary<string, List<(Camera, Vector3, double)>>();

        public RayTracer()
        {
            InitializeCameraPositions();
        }

        private void InitializeCameraPositions()
        {
            cameraPositions["Sphere Scene"] = new List<(Camera, Vector3, double)>
            {
                (new Camera(new Vector3(0, 0, 2.5), new Vector3(0, 0, -1)), new Vector3(0, 4.7, -8), Math.PI / 3), // Default
                (new Camera(new Vector3(0, 2, 0), new Vector3(0, -0.5, -1).Normalize()), new Vector3(4.5, 4.5, 2.5), Math.PI / 3),
                (new Camera(new Vector3(-4.8, 4.8, -9.8), new Vector3(1, -1, 1).Normalize()), new Vector3(-2, 4, -6), Math.PI / 2.5),
            };

            cameraPositions["Chess Scene"] = new List<(Camera, Vector3, double)>
            {
                (new Camera(new Vector3(0, 2, 8), new Vector3(0, 0, -1)), new Vector3(0, 6, 0), Math.PI / 3), // Default
                (new Camera(new Vector3(-5, 5, 4), new Vector3(1, -1, -1).Normalize()), new Vector3(2, 7, 2), Math.PI / 3),
                (new Camera(new Vector3(-4.8, 1.5, 1.28), new Vector3(1, 0, 0).Normalize()), new Vector3(-3, 8, 4), Math.PI / 3),
            };

            cameraPositions["Knight Scene"] = new List<(Camera, Vector3, double)>
            {
                (new Camera(new Vector3(2.5, 2, 3.5), new Vector3(-1.5, 0, -2).Normalize()), new Vector3(2, 4, 0), Math.PI / 3), // Default
                (new Camera(new Vector3(-2.5, 2, 3.5), new Vector3(1.5, 0, -2).Normalize()), new Vector3(-1.5, 6, 2), Math.PI / 3),
                (new Camera(new Vector3(3.5, 5.5, -3.5), new Vector3(-1.5, -2.5, 1).Normalize()), new Vector3(-2, 4, -2), Math.PI / 3),
            };
        }
    }
}
