using System.Diagnostics;
using AocHelpers.Input;
using AocHelpers.Solvers;

namespace AocHelpers;

public static class Runner
{
    public static int Year { get; set; }

    public static IEnumerable<TrySolveResult> TrySolveAllSolvers()
    {
        var results = new List<TrySolveResult>();
        foreach (var solver in SolverIndex.AllSolvers.OrderBy(s => s.Day))
        {
            results.Add(solver.TrySolve1(true));
            results.Add(solver.TrySolve1(false));
            results.Add(solver.TrySolve2(true));
            results.Add(solver.TrySolve2(false));
        }

        return results;
    }

    public static void OutputFormattedResults(IEnumerable<TrySolveResult> results)
    {
        if (results.Any())
        {
            var trySolveResults = results as TrySolveResult[] ?? results.ToArray();

            var longestResult = trySolveResults.Max(r => r.OutputLine1.Length);

            var tabs = (longestResult / 8) + 1;

            Console.WriteLine($"Day\tProblem\tTest\tResult{NTabs(tabs)}Time");
            Console.WriteLine($"---\t-------\t----\t------{NTabs(tabs)}---------");
            foreach (var result in trySolveResults.OrderBy(r => r.Day).ThenBy(r => r.Problem).ThenByDescending(r => r.Test))
            {
                Console.Write($"{result.Day}\t{result.Problem}\t{result.Test.ToString()}\t");
                if (result.Test)
                {
                    if (result.Success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }


                Console.Write(result.OutputLine1);

                Console.ResetColor();
                Console.WriteLine($"{NTabs(tabs - (result.OutputLine1.Length / 8))}{result.Ms}ms");
                if (result.OutputLinesRemainder != null)
                {
                    if (result.Test)
                    {
                        if (result.Success)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }

                    Console.WriteLine(result.OutputLinesRemainder);

                    Console.ResetColor();
                }
            }

            Console.WriteLine($"\nRunning all solvers took {trySolveResults.Sum(r => r.Ms)}ms");
        }
    }

    private static string NTabs(int n)
    {
        var tabArray = new char[n];
        Array.Fill(tabArray, '\t');
        return string.Join(null, tabArray);
    }
}