using System.Text.RegularExpressions;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day5 : Solver
{
    public override int Day => 5;

    public override object ExpectedOutput1 => "CMZ";

    public override object Solve1(string input)
    {
        var (stacks, moves) = StacksAndMovesFromData(input);
        foreach (var move in moves)
        {
            move.DoMove(stacks);
        }

        return string.Join("", stacks.Select(s => s.Peek()));
    }

    public override object ExpectedOutput2 => "MCD";

    public override object Solve2(string input)
    {
        var (stacks, moves) = StacksAndMovesFromData(input);
        foreach (var move in moves)
        {
            move.DoMoveUpgraded(stacks);
        }

        return string.Join("", stacks.Select(s => s.Peek()));
    }
    
    
    private static (Stack<char>[], List<Move>) StacksAndMovesFromData(string input)
    {
        var sections = input.SplitSections(false);
        return (StacksFromData(sections.First()), MovesFromData(sections.Last()));
    }

    private static List<Move> MovesFromData(string input)
    {
        return input.SplitLines(false).Select(MoveFromLine).ToList();
    }

    private static Move MoveFromLine(string line)
    {
        var regex = new Regex(@"move (\d+) from (\d+) to (\d)");
        var match = regex.Match(line);
        return new Move(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
    }

    private static Stack<char>[] StacksFromData(string input)
    {
        var lines = input.SplitLines(false).ToList();
        int numberOfStacks = lines.Last().Slices(4).Last().Skip(1).First().ToNumericInt();
        var stacks = new Stack<char>[numberOfStacks];
        lines.Take(lines.Count() - 1).Reverse().ToList().ForEach(line => ParseAndPushLineToStacks(line, stacks));
        return stacks;
    }

    private static void ParseAndPushLineToStacks(string line, Stack<char>[] stacks)
    {
        var crates = line.Slices(4).Select(GetCrateCharFromToken).ToList();

        foreach (var (crate, i) in crates.WithIndex())
        {
            if (stacks[i] == null)
            {
                stacks[i] = new Stack<char>();
            }

            if (crate != null)
            {
                stacks[i].Push(crate.Value);
            }
        }
    }

    private static char? GetCrateCharFromToken(IEnumerable<char> token)
    {
        return token.Skip(1).First().NullIf(' ');
    }
}