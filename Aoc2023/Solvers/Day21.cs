using AocHelpers;
using AocHelpers.Solvers;
using MathNet.Numerics;

public class Day21 : Solver
{
    public override int Day => 21;

    //Problem 1
    public override object ExpectedOutput1 => 42L;

    private HashSet<(long x, long y)> ReachedPositions { get; } = new();

    private long WalkGarden(char[,] garden, (long x, long y) start, long steps)
    {
        ReachedPositions.Clear();
        var currentPostions = new List<(long x, long y)>() { start };
        var landablePositions = 0;

        while (steps >= 0 && currentPostions.Any())
        {
            if (steps % 2 == 0)
            {
                landablePositions += currentPostions.Count;
            }

            foreach (var position in currentPostions)
            {
                ReachedPositions.Add(position);
            }

            steps--;
            var adjacentPositions = currentPostions.SelectMany(p => p.AdjacentPointsCardinal().Where(c => !ReachedPositions.Contains(c))).Distinct();


            currentPostions = adjacentPositions.Where(p => IsGardenPlot(garden, p.x, p.y)).ToList();
        }

        return landablePositions;
    }

    private long WalkInfiniteGarden(char[,] garden, (long x, long y) start, long steps)
    {
        ReachedPositions.Clear();
        var currentPostions = new List<(long x, long y)>() { start };
        var landablePositions = 0;

        while (steps >= 0 && currentPostions.Any())
        {
            if (steps % 2 == 0)
            {
                landablePositions += currentPostions.Count;
            }

            foreach (var position in currentPostions)
            {
                ReachedPositions.Add(position);
            }

            steps--;
            var adjacentPositions = currentPostions.SelectMany(p => p.AdjacentPointsCardinal().Where(c => !ReachedPositions.Contains(c))).Distinct();


            currentPostions = adjacentPositions.Where(p => IsInfiniteGardenPlot(garden, p.x, p.y)).ToList();
        }

        return landablePositions;
    }

    private bool IsGardenPlot(char[,] garden, long x, long y)
    {
        var gardenLength = garden.GetLength(0);
        if (x < 0 || x >= gardenLength || y < 0 || y >= gardenLength)
        {
            return false;
        }

        return garden[x, y] == '.';
    }

    private bool IsInfiniteGardenPlot(char[,] garden, long x, long y)
    {
        var gardenLength = garden.GetLength(0);
        while (x < 0)
        {
            x = x + gardenLength;
        }

        while (y < 0)
        {
            y = y + gardenLength;
        }

        return garden[x % gardenLength, y % gardenLength] == '.';
    }


    public override object Solve1(string input)
    {
        ReachedPositions.Clear();
        var garden = input.To2dCharArray();
        var start = garden.Find('S');
        return WalkGarden(garden, start, 64);
    }

    //Problem 2
    public override object ExpectedOutput2 => base.ExpectedOutput2;

    public override object Solve2(string input)
    {
        ReachedPositions.Clear();
        var garden = input.To2dCharArray();
        var start = garden.Find('S');
        garden[start.x, start.y] = '.';
        var gardenLength = garden.GetLength(1);
        long stepGoal = 26501365;
        var xN = stepGoal / gardenLength;
        var x1 = gardenLength * 1 + 65;
        var y1 = WalkInfiniteGarden(garden, start, x1);
        var x3 = gardenLength * 3 + 65;
        var y3 = WalkInfiniteGarden(garden, start, x3);
        var x5 = gardenLength * 5 + 65;
        var y5 = WalkInfiniteGarden(garden, start, x5);
        var coefficients = Fit.Polynomial(new[] { 1d, 3d, 5d }, new[] { (double)y1, (double)y3, (double)y5 }, 2);
        return Polynomial.Evaluate((double)xN, coefficients.Select(c=>Math.Round(c)).ToArray());
    }
}