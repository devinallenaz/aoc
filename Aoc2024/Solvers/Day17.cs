using Aoc2024.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day17 : Solver
{
    public override int Day => 17;

    //Problem 1
    public override object ExpectedOutput1 => "4,6,3,5,6,3,5,2,1,0";

    public override object Solve1(string input)
    {
        var lines = input.SplitLines();
        var computer = new Computer(int.Parse(lines.First().SplitAndTrim().Last()));
        var programString = lines.Last().Substring(9);
        return computer.RunProgram(programString);
    }

    //Problem 2
    public override object ExpectedOutput2 => 117440l;

    public override object Solve2(string input)
    {
        var lines = input.SplitLines();
        var programString = lines.Last().Substring(9);
        var program = programString.SplitCommas().Select(int.Parse);
        var digitsToFind = new Stack<int>();
        foreach (var digit in program)
        {
            digitsToFind.Push(digit);
        }

        var x = FindDigits(digitsToFind, 1, programString);
        Console.WriteLine(new Computer(x).RunProgram(programString));
        return x;
    }

    private static long FindDigits(Stack<int> digitsToFind, long currentA, string program)
    {
        var targetDigit = digitsToFind.Pop();
        var candidateAs = new List<long>();
        for (var a = currentA; a < currentA + 8; a++)
        {
            var computer = new Computer(a);
            var digits = computer.RunProgramDigits(program);
            if (digits[0] == targetDigit)
            {
                candidateAs.Add(a);
            }
        }

        if (!candidateAs.Any())
        {
            return long.MaxValue;
        }

        if (!digitsToFind.Any())
        {
            return candidateAs.Min();
        }
        return candidateAs.Min(a => FindDigits(new Stack<int>(digitsToFind.Reverse()), a << 3, program));
    }
}