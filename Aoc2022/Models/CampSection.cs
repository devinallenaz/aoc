namespace Aoc2022.Models;

public class CampSection
{
    public CampSection(int id)
    {
        Id = id;
    }

    private int Id { get; }

    public override bool Equals(object? obj)
    {
        if (obj is CampSection section)
        {
            return this.Id == section.Id;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return this.Id;
    }
}