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
        private Color TraceRay(Vector3 origin, Vector3 direction, ObjectScene scene, Vector3 lightPos, Color backgroundColor, int depth)
        {
            if (depth <= 0)
                return backgroundColor;

            double nearestT = double.MaxValue;
            Sphere nearestSphere = null;
            Wall nearestWall = null;
            ChessPiece nearestChessPiece = null;
            Vector3 closest_normal = null;
            bool hitObject = false;
            Cube nearestCube = null;


            // Check intersection with spheres
            foreach (Sphere sphere in scene.spheres)
            {
                if (sphere.Intersect(origin, direction, out double t) && t < nearestT)
                {
                    nearestT = t;
                    nearestSphere = sphere;
                    nearestWall = null;
                    nearestChessPiece = null;
                    hitObject = true;
                }
            }

            // Check intersection with walls
            foreach (Wall wall in scene.walls)
            {
                if (wall.Intersect(origin, direction, out double t) && t < nearestT)
                {
                    nearestT = t;
                    nearestWall = wall;
                    nearestChessPiece = null;
                    nearestSphere = null;
                    hitObject = true;
                }
            }

            // Check intersection with chess pieces
            foreach (ChessPiece chessPiece in scene.chessPieces)
            {
                if (chessPiece.IntersectRay(origin, direction, out double t, out Vector3 potential_normal) && t < nearestT)
                {
                    nearestT = t;
                    nearestChessPiece = chessPiece;
                    nearestSphere = null;
                    nearestWall = null;
                    hitObject = true;
                    closest_normal = potential_normal;
                }
            }

            foreach (Cube cube in scene.cubes)
            {
                if (cube.Intersect(origin, direction, out double t, out Vector3 potentialNormal) && t < nearestT)
                {
                    nearestT = t;
                    nearestCube = cube;
                    nearestSphere = null;
                    nearestWall = null;
                    nearestChessPiece = null;
                    hitObject = true;
                    closest_normal = potentialNormal;
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
                Color lightingColor = CalculateLighting(hitPoint, normal, lightPos, objectColor, scene);

                // Reflection
                if (nearestSphere.Reflection > 0)
                {
                    Vector3 reflectionDir = Reflect(direction, normal);
                    Color reflectionColor = TraceRay(hitPoint, reflectionDir, scene, lightPos, backgroundColor, depth - 1);
                    lightingColor = MixColors(lightingColor, reflectionColor, nearestSphere.Reflection);
                }

                return lightingColor;
            }
            else if (nearestWall != null)
            {
                // Wall hit
                normal = nearestWall.Normal;
                objectColor = nearestWall.SurfaceColor;

                // Calculate lighting with shadow check
                return CalculateLighting(hitPoint, normal, lightPos, objectColor, scene);
            }
            else if (nearestChessPiece != null)
            {
                // Chess piece hit
                objectColor = nearestChessPiece.Color;

                Color lightingColor = CalculateLighting(hitPoint, closest_normal, lightPos, objectColor, scene);

                // Reflection
                if (nearestChessPiece.Reflection > 0)
                {
                    Vector3 reflectionDir = Reflect(direction, closest_normal);
                    Color reflectionColor = TraceRay(hitPoint, reflectionDir, scene, lightPos, backgroundColor, depth - 1);
                    lightingColor = MixColors(lightingColor, reflectionColor, nearestChessPiece.Reflection);
                }

                return lightingColor;
            }
            else if (nearestCube != null)
            {
                // Cube hit
                normal = closest_normal;
                objectColor = nearestCube.SurfaceColor;

                // Calculate lighting with shadow check
                Color lightingColor = CalculateLighting(hitPoint, normal, lightPos, objectColor, scene);

                // Reflection
                if (nearestCube.Reflection > 0)
                {
                    Vector3 reflectionDir = Reflect(direction, normal);
                    Color reflectionColor = TraceRay(hitPoint, reflectionDir, scene, lightPos, backgroundColor, depth - 1);
                    lightingColor = MixColors(lightingColor, reflectionColor, nearestCube.Reflection);
                }

                return lightingColor;
            }

            return backgroundColor;
        }
    }
}