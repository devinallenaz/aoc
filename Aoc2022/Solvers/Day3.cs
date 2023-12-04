using Aoc2022.Helpers;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day3 : Solver
{
    public override int Day => 3;
    public override object ExpectedOutput1 => 157;
    public override object ExpectedOutput2 => 70;

    public override object Solve1(string input)
    {
        var rucksacks = RucksacksFromData(input);
        return rucksacks.Sum(r => r.Compartment1.First(c => r.Compartment2.Any(c2 => c2.Equals(c))).Priority);
    }

    public override object Solve2(string input)
    {
        var elfGroups = ElfGroupsFromRucksackData(input);

        return elfGroups.Sum(e => e.Badge.Priority);
    }
    
    
    public static List<Rucksack> RucksacksFromData(string input)
    {
        return RucksacksFromLines(input.SplitLines());
    }

    public static List<Rucksack> RucksacksFromLines(IEnumerable<string> lines)
    {
        return lines.Select(RucksackFromLine).ToList();
    }

    public static Rucksack RucksackFromLine(string line, int id)
    {
        var rucksack = new Rucksack(id);
        rucksack.AddRangeToCompartment1(line.Take(line.Length / 2).Select(c => new Item(c)));
        rucksack.AddRangeToCompartment2(line.Skip(line.Length / 2).Select(c => new Item(c)));
        return rucksack;
    }

    public static IEnumerable<ElfGroup> ElfGroupsFromRucksackData(string input)
    {
        var lines = input.SplitLines();
        var slices = lines.Slices(3);
        var elfGroups = slices.Select(ElfGroupFromLines);
        return elfGroups;
    }

    private static ElfGroup ElfGroupFromLines(IEnumerable<string> lines)
    {
        return new ElfGroup(lines.Select(ElfFromRucksackLine));
    }

    private static Elf ElfFromRucksackLine(string line, int id)
    {
        return new Elf(id, RucksackFromLine(line, id));
    }
}