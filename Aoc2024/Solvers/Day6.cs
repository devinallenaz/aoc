using AocHelpers;
using AocHelpers.Solvers;

public class Day6 : Solver
{
    public override int Day => 6;

    //Problem 1
    public override object ExpectedOutput1 => 41;

    private bool Patrol(char[,] grid, (int x, int y) currentPosition, (int x, int y) currentDirection, List<(int x, int y, int dx, int dy)>? pointsPatrolled = null)
    {
        pointsPatrolled ??= new();
        while (grid.ValueOrNull(currentPosition.x, currentPosition.y) != null)
        {
            if (pointsPatrolled.Contains((currentPosition.x, currentPosition.y, currentDirection.x, currentDirection.y)))
            {
                return true;
            }

            pointsPatrolled.Add((currentPosition.x, currentPosition.y, currentDirection.x, currentDirection.y));
            while (grid.ValueOrNull(currentPosition.Plus(currentDirection)) == '#')
            {
                currentDirection = currentDirection.RotateClockwise();
            }

            currentPosition = currentPosition.Plus(currentDirection);
        }

        return false;
    }

    public override object Solve1(string input)
    {
        var (grid, currentPosition, currentDirection) = Setup(input);
        var pointsPatrolled = new List<(int x, int y, int dx, int dy)>();
        Patrol(grid, currentPosition, currentDirection, pointsPatrolled);

        return pointsPatrolled.DistinctBy(p => (p.x, p.y)).Count();
    }

    //Problem 2
    public override object ExpectedOutput2 => 6;

    public override object Solve2(string input)
    {
        var (grid, currentPosition, currentDirection) = Setup(input);
        var startingPosition = currentPosition;
        var pointsPatrolled = new List<(int x, int y, int dx, int dy)>();
        Patrol(grid, currentPosition, currentDirection, pointsPatrolled);
        return pointsPatrolled.DistinctBy(p => (p.x, p.y)).Sum(point =>
        {
            if ((point.x, point.y) == startingPosition)
            {
                return 0;
            }

            grid[point.x, point.y] = '#';
            if (Patrol(grid, currentPosition, currentDirection))
            {
                grid[point.x, point.y] = '.';
                return 1;
            }
            else
            {
                grid[point.x, point.y] = '.';
                return 0;
            }
        });
    }

    private static (char[,] grid, (int x, int y) currentPosition, (int x, int y) currentDirection) Setup(string input)
    {
        var grid = input.To2dCharArray();
        (int x, int y) currentPosition = (0, 0);
        var currentDirection = Points.North;
        grid.Traverse((x, y, c) =>
        {
            if (c == '^')
            {
                currentPosition = (x, y);
            }
        });
        return (grid, currentPosition, currentDirection);
    }
}