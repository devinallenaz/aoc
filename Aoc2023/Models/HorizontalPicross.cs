using AocHelpers;

namespace Aoc2023.Models;

public class HorizontalPicross
{
    private int[] Clue { get; }
    private bool?[] Puzzle { get; }
    private Dictionary<(bool, int, int, int), long> Cache { get; } = new();

    public HorizontalPicross(string init, int copies = 1)
    {
        var parts = init.SplitAndTrim();
        var segment = parts.First().Select(c =>
        {
            switch (c)
            {
                case '.':
                    return false;
                case '#':
                    return true;
                default:
                    return (bool?)null;
            }
        }).ToArray();
        this.Puzzle = new bool?[(segment.Length * copies) + (copies - 1)];
        Array.Fill(this.Puzzle, null);
        for (int i = 0; i < copies; i++)
        {
            Array.Copy(segment, 0, this.Puzzle, i * segment.Length + i, segment.Length);
        }

        var clueSegment = parts.Last().SplitCommas().Select(s => int.Parse(s)).ToArray();

        this.Clue = new int[clueSegment.Length * copies];
        for (int i = 0; i < copies; i++)
        {
            Array.Copy(clueSegment, 0, this.Clue, i * clueSegment.Length, clueSegment.Length);
        }
    }

    public long PossibleSolutions()
    {
        return PossibleSolutions(this.Puzzle[0]);
    }

    private long PossibleSolutions(bool? currentIsBroken, int clueIndex = 0, int brokenCount = 0, int cursor = 0, bool[]? puzzleSoFar = null)
    {
        if (!currentIsBroken.HasValue)
        {
            var falseResult = PossibleSolutions(false, clueIndex, brokenCount, cursor, puzzleSoFar);
            var trueResult = PossibleSolutions(true, clueIndex, brokenCount, cursor, puzzleSoFar);
            return trueResult + falseResult;
        }

        if (Cache.ContainsKey((currentIsBroken.Value, clueIndex, brokenCount, cursor)))
        {
            return Cache[(currentIsBroken.Value, clueIndex, brokenCount, cursor)];
        }

        //
        // if (puzzleSoFar == null)
        // {
        //     puzzleSoFar = Array.Empty<bool?>();
        // }
        //
        // puzzleSoFar = puzzleSoFar.Append(currentValue).ToArray();
        if (cursor + 1 == this.Puzzle.Length)
        {
            if (currentIsBroken.Value)
            {
                if (clueIndex == this.Clue.Length - 1 && brokenCount + 1 == this.Clue[clueIndex])
                {
                    Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 1);
                    return 1;
                }
                else
                {
                    Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 0);
                    return 0;
                }
            }
            else
            {
                if (brokenCount > 0)
                {
                    if (clueIndex != this.Clue.Length - 1 || brokenCount != this.Clue[clueIndex])
                    {
                        Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 0);
                        return 0;
                    }
                    else
                    {
                        Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 1);
                        return 1;
                    }
                }
                else
                {
                    if (clueIndex == this.Clue.Length)
                    {
                        Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 1);
                        return 1;
                    }
                    else
                    {
                        Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 0);
                        return 0;
                    }
                }
            }
        }

        var minSpace = this.Clue.Skip(clueIndex).Sum() + this.Clue.Length - 1 - clueIndex;
        if (this.Puzzle.Length - cursor + brokenCount < minSpace)
        {
            Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 0);
            return 0;
        }

        if (!currentIsBroken.Value)
        {
            if (brokenCount > 0)
            {
                if (brokenCount != this.Clue[clueIndex])
                {
                    Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 0);
                    return 0;
                }
                else
                {
                    if (clueIndex == this.Clue.Length - 1)
                    {
                        var result = this.Puzzle.Skip(cursor).Contains(true) ? 0 : 1;
                        Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), result);
                        return result;
                    }
                    else
                    {

                        var result = PossibleSolutions(this.Puzzle[cursor + 1], clueIndex + 1, 0, cursor + 1, puzzleSoFar);
                        Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), result);

                        return result;
                    }
                }
            }
            else
            {
                var result = PossibleSolutions(this.Puzzle[cursor + 1], clueIndex, 0, cursor + 1, puzzleSoFar);
                Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), result);
                return result;
            }
        }
        else if (clueIndex >= this.Clue.Length || brokenCount >= this.Clue[clueIndex])
        {
            Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), 0);
            return 0;
        }
        else
        {
            var result = PossibleSolutions(this.Puzzle[cursor + 1], clueIndex, brokenCount + 1, cursor + 1, puzzleSoFar);
            Cache.Add((currentIsBroken.Value, clueIndex, brokenCount, cursor), result);
            return result;
        }
    }
}