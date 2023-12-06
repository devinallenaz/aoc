using AocHelpers;
using AocHelpers;
using AocHelpers.Solvers;
public class Day6 : Solver
{
  public override int Day => 6;
  
  //Problem 1
  public override object ExpectedOutput1 => 288d;

  private (double x1, double x2) Quadratic(double a, double b, double c)
  {
    double sqrtpart = (b * b) - (4 * a * c);
    var answer1 = ((-1)*b + Math.Sqrt(sqrtpart)) / (2 * a);
    var answer2 = ((-1)*b - Math.Sqrt(sqrtpart)) / (2 * a);
    return answer1 <= answer2 ? (answer1, answer2) : (answer2, answer1);
  }
  public override object Solve1(string input)
  {
    var lines = input.SplitLines().ToArray();
    var times = lines[0].SplitAndTrim(":").Last().SplitAndTrim().Select(s => double.Parse(s)).ToArray();
    var distances = lines[1].SplitAndTrim(":").Last().SplitAndTrim().Select(s => double.Parse(s)).ToArray();
    var timeDistances = times.Zip(distances);
    var waysToWin = timeDistances.Select((td) =>
    {
      var (lower, upper) = Quadratic(-1, td.Item1, -td.Item2);
      if (Math.Abs(lower % 1) <= (Double.Epsilon * 100))
      {
        lower += 1;
      }
      if (Math.Abs(upper % 1) <= (Double.Epsilon * 100))
      {
        upper -= 1;
      }
      return Math.Floor(upper-double.Epsilon) - Math.Ceiling(lower+double.Epsilon)+1;
    });
    return waysToWin.Aggregate((w1, w2) => w1 * w2);

  }

  //Problem 2
  public override object ExpectedOutput2 => 71503d;
  public override object Solve2(string input)
  {
    input = input.Replace(" ", "");
    var lines = input.SplitLines().ToArray();
    var times = lines[0].SplitAndTrim(":").Last().SplitAndTrim().Select(s => double.Parse(s)).ToArray();
    var distances = lines[1].SplitAndTrim(":").Last().SplitAndTrim().Select(s => double.Parse(s)).ToArray();

    var timeDistances = times.Zip(distances);
    var waysToWin = timeDistances.Select((td) =>
    {
      var (lower, upper) = Quadratic(-1, td.Item1, -td.Item2);
      if (Math.Abs(lower % 1) <= (Double.Epsilon * 100))
      {
        lower += 1;
      }
      if (Math.Abs(upper % 1) <= (Double.Epsilon * 100))
      {
        upper -= 1;
      }
      return Math.Floor(upper-double.Epsilon) - Math.Ceiling(lower+double.Epsilon)+1;
    });
    return waysToWin.Aggregate((w1, w2) => w1 * w2);
  }
}