using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;
public class Day20 : Solver
{
  public override int Day => 20;
  
  //Problem 1
  public override object ExpectedOutput1 => 11687500L;

  public override object Solve1(string input)
  {
      var system = new PulseModuleSystem(input);
      var (low, high) = system.PushButton(1000);
      return low * high;
  }

  //Problem 2
  public override object ExpectedOutput2 => base.ExpectedOutput2;
  public override object Solve2(string input)
  {
      var system = new PulseModuleSystem(input);
      return system.FindFirstOutput();
  }
}