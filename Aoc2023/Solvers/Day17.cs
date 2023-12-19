using System.Diagnostics;
using System.Text;
using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day17 : Solver
{
    public override int Day => 17;

    //Problem 1
    public override object ExpectedOutput1 => 102;


    public int LeastHeatLossFromTo(int[,] map, (int x, int y) start, (int x, int y) end, bool ultra = false)
    {
        HashSet<((int, int), (int, int), int)> visitedPositions = new();
        var startPath = new LavaPathState(map, start);
        var i = 0;

        var candidates = new List<LavaPathState>()
        {
            new(map, (0, 1), 1, startPath),
            new(map, (1, 0), 1, startPath),
        };
        while (candidates.Any())
        {
            var currentCandidate = candidates.MinBy(s => s.HeatLoss)!;
            candidates.Remove(currentCandidate);
            var nextCandidates = currentCandidate.NextStates(ultra);
            nextCandidates = nextCandidates.Where(c => !visitedPositions.Contains((c.Position, c.ArrivalVector, c.ArrivalVectorCount))).ToList();
            candidates.AddRange(nextCandidates);
            foreach (var next in nextCandidates)
            {
                if (next.Position == end)
                {
                    return next.HeatLoss;
                }

                visitedPositions.Add((next.Position, next.ArrivalVector, next.ArrivalVectorCount));
            }
        }

        return 0;
    }

    public override object Solve1(string input)
    {
        var map = input.To2dIntArray();
        var target = (map.GetLength(0) - 1, map.GetLength(1) - 1);
        return LeastHeatLossFromTo(map, (0, 0), target);
    }

//Problem 2
    public override object ExpectedOutput2 => 94;

    public override object Solve2(string input)
    {
        var map = input.To2dIntArray();
        var target = (map.GetLength(0) - 1, map.GetLength(1) - 1);
        return LeastHeatLossFromTo(map, (0, 0), target, true);
    }
}