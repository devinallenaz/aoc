using AocHelpers;

namespace Aoc2023.Models;

public class ScratchCard
{
    public int Id { get; }
    public IEnumerable<int> WinningNumbers { get; }
    public IEnumerable<int> MyNumbers { get; }

    public int Hits
    {
        get { return MyNumbers.Count(n => WinningNumbers.Contains(n)); }
    }

    public int Score
    {
        get
        {
            if (this.Hits == 0)
            {
                return 0;
            }

            return 1 << this.Hits - 1;
        }
    }

    public ScratchCard(string init)
    {
        var parts = init.SplitAndTrim(":");
        this.Id = int.Parse(parts.First().SplitAndTrim().Last());
        var segments = parts.Last().SplitAndTrim("|");
        this.WinningNumbers = segments.First().SplitAndTrim().Select(s => int.Parse(s));
        this.MyNumbers = segments.Last().SplitAndTrim().Select(s => int.Parse(s));
    }
}

public record ScratchCardStack(ScratchCard Card)
{
    public int Count { get; set; } = 1;
}