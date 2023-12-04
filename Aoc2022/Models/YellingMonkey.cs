namespace Aoc2022.Models;

public class YellingMonkey
{
    private const string Human = "humn";
    private long? Value { get; set; }
    private bool? UsesHuman { get; set; }
    private Func<long, long, long>? SolveForC { get; }
    private Func<long, long, long>? SolveForA { get; }
    private Func<long, long, long>? SolveForB { get; }
    private string? Name1 { get; }
    private string? Name2 { get; }

    public YellingMonkey(long value)
    {
        this.Value = value;
    }

    public YellingMonkey(string name1, string operation, string name2)
    {
        this.Name1 = name1;
        this.Name2 = name2;
        switch (operation)
        {
            case "+":
                this.SolveForC = SolvePlusForC;
                this.SolveForA = SolvePlusForAOrB;
                this.SolveForB = SolvePlusForAOrB;
                break;
            case "-":
                this.SolveForC = SolveMinusForC;
                this.SolveForA = SolveMinusForA;
                this.SolveForB = SolveMinusForB;
                break;
            case "*":
                this.SolveForC = SolveTimesForC;
                this.SolveForA = SolveTimesForAOrB;
                this.SolveForB = SolveTimesForAOrB;
                break;
            case "/":
                this.SolveForC = SolveDividedByForC;
                this.SolveForA = SolveDividedByForA;
                this.SolveForB = SolveDividedByForB;
                break;
        }
    }

    public long GetValue(Dictionary<string, YellingMonkey> monkeys)
    {
        if (this.Value == null)
        {
            if (this.SolveForC == null || this.Name1 == null || this.Name2 == null)
            {
                throw new ApplicationException("invalid initialization");
            }

            this.Value = this.SolveForC(monkeys[Name1].GetValue(monkeys), monkeys[Name2].GetValue(monkeys));
        }

        return this.Value.Value;
    }

    public long Solve(Dictionary<string, YellingMonkey> monkeys)
    {
        if (!this.GetUsesHuman(monkeys))
        {
            throw new ApplicationException("invalid call to solve");
        }

        if (monkeys[this.Name1].GetUsesHuman(monkeys))
        {
            return monkeys[this.Name1].SolveForHuman(monkeys, monkeys[this.Name2].GetValue(monkeys));
        }
        else
        {
            return monkeys[this.Name2].SolveForHuman(monkeys, monkeys[this.Name1].GetValue(monkeys));
        }
    }

    private long SolveForHuman(Dictionary<string, YellingMonkey> monkeys, long c)
    {
        if (SolveForA == null || SolveForB == null)
        {
            throw new ApplicationException("invalid call to solve");
        }

        if (!this.GetUsesHuman(monkeys))
        {
            throw new ApplicationException("invalid call to solve");
        }

        if (Name1 == Human)
        {
            var b = monkeys[Name2].GetValue(monkeys);
            return SolveForA(b, c);
        }

        if (Name2 == Human)
        {
            var a = monkeys[Name1].GetValue(monkeys);
            return SolveForB(a, c);
        }

        if (monkeys[Name1].GetUsesHuman(monkeys))
        {
            var b = monkeys[Name2].GetValue(monkeys);
            return monkeys[Name1].SolveForHuman(monkeys, this.SolveForA(b, c));
        }
        else
        {
            var a = monkeys[Name1].GetValue(monkeys);
            return monkeys[Name2].SolveForHuman(monkeys, this.SolveForB(a, c));
        }
    }

    public bool GetUsesHuman(Dictionary<string, YellingMonkey> monkeys)
    {
        if (this.UsesHuman == null)
        {
            CalculateUsesHuman(monkeys);
        }

        return this.UsesHuman.Value;
    }

    private void CalculateUsesHuman(Dictionary<string, YellingMonkey> monkeys)
    {
        if (this.Name1 == null && this.Name2 == null)
        {
            this.UsesHuman = false;
        }
        else if (this.Name1 == Human || this.Name2 == Human)
        {
            this.UsesHuman = true;
        }
        else
        {
            this.UsesHuman = monkeys[Name1].GetUsesHuman(monkeys) || monkeys[Name2].GetUsesHuman(monkeys);
        }
    }

    private long SolvePlusForC(long a, long b)
    {
        return a + b;
    }

    private long SolvePlusForAOrB(long aOrB, long target)
    {
        return target - aOrB;
    }

    private long SolveMinusForC(long a, long b)
    {
        return a - b;
    }

    private long SolveMinusForB(long a, long target)
    {
        return a - target;
    }

    private long SolveMinusForA(long b, long target)
    {
        return b + target;
    }

    private long SolveTimesForC(long a, long b)
    {
        return a * b;
    }

    private long SolveTimesForAOrB(long aOrB, long target)
    {
        return target / aOrB;
    }

    private long SolveDividedByForC(long a, long b)
    {
        return a / b;
    }

    private long SolveDividedByForA(long b, long target)
    {
        return target * b;
    }

    private long SolveDividedByForB(long a, long target)
    {
        return a / target;
    }
}