using AocHelpers;
using AocHelpers.Solvers;

public class Day22 : Solver
{
    public override int Day => 22;

    //Problem 1
    public override object ExpectedOutput1 => 37327623l;

    private static long Mix(long first, long second) => first ^ second;

    private static long Prune(long input)
    {
        return input % 16777216;
    }

    private static long NextNumber(long input)
    {
        var step1 = Prune(Mix(input * 64l, input));
        var step2 = Prune(Mix(step1 / 32l, step1));
        var step3 = Prune(Mix(step2 * 2048l, step2));
        return step3;
    }

    private static long NumberAfter(long start, int iterations)
    {
        var count = 0;
        var number = start;
        iterations.Times(() => { number = NextNumber(number); });

        return number;
    }

    private static Dictionary<(long, long, long, long), long> PriceDictionary = new();
    private static void UpdatePriceDictionary(long start)
    {
        var changeKeysSeen = new HashSet<(long, long, long, long)>();
        var current = start;
        long previousPrice;
        var changeQueue = new Queue<long>();
        var currentPrice = current % 10;
        2000.Times(() =>
        {
            current = NextNumber(current);

            previousPrice = currentPrice;
            currentPrice = current % 10;

            var change = currentPrice - previousPrice;
            changeQueue.Enqueue(change);
            if (changeQueue.Count == 4)
            {
                var changeArray = changeQueue.ToArray();
                var changeKey = (changeArray[0], changeArray[1], changeArray[2], changeArray[3]);
                if (!changeKeysSeen.Contains(changeKey))
                {
                    PriceDictionary[changeKey] = PriceDictionary.GetValueOrDefault(changeKey) + currentPrice;
                    changeKeysSeen.Add(changeKey);
                }

                changeQueue.Dequeue();
            }
        });
    }

    public override object Solve1(string input)
    {
        var monkeys = input.SplitLines().Select(long.Parse);
        return monkeys.Sum(m => NumberAfter(m, 2000));
    }

    //Problem 2
    public override object ExpectedOutput2 => 23l;

    public override object Solve2(string input)
    {
        var monkeys = input.SplitLines().Select(long.Parse).ToList();
        monkeys.ForEach(UpdatePriceDictionary);
        return PriceDictionary.Max(kvp => kvp.Value);
    }
}