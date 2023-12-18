namespace Aoc2023.Models;

public class LightBeam
{
    public LightBeam((int, int) position, (int, int) direction)
    {
        this.Position = position;
        this.Direction = direction;
    }

    public (int x, int y) Position { get; set; }
    public (int x, int y) Direction { get; set; }
}