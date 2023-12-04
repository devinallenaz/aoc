namespace Aoc2022.Models;

public class Rucksack
{
    public int Id { get; }
    private List<Item> _compartment1 = new();
    private List<Item> _compartment2 = new();
    public IEnumerable<Item> Compartment1 => _compartment1;
    public IEnumerable<Item> Compartment2 => _compartment2;

    public Rucksack(int id)
    {
        this.Id = id;
    }

    public IEnumerable<Item> AllItems
    {
        get
        {
            return _compartment1.Concat(_compartment2);
        }
    }

    public void AddToCompartment1(Item item)
    {
        _compartment1.Add(item);
    }
    public void AddToCompartment2(Item item)
    {
        _compartment2.Add(item);
    }
    public void AddRangeToCompartment1(IEnumerable<Item> items)
    {
        _compartment1.AddRange(items);
    }
    public void AddRangeToCompartment2(IEnumerable<Item> items)
    {
        _compartment2.AddRange(items);
    }
    
}