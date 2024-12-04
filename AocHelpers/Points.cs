namespace AocHelpers;
using Point = (int x, int y);
using LongPoint = (long x, long y);
using Point3d = (int x, int y, int z);
public static class Points
{
    public static readonly Point North = (0, -1);
    public static readonly Point South = (0, 1);
    public static readonly Point East = (1, 0);
    public static readonly Point West = (-1, 0);

    public static readonly Point NorthWest = North.Plus(West);
    public static readonly Point NorthEast = North.Plus(East);
    public static readonly Point SouthWest = South.Plus(West);
    public static readonly Point SouthEast = South.Plus(East);
    public static readonly Point[] CardinalDirections = [North, South, East, West];
    public static readonly Point[] DiagonalDirections = [NorthWest, NorthEast, SouthWest, SouthEast];
    public static readonly Point[] AllDirections = [..CardinalDirections, ..DiagonalDirections];
    public static Point Up => North;
    public static Point Down => South;
    public static Point Left => West;
    public static Point Right => East;

    public static Point Plus(this Point a, Point b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    public static Point Minus(this Point a, Point b)
    {
        return (a.x - b.x, a.y - b.y);
    }

    public static Point3d Plus(this Point3d a, Point3d b)
    {
        return (a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Point3d Minus(this Point3d a, Point3d b)
    {
        return (a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static Point3d Minus(this Point3d a, int bx, int by, int bz)
    {
        return (a.x - bx, a.y - by, a.z - bz);
    }
    public static Point3d Clamp(this Point3d point, int min, int max)
    {
        return (Math.Clamp(point.x, min, max), Math.Clamp(point.y, min, max), Math.Clamp(point.z, min, max));
    }

    public static (int x, long y) Plus(this (int x, long y) a, (int x, long y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    public static (int x, long y) Minus(this (int x, long y) a, (int x, long y) b)
    {
        return (a.x - b.x, a.y - b.y);
    }

    public static LongPoint Plus(this LongPoint a, LongPoint b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    public static int TaxiDistance(this Point point1, Point point2)
    {
        return Math.Abs(point2.x - point1.x) + Math.Abs(point2.y - point1.y);
    }

    public static long TaxiDistance(this LongPoint point1, LongPoint point2)
    {
        return Math.Abs(point2.x - point1.x) + Math.Abs(point2.y - point1.y);
    }

    public static IEnumerable<Point> ManhattanCircle(Point center, int distance)
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

    public static IEnumerable<Point> AdjacentPoints(this Point point)
    {
        var (x, y) = point;
        return new List<Point>()
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

    public static IEnumerable<Point> AdjacentPointsCardinal(this Point point)
    {
        var (x, y) = point;
        return new List<Point>()
        {
            (x - 1, y),
            (x, y - 1),
            (x, y + 1),
            (x + 1, y),
        };
    }

    public static IEnumerable<LongPoint> AdjacentPointsCardinal(this LongPoint point)
    {
        var (x, y) = point;
        return new List<LongPoint>()
        {
            (x - 1, y),
            (x, y - 1),
            (x, y + 1),
            (x + 1, y),
        };
    }
}