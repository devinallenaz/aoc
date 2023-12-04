namespace Aoc2022.Models;

public class SectionAssignment
{
    public IEnumerable<CampSection> Sections { get; }
    public SectionAssignment(IEnumerable<CampSection> sections)
    {
        Sections = sections;
    }

    public bool ContainsSectionAssignment(SectionAssignment other)
    {
        return other.Sections.All(s => this.Sections.Contains(s));
    }

    public bool OverlapsSectionAssigment(SectionAssignment other)
    {
        return other.Sections.Any(s => this.Sections.Contains(s));
    }

}