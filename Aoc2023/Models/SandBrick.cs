using AocHelpers;

namespace Aoc2023.Models;

public class SandBrick
{
    public (int x, int y, int z)[] Points { get; }
    public (int x, int y, int z)[] BottomEdge { get; }
    public int MinZ { get; private set; }
    public int MaxZ { get; private set; }
    public List<SandBrick> BricksThatSupportThis = new();
    public List<SandBrick> BricksThisSupports = new();


    public int Id { get; set; }

    public SandBrick(string init, int id)
    {
        this.Id = id;
        var parts = init.SplitAndTrim("~");
        var start = parts.First().SplitCommas().ToArray();
        var startPoint = (int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]));
        var end = parts.Last().SplitCommas().ToArray();
        var endPoint = (int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2]));
        var current = startPoint;
        var vector = endPoint.Minus(startPoint).Clamp(-1, 1);
        var points = new List<(int, int, int)>();
        points.Add(current);
        while (current != endPoint)
        {
            current = current.Plus(vector);
            points.Add(current);
        }

        this.Points = points.ToArray();
        this.MinZ = this.Points.Min(p => p.z);
        this.MaxZ = this.Points.Max(p => p.z);
        BottomEdge = this.Points.Where(p => p.z == this.MinZ).ToArray();
    }

    public void Drop(int distance)
    {
        BricksThatSupportThis.Clear();
        foreach (var other in BricksThisSupports)
        {
            other.BricksThatSupportThis.Remove(this);
        }

        for (var i = 0; i < this.Points.Length; i++)
        {
            this.Points[i] = this.Points[i].Minus(0, 0, distance);
        }

        for (var i = 0; i < this.BottomEdge.Length; i++)
        {
            this.BottomEdge[i] = this.BottomEdge[i].Minus(0, 0, distance);
        }

        MinZ -= distance;
        this.MaxZ -= distance;
    }

    public IEnumerable<(int, int, int)> SupportPoints
    {
        get { return this.BottomEdge.Select(p => p.Minus(0, 0, 1)); }
    }

    public bool Contains((int, int, int) other)
    {
        return this.Points.Contains(other);
    }

    public void Supports(SandBrick other)
    {
        BricksThisSupports.Add(other);
        other.BricksThatSupportThis.Add(this);
    }
}

public class BrickStack
{
    private const int Ground = -1;
    public List<SandBrick> Bricks { get; set; }
    private Dictionary<int, List<SandBrick>> SupportDictionary { get; } = new();

    private bool FindSupport(SandBrick brick)
    {
        if (brick.MinZ == 1)
        {
            return true;
        }

        bool foundSupport = false;
        foreach (var other in this.Bricks.Where(o => o.MinZ < brick.MinZ))
        {
            if (other != brick)
            {
                if (brick.SupportPoints.Any(p => other.Contains(p)))
                {
                    other.Supports(brick);
                    foundSupport = true;
                }
            }
        }

        return foundSupport;
    }

    public BrickStack(List<SandBrick> bricks)
    {
        this.Bricks = bricks;
        DropBricks();
    }

    private void DropBricks()
    {
        foreach (var brick in this.Bricks.OrderBy(b => b.MinZ).ToList())
        {
            if (!FindSupport(brick))
            {
                DropBrick(brick);
                FindSupport(brick);
            }
        }
    }

    private void DropBrick(SandBrick brick)
    {
        var otherPoints = this.Bricks.Where(b => b.MaxZ < brick.MinZ).SelectMany(b => b.Points).ToHashSet();

        var bottomEdgeXy = brick.BottomEdge.Select(p => (p.x, p.y));
        var height = otherPoints.Where(p => bottomEdgeXy.Contains((p.x, p.y))).Select(p => p.z).DefaultIfEmpty(0).Max();
        brick.Drop(brick.MinZ - height - 1);
    }

    public int BricksThatWouldFallIfBrickWereGone(SandBrick brick)
    {
        var goneBricks = new List<SandBrick>() { brick };
        var remainingBricks = new List<SandBrick>(this.Bricks);
        remainingBricks.Remove(brick);
        var nextBricks = remainingBricks.Where(b => b.MinZ > 1 && b.BricksThatSupportThis.All(s => goneBricks.Contains(s))).ToList();
        while (nextBricks.Any())
        {
            goneBricks.AddRange(nextBricks);
            foreach (var b in nextBricks)
            {
                remainingBricks.Remove(b);
            }

            nextBricks = remainingBricks.Where(b => b.MinZ > 1 && b.BricksThatSupportThis.All(s => goneBricks.Contains(s))).ToList();
        }

        return goneBricks.Count - 1;
    }
}