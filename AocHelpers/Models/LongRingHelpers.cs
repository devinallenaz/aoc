namespace AocHelpers.Models;

public static class NumericRingItemHelpers
{
    public static void Move(this RingItem<long> item, int moveModulus)
    {
        if (item.Value > 0)
        {
            item.MoveForward(moveModulus);
        }
        else
        {
            item.MoveBack(moveModulus);
        }
    }

    private static void MoveForward(this RingItem<long> item, int moveModulus)
    {
        var times = item.Value % moveModulus;
        while (times > 0)
        {
            var next = item.Next;
            next.Next.SetPrevious(item);
            next.SetPrevious(item.Previous);
            item.SetPrevious(next);
            times--;
        }
    }

    private static void MoveBack(this RingItem<long> item, int moveModulus)
    {
        var times = (-item.Value) % moveModulus;
        while (times > 0)
        {
            var prev = item.Previous;
            prev.Previous.SetNext(item);
            prev.SetNext(item.Next);
            item.SetNext(prev);
            times--;
        }
    }

    public static long Find(this Ring<long> ring, int value, int offset)
    {
        return ring.AllItems.First(i => i.Value == value).ForwardValue(offset);
    }

    public static long ForwardValue(this RingItem<long> item, int times)
    {
        while (times > 0)
        {
            item = item.Next;
            times--;
        }

        return item.Value;
    }


    public static void Mix(this Ring<long> ring)
    {
        var allItems = ring.AllItems.OrderBy(i => i.OriginalIndex).ToArray();
        var moveModulus = allItems.Length - 1;
        foreach (var item in allItems)
        {
            item.Move(moveModulus);
        }
    }
}