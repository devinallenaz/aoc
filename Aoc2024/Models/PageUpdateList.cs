using AocHelpers;

namespace Aoc2024.Models;

public class PageUpdateList
{
    private int[] Original { get; }
    private Dictionary<int, int> PageIndexes { get; } = new();
    public int Middle { get; }

    public PageUpdateList(string input)
    {
        var pages = input.SplitCommas().Select(int.Parse).ToList();
        pages.WithIndex().ToList().ForEach((x) => { PageIndexes[x.item] = x.index; });
        this.Middle = pages[pages.Count / 2];
        this.Original = pages.ToArray();
    }

    private bool Satisfies(PageSorterRule rule)
    {
        if (!PageIndexes.TryGetValue(rule.Before, out var beforeIndex) || !PageIndexes.TryGetValue(rule.After, out var afterIndex))
        {
            return true;
        }

        return beforeIndex < afterIndex;
    }

    public bool CorrectBy(IEnumerable<PageSorterRule> rules)
    {
        return rules.All(this.Satisfies);
    }

    public int MiddleWhenSortedBy(IEnumerable<PageSorterRule> rules)
    {
        var sorted = Original.ToList();
        sorted.Sort((a, b) =>
        {
            if (a == b)
            {
                return 0;
            }

            return rules.First(r => r.Covers(a, b)).Before == a ? 1 : -1;
        });
        return sorted[sorted.Count / 2];
    }
}