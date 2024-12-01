using AocHelpers;
using AocHelpers.Solvers;

public class Day1 : Solver
{
    public override int Day => 1;

    //Problem 1
    public override object ExpectedOutput1 => 11;

    private (List<int> left, List<int> right) BuildLists(string input)
    {
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in input.SplitLines())
        {
            var parts = line.SplitAndTrim();
            left.Add(int.Parse(parts.First()));
            right.Add(int.Parse(parts.Last()));
        }

        left.Sort();
        right.Sort();
        return (left, right);
    }

    public override object Solve1(string input)
    {
        var (left, right) = BuildLists(input);
        return left.Zip(right, (l, r) => Math.Abs(l - r)).Sum();
    }

    //Problem 2
    public override object ExpectedOutput2 => 31;

    public override object Solve2(string input)
    {
        var (left, right) = BuildLists(input);
        return left.Sum(l => l * right.Count(r => r == l));
    }
}