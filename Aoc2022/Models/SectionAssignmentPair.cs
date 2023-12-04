namespace Aoc2022.Models;

public class SectionAssignmentPair
{
    public SectionAssignment SectionAssignment1 { get; }

    public SectionAssignment SectionAssignment2 { get; }

    public SectionAssignmentPair(SectionAssignment sectionAssignment1, SectionAssignment sectionAssignment2)
    {
        SectionAssignment1 = sectionAssignment1;
        SectionAssignment2 = sectionAssignment2;
    }

    public bool ContainsAFullyRedundantAssignment => SectionAssignment1.ContainsSectionAssignment(SectionAssignment2) || SectionAssignment2.ContainsSectionAssignment(SectionAssignment1);
    public bool ContainsOverlappingAssignments => SectionAssignment1.OverlapsSectionAssigment(SectionAssignment2);
}