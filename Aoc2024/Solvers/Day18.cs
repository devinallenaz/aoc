using System.Collections.Immutable;
using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);

public class Day18 : Solver
{
    public override int Day => 18;

    //Problem 1
    public override object ExpectedOutput1 => 22;

    private static (int maxX, int maxY, int iterations, List<Point> points) Setup(string input)
    {
        var (first, rest) = input.SplitLines().HeadAndTail();
        var parameters = first.SplitCommas();
        var (maxX, maxY, iterations) = (int.Parse(parameters[0]), int.Parse(parameters[1]), int.Parse(parameters[2]));
        List<Point> points = rest.Select(s => s.SplitCommas().Select(int.Parse)).Select(a => (a.First(), a.Last())).ToList();
        return (maxX, maxY, iterations, points);
    }

    private int StepsToGoal(Point start, Point goal, ImmutableHashSet<Point> blocked)
    {
        HashSet<Point> visited = [start];
        List<(int score, Point point)> candidates = start.AdjacentPointsCardinal().Where(p => p.InBounds(goal) && !blocked.Contains(p)).Select(p => (1, p)).ToList();

        while (candidates.Any())
        {
            var current = candidates.MinBy(x => x.score);
            Console.WriteLine(current.score);
            candidates.Remove(current);
            visited.Add(current.point);
            if (current.point == goal)
            {
                return current.score;
            }

            Console.WriteLine(current.score);

            candidates.AddRange(
                current.point.AdjacentPointsCardinal()
                    .Where(p => p.InBounds(goal) && !blocked.Contains(p) && !visited.Contains(p) && candidates.All(c => c.point != p))
                    .Select(p => (current.score + 1, p))
            );
        }

        return int.MaxValue;
    }

    public override object Solve1(string input)
    {
        var (maxX, maxY, iterations, points) = Setup(input);
        var blocked = points.Take(iterations).ToImmutableHashSet();
        return StepsToGoal((0, 0), (maxX, maxY), blocked);
    }

    //Problem 2
    public override object ExpectedOutput2 => (6, 1);

    public override object Solve2(string input)
    {
        var (maxX, maxY, _, points) = Setup(input);
        var diff = points.Count / 2;
        var searchPoint = 0;
        bool clear = true;
        while (diff > 0)
        {
            if (clear)
            {
                searchPoint += diff;
            }
            else
            {
                searchPoint -= diff;
            }

            var blocked = points.Take(searchPoint).ToImmutableHashSet();
            clear = StepsToGoal((0, 0), (maxX, maxY), blocked) != int.MaxValue;
            if (diff <= 3)
            {
                diff--;
            }
            else
            {
                diff /= 2;
            }
        }

        if (diff < 0 && clear)
        {
            searchPoint++;
        }

        return points[searchPoint - (clear ? 0 : 1)];
    }
}