namespace Aoc2023.Models;

public class Line2d //A line represented by the equation y = Slope*x + Constant
{
    public decimal Slope { get; }
    public decimal Constant { get; }
    public Func<decimal, bool> IsPast {get;}

    public Line2d((decimal x, decimal y) point, (decimal dx, decimal dy) slope)
    {
        this.Slope = slope.dy / slope.dx;
        this.Constant = point.y - (this.Slope * point.x);
        IsPast = (decimal x) => x < point.x == slope.dx > 0;
    }

    public (decimal x, decimal y, bool past)? Intersection(Line2d other)
    {
        if (other.Slope == this.Slope)
        {
            return null;
        }


        var x = (other.Constant-this.Constant) / (this.Slope - other.Slope);
        var y = this.Slope * x + this.Constant;
        var past = this.IsPast(x) || other.IsPast(x);
        return (x, y, past);
    }
}