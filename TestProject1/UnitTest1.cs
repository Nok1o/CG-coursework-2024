using RayTracer;
using RayTracer.Objects;
using System.Drawing;
using Xunit;

namespace TestProject1
{
    public class Vector3Tests
    {
        [Fact]
        public void Constructor_SetsValuesCorrectly()
        {
            var v = new RayTracer.Vector3(1.0, 2.0, 3.0);
            Assert.Equal(1.0, v.X);
            Assert.Equal(2.0, v.Y);
            Assert.Equal(3.0, v.Z);
        }

        [Fact]
        public void AdditionOperator_WorksCorrectly()
        {
            var v1 = new Vector3(1, 2, 3);
            var v2 = new Vector3(4, 5, 6);
            var result = v1 + v2;

            Assert.True(new Vector3(5, 7, 9) == result);
        }

        [Fact]
        public void SubtractionOperator_WorksCorrectly()
        {
            var v1 = new Vector3(4, 5, 6);
            var v2 = new Vector3(1, 2, 3);
            var result = v1 - v2;

            Assert.True(new Vector3(3, 3, 3) == result);
        }

        [Fact]
        public void UnaryNegationOperator_WorksCorrectly()
        {
            var v = new Vector3(1, -2, 3);
            var result = -v;

            Assert.True(new Vector3(-1, 2, -3) == result);
        }

        [Fact]
        public void ScalarMultiplicationOperator_WorksCorrectly()
        {
            var v = new Vector3(1, 2, 3);
            var result = v * 2;

            Assert.True(new Vector3(2, 4, 6) == result);
        }

        [Fact]
        public void ScalarDivisionOperator_WorksCorrectly()
        {
            var v = new Vector3(2, 4, 6);
            var result = v / 2;

            Assert.True(new Vector3(1, 2, 3) == result);
        }

        [Fact]
        public void Indexer_WorksCorrectly()
        {
            var v = new Vector3(1, 2, 3);

            Assert.Equal(1, v[0]);
            Assert.Equal(2, v[1]);
            Assert.Equal(3, v[2]);

            Assert.Throws<ArgumentException>(() => v[3]);
        }

        [Fact]
        public void DotProduct_WorksCorrectly()
        {
            var v1 = new Vector3(1, 2, 3);
            var v2 = new Vector3(4, 5, 6);

            var result = v1.Dot(v2);
            Assert.Equal(32, result);
        }

        [Fact]
        public void CrossProduct_WorksCorrectly()
        {
            var v1 = new Vector3(1, 0, 0);
            var v2 = new Vector3(0, 1, 0);

            var result = v1.Cross(v2);
            Assert.True(new Vector3(0, 0, 1) == result);
        }

        [Fact]
        public void Normalize_WorksCorrectly()
        {
            var v = new Vector3(3, 0, 0);
            var result = v.Normalize();

            Assert.True(new Vector3(1, 0, 0) == result);
        }

        [Fact]
        public void Length_WorksCorrectly()
        {
            var v = new Vector3(3, 4, 0);
            var result = v.Length();

            Assert.Equal(5, result);
        }

        [Fact]
        public void Reflect_WorksCorrectly()
        {
            var v = new Vector3(1, -1, 0);
            var normal = new Vector3(0, 1, 0);

            var result = v.Reflect(normal);
            Assert.True(new Vector3(1, 1, 0) == result);
        }
    }

    public class IntersectRayTests
    {
        [Fact]
        public void SphereIntersectRay_HitsSphere()
        {
            var sphere = new Sphere(new Vector3(0, 0, -5), 1, System.Drawing.Color.Red, 0);
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 0, -1));

