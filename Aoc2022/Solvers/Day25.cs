using System.Xml.Schema;
using Aoc2022.Helpers;
using AocHelpers;
using AocHelpers.Solvers;
using Meta.Numerics.Extended;
using File = System.IO.File;

namespace Aoc2022.Solvers;

public class Day25 : Solver
{
    public override int Day => 25;

    public override object ExpectedOutput1 => "2=-1=0";

    public override object Solve1(string input)
    {
        var snafus = SnafusFromData(input);
        return string.Join("", snafus.Aggregate(AddSnafus).Select(i =>
        {
            return i switch
            {
                2 => '2',
                1 => '1',
                0 => '0',
                -1 => '-',
                -2 => '=',
                _ => throw new InvalidOperationException("invalid snafu")
            };
        }));
    }

    public override object ExpectedOutput2 => "2=-1=0";

    public override object Solve2(string input)
    {
        var snafus = SnafusFromData(input);
        return string.Join("", snafus.Aggregate(AddSnafus).Select(i =>
        {
            return i switch
            {
                2 => '2',
                1 => '1',
                0 => '0',
                -1 => '-',
                -2 => '=',
                _ => throw new InvalidOperationException("invalid snafu")
            };
        }));
    }

    private static IEnumerable<int[]> SnafusFromData(string input)
    {
        var snafus = new List<int[]>();
        foreach (var line in input.SplitLines())
        {
            var snafu = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '2':
                        snafu[i] = 2;
                        break;
                    case '1':
                        snafu[i] = 1;
                        break;
                    case '0':
                        snafu[i] = 0;
                        break;
                    case '-':
                        snafu[i] = -1;
                        break;
                    case '=':
                        snafu[i] = -2;
                        break;
                }
            }

            snafus.Add(snafu);
        }

        return snafus;
    }

    public int[] AddSnafus(int[] first, int[] second)
    {
        if (first.Any(d => d > 2 || d < -2) || second.Any(d => d > 2 || d < -2))
        {
            throw new InvalidOperationException("invalid snafu");
        }

        List<int> results = new List<int>();
        var operand3 = 0;
        for (int i = 0; i < Math.Max(first.Length, second.Length); i++)
        {
            var operand1 = first.Length > i ? first[first.Length - 1 - i] : 0;
            var operand2 = second.Length > i ? second[second.Length - 1 - i] : 0;
            var result = operand1 + operand2 + operand3;
            if (result > 2)
            {
                operand3 = 1;
                results.Insert(0, result - 5);
            }
            else if (result < -2)
            {
                operand3 = -1;
                results.Insert(0, result + 5);
            }
            else
            {
                operand3 = 0;

                results.Insert(0, result);
            }
        }

        if (operand3 != 0)
        {
            results.Insert(0, operand3);
        }

        return results.ToArray();
    }
}