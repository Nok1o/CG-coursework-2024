using System;
using System.Drawing;
using System.Windows.Forms;
using RayTracer.Objects;


namespace RayTracer
{
    public partial class RayTracer
    {
        private Color TraceRay(Objects.Ray ray, ObjectScene scene, Vector3 lightPos, Color backgroundColor, int depth, AbstractObject callingObject = null)
        {
            if (depth <= 0)
            {
                return backgroundColor;
            }

            // Find closest intersection
            double closestDistance = double.MaxValue;
            Vector3 hitNormal = default;
            AbstractObject closestObject = null;

            foreach (var obj in scene.objects)
            {
                if (callingObject != obj && obj.IntersectRay(ray, out double dist, out Vector3 normal) && dist < closestDistance)
                {
                    closestDistance = dist;
                    hitNormal = normal;
                    closestObject = obj;
                }
            }

            // No intersection: return background color
            if (closestObject == null)
            {
                return backgroundColor;
            }

            // Compute hit point
            Vector3 hitPoint = ray.origin + ray.dir * closestDistance;
            Color objectColor = closestObject.SurfaceColor;

            // Calculate lighting using Phong shading (lighting at the intersection point)
            Color lightingColor = colorCalculation.CalculateLighting(
                ray.origin, hitPoint, hitNormal, lightPos, objectColor, scene, intensity);

            // Handle reflections
            if (closestObject.Reflection > 0)
            {
                Vector3 reflectionDir = ray.dir.Reflect(hitNormal).Normalize();
                Objects.Ray reflectedRay = new Objects.Ray(hitPoint, reflectionDir);

                Color reflectionColor = TraceRay(reflectedRay, scene, lightPos, backgroundColor, depth - 1, closestObject);

                // Blend lighting and reflection colors
                lightingColor = ColorCalculation.MixColors(lightingColor, reflectionColor, closestObject.Reflection);
            }

            return lightingColor;
        }
    }
}