using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using AocHelpers;

namespace Aoc2023.Models;

public class Digger
{
    private List<DiggerInstruction> Instructions { get; }


    public Digger(string init, bool decode = false)
    {
        this.Instructions = init.SplitLines().Select(s => new DiggerInstruction(s, decode)).ToList();
    }

    public List<(long x, long y)> Dig()
    {
        (long x, long y) currentPosition = (0, 0);
        var output = new List<(long x, long y)>();
        bool bottomEdge = false;
        bool rightEdge = false;
        for (var i = 0; i < this.Instructions.Count; i++)
        {
            var current = this.Instructions[i];
            var next = this.Instructions[(i + 1) % this.Instructions.Count];
            var distance = current.Count;
            switch (current.DirectionName, next.DirectionName, rightEdge, bottomEdge)
            {
                case ("R", "D", false, false):
                    distance = distance + 1;
                    rightEdge = true;
                    break;

                case ("D", "L", true, false):
                    distance = distance + 1;
                    bottomEdge = true;
                    break;
                case ("D", "R", false, true):
                    distance = distance + 1;
                    bottomEdge = false;
                    break;
                case ("L", "U", true, true):
                    distance = distance + 1;
                    rightEdge = false;
                    break;
                case ("U", "R", false, true):
                {
                    distance = distance + 1;
                    bottomEdge = false;
                    break;
                }

                case ("U", "L", false, false):
                    distance = distance - 1;
                    bottomEdge = true;
                    break;
                case ("L", "D", false, true):
                    distance = distance - 1;
                    rightEdge = true;
                    break;
                case ("R", "U", true, false):
                    distance = distance - 1;
                    rightEdge = false;
                    break;
                case ("D", "L", false, false):
                    distance = distance - 1;
                    bottomEdge = true;
                    break;
                case ("D", "R", true, true):
                    distance = distance - 1;
                    bottomEdge = false;
                    break;
                case ("R", "D", true, true):
                    distance = distance - 1;
                    rightEdge = false;
                    break;
            }


            currentPosition = currentPosition.Plus((current.Direction.x * distance, current.Direction.y * distance));


            output.Add(currentPosition);
        }

        return output;
    }

    public class DiggerInstruction
    {
        private static Regex _regex = new Regex(@"([UDLR]) (\d+) \(#(\w\w\w\w\w\w)\)");
        public (long x, long y) Direction { get; }
        public string DirectionName { get; }
        public long Count { get; }

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
                this.DirectionName = groups[1].Value;
                this.Count = long.Parse(groups[2].Value);
            }
            else
            {
                var encoded = groups[3].Value;
                var distance = encoded.Substring(0, 5);
                var direction = encoded.Substring(5, 1);

                this.Direction = direction switch
                {
                    "0" => Points.Right,
                    "1" => Points.Down,
                    "2" => Points.Left,
                    "3" => Points.Up,
                    _ => throw new InvalidOperationException(),
                };
                this.DirectionName = direction switch
                {
                    "0" => "R",
                    "1" => "D",
                    "2" => "L",
                    "3" => "U",
                    _ => throw new InvalidOperationException(),
                };
                this.Count = long.Parse(distance, NumberStyles.HexNumber);
            }
        }
    }
}