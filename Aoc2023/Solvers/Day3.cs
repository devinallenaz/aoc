using System.Reflection.PortableExecutable;
using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day3 : Solver
{
    public override int Day => 3;
    public override object ExpectedOutput1 => 4361;
    public override object ExpectedOutput2 => 467835;

    public override object Solve1(string input)
    {
        var array = input.To2dCharArray();
        var numBuffer = "";
        var coordBuffer = new List<(int x, int y)>();
        var partNumbers = new List<int>();

        void FlushBuffer()
        {
            if (numBuffer != "")
            {
                if (coordBuffer.Any(point => point.AdjacentPoints().Any(ap => array.ValueOrNull(ap.x, ap.y) != null && array.ValueOrNull(ap.x, ap.y) != '.' && !array.ValueOrNull(ap.x, ap.y).IsDigit())))
                {
                    partNumbers.Add(int.Parse(numBuffer));
                }
            }

            numBuffer = "";
            coordBuffer.Clear();
        }

        for (var y = 0; y < array.GetLength(1); y++)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                var c = array[x, y];
                if (c.IsDigit())
                {
                    numBuffer += c.ToString();
                    coordBuffer.Add((x, y));
                }

                if (!c.IsDigit())
                {
                    FlushBuffer();
                }
            }

            FlushBuffer();
        }

        return partNumbers.Sum();
    }

    public override object Solve2(string input)
    {
        var array = input.To2dCharArray();
        var numBuffer = "";
        var coordBuffer = new List<(int x, int y)>();
        var gearBuffer = new Dictionary<(int x, int y), Gear>();

        void FlushNumBuffer()
        {
            if (numBuffer != "")
            {
                if (coordBuffer.SelectMany(p => p.AdjacentPoints()).Any(p => array.ValueOrNull(p.x, p.y) == '*'))
                {
                    var gearPoint = coordBuffer.SelectMany(p => p.AdjacentPoints()).First(p => array.ValueOrNull(p.x, p.y) == '*');
                    if (!gearBuffer.ContainsKey(gearPoint))
                    {
                        gearBuffer[gearPoint] = new Gear();
                    }

                    gearBuffer[gearPoint].AddPart(int.Parse(numBuffer));
                }
            }

            numBuffer = "";
            coordBuffer.Clear();
        }

        for (var y = 0; y < array.GetLength(1); y++)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                var c = array[x, y];
                if (c.IsDigit())
                {
                    numBuffer += c.ToString();
                    coordBuffer.Add((x, y));
                }

                if (!c.IsDigit())
                {
                    FlushNumBuffer();
                }
            }

            FlushNumBuffer();
        }

        return gearBuffer.Where(g => g.Value.IsComplete).Sum(g => g.Value.GearRatio);
    }
}