using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using AocHelpers;

namespace Aoc2023.Models;

public enum CornerType
{
    None,
    UL,
    UR,
    DL,
    DR,
}

public class Digger
{
    private List<DiggerInstruction> Instructions { get; }


    public Digger(string init)
    {
        this.Instructions = init.SplitLines().Select(s => new DiggerInstruction(s)).ToList();
    }

    public List<DugPoint> Dig()
    {
        (int x, int y) currentPosition = (0, 0);
        var output = new List<DugPoint>();

        for (var i = 0; i < this.Instructions.Count; i++)
        {
            var diggerInstruction = this.Instructions[i];
            var cornerType = CornerType.None;
            for (int j = 0; j < diggerInstruction.Count; j++)
            {
                if (j == diggerInstruction.Count - 1)
                {
                    cornerType = this.CalculateCornerType(diggerInstruction, this.Instructions[(i + 1) % this.Instructions.Count]);
                }

                currentPosition = currentPosition.Plus(diggerInstruction.Direction);
                output.Add(new DugPoint(currentPosition.x, currentPosition.y, diggerInstruction.Color, cornerType));
            }
        }

        return output;
    }

    private CornerType CalculateCornerType(DiggerInstruction current, DiggerInstruction next)
    {
        switch (current.Direction, next.Direction)
        {
            case ((1, 0), (0, 1)):
                return CornerType.DL;
            case ((1, 0), (0, -1)):
                return CornerType.UL;
            case ((-1, 0), (0, 1)):
                return CornerType.DR;
            case ((-1, 0), (0, -1)):
                return CornerType.UR;
            case ((0, 1), (1, 0)):
                return CornerType.UR;
            case ((0, 1), (-1, 0)):
                return CornerType.UL;
            case ((0, -1), (1, 0)):
                return CornerType.DR;
            case ((0, -1), (-1, 0)):
                return CornerType.DL;
        }

        throw new InvalidOperationException();
    }
}

public class DiggerInstruction
{
    private static Regex _regex = new Regex(@"([UDLR]) (\d+) \(#(\w\w\w\w\w\w)\)");
    public (int, int) Direction { get; }
    public int Count { get; }

    public DiggerInstruction(string init, bool decode = false)
    {
        var match = _regex.Match(init);
        var groups = match.Groups;
        if (!decode)
        {
            this.Direction = groups[1].Value switch
            {
                "U" => Points.Up,
                "D" => Points.Down,
                "R" => Points.Right,
                "L" => Points.Left,
                _ => throw new InvalidOperationException(),
            };
            this.Count = int.Parse(groups[2].Value);
        }
        else
        {
            var encoded = groups[3].Value;
            var distance = encoded.Substring(0, 5);
            var direction = encoded.Substring(5, 1);
            this.Count = int.Parse(distance, NumberStyles.HexNumber);
        }

        
    }
}

public record DugPoint(int X, int Y, string Color, CornerType CornerType)
{
}