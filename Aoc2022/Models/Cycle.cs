namespace Aoc2022.Models;

public class Cycle<T>
{
    private T[] Items { get; }
    public int CurrentIndex { get; set; }

    public int Length => Items.Length;

    public Cycle(T[] items)
    {
        this.Items = items;
    }

    public T Next()
    {
        var item = this.Items[CurrentIndex];
        CurrentIndex = (CurrentIndex + 1) % Items.Length;
        return item;
    }
    
}