using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day4 : Solver
{
    public override int Day => 4;

    public override object ExpectedOutput1 => 2;

    public override object Solve1(string input)
    {
        var sectionAssigments = SectionAssignmentsFromData(input);
        return sectionAssigments.Count(s => s.ContainsAFullyRedundantAssignment);
    }

    public override object ExpectedOutput2 => 4;

    public override object Solve2(string input)
    {
        var sectionAssigments = SectionAssignmentsFromData(input);
        return sectionAssigments.Count(s => s.ContainsOverlappingAssignments);
    }
    
    private static IEnumerable<SectionAssignmentPair> SectionAssignmentsFromData(string input)
    {
        return input.SplitLines().Select(SectionAssignmentPairFromLine);
    }

    private static SectionAssignmentPair SectionAssignmentPairFromLine(string line)
    {
        var assigments = line.SplitCommas().Select(SectionAssignmentFromToken).ToArray();
        return new SectionAssignmentPair(assigments[0], assigments[1]);
    }

    private static SectionAssignment SectionAssignmentFromToken(string token)
    {
        var sections = token.Split('-').Select(int.Parse).ToArray();
        return new SectionAssignment(Data.Range(sections[0], sections[1]).Select(i => new CampSection(i)));
    }

}