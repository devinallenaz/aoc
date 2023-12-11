namespace AocHelpers.Models;

public class MutablePoint
{
    public MutablePoint() : this(0, 0)
    {
    }

    public MutablePoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public static implicit operator ValueTuple<int, int>(MutablePoint p) => (p.X, p.Y);
}