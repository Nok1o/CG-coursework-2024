﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
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
        public static Vector3 operator -(Vector3 a) => new Vector3(-a.X, -a.Y, -a.Z);


        public static Vector3 operator *(Vector3 a, double scalar) => new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static Vector3 operator *(double scalar, Vector3 a) => new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);

        public static Vector3 operator /(Vector3 a, double scalar) => new Vector3(a.X / scalar, a.Y / scalar, a.Z / scalar);

        public static Vector3 operator /(double scalar, Vector3 a) => new Vector3(a.X / scalar, a.Y / scalar, a.Z / scalar);

        public static bool operator ==(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Vector3 a, Vector3 b) => a.X != b.X || a.Y == b.Y || a.Z == b.Z;

        public bool Equals(Vector3 a) => a.X == X && a.Y == Y && a.Z == Z;



        public double this[int num]
        {
            get
            {
                if (num == 0) return X;
                if (num == 1) return Y;
                if (num == 2) return Z;
                else throw new ArgumentException("Bad index");
            }
        }



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
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 Reflect(Vector3 normal) => this - normal * 2 * this.Dot(normal);
    }
}
