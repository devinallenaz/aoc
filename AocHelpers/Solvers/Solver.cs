using System.Diagnostics;
using AocHelpers.Input;

namespace AocHelpers.Solvers;

public abstract class Solver
{
    public abstract int Day { get; }
    public virtual object ExpectedOutput1 => "Not set";
    public virtual object ExpectedOutput2 => "Not set";

    public virtual object Solve1(string input) => "Not Implemented";
    public virtual object Solve2(string input) => "Not Implemented";

    public TrySolveResult TrySolve1(bool test)
    {
        return TrySolve(test, Solve1, ExpectedOutput1);
    }

    public TrySolveResult TrySolve2(bool test)
    {
        return TrySolve(test, Solve2, ExpectedOutput2, true);
    }

    private TrySolveResult TrySolve(bool test, Func<string, object> solve, object expectedOutput, bool problem2 = false)
    {
        var input = InputHelper.GetInputText(this.Day, test, problem2);
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var output = solve(input);
        stopwatch.Stop();
        var outputStrings = (output.ToString()?.Replace("\n", "\n\t\t") ?? "").Trim().Split('\n');
        var line1 = outputStrings.First();
        var linesRemaining = outputStrings.Skip(1).Any() ? string.Join('\n', outputStrings.Skip(1)) : null;
        return new TrySolveResult(this.Day, problem2 ? 2 : 1, line1, linesRemaining, stopwatch.ElapsedMilliseconds, test, test && output.Equals(expectedOutput));
    }
}