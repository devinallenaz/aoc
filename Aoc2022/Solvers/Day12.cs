using System.Diagnostics;
using Aoc2022.Helpers;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day12 : Solver
{
    public override int Day => 12;

    public override object ExpectedOutput1 => 31;


    public override object Solve1(string input)
    {
        var timer = new Stopwatch();
        var (map, start, end) = HeightMapFromData(input);
        var scoreMap = new int?[map.GetLength(0), map.GetLength(1)];
        timer.Start();
        scoreMap[end.x, end.y] = 0;

        return Seek(map, scoreMap, new() { end }, (_, x, y) => x == start.x && y == start.y, 1);
    }

    public override object ExpectedOutput2 => 29;

    public override object Solve2(string input)
    {
        var (map, _, end) = HeightMapFromData(input);
        var scoreMap = new int?[map.GetLength(0), map.GetLength(1)];
        scoreMap[end.x, end.y] = 0;

        return Seek(map, scoreMap, new() { end }, (c, _, _) => c == 'a', 1);
    }

    private static (char[,] map, (int x, int y) start, (int x, int y) end) HeightMapFromData(string input)
    {
        var lines = input.SplitLines().ToArray();
        var w = lines.First().Length;
        var h = lines.Length;
        var map = new char[w, h];
        (int x, int y)? start = null;
        (int x, int y)? end = null;
        foreach (var (line, y) in lines.WithIndex())
        {
            foreach (var (c, x) in line.WithIndex())
            {
                if (c == 'S')
                {
                    start = (x, y);
                    map[x, y] = 'a';
                }
                else if (c == 'E')
                {
                    end = (x, y);
                    map[x, y] = 'z';
                }
                else
                {
                    map[x, y] = c;
                }
            }
        }

        if (start == null || end == null)
        {
            throw new ApplicationException("Could not find start/end");
        }

        return (map, start.Value, end.Value);
    }

    private List<(int x, int y)> Directions = new() { (1, 0), (-1, 0), (0, 1), (0, -1) };

    private bool OnMap(char[,] map, (int x, int y) pos)
    {
        return pos.x >= 0
               && pos.x < map.GetLength(0)
               && pos.y >= 0
               && pos.y < map.GetLength(1);
    }

    private bool CanArriveFrom(char[,] map, (int x, int y) end, (int x, int y) start)
    {
        if (!OnMap(map, start))
        {
            return false;
        }

        return map[start.x, start.y] >= map[end.x, end.y] - 1;
    }

    private int Seek(char[,] map, int?[,] scoreMap, List<(int x, int y)> nodes, Func<char, int, int, bool> seek, int score = 0)
    {
        var allNeighbors = nodes.SelectMany(end => Directions.Select(d => end.Plus(d)).Where(s => CanArriveFrom(map, end, s) && scoreMap[s.x, s.y] == null)).Distinct().ToList();
        foreach (var pos in allNeighbors)
        {
            if (seek(map[pos.x, pos.y], pos.x, pos.y))
            {
                return score;
            }

            scoreMap[pos.x, pos.y] = score;
        }


        return Seek(map, scoreMap, allNeighbors, seek, score + 1);
    }
}