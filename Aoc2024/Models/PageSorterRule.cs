using AocHelpers;

namespace Aoc2024.Models;

public record PageSorterRule
{
    public int Before { get; }
    public int After { get; }

    public PageSorterRule(string input)
    {
        (Before, After) = input.SplitAndTrim("|").Select(int.Parse).FirstAndLast();
    }

    public bool Covers(int first, int second)
    {
        return (Before == first && After == second) || (Before == second && After == first);
    }
}