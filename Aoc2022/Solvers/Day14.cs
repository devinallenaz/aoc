using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day14 : Solver
{
    public override int Day => 14;

    public override object ExpectedOutput1 => 24;

    private static (int x, int y) Down => (0, 1);
    private static (int x, int y) DownLeft => (-1, 1);
    private static (int x, int y) DownRight => (1, 1);
    private static (int x, int y) SandOrigin => (500, 0);

    public override object Solve1(string input)
    {
        var cave = CaveFromData(input, false);
        var count = 0;
        while (DropSand(cave, SandOrigin))
        {
            count++;
        }

        return count;
    }
    
    public override object ExpectedOutput2 => 93;


    public override object Solve2(string input)
    {
        var cave = CaveFromData(input, true);
        var count = 0;
        while (DropSand(cave, SandOrigin))
        {
            count++;
        }

        return count;
    }
    
    
    public static char?[,] CaveFromData(string input, bool withFloor)
    {
        var cave = new char?[1500, 200];
        var maxY = 0;
        foreach (var path in input.SplitLines())
        {
            var parts = path.Split(" -> ").NonEmpty().Select(s =>
            {
                var parts2 = s.SplitCommas().ToArray();
                var v = (int.Parse(parts2[0]), int.Parse(parts2[1]));
                maxY = Math.Max(v.Item2, maxY);
                return v;
            }).ToArray();
            for (var i = 0; i < parts.Length - 1; i++)
            {
                FillPath(cave, parts[i], parts[i + 1]);
            }
        }

        if (withFloor)
        {
            FillPath(cave, (0, maxY + 2), (1499, maxY + 2));
        }

        return cave;
    }

    private static void FillPath(char?[,] cave, (int x, int y) start, (int x, int y) end)
    {
        var vector = end.Minus(start);
        var unitVector = (Math.Clamp(vector.x, -1, 1), Math.Clamp(vector.y, -1, 1));
        cave[start.x, start.y] = '#';
        var pos = (start.x, start.y);
        while (pos != end)
        {
            pos = pos.Plus(unitVector);
            cave[pos.x, pos.y] = '#';
        }
    }
    
    
    private static bool DropSand(char?[,] cave, (int x, int y) sandOrigin)
    {
        if (sandOrigin.y == cave.GetLength(1) - 1)
        {
            return false;
        }

        var nextPosition = NextPostion(cave, sandOrigin);
        if (nextPosition != null)
        {
            return DropSand(cave, nextPosition.Value);
        }

        if (cave[sandOrigin.x, sandOrigin.y] == null)
        {
            cave[sandOrigin.x, sandOrigin.y] = '*';
            return true;
        }

        return false;
    }

    private static (int x, int y)? NextPostion(char?[,] cave, (int x, int y) from)
    {
        var down = from.Plus(Down);
        if (cave[down.x, down.y] == null)
        {
            return down;
        }

        var downLeft = from.Plus(DownLeft);
        if (cave[downLeft.x, downLeft.y] == null)
        {
            return downLeft;
        }

        var downRight = from.Plus(DownRight);
        if (cave[downRight.x, downRight.y] == null)
        {
            return downRight;
        }

        return null;
    }
}