using System.Diagnostics;
using DotnetLab3;

var a = Matrix.CreateRandom(1000, 1000);
var b = Matrix.CreateRandom(1000, 1000);

PrintTable(new List<BenchmarkRow>
{
    new(1, "Single-threaded", Measure(() => Matrix.Multiply(a, b)))
});

var multiThreadedResults = new List<BenchmarkRow>();
var lowLevelResults = new List<BenchmarkRow>();

for(var i = 2; i < 15; i++)
{
    var threads = i;

    multiThreadedResults.Add(new BenchmarkRow(threads, "Multi-threaded", Measure(() => Matrix.Multiply(a, b, threads))));
    lowLevelResults.Add(new BenchmarkRow(threads, "Multi-threaded Low-Level", Measure(() => Matrix.MultiplyLowLevel(a, b, threads))));
}

PrintTable(multiThreadedResults);
PrintTable(lowLevelResults);

return;

static TimeSpan Measure<T>(Func<T> action)
{
    const int measurementCount = 5;

    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    long totalTicks = 0;

    for (var i = 0; i < measurementCount; i++)
    {
        var sw = Stopwatch.StartNew();
        _ = action();
        sw.Stop();

        totalTicks += sw.ElapsedTicks;
    }

    return TimeSpan.FromTicks(totalTicks / measurementCount);
}

static void PrintTable(IReadOnlyList<BenchmarkRow> results)
{
    const string threadsHeader = "Threads";
    const string benchmarkHeader = "Benchmark";
    const string millisecondsHeader = "Milliseconds";
    const string elapsedHeader = "Elapsed";

    var threadsWidth = Math.Max(threadsHeader.Length, results.Max(r => r.Threads.ToString().Length));
    var benchmarkWidth = Math.Max(benchmarkHeader.Length, results.Max(r => r.Name.Length));
    var millisecondsWidth = Math.Max(millisecondsHeader.Length, results.Max(r => r.Elapsed.TotalMilliseconds.ToString("0.000").Length));
    var elapsedWidth = Math.Max(elapsedHeader.Length, results.Max(r => r.Elapsed.ToString().Length));

    Console.WriteLine(Separator(threadsWidth, benchmarkWidth, millisecondsWidth, elapsedWidth));
    Console.WriteLine($"| {threadsHeader.PadRight(threadsWidth)} | {benchmarkHeader.PadRight(benchmarkWidth)} | {millisecondsHeader.PadLeft(millisecondsWidth)} | {elapsedHeader.PadRight(elapsedWidth)} |");
    Console.WriteLine(Separator(threadsWidth, benchmarkWidth, millisecondsWidth, elapsedWidth));

    foreach (var row in results)
    {
        Console.WriteLine($"| {row.Threads.ToString().PadRight(threadsWidth)} | {row.Name.PadRight(benchmarkWidth)} | {row.Elapsed.TotalMilliseconds.ToString("0.000").PadLeft(millisecondsWidth)} | {row.Elapsed.ToString().PadRight(elapsedWidth)} |");
    }

    Console.WriteLine(Separator(threadsWidth, benchmarkWidth, millisecondsWidth, elapsedWidth));
    return;

    static string Separator(int threadsWidth, int benchmarkWidth, int millisecondsWidth, int elapsedWidth)
        => $"+-{new string('-', threadsWidth)}-+-{new string('-', benchmarkWidth)}-+-{new string('-', millisecondsWidth)}-+-{new string('-', elapsedWidth)}-+";
}

internal readonly record struct BenchmarkRow(int Threads, string Name, TimeSpan Elapsed);
