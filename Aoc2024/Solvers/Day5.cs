using Aoc2024.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day5 : Solver
{
    public override int Day => 5;

    //Problem 1
    public override object ExpectedOutput1 => 143;

    private static (IEnumerable<PageSorterRule> rules, IEnumerable<PageUpdateList> updates) ProcessInput(string input)
    {
        var (ruleText, updateText) = input.SplitSections().FirstAndLast();
        var rules = ruleText.SplitLines().Select(s => new PageSorterRule(s));
        var updates = updateText.SplitLines().Select(s => new PageUpdateList(s));
        return (rules, updates);
    }

    public override object Solve1(string input)
    {
        var (rules, updates) = ProcessInput(input);
        return updates.Where(u => u.CorrectBy(rules)).Sum(u => u.Middle);
    }


    //Problem 2
    public override object ExpectedOutput2 => 123;

    public override object Solve2(string input)
    {
        var (rules, updates) = ProcessInput(input);
        var incorrectUpdates = updates.Where(u => !u.CorrectBy(rules)).ToList();
        return incorrectUpdates.Sum(u => u.MiddleWhenSortedBy(rules));
    }

}