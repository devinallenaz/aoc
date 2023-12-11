using System.Text.RegularExpressions;
using AocHelpers;
using AocHelpers.Models;
using AocHelpers.Solvers;

public class Day8 : Solver
{
    public override int Day => 8;

    private (Ring<char> instructions, Dictionary<string, (string Left, string Right)> nodes) ParseInput(string input)
    {
        var sections = input.SplitSections();
        var instructions = new Ring<char>(sections.First().ToCharArray());
        var nodes = new Dictionary<string, (string Left, string Right)>();
        foreach (var line in sections.Last().SplitLines())
        {
            var matches = this.Regex.Matches(line);
            nodes.Add(matches[0].Groups[1].Value, (matches[0].Groups[2].Value, matches[0].Groups[3].Value));
        }

        return (instructions, nodes);
    }

    private Regex Regex => new(@"(\w\w\w) = \((\w\w\w), (\w\w\w)\)");

    //Problem 1
    public override object ExpectedOutput1 => 6;

    public override object Solve1(string input)
    {
        var (instructions, nodes) = this.ParseInput(input);
        var currentInstruction = instructions.Head;
        var currentLocation = "AAA";
        var count = 0;
        while (currentLocation != "ZZZ")
        {
            count++;
            if (currentInstruction.Value == 'L')
            {
                currentLocation = nodes[currentLocation].Left;
            }
            else
            {
                currentLocation = nodes[currentLocation].Right;
            }

            currentInstruction = currentInstruction.Next;
        }

        return count;
    }

    //Problem 2
    public override object ExpectedOutput2 => 6l;

    public override object Solve2(string input)
    {
        var (instructions, nodes) = this.ParseInput(input);
        var currentInstruction = instructions.Head;

        var startingLocations = nodes.Where(n => n.Key.EndsWith("A")).Select(kvp => kvp.Key).ToList();
        var patterns = new List<(long length, long zIndex)>();
        foreach (var startingLocation in startingLocations)
        {
            var currentLocation = startingLocation;
            long zIndex = 0;
            var startLocations = new List<(long index, string location)>();
            var count = 0;
            while (!(currentInstruction.OriginalIndex == 0 && startLocations.Any(l => l.location == currentLocation)))
            {
                if (currentLocation.EndsWith('Z'))
                {
                    zIndex = count;
                }

                if (currentInstruction.OriginalIndex == 0)
                {
                    startLocations.Add((count, currentLocation));
                }

                count++;
                if (currentInstruction.Value == 'L')
                {
                    currentLocation = nodes[currentLocation].Left;
                }
                else
                {
                    currentLocation = nodes[currentLocation].Right;
                }

                currentInstruction = currentInstruction.Next;
            }

            patterns.Add((count - startLocations.First(l => l.location == currentLocation).index, zIndex));
        }



        return patterns.Aggregate(1l, (s, p) => Data.Lcm(p.zIndex, s));
    }
}