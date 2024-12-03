using System;
using System.Drawing;
using System.Windows.Forms;
using RayTracer.Objects;


namespace RayTracer
{
    public partial class Form1 : Form
    {
        private Color TraceRay(Objects.Ray ray, ObjectScene scene, Vector3 lightPos, Color backgroundColor, int depth)
        {
            if (depth <= 0)
            {
                return backgroundColor;
            }

            double closestDistance = double.MaxValue;
            Vector3 hitNormal = new Vector3(0, 0, 0);
            AbstractObject closestObject = null;

            foreach (var obj in scene.objects)
            {
                if (obj.IntersectRay(ray, out double dist, out Vector3 normal) && dist < closestDistance)
                {
                    closestDistance = dist;
                    hitNormal = normal;
                    closestObject = obj;
                }
            }

            if (closestObject == null)
            {
                return backgroundColor;
            }

            Vector3 hitPoint = ray.origin + ray.dir * closestDistance;
            Color objectColor = ((dynamic)closestObject).SurfaceColor;


            Color lightingColor = CalculateLighting(hitPoint, hitNormal, lightPos, objectColor, scene);


            if (closestObject.Reflection > 0)
            {
                Vector3 reflectionDir = ray.dir.Reflect(hitNormal);
                Color reflectionColor = TraceRay(new Objects.Ray(hitPoint, reflectionDir), scene, lightPos, backgroundColor, depth - 1);
                lightingColor = MixColors(lightingColor, reflectionColor, closestObject.Reflection);
            }

            return lightingColor;
        }

        // Helper: Generate a random point in a unit circle (for aperture jittering)
        private Vector3 RandomInUnitCircle()
        {
            if (random is null)
                random = new Random();
            double angle = random.NextDouble() * 2 * Math.PI;
            double radius = Math.Sqrt(random.NextDouble()); // Uniform sampling
            return new Vector3(Math.Cos(angle) * radius, Math.Sin(angle) * radius, 0);
        }

        // Helper: Clamp a value between min and max
        private int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}