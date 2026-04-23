namespace DotnetLab3;

public abstract class Matrix
{
    public static int[,] CreateRandom(int rows, int cols)
    {
        if (rows <= 0) throw new ArgumentException("rows must be > 0", nameof(rows));
        if (cols <= 0) throw new ArgumentException("cols must be > 0", nameof(cols));

        var rng = new Random();
        var result = new int[rows, cols];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                result[i, j] = rng.Next();
            }
        }

        return result;
    }
    
    public static int[,] Multiply(int[,] a, int[,] b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var aRows = a.GetLength(0);
        var aCols = a.GetLength(1);
        var bRows = b.GetLength(0);
        var bCols = b.GetLength(1);

        if (aCols != bRows) throw new ArgumentException("Number of columns of A must equal number of rows of B.");

        var result = new int[aRows, bCols];

        for (var i = 0; i < aRows; i++)
        {
            for (var j = 0; j < bCols; j++)
            {
                var sum = 0;
                for (var k = 0; k < aCols; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        }

        return result;
    }
    
    public static int[,] Multiply(int[,] a, int[,] b, int maxThreads)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        if (maxThreads <= 0) throw new ArgumentException("maxThreads must be > 0", nameof(maxThreads));

        var aRows = a.GetLength(0);
        var aCols = a.GetLength(1);
        var bRows = b.GetLength(0);
        var bCols = b.GetLength(1);

        if (aCols != bRows) throw new ArgumentException("Number of columns of A must equal number of rows of B.");

        var result = new int[aRows, bCols];

        var po = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };

        Parallel.For(0, aRows, po, i =>
        {
            for (var j = 0; j < bCols; j++)
            {
                var sum = 0;
                for (var k = 0; k < aCols; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        });

        return result;
    }
}