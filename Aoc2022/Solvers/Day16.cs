using System.Text.RegularExpressions;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day16 : Solver
{
    public override int Day => 16;

    public override object ExpectedOutput1 => 1651;

    public override object Solve1(string input)
    {
        var valveSystem = ValveSystemFromData(input);
        var initialState = valveSystem.GetInitialState();
        return valveSystem.StartingNode.MaxRelease(initialState);
    }

    public override object ExpectedOutput2 => 1707;

    public override object Solve2(string input)
    {
        var valveSystem = ValveSystemFromData(input);
        var initialState = valveSystem.GetInitialState();
        var possibleMeAssignments = initialState.ClosedValves.AllSubSets().Select(s => s.ToArray());
        return possibleMeAssignments.Max(a => valveSystem.StartingNode.MaxRelease(new ValveSystemState(26, a)) + valveSystem.StartingNode.MaxRelease(new ValveSystemState(26, initialState.ClosedValves.Where(v => !a.Contains(v)).ToArray())));
    }
    
    private static ValveSystem ValveSystemFromData(string input)
    {
        var regex = new Regex(@"Valve ([A-Z][A-Z]) has flow rate=(\d+); tunnels? leads? to valves? ([A-Z, ]+)");
        var valveInits = regex.Matches(input).Select(m => new ValveSystem.ValveInit(m.Groups[1].Value, int.Parse(m.Groups[2].Value), m.Groups[3].Value.Split(", ")));
        return new ValveSystem(valveInits);
    }
}