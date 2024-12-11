using RayTracer.Objects;
using System;
using System.Drawing;
using System.Windows.Forms;



namespace RayTracer
{

    public class ColorCalculation
    {
        public double finishFactor { get; set; } = 32 / 130.0;

        public Color CalculateLighting(Vector3 cameraOrigin, Vector3 hitPoint, Vector3 normal, Vector3 lightPos, Color objectColor, ObjectScene scene, double ambientIntensity)
        {
            // Ambient light factor
            Color ambientLight = ApplyAmbientLight(objectColor, ambientIntensity);

            // Diffuse lighting (angle of incidence)
            Vector3 lightDir = (lightPos - hitPoint).Normalize();
            double diffuseIntensity = Math.Max(0, normal.Dot(lightDir));

            // Specular lighting (Phong and Fresnel combined)
            int shininess = (int)(20 + finishFactor * 150);
            Color specularLight = CalculateSpecular(cameraOrigin, hitPoint, normal, lightPos, ambientIntensity, shininess, objectColor);

            // Combine diffuse and specular contributions
            double specularFactor = finishFactor; // Controls shininess
            double diffuseFactor = 1.0 - finishFactor; // Controls matteness
            double normalizationFactor = 1.0 / (diffuseFactor + specularFactor); // Normalize

            diffuseFactor *= normalizationFactor;
            specularFactor *= normalizationFactor;
            double ambientBoost = 1.0 - finishFactor; // Increase ambient for matte surfaces
            ambientIntensity += ambientBoost * 0.1;  // Adjust factor as needed


            // Compute final lighting
            int r = Clamp((int)(
                objectColor.R * (ambientIntensity + diffuseIntensity) +
                specularLight.R * finishFactor));

            int g = Clamp((int)(
                objectColor.G * (ambientIntensity + diffuseIntensity) +
                specularLight.G * finishFactor));

            int b = Clamp((int)(
                objectColor.B * (ambientIntensity + diffuseIntensity) +
                specularLight.B * finishFactor));

            Color finalColor = Color.FromArgb(r, g, b);

            // Shadow handling
            if (IsInShadow(hitPoint, lightDir, lightPos, scene))
            {
                finalColor = ApplyShadow(finalColor, ambientIntensity);
            }

            return finalColor;
        }

        int Clamp(int value)
        {
            return Math.Max(0, Math.Min(255, value));
        }


        public Color CombineLighting(Color objectColor, Color ambientLight, double diffuseIntensity, double diffuseWeight, Color specularLight, double specularWeight)
        {
            // Combine ambient, normalized diffuse, and normalized specular lighting
            int r = Math.Min(255, (int)(objectColor.R * (diffuseIntensity * diffuseWeight) + ambientLight.R + specularLight.R * specularWeight));
            int g = Math.Min(255, (int)(objectColor.G * (diffuseIntensity * diffuseWeight) + ambientLight.G + specularLight.G * specularWeight));
            int b = Math.Min(255, (int)(objectColor.B * (diffuseIntensity * diffuseWeight) + ambientLight.B + specularLight.B * specularWeight));

            return Color.FromArgb(r, g, b);
        }





        public Color CalculateSpecular(Vector3 cameraOrigin, Vector3 hitPoint, Vector3 normal, Vector3 lightPos, double intensity, int shininess, Color objectColor)
        {
            // Phong Specular Calculation
            Vector3 viewDir = (cameraOrigin - hitPoint).Normalize();
            Vector3 lightDir = (lightPos - hitPoint).Normalize();
            Vector3 reflectDir = (normal * (2.0 * normal.Dot(lightDir)) - lightDir).Normalize();
            double phongSpecFactor = Math.Pow(Math.Max(viewDir.Dot(reflectDir), 0), shininess);

            // Fresnel Specular Calculation
            double cosTheta = Math.Max(0, viewDir.Dot(normal));
            double fresnelFactor = Math.Pow(1 - cosTheta, 5) * intensity + 0.1; // Schlick's approximation

            // Adjust blending rate based on shininess
            double phongWeight = Math.Min(1.0, shininess / 89.6); // Shinier surfaces favor Phong
            double fresnelWeight = 1.0 - phongWeight;             // Matte surfaces favor Fresnel

            // Combine Phong and Fresnel components
            double specFactor = phongSpecFactor * phongWeight + fresnelFactor * fresnelWeight;

            // Return combined specular highlight (white for simplicity)
            int specIntensity = Clamp((int)(255 * specFactor), 0, 255);
            return Color.FromArgb(Clamp((int) (specFactor * objectColor.R)), Clamp((int)(specFactor * objectColor.G)), Clamp((int)(specFactor * objectColor.B)));
        }



        public Color CalculatePhongSpecular(Vector3 cameraOrigin, Vector3 hitPoint, Vector3 normal, Vector3 lightPos, double intensity)
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

        public Color CalculateFresnelSpecular(Vector3 cameraOrigin, Vector3 hitPoint, Vector3 normal, Vector3 lightPos, double intensity)
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

        public Color CombineLighting(Color objectColor, Color ambientLight, double diffuseWeight, Color specularLight, double specularWeight)
        {
            // Combine ambient, normalized diffuse, and normalized specular lighting
            int r = Math.Min(255, (int)(objectColor.R * diffuseWeight + ambientLight.R + specularLight.R * specularWeight));
            int g = Math.Min(255, (int)(objectColor.G * diffuseWeight + ambientLight.G + specularLight.G * specularWeight));
            int b = Math.Min(255, (int)(objectColor.B * diffuseWeight + ambientLight.B + specularLight.B * specularWeight));

            return Color.FromArgb(r, g, b);
        }




        public Color ApplyAmbientLight(Color objectColor, double ambientIntensity)
        {
            return Color.FromArgb(
                (int)(objectColor.R * ambientIntensity),
                (int)(objectColor.G * ambientIntensity),
                (int)(objectColor.B * ambientIntensity)
            );
        }

        public Color ApplyShadow(Color finalColor, double lightIntensity)
        {
            // Reduce brightness of the color for shadow areas
            return Color.FromArgb(
                (int)(finalColor.R * Math.Max(0.4, Math.Min(lightIntensity, 0.8))),
                (int)(finalColor.G * Math.Max(0.4, Math.Min(lightIntensity, 0.8))),
                (int)(finalColor.B * Math.Max(0.4, Math.Min(lightIntensity, 0.8)))
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
        public bool IsInShadow(Vector3 hitPoint, Vector3 lightDir, Vector3 lightPos, ObjectScene scene)
        {
            Vector3 shadowOrigin = hitPoint + lightDir * 1e-4;
            double lightDistance = (lightPos - hitPoint).Dot(lightPos - hitPoint);

            foreach (var obj in scene.objects)
            {
                if (obj.GetType().Name == "Sphere")
                {
                    if (((Sphere)obj).IntersectRayForShadow(new Ray(shadowOrigin, lightDir), out double t_))
                    {
                        if (t_ * t_ < lightDistance)
                            return true;
                    }
                    else
                        continue;
                }
                if (obj.IntersectRay(new Ray(shadowOrigin, lightDir), out double t, out Vector3 _))
                {
                    if (t * t < lightDistance)
                        return true;
                }
            }

            return false;
        }

        public static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}
