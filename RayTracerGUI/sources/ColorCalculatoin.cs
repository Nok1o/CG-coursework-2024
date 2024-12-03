using RayTracer.Objects;
using System;
using System.Drawing;
using System.Windows.Forms;



namespace RayTracer
{
    public partial class Form1 : Form
    {
        private Color CalculatePhongSpecular(Vector3 hitPoint, Vector3 normal, Vector3 lightPos)
        {
            int shininess = 32;

            // Calculate reflection direction
            Vector3 viewDir = (camera.origin - hitPoint).Normalize();
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

        private Color CalculateFresnelSpecular(Vector3 hitPoint, Vector3 normal, Vector3 lightPos)
        {
            // Fresnel reflection using Schlick's approximation
            Vector3 viewDir = (camera.origin - hitPoint).Normalize(); // Assume camera at origin
            double cosTheta = Math.Max(0, viewDir.Dot(normal));
            double fresnelFactor = Math.Pow(1 - cosTheta, 10) * 0.01 + 0.1; // зеркальная составляющая

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

        private Color[] CalculateVertexLighting(ChessPiece.Triangle triangle, Vector3 lightPos, Color baseColor, ObjectScene scene)
        {
            return new[]
            {
                CalculateLighting(triangle.Vertex1, triangle.Normal1, lightPos, baseColor, scene),
                CalculateLighting(triangle.Vertex2, triangle.Normal2, lightPos, baseColor, scene),
                CalculateLighting(triangle.Vertex3, triangle.Normal3, lightPos, baseColor, scene)
            };
        }

    }
}
