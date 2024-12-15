using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);

public class Day15 : Solver
{
    public override int Day => 15;

    //Problem 1
    public override object ExpectedOutput1 => 10092;

    public bool Move(char[,] grid, Point from, Point direction)
    {

        var snapShot = (char[,])grid.Clone();
        var current = grid[from.x, from.y];
        if (current == '.')
        {
            return true;
        }

        if (current == '#')
        {
            return false;
        }

        var next = from.Plus(direction);
        if (direction == Points.Up || direction == Points.Down)
        {
            if (current == '[')
            {
                var otherFrom = from.Plus(Points.Right);
                var otherNext = otherFrom.Plus(direction);
                if (Move(grid, next, direction))
                {
                    if (Move(grid, otherNext, direction))
                    {
                        grid[next.x, next.y] = current;
                        grid[otherNext.x, next.y] = ']';
                        grid[from.x, from.y] = '.';
                        grid[otherFrom.x, otherFrom.y] = '.';
                        return true;
                    }

                    Array.Copy(snapShot, grid, snapShot.Length);
                }

                return false;
            }

            if (current == ']')
            {
                var otherFrom = from.Plus(Points.Left);
                var otherNext = otherFrom.Plus(direction);
                if (Move(grid, next, direction))
                {
                    if (Move(grid, otherNext, direction))
                    {
                        grid[next.x, next.y] = current;
                        grid[otherNext.x, next.y] = '[';
                        grid[from.x, from.y] = '.';
                        grid[otherFrom.x, otherFrom.y] = '.';
                        return true;
                    }
                    Array.Copy(snapShot, grid, snapShot.Length);
                }

                return false;
            }
        }

        if (Move(grid, next, direction))
        {
            grid[next.x, next.y] = current;
            grid[from.x, from.y] = '.';
            return true;
        }

        return false;
    }

    public override object Solve1(string input)
    {
        var sections = input.SplitSections();
        var grid = sections.First().To2dCharArray();
        var instructions = sections.Last();
        DoInstructions(grid, instructions);

        var sum = 0;
        grid.Traverse((x, y, c) =>
        {
            if (c == 'O')
            {
                sum += x + (100 * y);
            }
        });
        return sum;
    }

    private void DoInstructions(char[,] grid, string instructions,bool visualize = false)
    {
        var robot = grid.Find('@');
        foreach (var instruction in instructions)
        {
            var direction = instruction switch
            {
                '^' => Points.Up,
                '>' => Points.Right,
                '<' => Points.Left,
                'v' => Points.Down,
                _ => throw new NotImplementedException(),
            };
            if (Move(grid, robot, direction))
            {
                robot = robot.Plus(direction);
            }

            if (visualize)
            {
                Console.WriteLine(instruction);
                grid.Visualize();
            }
        }
    }

    //Problem 2
    public override object ExpectedOutput2 => 9021;

    public override object Solve2(string input)
    {
        var sections = input.SplitSections();
        var lines = sections.First().SplitLines();
        var width = lines.First().Length;
        var height = lines.Count();
        var grid = new char[width * 2, height];
        foreach (var (line, y) in lines.WithIndex())
        {
            foreach (var (c, x) in line.WithIndex())
            {
                switch (c)
                {
                    case '@':
                        grid[2 * x, y] = '@';
                        grid[2 * x + 1, y] = '.';
                        break;
                    case 'O':
                        grid[2 * x, y] = '[';
                        grid[2 * x + 1, y] = ']';
                        break;
                    default:
                        grid[2 * x, y] = c;
                        grid[2 * x + 1, y] = c;
                        break;
                }
            }
        }

        var instructions = sections.Last();
        DoInstructions(grid, instructions);

        var sum = 0;
        grid.Traverse((x, y, c) =>
        {
            if (c == '[')
            {
                sum += x + (100 * y);
            }
        });
        return sum;
    }
}