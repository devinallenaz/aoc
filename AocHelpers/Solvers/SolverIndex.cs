using System.Reflection;

namespace AocHelpers.Solvers;

public static class SolverIndex
{
    private static Dictionary<int, Solver> Solvers = new Dictionary<int, Solver>();

    static SolverIndex()
    {
        var assembly = Assembly.GetEntryAssembly();
        var types = assembly!.GetTypes();
        var typesInNamespace = types.Where(t => t.IsClass && !t.IsAbstract);
        var solvers = typesInNamespace.Where(t => t.IsAssignableTo(typeof(Solver)));
        solvers.Select(Activator.CreateInstance)
            .Where(o => o != null)
            .OfType<Solver>()
            .ToList()
            .ForEach(s => { Solvers.Add(s.Day, s); });
    }

    public static Solver GetSolver(int day)
    {
        return Solvers[day];
    }

    public static List<Solver> AllSolvers
    {
        get { return Solvers.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList(); }
    }
}