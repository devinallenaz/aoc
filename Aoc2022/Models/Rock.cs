using Aoc2022.Helpers;
using AocHelpers;

namespace Aoc2022.Models;

public struct Rock
{
    /// <summary>
    /// ####
    /// </summary>
    public static (int, long)[] HorizontalShape = { (0, 0), (1, 0), (2, 0), (3, 0) };

    /// <summary>
    /// .#.
    /// ###
    /// .#.
    /// </summary>
    public static (int, long)[] PlusShape = { (0, 1), (1, 0), (1, 1), (2, 1), (1, 2) };

    /// <summary>
    /// ..#
    /// ..#
    /// ###
    /// </summary>
    public static (int, long)[] LShape = { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) };

    /// <summary>
    /// #
    /// #
    /// #
    /// #
    /// </summary>
    public static (int, long)[] VerticalShape = { (0, 0), (0, 1), (0, 2), (0, 3) };

    /// <summary>
    /// ##
    /// ##
    /// </summary>
    public static (int, long)[] SquareShape = { (0, 0), (0, 1), (1, 0), (1, 1) };


    public Rock((int x, long y) origin, (int, long)[] shape)
    {
        Origin = origin;
        this.Shape = shape;
        this.PointsCovered = shape.Select(p => origin.Plus(p)).ToArray();
    }

    private (int x, long y) Origin { get; }
    private (int x, long y)[] Shape { get; }
    public IEnumerable<(int x, long y)> PointsCovered { get; }

    public int MinX
    {
        get
        {
            return this.PointsCovered.Min(p => p.x);
        }
    }
    public int MaxX
    {
        get
        {
            return this.PointsCovered.Max(p => p.x);
        }
    }

    public long MaxY
    {
        get
        {
            return this.PointsCovered.Max(p => p.y);
        }
    }

    public bool Overlaps(Rock other)
    {
        return this.PointsCovered.Any(p => other.PointsCovered.Contains(p));
    }

    public Rock MoveDown()
    {
        return new Rock(this.Origin.Plus((0, -1)), this.Shape);
    }

    public Rock MoveLeft()
    {
        return new Rock(this.Origin.Plus((-1, 0)), this.Shape);
    }

    public Rock MoveRight()
    {
        return new Rock(this.Origin.Plus((1, 0)), this.Shape);
    }

    public bool Equals(object other)
    {
        if (other is Rock rock)
        {
            var pointsCovered = this.PointsCovered;
            return pointsCovered.All(p => rock.PointsCovered.Contains(p)) && rock.PointsCovered.All(p => pointsCovered.Contains(p));
        }

        return base.Equals(other);
    }
    
}