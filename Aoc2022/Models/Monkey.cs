namespace Aoc2022.Models;

public class Monkey
{
    public Queue<ItemWithWorry> Inventory { get; } = new();
    public Func<long, long> Operation { get; }
    public long TestDivisor { get; }
    public long ItemsInspected { get; private set; }
    private Action<ItemWithWorry> ThrowToTrueMonkey { get; }
    private Action<ItemWithWorry> ThrowToFalseMonkey { get; }
    private Func<long> GetAllDivisors { get; }

    public Monkey(IEnumerable<ItemWithWorry> items, Func<long, long> operation, long testDivisor, Action<ItemWithWorry> throwToTrueMonkey, Action<ItemWithWorry> throwToFalseMonkey,Func<long> getAllDivisors)
    {
        Operation = operation;
        TestDivisor = testDivisor;
        ThrowToTrueMonkey = throwToTrueMonkey;
        ThrowToFalseMonkey = throwToFalseMonkey;
        GetAllDivisors = getAllDivisors;
        foreach (var itemWithWorry in items)
        {
            this.Inventory.Enqueue(itemWithWorry);
        }
    }

    public void TakeTurn(bool withRelief)
    {
        checked
        {
            while (this.Inventory.Any())
            {
                this.ItemsInspected++;
                var item = this.Inventory.Dequeue();
                item.WorryLevel = ConstrainWorry( this.Operation(item.WorryLevel) / (withRelief ? 3 : 1));
                if (Test(item))
                {
                    this.ThrowToTrueMonkey(item);
                }
                else
                {
                    this.ThrowToFalseMonkey(item);
                }
            }
        }
    }

    public void ReceiveItem(ItemWithWorry item)
    {
        this.Inventory.Enqueue(item);
    }

    private bool Test(ItemWithWorry item)
    {
        return item.WorryLevel % TestDivisor == 0;
    }

    private long ConstrainWorry(long w)
    {
        var allDivisors = this.GetAllDivisors();
        if (w > allDivisors)
        {
            return w % allDivisors;
        }

        return w;
    }
}