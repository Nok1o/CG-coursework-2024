using System;
using System.Drawing;
using System.Windows.Forms;
using RayTracer.Objects;

namespace RayTracer
{
    public class Settings
    {
        public static Sphere[] GetDefaultSpheres()
        {
            return new Sphere[]
            {
                new Sphere(new Vector3(-2, 0, -6), 2, Color.Red, 0.5, "Сфера дальше от угла"),
                new Sphere(new Vector3(2, 0, -8), 2, Color.Green, 0.5, "Сфера ближе к углу"),
            };
        }

        public static Wall[] GetDefaultWalls()
        {
            return new Wall[]
            {

                new Wall(new Vector3(-5, 0, 0), new Vector3(1, 0, 0), Color.Red, 0, "Левая стена"),   // Left wall
                new Wall(new Vector3(5, 0, 0), new Vector3(-1, 0, 0), Color.Green, 0, "Правая стена"), // Right wall
                new Wall(new Vector3(0, 0, -10), new Vector3(0, 0, 1), Color.Blue, 0, "Передняя стена"), // Front wall
                new Wall(new Vector3(0, 5.01, 0), new Vector3(0, -1, 0), Color.White, 0, "Потолок"), // Ceiling
                new Wall(new Vector3(0, -2, 0), new Vector3(0, 1, 0), Color.Yellow, 0, "Пол"), // Floor
                new Wall(new Vector3(0, 0, 3), new Vector3(0, 0, -1), Color.SandyBrown, 0, "Задняя стена") // Back wall
            };
        }

        public static Wall[] GetChessWalls()
        {
            return new Wall[]
            {

                new Wall(new Vector3(-6, 0, 0), new Vector3(1, 0, 0), Color.Red, 0, "Левая стена"),   // Left wall
                new Wall(new Vector3(6, 0, 0), new Vector3(-1, 0, 0), Color.Green, 0, "Правая стена"), // Right wall
                new Wall(new Vector3(0, 0, -12), new Vector3(0, 0, 1), Color.Blue, 0, "Передняя стена"), // Front wall
                new Wall(new Vector3(0, 10, 0), new Vector3(0, -1, 0), Color.White, 0, "Потолок"), // Ceiling
                new Wall(new Vector3(0, 0, 0), new Vector3(0, 1, 0), Color.Yellow, 0, "Пол"), // Floor
                new Wall(new Vector3(0, 0, 12), new Vector3(0, 0, -1), Color.SandyBrown, 0, "Задняя стена") // Back wall
            };
        }

        public static ObjectScene setupSphereScene()
        {
            return new ObjectScene(GetDefaultSpheres(), GetDefaultWalls());
        }

        public static ObjectScene setupChessScene()
        {
            var king = ChessLoader.LoadChessPiece("king.obj", new Vector3(0.78, 0, 0), "Король");
            var queen = ChessLoader.LoadChessPiece("queen.obj", new Vector3(-0.78, 0, 0), "Ферзь");
            var bishop1 = ChessLoader.LoadChessPiece("bishop.obj", new Vector3(0.78 * 2 + 0.59, 0, 0), "Слон");
            var bishop2 = ChessLoader.LoadChessPiece("bishop.obj", new Vector3(- (0.78 * 2 + 0.59), 0, 0), "Слон");
            var knight1 = ChessLoader.LoadChessPiece("knight.obj", new Vector3(0.78 * 3 + 0.59 * 2, 0, 0), "Конь");
            var knight2 = ChessLoader.LoadChessPiece("knight.obj", new Vector3(-(0.78 * 3 + 0.59 * 2), 0, 0), "Конь");
            var rook1 = ChessLoader.LoadChessPiece("rook.obj", new Vector3(0.78 * 4 + 0.59 * 3, 0, 0), "Ладья");
            var rook2 = ChessLoader.LoadChessPiece("rook.obj", new Vector3(-(0.78 * 4 + 0.59 * 3), 0, 0), "Ладья");

            var pawn1 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(0.78, 0, 2.5), "Пешка");
            var pawn2 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(-0.78, 0, 2.5), "Пешка");
            var pawn3 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(0.78 * 2 + 0.59, 0, 2.5), "Пешка");
            var pawn4 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(-(0.78 * 2 + 0.59), 0, 2.5), "Пешка");
            var pawn5 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(0.78 * 3 + 0.59 * 2, 0, 2.5), "Пешка");
            var pawn6 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(-(0.78 * 3 + 0.59 * 2), 0, 2.5), "Пешка");
            var pawn7 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(0.78 * 4 + 0.59 * 3, 0, 2.5), "Пешка");
            var pawn8 = ChessLoader.LoadChessPiece("pawn.obj", new Vector3(-(0.78 * 4 + 0.59 * 3), 0, 2.5), "Пешка");

            pawn2.SurfaceColor = Color.Aqua;
            pawn4.SurfaceColor = Color.Brown;
            rook1.SurfaceColor = Color.Cyan;
            bishop1.SurfaceColor = Color.DarkGreen;
            knight2.SurfaceColor = Color.DarkOrange;
            queen.SurfaceColor = Color.DarkRed;
            king.SurfaceColor = Color.DarkViolet;

            return new ObjectScene(null, GetChessWalls(), new [] { 
                king, queen, bishop1, bishop2, knight1, knight2, rook1, rook2,
                pawn1, pawn2, pawn3, pawn4, pawn5, pawn6, pawn7, pawn8    
            });
        }

        public static ObjectScene SetupKnightScene()
        {
            var knight = ChessLoader.LoadChessPiece("knight.obj", new Vector3(0, 0, 0.22), "Конь");
            knight.SurfaceColor = Color.FromArgb(0, 128, 128);

            return new ObjectScene(new[] { new Sphere(new Vector3(0, 2.5, -3), 1.5, Color.White, 0.5, "Сфера") },
                GetChessWalls(), new[] { knight },
                new[] {new Cube(new Vector3(0, 0.5, -3), 1, Color.White, 0, "Подиум")});
        }
    }
}