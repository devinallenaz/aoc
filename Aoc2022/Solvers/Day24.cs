using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day24 : Solver
{
    public override int Day => 24;

    public override object ExpectedOutput1 => 18;


    public override object Solve1(string input)
    {
        var lines = input.SplitLines().ToArray();
        var blizzMap = new char[lines[1].Length - 2, lines.Length - 2];
        for (var y = 0; y < lines.Length - 2; y++)
        {
            for (var x = 0; x < lines[y].Length - 2; x++)
            {
                blizzMap[x, y] = lines[y + 1][x + 1];
            }
        }

        var height = blizzMap.GetLength(1);
        var width = blizzMap.GetLength(0);
        var maxX = width - 1;
        var maxY = height - 1;

        var t = 1;
        List<(int, int)> possiblePositions = new List<(int, int)> { (0, 0) };
        while (!possiblePositions.Contains((maxX, maxY)))
        {
            t++;
            var nextPositions = NextPositions(possiblePositions, maxX, maxY);
            possiblePositions = nextPositions.Where(p => blizzMap[p.x, ((p.y + height - (t % height)) % height)] != 'v'
                                                         && blizzMap[p.x, (p.y + t) % height] != '^'
                                                         && blizzMap[((p.x + width - (t % width)) % width), p.y] != '>'
                                                         && blizzMap[(p.x + t) % width, p.y] != '<').ToList();
        }

        return t + 1;
    }

    public override object ExpectedOutput2 => 54;


    public override object Solve2(string input)
    {
        var lines = input.SplitLines().ToArray();
        var blizzMap = new char[lines[1].Length - 2, lines.Length - 2];
        for (var y = 0; y < lines.Length - 2; y++)
        {
            for (var x = 0; x < lines[y].Length - 2; x++)
            {
                blizzMap[x, y] = lines[y + 1][x + 1];
            }
        }

        var height = blizzMap.GetLength(1);
        var width = blizzMap.GetLength(0);
        var maxX = width - 1;
        var maxY = height - 1;

        var t = 0;
        GoFromTo((-1, 0), (maxX, maxY), blizzMap, ref t);
        t++;
        GoFromTo((maxX, maxY + 1), (0, 0), blizzMap, ref t);
        t++;
        GoFromTo((-1, 0), (maxX, maxY), blizzMap, ref t);

        return t + 1;
    }

    public List<(int x, int y)> NextPositions(List<(int x, int y)> possiblePositions, int maxX, int maxY)
    {
        var ret = new List<(int, int)>();
        foreach (var (x, y) in possiblePositions
                     .SelectMany(pair => new[]
                         { (pair.x, pair.y), (pair.x + 1, pair.y), (pair.x - 1, pair.y), (pair.x, pair.y + 1), (pair.x, pair.y - 1) }))

        {
            if (x >= 0 && y >= 0 && x <= maxX && y <= maxY)
            {
                ret.Add((x, y));
            }
        }

        return ret.Distinct().ToList();
    }

    private void GoFromTo((int x, int y) a, (int x, int y) b, char[,] blizzMap, ref int t)
    {
        var height = blizzMap.GetLength(1);
        var width = blizzMap.GetLength(0);
        var maxX = width - 1;
        var maxY = height - 1;

        List<(int, int)> possiblePositions = new List<(int, int)> { a };
        while (!possiblePositions.Contains(b))
        {
            t++;
            var time = t;
            var nextPositions = NextPositions(possiblePositions, maxX, maxY);
            possiblePositions = nextPositions.Where(p => blizzMap[p.x, ((p.y + height - (time % height)) % height)] != 'v'
                                                         && blizzMap[p.x, (p.y + time) % height] != '^'
                                                         && blizzMap[((p.x + width - (time % width)) % width), p.y] != '>'
                                                         && blizzMap[(p.x + time) % width, p.y] != '<').ToList();
            if (!possiblePositions.Any())
            {
                possiblePositions.Add(a);
            }
        }
    }
}