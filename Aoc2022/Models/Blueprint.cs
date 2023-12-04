namespace Aoc2022.Models;

public class Blueprint
{
    public Blueprint(int id, int orebotCost, int claybotCost, int obsidionbotOreCost, int obsidionbotClayCost, int geodebotOreCost, int geodebotObsidionCost)
    {
        Id = id;
        OrebotCost = orebotCost;
        ClaybotCost = claybotCost;
        ObsidionbotOreCost = obsidionbotOreCost;
        ObsidionbotClayCost = obsidionbotClayCost;
        GeodebotOreCost = geodebotOreCost;
        GeodebotObsidionCost = geodebotObsidionCost;
        MaxOreCost = Math.Max(claybotCost, Math.Max(obsidionbotOreCost, geodebotOreCost));
    }


    public int Id { get; }
    public int OrebotCost { get; }
    public int ClaybotCost { get; }
    public int ObsidionbotOreCost { get; }
    public int ObsidionbotClayCost { get; }
    public int GeodebotOreCost { get; }
    public int GeodebotObsidionCost { get; }
    private int MaxOreCost { get; }

    public int MaxGeodes(int time, OperationState operationState)
    {
        List<OperationState> currentStates = new List<OperationState>() { operationState };
        List<OperationState> allStates = new List<OperationState>();
        allStates.AddRange(currentStates);
        var currentTime = 0;
        while (currentTime < time)
        {
            currentStates = PossibleNextStates(currentStates, allStates);
            currentStates = PruneStates(currentStates);
            allStates.AddRange(currentStates);
            currentTime++;
        }

        return currentStates.Max(s => s.InventoryState.Geodes);
    }

    private static List<OperationState> PruneStates(List<OperationState> currentStates)
    {
        var bestGeodes = currentStates.Max(s => s.InventoryState.Geodes);
        return currentStates.Where(s => s.InventoryState.Geodes >= bestGeodes - 5).ToList();
    }

    public List<OperationState> PossibleNextStates(List<OperationState> currentStates, List<OperationState> allPreviousStates)
    {
        var nextStates = currentStates.SelectMany(s => GetPossibleBuilds(s).Select(b => NextState(s, b))).Distinct().ToArray();
        var all = allPreviousStates.Concat(nextStates).ToArray();
        var worseThan = nextStates.Where(s => all.Any(s.WorseThan)).ToList();
        return nextStates.Where(n => !worseThan.Contains(n)).ToList();
    }

    public OperationState NextState(OperationState operationState, Func<OperationState, OperationState> buildAction)
    {
        operationState = Harvest(operationState);
        return buildAction(operationState);
    }

    private static OperationState Harvest(OperationState operationState)
    {
        return new OperationState(
            new InventoryState(
                operationState.InventoryState.Ore + operationState.RobotState.OreRobots,
                operationState.InventoryState.Clay + operationState.RobotState.ClayRobots,
                operationState.InventoryState.Obsidion + operationState.RobotState.ObsidionRobots,
                operationState.InventoryState.Geodes + operationState.RobotState.GeodeRobots
            ),
            operationState.RobotState);
    }

    private IEnumerable<Func<OperationState, OperationState>> GetPossibleBuilds(OperationState operationState)
    {
        var inventoryState = operationState.InventoryState;
        var robotState = operationState.RobotState;

        if (CouldBuildGeodeBot(robotState, inventoryState))
        {
            yield return BuildGeodebot;
        }
        else
        {
            if (CouldBuildObsidionBot(robotState, inventoryState))
            {
                yield return BuildObsidionbot;
            }

            if (CouldBuildClaybot(robotState, inventoryState))
            {
                yield return BuildClaybot;
            }

            if (CouldBuildOrebot(robotState, inventoryState))
            {
                yield return BuildOrebot;
            }

            if (!CouldBuildOrebot(robotState, inventoryState) || !CouldBuildClaybot(robotState, inventoryState) || (!CouldBuildObsidionBot(robotState, inventoryState) && robotState.ClayRobots > 0))
            {
                yield return BuildNothing;
            }
        }
    }

    private bool CouldBuildGeodeBot(RobotState robotState, InventoryState inventoryState)
    {
        return inventoryState.Ore >= this.GeodebotOreCost && inventoryState.Obsidion >= this.GeodebotObsidionCost;
    }

    private bool CouldBuildOrebot(RobotState robotState, InventoryState inventoryState)
    {
        return robotState.OreRobots < this.MaxOreCost && inventoryState.Ore >= this.OrebotCost;
    }

    private bool CouldBuildClaybot(RobotState robotState, InventoryState inventoryState)
    {
        return robotState.ClayRobots < this.ObsidionbotClayCost && inventoryState.Ore >= this.ClaybotCost;
    }

    public bool CouldBuildObsidionBot(RobotState robotState, InventoryState inventoryState)
    {
        return robotState.ObsidionRobots < this.GeodebotObsidionCost && inventoryState.Ore >= this.ObsidionbotOreCost && inventoryState.Clay >= this.ObsidionbotClayCost;
    }

