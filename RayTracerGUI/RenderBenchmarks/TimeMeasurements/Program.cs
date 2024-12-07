using System;
using System.Drawing;

namespace Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int width = 800;  // Set render width
            int height = 600; // Set render height
            int runs = 20;    // Number of runs for averaging
            string outputFilePath = "render_times.csv";

            // Define scene rendering functions
            var renderScenes = new Func<System.Drawing.Bitmap>[] // Simplify collection initialization
            {
                    RenderScene1,
                    RenderScene2,
                    RenderScene3
            };

            // Initialize the benchmarking tool
            RenderBenchmark benchmark = new RenderBenchmark(width, height, renderScenes, outputFilePath);

            // Run the benchmark
            Console.WriteLine("Starting render time measurements...");
            benchmark.MeasureRenderTimes(runs);

            Console.WriteLine("Benchmark complete. Results saved to render_times.csv");
            Console.ReadKey();
        }

        private static Bitmap RenderScene1()
        {
            Bitmap bitmap = new Bitmap(800, 600);
            // Add your rendering logic for Scene 1 here
            return bitmap;
        }

        private static Bitmap RenderScene2()
        {
            Bitmap bitmap = new Bitmap(800, 600);
            // Add your rendering logic for Scene 2 here
            return bitmap;
        }

        private static Bitmap RenderScene3()
        {
            Bitmap bitmap = new Bitmap(800, 600);
            // Add your rendering logic for Scene 3 here
            return bitmap;
        }
    }
}
