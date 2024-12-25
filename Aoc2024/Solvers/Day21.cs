using System.Text;
using AocHelpers;
using AocHelpers.Solvers;
using Point = (int x, int y);

public class Day21 : Solver
{
    public override int Day => 21;

    //Problem 1
    public override object ExpectedOutput1 => 126384l;

    private static Dictionary<char, Point> NumberPad = new()
    {
        { '7', (0, 0) },
        { '8', (1, 0) },
        { '9', (2, 0) },
        { '4', (0, 1) },
        { '5', (1, 1) },
        { '6', (2, 1) },
        { '1', (0, 2) },
        { '2', (1, 2) },
        { '3', (2, 2) },
        { '#', (0, 3) },
        { '0', (1, 3) },
        { 'A', (2, 3) },
    };

    private static Dictionary<char, Point> DirectionPad = new()
    {
        { '#', (0, 0) },
        { '^', (1, 0) },
        { 'A', (2, 0) },
        { '<', (0, 1) },
        { 'v', (1, 1) },
        { '>', (2, 1) },
    };

    private static Dictionary<(string, int), long> SequenceDepthLengthCache = new();

    private static string ShortestPath(Dictionary<char, Point> keypad, char start, char target)
    {
        
        var startPoint = keypad[start];
        var targetPoint = keypad[target];

        var ud = String.Join("", (targetPoint.y > startPoint.y) ? "vvvv".Take(targetPoint.y - startPoint.y) : "^^^^".Take(startPoint.y - targetPoint.y));
        var lr = String.Join("", (targetPoint.x > startPoint.x) ? ">>>>".Take(targetPoint.x - startPoint.x) : "<<<<".Take(startPoint.x - targetPoint.x));

        //Somehow the following order of attempting paths always gets the path with the shortest parent path
        if (targetPoint.x > startPoint.x && (startPoint.x, targetPoint.y) != keypad['#'])
        {
            // Safe to move vertically first if heading right and corner point isn't the gap
            return $"{ud}{lr}A";
        }

        if ((targetPoint.x, startPoint.y) != keypad['#'])
        {
            // Safe to move horizontally first if corner point isn't the gap
            return $"{lr}{ud}A";
        }

        // Must be safe to move vertically first because we can't be in same column as gap.
        return $"{ud}{lr}A";
    }

    private static long CalculateSequenceLength(Dictionary<char, Point> keypad, string sequence, int robotsAbove)
    {
        if (SequenceDepthLengthCache.TryGetValue((sequence, robotsAbove), out var length))
        {
            return length;
        }

        var currentKey = 'A';
        var totalLength = 0l;

        foreach (var targetKey in sequence)
        {
            var paths = ShortestPath(keypad, currentKey, targetKey);
            if (robotsAbove == 0)
            {
                totalLength += paths.Length;
            }
            else
            {
                totalLength += CalculateSequenceLength(DirectionPad, paths, robotsAbove - 1);
            }

            currentKey = targetKey;
        }


        SequenceDepthLengthCache[(sequence, robotsAbove)] = totalLength;
        return totalLength;
    }

    public override object Solve1(string input)
    {
        return input.SplitLines().Sum(s => int.Parse(s[..3]) * CalculateSequenceLength(NumberPad, s, 2));
    }

    //Problem 2
    public override object ExpectedOutput2 => 154115708116294l;

    public override object Solve2(string input)
    {
        return input.SplitLines().Sum(s => int.Parse(s[..3]) * CalculateSequenceLength(NumberPad, s, 25));
    }
}