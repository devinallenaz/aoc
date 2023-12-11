using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day5 : Solver
{
    public override int Day => 5;

    //Problem 1
    public override object ExpectedOutput1 => 35l;

    public override object Solve1(string input)
    {
        var (seedsInit, mapsInit) = input.SplitSections().HeadAndTail();
        var seeds = seedsInit.SplitAndTrim(":").Last().SplitAndTrim().Select(s => long.Parse(s));
        var maps = mapsInit.Select(s => new RangedMap(s));
        var locations = seeds.Select(s => { return maps.Aggregate(s, (n, map) => map.Map(n)); });
        return locations.Min();
    }

    //Problem 2
    public override object ExpectedOutput2 => 46l;

    public override object Solve2(string input)
    {
        var (seedsInit, mapsInit) = input.SplitSections().HeadAndTail();
        IEnumerable<(long start, long length)> seedRanges = seedsInit.SplitAndTrim(":").Last().SplitAndTrim().Slices(2).Select(pair => (long.Parse(pair.First()), long.Parse(pair.Last())));
        var maps = mapsInit.Select(s => new RangedMap(s));
        var locationRanges = maps.Aggregate(seedRanges, (ranges, map) => ranges.SelectMany(r => map.MapRange(r.start, r.length)));
        return locationRanges.Min(l => l.start);
    }
}