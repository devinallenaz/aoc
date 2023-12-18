using System.Drawing;
using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day16 : Solver
{
    public override int Day => 16;

    //Problem 1
    public override object ExpectedOutput1 => 46;
    private HashSet<(int, int, int, int)> Cache { get; set; }

    private IEnumerable<LightBeam> Refract(char[,] cave, LightBeam beam)
    {
        if (!Cache.Contains((beam.Position.x, beam.Position.y, beam.Direction.x, beam.Direction.y)))
        {
            var caveAt = cave.ValueOrNull(beam.Position.x, beam.Position.y);
            if (caveAt != null)
            {
                Cache.Add((beam.Position.x, beam.Position.y, beam.Direction.x, beam.Direction.y));
                switch (caveAt)
                {
                    case '.':
                        beam.Position = beam.Position.Plus(beam.Direction);
                        yield return beam;
                        break;
                    case '\\':
                        switch (beam.Direction.x, beam.Direction.y)
                        {
                            case (0, -1):
                                beam.Direction = Points.Left;
                                break;
                            case (0, 1):
                                beam.Direction = Points.Right;
                                break;
                            case (1, 0):
                                beam.Direction = Points.Down;
                                break;
                            case (-1, 0):
                                beam.Direction = Points.Up;
                                break;
                        }

                        beam.Position = beam.Position.Plus(beam.Direction);
                        yield return beam;
                        break;
                    case '/':
                        switch (beam.Direction.x, beam.Direction.y)
                        {
                            case (0, -1):
                                beam.Direction = Points.Right;
                                break;
                            case (0, 1):
                                beam.Direction = Points.Left;
                                break;
                            case (1, 0):
                                beam.Direction = Points.Up;
                                break;
                            case (-1, 0):
                                beam.Direction = Points.Down;
                                break;
                        }

                        beam.Position = beam.Position.Plus(beam.Direction);
                        yield return beam;
                        break;
                    case '-':
                        switch (beam.Direction.x, beam.Direction.y)
                        {
                            case (0, -1):
                            case (0, 1):
                                beam.Direction = Points.Left;
                                var otherBeam = new LightBeam(beam.Position, Points.Right);
                                beam.Position = beam.Position.Plus(beam.Direction);
                                yield return beam;
                                otherBeam.Position = otherBeam.Position.Plus(otherBeam.Direction);
                                yield return otherBeam;
                                break;
                            case (1, 0):
                            case (-1, 0):
                                beam.Position = beam.Position.Plus(beam.Direction);
                                yield return beam;
                                break;
                        }

                        break;
                    case '|':
                        switch (beam.Direction.x, beam.Direction.y)
                        {
                            case (0, -1):
                            case (0, 1):
                                beam.Position = beam.Position.Plus(beam.Direction);
                                yield return beam;
                                break;
                            case (1, 0):
                            case (-1, 0):
                                beam.Direction = Points.Up;
                                var otherBeam = new LightBeam(beam.Position, Points.Down);
                                beam.Position = beam.Position.Plus(beam.Direction);
                                yield return beam;
                                otherBeam.Position = otherBeam.Position.Plus(otherBeam.Direction);
                                yield return otherBeam;
                                break;
                        }

                        break;
                }
            }
        }
    }

    private int CountEnergized(char[,] cave, LightBeam beam)
    {
        this.Cache = new HashSet<(int, int, int, int)>();
        List<LightBeam> beams = new List<LightBeam>() { beam };
        while (beams.Any())
        {
            beams = beams.SelectMany(b => Refract(cave, b)).ToList();
        }

        return Cache.Select(c => (c.Item1, c.Item2)).Distinct().Count();
    }

    public override object Solve1(string input)
    {
        var cave = input.To2dCharArray();
        var beam = new LightBeam((0, 0), Points.Right);
        return this.CountEnergized(cave, beam);
    }

    //Problem 2
    public override object ExpectedOutput2 => 51;

    public override object Solve2(string input)
    {
        var cave = input.To2dCharArray();
        var candidateBeams = new List<LightBeam>();
        var width = cave.GetLength(0);
        var height = cave.GetLength(1);
        for (var x = 0; x <width; x++)
        {
            candidateBeams.Add(new LightBeam((x, 0), Points.Down));
            candidateBeams.Add(new LightBeam((x, height - 1), Points.Up));
        }
        for (var y = 0; y < height; y++)
        {
            candidateBeams.Add(new LightBeam((0, y), Points.Right));
            candidateBeams.Add(new LightBeam((width-1, y), Points.Left));
        }

        return candidateBeams.Max(b => CountEnergized(cave, b));
    }
}