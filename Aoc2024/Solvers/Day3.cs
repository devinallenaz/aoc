using System.Text.RegularExpressions;
using AocHelpers;
using AocHelpers.Solvers;

public class Day3 : Solver
{
    public override int Day => 3;

    //Problem 1
    public override object ExpectedOutput1 => 161;

    public override object Solve1(string input)
    {
        return FindMultiples(input);
    }

    private int FindMultiples(string input)
    {
        var matches = new Regex(@"mul\((\d\d?\d?),(\d\d?\d?)\)").Matches(input);
        return matches.Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));
    }

    //Problem 2
    public override object ExpectedOutput2 => 48;

    public override object Solve2(string input)
    {
        var improved = new Regex(@"don't\(\)(.|\n)*?((do\(\))|$)").Replace(input, "x");
        return FindMultiples(improved);
    }
}