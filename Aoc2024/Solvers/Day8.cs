using AocHelpers;
using AocHelpers.Solvers;

public class Day8 : Solver
{
    public override int Day => 8;

    //Problem 1
    public override object ExpectedOutput1 => 14;

    public override object Solve1(string input)
    {
        var (grid, nodesByFrequency) = Setup(input);
        var antinodes = new List<(int x, int y)>();
        foreach (var key in nodesByFrequency.Keys)
        {
            var points = nodesByFrequency[key];
            foreach (var (first, second) in points.AllPairs())
            {
                var vector = second.Minus(first);
                antinodes.Add(first.Minus(vector));
                antinodes.Add(second.Plus(vector));
            }
        }

        return antinodes.Distinct().Count(n => grid.ContainsCoordinate(n));
    }


    //Problem 2
    public override object ExpectedOutput2 => 34;

    public override object Solve2(string input)
    {
        var (grid, nodesByFrequency) = Setup(input);
        var antinodes = new List<(int x, int y)>();
        foreach (var key in nodesByFrequency.Keys)
        {
            var points = nodesByFrequency[key];
            foreach (var (first, second) in points.AllPairs())
            {
                antinodes.Add(second);
                var vector = second.Minus(first);
                var antinode = first;
                while (grid.ContainsCoordinate(antinode))
                {
                    antinodes.Add(antinode);
                    antinode = antinode.Minus(vector);
                }

                antinode = second.Plus(vector);
                while (grid.ContainsCoordinate(antinode))
                {
                    antinodes.Add(antinode);
                    antinode = antinode.Plus(vector);
                }
            }
        }

        return antinodes.Distinct().Count();
    }

    private static (char[,] grid, Dictionary<char, List<(int x, int y)>> nodesByFrequency) Setup(string input)
    {
        var grid = input.To2dCharArray();
        var nodesByFrequency = new Dictionary<char, List<(int x, int y)>>();
        grid.Traverse((x, y, c) =>
        {
            if (c != '.')
            {
                if (!nodesByFrequency.ContainsKey(c))
                {
                    nodesByFrequency[c] = new List<(int x, int y)>();
                }

                nodesByFrequency[c].Add((x, y));
            }
        });
        return (grid, nodesByFrequency);
    }
}