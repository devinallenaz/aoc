using AocHelpers;
using AocHelpers.Solvers;

public class Day10 : Solver
{
    public override int Day => 10;

    //Problem 1
    public override object ExpectedOutput1 => 8;

    private static bool CanGoUp(char[,] maze, (int x, int y) point)
    {
        switch (maze.ValueOrNull(point.x, point.y))
        {
            case 'S':
                switch (maze.ValueOrNull(point.x, point.y - 1))
                {
                    case '|':
                    case '7':
                    case 'F':
                        return true;
                    default:
                        return false;
                }
            case '|':
            case 'J':
            case 'L':
                return true;
            default: return false;
        }
    }

    private static bool CanGoDown(char[,] maze, (int x, int y) point)
    {
        switch (maze.ValueOrNull(point.x, point.y))
        {
            case 'S':
                switch (maze.ValueOrNull(point.x, point.y + 1))
                {
                    case '|':
                    case 'J':
                    case 'L':
                        return true;
                    default:
                        return false;
                }

            case '|':
            case '7':
            case 'F':
                return true;
            default:
                return false;
        }
    }

    private static bool CanGoLeft(char[,] maze, (int x, int y) point)
    {
        switch (maze.ValueOrNull(point.x, point.y))
        {
            case 'S':
                switch (maze.ValueOrNull(point.x - 1, point.y))
                {
                    case '-':
                    case 'F':
                    case 'L':
                        return true;
                    default:
                        return false;
                }
            case '-':
            case 'J':
            case '7':
                return true;
            default:
                return false;
        }
    }

    private static bool CanGoRight(char[,] maze, (int x, int y) point)
    {
        switch (maze.ValueOrNull(point.x, point.y))
        {
            case 'S':
                switch (maze.ValueOrNull(point.x + 1, point.y))
                {
                    case '-':
                    case 'J':
                    case '7':
                        return true;
                    default:
                        return false;
                }
            case '-':
            case 'F':
            case 'L':
                return true;
            default:
                return false;
        }
    }

    public int StepsToStart(char[,] maze, (int x, int y) pos, int steps, char? from = null, List<(int x, int y)>? path = null)
    {
        if (steps != 0 && maze.ValueOrNull(pos.x, pos.y) == 'S')
        {
            return steps;
        }

        path?.Add(pos);

        steps++;
        if (from != 'U' && CanGoUp(maze, pos))
        {
            return StepsToStart(maze, pos.Plus((0, -1)), steps, 'D', path);
        }

        if (from != 'D' && CanGoDown(maze, pos))
        {
            return StepsToStart(maze, pos.Plus((0, 1)), steps, 'U', path);
        }

        if (from != 'L' && CanGoLeft(maze, pos))
        {
            return StepsToStart(maze, pos.Plus((-1, 0)), steps, 'R', path);
        }

        if (from != 'R' && CanGoRight(maze, pos))
        {
            return StepsToStart(maze, pos.Plus((1, 0)), steps, 'L', path);
        }

        throw new NotImplementedException();
    }

    public override object Solve1(string input)
    {
        var maze = input.To2dCharArray();
        var start = maze.Find('S');
        var path = new List<(int x, int y)>();
        return StepsToStart(maze, start, 0) / 2;
    }

    //Problem 2
    public override object ExpectedOutput2 => 10;

    public override object Solve2(string input)
    {
        var maze = input.To2dCharArray();
        var start = maze.Find('S');

        var path = new List<(int x, int y)>();
        StepsToStart(maze, start, 0, null, path);
        FixStart(maze, start);
        var count = 0;
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            bool inside = false;
            var recentIntersect = (char?)null;
            for (int y = maze.GetLength(1) - 1; y >= 0; y--)
            {
                if (!path.Contains((x, y)))
                {
                    if (inside)
                    {
                        count++;
                    }
                }
                else
                {
                    var val = maze.ValueOrNull(x, y);
                    switch (val)
                    {
                        case '-':
                            inside = !inside;
                            break;
                        case 'L':
                            recentIntersect = 'L';
                            inside = !inside;
                            break;
                        case 'J':
                            recentIntersect = 'J';
                            inside = !inside;
                            break;
                        case 'F':
                            if (recentIntersect != 'J')
                            {
                                inside = !inside;
                            }

                            recentIntersect = null;

                            break;

                        case '7':
                            if (recentIntersect != 'L')
                            {
                                inside = !inside;
                            }

                            recentIntersect = null;


                            break;
                    }
                }
            }
        }

        return count;
    }

    private void FixStart(char[,] maze, (int x, int y) start)
    {
        if (CanGoUp(maze, start))
        {
            if (CanGoDown(maze, start))
            {
                maze[start.x, start.y] = '|';
            }

            if (CanGoLeft(maze, start))
            {
                maze[start.x, start.y] = 'J';
            }

            if (CanGoRight(maze, start))
            {
                maze[start.x, start.y] = 'L';
            }
        }

        if (CanGoDown(maze, start))
        {
            if (CanGoUp(maze, start))
            {
                maze[start.x, start.y] = '|';
            }

            if (CanGoLeft(maze, start))
            {
                maze[start.x, start.y] = '7';
            }

            if (CanGoRight(maze, start))
            {
                maze[start.x, start.y] = 'F';
            }
        }

        if (CanGoLeft(maze, start))
        {
            if (CanGoUp(maze, start))
            {
                maze[start.x, start.y] = 'J';
            }

            if (CanGoDown(maze, start))
            {
                maze[start.x, start.y] = '7';
            }

            if (CanGoRight(maze, start))
            {
                maze[start.x, start.y] = '-';
            }
        }

        if (CanGoRight(maze, start))
        {
            if (CanGoUp(maze, start))
            {
                maze[start.x, start.y] = 'L';
            }

            if (CanGoDown(maze, start))
            {
                maze[start.x, start.y] = 'F';
            }

            if (CanGoLeft(maze, start))
            {
                maze[start.x, start.y] = '-';
            }
        }
    }

    private void VisualizeMaze(char[,] maze, List<(int x, int y)> path, List<(int x, int y)> inside)
    {
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            for (int x = 0; x < maze.GetLength(0); x++)
            {
                if (path.Contains((x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else if (inside.Contains((x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                Console.Write(maze[x, y]);
            }

            Console.Write('\n');
        }
    }
}