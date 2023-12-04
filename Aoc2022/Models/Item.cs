namespace Aoc2022.Models;

public class Item
{
    public char Name { get; }

    public int Priority
    {
        get
        {
            if (this.Name < 91)
            {
                return this.Name - 38;
            }
            else
            {
                return this.Name - 96;
            }
        }
    }

    public Item(char name)
    {
        this.Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Item item)
        {
            return this.Name == item.Name;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        
        return this.Name.GetHashCode();
    }
}