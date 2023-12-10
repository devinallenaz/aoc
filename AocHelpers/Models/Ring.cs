using System.Text;

namespace AocHelpers.Models;

public class Ring<T>
{
    public Ring(List<T> items) : this(items.ToArray())
    {
    }

    public Ring(T[] items)
    {
        this.AllItems = new List<RingItem<T>>();
        var head = new RingItem<T>(0, items[0], null);
        this.Head = head;
        this.AllItems.Add(head);
        var prev = head;
        for (int i = 1; i < items.Length; i++)
        {
            var ringItem = new RingItem<T>(i, items[i], prev);
            this.AllItems.Add(ringItem);
            prev = ringItem;
        }

        head.SetPrevious(prev);

        this.Length = items.Length;
    }

    public Ring(T[] items, Func<T, T> calculateValue)
    {
        this.AllItems = new List<RingItem<T>>();
        var head = new RingItem<T>(0, calculateValue(items[0]), null);
        this.Head = head;
        this.AllItems.Add(head);
        var prev = head;
        for (int i = 1; i < items.Length; i++)
        {
            var ringItem = new RingItem<T>(i, calculateValue(items[i]), prev);
            this.AllItems.Add(ringItem);
            prev = ringItem;
        }

        head.SetPrevious(prev);

        this.Length = items.Length;
    }

    public RingItem<T> Head { get; }
    public int Length { get; }
    public List<RingItem<T>> AllItems { get; }

    public override string ToString()
    {
        var sb = new StringBuilder("[");
        sb.Append(Head.Value);
        var item = Head.Next;
        while (item != Head)
        {
            sb.Append($", {item.Value}");
            item = item.Next;
        }

        sb.Append("]");
        return sb.ToString();
    }
}