namespace Aoc2023.Models;

public class Node<T>(T location)
{
    public List<(Node<T> node, int distance)> Neighbors { get; } = [];
    public T Label { get; } = location;


    public void AddNeighbor(Node<T> node, int distance)
    {
        if (this.Neighbors.All(n => n.node != node))
        {
            this.Neighbors.Add((node, distance));
        }
    }

    public void RemoveNeighbor(Node<T> other)
    {
        var nodeToRemove = this.Neighbors.First(n => n.node == other);

        this.Neighbors.Remove(nodeToRemove);
    }

    public List<Node<T>> AllNodes()
    {
        var knownNodes = new List<Node<T>> { this };
        var allNeighbors = this.Neighbors.Select(n => n.node).ToList();
        while (allNeighbors.Any())
        {
            knownNodes.AddRange(allNeighbors);
            allNeighbors = allNeighbors.SelectMany(n => n.Neighbors.Select(n => n.node)).Where(n => !knownNodes.Contains(n)).Distinct().ToList();
        }

        return knownNodes;
    }
}