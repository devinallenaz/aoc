using Aoc2022.Helpers;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day1 : Solver
{
    public override int Day => 1;
    public override object ExpectedOutput1 => 24000;
    public override object ExpectedOutput2 => 45000;

    public override object Solve1(string input)
    {
        var elves = ElvesFromCalorieData(input);
        var maxCalories = elves.Select(e => e.CaloriesCarried).Max();
        return maxCalories;
        ;
    }

    public override object Solve2(string input)
    {
        var elves = ElvesFromCalorieData(input);
        return elves.OrderByDescending(e => e.CaloriesCarried).Take(3).Sum(e => e.CaloriesCarried);
    }

    private static List<Elf> ElvesFromCalorieData(string input)
    {
        return input.SplitSections().Select((e, i) =>
        {
            var elf = new Elf(i);
            foreach (var s in e.SplitLines())
            {
                elf.Rucksack.AddToCompartment1(new Food(int.Parse(s)));
            }

            return elf;
        }).ToList();
    }
}