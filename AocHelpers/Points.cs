namespace AocHelpers;

public static class Points
{
    public static (int x, int y) North = (0, -1);
    public static (int x, int y) South = (0, 1);
    public static (int x, int y) East = (1, 0);
    public static (int x, int y) West = (-1, 0);
    public static (int x, int y) Up => North;
    public static (int x, int y) Down => South;
    public static (int x, int y) Left => West;
    public static (int x, int y) Right => East;

    public static (int x, int y) Plus(this (int x, int y) a, (int x, int y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    public static (int x, int y) Minus(this (int x, int y) a, (int x, int y) b)
    {
        return (a.x - b.x, a.y - b.y);
    }

    public static (int x, long y) Plus(this (int x, long y) a, (int x, long y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    public static (int x, long y) Minus(this (int x, long y) a, (int x, long y) b)
    {
        return (a.x - b.x, a.y - b.y);
    }

    public static (long x, long y) Plus(this (long x, long y) a, (long x, long y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    public static int TaxiDistance(this (int x, int y) point1, (int x, int y) point2)
    {
        return Math.Abs(point2.x - point1.x) + Math.Abs(point2.y - point1.y);
    }

    public static long TaxiDistance(this (long x, long y) point1, (long x, long y) point2)
    {
        return Math.Abs(point2.x - point1.x) + Math.Abs(point2.y - point1.y);
    }

    public static IEnumerable<(int x, int y)> ManhattanCircle((int x, int y) center, int distance)
    {
        for (int x = -distance; x <= distance; x++)
        {
            var y1 = distance - Math.Abs(x);
            var y2 = -y1;
            yield return center.Plus((x, y1));
            if (y1 != y2)
            {
                yield return center.Plus((x, y2));
            }
        }
    }

    public static IEnumerable<(int x, int y)> AdjacentPoints(this (int x, int y) point)
    {
        var (x, y) = point;
        return new List<(int x, int y)>()
        {
            (x - 1, y - 1),
            (x - 1, y),
            (x - 1, y + 1),
            (x, y - 1),
            (x, y + 1),
            (x + 1, y - 1),
            (x + 1, y),
            (x + 1, y + 1),
        };
    }
}