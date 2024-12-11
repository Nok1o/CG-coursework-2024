using Xunit;
using RayTracer;

namespace RayTracer.Tests
{
    public class RayTracerTests
    {
        [Fact]
        public void ExampleTest_ShouldPass()
        {
            RayTracer.Vector3 v = new RayTracer.Vector3(1, 2, 3);
            // Arrange
            int expected = 5;

            // Act
            int actual = 2 + 3;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}