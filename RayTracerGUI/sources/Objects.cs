using System;
using System.Collections.Generic;
using System.Drawing;

namespace RayTracer
{
    namespace Objects
    {
        public abstract class AbstractObject
        {
            abstract public bool IntersectRay(Ray ray, out double distance, out Vector3 Normal);
            public double Reflection { get; set; } = 0;

            public Color SurfaceColor { get; set; } = Color.Gray;

            public string Name { get; set; } = null;
        }


        public class Ray
        {
            public Vector3 origin;
            public Vector3 dir;

            public Ray()
            {
                this.origin = new Vector3(0, 0, 0);
                this.dir = new Vector3(0, 0, -1);
            }

            public Ray(Vector3 origin, Vector3 dir)
            {
                this.origin = origin;
                this.dir = dir;
            }

            public Ray Reflect(Vector3 normal) => new Ray(this.origin, this.dir.Reflect(normal));
        }


        public class ObjectScene
        {
            public List<AbstractObject> objects = new List<AbstractObject>();

            public ObjectScene(Sphere[] spheres = null, Wall[] walls = null, ChessPiece[] chessPieces = null, Cube[] cubes = null)
            {
                if (spheres != null)
                {
                    objects.AddRange(spheres);
                }

                if (walls != null)
                {
                    objects.AddRange(walls);
                }

                if (chessPieces != null)
                {
                    objects.AddRange(chessPieces);
                }

                if (cubes != null)
                {
                    objects.AddRange(cubes);
                }
            }
        }

        public class ChessPiece: AbstractObject
        {
            private Sphere BoundingSphere = null;
            public Vector3 Position { get; set; }
            public Triangle[] Triangles { get; set; }

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

