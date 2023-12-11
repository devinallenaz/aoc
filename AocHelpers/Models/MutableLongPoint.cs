namespace AocHelpers.Models;

public class MutableLongPoint
{
    public MutableLongPoint() : this(0l, 0l)
    {
    }

    public MutableLongPoint(long x, long y)
    {
        X = x;
        Y = y;
    }

    public long X { get; set; }
    public long Y { get; set; }
    
    public static implicit operator ValueTuple<long, long>(MutableLongPoint p) => (p.X, p.Y);
}