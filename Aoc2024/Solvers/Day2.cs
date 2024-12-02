using AocHelpers;
using AocHelpers.Solvers;

public class Day2 : Solver
{
    public override int Day => 2;

    //Problem 1
    public override object ExpectedOutput1 => 2;


    private bool ReportIsSafe(int[] report)
    {
        var differences = report.Differences();
        return differences.All(n => n < 0 && Math.Abs(n).Between(1, 3)) || differences.All(n => n > 0 && Math.Abs(n).Between(1, 3));
    }

    private bool ReportIsSafeWithDampening(int[] report)
    {
        if (ReportIsSafe(report))
        {
            return true;
        }

        for (int i = 0; i < report.Length; i++)
        {
            var newReport = report.Where((_, index) => index != i).ToArray();
            if (ReportIsSafe(newReport))
            {
                return true;
            }
        }

        return false;
    }

    public override object Solve1(string input)
    {
        var reports = input.SplitLines();
        return reports.Select(r => r.Split().Select(int.Parse).ToArray()).Count(ReportIsSafe);
    }

    //Problem 2
    public override object ExpectedOutput2 => 4;

    public override object Solve2(string input)
    {
        var reports = input.SplitLines();
        return reports.Select(r => r.Split().Select(int.Parse).ToArray()).Count(ReportIsSafeWithDampening);
    }
}