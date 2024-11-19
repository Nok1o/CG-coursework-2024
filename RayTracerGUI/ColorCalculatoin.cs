using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayTracerGUI
{
    public partial class Form1 : Form
    {
        private Color CalculatePhongSpecular(Vector3 hitPoint, Vector3 normal, Vector3 lightPos)
        {
            int shininess = 32;

            // Calculate reflection direction
            Vector3 viewDir = (new Vector3(0, 0, 0) - hitPoint).Normalize(); // Assume camera at origin
            Vector3 lightDir = (lightPos - hitPoint).Normalize();
            Vector3 reflectDir = (normal * (2.0 * normal.Dot(lightDir)) - lightDir).Normalize();

            // Calculate specular component
            double specFactor = Math.Pow(Math.Max(viewDir.Dot(reflectDir), 0), shininess) * intensity;

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
            Vector3 viewDir = (cameraPos - hitPoint).Normalize(); // Assume camera at origin
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
    }
}
