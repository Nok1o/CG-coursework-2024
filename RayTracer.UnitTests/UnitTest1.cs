using Xunit;

namespace RayTracer.UnitTests
{
    public class RayTracerTests
    {
        [Fact]
        public void ExampleTest_ShouldPass()
        {
            // Arrange
            int expected = 5;

            // Act
            int actual = 2 + 3;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}