using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day12 : Solver
{
    public override int Day => 12;

    //Problem 1
    public override object ExpectedOutput1 => 21;

    public override object Solve1(string input)
    {
        var picrosses = input.SplitLines().Select(l => new HorizontalPicross(l));
        return picrosses.Sum(p => p.PossibleSolutions());
    }

    //Problem 2
    public override object ExpectedOutput2 => 525152;

    public override object Solve2(string input)
    {
        var picrosses = input.SplitLines().Select(l => new HorizontalPicross(l, 5));

        return picrosses.Sum(p => p.PossibleSolutions());
    }
}