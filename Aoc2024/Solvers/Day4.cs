using AocHelpers;
using AocHelpers.Solvers;

public class Day4 : Solver
{
    public override int Day => 4;

    //Problem 1
    public override object ExpectedOutput1 => 18;

    public override object Solve1(string input)
    {
        var grid = input.To2dCharArray();
        var count = 0;
        grid.Traverse((x, y, c) =>
        {
            if (c == 'X')
            {
                foreach (var dir in Points.AllDirections)
                {
                    var m = (x, y).Plus(dir);
                    if (grid.ValueOrNull(m) == 'M')
                    {
                        var a = m.Plus(dir);
                        if (grid.ValueOrNull(a) == 'A')
                        {
                            var s = a.Plus(dir);
                            if (grid.ValueOrNull(s) == 'S')
                            {
                                count++;
                            }
                        }
                    }
                }
            }
        });
        return count;
    }

    //Problem 2
    public override object ExpectedOutput2 => 9;

    public override object Solve2(string input)
    {
        var grid = input.To2dCharArray();
        var count = 0;
        grid.Traverse((x, y, c) =>
        {
            var center = (x, y);
            if (c == 'A')
            {
                char?[] diagonal1 = [grid.ValueOrNull(center.Plus(Points.UpLeft)), grid.ValueOrNull(center.Plus(Points.DownRight))];
                char?[] diagonal2 = [grid.ValueOrNull(center.Plus(Points.UpRight)), grid.ValueOrNull(center.Plus(Points.DownLeft))];
                if (diagonal1.Contains('M') && diagonal1.Contains('S') && diagonal2.Contains('M') && diagonal2.Contains('S'))
                {
                    count++;
                }
            }
        });
        return count;
    }
}