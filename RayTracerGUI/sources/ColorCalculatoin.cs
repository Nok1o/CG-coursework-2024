using RayTracer.Objects;
using System;
using System.Drawing;
using System.Windows.Forms;



namespace RayTracer
{
    public class ColorCalculation
    {
        public Color CalculateLighting(Vector3 CameraOrigin, Vector3 hitPoint, Vector3 normal, Vector3 lightPos, Color objectColor, ObjectScene scene, double ambientIntensity, bool Phong)
        {
            // Ambient light factor (constant across the entire scene)
            Color ambientLight = ApplyAmbientLight(objectColor, ambientIntensity);

            // Diffuse lighting (based on angle of incidence)
            Vector3 lightDir = (lightPos - hitPoint).Normalize();
            double diffuseIntensity = Math.Max(0, normal.Dot(lightDir)); // Diffuse component

            // Specular lighting (Phong or Fresnel, depends on user selection)
            Color specularLight = Color.Black;
            if (Phong)
            {
                specularLight = CalculatePhongSpecular(CameraOrigin, hitPoint, normal, lightPos, ambientIntensity);
            }
            else
            {
                specularLight = CalculateFresnelSpecular(CameraOrigin, hitPoint, normal, lightPos, ambientIntensity);
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

        private Color CalculatePhongSpecular(Vector3 cameraOrigin, Vector3 hitPoint, Vector3 normal, Vector3 lightPos, double intensity)
        {
            int shininess = 32;

            // Calculate reflection direction
            Vector3 viewDir = (cameraOrigin - hitPoint).Normalize();
            Vector3 lightDir = (lightPos - hitPoint).Normalize();
            Vector3 reflectDir = (normal * (2.0 * normal.Dot(lightDir)) - lightDir).Normalize();

            // Calculate specular component
            double specFactor = Math.Pow(Math.Max(viewDir.Dot(reflectDir), 0), shininess) * intensity;

            // Return specular component (white highlight)
            return Color.FromArgb(
                Clamp((int)(255 * specFactor), 0, 255),
                Clamp((int)(255 * specFactor), 0, 255),
                Clamp((int)(255 * specFactor), 0, 255)
            );
        }

        private Color CalculateFresnelSpecular(Vector3 cameraOrigin, Vector3 hitPoint, Vector3 normal, Vector3 lightPos, double intensity)
        {
            // Fresnel reflection using Schlick's approximation
            Vector3 viewDir = (cameraOrigin - hitPoint).Normalize(); // Assume camera at origin
            double cosTheta = Math.Max(0, viewDir.Dot(normal));
            //double fresnelFactor = Math.Pow(1 - cosTheta, 10) * 0.01 + 0.1; // зеркальная составляющая
            double fresnelFactor = Math.Pow(1 - cosTheta, 10) * intensity + 0.1; // зеркальная составляющая

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
        public static Color MixColors(Color color1, Color color2, double factor)
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

        public static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        private Color MultiplyColor(Color color, float intensity)
        {
            int r = Math.Min(255, (int)(color.R * intensity));
            int g = Math.Min(255, (int)(color.G * intensity));
            int b = Math.Min(255, (int)(color.B * intensity));
            return Color.FromArgb(r, g, b);
        }
    }
}
