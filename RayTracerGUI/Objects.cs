using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Assimp;

namespace RayTracerGUI
{
    public class ObjectScene
    {

        public Sphere[] spheres = Array.Empty<Sphere>();
        public Wall[] walls = Array.Empty<Wall>();
        public ChessPiece[] chessPieces = Array.Empty<ChessPiece>();
        public Cube[] cubes = Array.Empty<Cube>();

        public ObjectScene(Sphere[] spheres = null, Wall[] walls = null, ChessPiece[] chessPieces = null, Cube[] cubes = null)
        {
            this.spheres = spheres ?? Array.Empty<Sphere>();
            this.walls = walls ?? Array.Empty<Wall>();
            this.chessPieces = chessPieces ?? Array.Empty<ChessPiece>();
            this.cubes = cubes ?? Array.Empty<Cube>();
        }
    }

    public class ChessPiece
    {
        private Sphere BoundingSphere = null;
        public double Reflection { get; set; }
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public Triangle[] Triangles { get; set; } // Array of triangles if loaded from OBJ

        public ChessPiece(Vector3 position, Color color, Triangle[] triangles, double reflection)
        {
            Position = position;
            Color = color;
            Triangles = triangles;
            Reflection = reflection;

            CalculateBoundingSphere();
        }

        public bool IntersectRay(Vector3 origin, Vector3 direction, out double distance, out Vector3 interpolatedNormal)
        {
            distance = double.MaxValue;
            interpolatedNormal = new Vector3(0, 0, 0);

            // Step 1: Check if the ray intersects the bounding sphere of the chess piece
            if (!BoundingSphere.Intersect(origin, direction, out double sphereDist))
            {
                return false; // Skip the chess piece if no intersection with bounding sphere
            }

            // Step 2: If intersected with bounding sphere, check individual triangles
            bool hit = false;
            foreach (var triangle in Triangles)
            {
                if (triangle.IntersectRay(origin, direction, out double dist, out Vector3 barycentricCoords) && dist < distance)
                {
                    distance = dist;
                    hit = true;

                    // Interpolate normals using barycentric coordinates
                    // GURO KNIGHT TODO: PHONG
                    interpolatedNormal =
                        barycentricCoords.X * triangle.Normal1 +
                        barycentricCoords.Y * triangle.Normal2 +
                        barycentricCoords.Z * triangle.Normal3;

                    // Normalize the interpolated normal
                    interpolatedNormal = interpolatedNormal.Normalize();
                }
            }

            return hit;
        }

        private void CalculateBoundingSphere()
        {
            // Step 1: Calculate the center of the bounding sphere by averaging triangle vertices
            Vector3 sum = new Vector3(0, 0, 0);

            foreach (var triangle in Triangles)
                sum += (triangle.Vertex1 + triangle.Vertex2 + triangle.Vertex3) / 3;

            Vector3 center = sum / Triangles.Length; // Average position of all vertices

            // Step 2: Calculate the radius as the maximum distance from the center to any vertex
            double maxDistance = 0;
            foreach (var triangle in Triangles)
            {
                maxDistance = Math.Max(maxDistance, (triangle.Vertex1 - center).Length());
                maxDistance = Math.Max(maxDistance, (triangle.Vertex2 - center).Length());
                maxDistance = Math.Max(maxDistance, (triangle.Vertex3 - center).Length());
            }

            BoundingSphere = new Sphere(center, maxDistance, Color.Empty, 0);
        }
    }

    public class Triangle
    {
        public Vector3 Vertex1 { get; set; }
        public Vector3 Vertex2 { get; set; }
        public Vector3 Vertex3 { get; set; }

        public Vector3 Normal1 { get; set; }
        public Vector3 Normal2 { get; set; }
        public Vector3 Normal3 { get; set; }
        public Vector3 FaceNormal { get; set; }

