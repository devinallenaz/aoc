using AocHelpers;
using AocHelpers.Solvers;

public class Day23 : Solver
{
    public override int Day => 23;

    //Problem 1
    public override object ExpectedOutput1 => 7;

    private class Computer(string name)
    {
        public string Name { get; } = name;
        private HashSet<Computer> ConnectedComputers { get; } = [];

        public void ConnectComputer(Computer other)
        {
            ConnectedComputers.Add(other);
        }

        public List<HashSet<Computer>> Trios
        {
            get
            {
                if (this.ConnectedComputers.Count < 2)
                {
                    return [];
                }

                return this.ConnectedComputers.AllPairs().Where(pair => pair.Item1.IsConnectedTo(pair.Item2)).Select<(Computer, Computer), HashSet<Computer>>(pair => [pair.Item1, pair.Item2, this]).ToList();
            }
        }

        public bool IsConnectedTo(Computer other)
        {
            return ConnectedComputers.Contains(other);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    private static Dictionary<string, Computer> Setup(string input)
    {
        var computerDictionary = new Dictionary<string, Computer>();
        var lines = input.SplitLines();
        foreach (var line in lines)
        {
            var first = line[..2];
            var second = line[3..];
            if (!computerDictionary.ContainsKey(first))
            {
                computerDictionary[first] = new Computer(first);
            }

            if (!computerDictionary.ContainsKey(second))
            {
                computerDictionary[second] = new Computer(second);
            }

            computerDictionary[first].ConnectComputer(computerDictionary[second]);
            computerDictionary[second].ConnectComputer(computerDictionary[first]);
        }

        return computerDictionary;
    }


    public override object Solve1(string input)
    {
        var computerDictionary = Setup(input);

        var trios = computerDictionary.Where(c => c.Key.StartsWith('t')).SelectMany(c => c.Value.Trios);
        var keys = trios.Select(s => string.Join(',', s.Select(c => c.Name).Order()));
        return keys.Distinct().Count();
    }

    //Problem 2
    public override object ExpectedOutput2 => "co,de,ka,ta";

    public override object Solve2(string input)
    {
        var computerDictionary = Setup(input);
        var networks = computerDictionary.Values.Select(c => new HashSet<Computer> { c }).ToList();
        foreach (var computer in computerDictionary.Values)
        {
            var networksToAdd = networks.Where(n => n.All(o => computer.IsConnectedTo(o))).Select(network => (HashSet<Computer>) [..network, computer]).ToList();

            networks.AddRange(networksToAdd);
        }

        return string.Join(',', networks.MaxBy(n => n.Count).Select(c => c.Name).Order());
    }
}