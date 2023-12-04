namespace Aoc2022.Models;

public class ItemWithWorry
{
    public long WorryLevel { get; set; }

    public ItemWithWorry(int worry)
    {
        this.WorryLevel = worry;
    }
}