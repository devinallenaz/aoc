using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day25 : Solver
{
    public override int Day => 25;

    //Problem 1
    public override object ExpectedOutput1 => 54;
    private Dictionary<(string, string), int> VisitedEdges { get; } = new();
    private Dictionary<string, Node<string>> Nodes { get; } = new();

    private void BuildNodes(string input)
    {
        foreach (var line in input.SplitLines())
        {
            var parts = line.SplitAndTrim(":");
            var thisNodeLabel = parts.First();
            var otherNodeLabels = parts.Last().Split();
            if (!Nodes.ContainsKey(thisNodeLabel))
            {
                Nodes[thisNodeLabel] = new Node<string>(thisNodeLabel);
            }

            foreach (var other in otherNodeLabels)
            {
                if (!Nodes.ContainsKey(other))
                {
                    Nodes[other] = new Node<string>(other);
                }

                Nodes[thisNodeLabel].AddNeighbor(Nodes[other], 1);
                Nodes[other].AddNeighbor(Nodes[thisNodeLabel], 1);
            }
        }
    }

    public List<Node<string>> ShortestPath(Node<string> start, Node<string> end)
    {
        var visited = new List<Node<string>>() { start };
        var pathCandidates = start.Neighbors.Select(n => new List<Node<string>>() { start, n.node }).ToList();
        visited.AddRange(pathCandidates.Select(n => n.Last()));
        while (pathCandidates.All(n => n.Last() != end))
        {
            var newPathCandidates = new List<List<Node<string>>>();
            foreach (var neighbor in pathCandidates)
            {
                foreach (var newNeighbor in neighbor.Last().Neighbors.Select(n => n.node))
                {
                    if (!visited.Contains(newNeighbor))
                    {
                        visited.Add(newNeighbor);
                        newPathCandidates.Add(neighbor.Append(newNeighbor).ToList());
                    }
                }
            }

            pathCandidates = newPathCandidates;
        }

        return pathCandidates.First(n => n.Last() == end);
    }

    private void LogEdge(string label1, string label2)
    {
        if (label1.CompareTo(label2) < 0)
        {
            LogEdge(label2, label1);
        }
        else
        {
            if (!this.VisitedEdges.ContainsKey((label1, label2)))
            {
                this.VisitedEdges[(label1, label2)] = 0;
            }

            this.VisitedEdges[(label1, label2)]++;
        }
    }

    public override object Solve1(string input)
    {
        Nodes.Clear();
        VisitedEdges.Clear();
        BuildNodes(input);
        var totalNodes = Nodes.Values.Count;

        foreach (var nodePair in Nodes.Values.OrderBy(n => n.Label).AllPairs().Take(10000))
        {
            var path = ShortestPath(nodePair.Item1, nodePair.Item2);
            for (int i = 0; i < path.Count - 1; i++)
            {
                LogEdge(path[i].Label, path[i + 1].Label);
            }
        }

        var top3Edges = VisitedEdges.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).Take(3).ToList();
        foreach (var (first, second) in top3Edges)
        {
            Nodes[first].RemoveNeighbor(Nodes[second]);
            Nodes[second].RemoveNeighbor(Nodes[first]);
        }

        var groupCount = Nodes.First().Value.AllNodes().Count;

        return (totalNodes - groupCount) * groupCount;
    }


    //Problem 2
    public override object ExpectedOutput2 => base.ExpectedOutput2;

    public override object Solve2(string input)
    {
        return base.Solve2(input);
    }
}