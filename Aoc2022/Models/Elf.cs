namespace Aoc2022.Models;

public class Elf
{
    public int Id { get; }

    public Elf(int id)
    {
        this.Id = id;
        this.Rucksack = new Rucksack(this.Id);
    }

    public Elf(int id, Rucksack rucksack)
    {
        this.Id = id;
        this.Rucksack = rucksack;
    }

    public int CaloriesCarried
    {
        get
        {
            return this.Rucksack.AllItems.OfType<Food>().Sum(f => f.Calories);
        }
    }

    public Rucksack Rucksack { get; }
}
