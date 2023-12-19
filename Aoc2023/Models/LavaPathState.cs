using AocHelpers;

namespace Aoc2023.Models;

public class LavaPathState
{
    private static List<(int, int)> FourDirections { get; } = new List<(int, int)>()
    {
        Points.Up,
        Points.Down,
        Points.Left,
        Points.Right,
    };

    public int HeatLoss { get; }
    public (int x, int y) Position { get; }
    public LavaPathState? Previous { get; }
    public (int x, int y) ArrivalVector { get; }
    public int ArrivalVectorCount { get; }
    private int[,] Map { get; }

    public LavaPathState(int[,] map, (int x, int y) position, int arrivalVectorCount = 1, LavaPathState? previous = null)
    {
        this.Previous = previous;
        this.HeatLoss = previous != null ? previous.HeatLoss + map[position.x, position.y] : 0;
        this.Position = position;
        this.ArrivalVectorCount = arrivalVectorCount;
        if (this.Previous != null)
        {
            this.ArrivalVector = this.Position.Minus(this.Previous.Position);
        }

        this.Map = map;
    }

    public bool HasVisited((int x, int y) position)
    {
        var state = this;
        while (state != null)
        {
            if (state.Position == position)
            {
                return true;
            }

            state = state.Previous;
        }

        return false;
    }

    public List<LavaPathState> NextStates(bool ultra = false)
    {
        var output = new List<LavaPathState>();

        foreach (var dir in FourDirections)
        {
            var arrivalVectorCount = 1;
            if (dir.Plus(ArrivalVector) == (0, 0))
            {
                continue;
            }

            if (dir == ArrivalVector)
            {
                if ((!ultra && ArrivalVectorCount == 3) || (ultra && ArrivalVectorCount == 10))
                {
                    continue;
                }

                arrivalVectorCount = this.ArrivalVectorCount + 1;
            }
            else if (ultra && this.ArrivalVectorCount < 4)
            {
                continue;
            }

            var newPosition = this.Position.Plus(dir);
            if (Map.ValueOrNull(newPosition) == null)
            {
                continue;
            }

            output.Add(new LavaPathState(Map, this.Position.Plus(dir), arrivalVectorCount, this));
        }

        return output;
    }
}