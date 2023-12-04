using System.Text.RegularExpressions;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day19 : Solver
{
    public override int Day => 19;


    public override object ExpectedOutput1 => 33;


    public override object Solve1(string input)
    {
        var blueprints = BlueprintsFromData(input);
        return blueprints.Sum(b => b.Id * b.MaxGeodes(24, new OperationState(new InventoryState(0, 0, 0, 0), new RobotState(1, 0, 0, 0))));
    }

    public override object ExpectedOutput2 => 62 * 56;


    public override object Solve2(string input)
    {
        var blueprints = BlueprintsFromData(input).OrderBy(b => b.Id).Take(3).ToArray();
        var maxes = new int[3];
        for (int i = 0; i < Math.Min(3, blueprints.Length); i++)
        {
            maxes[i] = blueprints[i].MaxGeodes(32, new OperationState(new InventoryState(0, 0, 0, 0), new RobotState(1, 0, 0, 0)));
        }

        return maxes.Where(m => m != 0).Aggregate(1, (prev, m) => prev * m);
    }
    
    
    public static Blueprint[] BlueprintsFromData(string input)
    {
        var regex = new Regex(@"Blueprint (\d+): Each ore robot costs (\d+) ore\. Each clay robot costs (\d+) ore\. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian\.");
        return regex.Matches(input).Select(m =>
            new Blueprint(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value), int.Parse(m.Groups[6].Value), int.Parse(m.Groups[7].Value))).ToArray();
    }
}