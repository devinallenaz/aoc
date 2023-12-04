using Aoc2022.Helpers;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Models;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day23 : Solver
{
    public override int Day => 23;

    public override object ExpectedOutput1 => 110L;
    
    public override object Solve1(string input)
    {
        var currentPositions = ElfPositionsFromData(input);
        var proposedPositions = new Dictionary<(long x, long y), List<(long x, long y)>>();
        var moveArray = new[] { ((0, 1), (1, 1), (-1, 1)), ((0, -1), (1, -1), (-1, -1)), ((-1, 0), (-1, 1), (-1, -1)), ((1, 0), (1, 1), (1, -1)) };
        var moveRing = new Ring<((int x, int y) proposedMove, (int x, int y) other1, (int x, int y) other2)>(moveArray);
        var nextMove = moveRing.Head;
        for (int i = 0; i < 10; i++)
        {
            foreach (var elf in currentPositions.Keys.ToArray())
            {
                if (AllClear(currentPositions, elf))
                {
                    AddToProposed(proposedPositions, elf, elf);
                    continue;
                }

                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    continue;
                }

                nextMove = nextMove.Next;
                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    nextMove = nextMove.Previous;
                    continue;
                }

                nextMove = nextMove.Next;
                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    nextMove = nextMove.Previous.Previous;
                    continue;
                }

                nextMove = nextMove.Next;
                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    nextMove = nextMove.Next;
                    continue;
                }

                nextMove = nextMove.Next;
                AddToProposed(proposedPositions, elf, elf);
            }

            nextMove = nextMove.Next;
            currentPositions.Clear();
            foreach (var key in proposedPositions.Keys)
            {
                if (proposedPositions[key].Count > 1)
                {
                    foreach (var origin in proposedPositions[key])
                    {
                        currentPositions.Add(origin, true);
                    }
                }
                else
                {
                    currentPositions.Add(key, true);
                }
            }

            proposedPositions.Clear();
        }

        return (currentPositions.Keys.Max(p => p.y) + 1 - currentPositions.Keys.Min(p => p.y)) * (currentPositions.Keys.Max(p => p.x) + 1 - currentPositions.Keys.Min(p => p.x)) - currentPositions.Keys.Count;
    }
    public override object ExpectedOutput2 => 20;


    public override object Solve2(string input)
    {
        var currentPositions = ElfPositionsFromData(input);
        var proposedPositions = new Dictionary<(long x, long y), List<(long x, long y)>>();
        var moveArray = new[] { ((0, 1), (1, 1), (-1, 1)), ((0, -1), (1, -1), (-1, -1)), ((-1, 0), (-1, 1), (-1, -1)), ((1, 0), (1, 1), (1, -1)) };
        var moveRing = new Ring<((int x, int y) proposedMove, (int x, int y) other1, (int x, int y) other2)>(moveArray);
        var nextMove = moveRing.Head;
        var round = 0;
        var elfMoved = true;
        while (elfMoved)
        {
            round++;
            elfMoved = false;
            foreach (var elf in currentPositions.Keys.ToArray())
            {
                if (AllClear(currentPositions, elf))
                {
                    AddToProposed(proposedPositions, elf, elf);
                    continue;
                }

                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    elfMoved = true;
                    continue;
                }

                nextMove = nextMove.Next;
                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    elfMoved = true;
                    nextMove = nextMove.Previous;
                    continue;
                }

                nextMove = nextMove.Next;
                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    elfMoved = true;
                    nextMove = nextMove.Previous.Previous;
                    continue;
                }

                nextMove = nextMove.Next;
                if (MoveClear(currentPositions, elf, nextMove.Value))
                {
                    AddToProposed(proposedPositions, elf.Plus(nextMove.Value.proposedMove), elf);
                    elfMoved = true;
                    nextMove = nextMove.Next;
                    continue;
                }

                nextMove = nextMove.Next;
                AddToProposed(proposedPositions, elf, elf);
            }

            nextMove = nextMove.Next;
            currentPositions.Clear();
            foreach (var key in proposedPositions.Keys)
            {
                if (proposedPositions[key].Count > 1)
                {
                    foreach (var origin in proposedPositions[key])
                    {
                        currentPositions.Add(origin, true);
                    }
                }
                else
                {
                    currentPositions.Add(key, true);
                }
            }

            proposedPositions.Clear();
        }

        return round;
    }
    
    public static Dictionary<(long x, long y), bool> ElfPositionsFromData(string input)
    {
        var lines = input.SplitLines().ToArray();
        var positions = new Dictionary<(long x, long y), bool>();
        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[(lines.Length - y) - 1];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    positions[(x, y)] = true;
                }
            }
        }

        return positions;
    }
    
    public static bool AllClear(Dictionary<(long x, long y), bool> currentPositions, (long x, long y) elf)
    {
        return !currentPositions.ContainsKey(elf.Plus((0, 1)))
               && !currentPositions.ContainsKey(elf.Plus((1, 1)))
               && !currentPositions.ContainsKey(elf.Plus((-1, 1)))
               && !currentPositions.ContainsKey(elf.Plus((0, -1)))
               && !currentPositions.ContainsKey(elf.Plus((1, -1)))
               && !currentPositions.ContainsKey(elf.Plus((-1, -1)))
               && !currentPositions.ContainsKey(elf.Plus((1, 0)))
               && !currentPositions.ContainsKey(elf.Plus((-1, 0)));
    }

    public static bool MoveClear(Dictionary<(long x, long y), bool> currentPositions, (long x, long y) elf, ((int x, int y) proposedMove, (int x, int y) other1, (int x, int y) other2) move)
    {
        return !currentPositions.ContainsKey(elf.Plus(move.proposedMove)) && !currentPositions.ContainsKey(elf.Plus(move.other1)) && !currentPositions.ContainsKey(elf.Plus(move.other2));
    }

    public static void AddToProposed(Dictionary<(long, long), List<(long, long)>> proposedPositions, (long, long) proposed, (long, long) origin)
    {
        if (!proposedPositions.ContainsKey(proposed))
        {
            proposedPositions[proposed] = new List<(long, long)>();
        }

        proposedPositions[proposed].Add(origin);
    }
}