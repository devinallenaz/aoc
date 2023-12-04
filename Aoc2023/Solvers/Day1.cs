using System.Text.RegularExpressions;
using AocHelpers;
using AocHelpers.Solvers;

public class Day1 : Solver
{
    public override int Day => 1;
    public override object ExpectedOutput1 => 142;
    public override object ExpectedOutput2 => 281;

    private Dictionary<string, int> Matches = new()
    {
        { "1", 1 },
        { "2", 2 },
        { "3", 3 },
        { "4", 4 },
        { "5", 5 },
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    public override object Solve1(string input)
    {
        var lines = input.SplitLines();
        var total = lines.Sum(l => 10 * l.First(c => char.IsDigit(c)).ToNumericInt() + l.Last(c => char.IsDigit(c)).ToNumericInt());
        return total;
    }

    public override object Solve2(string input)
    {
        var lines = input.SplitLines();

        var lineFirstValues = lines.Select(l =>
        {
            var i1 = 0;
            while (i1 < l.Length)
            {
                var match = Matches.Keys.FirstOrDefault(k => l.Substring(i1).StartsWith(k));
                if (match != null)
                {
                    return Matches[match];
                }

                i1++;
            }

            return 0;
        });
        var lineLastValues = lines.Select(l =>
        {
            var i2 = l.Length;
            while (i2 >= 0)
            {
                var match = Matches.Keys.FirstOrDefault(k => l.Substring(0,i2).EndsWith(k));
                if (match != null)
                {
                    return Matches[match];
                }

                i2--;
            }

            return 0;
        });
        var total = lineFirstValues.Sum(v => v * 10) + lineLastValues.Sum();
        return total;
    }
}