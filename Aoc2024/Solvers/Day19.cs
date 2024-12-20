using AocHelpers;
using AocHelpers.Solvers;

public class Day19 : Solver
{
    public override int Day => 19;

    private Dictionary<string, long> _patternCache = null!;


    private (List<string>, List<string>) Setup(string input)
    {
        var sections = input.SplitSections();
        var towels = sections[0].SplitCommas().OrderByDescending(t => t.Length).ToList();
        var patterns = sections[1].SplitLines().ToList();
        _patternCache = new Dictionary<string, long>();

        return (towels, patterns);
    }

    private long WaysToMakePattern(string pattern, List<string> towels)
    {
        if (pattern == "")
        {
            return 1;
        }

        if (_patternCache.TryGetValue(pattern, out var ways))
        {
            return ways;
        }

        var validStarts = towels.Where(pattern.StartsWith).ToList();
        var waysToMakePattern = 0l;
        if (validStarts.Count != 0)
        {
            waysToMakePattern = validStarts.Sum(t => WaysToMakePattern(pattern[t.Length..], towels));
        }

        _patternCache[pattern] = waysToMakePattern;

        return waysToMakePattern;
    }

    //Problem 1
    public override object ExpectedOutput1 => 6;

    public override object Solve1(string input)
    {
        var (towels, patterns) = Setup(input);

        return patterns.Count(p => WaysToMakePattern(p, towels) > 0);
    }


    //Problem 2
    public override object ExpectedOutput2 => 16;

    public override object Solve2(string input)
    {
        var (towels, patterns) = Setup(input);

        return patterns.Sum(p => WaysToMakePattern(p, towels));
    }
}