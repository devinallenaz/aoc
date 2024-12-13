using System.Text.RegularExpressions;
using AocHelpers;
using AocHelpers.Solvers;
using Claw = (decimal ax, decimal ay, decimal bx, decimal by, decimal tx, decimal ty);

public partial class Day13 : Solver
{
    public override int Day => 13;

    //Problem 1
    public override object ExpectedOutput1 => 480l;

    private static IEnumerable<Claw> Setup(string input, bool plusALot = false)
    {
        return InputRegex().Matches(input).Select(m => (
                decimal.Parse(m.Groups[1].Value),
                decimal.Parse(m.Groups[2].Value),
                decimal.Parse(m.Groups[3].Value),
                decimal.Parse(m.Groups[4].Value),
                decimal.Parse(m.Groups[5].Value) + (plusALot ? 10000000000000l : 0),
                decimal.Parse(m.Groups[6].Value) + (plusALot ? 10000000000000l : 0)
            )
        );
    }

    private static long MinimumCostForPrize(Claw claw)
    {
        //ax*a + bx*b = tx
        //ay*a + by*b = ty
        //ay*a = ty - by*b
        //a = (ty -by*b)/ay
        //ax*((ty -by*b)/ay) + bx*b = tx
        //ax*(ty/ay - (by/ay)b)+bx*b = tx
        //(ax*ty/ay) - (ax*by/ay)b + bx*b = tx
        //(ax*ty/ay) + bx*b - (ax*by/ay)b = tx
        //bx*b - (ax*by/ay)b = tx - (ax*ty/ay)
        //b(bx-(ax*by/ay)) = tx - (ax*ty/ay)
        //b = (tx - (ax*ty/ay))/(bx-(ax*by/ay))
        var b = (claw.tx - (claw.ax * claw.ty / claw.ay)) / (claw.bx - (claw.ax * claw.by / claw.ay));
        var a = (claw.ty - (claw.by * b)) / claw.ay;
        if (a > 0 && b > 0 && a.RoundsToWholeNumber() && b.RoundsToWholeNumber())
        {
            var final = a * 3 + b;
            var roundUp = final % 1 > .5m;
            return (long)final + (roundUp ? 1 : 0);
        }
        else
        {
            return 0;
        }
    }

    public override object Solve1(string input)
    {
        var claws = Setup(input);


        return claws.Sum(MinimumCostForPrize);
    }


    //Problem 2
    public override object ExpectedOutput2 => 875318608908l;

    public override object Solve2(string input)
    {
        var claws = Setup(input, true);
        return claws.Sum(MinimumCostForPrize);
    }

    [GeneratedRegex(@"Button A: X\+(\d+), Y\+(\d+)
Button B: X\+(\d+), Y\+(\d+)
Prize: X=(\d+), Y=(\d+)", RegexOptions.Multiline)]
    private static partial Regex InputRegex();
}