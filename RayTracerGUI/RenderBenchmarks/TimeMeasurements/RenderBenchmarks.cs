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
                writer.WriteLine("Scene,Run,Time (ms)");

                for (int sceneIndex = 0; sceneIndex < 6; sceneIndex++)
                {
                    tracer.setAmountOfParallelWorkers((int) Math.Pow(2, sceneIndex));

                    renderTimes.Clear();
                    //Func<Bitmap> renderScene = renderScenes[sceneIndex];

                    for (int run = 0; run <= 5; run++)
                    {
                        // Measure rendering time
                        stopwatch.Restart();
                        
                        tracer.RenderSceneInterface(width, height); // Call the scene rendering method
                        stopwatch.Stop();

                        double elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
                        if (run >= 2)
                            renderTimes.Add(elapsedMs);

                        // Write to file
                        writer.WriteLine($"{sceneIndex + 1}, {run}, {elapsedMs}");
                        Console.WriteLine($"Scene {sceneIndex + 1}, Run {run}: {elapsedMs} ms");
                    }

                    // Write average time for the scene
                    double averageTime = renderTimes.Average();
                    //writer.WriteLine($"{sceneIndex + 1}, Average, {averageTime}");
                    Console.WriteLine($"Scene {sceneIndex + 1}: Average Time = {averageTime} ms");
                }
            }

            Console.WriteLine($"Render times written to {outputFilePath}");
        }
    }
}
