using Aoc2022.Helpers;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day2 : Solver
{
    public override int Day => 2;
    public override object ExpectedOutput1 => 15;
    public override object ExpectedOutput2 => 12;

    public override object Solve1(string input)
    {
        var games = RpsGamesFromData(input);
        return games.Sum(g => g.MyPoints);
        ;
    }

    public override object Solve2(string input)
    {
        var games = RpsGamesFromPreferredOutcomeData(input);
        return games.Sum(g => g.MyPoints);
    }

    public static List<RpsGame> RpsGamesFromData(string input)
    {
        return input.SplitLines(false).Select((g, i) =>
        {
            var plays = g.Split();
            return new RpsGame(i, plays[0], plays[1]);
        }).ToList();
    }

    public static List<RpsGame> RpsGamesFromPreferredOutcomeData(string input)
    {
        return input.SplitLines(false).Select((g, i) =>
        {
            var plays = g.Split();
            return new RpsGameWithPreferredOutcome(i, plays[0], plays[1]);
        }).OfType<RpsGame>().ToList();
    }
}