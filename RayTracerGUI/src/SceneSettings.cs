using System;
using System.Drawing;
using System.Windows.Forms;
using RayTracer.Objects;

namespace RayTracer
{
    public partial class Form1 : Form
    {
        private Sphere[] GetDefaultSpheres()
        {
            return new Sphere[]
            {
                new Sphere(new Vector3(-2, 0, -6), 2, Color.Red, reflectionFactor),
                new Sphere(new Vector3(2, 0, -8), 2, Color.Green, reflectionFactor),
            };
        }

        private Wall[] GetDefaultWalls()
        {
            return new Wall[]
            {

                new Wall(new Vector3(-5, 0, 0), new Vector3(1, 0, 0), Color.Red, 0),   // Left wall
                new Wall(new Vector3(5, 0, 0), new Vector3(-1, 0, 0), Color.Green, 0), // Right wall
                new Wall(new Vector3(0, 0, -10), new Vector3(0, 0, 1), Color.Blue, 0), // Front wall
                new Wall(new Vector3(0, 5, 0), new Vector3(0, -1, 0), Color.White, 0), // Ceiling
                new Wall(new Vector3(0, -2, 0), new Vector3(0, 1, 0), Color.Yellow, 0), // Floor
                new Wall(new Vector3(0, 0, 3), new Vector3(0, 0, -1), Color.SandyBrown, 0) // Back wall
            };
        }

        private Wall[] GetChessWalls()
        {
            return new Wall[]
            {

                new Wall(new Vector3(-6, 0, 0), new Vector3(1, 0, 0), Color.Red, 0),   // Left wall
                new Wall(new Vector3(6, 0, 0), new Vector3(-1, 0, 0), Color.Green, 0), // Right wall
                new Wall(new Vector3(0, 0, -12), new Vector3(0, 0, 1), Color.Blue, 0), // Front wall
                new Wall(new Vector3(0, 10, 0), new Vector3(0, -1, 0), Color.White, 0), // Ceiling
                new Wall(new Vector3(0, 0, 0), new Vector3(0, 1, 0), Color.Yellow, 0), // Floor
                new Wall(new Vector3(0, 0, 12), new Vector3(0, 0, -1), Color.SandyBrown, 0) // Back wall
            };
        }

        private ObjectScene setupSphereScene()
        {
            return new ObjectScene(GetDefaultSpheres(), GetDefaultWalls());
        }

        private void setupSphereCamera()
        {
            camera.origin = new Vector3(0, 0, 2.5);
            lightPos = new Vector3(0, 4.7, -8);
            camera.dir = new Vector3(0, 0, -1);
        }

        private ObjectScene setupChessScene()
        {
            //return new ObjectScene(null, GetChessWalls(), LoadChessPieces());
            return new ObjectScene(null, GetChessWalls(), new [] { ChessLoader.LoadChessPiece("king.obj") });
        }

        private void setupChessCamera()
        {

            camera.origin = new Vector3(0, 2, 8);
            lightPos = new Vector3(0, 6, 0);
            camera.dir = new Vector3(0, 0, -1);

        }

        private ObjectScene setupKnightScene()
        {
            return new ObjectScene(new[] { new Sphere(new Vector3(0, 2.5, -3), 1.5, Color.White, 0.5) }, GetChessWalls(), LoadChessPieces(0),
                cubes: new[] {new Cube(new Vector3(0, 0.5, -3), 1, Color.White, 0)});
        }

        private void setupKnightCamera()
        {
            camera.origin = new Vector3(2.5, 2, 3.5);
            lightPos = new Vector3(2, 4, 0);
            camera.dir = new Vector3(-1.5, 0, -2).Normalize();
        }
    }
}

/*
        private void setupKnightCamera()
        {
            camera.origin = new Vector3(2.5, 2, 3.5);
            lightPos = new Vector3(2, 4, 0);
            camera.dir = new Vector3(-1.5, 0, -2).Normalize();
        }
 */
/*
        private ObjectScene setupKnightScene()
        {
            var cube = new Cube(new[]
                {
                    new Vector3(-0.5, 0, -3.5), new Vector3(0.5, 0, -3.5), new Vector3(0.5, 1, -3.5), new Vector3(-0.5, 1, -3.5),
                    new Vector3(-0.5, 1, -2.5), new Vector3(0.5, 1, -2.5), new Vector3(0.5, 0, -2.5), new Vector3(-0.5, 0, -2.5),
            }, Color.Red, 0);
            return new ObjectScene(new[] { new Sphere(new Vector3(0, 2.5, -3), 1.5, Color.White, 0.5) }, GetChessWalls(), LoadChessPieces(0),
                cubes: new[] {cube});
        }
*/
