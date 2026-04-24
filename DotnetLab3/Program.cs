using System.Diagnostics;
using DotnetLab3;

var a = Matrix.CreateRandom(5, 5);
var b = Matrix.CreateRandom(5, 5);

var res1 = Measure(() => Matrix.Multiply(a, b), "Single-threaded");
var res2 = Measure(() => Matrix.Multiply(a, b, 10), "Multi-threaded");
var res3 = Measure(() => Matrix.MultiplyLowLevel(a, b, 10), "Multi-threaded Low-Level");

Matrix.Print(res1);
Console.WriteLine();
Matrix.Print(res2);
Console.WriteLine();
Matrix.Print(res3);

return;

static T Measure<T>(Func<T> action, string name)
{
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    var sw = Stopwatch.StartNew();
    var result = action();
    sw.Stop();
    Console.WriteLine($"{name}: {sw.Elapsed.TotalMilliseconds} ms ({sw.Elapsed})");

    return result;
}