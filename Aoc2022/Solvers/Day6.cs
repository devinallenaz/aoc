using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day6 : Solver
{
    public override int Day => 6;
    public override object ExpectedOutput1 => 11;

    public override object Solve1(string input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if (input.Skip(i).Take(4).Distinct().Count() == 4)
            {
                return i + 4;
            }
        }

        throw new NotImplementedException();
    }

    public override object ExpectedOutput2 => 26;

    public override object Solve2(string input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if (input.Skip(i).Take(14).Distinct().Count() == 14)
            {
                return i + 14;
            }
        }

        throw new NotImplementedException();
    }
}