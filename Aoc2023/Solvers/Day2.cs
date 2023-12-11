using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day2 : Solver
{
    public override int Day => 2;
    public override object ExpectedOutput1 => 8;
    public override object ExpectedOutput2 => 2286;

    public override object Solve1(string input)
    {
        var redCount = 12;
        var greenCount = 13;
        var blueCount = 14;
        var cubeDrawGames = input.SplitLines().Select(s => new CubeDrawGame(s));
        var possibleGames = cubeDrawGames.Where(g => g.Draws.All(d => d.Green <= greenCount && d.Red <= redCount && d.Blue <= blueCount));
        return possibleGames.Sum(g => g.Id);
    }

    public override object Solve2(string input)
    {
        var cubeDrawGames = input.SplitLines().Select(s => new CubeDrawGame(s));
        return cubeDrawGames.Sum(g => g.Draws.Max(d => d.Red) * g.Draws.Max(d => d.Green) * g.Draws.Max(d => d.Blue));
    }
}