namespace Aoc2023.Models;

public class Node
{
    public List<(Node node, int distance)> Neighbors { get; } = new();
    public (int x, int y) Location { get; }

    public Node((int, int) location)
    {
        this.Location = location;
    }


    public void AddNeighbor(Node node, int distance)
    {
        if (this.Neighbors.All(n => n.node != node))
        {
            this.Neighbors.Add((node, distance));
        }
    }
}