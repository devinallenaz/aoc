using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day13 : Solver
{
    public override int Day => 13;

    //Problem 1
    public override object ExpectedOutput1 => 505;


    public override object Solve1(string input)
    {
        var maps = input.SplitSections().Select(s => new RockMirrorMap(s)).ToList();
        var verticalTotal = 0;
        var horizontalTotal = 0;
        foreach (var map in maps)
        {
            var vertical = map.VerticalSymmetryIndex();
            if (vertical != null)
            {
                verticalTotal += vertical.Value;
            }

            var horizontal = map.HorizontalSymmetryIndex();
            if (horizontal != null)
            {
                horizontalTotal += horizontal.Value;
            }

            if (vertical == null && horizontal == null)
            {
                throw new InvalidOperationException();
            }
        }

        return 100 * verticalTotal + horizontalTotal;
    }

    //Problem 2
    public override object ExpectedOutput2 => 401;

    public override object Solve2(string input)
    {
        var maps = input.SplitSections().Select(s => new RockMirrorMap(s)).ToList();
        var total = 0;
        foreach (var map in maps)
        {
            var vertical = map.FixedVerticalSymmetryIndex();
            if (vertical != null)
            {
                total += 100 * vertical.Value;
            }
            else
            {
                var horizontal = map.FixedHorizontalSymmetryIndex();
                if (horizontal != null)
                {
                    total += horizontal.Value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

        }

        return total;
    }
}