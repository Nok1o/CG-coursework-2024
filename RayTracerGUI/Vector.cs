using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerGUI
{
    public class Vector3
    {
        public double X, Y, Z;

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3 operator *(Vector3 a, double scalar) => new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static Vector3 operator /(Vector3 a, double scalar) => new Vector3(a.X / scalar, a.Y / scalar, a.Z / scalar);


        public double Dot(Vector3 other) => X * other.X + Y * other.Y + Z * other.Z;

        public Vector3 Cross(Vector3 other)
        {
            return new Vector3(
                Y * other.Z - Z * other.Y,
                Z * other.X - X * other.Z,
                X * other.Y - Y * other.X
            );
        }

        public Vector3 Normalize()
        {
            double length = Math.Sqrt(X * X + Y * Y + Z * Z);
            return new Vector3(X / length, Y / length, Z / length);
        }

        public double Length()
        {
            return X * X + Y * Y + Z * Z;
        }
    }


}
