using AocHelpers;
using AocHelpers.Solvers;

public class Day11 : Solver
{
    public override int Day => 11;

    private static Dictionary<(long value, long splits), long> Cache = new();

    private static long StonesAfterSplits(long value, long splits)
    {
        if (splits == 0)
        {
            return 1l;
        }

        if (Cache.TryGetValue((value, splits), out var stones))
        {
            return stones;
        }

        if (value == 0)
        {
            Cache[(value, splits)] = StonesAfterSplits(1, splits - 1);
        }
        else
        {

            var valueString = value.ToString();
            if (valueString.Length % 2 == 0)
            {
                var half = valueString.Length / 2;
                var stone1 = long.Parse(valueString[..half]);
                var stone2 = long.Parse(valueString[half..]);
                Cache[(value, splits)] = StonesAfterSplits(stone1, splits - 1) + StonesAfterSplits(stone2, splits - 1);
            }
            else
            {
                Cache[(value, splits)] = StonesAfterSplits(value * 2024, splits - 1);
            }
        }

        return Cache[(value, splits)];
    }

    //Problem 1
    public override object ExpectedOutput1 => 55312l;

    public override object Solve1(string input)
    {
        var stones = input.SplitAndTrim().Select(long.Parse);
        Cache.Clear();
        return stones.Sum(v => StonesAfterSplits(v, 25));
    }

    //Problem 2
    public override object ExpectedOutput2 => 65601038650482l;

    public override object Solve2(string input)
    {
        var stones = input.SplitAndTrim().Select(long.Parse);
        Cache.Clear();
        return stones.Sum(v => StonesAfterSplits(v, 75));
    }
}