    public static OperationState BuildNothing(OperationState operationState)
    {
        return operationState;
    }

    public OperationState BuildOrebot(OperationState operationState)
    {
        return new OperationState(
            new InventoryState(
                operationState.InventoryState.Ore - this.OrebotCost,
                operationState.InventoryState.Clay,
                operationState.InventoryState.Obsidion,
                operationState.InventoryState.Geodes),
            new RobotState(
                operationState.RobotState.OreRobots + 1,
                operationState.RobotState.ClayRobots,
                operationState.RobotState.ObsidionRobots,
                operationState.RobotState.GeodeRobots
            ));
    }

    public OperationState BuildClaybot(OperationState operationState)
    {
        return new OperationState(
            new InventoryState(
                operationState.InventoryState.Ore - this.ClaybotCost,
                operationState.InventoryState.Clay,
                operationState.InventoryState.Obsidion,
                operationState.InventoryState.Geodes),
            new RobotState(
                operationState.RobotState.OreRobots,
                operationState.RobotState.ClayRobots + 1,
                operationState.RobotState.ObsidionRobots,
                operationState.RobotState.GeodeRobots
            ));
    }

    public OperationState BuildObsidionbot(OperationState operationState)
    {
        return new OperationState(
            new InventoryState(
                operationState.InventoryState.Ore - this.ObsidionbotOreCost,
                operationState.InventoryState.Clay - this.ObsidionbotClayCost,
                operationState.InventoryState.Obsidion,
                operationState.InventoryState.Geodes),
            new RobotState(
                operationState.RobotState.OreRobots,
                operationState.RobotState.ClayRobots,
                operationState.RobotState.ObsidionRobots + 1,
                operationState.RobotState.GeodeRobots
            ));
    }

    public OperationState BuildGeodebot(OperationState operationState)
    {
        return new OperationState(
            new InventoryState(
                operationState.InventoryState.Ore - this.GeodebotOreCost,
                operationState.InventoryState.Clay,
                operationState.InventoryState.Obsidion - this.GeodebotObsidionCost,
                operationState.InventoryState.Geodes),
            new RobotState(
                operationState.RobotState.OreRobots,
                operationState.RobotState.ClayRobots,
                operationState.RobotState.ObsidionRobots,
                operationState.RobotState.GeodeRobots + 1
            ));
    }
}

public struct OperationState
{
    public OperationState(InventoryState inventoryState, RobotState robotState)
    {
        InventoryState = inventoryState;
        RobotState = robotState;
    }

    public InventoryState InventoryState { get; }
    public RobotState RobotState { get; }

    public override bool Equals(object? obj)
    {
        if (obj is OperationState op)
        {
            return this.InventoryState.Equals(op.InventoryState) && this.RobotState.Equals(op.RobotState);
        }

        return base.Equals(obj);
    }

    public bool WorseThan(OperationState other)
    {
        return !this.Equals(other) && this.InventoryState.WorseThan(other.InventoryState) && this.RobotState.WorseThan(other.RobotState);
    }
}

public struct InventoryState
{
    public InventoryState(int ore, int clay, int obsidion, int geodes)
    {
        Ore = ore;
        Clay = clay;
        Obsidion = obsidion;
        Geodes = geodes;
    }

    public int Ore { get; }
    public int Clay { get; }
    public int Obsidion { get; }
    public int Geodes { get; }

    public override bool Equals(object? obj)
    {
        if (obj is InventoryState inv)
        {
            return this.Ore == inv.Ore && this.Clay == inv.Clay && this.Obsidion == inv.Obsidion && this.Geodes == inv.Geodes;
        }

        return base.Equals(obj);
    }

    public bool WorseThan(InventoryState other)
    {
        return this.Ore <= other.Ore && this.Clay <= other.Clay && this.Obsidion <= other.Obsidion && this.Geodes <= other.Geodes;
    }
}

public struct RobotState
{
    public RobotState(int oreRobots, int clayRobots, int obsidionRobots, int geodeRobots)
    {
        OreRobots = oreRobots;
        ClayRobots = clayRobots;
        ObsidionRobots = obsidionRobots;
        GeodeRobots = geodeRobots;
    }

    public int OreRobots { get; }
    public int ClayRobots { get; }
    public int ObsidionRobots { get; }
    public int GeodeRobots { get; }

    public override bool Equals(object? obj)
    {
        if (obj is RobotState rob)
        {
            return this.OreRobots == rob.OreRobots && this.ClayRobots == rob.ClayRobots && this.ObsidionRobots == rob.ObsidionRobots && this.GeodeRobots == rob.GeodeRobots;
        }

        return base.Equals(obj);
    }

    public bool WorseThan(RobotState other)
    {
        return this.OreRobots <= other.OreRobots && this.ClayRobots <= other.ClayRobots && this.ObsidionRobots <= other.ObsidionRobots && this.GeodeRobots <= other.GeodeRobots;
    }
}