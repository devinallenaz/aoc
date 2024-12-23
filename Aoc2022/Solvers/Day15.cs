using System.Text.RegularExpressions;
using Aoc2022.Helpers;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day15 : Solver
{
    public override int Day => 15;

    public override object ExpectedOutput1 => 26;

    public override object Solve1(string input)
    {
        var (sensorBeacons, y, _) = SensorBeaconsFromData(input);
        var count = 0;
        for (int x = -10000; x < 10000; x++)
        {
            if (sensorBeacons.Any(sb => (x, y).ManhattanDistance(sb.sensor) <= sb.distance && (x, y) != sb.beacon))
            {
                count++;
            }
        }

        return count;
    }


    public override object ExpectedOutput2 => 56000011L;

    public override object Solve2(string input)
    {
        var (sensorBeacons, _, max) = SensorBeaconsFromData(input);

        var min = 0;
        var candidates = sensorBeacons.SelectMany(sb => Points.ManhattanCircle(sb.sensor, sb.distance + 1)).Distinct().Where(c => c.x >= min && c.x <= max && c.y >= min && c.y <= max);


        foreach (var candidate in candidates)
        {
            if (sensorBeacons.All(sb => candidate.ManhattanDistance(sb.sensor) > sb.distance))
            {
                return candidate.x * 4000000L + candidate.y;
            }
        }


        return "Nothing";
    }

    private static (List<((int x, int y) sensor, (int x, int y) beacon, int distance)>, int y1, int bound) SensorBeaconsFromData(string input)
    {
        var parts = input.Split('#');
        var y1 = int.Parse(parts[0]);
        var bound = int.Parse(parts[1]);
        var regex = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)", RegexOptions.Singleline);
        return (regex.Matches(parts[2]).Select(m =>
            {
                var sensor = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
                var beacon = (int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));
                var distance = sensor.ManhattanDistance(beacon);
                return (sensor, beacon, distance);
            }
        ).ToList(), y1, bound);
    }
}