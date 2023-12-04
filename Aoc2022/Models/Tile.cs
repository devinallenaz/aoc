using System.Drawing;

namespace Aoc2022.Models;

public class Tile
{
    public (int x, int y) Coordinates { get; }
    public bool HasRock { get; }

    public ConsoleColor Color { get; } = ConsoleColor.Black;

    public ConsoleColor DrawColor
    {
        get
        {
            if (this.LeftNeighbor.Color != this.Color)
            {
                return this.LeftNeighbor.Color;
            }
            if (this.RightNeighbor.Color != this.Color)
            {
                return this.RightNeighbor.Color;
            }
            if (this.UpNeighbor.Color != this.Color)
            {
                return this.UpNeighbor.Color;
            }
            if (this.DownNeighbor.Color != this.Color)
            {
                return this.DownNeighbor.Color;
            }

            return this.Color;
        }
    }

    public Tile((int x, int y) coordinates, bool hasRock)
    {
        Coordinates = coordinates;
        HasRock = hasRock;
        if (coordinates.x >= 51 && coordinates.x <= 100 && coordinates.y >= 1 && coordinates.y <= 50)
        {
            this.Color = ConsoleColor.Magenta;
        }

        if (coordinates.x >= 101 && coordinates.x <= 150 && coordinates.y >= 1 && coordinates.y <= 50)
        {
            this.Color = ConsoleColor.Green;
        }

        if (coordinates.x >= 51 && coordinates.x <= 100 && coordinates.y >= 51 && coordinates.y <= 100)
        {
            this.Color = ConsoleColor.Cyan;
        }

        if (coordinates.x >= 51 && coordinates.x <= 100 && coordinates.y >= 101 && coordinates.y <= 150)
        {
            this.Color = ConsoleColor.Yellow;
        }

        if (coordinates.x >= 1 && coordinates.x <= 50 && coordinates.y >= 101 && coordinates.y <= 150)
        {
            this.Color = ConsoleColor.Blue;
        }

        if (coordinates.x >= 1 && coordinates.x <= 50 && coordinates.y >= 151 && coordinates.y <= 200)
        {
            this.Color = ConsoleColor.Red;
        }
    }

    private Tile? _leftNeighbor;
    private Tile? _upNeighbor;
    private Tile? _downNeighbor;
    private Tile? _rightNeighbor;

    public void SetLeftNeighbor(Tile tile, Action<Character>? skew = null)
    {
        _leftNeighbor = tile;
        LeftNeighborSkew = skew;
    }

    public void SetUpNeighbor(Tile tile, Action<Character>? skew = null)
    {
        _upNeighbor = tile;
        UpNeighborSkew = skew;
    }

    public void SetDownNeighbor(Tile tile, Action<Character>? skew = null)
    {
        _downNeighbor = tile;
        DownNeighborSkew = skew;
    }

    public void SetRightNeighbor(Tile tile, Action<Character>? skew = null)
    {
        _rightNeighbor = tile;
        RightNeighborSkew = skew;
    }

    public Tile LeftNeighbor => _leftNeighbor ?? throw new ApplicationException("invalid initialization");
    public Tile UpNeighbor => _upNeighbor ?? throw new ApplicationException("invalid initialization");
    public Tile DownNeighbor => _downNeighbor ?? throw new ApplicationException("invalid initialization");
    public Tile RightNeighbor => _rightNeighbor ?? throw new ApplicationException("invalid initialization");

    public Action<Character>? RightNeighborSkew { get; set; }
    public Action<Character>? UpNeighborSkew { get; set; }

    public Action<Character>? LeftNeighborSkew { get; set; }
    public Action<Character>? DownNeighborSkew { get; set; }
}