namespace Aoc2022.Models;

public class ValveSystem
{
    public IEnumerable<ValveNode> AllNodes { get; }
    public ValveNode StartingNode { get; }

    public IEnumerable<ValveNode> RelevantNodes
    {
        get { return AllNodes.Where(n => n.ReleaseValue > 0); }
    }

    public ValveSystem(IEnumerable<ValveInit> valveInits)
    {
        var valves = new Dictionary<string, ValveNode>();
        foreach (var valveInit in valveInits)
        {
            valves.Add(valveInit.Name, new ValveNode(valveInit.Name, valveInit.ReleaseValue));
            if (valveInit.Name == "AA")
            {
                this.StartingNode = valves[valveInit.Name];
            }
        }

        if (this.StartingNode == null)
        {
            throw new ApplicationException("no starting node");
        }

        foreach (var valveInit in valveInits)
        {
            valves[valveInit.Name].SetDirectConnections(valveInit.DirectConnections.Select(n => valves[n]));
        }

        this.AllNodes = valves.Values;
        foreach (var valve in this.RelevantNodes)
        {
            valve.CalculateDistancesToRelevantNodes(this.RelevantNodes);
        }

        this.StartingNode.CalculateDistancesToRelevantNodes(this.RelevantNodes);
    }

    public ValveSystemState GetInitialState()
    {
        return new ValveSystemState(30, this.RelevantNodes.ToArray());
    }

    public struct ValveInit
    {
        public ValveInit(string name, int releaseValue, string[] directConnections)
        {
            Name = name;
            ReleaseValue = releaseValue;
            DirectConnections = directConnections;
        }

        public string Name { get; }
        public int ReleaseValue { get; }
        public string[] DirectConnections { get; }
    }

    public static int Find(string valveName, IEnumerable<ValveNode> fromNodes, int distance = 1)
    {
        if (fromNodes.Any(n => n.Name == valveName))
        {
            return distance;
        }

        var newNodes = fromNodes.SelectMany(n => n.DirectConnections).Distinct().Where(n => !fromNodes.Contains(n)).ToList();
        if (newNodes.Any(n => n.Name == valveName))
        {
            return distance;
        }

        return Find(valveName, newNodes.Concat(fromNodes), distance + 1);
    }
}

public class ValveNode
{
    public string Name { get; }
    public int ReleaseValue { get; }
    private ValveNode[]? _directConnections;
    public IEnumerable<ValveNode> DirectConnections => _directConnections ?? throw new ApplicationException("DirectConnections not initialized");
    private Dictionary<string, int>? _nodeDistances;
    public Dictionary<string, int> NodeDistances => _nodeDistances ?? throw new ApplicationException("Node distances not initialized");

    public ValveNode(string name, int releaseValue)
    {
        Name = name;
        ReleaseValue = releaseValue;
    }

    public void SetDirectConnections(IEnumerable<ValveNode> directConnections)
    {
        _directConnections = directConnections.ToArray();
    }

    public void CalculateDistancesToRelevantNodes(IEnumerable<ValveNode> relevantNodes)
    {
        _nodeDistances = relevantNodes.ToDictionary(n => n.Name, n => ValveSystem.Find(n.Name, new[] { this }));
    }

    private int DistanceTo(ValveNode other)
    {
        return NodeDistances[other.Name];
    }

    private int ReleaseAmount(int timeLeft)
    {
        return ReleaseValue * timeLeft;
    }

    public int MaxRelease(ValveSystemState state) //Maximum possible release by opening this valve.
    {
        if (state.TimeLeft <= 1)//We've run out of time, we can't release any more by opening this valve.
        {
            return 0;
        }

        var timeLeft = this.ReleaseValue > 0 
            ? state.TimeLeft - 1 //spend 1 minute opening this value
            : state.TimeLeft; //we only get here on the starting node, which we don't want to open
        var openValves = state.ClosedValves.Where(v => v != this).ToArray();//Remove this valve the set of remaining valves.
        if (!openValves.Any())//this is the last valve
        {
            return ReleaseAmount(timeLeft); 
        }

        return ReleaseAmount(timeLeft) //amount released by opening this valve now
               //For every remaining valve find the max release in the remaining time:
               + openValves.Max(v => 
                   v.MaxRelease(new ValveSystemState(timeLeft - DistanceTo(v) /*deduct travel time to next node*/, openValves)) 
                   );
    }
}

public struct ValveSystemState
{
    public int TimeLeft { get; }
    public ValveNode[] ClosedValves { get; }

    public ValveSystemState(int timeLeft, ValveNode[] closedValves)
    {
        TimeLeft = timeLeft;
        ClosedValves = closedValves;
    }
}