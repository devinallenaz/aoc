using System.Text.RegularExpressions;
using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;
using MathNet.Numerics.LinearAlgebra;


public class Day24 : Solver
{
    public override int Day => 24;

    //Problem 1
    public override object ExpectedOutput1 => 2;
    private Regex LineRegex { get; } = new Regex(@"(-?\d+),  ?(-?\d+),  ?(-?\d+) @  ?(-?\d+), ? (-?\d+),  ?(-?\d+)");

    private IEnumerable<Line2d> Parse2d(string input)
    {
        foreach (var line in input.SplitLines())
        {
            var groups = LineRegex.Match(line).Groups;
            var x = decimal.Parse(groups[1].Value);
            var y = decimal.Parse(groups[2].Value);
            var dx = decimal.Parse(groups[4].Value);
            var dy = decimal.Parse(groups[5].Value);
            yield return new Line2d((x, y), (dx, dy));
        }
    }

    private IEnumerable<(double x, double y, double z, double dx, double dy, double dz)> Parse3d(string input)
    {
        foreach (var line in input.SplitLines())
        {
            var groups = LineRegex.Match(line).Groups;
            var x = double.Parse(groups[1].Value);
            var y = double.Parse(groups[2].Value);
            var z = double.Parse(groups[3].Value);
            var dx = double.Parse(groups[4].Value);
            var dy = double.Parse(groups[5].Value);
            var dz = double.Parse(groups[6].Value);
            yield return (x, y, z, dx, dy, dz);
        }
    }

    public override object Solve1(string input)
    {
        var sections = input.SplitSections();
        var parts = sections.First().Split();
        var min = decimal.Parse(parts.First());
        var max = decimal.Parse(parts.Last());
        var lines = Parse2d(sections.Last());
        return lines.AllPairs().Count(pair =>
        {
            var intersection = pair.Item1.Intersection(pair.Item2);
            return intersection is { past: false } && intersection.Value.x.Between(min, max) && intersection.Value.y.Between(min, max);
        });
    }

    //Problem 2
    public override object ExpectedOutput2 => 47l;

    public override object Solve2(string input)
    {
        var sections = input.SplitSections();
        var hailstones = Parse3d(sections.Last()).ToArray();

        // Stolen from https://www.reddit.com/r/adventofcode/comments/18pnycy/comment/kfcccsz/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button

        // Which was inspired by https://reddit.com/r/adventofcode/comments/18q40he/2023_day_24_part_2_a_straightforward_nonsolver/
        // and this comment https://www.reddit.com/r/adventofcode/comments/18q40he/comment/kesv08n/
        // In theory, each pair of hailstones corresponds to 3 equations, and we only need 6 to completely determine the 6 unknowns.
        // However, it turns out that taking 2 pairs out of 3 random hailstones isn't precise enough; the test had only around 10% success rate.
        // So we generate a lot more equations, using the pairs AB, BC, CD and so on until the end of the list.
        
        List<double[]> coefficientList = new();
        List<double> constantList = new();
        for (int i = 0; i < hailstones.Length - 1; i++)
        {
            var hailA = hailstones[i];
            var hailB = hailstones[i + 1];
            var row0 = new double[6];
            row0[0] = hailA.dy - hailB.dy;
            row0[1] = hailB.dx - hailA.dx;
            row0[3] = hailB.y - hailA.y;
            row0[4] = hailA.x - hailB.x;
            coefficientList.Add(row0);
            constantList.Add(hailB.y * hailB.dx - hailB.x * hailB.dy - hailA.y * hailA.dx + hailA.x * hailA.dy);
            var row1 = new double[6];
            row1[0] = hailA.dz - hailB.dz;
            row1[2] = hailB.dx - hailA.dx;
            row1[3] = hailB.z - hailA.z;
            row1[5] = hailA.x - hailB.x;
            coefficientList.Add(row1);
            constantList.Add(hailB.z * hailB.dx - hailB.x * hailB.dz - hailA.z * hailA.dx + hailA.x * hailA.dz);
            var row2 = new double[6];
            row2[1] = hailA.dz - hailB.dz;
            row2[2] = hailB.dy - hailA.dy;
            row2[4] = hailB.z - hailA.z;
            row2[5] = hailA.y - hailB.y;
            coefficientList.Add(row2);
            constantList.Add(hailB.z * hailB.dy - hailB.y * hailB.dz - hailA.z * hailA.dy + hailA.y * hailA.dz);
        }

        var coefficients = Matrix<double>.Build.DenseOfRowArrays(coefficientList);
        var constants = Vector<double>.Build.DenseOfEnumerable(constantList);
        var solution = coefficients.Solve(constants);
        var coords = solution.Take(3).Select(x => (long)Math.Round(x)).ToList();
        return coords.Sum();
    }
}