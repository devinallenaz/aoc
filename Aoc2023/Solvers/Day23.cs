using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day23 : Solver
{
    public override int Day => 23;

    //Problem 1
    public override object ExpectedOutput1 => 94;

    private Dictionary<(int, int), Node<(int,int)>> BuildGraph(char[,] forest, bool respectSlopes = true)
    {
        var (start, end) = FindStartEnd(forest);
        var graph = new Dictionary<(int, int), Node<(int,int)>>();
        graph.Add(start, new Node<(int,int)>(start));
        graph.Add(end, new Node<(int,int)>(end));
        foreach (var next in NextPositions(forest, start, null, respectSlopes))
        {
            FollowPath(forest, graph, start, next, respectSlopes);
        }

        return graph;
    }

    private Node<(int,int)>? FollowPath(char[,] forest, Dictionary<(int, int), Node<(int,int)>> graph, (int, int) fromNodePosition, (int x, int y) dir, bool respectSlopes = true)
    {
        var previousPosition = fromNodePosition;
        var currentPosition = dir;
        var nextPositions = NextPositions(forest, currentPosition, previousPosition, respectSlopes);
        var steps = 1;
        while (nextPositions.Count == 1)
        {
            previousPosition = currentPosition;
            currentPosition = nextPositions.Single();
            nextPositions = NextPositions(forest, currentPosition, previousPosition, respectSlopes);
            steps++;
        }

        if (!graph.ContainsKey(currentPosition))
        {
            var node = new Node<(int,int)>(currentPosition);
            graph[currentPosition] = node;
            nextPositions.Add(previousPosition);
            foreach (var nextDir in nextPositions)
            {
                FollowPath(forest, graph, currentPosition, nextDir, respectSlopes);
            }
        }
        graph[fromNodePosition].AddNeighbor(graph[currentPosition], steps);
        return graph[currentPosition];
    }

    private List<(int, int)> NextPositions(char[,] forest, (int x, int y) currentPosition, (int, int)? previousPosition = null, bool respectSlopes = true)
    {
        IEnumerable<(int, int)> next;
        if (respectSlopes)
        {
            switch (forest[currentPosition.x, currentPosition.y])
            {
                case '>':
                    next = new List<(int, int)>() { currentPosition.Plus(Points.Right) };
                    break;
                case '<':
                    next = new List<(int, int)>() { currentPosition.Plus(Points.Left) };
                    break;
                case '^':
                    next = new List<(int, int)>() { currentPosition.Plus(Points.Up) };
                    break;
                case 'v':
                    next = new List<(int, int)>() { currentPosition.Plus(Points.Down) };
                    break;
                case '.':
                    next = currentPosition.AdjacentPointsCardinal();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        else
        {
            switch (forest[currentPosition.x, currentPosition.y])
            {
                case '>':
                case '<':
                case '^':
                case 'v':
                case '.':
                    next = currentPosition.AdjacentPointsCardinal();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return next.Where(n => previousPosition != n && forest.ValueOrNull(n) != null && forest.ValueOrNull(n) != '#').ToList();
    }

    private static ((int, int) start, (int, int) end) FindStartEnd(char[,] forest)
    {
        var width = forest.GetLength(0);
        var height = forest.GetLength(1);
        var start = (-1, -1);
        var end = (-1, -1);
        for (int x = 0; x < width; x++)
        {
            if (forest[x, 0] == '.')
            {
                start = (x, 0);
            }

            if (forest[x, height - 1] == '.')
            {
                end = (x, height - 1);
            }
        }

        return (start, end);
    }

    private int LongestPath(Dictionary<(int, int), Node<(int,int)>> graph, (int, int) start, (int, int) end, HashSet<(int, int)>? traversed = null)
    {
        if (start == end)
        {
            return 0;
        }

        if (traversed == null)
        {
            traversed = new HashSet<(int, int)>();
        }

        var nextNeighbors = graph[start].Neighbors.Where(n => !traversed.Contains(n.node.Label)).ToList();
        if (!nextNeighbors.Any())
        {
            return int.MinValue;
        }

        return nextNeighbors.Max(n => n.distance + LongestPath(graph, n.node.Label, end, traversed.Append(n.node.Label).ToHashSet()));
    }

    public override object Solve1(string input)
    {
        var forest = input.To2dCharArray();
        var (start, end) = FindStartEnd(forest);
        var graph = BuildGraph(forest);

        return LongestPath(graph, start, end);
    }


    //Problem 2
    public override object ExpectedOutput2 => 154;

    public override object Solve2(string input)
    {
        var forest = input.To2dCharArray();
        var (start, end) = FindStartEnd(forest);
        var graph = BuildGraph(forest, false);

        return LongestPath(graph, start, end);
    }
}