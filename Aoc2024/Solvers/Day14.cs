using System.Text.RegularExpressions;
using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);
using Robot = ((int x, int y) position, (int x, int y) velocity);

public partial class Day14 : Solver
{
    public override int Day => 14;

    private static (int width, int height, List<Robot> robots) Setup(string input)
    {
        var (dimensionsText, robotsLines) = input.SplitLines().HeadAndTail();
        var width = int.Parse(dimensionsText.Split().First());
        var height = int.Parse(dimensionsText.Split().Last());
        var robots = robotsLines.Select(s =>
        {
            var match = InputRegex().Match(s);
            return ((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
        }).ToList();
        return (width, height, robots);
    }

    private static Robot MoveRobot(Robot robot, Point dimensions, int seconds)
    {
        var fullDistance = (robot.velocity.x * seconds, robot.velocity.y * seconds);
        var offMapPosition = robot.position.Plus(fullDistance);
        var newX = offMapPosition.x >= 0 ? offMapPosition.x % dimensions.x : offMapPosition.x + (Math.Abs(offMapPosition.x / dimensions.x) + (offMapPosition.x % dimensions.x == 0 ? 0 : 1)) * dimensions.x;
        var newY = offMapPosition.y >= 0 ? offMapPosition.y % dimensions.y : offMapPosition.y + (Math.Abs(offMapPosition.y / dimensions.y) + (offMapPosition.y % dimensions.y == 0 ? 0 : 1)) * dimensions.y;
        return ((newX, newY), robot.velocity);
    }

    //Problem 1
    public override object ExpectedOutput1 => 12;

    public override object Solve1(string input)
    {
        var (width, height, robots) = Setup(input);
        Point midpoint = (width / 2, height / 2);
        var movedRobots = robots.Select(r => MoveRobot(r, (width, height), 100)).ToList();
        return movedRobots.Count(r => r.position.x < midpoint.x && r.position.y < midpoint.y)
               * movedRobots.Count(r => r.position.x > midpoint.x && r.position.y < midpoint.y)
               * movedRobots.Count(r => r.position.x < midpoint.x && r.position.y > midpoint.y)
               * movedRobots.Count(r => r.position.x > midpoint.x && r.position.y > midpoint.y);
    }

    private static bool HasChristmasTree(int width,int height, List<Robot> robots)
    {
        var points = robots.Select(r => r.position).ToHashSet();
        var hits = 0;
        for (var y = 0; y < width; y++)
        {
            for (var x = 0; x < height; x++)
            {
                if (points.Contains((x, y)))
                {
                    hits++;
                    if (hits == 8)
                    {
                        return true;
                    }
                }
                else
                {
                    hits = 0;
                }
            }
        }

        return false;
    }

    //Problem 2
    public override object ExpectedOutput2 => base.ExpectedOutput2;

    public override object Solve2(string input)
    {
        var (width, height, robots) = Setup(input);
        var count = 0;
        var movedRobots = robots;
        while (!HasChristmasTree(width,height,movedRobots))
        {
            movedRobots = movedRobots.Select(r => MoveRobot(r, (width, height), 1)).ToList();
        }
        //Visualize(width, height, movedRobots);

        return count;
    }

    private static void Visualize(int width, int height, List<Robot> movedRobots)
    {
        var grid = new int[width, height];
        grid.Traverse((x, y, _) => { grid[x, y] = movedRobots.Count(r => r.position.x == x && r.position.y == y); });
        grid.Visualize();
    }


    [GeneratedRegex(@"p=(\d+),(\d+) v=([0-9-]+),([0-9-]+)")]
    private static partial Regex InputRegex();
}