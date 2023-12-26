using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;
public class Day22 : Solver
{
  public override int Day => 22;
  
  //Problem 1
  public override object ExpectedOutput1 => 5;

  public override object Solve1(string input)
  {
    var bricks = input.SplitLines().Select((s,i) => new SandBrick(s,i)).ToList();
    var stack = new BrickStack(bricks);
    return bricks.Count() - bricks.Where(b => b.BricksThatSupportThis.Count == 1).SelectMany(b => b.BricksThatSupportThis).Distinct().Count();

  }

  //Problem 2
  public override object ExpectedOutput2 => 7;
  public override object Solve2(string input)
  {
    var bricks = input.SplitLines().Select((s,i) => new SandBrick(s,i)).ToList();
    var stack = new BrickStack(bricks);
    return bricks.Sum(b => stack.BricksThatWouldFallIfBrickWereGone(b));
  }
}