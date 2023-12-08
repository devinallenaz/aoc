using System.Diagnostics;
using AocHelpers;

namespace Aoc2023.Models;

public class CamelCardGame : IComparable<CamelCardGame>
{
    public int[] Cards { get; }
    public int Wager { get; }
    public int Score { get; }

    public CamelCardGame(string init, bool withJokers = false)
    {
        var parts = init.SplitAndTrim();
        this.Cards = parts.First().Select(c =>
        {
            return c switch
            {
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                'T' => 10,
                'J' => withJokers ? 0 : 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new NotImplementedException(),
            };
        }).ToArray();
        this.Wager = int.Parse(parts.Last());
        this.Score = ScoreCards(this.Cards);
    }

    private static int ScoreCards(int[] cards)
    {
        var groups = cards.GroupBy(n => n);
        var groupCount = groups.Count();
        var maxGroup = groups.Max(g => g.Count());

        if (groups.Any(g => g.Key == 0) && groups.Any(g => g.Key != 0))
        {
            groupCount--;
            maxGroup = groups.Where(g => g.Key != 0).Max(g => g.Count()) + groups.First(g => g.Key == 0).Count();
        }

        switch (groupCount)
        {
            case 1:
                return 7;
            case 2:
                if (maxGroup == 4)
                {
                    return 6;
                }
                else
                {
                    return 5;
                }
            case 3:
                if (maxGroup == 3)
                {
                    return 4;
                }
                else
                {
                    return 3;
                }
            case 4:
                return 2;
            default:
                return 1;
        }
    }

    public int CompareTo(CamelCardGame? other)
    {
        if (other == null)
        {
            return 1;
        }

        var comp = this.Score - other.Score;
        var i = 0;
        while (comp == 0 && i < this.Cards.Length)
        {
            comp = this.Cards[i] - other.Cards[i];
            i++;
        }

        return comp;
    }
}