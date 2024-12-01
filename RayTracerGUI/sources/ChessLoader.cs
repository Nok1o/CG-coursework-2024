using RayTracer.Objects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;


namespace RayTracer
{
    internal class ChessLoader
    {
        public ChessLoader() { }


        static public ChessPiece LoadChessPiece(string filename, Vector3 offset = null, string name = null)
        {
            var importer = new Assimp.AssimpContext();
            var scene = importer.ImportFile("obj_models/" + filename, Assimp.PostProcessSteps.Triangulate);
            if (scene == null)
                throw new Exception("Error loading file: " + filename);

            int totalMeshes = scene.Meshes.Count;
            if (totalMeshes < 1)
                throw new Exception("No meshes found in file: " + filename);

            var mesh = scene.Meshes[0];
            var triangles = new ConcurrentBag<ChessPiece.Triangle>();
            object lockObject = new object();


            if (offset is null)
                Parallel.ForEach(mesh.Faces, face =>
                {
                    if (face.Indices.Count == 3)
                    {
                        var v0 = mesh.Vertices[face.Indices[0]];
                        var v1 = mesh.Vertices[face.Indices[1]];
                        var v2 = mesh.Vertices[face.Indices[2]];
                        var n0 = mesh.Normals[face.Indices[0]];
                        var n1 = mesh.Normals[face.Indices[1]];
                        var n2 = mesh.Normals[face.Indices[2]];

                        var triangle = new ChessPiece.Triangle(
                            new Vector3(v0.X, v0.Y, v0.Z),
                            new Vector3(v1.X, v1.Y, v1.Z),
                            new Vector3(v2.X, v2.Y, v2.Z),
                            new Vector3(n0.X, n0.Y, n0.Z),
                            new Vector3(n1.X, n1.Y, n1.Z),
                            new Vector3(n2.X, n2.Y, n2.Z)
                        );

                        lock (lockObject)
                        {
                            triangles.Add(triangle);
                        }
                    }
                });
            else
            {
                Parallel.ForEach(mesh.Faces, face =>
                {
                    if (face.Indices.Count == 3)
                    {
                        var v0 = mesh.Vertices[face.Indices[0]];
                        var v1 = mesh.Vertices[face.Indices[1]];
                        var v2 = mesh.Vertices[face.Indices[2]];
                        var n0 = mesh.Normals[face.Indices[0]];
                        var n1 = mesh.Normals[face.Indices[1]];
                        var n2 = mesh.Normals[face.Indices[2]];

                        var triangle = new ChessPiece.Triangle(
                            new Vector3(v0.X, v0.Y, v0.Z) + offset,
                            new Vector3(v1.X, v1.Y, v1.Z) + offset,
                            new Vector3(v2.X, v2.Y, v2.Z) + offset,
                            new Vector3(n0.X, n0.Y, n0.Z),
                            new Vector3(n1.X, n1.Y, n1.Z),
                            new Vector3(n2.X, n2.Y, n2.Z)
                        );

                        lock (lockObject)
                        {
                            triangles.Add(triangle);
                        }
                    }
                });
            }

            return new ChessPiece(
                new Vector3(0, 0, 0),
                Color.White,
                triangles.ToArray(),
                0,
                name ?? mesh.Name
            );
        }
    }
}
