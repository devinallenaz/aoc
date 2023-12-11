using AocHelpers;
using AocHelpers.Models;
using AocHelpers.Solvers;

public class Day11 : Solver
{
    public override int Day => 11;


    private static (List<MutableLongPoint> galaxies, HashSet<int> allX, HashSet<int> allY, int maxX, int maxY) InitGalaxies(string input)
    {
        var galaxies = new List<MutableLongPoint>();
        var allX = new HashSet<int>();
        var allY = new HashSet<int>();
        var maxX = 0;
        var maxY = 0;
        input.To2dCharArray().Traverse((x, y, c) =>
        {
            if (c == '#')
            {
                galaxies.Add(new(x, y));
                allX.Add(x);
                allY.Add(y);
            }

            maxX = Math.Max(x, maxX);
            maxY = Math.Max(y, maxY);
        });
        return (galaxies, allX, allY, maxX, maxY);
    }

    private void ExpandGalaxies(List<MutableLongPoint> galaxies, int maxX, int maxY, HashSet<int> allX, HashSet<int> allY, long factor = 1L)
    {
        for (int x = maxX; x >= 0; x--)
        {
            if (!allX.Contains(x))
            {
                foreach (var point in galaxies.Where(g => g.X > x))
                {
                    point.X += factor;
                }
            }
        }

        for (int y = maxY; y >= 0; y--)
        {
            if (!allY.Contains(y))
            {
                foreach (var point in galaxies.Where(g => g.Y > y))
                {
                    point.Y += factor;
                }
            }
        }
    }

    //Problem 1
    public override object ExpectedOutput1 => 374l;

    public override object Solve1(string input)
    {
        var (galaxies, allX, allY, maxX, maxY) = InitGalaxies(input);
        ExpandGalaxies(galaxies, maxX, maxY, allX, allY);
        return galaxies.AllPairs().Sum(s => Points.TaxiDistance(s.Item1, s.Item2));
    }

    //Problem 2
    public override object ExpectedOutput2 => 82000210L;

    public override object Solve2(string input)
    {
        var (galaxies, allX, allY, maxX, maxY) = InitGalaxies(input);
        ExpandGalaxies(galaxies, maxX, maxY, allX, allY, 999999L);
        return galaxies.AllPairs().Sum(s => Points.TaxiDistance(s.Item1, s.Item2));
    }
}