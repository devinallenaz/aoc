namespace AocHelpers;

public static class Data
{
    public static IEnumerable<string> SplitLines(this string input, bool andTrim = true)
    {
        return (andTrim ? input.SplitAndTrim("\n") : input.Split("\n")).NonEmpty();
    }

    public static IEnumerable<string> SplitSections(this string input, bool andTrim = true)
    {
        return (andTrim ? input.SplitAndTrim("\n\n") : input.Split("\n")).NonEmpty();
    }

    public static IEnumerable<string> SplitCommas(this string input, bool andTrim = true)
    {
        return (andTrim ? input.SplitAndTrim(",") : input.Split("\n")).NonEmpty();
    }

    public static IEnumerable<string> SplitAndTrim(this string input)
    {
        return input.Split().Select(s => s.Trim()).NonEmpty();
    }

    public static IEnumerable<string> SplitAndTrim(this string input, string separator)
    {
        return input.Split(separator).Select(s => s.Trim()).NonEmpty();
    }

    public static IEnumerable<string> NonEmpty(this IEnumerable<string> input)
    {
        return input.Where(s => !string.IsNullOrWhiteSpace(s));
    }

    public static IEnumerable<char> NonNull(this IEnumerable<char?> input)
    {
        return input.Where(c => c != null).Cast<char>().ToList();
    }

    public static IEnumerable<int> Range(int start, int end)
    {
        var range = new List<int>();
        for (var i = start; i <= end; i++)
        {
            range.Add(i);
        }

        return range;
    }

    public static IEnumerable<IEnumerable<T>> Slices<T>(this IEnumerable<T> input, int size)
    {
        var i = 0;
        var slice = new List<T>();
        foreach (var t in input)
        {
            slice.Add(t);
            if (++i % size == 0)
            {
                yield return slice.ToArray();
                slice.Clear();
            }
        }

        if (slice.Any())
        {
            yield return slice;
        }
    }

    public static int ToNumericInt(this char input)
    {
        return (int)char.GetNumericValue(input);
    }

    /// <summary>
    /// Return null if input == test
    /// </summary>
    /// <param name="input"></param>
    /// <param name="test"></param>
    /// <returns></returns>
    public static string? NullIf(this string? input, string test)
    {
        return input == test ? null : input;
    }

    /// <summary>
    /// Return null if input == test
    /// </summary>
    /// <param name="input"></param>
    /// <param name="test"></param>
    /// <returns></returns>
    public static char? NullIf(this char? input, char test)
    {
        return input == test ? null : input;
    }

    /// <summary>
    /// Return null if input == test
    /// </summary>
    /// <param name="input"></param>
    /// <param name="test"></param>
    /// <returns></returns>
    public static char? NullIf(this char input, char test)
    {
        return input == test ? null : input;
    }


    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }

    /// <summary>
    /// Filter points within a 2D array
    /// </summary>
    /// <param name="input">a 2d array of T</param>
    /// <param name="test">Function to test a given point (T value, int x, int y) => bool</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> Where<T>(this T[,] input, Func<T, int, int, bool> test)
    {
        var width = input.GetLength(0);
        var height = input.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (test(input[x, y], x, y))
                {
                    yield return input[x, y];
                }
            }
        }
    }

    /// <summary>
    /// Find the maximum point in a 2D array according to the given function to calculate an integer value for each point
    /// </summary>
    /// <param name="input">a 2D array</param>
    /// <param name="clause">A function to calculate an integer value for each point on the array (T value, int x, int y) => int</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Max<T>(this T[,] input, Func<T, int, int, int> clause)
    {
        if (input.Length == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        var max = int.MinValue;
        var width = input.GetLength(0);
        var height = input.GetLength(1);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                max = Math.Max(max, clause(input[x, y], x, y));
            }
        }

        return max;
    }

    /// <summary>
    /// Find the minimum point in a 2D array according to the given function to calculate an integer value for each point
    /// </summary>
    /// <param name="input">a 2D array</param>
    /// <param name="clause">A function to calculate an integer value for each point on the array (T value, int x, int y) => int</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Min<T>(this T[,] input, Func<T, int, int, int> clause)
    {
        if (input.Length == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        var min = int.MaxValue;
        var width = input.GetLength(0);
        var height = input.GetLength(1);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                min = Math.Min(min, clause(input[x, y], x, y));
            }
        }

        return min;
    }

    public static IEnumerable<IEnumerable<T>> AllSubSets<T>(this IEnumerable<T> source)
    {
        if (!source.Any())
            return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

        var element = source.Take(1);

        var haveNots = source.Skip(1).AllSubSets();
        var haves = haveNots.Select(set => element.Concat(set));

        return haves.Concat(haveNots);
    }

    public static char[,] To2dCharArray(this string input)
    {
        var lines = input.SplitLines();
        var width = lines.First().Length;
        var height = lines.Count();
        var output = new char[width, height];
        foreach (var (line, y) in lines.WithIndex())
        {
            foreach (var (c, x) in line.WithIndex())
            {
                output[x, y] = c;
            }
        }

        return output;
    }

    public static T? ValueOrNull<T>(this T[,] array, int x, int y) where T : struct
    {
        if (x < 0 || x >= array.GetLength(0) || y < 0 || y >= array.GetLength(1))
        {
            return null;
        }

        return array[x, y];
    }

    public static bool IsDigit(this char c)
    {
        return char.IsDigit(c);
    }
    
    public static bool IsDigit(this char? c)
    {
        if (c == null)
        {
            return false;
        }
        return char.IsDigit(c.Value);
    }

    public static void Traverse<T>(this T[,] array, Action<int, int, T> action)
    {

        for (var y = 0; y < array.GetLength(1); y++)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                action(x, y, array[x, y]);
            }
        }
    }

    public static (T, IEnumerable<T>) HeadAndTail<T>(this IEnumerable<T> source)
    {
        return (source.First(), source.Skip(1));
    }
}