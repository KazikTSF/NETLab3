using System.Diagnostics;
using DotnetLab3;

var a = Matrix.CreateRandom(2000, 2000);
var b = Matrix.CreateRandom(2000, 2000);

Measure(() => Matrix.Multiply(a, b), "Single-threaded");
Measure(() => Matrix.Multiply(a, b, 10), "Multi-threaded");
Measure(() => Matrix.MultiplyLowLevel(a, b, 10), "Multi-threaded Low-Level");
return;

static void Measure(Action action, string name)
{
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    var sw = Stopwatch.StartNew();
    action();
    sw.Stop();
    Console.WriteLine($"{name}: {sw.Elapsed.TotalMilliseconds} ms ({sw.Elapsed})");
}