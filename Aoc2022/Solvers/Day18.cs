using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day18 : Solver
{
    public override int Day => 18;

    public override object ExpectedOutput1 => 64;

    public override object Solve1(string input)
    {
        var droplet = DropletFromData(input);


        return CountSurfaces(droplet);
    }

    public override object ExpectedOutput2 => 58;

    public override object Solve2(string input)
    {
        var droplet = DropletFromData(input, true);
        return CountSurfaces(droplet);
    }

    private static int CountSurfaces(bool[,,] droplet)
    {
        int surfaces = 0;
        for (var x = 0; x < droplet.GetLength(0); x++)
        {
            for (var y = 0; y < droplet.GetLength(1); y++)
            {
                var prev = false;
                for (var z = 0; z < droplet.GetLength(2); z++)
                {
                    if (prev ^ droplet[x, y, z])
                    {
                        surfaces++;
                    }

                    prev = droplet[x, y, z];
                }
            }
        }

        for (var x = 0; x < droplet.GetLength(0); x++)
        {
            for (var z = 0; z < droplet.GetLength(2); z++)
            {
                var prev = false;
                for (var y = 0; y < droplet.GetLength(1); y++)
                {
                    if (prev ^ droplet[x, y, z])
                    {
                        surfaces++;
                    }

                    prev = droplet[x, y, z];
                }
            }
        }

        for (var y = 0; y < droplet.GetLength(1); y++)
        {
            for (var z = 0; z < droplet.GetLength(2); z++)
            {
                var prev = false;
                for (var x = 0; x < droplet.GetLength(0); x++)
                {
                    if (prev ^ droplet[x, y, z])
                    {
                        surfaces++;
                    }

                    prev = droplet[x, y, z];
                }
            }
        }

        return surfaces;
    }

    private static bool[,,] DropletFromData(string input, bool fill = false)
    {
        var coords = input.SplitLines().Select(s =>
        {
            var parts = s.SplitCommas().ToArray();
            return (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        });
        var maxX = coords.Max(c => c.Item1);
        var maxY = coords.Max(c => c.Item2);
        var maxZ = coords.Max(c => c.Item3);
        var droplet = new bool[maxX + 2, maxY + 2, maxZ + 2];
        foreach (var coord in coords)
        {
            droplet[coord.Item1, coord.Item2, coord.Item3] = true;
        }

        if (fill)
        {
            droplet = FillDroplet(droplet);
        }

        return droplet;
    }

    private static bool[,,] FillDroplet(bool[,,] droplet)
    {
        var air = FindAir(droplet, new List<(int x, int y, int z)>(), new[] { (0, 0, 0) });
        var newDroplet = new bool[droplet.GetLength(0), droplet.GetLength(1), droplet.GetLength(2)];
        for (var x = 0; x < droplet.GetLength(0); x++)
        {
            for (var y = 0; y < droplet.GetLength(1); y++)
            {
                for (var z = 0; z < droplet.GetLength(2); z++)
                {
                    newDroplet[x, y, z] = droplet[x, y, z] || !air.Contains((x, y, z));
                }
            }
        }

        return newDroplet;
    }

    private static (int x, int y, int z)[] FindAir(bool[,,] droplet, IEnumerable<(int x, int y, int z)> allAir, IEnumerable<(int x, int y, int z)> airToCheck)
    {
        var allNeighbors = airToCheck.SelectMany(a => AirNeighbors(droplet, a)).Distinct().Where(a => !allAir.Contains(a)).ToArray();
        if (!allNeighbors.Any())
        {
            return allAir.ToArray();
        }

        return FindAir(droplet, allNeighbors.Concat(allAir), allNeighbors);
    }

    private static IEnumerable<(int x, int y, int z)> AirNeighbors(bool[,,] droplet, (int x, int y, int z) point)
    {
        if (point.x > 0 && !droplet[point.x - 1, point.y, point.z])
        {
            yield return (point.x - 1, point.y, point.z);
        }

        if (point.y > 0 && !droplet[point.x, point.y - 1, point.z])
        {
            yield return (point.x, point.y - 1, point.z);
        }

        if (point.z > 0 && !droplet[point.x, point.y, point.z - 1])
        {
            yield return (point.x, point.y, point.z - 1);
        }

        if (point.x + 1 < droplet.GetLength(0) && !droplet[point.x + 1, point.y, point.z])
        {
            yield return (point.x + 1, point.y, point.z);
        }

        if (point.y + 1 < droplet.GetLength(1) && !droplet[point.x, point.y + 1, point.z])
        {
            yield return (point.x, point.y + 1, point.z);
        }

        if (point.z + 1 < droplet.GetLength(2) && !droplet[point.x, point.y, point.z + 1])
        {
            yield return (point.x, point.y, point.z + 1);
        }
    }
}