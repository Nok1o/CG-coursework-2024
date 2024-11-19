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

                new Wall(new Vector3(-5, 0, 0), new Vector3(1, 0, 0), Color.Red),   // Left wall
                new Wall(new Vector3(5, 0, 0), new Vector3(-1, 0, 0), Color.Green), // Right wall
                new Wall(new Vector3(0, 0, -10), new Vector3(0, 0, 1), Color.Blue), // Front wall
                new Wall(new Vector3(0, 5, 0), new Vector3(0, -1, 0), Color.White), // Ceiling
                new Wall(new Vector3(0, -2, 0), new Vector3(0, 1, 0), Color.Yellow), // Floor
                new Wall(new Vector3(0, 0, 3), new Vector3(0, 0, -1), Color.SandyBrown) // Back wall
            };
        }

        private Wall[] GetChessWalls()
        {
            return new Wall[]
            {

                new Wall(new Vector3(-6, 0, 0), new Vector3(1, 0, 0), Color.Red),   // Left wall
                new Wall(new Vector3(6, 0, 0), new Vector3(-1, 0, 0), Color.Green), // Right wall
                new Wall(new Vector3(0, 0, -12), new Vector3(0, 0, 1), Color.Blue), // Front wall
                new Wall(new Vector3(0, 10, 0), new Vector3(0, -1, 0), Color.White), // Ceiling
                new Wall(new Vector3(0, 0, 0), new Vector3(0, 1, 0), Color.Yellow), // Floor
                new Wall(new Vector3(0, 0, 12), new Vector3(0, 0, -1), Color.SandyBrown) // Back wall
            };
        }

        private ObjectScene setupSphereScene()
        {
            return new ObjectScene(GetDefaultSpheres(), GetDefaultWalls());
        }

        private void setupSphereCamera()
        {
            cameraPos = new Vector3(0, 0, 2.5);
            lightPos = new Vector3(0, 4.7, -8);
            cameraDirection = new Vector3(0, 0, -1);
        }

        private ObjectScene setupChessScene()
        {
            return new ObjectScene(null, GetChessWalls(), LoadChessPieces());
        }

        private void setupChessCamera()
        {

            cameraPos = new Vector3(0, 2, 8);
            lightPos = new Vector3(0, 6, 0);
            cameraDirection = new Vector3(0, 0, -1);

        }

        private ObjectScene setupKnightScene()
        {
            return new ObjectScene(new[] { new Sphere(new Vector3(0, 2.5, -3), 1.5, Color.White, 0.5) }, GetChessWalls(), LoadChessPieces(0),
                cubes: new[] {new Cube(new Vector3(0, 0.5, -3), 1, Color.White, 0)});
        }

        private void setupKnightCamera()
        {
            cameraPos = new Vector3(2.5, 2, 3.5);
            lightPos = new Vector3(2, 4, 0);
            cameraDirection = new Vector3(-1.5, 0, -2).Normalize();
        }
    }
}

/*
        private void setupKnightCamera()
        {
            cameraPos = new Vector3(2.5, 2, 3.5);
            lightPos = new Vector3(2, 4, 0);
            cameraDirection = new Vector3(-1.5, 0, -2).Normalize();
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