                public bool IntersectRay(Ray ray, out double distance, out Vector3 barycentricCoords)
                {
                    barycentricCoords = new Vector3(0, 0, 0);
                    distance = 0;

                    Vector3 edge1 = Vertex2 - Vertex1;
                    Vector3 edge2 = Vertex3 - Vertex1;

                    // Begin Möller–Trumbore algorithm
                    Vector3 pvec = ray.dir.Cross(edge2);
                    double det = edge1.Dot(pvec);

                    const double epsilon = 1e-8;
                    if (Math.Abs(det) < epsilon)
                    {
                        return false; // Ray is parallel to the triangle
                    }

                    double invDet = 1.0 / det;

                    Vector3 tvec = ray.origin - Vertex1;
                    double u = tvec.Dot(pvec) * invDet;

                    // Check if the intersection lies outside the triangle
                    if (u < 0.0 || u > 1.0)
                    {
                        return false;
                    }

                    Vector3 qvec = tvec.Cross(edge1);
                    double v = ray.dir.Dot(qvec) * invDet;

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

            public ChessPiece(Vector3 position, Color color, Triangle[] triangles, double reflection, string name=null)
            {
                Position = position;
                SurfaceColor = color;
                Triangles = triangles;
                Reflection = reflection;
                Name = name;

                CalculateBoundingSphere();
            }

            public override bool IntersectRay(Ray ray, out double distance, out Vector3 interpolatedNormal)
            {
                distance = double.MaxValue;
                interpolatedNormal = new Vector3(0, 0, 0);

                if (!BoundingSphere.IntersectRay(ray, out double sphereDist, out Vector3 Normal))
                {
                    return false;
                }

                bool hit = false;
                foreach (var triangle in Triangles)
                {
                    if (triangle.IntersectRay(ray, out double dist, out Vector3 barycentricCoords) && dist < distance)
                    {
                        distance = dist;
                        hit = true;

                        // GURO KNIGHT TODO: PHONG
                        interpolatedNormal =
                            barycentricCoords.X * triangle.Normal1 +
                            barycentricCoords.Y * triangle.Normal2 +
                            barycentricCoords.Z * triangle.Normal3;

                        interpolatedNormal = interpolatedNormal.Normalize();
                    }
                }

                return hit;
            }

            private void CalculateBoundingSphere()
            {
                Vector3 sum = new Vector3(0, 0, 0);

                foreach (var triangle in Triangles)
                    sum += (triangle.Vertex1 + triangle.Vertex2 + triangle.Vertex3) / 3;

                Vector3 center = sum / Triangles.Length; // center of bounding sphere

                // finding radius as max distance from center
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


        public class Sphere : AbstractObject
        {
            public Vector3 Center;
            public double Radius;

            public Sphere(Vector3 center, double radius, Color color, double reflection, string name=null)
            {
                Center = center;
                Radius = radius;
                SurfaceColor = color;
                Reflection = reflection;
                Name = name;
            }

            public override bool IntersectRay(Ray ray, out double t, out Vector3 Normal)
            {
                Vector3 oc = ray.origin - Center;
                double a = ray.dir.Dot(ray.dir);
                double b = 2.0 * oc.Dot(ray.dir);
                double c = oc.Dot(oc) - Radius * Radius;
                double discriminant = b * b - 4 * a * c;
                Normal = null;

                if (discriminant < 0)
                {
                    t = 0;
                    return false;
                }

                t = (-b - Math.Sqrt(discriminant)) / (2.0 * a);

                if (t > 0)
                    Normal = (ray.origin + ray.dir * t - Center).Normalize();

                return t > 0;
            }
        }

        public class Wall : AbstractObject
        {
            public Vector3 Point { get; set; } // plane = point + normal
            public Vector3 Normal { get; set; }

            public Wall(Vector3 point, Vector3 normal, Color color, double reflection, string name=null)
            {
                Point = point;
                Normal = normal.Normalize();
                SurfaceColor = color;
                Reflection = reflection;
                Name = name;
            }

            public override bool IntersectRay(Ray ray, out double t, out Vector3 normal)
            {
                t = 0;
                normal = new Vector3(0, 0, 0);

                double denom = Normal.Dot(ray.dir);
                if (Math.Abs(denom) > 1e-6)
                {
                    t = (Point - ray.origin).Dot(Normal) / denom;
                    if (t >= 0)
                    {
                        normal = Normal;
                        return true;
                    }
                }
                return false;
            }
        }

        public class Cube: AbstractObject
        {
            public Vector3 Position { get; set; }
            public double Size { get; set; } // Edge length of the cube

            private Vector3 Min { get; set; }
            private Vector3 Max { get; set; }

            public Cube(Vector3 position, double size, Color color, double reflection, string name=null)
            {
                Position = position;
                Size = size;
                SurfaceColor = color;
                Reflection = reflection;

                Min = position - new Vector3(size / 2, size / 2, size / 2);
                Max = position + new Vector3(size / 2, size / 2, size / 2);
                Name = name;
            }

            public override bool IntersectRay(Ray ray, out double t, out Vector3 normal)
            {
                t = double.MaxValue;
                normal = new Vector3(0, 0, 0);

                double tMin = (Min.X - ray.origin.X) / ray.dir.X;
                double tMax = (Max.X - ray.origin.X) / ray.dir.X;

                if (tMin > tMax) (tMin, tMax) = (tMax, tMin);

                double tyMin = (Min.Y - ray.origin.Y) / ray.dir.Y;
                double tyMax = (Max.Y - ray.origin.Y) / ray.dir.Y;

                if (tyMin > tyMax) (tyMin, tyMax) = (tyMax, tyMin);

                if ((tMin > tyMax) || (tyMin > tMax))
                    return false;

                if (tyMin > tMin) tMin = tyMin;
                if (tyMax < tMax) tMax = tyMax;

                double tzMin = (Min.Z - ray.origin.Z) / ray.dir.Z;
                double tzMax = (Max.Z - ray.origin.Z) / ray.dir.Z;

                if (tzMin > tzMax) (tzMin, tzMax) = (tzMax, tzMin);

                double epsilon = 1e-8;

                if (tMin > tzMax + epsilon || tzMin > tMax + epsilon)
                    return false;

                tMin = Math.Max(tMin, tzMin);
                tMax = Math.Min(tMax, tzMax);

                if (tMin < 0)
                {
                    t = tMax; // If inside the cube, use farther hit
                    if (tMax < 0) 
                        return false;
                }
                else
                {
                    t = tMin;
                }

                Vector3 hitPoint = ray.origin + ray.dir * t;

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
}
