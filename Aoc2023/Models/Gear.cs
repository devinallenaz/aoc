using System.Diagnostics.CodeAnalysis;

namespace Aoc2023.Models;

public class Gear
{
    public void AddPart(int part)
    {
        if (!this.Part1.HasValue)
        {
            this.Part1 = part;
        }
        else if (!this.Part2.HasValue)
        {
            this.Part2 = part;
        }
        else
        {
            throw new InvalidOperationException("Third part in gear");
        }
    }

    public int? Part1 { get; private set; }
    public int? Part2 { get; private set; }

    public int GearRatio    
    {
        get
        {
            if (!this.IsComplete)
            {
                throw new InvalidOperationException("Incomplete gear ratio");
            }

            return Part1.Value * Part2.Value;
        }
    }

    [MemberNotNullWhen(true, "Part1")]
    [MemberNotNullWhen(true, "Part2")]
    public bool IsComplete => this.Part1.HasValue && this.Part2.HasValue;
}