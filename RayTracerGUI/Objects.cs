using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Assimp;

namespace RayTracerGUI
{

    public class ChessPiece
    {
        public double Reflection {  get; set; }

        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public Triangle[] Triangles { get; set; } // Array of triangles if loaded from OBJ

        public ChessPiece(Vector3 position, Color color, Triangle[] triangles, double reflection)
        {
            Position = position;
            Color = color;
            Triangles = triangles;
            Reflection = reflection;
        }

        public bool IntersectRay(Vector3 origin, Vector3 direction, out double distance, out Vector3 normal)
        {
            distance = double.MaxValue;
            normal = new Vector3(0, 0, 0);
            bool hit = false;

            foreach (var triangle in Triangles)
            {
                // Step 1: Check if the ray intersects the bounding sphere of the triangle
                //Sphere boundingSphere = triangle.GetBoundingSphere();
                //if (!boundingSphere.Intersect(origin, direction, out double sphereDist))
                //{
                //    continue; // Skip triangle if no intersection with bounding sphere
                //}

                // Step 2: Perform precise intersection with the triangle
                if (triangle.IntersectRay(origin, direction, out double dist) && dist < distance)
                {
                    distance = dist;
                    hit = true;
                    normal = triangle.Normal;
                }
            }
            return hit;
        }
    }

    public class Triangle
    {
        public Vector3 Vertex1 { get; set; }
        public Vector3 Vertex2 { get; set; }
        public Vector3 Vertex3 { get; set; }
        public Vector3 Normal { get; set; }

        public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
            Normal = normal;
        }

        public bool IntersectRay(Vector3 origin, Vector3 direction, out double distance)
        {
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
                return false; // Ray is parallel to the triangle plane
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
            return distance > epsilon;
        }


        public Sphere GetBoundingSphere()
        {
            Vector3 center = (Vertex1 + Vertex2 + Vertex3) / 3;
            double radius = Math.Max((Vertex1 - center).Length(), Math.Max((Vertex2 - center).Length(), (Vertex3 - center).Length()));
            return new Sphere(center, radius, Color.Empty, 0);
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

    // Plane class representing a plane object in the scene
    public class Plane
    {
        public Vector3 Point;
        public Vector3 Normal;
        public Color SurfaceColor;

        public Plane(Vector3 point, Vector3 normal, Color color)
        {
            Point = point;
            Normal = normal.Normalize();
            SurfaceColor = color;
        }

        // Ray-plane intersection
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
}
