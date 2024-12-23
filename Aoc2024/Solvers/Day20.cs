using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);

public class Day20 : Solver
{
    public override int Day => 20;

    //Problem 1
    public override object ExpectedOutput1 => 1;

    private static (int target, Dictionary<Point, int> scoredPoints) Setup(string input)
    {
        var sections = input.SplitSections();
        var target = int.Parse(sections[0]);
        var grid = sections[1].To2dCharArray();
        var (start, end) = grid.Find('S', 'E');
        var scoredPoints = new Dictionary<Point, int>();
        var current = end;
        var picosecondsToEnd = 0;
        while (current != start)
        {
            scoredPoints[current] = picosecondsToEnd;
            picosecondsToEnd++;
            current = current.AdjacentPointsCardinal().First(p => !scoredPoints.ContainsKey(p) && (grid.ValueOrNull(p) == '.' || grid.ValueOrNull(p) == 'S'));
        }

        scoredPoints[start] = picosecondsToEnd;
        return (target, scoredPoints);
    }

    public override object Solve1(string input)
    {
        var (target, scoredPoints) = Setup(input);
        var cheats = new List<(Point pointFrom, Point pointTo, int picosecondsSaved)>();
        foreach (var scoredPoint in scoredPoints)
        {
            foreach (var other in Points.ManhattanCircle(scoredPoint.Key, 2))
            {
                if (scoredPoints.TryGetValue(other, out var otherScore))
                {
                    if (otherScore < scoredPoint.Value - 2)
                    {
                        cheats.Add((scoredPoint.Key, other, scoredPoint.Value - 2 - otherScore));
                    }
                }
            }
        }

        return cheats.Count(c => c.picosecondsSaved >= target);
    }


    //Problem 2
    public override object ExpectedOutput2 => 285;

    public override object Solve2(string input)
    {
        var (target, scoredPoints) = Setup(input);
        var cheats = new List<(Point pointFrom, Point pointTo, int picosecondsSaved)>();
        foreach (var scoredPoint in scoredPoints)
        {
            var originalPoint = scoredPoint.Key;
            var originalScore = scoredPoint.Value;
            foreach (var otherWithDistance in scoredPoints.Select(other => (other, other.Key.ManhattanDistance(originalPoint))).Where(other => other.Item2 <= 20))
            {
                var otherPoint = otherWithDistance.other.Key;
                var otherScore = otherWithDistance.other.Value;
                var scoreToBeat = originalScore - otherWithDistance.Item2; //If the destination point doesn't beat this, it's not a shortcut
                if (otherScore < scoreToBeat)
                {
                    cheats.Add((originalPoint, otherPoint, scoreToBeat - otherScore));
                }
            }
        }

        return cheats.Count(c => c.picosecondsSaved >= target);
    }
}