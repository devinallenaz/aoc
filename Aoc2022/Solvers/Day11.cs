using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day11 : Solver
{
    public override int Day => 11;

    public override object ExpectedOutput1 => 10605l;

    public override object Solve1(string input)
    {
        var monkeys = MonkeysFromData(input);
        for (var i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.TakeTurn(true);
            }
        }

        return monkeys.OrderByDescending(m => m.ItemsInspected).Take(2).Select(m => m.ItemsInspected).Aggregate((i, j) => i * j);
    }

    public override object ExpectedOutput2 => 2713310158l;

    public override object Solve2(string input)
    {
        var monkeys = MonkeysFromData(input);
        for (var i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.TakeTurn(false);
            }
        }

        return monkeys.OrderByDescending(m => m.ItemsInspected).Take(2).Select(m => m.ItemsInspected).Aggregate((i, j) => i * j);
    }
    
    
    public static List<Monkey> MonkeysFromData(string input)
    {
        checked
        {
            var monkeyDictionary = new Dictionary<int, Monkey>();
            var getAllDivisors = () => monkeyDictionary.Select(m => m.Value.TestDivisor).Aggregate((i, j) => i * j);
            return input.SplitSections().Select((section, i) =>
                {
                    var lines = section.SplitLines().ToArray();
                    var items = lines[1].Split(':')[1].Trim().SplitCommas().Select(s => new ItemWithWorry(int.Parse(s)));
                    var operationParts = lines[2].Split('=')[1].Trim().Split();
                    Func<long, long> getOperand1 = getOperandFunction(operationParts[0]);
                    Func<long, long> getOperand2 = getOperandFunction(operationParts[2]);
                    Func<long, long> operation;
                    if (operationParts[1] == "*")
                    {
                        operation = (w) => getOperand1(w) * getOperand2(w);
                    }
                    else
                    {
                        operation = (w) => getOperand1(w) + getOperand2(w);
                    }

                    var testDivisor = long.Parse(lines[3].Split("by ")[1]);
                    var trueTarget = int.Parse(lines[4].Split("monkey ")[1]);
                    var throwToTrue = (ItemWithWorry item) => monkeyDictionary[trueTarget].ReceiveItem(item);
                    var falseTarget = int.Parse(lines[5].Split("monkey ")[1]);
                    var throwToFalse = (ItemWithWorry item) => monkeyDictionary[falseTarget].ReceiveItem(item);
                    var monkey = new Monkey(items, operation, testDivisor, throwToTrue, throwToFalse, getAllDivisors);
                    monkeyDictionary[i] = monkey;
                    return monkey;
                }
            ).ToList();
        }
    }

    private static Func<long, long> getOperandFunction(string operand)
    {
        if (operand == "old")
        {
            return (w) => w;
        }
        else
        {
            var opInt = long.Parse(operand);
            return (w) => opInt;
        }
    }
}