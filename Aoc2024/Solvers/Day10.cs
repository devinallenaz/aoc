using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);

public class Day10 : Solver
{
    public override int Day => 10;

    //Problem 1
    public override object ExpectedOutput1 => 36;
    private static List<Point> ReachableNines(int[,] grid, Point current, int currentValue)
    {
        if (currentValue == 9)
        {
            return [current];
        }


        var nextValue = currentValue + 1;

        var reachable = current.AdjacentPointsCardinal().Select(p => (p, grid.ValueOrNull(p.x, p.y))).Where(a => a.Item2 == nextValue).SelectMany(a => ReachableNines(grid, a.p, nextValue)).Distinct().ToList();
        
        return reachable;
    }

    private static int ReachableTrails(int[,] grid, Point current, int currentValue)
    {
        if (currentValue == 9)
        {
            return 1;
        }

        var nextValue = currentValue + 1;

        var reachable = current.AdjacentPointsCardinal().Where(next => grid.ValueOrNull(next.x, next.y) == nextValue)
            .Sum(next => ReachableTrails(grid, next, nextValue));
        return reachable;
    }

    public override object Solve1(string input)
    {
        var grid = input.To2dIntArray();
        var startingPoints = grid.FindAll(0);
        return startingPoints.Sum(p => ReachableNines(grid, p, 0).Count);
    }

    //Problem 2
    public override object ExpectedOutput2 => 81;

    public override object Solve2(string input)
    {
        var grid = input.To2dIntArray();
        var startingPoints = grid.FindAll(0);
        var first = ReachableTrails(grid, startingPoints.First(), 0);
        return startingPoints.Sum(p => ReachableTrails(grid, p, 0));
    }
}