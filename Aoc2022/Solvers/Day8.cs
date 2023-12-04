using Aoc2022.Helpers;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day8 : Solver
{
    public override int Day => 8;
    public override object ExpectedOutput1 => 21;

    public override object Solve1(string input)
    {
        var trees = TreeGridFromData(input);
        return trees.Where(
            (tree, x, y) =>
                trees.Where((tree2, x2, y2) => x == x2 && y < y2)
                    .All(t => tree > t) ||
                trees.Where((tree2, x2, y2) => x == x2 && y > y2)
                    .All(t => tree > t) ||
                trees.Where((tree2, x2, y2) => x < x2 && y == y2)
                    .All(t => tree > t) ||
                trees.Where((tree2, x2, y2) => x > x2 && y == y2)
                    .All(t => tree > t)
        ).Count();
    }

    public override object ExpectedOutput2 => 8;


    public override object Solve2(string input)
    {
        var trees = TreeGridFromData(input);
        return trees.Max((tree, x, y) =>
            Look(trees, (x, y), (1, 0))
            * Look(trees, (x, y), (-1, 0))
            * Look(trees, (x, y), (0, 1))
            * Look(trees, (x, y), (0, -1))
        );
    }

    public static int[,] TreeGridFromData(string input)
    {
        var lines = input.SplitLines();
        var height = lines.Count();
        var width = lines.First().Length;
        var trees = new int[width, height];
        foreach (var (line, h) in lines.WithIndex())
        {
            foreach (var (tree, w) in line.WithIndex())
            {
                trees[w, h] = tree.ToNumericInt();
            }
        }

        return trees;
    }

    private int Look(int[,] trees, (int x, int y) pos, (int x, int y) direction)
    {
        var tree = trees[pos.x, pos.y];
        var width = trees.GetLength(0);
        var height = trees.GetLength(1);
        var count = 0;
        pos = (pos.x + direction.x, pos.y + direction.y);
        while (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
        {
            count++;
            if (trees[pos.x, pos.y] >= tree)
            {
                return count;
            }

            pos = (pos.x + direction.x, pos.y + direction.y);
        }

        return count;
    }
}