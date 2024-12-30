using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);
using Region = (int perimeter, int sides, int area, char crop, System.Collections.Generic.HashSet<(int x, int y)>);

public class Day12 : Solver
{
    public override int Day => 12;

    //Problem 1
    public override object ExpectedOutput1 => 1930;

    private static Dictionary<Point, char> Unregioned = null!;

    private static List<Region> BuildRegions(string input)
    {
        Unregioned = input.To2dCharDictionary();
        var regions = new List<Region>();
        while (Unregioned.Any())
        {
            var start = Unregioned.First().Key;
            regions.Add(BuildRegion(start));
        }

        return regions;
    }

    private static Region BuildRegion(Point start)
    {
        var crop = Unregioned[start];
        Unregioned.Remove(start);
        var regionPoints = new HashSet<Point> { start };
        var perimeter = 0;
        var sides = 0;
        var area = 0;
        HashSet<Point> pointsToCheck = [start];
        var perimetersToCheck = new HashSet<(Point current, Point adjacent)>();

        while (pointsToCheck.Any())
        {
            var current = pointsToCheck.First();
            pointsToCheck.Remove(current);
            area++;
            foreach (var adjacent in current.AdjacentPointsCardinal())
            {
                if (Unregioned.TryGetValue(adjacent, out var value) && value == crop)
                {
                    Unregioned.Remove(adjacent);
                    regionPoints.Add(adjacent);
                    pointsToCheck.Add(adjacent);
                }
                else if (!regionPoints.Contains(adjacent))
                {
                    perimeter++;
                    perimetersToCheck.Add((current, adjacent));
                }
            }
        }

        foreach (var (currentPerimeter, adjacent) in perimetersToCheck)
        {
            var right = adjacent.Minus(currentPerimeter).RotateClockwise();
            if (!(regionPoints.Contains(currentPerimeter.Plus(right)) && !regionPoints.Contains(adjacent.Plus(right))))
            {
                sides++;
            }
        }

        return (perimeter, sides, area, crop, regionPoints);
    }

    public override object Solve1(string input)
    {
        var regions = BuildRegions(input);

        return regions.Sum(r => r.area * r.perimeter);
    }


    //Problem 2
    public override object ExpectedOutput2 => 1206;

    public override object Solve2(string input)
    {
        var regions = BuildRegions(input);

        return regions.Sum(r => r.area * r.sides);
    }
}