using AocHelpers.Models;

namespace Aoc2022.Models;

public enum Direction
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3,
}

public class Character
{
    public Tile CurrentTile { get; private set; }

    public List<Action> Moves { get; } = new List<Action>();

    public RingItem<Direction> CurrentFacing { get; set; }

    public Character(Tile currentTile, List<string> moves)
    {
        this.CurrentTile = currentTile;
        CurrentFacing = new Ring<Direction>(new[]
        {
            Direction.Right,
            Direction.Down,
            Direction.Left,
            Direction.Up,
        }).Head;


        foreach (var move in moves)
        {
            switch (move)
            {
                case "R":
                    this.Moves.Add(TurnRight);
                    break;
                case "L":
                    this.Moves.Add(TurnLeft);
                    break;
                default:
                    var times = int.Parse(move);
                    for (var i = 0; i < times; i++)
                    {
                        this.Moves.Add(() => MoveForward());
                    }

                    break;
            }
        }
    }

    public void TurnLeft()
    {
        this.CurrentFacing = this.CurrentFacing.Previous;
    }

    public void TurnRight()
    {
        this.CurrentFacing = this.CurrentFacing.Next;
    }

    public void Noop()
    {
    }

    public void MoveForward(bool fly = false, int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            this.MoveTo(this.CurrentFacing.Value, fly);
        }
    }

    public void MoveTo(Direction direction, bool fly = false)
    {
        Tile tile;
        Action<Character>? skew;
        switch (direction)
        {
            case Direction.Right:
                tile = this.CurrentTile.RightNeighbor;
                skew = this.CurrentTile.RightNeighborSkew;
                break;
            case Direction.Down:
                tile = this.CurrentTile.DownNeighbor;
                skew = this.CurrentTile.DownNeighborSkew;
                break;
            case Direction.Left:
                tile = this.CurrentTile.LeftNeighbor;
                skew = this.CurrentTile.LeftNeighborSkew;
                break;
            case Direction.Up:
                tile = this.CurrentTile.UpNeighbor;
                skew = this.CurrentTile.UpNeighborSkew;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        if (!tile.HasRock || fly)
        {
            this.CurrentTile = tile;


            if (skew != null)
            {
                skew(this);
            }
        }
    }

    public void DoAllMoves()
    {
        foreach (var move in this.Moves)
        {
            move();
        }
    }
}