using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day18 : Solver
{
    public override int Day => 18;

    //Problem 1
    public override object ExpectedOutput1 => 62;

    private int CalculateDugArea(List<DugPoint> points)
    {
        var minX = points.Min(p => p.X);
        var maxX = points.Max(p => p.X);
        var minY = points.Min(p => p.Y);
        var maxY = points.Max(p => p.Y);
        var area = 0;
        for (int x = minX; x <= maxX; x++)
        {
            var inside = false;
            var previousCorner = (CornerType?)null;
            for (int y = minY; y <= maxY; y++)
            {
                var point = points.FirstOrDefault(p => p.X == x && p.Y == y);
                if (point != null)
                {
                    switch (previousCorner, point.CornerType)
                    {
                        case (null, CornerType.None):
                            inside = !inside;
                            break;
                        case (_,CornerType.None):
                            break;
                        case (null, CornerType.DL):
                        case (null, CornerType.DR):
                            inside = !inside;
                            previousCorner = point.CornerType;
                            break;
                        case (CornerType.DL, CornerType.UL):
                            inside = !inside;
                            previousCorner = null;
                            break;
                        case (CornerType.DL, CornerType.UR):
                            previousCorner = null;
                            break;
                        case (CornerType.DR, CornerType.UR):
                            inside = !inside;
                            previousCorner = null;
                            break;
                        case (CornerType.DR, CornerType.UL):
                            previousCorner = null;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                if (inside || point != null)
                {
                    area++;
                }
            }
        }

        return area;
    }


    public override object Solve1(string input)
    {
        var digger = new Digger(input);
        var dugPoints = digger.Dig();
        return CalculateDugArea(dugPoints);
    }

    //Problem 2
    public override object ExpectedOutput2 => base.ExpectedOutput2;

    public override object Solve2(string input)
    {
        return base.Solve2(input);
    }
}