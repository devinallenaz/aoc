namespace Aoc2022.Models;

public class ElfGroup
{
    public IEnumerable<Elf> Elves { get; }

    public Item Badge
    {
        get
        {
            return this.Elves.First().Rucksack.AllItems.First(item => Elves.All(e => e.Rucksack.AllItems.Contains(item)));
        }
    }

    public ElfGroup(IEnumerable<Elf> elves)
    {
        this.Elves = elves;
    }
}