            bool hit = sphere.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.True(hit);
            Assert.Equal(4, distance, 1e-4);
            Assert.True(new Vector3(0, 0, 1) == normal);
        }

        [Fact]
        public void SphereIntersectRay_MissesSphere()
        {
            var sphere = new Sphere(new Vector3(0, 0, -5), 1, System.Drawing.Color.Red, 0);
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

            bool hit = sphere.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.False(hit);
        }

        [Fact]
        public void WallIntersectRay_HitsWall()
        {
            var wall = new Wall(new Vector3(0, 0, -5), new Vector3(0, 0, 1), System.Drawing.Color.Gray, 0);
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 0, -1));

            bool hit = wall.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.True(hit);
            Assert.Equal(5, distance);
            Assert.True(new Vector3(0, 0, 1) == normal);
        }

        [Fact]
        public void WallIntersectRay_MissesWall()
        {
            var wall = new Wall(new Vector3(0, 0, -5), new Vector3(0, 0, 1), System.Drawing.Color.Gray, 0);
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

            bool hit = wall.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.False(hit);
        }

        [Fact]
        public void CubeIntersectRay_HitsCube()
        {
            var cube = new Cube(new Vector3(0, 0, -5), 2, System.Drawing.Color.Blue, 0);
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 0, -1));

            bool hit = cube.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.True(hit);
            Assert.Equal(4, distance);
        }

        [Fact]
        public void CubeIntersectRay_MissesCube()
        {
            var cube = new Cube(new Vector3(0, 0, -5), 2, System.Drawing.Color.Blue, 0);
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

            bool hit = cube.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.False(hit);
        }

        [Fact]
        public void ChessPieceIntersectRay_HitsTriangle()
        {
            var triangle1 = new ChessPiece.Triangle(
                new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
                new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1)
            );
            var chessPiece = new ChessPiece(
                new Vector3(0, 0, 0), System.Drawing.Color.White, new ChessPiece.Triangle[] { triangle1 }, 0
            );
            var ray = new Ray(new Vector3(0.5, 0.5, -1), new Vector3(0, 0, 1));

            bool hit = chessPiece.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.True(hit);
            Assert.Equal(1, distance, 1e-4);
            Assert.True(new Vector3(0, 0, 1) == normal);
        }

        [Fact]
        public void ChessPieceIntersectRay_MissesTriangle()
        {
            var triangle1 = new ChessPiece.Triangle(
                new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
                new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1)
            );
            var chessPiece = new ChessPiece(
                new Vector3(0, 0, 0), System.Drawing.Color.White, new ChessPiece.Triangle[] { triangle1 }, 0
            );
            var ray = new Ray(new Vector3(2, 2, -1), new Vector3(0, 0, 1));

            bool hit = chessPiece.IntersectRay(ray, out double distance, out Vector3 normal);

            Assert.False(hit);
        }
    }

    public class ColorCalculationTests
    {
        private ColorCalculation colorCalc = new ColorCalculation();

        [Fact]
        public void TestCalculateAmbientLighting()
        {
            Color objectColor = Color.FromArgb(100, 100, 100); // Grey color
            double ambientIntensity = 0.5;

            Color result = colorCalc.ApplyAmbientLight(objectColor, ambientIntensity);

            Assert.Equal(Color.FromArgb(50, 50, 50), result); // 50% of the original color
        }

        [Fact]
        public void TestCalculatePhongSpecular()
        {
            Vector3 cameraOrigin = new Vector3(0, 0, 0);
            Vector3 hitPoint = new Vector3(1, 1, 1);
            Vector3 normal = new Vector3(0, 0, 1);
            Vector3 lightPos = new Vector3(1, 2, 3);
            Color objectColor = Color.Red;
            double intensity = 1.0;

            Color result = colorCalc.CalculatePhongSpecular(cameraOrigin, hitPoint, normal, lightPos, intensity);

            // Check if the result is a lightened color (due to the reflection)
            Assert.InRange(result.R, 0, 255);
            Assert.InRange(result.G, 0, 255);
            Assert.InRange(result.B, 0, 255);
        }

        [Fact]
        public void TestCalculateFresnelSpecular()
        {
            Vector3 cameraOrigin = new Vector3(0, 0, 0);
            Vector3 hitPoint = new Vector3(1, 1, 1);
            Vector3 normal = new Vector3(0, 0, 1);
            Vector3 lightPos = new Vector3(1, 2, 3);
            Color objectColor = Color.Red;
            double intensity = 0.8;

            Color result = colorCalc.CalculateFresnelSpecular(cameraOrigin, hitPoint, normal, lightPos, intensity);

            // Check if the result reflects Fresnel reflection (non-zero specular)
            Assert.InRange(result.R, 0, 255);
            Assert.InRange(result.G, 0, 255);
            Assert.InRange(result.B, 0, 255);
        }

        [Fact]
        public void TestShadowCalculation()
        {
            Vector3 hitPoint = new Vector3(0, 0, 0);
            Vector3 lightPos = new Vector3(10, 10, 10);
            Vector3 lightDir = (lightPos - hitPoint).Normalize();

            // Create a mock scene with an object that should block the light
            var scene = new ObjectScene(new Sphere[] { new Sphere(new Vector3(5, 5, 5), 1, Color.Red, 0) });
            bool result = colorCalc.IsInShadow(hitPoint, lightDir, lightPos, scene);

            Assert.True(result); // The light should be blocked by the sphere in the scene
        }

        [Fact]
        public void TestCombineLighting()
        {
            Color objectColor = Color.Red;
            Color ambientLight = Color.FromArgb(50, 50, 50);
            double diffuseIntensity = 0.5;
            Color specularLight = Color.FromArgb(100, 100, 100);

            Color result = colorCalc.CombineLighting(objectColor, ambientLight, diffuseIntensity, specularLight);

            // Test combined lighting result, ensuring it is within valid color range
            Assert.InRange(result.R, 0, 255);
            Assert.InRange(result.G, 0, 255);
            Assert.InRange(result.B, 0, 255);
        }

        [Fact]
        public void TestMixColors()
        {
            Color color1 = Color.Red;
            Color color2 = Color.Blue;
            double factor = 0.5;

            Color result = ColorCalculation.MixColors(color1, color2, factor);

            // Check if the result is a mixture of the two colors
            Assert.Equal(Color.FromArgb(127, 0, 127), result); // Purple (50% red and 50% blue)
        }

    }
}