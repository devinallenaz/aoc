using AocHelpers;
using AocHelpers.Solvers;

public class Day9 : Solver
{
    public override int Day => 9;

    //Problem 1
    public override object ExpectedOutput1 => 114;

    private int Next(int[] input)
    {
        if (input.All(i => i == 0))
        {
            return 0;
        }

        return input.Last() + Next(Differences(input));
    }

    private int Previous(int[] input)
    {
        if (input.All(i => i == 0))
        {
            return 0;
        }

        return input.First() - Previous(input.Differences());
    }

    public override object Solve1(string input)
    {
        var histories = input.SplitLines().Select(l => l.SplitAndTrim().Select(int.Parse).ToArray());
        return histories.Sum(h => Next(h));
    }

    //Problem 2
    public override object ExpectedOutput2 => 2;

    public override object Solve2(string input)
    {
        
        var histories = input.SplitLines().Select(l => l.SplitAndTrim().Select(int.Parse).ToArray());
        return histories.Sum(h => Previous(h));
    }
}