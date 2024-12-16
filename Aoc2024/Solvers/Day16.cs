using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);
using Step = ((int x, int y) position, (int x, int y) direction, int score);

public class Day16 : Solver
{
    public override int Day => 16;

    private static (int score, IEnumerable<Point> paths) PathToEnd(Dictionary<Point, char> maze, bool findAll = false)
    {
        Step start = (maze.First(kvp => kvp.Value == 'S').Key, Points.East, 0);
        var candidates = new List<(Step step, IEnumerable<Point> path)>() { (start, []) };
        var visited = new HashSet<(Point position, Point direction)>() { (start.position, start.direction) };
        var optimalPaths = new List<Point>() { start.position };
        var finalScore = int.MaxValue;

        while (candidates.Any(c => c.step.score < finalScore))
        {
            var cheapest = candidates.MinBy(s => s.step.score);
            candidates.Remove(cheapest);
            visited.Add((cheapest.step.position, cheapest.step.direction));
            var forward = cheapest.step.position.Plus(cheapest.step.direction);

            if (maze[forward] == 'E')
            {
                if (!findAll)
                {
                    return (cheapest.step.score + 1, optimalPaths);
                }

                optimalPaths.AddRange(cheapest.path);
                optimalPaths.Add(forward);
                finalScore = cheapest.step.score + 1;
            }

            if (maze[forward] != '#' && !visited.Contains((forward, cheapest.step.direction)))
            {
                candidates.Add(((forward, cheapest.step.direction, cheapest.step.score + 1), cheapest.path.Append(forward)));
            }

            var clockwiseDirection = cheapest.step.direction.RotateClockwise();
            var clockwisePosition = cheapest.step.position.Plus(clockwiseDirection);
            if (maze[clockwisePosition] != '#' && !visited.Contains((clockwisePosition, clockwiseDirection)))
            {
                candidates.Add(((clockwisePosition, clockwiseDirection, cheapest.step.score + 1001), cheapest.path.Append(clockwisePosition)));
            }

            var counterClockwiseDirection = cheapest.step.direction.RotateCounterClockwise();
            var counterClockwisePosition = cheapest.step.position.Plus(counterClockwiseDirection);
            if (maze[counterClockwisePosition] != '#' && !visited.Contains((counterClockwisePosition, counterClockwiseDirection)))
            {
                candidates.Add(((counterClockwisePosition, counterClockwiseDirection, cheapest.step.score + 1001), cheapest.path.Append(counterClockwisePosition)));
            }
        }

        return (finalScore, optimalPaths.Distinct());
    }

    //Problem 1
    public override object ExpectedOutput1 => 11048;

    public override object Solve1(string input)
    {
        var maze = input.ToDictionary();
        return PathToEnd(maze).score;
    }

    //Problem 2
    public override object ExpectedOutput2 => 64;

    public override object Solve2(string input)
    {
        var maze = input.ToDictionary();
        return PathToEnd(maze, true).paths.Count();
    }
}