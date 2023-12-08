using Aoc2023.Models;
using AocHelpers;
using AocHelpers;
using AocHelpers.Solvers;

public class Day7 : Solver
{
    public override int Day => 7;

    //Problem 1
    public override object ExpectedOutput1 => 6440;

    public override object Solve1(string input)
    {
        var hands = input.SplitLines().Select(l => new CamelCardGame(l)).Order();
        return hands.WithIndex().Sum((h) => h.item.Wager * (h.index + 1));
    }

    //Problem 2
    public override object ExpectedOutput2 => 5905;

    public override object Solve2(string input)
    {
        var hands = input.SplitLines().Select(l => new CamelCardGame(l, true)).Order();
        return hands.WithIndex().Sum((h) => h.item.Wager * (h.index + 1));
    }
}