        public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal1, Vector3 normal2, Vector3 normal3)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
            Normal1 = normal1;
            Normal2 = normal2;
            Normal3 = normal3;
            FaceNormal = (normal1 + normal2 + normal3).Normalize();
        }

        public bool IntersectRay(Vector3 origin, Vector3 direction, out double distance, out Vector3 barycentricCoords)
        {
            barycentricCoords = new Vector3(0, 0, 0);
            distance = 0;

            Vector3 edge1 = Vertex2 - Vertex1;
            Vector3 edge2 = Vertex3 - Vertex1;

            // Begin Möller–Trumbore algorithm
            Vector3 pvec = direction.Cross(edge2);
            double det = edge1.Dot(pvec);

            // Use a small epsilon for improved accuracy
            const double epsilon = 1e-8;
            if (Math.Abs(det) < epsilon)
            {
                return false; // Ray is parallel to the triangle
            }

            double invDet = 1.0 / det;

            Vector3 tvec = origin - Vertex1;
            double u = tvec.Dot(pvec) * invDet;

            // Check if the intersection lies outside the triangle
            if (u < 0.0 || u > 1.0)
            {
                return false;
            }

            Vector3 qvec = tvec.Cross(edge1);
            double v = direction.Dot(qvec) * invDet;

            // Check if the intersection lies outside the triangle
            if (v < 0.0 || u + v > 1.0)
            {
                return false;
            }

            // Calculate the distance from origin to the intersection point
            distance = edge2.Dot(qvec) * invDet;

            // Ensure the intersection point is in the forward direction of the ray
            if (distance > epsilon)
            {
                // Calculate barycentric coordinates
                barycentricCoords = new Vector3(1 - u - v, u, v);
                return true;
            }

            return false;
        }
    }


    // Sphere class representing a sphere object in the scene
    public class Sphere
    {
        public Vector3 Center;
        public double Radius;
        public Color SurfaceColor;
        public double Reflection;

        public Sphere(Vector3 center, double radius, Color color, double reflection)
        {
            Center = center;
            Radius = radius;
            SurfaceColor = color;
            Reflection = reflection;
        }

        public bool Intersect(Vector3 rayOrigin, Vector3 rayDirection, out double t)
        {
            Vector3 oc = rayOrigin - Center;
            double a = rayDirection.Dot(rayDirection);
            double b = 2.0 * oc.Dot(rayDirection);
            double c = oc.Dot(oc) - Radius * Radius;
            double discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                t = 0;
                return false;
            }

            t = (-b - Math.Sqrt(discriminant)) / (2.0 * a);
            return t > 0;
        }
    }

    // Wall class representing a Wall object in the scene
    public class Wall
    {
        public Vector3 Point;
        public Vector3 Normal;
        public Color SurfaceColor;
        public double Reflection;

        public Wall(Vector3 point, Vector3 normal, Color color)
        {
            Point = point;
            Normal = normal.Normalize();
            SurfaceColor = color;
        }


        public bool Intersect(Vector3 rayOrigin, Vector3 rayDirection, out double t)
        {
            double denom = Normal.Dot(rayDirection);
            if (Math.Abs(denom) > 1e-6)
            {
                t = (Point - rayOrigin).Dot(Normal) / denom;
                return t >= 0;
            }
            t = 0;
            return false;
        }
    }

    public class Cube
    {
        public Vector3 Position { get; set; }
        public double Size { get; set; } // Edge length of the cube
        public Color SurfaceColor { get; set; }
        public double Reflection { get; set; }

        private Vector3 Min { get; set; }
        private Vector3 Max { get; set; }

        public Cube(Vector3 position, double size, Color color, double reflection)
        {
            Position = position;
            Size = size;
            SurfaceColor = color;
            Reflection = reflection;

            Min = position - new Vector3(size / 2, size / 2, size / 2);
            Max = position + new Vector3(size / 2, size / 2, size / 2);
        }

        public bool Intersect(Vector3 rayOrigin, Vector3 rayDirection, out double t, out Vector3 normal)
        {
            t = double.MaxValue;
            normal = new Vector3(0, 0, 0);

            double tMin = (Min.X - rayOrigin.X) / rayDirection.X;
            double tMax = (Max.X - rayOrigin.X) / rayDirection.X;

            if (tMin > tMax) (tMin, tMax) = (tMax, tMin);

            double tyMin = (Min.Y - rayOrigin.Y) / rayDirection.Y;
            double tyMax = (Max.Y - rayOrigin.Y) / rayDirection.Y;

            if (tyMin > tyMax) (tyMin, tyMax) = (tyMax, tyMin);

            if ((tMin > tyMax) || (tyMin > tMax))
                return false;

            if (tyMin > tMin) tMin = tyMin;
            if (tyMax < tMax) tMax = tyMax;

            double tzMin = (Min.Z - rayOrigin.Z) / rayDirection.Z;
            double tzMax = (Max.Z - rayOrigin.Z) / rayDirection.Z;

            if (tzMin > tzMax) (tzMin, tzMax) = (tzMax, tzMin);

            double epsilon = 1e-8; // Smaller epsilon for higher precision

            if (tMin > tzMax + epsilon || tzMin > tMax + epsilon)
                return false;

            tMin = Math.Max(tMin, tzMin);
            tMax = Math.Min(tMax, tzMax);

            if (tMin < 0)
            {
                t = tMax; // If inside the cube, use farther hit
                if (tMax < 0) return false; // No valid hit
            }
            else
            {
                t = tMin;
            }

            // Calculate the surface normal at the intersection point
            Vector3 hitPoint = rayOrigin + rayDirection * t;

            if (Math.Abs(hitPoint.X - Min.X) < epsilon) normal = new Vector3(-1, 0, 0); // Left face
            else if (Math.Abs(hitPoint.X - Max.X) < epsilon) normal = new Vector3(1, 0, 0);  // Right face
            else if (Math.Abs(hitPoint.Y - Min.Y) < epsilon) normal = new Vector3(0, -1, 0); // Bottom face
            else if (Math.Abs(hitPoint.Y - Max.Y) < epsilon) normal = new Vector3(0, 1, 0);  // Top face
            else if (Math.Abs(hitPoint.Z - Min.Z) < epsilon) normal = new Vector3(0, 0, -1); // Front face
            else if (Math.Abs(hitPoint.Z - Max.Z) < epsilon) normal = new Vector3(0, 0, 1);  // Back face

            return true;
        }

    }

}
