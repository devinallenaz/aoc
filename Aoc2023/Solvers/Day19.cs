using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day19 : Solver
{
    public override int Day => 19;

    //Problem 1
    public override object ExpectedOutput1 => 19114L;

    private static (Dictionary<string, PartSorter> partSorters, IEnumerable<Part> parts) ParseInput(string input)
    {
        var sections = input.SplitSections();
        var partSorters = sections.First().SplitLines().Select(s => new PartSorter(s)).ToDictionary(ps => ps.Label);
        var parts = sections.Last().SplitLines().Select(s => new Part(s));
        return (partSorters, parts);
    }

    public override object Solve1(string input)
    {
        var (partSorters, parts) = ParseInput(input);
        long total = 0;
        foreach (var part in parts)
        {
            var location = "in";
            while (location != "A" && location != "R")
            {
                location = partSorters[location].Sort(part);
            }

            if (location == "A")
            {
                total += part.Total;
            }
        }

        return total;
    }


    //Problem 2
    public override object ExpectedOutput2 => 167409079868000;

    public override object Solve2(string input)
    {

        var (partSorters, _) = ParseInput(input);
        var partRangesToTest = new List<PartRange>()
        {
            new(),
        };
        var acceptedRanges = new List<PartRange>();
        while (partRangesToTest.Any())
        {
            var current = partRangesToTest.First();
            partRangesToTest.Remove(current);
            var resultRanges = partSorters[current.Destination].Sort(current).ToList();
            partRangesToTest.AddRange(resultRanges.Where(r => r.IsValid && r.Destination != "A" && r.Destination != "R"));
            acceptedRanges.AddRange(resultRanges.Where(r => r.IsValid && r.Destination == "A"));
        }

        return acceptedRanges.Sum(r => r.PossibleVariations);
    }
}