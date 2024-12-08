using Assimp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using RayTracer;

namespace Benchmarks
{
    public class RenderBenchmark
    {
        private readonly int width;
        private readonly int height;
        private readonly Func<Bitmap>[] renderScenes;
        private readonly string outputFilePath;
        private RayTracer.RayTracer tracer = new RayTracer.RayTracer();

        public RenderBenchmark(int width, int height, Func<Bitmap>[] renderScenes, string outputFilePath)
        {
            this.width = width;
            this.height = height;
            this.renderScenes = renderScenes;
            this.outputFilePath = outputFilePath;
            tracer.selectedSceneChanged(0);
            tracer.selectedCameraChanged(0, false);
        }

        public void MeasureRenderTimes(int runs)
        {
            // Initialize stopwatch
            Stopwatch stopwatch = new Stopwatch();
            var renderTimes = new List<double>();

            // Open output file for writing
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine("Threads,Run,Time (ms)");
                List<int> num_workers = new List<int> { 1, 2, 4, 8, 12, 16, 20, 32};

                foreach (int workers in num_workers)
                {
                    tracer.setAmountOfParallelWorkers(workers);

                    renderTimes.Clear();
                    //Func<Bitmap> renderScene = renderScenes[sceneIndex];

                    for (int run = -2; run <= 60; run++)
                    {
                        // Measure rendering time
                        stopwatch.Restart();
                        
                        tracer.RenderSceneInterface(width, height); // Call the scene rendering method
                        stopwatch.Stop();

                        double elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
                        if (run >= 0)
                        {
                            renderTimes.Add(elapsedMs);
                            Console.WriteLine($"Workers {workers}, Run {run}: {elapsedMs} ms");
                            writer.WriteLine($"{workers},{run},{elapsedMs}");
                        }
                    }

                    // Write average time for the scene
                    renderTimes.Sort();
                    double averageTime = renderTimes.Average();
                    double medianTime = renderTimes.OrderBy(t => t).ElementAt(renderTimes.Count / 2);
                    //writer.WriteLine($"{sceneIndex + 1}, Average, {averageTime}");
                    Console.WriteLine($"Workers {workers}: Average Time = {averageTime} ms, median = {medianTime}ms.");
                }
            }

            Console.WriteLine($"Render times written to {outputFilePath}");
        }
    }
}
