using AocHelpers;
using AocHelpers.Solvers;

public class Day14 : Solver
{
    public override int Day => 14;

    //Problem 1
    public override object ExpectedOutput1 => 136;

    private void ShiftNorth(char[,] platform)
    {
        for (var x = 0; x < platform.GetLength(0); x++)
        {
            for (var y = 0; y < platform.GetLength(1); y++)
            {
                if (platform[x, y] == 'O')
                {
                    (int x, int y) current = (x, y);
                    (int x, int y) next = current.Plus(Points.North);
                    while (platform.ValueOrNull(next.x, next.y) == '.')
                    {
                        platform[next.x, next.y] = 'O';
                        platform[current.x, current.y] = '.';
                        current = next;
                        next = current.Plus(Points.North);
                    }
                }
            }
        }
    }

    private void ShiftSouth(char[,] platform)
    {
        for (var x = 0; x < platform.GetLength(0); x++)
        {
            for (var y = platform.GetLength(1) - 1; y >= 0; y--)
            {
                if (platform[x, y] == 'O')
                {
                    (int x, int y) current = (x, y);
                    (int x, int y) next = current.Plus(Points.South);
                    while (platform.ValueOrNull(next.x, next.y) == '.')
                    {
                        platform[next.x, next.y] = 'O';
                        platform[current.x, current.y] = '.';
                        current = next;
                        next = current.Plus(Points.South);
                    }
                }
            }
        }
    }

    private void ShiftWest(char[,] platform)
    {
        for (var y = 0; y < platform.GetLength(1); y++)
        {
            for (var x = 0; x < platform.GetLength(0); x++)
            {
                if (platform[x, y] == 'O')
                {
                    (int x, int y) current = (x, y);
                    (int x, int y) next = current.Plus(Points.West);
                    while (platform.ValueOrNull(next.x, next.y) == '.')
                    {
                        platform[next.x, next.y] = 'O';
                        platform[current.x, current.y] = '.';
                        current = next;
                        next = current.Plus(Points.West);
                    }
                }
            }
        }
    }

    private void ShiftEast(char[,] platform)
    {
        for (var y = 0; y < platform.GetLength(1); y++)
        {
            for (var x = platform.GetLength(0) - 1; x >= 0; x--)
            {
                if (platform[x, y] == 'O')
                {
                    (int x, int y) current = (x, y);
                    (int x, int y) next = current.Plus(Points.East);
                    while (platform.ValueOrNull(next.x, next.y) == '.')
                    {
                        platform[next.x, next.y] = 'O';
                        platform[current.x, current.y] = '.';
                        current = next;
                        next = current.Plus(Points.East);
                    }
                }
            }
        }
    }

    private void Cycle(char[,] platform)
    {
        ShiftNorth(platform);
        ShiftWest(platform);
        ShiftSouth(platform);
        ShiftEast(platform);
    }

    private (int, bool[]) CalculateLoadAndBoolify(char[,] platform)
    {
        var platformHeight = platform.GetLength(1);
        var platformWidth = platform.GetLength(1);

        var boolifiedArray = new bool[platformWidth * platformHeight];
        var totalLoad = 0;
        platform.Traverse((x, y, c) =>
        {
            if (c == 'O')
            {
                boolifiedArray[(y * platformHeight) + x] = true;
                totalLoad += platformHeight - y;
            }
        });
        return (totalLoad, boolifiedArray);
    }


    public override object Solve1(string input)
    {
        var platform = input.To2dCharArray();

        ShiftNorth(platform);
        var (load, _) = CalculateLoadAndBoolify(platform);

        return load;
    }

    //Problem 2
    public override object ExpectedOutput2 => 64;

    public override object Solve2(string input)
    {
        var platform = input.To2dCharArray();
        var count = 0;
        var cache = new List<(int count, int load, bool[] boolifiedArray)>();
        while (true)
        {
            var (l, b) = CalculateLoadAndBoolify(platform);
            var match = cache.FirstOrDefault(c => c.load == l && c.boolifiedArray.SequenceEqual(b));
            if (match != default((int, int, bool[])))
            {
                var period = count - match.count;
                var answerIndex = 1_000_000_000L % period;
                while (answerIndex < match.count)
                {
                    answerIndex += period;
                }
                return cache.First(c => c.count ==answerIndex).load;
            }

            Cycle(platform);
            cache.Add((count, l, b));
            count++;
        }
    }
}