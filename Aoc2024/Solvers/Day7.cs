using AocHelpers;
using AocHelpers.Solvers;
public class Day7 : Solver
{
  public override int Day => 7;
  
  //Problem 1
  public override object ExpectedOutput1 => 3749l;

  public override object Solve1(string input)
  {
    var equations = input.SplitLines().Select(s => new PossibleEquation(s));
    return equations.Where(e => e.IsPossible).Sum(e => e.TestValue);
  }

  //Problem 2
  public override object ExpectedOutput2 => 11387l;
  public override object Solve2(string input)
  {
    var equations = input.SplitLines().Select(s => new PossibleEquation(s));
    return equations.Where(e => e.IsPossibleWithConcatenation).Sum(e => e.TestValue);
  }
}