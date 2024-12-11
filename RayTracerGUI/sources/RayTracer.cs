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

            double closestDistance = double.MaxValue;
            Vector3 hitNormal = new Vector3(0, 0, 0);
            AbstractObject closestObject = null;

            foreach (var obj in scene.objects)
            {
                if (callingObject != obj)
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


            Color lightingColor = colorCalculation.CalculateLighting(ray.origin, hitPoint, hitNormal, lightPos, objectColor, scene, intensity);


            if (closestObject.Reflection > 0)
            {
                Vector3 reflectionDir = ray.dir.Reflect(hitNormal);
                Color reflectionColor = TraceRay(new Objects.Ray(hitPoint, reflectionDir), scene, lightPos, backgroundColor, depth - 1, closestObject);
                lightingColor = ColorCalculation.MixColors(lightingColor, reflectionColor, closestObject.Reflection);
            }

            return lightingColor;
        }
    }
}