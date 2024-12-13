using System.Numerics;
using Point = (int x, int y);

namespace AocHelpers;

public static class Data
{
    public static IEnumerable<string> SplitLines(this string input, bool andTrim = true)
    {
        return (andTrim ? input.SplitAndTrim("\n") : input.Split("\n")).NonEmpty();
    }

    public static IEnumerable<string> SplitSections(this string input, bool andTrim = true)
    {
        return (andTrim ? input.SplitAndTrim("\n\n") : input.Split("\n\n")).NonEmpty();
    }

    public static IEnumerable<string> SplitCommas(this string input, bool andTrim = true)
    {
        return (andTrim ? input.SplitAndTrim(",") : input.Split(",")).NonEmpty();
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

    public static IEnumerable<T> NonNull<T>(this IEnumerable<T?> input)
    {
        return input.Where(c => c != null).Cast<T>().ToList();
    }

    public static IEnumerable<int> Range(int start, int end)
    {
        for (var i = start; i <= end; i++)
        {
            yield return i;
        }
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
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
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
        var enumerable = source.ToList();
        if (enumerable.Count == 0)
            return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

        var element = enumerable.Take(1);

        var haveNots = enumerable.Skip(1).AllSubSets().ToList();
        var haves = haveNots.Select(set => element.Concat(set));

        return haves.Concat(haveNots);
    }

    public static IEnumerable<(T, T)> AllPairs<T>(this IEnumerable<T> source)
    {
        if (source.Any())
        {
            var (head, tail) = source.HeadAndTail();
            while (tail.Any())
            {
                foreach (var other in tail)
                {
                    yield return (head, other);
                }

                (head, tail) = tail.HeadAndTail();
            }
        }
    }

    public static void Times(this int times, Action a)
    {
        for (var i = 0; i < times; i++)
        {
            a();
        }
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

    public static Dictionary<Point,char> ToDictionary(this string input)
    {
        var lines = input.SplitLines();
        var width = lines.First().Length;
        var height = lines.Count();
        var output = new Dictionary<Point,char>();
        foreach (var (line, y) in lines.WithIndex())
        {
            foreach (var (c, x) in line.WithIndex())
            {
                output[(x, y)] = c;
            }
        }

        return output;
    }

    public static int[,] To2dIntArray(this string input)
    {
        var lines = input.SplitLines();
        var width = lines.First().Length;
        var height = lines.Count();
        var output = new int[width, height];
        foreach (var (line, y) in lines.WithIndex())
        {
            foreach (var (c, x) in line.WithIndex())
            {
                output[x, y] = int.Parse(c.ToString());
            }
        }

        return output;
    }

    public static bool[,] To2dBoolArray(this string input, char falseChar, char trueChar)
    {
        var lines = input.SplitLines();
        var width = lines.First().Length;
        var height = lines.Count();
        var output = new bool[width, height];
        foreach (var (line, y) in lines.WithIndex())
        {
            foreach (var (c, x) in line.WithIndex())
            {
                if (c == falseChar)
                {
                    output[x, y] = false;
                }
                else if (c == trueChar)
                {
                    output[x, y] = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        return output;
    }

    public static Point Find<T>(this T[,] array, T target)
    {
        var output = (-1, -1);
        array.Traverse((x, y, v) =>
        {
            if (v?.Equals(target) ?? false)
            {
                output = (x, y);
            }
        });
        return output;
    }

    public static IEnumerable<Point> FindAll<T>(this T[,] array, T target)
    {
        for (var y = 0; y < array.GetLength(1); y++)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                if (array[x, y]?.Equals(target) ?? false)
                {
                    yield return (x, y);
                }
            }
        }
    }

    public static bool ContainsCoordinate<T>(this T[,] array, int x, int y) where T : struct
    {
        return x.Between(0, array.GetLength(0) - 1) && y.Between(0, array.GetLength(1) - 1);
    }

    public static bool ContainsCoordinate<T>(this T[,] array, Point point) where T : struct
    {
        return array.ContainsCoordinate(point.x, point.y);
    }

    public static T? ValueOrNull<T>(this T[,] array, int x, int y) where T : struct
    {
        if (!x.Between(0, array.GetLength(0) - 1) || !y.Between(0, array.GetLength(1) - 1))
        {
            return null;
        }

        return array[x, y];
    }

    public static T? ValueOrNull<T>(this T[,] array, Point point) where T : struct
    {
        return array.ValueOrNull(point.x, point.y);
    }

    public static int Count<T>(this T[,] array, Func<int, int, T, bool> condition)
    {
        var count = 0;
        array.Traverse((x, y, c) =>
        {
            if (condition(x, y, c))
            {
                count++;
            }
        });
        return count;
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

    public static (T Head, IEnumerable<T> Tail) HeadAndTail<T>(this IEnumerable<T> source)
    {
        return (source.First(), source.Skip(1));
    }

    public static (T Head, T Last) FirstAndLast<T>(this IEnumerable<T> source)
    {
        return (source.First(), source.Last());
    }

    public static TProduct Product<TSource, TProduct>(this IEnumerable<TSource> input, Func<TSource, TProduct> selector) where TProduct : IMultiplyOperators<TProduct, TProduct, TProduct>
    {
        return input.Select(selector).Product();
    }

    public static TProduct Product<TProduct>(this IEnumerable<TProduct> input) where TProduct : IMultiplyOperators<TProduct, TProduct, TProduct>
    {
        return input.Aggregate((agg, next) => agg * next);
    }

    public static long Lcm(long num1, long num2)
    {
        var x = num1;
        var y = num2;
        while (num1 != num2)
        {
            if (num1 > num2)
            {
                num1 -= num2;
            }
            else
            {
                num2 -= num1;
            }
        }

        return (x * y) / num1;
    }

    public static T[] CopyWithReplacement<T>(this T[] source, int replacementIndex, T replacementValue)
    {
        var result = new T[source.Length];
        for (int i = 0; i < source.Length; i++)
        {
            if (i == replacementIndex)
            {
                result[i] = replacementValue;
            }
            else
            {
                result[i] = source[i];
            }
        }

        return result;
    }

    public static bool Between(this decimal source, decimal min, decimal max, bool exclusive = false)
    {
        if (exclusive)
        {
            return source > min && source < max;
        }

        return source >= min && source <= max;
    }

    public static bool Between(this int source, int min, int max, bool inclusive = true)
    {
        if (inclusive)
        {
            return source >= min && source <= max;
        }

        return source > min && source < max;
    }

    public static int[] Differences(this int[] input)
    {
        var output = new int[input.Length - 1];
        for (var i = 0; i < input.Length - 1; i++)
        {
            output[i] = input[i + 1] - input[i];
        }

        return output;
    }
}