using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day18 : Solver
{
    public override int Day => 18;

    //Problem 1
    public override object ExpectedOutput1 => 62L;

    private long CalculateDugArea(List<(long x, long y)> points)
    {
        List<(double x, double y)> doublePoints = points.Select(p => ((double)p.x, (double)p.y)).ToList();
        doublePoints.Add(doublePoints[0]);
        var area = Math.Abs(doublePoints.Take(doublePoints.Count - 1)
            .Select((p, i) => (doublePoints[i + 1].x - p.x) * (doublePoints[i + 1].y + p.y))
            .Sum() / 2);

        return (long)area;
    }


    public override object Solve1(string input)
    {
        var digger = new Digger(input);
        var dugPoints = digger.Dig();
        return CalculateDugArea(dugPoints);
    }

    //Problem 2
    public override object ExpectedOutput2 => 952408144115L;

    public override object Solve2(string input)
    {
        
        var digger = new Digger(input,true);
        var dugPoints = digger.Dig();
        return CalculateDugArea(dugPoints);
    }
}