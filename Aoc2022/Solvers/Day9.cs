using System.Numerics;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day9 : Solver
{
    public override int Day => 9;
    public override object ExpectedOutput1 => 13;

    public override object Solve1(string input)
    {
        var knots = new Vector2[2];
        Array.Fill(knots, new Vector2(0, 0));
        var movements = MovementsFromData(input);
        var allPositions = new List<Vector2>();
        foreach (var movement in movements)
        {
            knots[0] += movement;
            for (var i = 1; i < knots.Length; i++)
            {
                knots[i] = Follow(knots[i - 1], knots[i]);
            }

            allPositions.Add(knots.Last());
        }

        return allPositions.Distinct().Count();
    }
    
    public override object ExpectedOutput2 => 1;

    public override object Solve2(string input)
    {
        var knots = new Vector2[10];
        Array.Fill(knots, new Vector2(0, 0));
        var movements = MovementsFromData(input);
        var allPositions = new List<Vector2>();
        foreach (var movement in movements)
        {
            knots[0] += movement;
            for (var i = 1; i < knots.Length; i++)
            {
                knots[i] = Follow(knots[i - 1], knots[i]);
            }

            allPositions.Add(knots.Last());
        }

        return allPositions.Distinct().Count();
    }
    
    private static Vector2 VectorFromLetter(string input)
    {
        switch (input)
        {
            case "R": return new Vector2(1, 0);
            case "L": return new Vector2(-1, 0);
            case "U": return new Vector2(0, 1);
            case "D": return new Vector2(0, -1);
            default: throw new NotImplementedException();
        }
    }

    private static IEnumerable<Vector2> VectorsFromLine(string line)
    {
        var parts = line.Split();
        var times = int.Parse(parts[1]);
        for (var i = 0; i < times; i++)
        {
            yield return VectorFromLetter(parts[0]);
        }
    }

    public static List<Vector2> MovementsFromData(string input)
    {
        return input.SplitLines().SelectMany(VectorsFromLine).ToList();
    }
    
    private static Vector2 OneSquareAway => new Vector2(1, 1);
    private Vector2 Follow(Vector2 front, Vector2 back)
    {
        var followVector = front - back;
        if (followVector.Length() <= OneSquareAway.Length())
        {
            return back;
        }

        return back + new Vector2(Math.Clamp(followVector.X, -1, 1), Math.Clamp(followVector.Y, -1, 1));
    }

}